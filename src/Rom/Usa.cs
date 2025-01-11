namespace Seisaboten.NET.Rom;

public class Usa : IRom {
	public string Code => "U";
	public string CleanCrc32 => "7F1EAC75";
	public string StoryTextLocation => "3EB0";
	public string RealMasterTableLocation => "65D8";
	public string CharmapStart => "E7BB62";
	public string EnemyData => "E415B0";
	public string EncounterData => "E3D4D8";
	public string MonsterBookPointer => "8302C";
}
