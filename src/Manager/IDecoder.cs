namespace Seisaboten.NET.Manager;

public interface IDecoder {
	public string Decode(IEnumerable<byte> encoded);
}
