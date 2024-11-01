using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.Json;
using MagmaMc.JEF;

namespace VRCHub
{
	// Token: 0x02000005 RID: 5
	[NullableContext(1)]
	[Nullable(0)]
	public static class Config
	{
		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000014 RID: 20 RVA: 0x004EBA50 File Offset: 0x004EBA50
		// (set) Token: 0x06000015 RID: 21 RVA: 0x004EBA58 File Offset: 0x004EBA58
		public static string VRC_Path { get; set; } = "C:\\Program Files (x86)\\Steam\\steamapps\\common\\VRChat\\VRChat.exe";

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000016 RID: 22 RVA: 0x004EBA60 File Offset: 0x004EBA60
		// (set) Token: 0x06000017 RID: 23 RVA: 0x004EBA68 File Offset: 0x004EBA68
		public static bool SendAnalytics { get; set; } = true;

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000018 RID: 24 RVA: 0x004EBA70 File Offset: 0x004EBA70
		// (set) Token: 0x06000019 RID: 25 RVA: 0x004EBA78 File Offset: 0x004EBA78
		[Nullable(new byte[]
		{
			2,
			1
		})]
		public static RegKey[] VRChatRegBackup { [return: Nullable(new byte[]
		{
			2,
			1
		})] get; [param: Nullable(new byte[]
		{
			2,
			1
		})] set; }

		// Token: 0x0600001A RID: 26 RVA: 0x004EBA80 File Offset: 0x004EBA80
		public static void LoadConfig()
		{
			try
			{
				if (!Directory.Exists(Config.ConfigPath))
				{
					Directory.CreateDirectory(Config.ConfigPath);
				}
				if (File.Exists(Config.ConfigFilename))
				{
					Config.ConfigData configData = JsonSerializer.Deserialize<Config.ConfigData>(File.ReadAllText(Config.ConfigFilename), Config.JsonConfig);
					if (configData != null)
					{
						Config.VRC_Path = (configData.VRC_Path ?? Config.VRC_Path);
						Config.SendAnalytics = configData.SendAnalytics;
						Config.VRChatRegBackup = configData.VRChatRegBackup;
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error loading config: " + ex.Message);
			}
		}

		// Token: 0x0600001B RID: 27 RVA: 0x004EBB1C File Offset: 0x004EBB1C
		public static void SaveConfig()
		{
			try
			{
				if (!Directory.Exists(Config.ConfigPath))
				{
					Directory.CreateDirectory(Config.ConfigPath);
				}
				string contents = JsonSerializer.Serialize<Config.ConfigData>(new Config.ConfigData
				{
					VRC_Path = Config.VRC_Path,
					SendAnalytics = Config.SendAnalytics,
					VRChatRegBackup = Config.VRChatRegBackup
				}, Config.JsonConfig);
				File.WriteAllText(Config.ConfigFilename, contents);
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error saving config: " + ex.Message);
			}
		}

		// Token: 0x0400000A RID: 10
		private static readonly string ConfigPath = Path.Combine(JEF.Utils.Folders.LocalUserAppData, "VRCHub");

		// Token: 0x0400000B RID: 11
		private static readonly string ConfigFilename = Path.Combine(Config.ConfigPath, "Config.json");

		// Token: 0x0400000C RID: 12
		private static readonly JsonSerializerOptions JsonConfig = new JsonSerializerOptions
		{
			ReadCommentHandling = JsonCommentHandling.Skip,
			WriteIndented = true,
			IndentSize = 4
		};

		// Token: 0x02000006 RID: 6
		[NullableContext(2)]
		[Nullable(0)]
		private class ConfigData
		{
			// Token: 0x1700000D RID: 13
			// (get) Token: 0x0600001D RID: 29 RVA: 0x004EBC0C File Offset: 0x004EBC0C
			// (set) Token: 0x0600001E RID: 30 RVA: 0x004EBC14 File Offset: 0x004EBC14
			public string VRC_Path { get; set; }

			// Token: 0x1700000E RID: 14
			// (get) Token: 0x0600001F RID: 31 RVA: 0x004EBC20 File Offset: 0x004EBC20
			// (set) Token: 0x06000020 RID: 32 RVA: 0x004EBC28 File Offset: 0x004EBC28
			public bool SendAnalytics { get; set; }

			// Token: 0x1700000F RID: 15
			// (get) Token: 0x06000021 RID: 33 RVA: 0x004EBC34 File Offset: 0x004EBC34
			// (set) Token: 0x06000022 RID: 34 RVA: 0x004EBC3C File Offset: 0x004EBC3C
			[Nullable(new byte[]
			{
				2,
				1
			})]
			public RegKey[] VRChatRegBackup { [return: Nullable(new byte[]
			{
				2,
				1
			})] get; [param: Nullable(new byte[]
			{
				2,
				1
			})] set; }
		}
	}
}
