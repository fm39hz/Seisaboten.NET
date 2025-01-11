namespace Seisaboten.NET.Rom;

public static class RomFactory {
	public static IRom Create(string romCode) =>
		romCode switch {
			"AVSJ" => new Japan(),
			"AVSE" => new Usa(),
			"AVSP" => new Europe(),
			_ => throw new ArgumentOutOfRangeException(nameof(romCode), romCode, null)
		};
}
