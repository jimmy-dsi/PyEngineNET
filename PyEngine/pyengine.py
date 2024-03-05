import os
import sys
import platform
import msgpack
import struct
if platform.system() == 'Windows':
	import win32file

___named_pipe = None

class NETException(Exception):
	"Raised when an Exception occurs on the .NET side"
	def __init__(self, typename, message):
		super().__init__(typename + ' - ' + message)
		self.typename = typename

		
class UnixNamedPipe:
	# TODO: Provide implementation that uses Unix Domain Sockets
	def __init__(self, pipe_name):
		self.pipe_name = pipe_name


class WindowsNamedPipe:
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

	def write(self, message):
		length_prefix = struct.pack('<I', len(message))
		win32file.WriteFile(self.pipe, length_prefix)
		win32file.WriteFile(self.pipe, message)

	def read(self):
		_, length_prefix = win32file.ReadFile(self.pipe, 4)
		length = struct.unpack('<I', length_prefix)[0]
		_, data = win32file.ReadFile(self.pipe, length)
		return data


def ___exc_handler(ex):
	print(ex)


def ___main(pipe_name):
	print('Pipe name:', pipe_name)

	global ___named_pipe
	if platform.system() == 'Windows':
		___named_pipe = WindowsNamedPipe(pipe_name)
	else:
		___named_pipe = UnixNamedPipe(pipe_name)

	result = ___send_and_recv({'cm': 'ready'})
	while True:
		if result['cm'] == 'exec':
			result = ___process_exec(result)


def ___call_cs_method(func_name, *args):
	while True:
		result = ___send_and_recv({'cm': 'call', 'func': func_name})
		if result['cm'] == 'exec':
			result = ___process_exec(result, *args)
		if result['cm'] == 'retn':
			return eval(result['dt'])
		elif result['cm'] == 'err':
			err_info = result['dt']
			raise NETException(err_info[0], err_info[1])


def ___process_exec(result, *args):
	try:
		exec(result['dt'])
	except NETException as e:
		# Since this is a .NET-side error, we don't report an error back to .NET, just handle it gracefully and move on.
		___exc_handler(e)
		result = ___send_and_recv({'cm': 'done'})
	except Exception as e:
		# All Python-side errors should be reported to the .NET side, however.
		___exc_handler(e)
		result = ___send_and_recv({'cm': 'err'})
	else:
		# General case: No errors, signal a 'done'.
		result = ___send_and_recv({'cm': 'done'})
	return result


def ___send_and_recv(data_dict):
	global ___named_pipe
	___named_pipe.write(msgpack.packb(data_dict))
	return msgpack.unpackb(___named_pipe.read())


if __name__ == '__main__':
	if len(sys.argv) < 2:
		print('Usage: pyengine.exe <shared_pipe_name>')
		sys.exit(1)
	___main(sys.argv[1])
