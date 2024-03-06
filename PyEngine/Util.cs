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
}
