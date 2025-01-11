namespace Seisaboten.NET.Utils;

public static class Validator {
	public static uint CalculateCrc32(byte[] data) {
		var crc = 0xFFFFFFFF;
		foreach (var crcByte in data) {
			crc ^= crcByte;
			for (var j = 0; j < 8; j++) {
				crc = (uint)((crc >> 1) ^ (0xEDB88320 & -(crc & 1)));
			}
		}

		return ~crc;
	}
}
