namespace Seisaboten.NET.Rom;

public class Japan : IRom {
	public string Code => "J";
	public string CleanCrc32 => "88E64A8A";
	public string StoryTextLocation => "3EC0";
	public string RealMasterTableLocation => "65E8";
	public string CharmapStart => "E341EE";
	public string EnemyData => "DFA0F0";
	public string EncounterData => "DF6018";
	public string MonsterBookPointer => "82F48";
}
