namespace Seisaboten.NET.Rom;

using System.Text;
using Utils;

public interface IRom {
	public static IRom ValidateRomInfo(byte[] romData) {
		var romCode = Encoding.ASCII.GetString(romData.AsSpan(0xAC, 4));
		var crc32 = Validator.CalculateCrc32(romData);
		var crcString = $"{crc32:X8}";
		var regionInfo = RomFactory.Create(romCode);
		var isCleanRom = crcString == regionInfo.CleanCrc32;
		Console.WriteLine($"ROM Region: {regionInfo.GetType().Name}");
		Console.WriteLine($"CRC32: {crcString} ({(isCleanRom? "Clean ROM" : "Modified ROM")})");
		return regionInfo;
	}
	public string Code { get; }
	public string CleanCrc32 { get; }
	public string StoryTextLocation { get; }
	public string RealMasterTableLocation { get; }
	public string CharmapStart { get; }
	public string EnemyData { get; }
	public string EncounterData { get; }
	public string MonsterBookPointer { get; }
}
