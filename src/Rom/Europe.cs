namespace Seisaboten.NET.Rom;

public class Europe : IRom {
	public string Code => "E";
	public string CleanCrc32 => "31B220E5";
	public string StoryTextLocation => "3EB0";
	public string RealMasterTableLocation => "65D8";
	public string CharmapStart => "E7BB62";
	public string EnemyData => "E415B0";
	public string EncounterData => "E3D4D8";
	public string MonsterBookPointer => "8302C";
}
