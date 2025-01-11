namespace Seisaboten.NET.Info;

public class Dialog {
	public int Id { get; set; }
	public int Start { get; set; }
	public int End { get; set; }
	public required Actor Actor { get; set; }
	public required string Content { get; set; }
}
