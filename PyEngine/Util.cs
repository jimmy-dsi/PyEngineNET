namespace PyEngine;

using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

internal static class Util {
	internal static Regex IdentRegex = new Regex(@"^[A-Za-z_][A-Za-z0-9_]*$", RegexOptions.Compiled);

	internal static string AsHex(this int n) {
		return n.ToString("X8").ToUpper();
	}

	internal static OSPlatform GetOS() {
		if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
			return OSPlatform.Windows;
		} else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) {
			return OSPlatform.Linux;
		} else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) {
			return OSPlatform.OSX;
		} else if (RuntimeInformation.IsOSPlatform(OSPlatform.FreeBSD)) {
			return OSPlatform.FreeBSD;
		} else {
			throw new NotSupportedException("App is running on an unknown/unsupported Platform.");
		}
	}

	internal static int SwapBits(this int n, int bitIndex1, int bitIndex2) {
		var b1 = (n & (1 << bitIndex1)) != 0 ? 1 : 0;
		var b2 = (n & (1 << bitIndex2)) != 0 ? 1 : 0;

		n = n & ~(1 << bitIndex1) & ~(1 << bitIndex2);
		n = n | (b2 << bitIndex1) | (b1 << bitIndex2);

		return n;
	}

	internal static string ShuffleHash(this int n) {
		uint a = 1664525;    // Multiplier
		uint c = 1013904223; // Increment

		// The LCG formula
		int hash = (int) ((uint) (a * n) + c);
		return hash.AsHex();
	}

	internal static int? ForceInt(this object? obj) {
		if (obj == null) {
			return null;
		} else {
			var otype = obj.GetType();
			if (otype == typeof(int)) {
				return (int?) obj;
			} else if (otype == typeof(uint)) {
				return (int?) (uint?) obj;
			} else if (otype == typeof(long)) {
				return (int?) (long?) obj;
			} else if (otype == typeof(ulong)) {
				return (int?) (ulong?) obj;
			} else if (otype == typeof(byte)) {
				return (int?) (byte?) obj;
			} else if (otype == typeof(sbyte)) {
				return (int?) (sbyte?) obj;
			} else if (otype == typeof(ushort)) {
				return (int?) (ushort?) obj;
			} else if (otype == typeof(short)) {
				return (int?) (short?) obj;
			} else {
				return (int?) obj;
			}
		}
	}

	internal static byte AsByte(this object obj) {
		var otype = obj.GetType();
		if (otype == typeof(byte)) {
			return (byte) obj;
		} else if (otype == typeof(sbyte)) {
			return (byte) (sbyte) obj;
		} else if (otype == typeof(ushort)) {
			return (byte) (ushort) obj;
		} else if (otype == typeof(short)) {
			return (byte) (short) obj;
		} else if (otype == typeof(uint)) {
			return (byte) (uint) obj;
		} else if (otype == typeof(int)) {
			return (byte) (int) obj;
		} else if (otype == typeof(ulong)) {
			return (byte) (ulong) obj;
		} else if (otype == typeof(long)) {
			return (byte) (long) obj;
		}

		throw new InvalidOperationException("Object is not an integer type.");
	}

	internal static sbyte AsSByte(this object obj) {
		var otype = obj.GetType();
		if (otype == typeof(byte)) {
			return (sbyte) (byte) obj;
		} else if (otype == typeof(sbyte)) {
			return (sbyte) obj;
		} else if (otype == typeof(ushort)) {
			return (sbyte) (ushort) obj;
		} else if (otype == typeof(short)) {
			return (sbyte) (short) obj;
		} else if (otype == typeof(uint)) {
			return (sbyte) (uint) obj;
		} else if (otype == typeof(int)) {
			return (sbyte) (int) obj;
		} else if (otype == typeof(ulong)) {
			return (sbyte) (ulong) obj;
		} else if (otype == typeof(long)) {
			return (sbyte) (long) obj;
		}

		throw new InvalidOperationException("Object is not an integer type.");
	}

	internal static ushort AsUShort(this object obj) {
		var otype = obj.GetType();
		if (otype == typeof(byte)) {
			return (ushort) (byte) obj;
		} else if (otype == typeof(sbyte)) {
			return (ushort) (sbyte) obj;
		} else if (otype == typeof(ushort)) {
			return (ushort) obj;
		} else if (otype == typeof(short)) {
			return (ushort) (short) obj;
		} else if (otype == typeof(uint)) {
			return (ushort) (uint) obj;
		} else if (otype == typeof(int)) {
			return (ushort) (int) obj;
		} else if (otype == typeof(ulong)) {
			return (ushort) (ulong) obj;
		} else if (otype == typeof(long)) {
			return (ushort) (long) obj;
		}

		throw new InvalidOperationException("Object is not an integer type.");
	}

	internal static short AsShort(this object obj) {
		var otype = obj.GetType();
		if (otype == typeof(byte)) {
			return (short) (byte) obj;
		} else if (otype == typeof(sbyte)) {
			return (short) (sbyte) obj;
		} else if (otype == typeof(ushort)) {
			return (short) (ushort) obj;
		} else if (otype == typeof(short)) {
			return (short) obj;
		} else if (otype == typeof(uint)) {
			return (short) (uint) obj;
		} else if (otype == typeof(int)) {
			return (short) (int) obj;
		} else if (otype == typeof(ulong)) {
			return (short) (ulong) obj;
		} else if (otype == typeof(long)) {
			return (short) (long) obj;
		}

		throw new InvalidOperationException("Object is not an integer type.");
	}

	internal static uint AsUInt(this object obj) {
		var otype = obj.GetType();
		if (otype == typeof(byte)) {
			return (uint) (byte) obj;
		} else if (otype == typeof(sbyte)) {
			return (uint) (sbyte) obj;
		} else if (otype == typeof(ushort)) {
			return (uint) (ushort) obj;
		} else if (otype == typeof(short)) {
			return (uint) (short) obj;
		} else if (otype == typeof(uint)) {
			return (uint) obj;
		} else if (otype == typeof(int)) {
			return (uint) (int) obj;
		} else if (otype == typeof(ulong)) {
			return (uint) (ulong) obj;
		} else if (otype == typeof(long)) {
			return (uint) (long) obj;
		}

		throw new InvalidOperationException("Object is not an integer type.");
	}

	internal static int AsInt(this object obj) {
		var otype = obj.GetType();
		if (otype == typeof(byte)) {
			return (int) (byte) obj;
		} else if (otype == typeof(sbyte)) {
			return (int) (sbyte) obj;
		} else if (otype == typeof(ushort)) {
			return (int) (ushort) obj;
		} else if (otype == typeof(short)) {
			return (int) (short) obj;
		} else if (otype == typeof(uint)) {
			return (int) (uint) obj;
		} else if (otype == typeof(int)) {
			return (int) obj;
		} else if (otype == typeof(ulong)) {
			return (int) (ulong) obj;
		} else if (otype == typeof(long)) {
			return (int) (long) obj;
		}

		throw new InvalidOperationException("Object is not an integer type.");
	}

	internal static ulong AsULong(this object obj) {
		var otype = obj.GetType();
		if (otype == typeof(byte)) {
			return (ulong) (byte) obj;
		} else if (otype == typeof(sbyte)) {
			return (ulong) (sbyte) obj;
		} else if (otype == typeof(ushort)) {
			return (ulong) (ushort) obj;
		} else if (otype == typeof(short)) {
			return (ulong) (short) obj;
		} else if (otype == typeof(uint)) {
			return (ulong) (uint) obj;
		} else if (otype == typeof(int)) {
			return (ulong) (int) obj;
		} else if (otype == typeof(ulong)) {
			return (ulong) obj;
		} else if (otype == typeof(long)) {
			return (ulong) (long) obj;
		}

		throw new InvalidOperationException("Object is not an integer type.");
	}

	internal static long AsLong(this object obj) {
		var otype = obj.GetType();
		if (otype == typeof(byte)) {
			return (long) (byte) obj;
		} else if (otype == typeof(sbyte)) {
			return (long) (sbyte) obj;
		} else if (otype == typeof(ushort)) {
			return (long) (ushort) obj;
		} else if (otype == typeof(short)) {
			return (long) (short) obj;
		} else if (otype == typeof(uint)) {
			return (long) (uint) obj;
		} else if (otype == typeof(int)) {
			return (long) (int) obj;
		} else if (otype == typeof(ulong)) {
			return (long) (ulong) obj;
		} else if (otype == typeof(long)) {
			return (long) obj;
		}

		throw new InvalidOperationException("Object is not an integer type.");
	}

	internal static T AsIntType<T>(this object obj) {
		if (typeof(T) == typeof(byte)) {
			return (T) (object) obj.AsByte();
		} else if (typeof(T) == typeof(sbyte)) {
			return (T) (object) obj.AsSByte();
		} else if (typeof(T) == typeof(ushort)) {
			return (T) (object) obj.AsUShort();
		} else if (typeof(T) == typeof(short)) {
			return (T) (object) obj.AsShort();
		} else if (typeof(T) == typeof(uint)) {
			return (T) (object) obj.AsUInt();
		} else if (typeof(T) == typeof(int)) {
			return (T) (object) obj.AsInt();
		} else if (typeof(T) == typeof(ulong)) {
			return (T) (object) obj.AsULong();
		} else if (typeof(T) == typeof(long)) {
			return (T) (object) obj.AsLong();
		} else {
			throw new InvalidOperationException("Object is not an integer type.");
		}
	}

	internal static bool IsIntegerType(this Type type) {
		return type == typeof(byte)
		    || type == typeof(sbyte)
		    || type == typeof(ushort)
		    || type == typeof(short)
		    || type == typeof(int)
		    || type == typeof(uint)
		    || type == typeof(long)
		    || type == typeof(ulong);
	}

	internal static bool IsDecimalType(this Type type) {
		return type == typeof(float)
		    || type == typeof(double)
		    || type == typeof(decimal);
	}

	internal static bool IsNumericType(this Type type) {
		return type.IsIntegerType() || type.IsDecimalType();
	}
}
