namespace Seisaboten.NET.Manager;

using System.Text;
using Info;
using Rom;

public class TextManager {
	private const int HIRAGANA_SMALL_A_GAME = 0x005b;
	private const int KATAKANA_SMALL_A_GAME = 0x00ab;
	private const int HIRAGANA_SMALL_A_UNICODE = 0x3041;
	private const int KATAKANA_SMALL_A_UNICODE = 0x30A1;
	private const int CHARMAP_LENGTH = 0xD6;

	private readonly Dictionary<int, string> _charDict = [];
	private readonly StringDecoder _decoder;

	private readonly string _englishChars =
		"ABCDEFGHIJKLMNOPQRSTUVWXYZ()[]abcdefghijklmnopqrstuvwxyz「」『』0123456789,.·\"-:…⋯!? /~<>À♪★+&";

	private readonly byte[] _fileData;
	private readonly Dictionary<string, int> _invCharDict = [];

	private readonly string _japaneseChars =
		"ABCDEFGHIJKLMNOPQRSTUVWXYZ()[]abcdefghijklmnopqrstuvwxyz「」『』0123456789、。·\"-:⋯‥!?ー/~<>♥️♪★+&";

	private readonly string _kanjiList =
		"哀愛悪握扱安暗宷闇以囲意異?遦域育一印員因引飲隠右?噂運雲影映栄英衛越延炎遠汚奥応押王屋憶桶乙俺穏恩音下化仮何価加可嫁家?果歌河火花過我画餓会解回壊怪悔改?海界開階外鎧??覚革学楽掛割活滑噛乾喚感慣換敢棺看管簡観間関館頑顔?企危喜器奇寄希揮機帰気祈記貫起輝飢騎鬼偽?技犠疑義議客逆仇休及吸宮急救求泣究給去居拠許供共協叫強恐教況狂興郷鏡?仰?琴禁近金吟苦駆具愚喰空屈掘窟君軍係刑兄形恵提?計?激決結血月件健剣???犬肩見賢険元原幻減現言個古呼孤故枯湖誇五互後御悟語誤護?光公?ロ向坑好幸広控攻構皇紅考行鉱高号合拷刻国酷黒獄骨込頃今婚根魂左差砂鎖座催最妻才歳?砕祭在材罪作策殺雑?三参山散産斬残仕使士姉姿子志思指支止死私紙視詩試資飼事似侍慈持時次治耳自識執失室質実射捨?者車蛇邪借?若弱主取守手狩酒受呪寿樹周修終習??集?住十獣垂宿出瞬準純順処初所緒?助女除傷勝召商小少床招沼消焼照硝章笑証詳障上丈乗冗城場嬢常情?状色食信侵寝心?新森深申真神臣親身進震人尋吹水衰数世制勢征性成政整晴正牲生盛精聖声西誓請静席惜昔石責?切接説絶先占戦洗染線船選前然全狙祖組阻双想早巣争相窓聡草装走送騒像増憎造側即息束足賊族続存尊村他多堕打駄体対待態替?袋貸退代台大題滝択託逹?脱竪誰単探短端鍛壇断段男談値知地置遅築茶?中仲注張彫朝町調長鳥直沈追痛通停定帝底庭弟締艇敵的溺鉄天展転伝殿吐徒渡登賭途度土?怒倒塔投東盗当等答統討踏逃頭闘動同堂導洞道得特毒独読内謎難二日乳入任認熱年念燃悩?能派破廃敗杯背配売伯泊白迫?肌発抜判反飯番否妃彼悲秘非飛備眉美必姫百氷評病品不付夫怖敷浮父腐負武舞部封風伏復服福腹沸仏物分文聞兵平閉別変片返捕歩補??母報抱放方法砲褒飽亡忘暴望冒北?本魔妹毎末万満味未魅密脈民眠務夢無霧娘冥名命明迷鳴?面盲木目戻貰問紋門治夜野役約薬訳躍油優勇友幽有由誘遊雄予余与幼妖容様用集要欲来頼落乱覧利理離陸立流竜旅了両料良?カ涙令冷礼隷霊裂恋練蓮連路浪牢老和話惑腕掟絆盾移防降灼煙雫剛黄球虫肉匕効系?類亜植昆嗅聴昼設帳標南岸司業完週扉裏捜星曜買辺図符種綿絹布甲象牙銅鋼鉛銀魚隕晶赤廊?丘半避儖倉庫橋岩潜景突板歴史衆刈勉詠唱喚耐?罰再保鍵碑録?冊棚矛称匹+.,…'濃雨雪菜響触限↑←↓→菓易荒検爆介?功日謝術誕宴収字建汝?荷払便製弓叩帽々経験操低青髪敬脅速慎更期踊排繰点島写基礎隊困縁絡久労妨示軌園表威秒副倍泡包複費柱尾弾届距杭崖率谷店貨浴夏校雷巨崩衝詮始隣己氏郎眼契比級趣胃預宝券訪超混軽俳徊努汗犯恥富?淪局珍罠清楚胸懐候詰刀拒混沌留監芸唄辛例紹刃圏些細恨歯描弁巻妙都迎焦察削膝諦永却謀覗条酔?領黙厳拾固吉蝕?漂普告陽圧剰枢胆蜂粉往際尽暖針俗訴枚酬額卒働昨第提依従塞害徴源矢枕市劇批拘拝?句棒稼挙雇欠甘桁災式殊?悴横穴米粗太宅鉢承譲寂凍?戒潮劫某継怨畜幹丸略凶紛党華肖祝微羽玉首炉師位測?隅卵折壁快績縛呂煮?疎?偏絵亀糸四浅枝疲泉津授炉忠境裕首担損箱劣培善歓墜堵瞳透薄卑?程丁皮腰臆遇隙群創千揺叶粋寒偉淵鱗宮没採棲";

	// Lists to store table data
	private readonly List<int> _masterTableAddresses = [];
	private readonly List<List<string>> _masterTableList = [];
	private readonly IRom _rom;
	private readonly List<Dialog> _storyTableList = [];
	private int _storyTableAddress;

	public TextManager(byte[] fileData) {
		_rom = IRom.ValidateRomInfo(fileData);
		_fileData = fileData;
		InitializeCharacterDictionaries();
		_decoder = new StringDecoder(_charDict);
		InitializeStoryTableAddress();
		_storyTableList = ReadStoryTable(_storyTableAddress);
		ReadMasterTable();
	}

	public IReadOnlyList<List<string>> MasterTableList => _masterTableList;
	public IReadOnlyList<Dialog> StoryTableList => _storyTableList;

	private void InitializeCharacterDictionaries() {
		var hiraganaString = BuildKanaString(HIRAGANA_SMALL_A_UNICODE, 83, ["ゐ", "ゑ", "ゎ"]);
		var katakanaString = BuildKanaString(KATAKANA_SMALL_A_UNICODE, 84, ["ヰ", "ヱ", "ヮ"]);

		for (var i = 0; i < hiraganaString.Length; i++) {
			_charDict[HIRAGANA_SMALL_A_GAME + i] = hiraganaString[i].ToString();
		}

		for (var i = 0; i < katakanaString.Length; i++) {
			_charDict[KATAKANA_SMALL_A_GAME + i] = katakanaString[i].ToString();
		}

		for (var i = 0; i < _kanjiList.Length; i++) {
			_charDict[i + 0x010C] = _kanjiList[i].ToString();
		}

		var charmapStart = Convert.ToInt32(_rom.CharmapStart, 16);
		var position = 0;

		for (var i = 0; i < CHARMAP_LENGTH; i += 2) {
			var x = _fileData[charmapStart + i];
			var y = _fileData[charmapStart + i + 1];
			var charCode = (x << 8) | y;

			if (charCode != 0xFFFF) {
				var chars = _rom.Code == "J"? _japaneseChars : _englishChars;
				if (position < chars.Length) {
					_charDict[charCode] = chars[position].ToString();
				}

				position++;
			}
		}

		Dictionary<int, string> manualMappings = new() {
			{ 0x04a1, "." },
			{ 0x04a2, "," },
			{ 0x04a4, "'" },
			{ 0x11ff, "\n" },
			{ 0x0093, "'" },
			{ 0x0095, ";" },
			{ 0x009d, "\"" },
			{ 0x000d, "\"" },
			{ 0x0000, " " },
			{ 0x0008, "ー" }
		};

		foreach (var mapping in manualMappings) {
			_charDict[mapping.Key] = mapping.Value;
		}

		foreach (var item in _charDict) {
			_invCharDict.TryAdd(item.Value, item.Key);
		}
	}

	private static string BuildKanaString(int startCode, int count, string[] charsToRemove) {
		StringBuilder sb = new();
		for (var i = 0; i < count; i++) {
			_ = sb.Append(char.ConvertFromUtf32(startCode + i));
		}

		var result = sb.ToString();
		foreach (var charToRemove in charsToRemove) {
			result = result.Replace(charToRemove, "");
		}

		return result;
	}

	private void InitializeStoryTableAddress() {
		var storyTableOffset = Convert.ToInt32(_rom.StoryTextLocation, 16);
		var bytes = new byte[4];
		Array.Copy(_fileData, storyTableOffset, bytes, 0, 4);
		_storyTableAddress = BitConverter.ToInt32(bytes, 0) - 0x08000000;
	}

	public List<Dialog> ReadStoryTable(int tableStart) {
		var numEntries = BitConverter.ToInt16(_fileData, tableStart + 0x2);
		var numOffsets = numEntries + 1;
		var offsetsBlock = new byte[0x2 * numOffsets];
		Array.Copy(_fileData, tableStart + 0x4, offsetsBlock, 0, offsetsBlock.Length);

		List<int> offsetList = [];
		var overflowCount = 0;
		var prevValue = 0;

		for (var i = 0; i < offsetsBlock.Length; i += 2) {
			var value = (offsetsBlock[i + 1] << 8) | offsetsBlock[i];
			if (prevValue > value) {
				overflowCount++;
			}

			prevValue = value;
			value += 0x10000 * overflowCount;
			value += tableStart;
			offsetList.Add(value);
		}

		List<(int first, int second)> addressList =
			[.. offsetList.Zip(offsetList.Skip(1), static (first, second) => (first, second))];
		List<Dialog> storyList = [];

		foreach (var (start, end) in addressList) {
			Dialog info = new() {
				Id = storyList.Count,
				Start = start,
				End = end,
				Actor = new Actor(),
				Content = string.Empty
			};

			var firstByte = _fileData[info.Start];
			if (start < end) {
				if (firstByte is 0x8B or 0x99) {
					info.Actor.Position = firstByte == 0x8B? "Left" : "Right";
					info.Actor.Id = BitConverter.ToInt16(_fileData, info.Start + 0x1);
					ArraySegment<byte> textBytes = new(_fileData, info.Start + 0x3, end - (info.Start + 0x3));
					info.Content = _decoder.Decode(textBytes);
				}
				else {
					ArraySegment<byte> textBytes = new(_fileData, info.Start, end - info.Start);
					info.Content = _decoder.Decode(textBytes);
				}
			}
			else {
				info.Content = "{BLANK}";
			}

			storyList.Add(info);
		}

		return storyList;
	}

	private void ReadMasterTable() {
		var masterTableOffset = Convert.ToInt32(_rom.RealMasterTableLocation, 16);
		var masterTableStartAddress = BitConverter.ToInt32(_fileData, masterTableOffset) - 0x08000000;

		var numEntries = BitConverter.ToInt32(_fileData, masterTableStartAddress + 0x8);
		var offsetStart = masterTableStartAddress + 0x4 + 0x8;

		for (var i = 0; i < numEntries; i++) {
			var startOffsetAddress = offsetStart + (i * 0x4);
			var startOffset = BitConverter.ToInt32(_fileData, startOffsetAddress);
			var tableStartAddress = masterTableStartAddress + startOffset;
			_masterTableAddresses.Add(tableStartAddress);
		}

		_ = GetEndOfTable(_masterTableAddresses[^1]);

		for (var i = 0; i < _masterTableAddresses.Count; i++) {
			if (i is 43 or 47) {
				_masterTableList.Add(["Unavailable for editing"]);
			}
			else {
				_masterTableList.Add(GetAllEntriesTextTable(_masterTableAddresses[i]));
			}
		}
	}

	private int GetEndOfTable(int tableStart) {
		var numEntries = BitConverter.ToInt32(_fileData, tableStart);
		var offsetStart = tableStart + 0x4;
		var startOffsetAddress = offsetStart + ((numEntries - 1) * 0x2);
		var endOffsetAddress = startOffsetAddress + 0x2;
		var endOffset = BitConverter.ToInt16(_fileData, endOffsetAddress);
		return tableStart + endOffset;
	}

	public List<string> GetAllEntriesTextTable(int tableStart) {
		var numEntries = BitConverter.ToInt32(_fileData, tableStart);
		List<string> textList = [];

		for (var i = 0; i < numEntries; i++) {
			var text = GetElementTextTable(tableStart, i);
			textList.Add(text);
		}

		return textList;
	}

	public string GetElementTextTable(int tableStart, int itemNum) {
		var offsetStart = tableStart + 0x4;
		var startOffsetAddress = offsetStart + (itemNum * 0x2);
		var endOffsetAddress = startOffsetAddress + 0x2;

		var startOffset = BitConverter.ToInt16(_fileData, startOffsetAddress);
		var endOffset = BitConverter.ToInt16(_fileData, endOffsetAddress);

		var startTextAddress = tableStart + startOffset;
		var endTextAddress = tableStart + endOffset;

		var textBytes = new byte[endTextAddress - startTextAddress];
		Array.Copy(_fileData, startTextAddress, textBytes, 0, textBytes.Length);

		StringBuilder result = new();
		for (var i = 0; i < textBytes.Length; i += 2) {
			var charCode = (textBytes[i] << 8) | textBytes[i + 1];
			if (_charDict.TryGetValue(charCode, out var letter)) {
				_ = result.Append(letter);
			}
		}

		return result.ToString();
	}

	public void RecreateEnemyNameTable(int tableStart, List<string> nameList) {
		var entryCount = nameList.Count;
		if (_fileData[tableStart] != entryCount) {
			throw new InvalidOperationException("Writing a different amount of entries than original");
		}

		var offsetStart = tableStart + 0x4;
		var textStart = tableStart + 0x4 + (entryCount * 0x2);
		var textEndByteAddress = textStart;

		for (var i = 0; i < nameList.Count; i++) {
			List<byte> nameBytes = [];
			foreach (var ch in nameList[i]) {
				if (_invCharDict.TryGetValue(ch.ToString(), out var charCode)) {
					var bytes = BitConverter.GetBytes((ushort)charCode);
					nameBytes.AddRange(bytes);
				}
			}

			var textStartByteAddress = textEndByteAddress;
			textEndByteAddress += nameBytes.Count;

			var currentOffsetAddress = offsetStart + ((i + 1) * 0x2);
			var currentOffsetValue = BitConverter.GetBytes((ushort)(textEndByteAddress - tableStart));

			Array.Copy(nameBytes.ToArray(), 0, _fileData, textStartByteAddress, nameBytes.Count);
			Array.Copy(currentOffsetValue, 0, _fileData, currentOffsetAddress, 2);
		}
	}
}
