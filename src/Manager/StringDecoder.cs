namespace Seisaboten.NET.Manager;

using System.Text;

public class StringDecoder(Dictionary<int, string> charDict) : IDecoder {
	private readonly Dictionary<int, string> _charDict = charDict ?? throw new ArgumentNullException(nameof(charDict));

	public string Decode(IEnumerable<byte> encoded) {
		ArgumentNullException.ThrowIfNull(encoded);
		StringBuilder result = new();
		using (var enumerator = encoded.GetEnumerator()) {
			try {
				while (enumerator.MoveNext()) {
					var a = enumerator.Current;
					switch (a) {
						case 0x80:
							_ = result.Append('\n');
							break;

						case 0x82:
							_ = result.Append("{A}");
							break;

						case 0x83:
							_ = result.Append("{CHOICE}");
							break;

						case 0x84:
							_ = result.Append("{END_CHOICES}");
							break;

						case 0x8E:
							_ = result.Append("{RED}");
							break;

						case 0x8D:
							_ = result.Append("{END_COLOR}");
							break;

						case 0x8B:
						case 0x99:
							if (!enumerator.MoveNext()) {
								return "error";
							}

							var c1 = enumerator.Current;

							if (!enumerator.MoveNext()) {
								return "error";
							}

							var c2 = enumerator.Current;

							var actorId = (c1 << 8) | c2;
							var positionValue = a == 0x8B? "POS_L" : "POS_R";
							_ = result.Append($"{positionValue} ACTOR_{actorId:X4}\n");
							break;

						default:
							if (!enumerator.MoveNext()) {
								return "error";
							}

							var b = enumerator.Current;
							var charCode = (a << 8) | b;

							if (a == 0x86) {
								var actor = b switch {
									0 => "{HERO}",
									1 => "{HEROINE}",
									_ => $"ACTOR{b}"
								};
								_ = result.Append(actor);
								continue;
							}

							if (_charDict.TryGetValue(charCode, out var letter)) {
								_ = result.Append(letter);
							}

							break;
					}
				}
			}
			catch (Exception) {
				return "error";
			}
		}

		return result.ToString();
	}
}
