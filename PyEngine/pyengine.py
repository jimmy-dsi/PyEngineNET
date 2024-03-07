import os
import sys
import platform
import msgpack
import struct
import socket
if platform.system() == 'Windows':
	import win32file
	import pywintypes

___named_pipe = None

# Classes
class NETException(Exception):
	"Raised when an Exception occurs on the .NET side"
	def __init__(self, typename, message):
		super().__init__(typename + ' - ' + message)
		self.message = message
		self.typename = typename


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


# Default exception handlers. A .NET engine can override these via `engine.Exec("def ___exc_handler(ex): ...")`
def ___exc_handler(ex):
	print(ex, file=sys.stderr)

def ___py_exc_handler(ex):
	___exc_handler(ex)

def ___net_exc_handler(ex):
	___exc_handler(ex)


# Main function
def ___main(pipe_name):
	print('Pipe name:', pipe_name)

	global ___named_pipe
	if platform.system() == 'Windows':
		___named_pipe = WindowsNamedPipe(pipe_name)
	else:
		___named_pipe = UnixNamedPipe(pipe_name)

	result = ___send_and_recv({'cm': 'ready', 'dt': int(os.getpid())})
	while True:
		if result['cm'] == 'exec':
			result = ___process_exec(result)
		elif result['cm'] == 'eval':
			result = ___process_eval(result)


def ___call_cs_method(func_name, *args):
	result = ___send_and_recv({'cm': 'call', 'func': func_name})

	while True:
		if result['cm'] == 'exec':
			result = ___process_exec(result, *args)
		elif result['cm'] == 'eval':
			result = ___process_eval(result)
		elif result['cm'] == 'retn':
			return eval(result['dt'])
		elif result['cm'] == 'err':
			err_info = result['dt']
			raise NETException(err_info[0], err_info[1])


def ___process_exec(result, *args):
	try:
		print(result['dt'])
		exec(result['dt'])
		#print(___pye_var___3C6EF35F('Hello'))
	except NETException as e:
		# Propogate original .NET-side error.
		___net_exc_handler(e)
		result = ___send_and_recv({'cm': 'err', 'dt': [str(type(e)), e.message]})
	except Exception as e:
		# All Python-side errors should be reported to the .NET side, however.
		___py_exc_handler(e)
		result = ___send_and_recv({'cm': 'err', 'dt': [str(type(e)), e.message]})
	else:
		# General case: No errors, signal a 'done'.
		result = ___send_and_recv({'cm': 'done'})
	return result


def ___process_eval(result, *args):
	try:
		print(result['dt'])
		value = eval(result['dt'])
	except NETException as e:
		# Propogate original .NET-side error.
		___net_exc_handler(e)
		result = ___send_and_recv({'cm': 'err', 'dt': [str(type(e)), e.message]})
	except Exception as e:
		# All Python-side errors should be reported to the .NET side, however.
		___py_exc_handler(e)
		result = ___send_and_recv({'cm': 'err', 'dt': [str(type(e)), e.message]})
	else:
		# General case: No errors, signal a 'res' indicating eval result is done.
		# TODO: Support serializable classes
		result = ___send_and_recv({'cm': 'res', 'dt': value})
	return result


def ___send_and_recv(data_dict):
	global ___named_pipe
	try:
		___named_pipe.write(msgpack.packb(data_dict))
	except PipeDataException as e:
		___named_pipe.write(msgpack.packb({'cm': 'err', 'dt': [str(type(e)), e.message]}))
	return msgpack.unpackb(___named_pipe.read())


if __name__ == '__main__':
	if len(sys.argv) < 2:
		print('Usage: pyengine.exe <shared_pipe_name>')
		sys.exit(1)
	___main(sys.argv[1])
