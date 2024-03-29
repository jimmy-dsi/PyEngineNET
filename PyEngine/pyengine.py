import os, sys, platform
import struct, dataclasses
import socket
import traceback
import signal
import msgpack
if platform.system() == 'Windows':
	import win32file, pywintypes

named_pipe = None

# Classes
class Frame:
	def __init__(self, tup):
		self.filename = tup[0]
		self.lineno   = tup[1]
		self.name     = tup[2]
		self.line     = tup[3]
		self.params   = tup[4]


class NETException(Exception):
	"Raised when an Exception occurs on the .NET side"
	def __init__(self, typename, message, net_traceback):
		super().__init__(typename + ' - ' + message)
		self.message = message
		self.typename = typename
		self.net_traceback = [Frame(x) for x in net_traceback]


class PipeDataException(Exception):
	"Raised when there is an error either packing, unpacking, sending, or receiving data"
	def __init__(self, message):
		self.message = message
		super().__init__(message)


class NamedPipe:
	def __init__(self, pipe_name):
		pass

	def write(self, message):
		if len(message) >= 0x8000_0000:
			raise PipeDataException('Data too large for pipe')
		
		try:
			length_prefix = struct.pack('<I', len(message))
		except Exception as e:
			raise PipeDataException('Error packing data for pipe')

		try:
			self._write(length_prefix, message)
		except Exception as e:
			raise PipeDataException('Error writing data to pipe')

	def read(self):
		try:
			length_prefix = self._readlen()
		except Exception as e:
			raise PipeDataException('Error reading data from pipe')
			
		try:
			length = struct.unpack('<I', length_prefix)[0]
		except Exception as e:
			raise PipeDataException('Error unpacking data from pipe')

		if length < 0:
			raise PipeDataException('Invalid data size from pipe')
			
		try:
			data = self._read(length)
		except Exception as e:
			raise PipeDataException('Error reading data from pipe')

		return data

		
class UnixNamedPipe(NamedPipe):
	def __init__(self, pipe_name):
		self.pipe_name = pipe_name
		self.socket_path = f'/tmp/CoreFxPipe_{pipe_name}' # .NET seems to prefix Unix pipe files with 'CoreFxPipe_'. Let's just roll with it I guess.
		self.client = socket.socket(socket.AF_UNIX, socket.SOCK_STREAM)
		self.client.connect(self.socket_path)

	def _write(self, length_prefix, message):
		self.client.sendall(length_prefix)
		self.client.sendall(message)

	def _readlen(self):
		try:
			length = self.client.recv(4)
			if len(length) < 4: # Byte length being less than what we expect indicates that the connection has been terminated.
				self._handle_broken_pipe()
		except ConnectionResetError:
			self._handle_broken_pipe()

		return length

	def _read(self, length):
		try:
			data = self.client.recv(length)
			if len(data) < length: # Data length being less than what we expect indicates that the connection has been terminated.
				self._handle_broken_pipe()
		except ConnectionResetError:
			self._handle_broken_pipe()

		return data

	def _handle_broken_pipe(self):
		try:
			self.client.close()
		except Exception as ie:
			print(ie.message, file=sys.stderr)
			sys.exit(1)
		else:
			sys.exit(0)


class WindowsNamedPipe(NamedPipe):
	def __init__(self, pipe_name):
		self.pipe_name = pipe_name
		full_pipe_name = rf'\\.\pipe\{pipe_name}'
		# Open existing named pipe_name
		self.pipe = \
			win32file.CreateFile(full_pipe_name,
			                     win32file.GENERIC_READ | win32file.GENERIC_WRITE,
			                     0, None,
			                     win32file.OPEN_EXISTING,
			                     0, None)

	def _write(self, length_prefix, message):
		try:
			win32file.WriteFile(self.pipe, length_prefix)
			win32file.WriteFile(self.pipe, message)
		except pywintypes.error as e:
			self._handle_winerror(e)

	def _readlen(self):
		try:
			_, length_prefix = win32file.ReadFile(self.pipe, 4)
			return length_prefix
		except pywintypes.error as e:
			self._handle_winerror(e)

	def _read(self, length):
		try:
			_, data = win32file.ReadFile(self.pipe, length)
			return data
		except pywintypes.error as e:
			self._handle_winerror(e)

	def _handle_winerror(self, e):
		if e.winerror == 109: # If pipe was closed by the other process, exit gracefully
			try:
				win32file.CloseHandle(self.pipe)
			except Exception as ie:
				print(ie.message, file=sys.stderr)
				sys.exit(1)
			else:
				sys.exit(0)
		else:
			raise


___next_id = 0

# For some reason, any calls or accesses to a global variable with leading underscores from within the ___NETGenerator class REFUSES to work properly,
# even when `global <___var_name>` is explicitly added in the method body. So, we're going to do things in this stupid way.
def ng___init(self, func_name, *args):
	global ___next_id
	self.id = ___next_id
	___next_id += 1
	___call_cs_method(func_name, self.id, *args)


def ng___next(self):
	result = send_and_recv({'cm': 'step', 'id': self.id})

	global _globals

	while True:
		if result['cm'] == 'exec':
			try:
				exec(result['dt'], _globals)
			except Exception as e:
				update_globals()
				result = catch_exec_eval(e)
			else:
				update_globals()
				result = send_and_recv({'cm': 'done'})
		elif result['cm'] == 'eval':
			try:
				value = eval(result['dt'], _globals)
				update_globals()
				result = send_and_recv({'cm': 'res', 'dt': ser(value)})
			except Exception as e:
				update_globals()
				result = catch_exec_eval(e)
		elif result['cm'] == 'yld':
			res = eval(result['dt'], _globals)
			update_globals()
			return res
		elif result['cm'] == 'stop':
			raise StopIteration()
		elif result['cm'] == 'err':
			err_info = eval(result['dt'])
			raise NETException(err_info[0], err_info[1], err_info[2])
	

class ___NETGenerator:
	def __init__(self, func_name, *args):
		ng___init(self, func_name, *args)

	def __iter__(self):
		return self

	def __next__(self):
		return ng___next(self)


# Default exception handlers. A .NET engine can override these via `engine.Exec("def ___exc_handler(ex): ...")`
def ___exc_handler(ex):
	print(ex, file=sys.stderr)

def ___py_exc_handler(ex):
	___exc_handler(ex)

def ___net_exc_handler(ex):
	___exc_handler(ex)


# Used to convert iterables to generators for use on .NET side
def ___make_gen(it):
	for item in it:
		yield item


# Serializer
primitives = (bool, int, float, str, bytes, bytearray, tuple, type(None))
def ser(value):
	if isinstance(value, primitives):
		return value
	elif isinstance(value, list):
		return [ser(x) for x in value]
	elif isinstance(value, dict):
		return {ser(k): ser(v) for k, v in value.items()}
	elif isinstance(value, set):
		return {'___type': 'set', '___set': ser(list(value))}
	elif dataclasses.is_dataclass(value):
		dt = type(value)
		return {
			'___type': dt.__name__ if dt.__module__ in {'__main__', 'builtins'} else f'{dt.__module__}.{dt.__name__}',
			'___data': ser(
				[
					[x.name, ser(getattr(value, x.name))]
					for x in dataclasses.fields(dt)
				]
			)
		}
	else:
		return value # TODO: Raise error about un-serializable class


def ___call_cs_method(func_name, gen_id, *___args):
	result = send_and_recv({'cm': 'call', 'func': func_name, 'id': gen_id})

	global _globals
	_locals = {'___args': ___args}

	while True:
		if result['cm'] == 'exec':
			try:
				exec(result['dt'], _globals, _locals)
			except Exception as e:
				update_globals()
				result = catch_exec_eval(e)
			else:
				update_globals()
				result = send_and_recv({'cm': 'done'})
		elif result['cm'] == 'eval':
			try:
				value = eval(result['dt'], _globals, _locals)
				update_globals()
				result = send_and_recv({'cm': 'res', 'dt': ser(value)})
			except Exception as e:
				update_globals()
				result = catch_exec_eval(e)
		elif result['cm'] == 'retn':
			res = eval(result['dt'], _globals, _locals)
			update_globals()
			return res
		elif result['cm'] == 'err':
			err_info = eval(result['dt'])
			raise NETException(err_info[0], err_info[1], err_info[2])


def update_globals():
	global _globals, ___exc_handler, ___py_exc_handler, ___net_exc_handler, ___call_cs_method, ___pye_var___None
	# Back-update handler globals in driver
	___exc_handler     = _globals['___exc_handler']
	___py_exc_handler  = _globals['___py_exc_handler']
	___net_exc_handler = _globals['___net_exc_handler']
	___next_id         = _globals['___next_id']
	# Reset visible globals to .NET process
	_globals['___call_cs_method'] = ___call_cs_method
	_globals['___pye_var___None'] = ___pye_var___None
	_globals['___make_gen']       = ___make_gen
	_globals['___NETGenerator']   = ___NETGenerator
	_globals['ng___init']         = ng___init
	_globals['ng___next']         = ng___next


def exc_traceback(e):
	tb = []
	for x in traceback.extract_tb(e.__traceback__):
		tb.append(x)
	if isinstance(e, NETException):
		for x in e.net_traceback:
			tb.append(x)
	return tb


def exc_dict(e):
	dt = type(e)
	true_dtname = dt.__name__ if dt.__module__ in {'__main__', 'builtins'} else f'{dt.__module__}.{dt.__name__}'
	msg = str(e)
	if true_dtname == 'NETException':
		true_dtname = e.typename
		msg = e.message

	if isinstance(e, IOError):
		dtname = 'IOError'
	elif isinstance(e, FileNotFoundError):
		dtname = 'FileNotFoundError'
	elif isinstance(e, FileExistsError):
		dtname = 'FileExistsError'
	elif isinstance(e, PermissionError):
		dtname = 'PermissionError'
	elif isinstance(e, ConnectionError):
		dtname = 'ConnectionError'
	elif isinstance(e, TimeoutError):
		dtname = 'TimeoutError'
	elif isinstance(e, IsADirectoryError):
		dtname = 'IsADirectoryError'
	elif isinstance(e, OSError):
		dtname = 'OSError'
	elif isinstance(e, ValueError):
		dtname = 'ValueError'
	elif isinstance(e, TypeError):
		dtname = 'TypeError'
	elif isinstance(e, AttributeError):
		dtname = 'AttributeError'
	elif isinstance(e, NameError):
		dtname = 'NameError'
	elif isinstance(e, ModuleNotFoundError):
		dtname = 'ModuleNotFoundError'
	elif isinstance(e, ImportError):
		dtname = 'ImportError'
	elif isinstance(e, IndexError):
		dtname = 'IndexError'
	elif isinstance(e, KeyError):
		dtname = 'KeyError'
	elif isinstance(e, LookupError):
		dtname = 'LookupError'
	elif isinstance(e, StopIteration):
		dtname = 'StopIteration'
	elif isinstance(e, StopAsyncIteration):
		dtname = 'StopAsyncIteration'
	elif isinstance(e, ZeroDivisionError):
		dtname = 'ZeroDivisionError'
	elif isinstance(e, OverflowError):
		dtname = 'OverflowError'
	elif isinstance(e, FloatingPointError):
		dtname = 'FloatingPointError'
	elif isinstance(e, AssertionError):
		dtname = 'AssertionError'
	elif isinstance(e, RuntimeError):
		dtname = 'RuntimeError'
	elif isinstance(e, NotImplementedError):
		dtname = 'NotImplementedError'
	elif isinstance(e, MemoryError):
		dtname = 'MemoryError'
	elif isinstance(e, BufferError):
		dtname = 'BufferError'
	elif isinstance(e, ReferenceError):
		dtname = 'ReferenceError'
	elif isinstance(e, SystemError):
		dtname = 'SystemError'
	elif isinstance(e, SyntaxError):
		dtname = 'SyntaxError'
	elif isinstance(e, EOFError):
		dtname = 'EOFError'
	elif isinstance(e, NETException):
		dtname = 'NETException'
	else:
		dtname = 'Exception'

	return {
		'cm': 'err',
		'dt': [
			dtname,
			true_dtname,
			msg,
			[
				(x.filename, x.lineno, x.name, x.line, x.params if isinstance(x, Frame) else '')
				for x in exc_traceback(e)
			]
		]
	}


def catch_exec_eval(e):
	if isinstance(e, NETException):
		___net_exc_handler(e)
	else:
		___py_exc_handler(e)
	return send_and_recv(exc_dict(e))


def send_and_recv(data_dict):
	global named_pipe
	try:
		named_pipe.write(msgpack.packb(data_dict))
	except PipeDataException as e:
		named_pipe.write(msgpack.packb(exc_dict(e)))
	return msgpack.unpackb(named_pipe.read())


if __name__ == '__main__':
	signal.signal(signal.SIGINT, signal.SIG_IGN) # Ignore keyboard interrupts

	if len(sys.argv) < 2:
		print('Usage: pyengine.exe <shared_pipe_name>')
		sys.exit(1)

	# Main body
	pipe_name = sys.argv[1]

	if platform.system() == 'Windows':
		named_pipe = WindowsNamedPipe(pipe_name)
	else:
		named_pipe = UnixNamedPipe(pipe_name)

	___pye_var___None = None

	# Security: Only allow some globals to be available for exec/eval
	_globals = {
		'___call_cs_method':  ___call_cs_method,
		'___pye_var___None':  ___pye_var___None,
		'___exc_handler':     ___exc_handler,
		'___py_exc_handler':  ___py_exc_handler,
		'___net_exc_handler': ___net_exc_handler,
		'___make_gen':        ___make_gen,
		'___NETGenerator':    ___NETGenerator,
		'___next_id':         ___next_id,
		'ng___init':          ng___init,
		'ng___next':          ng___next
	}

	result = send_and_recv({'cm': 'ready', 'dt': int(os.getpid())})
	while True:
		if result['cm'] == 'exec':
			try:
				exec(result['dt'], _globals)
			except Exception as e:
				update_globals()
				result = catch_exec_eval(e)
			else:
				update_globals()
				result = send_and_recv({'cm': 'done'})
		elif result['cm'] == 'eval':
			try:
				value = eval(result['dt'], _globals)
				update_globals()
				result = send_and_recv({'cm': 'res', 'dt': ser(value)})
			except Exception as e:
				update_globals()
				result = catch_exec_eval(e)
