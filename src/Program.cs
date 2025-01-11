namespace Seisaboten.NET;

using System.Text.Encodings.Web;
using System.Text.Json;
using Manager;

public static class Program {
	private const string VERSION = "0.1";

	private static JsonSerializerOptions JsonOptions { get; } = new() {
		WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
	};

	public static void Main(string[] args) {
		Console.WriteLine($"Seisaboten.NET v{VERSION}");
		if (args.Length < 1) {
			Console.WriteLine("Please provide a ROM file path.");
			return;
		}

		var romPath = args[0];
		if (!File.Exists(romPath)) {
			Console.WriteLine($"ROM file not found: {romPath}");
			return;
		}

		ProcessRom(romPath);
	}

	private static void ProcessRom(string romPath) {
		var romData = File.ReadAllBytes(romPath);
		var textManager = new TextManager(romData);

		while (true) {
			Console.WriteLine("\nAvailable commands:");
			Console.WriteLine("1. View story table");
			Console.WriteLine("2. View master table");
			Console.WriteLine("3. Export dialog to JSON");
			Console.WriteLine("4. Save ROM");
			Console.WriteLine("5. Exit");
			Console.Write("\nEnter command number: ");
			if (!int.TryParse(Console.ReadLine(), out var choice)) {
				continue;
			}

			switch (choice) {
				case 1:
					ViewStoryTable(textManager);
					break;
				case 2:
					ViewMasterTable(textManager);
					break;
				case 3:
					ExportDialogToJson(textManager);
					break;
				case 4:
					SaveRom(romData, romPath);
					break;
				case 5:
					return;
			}
		}
	}

	private static void ViewStoryTable(TextManager textManager) {
		var storyEntries = textManager.StoryTableList;
		Console.WriteLine($"\nTotal story entries: {storyEntries.Count}");
		Console.Write("Enter entry number to view (0-{0}): ", storyEntries.Count - 1);
		if (!int.TryParse(Console.ReadLine(), out var index) || index < 0 || index >= storyEntries.Count) {
			return;
		}

		var entry = storyEntries[index];
		Console.WriteLine($"\nEntry {index}:");
		Console.WriteLine($"String: {entry.Content}");
		if (entry.Actor.Position == null) {
			return;
		}

		Console.WriteLine($"Actor Position: {entry.Actor.Position}");
		Console.WriteLine($"Actor ID: {entry.Actor.Id:X4}");
	}

	private static void ViewMasterTable(TextManager textManager) {
		var masterTables = textManager.MasterTableList;
		Console.WriteLine($"\nTotal master tables: {masterTables.Count}");
		Console.Write($"Enter table number to view (0-{masterTables.Count - 1}): ");
		if (!int.TryParse(Console.ReadLine(), out var tableIndex) || tableIndex < 0 ||
			tableIndex >= masterTables.Count) {
			return;
		}

		var table = masterTables[tableIndex];
		Console.WriteLine($"\nTable {tableIndex} entries:");
		for (var i = 0; i < table.Count; i++) {
			Console.WriteLine($"[{i}] {table[i]}");
		}
	}

	private static void ExportDialogToJson(TextManager textManager) {
		Console.Write("Enter output JSON file path: ");
		var path = Console.ReadLine();
		if (string.IsNullOrWhiteSpace(path)) {
			return;
		}

		var json = JsonSerializer.Serialize(textManager.StoryTableList, JsonOptions);
		File.WriteAllText(path, json);
		Console.WriteLine($"Dialog exported to {path}");
	}

	private static void SaveRom(byte[] romData, string originalPath) {
		Console.Write("Enter output ROM file path (or press Enter to overwrite): ");
		var path = Console.ReadLine();
		if (string.IsNullOrWhiteSpace(path)) {
			path = originalPath;
		}

		Array.Resize(ref romData, romData.Length + 0x01000000);

		File.WriteAllBytes(path, romData);
		Console.WriteLine($"ROM saved to {path}");
	}
}
