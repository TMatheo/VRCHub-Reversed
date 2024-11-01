using System;
using System.IO;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;

namespace VRCHub.Models
{
	// Token: 0x02000034 RID: 52
	[NullableContext(1)]
	[Nullable(0)]
	public class VRCConfig
	{
		// Token: 0x17000025 RID: 37
		// (get) Token: 0x0600013E RID: 318 RVA: 0x004F1658 File Offset: 0x004F1658
		// (set) Token: 0x0600013F RID: 319 RVA: 0x004F1660 File Offset: 0x004F1660
		public int cache_expire_delay { get; set; }

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000140 RID: 320 RVA: 0x004F166C File Offset: 0x004F166C
		// (set) Token: 0x06000141 RID: 321 RVA: 0x004F1674 File Offset: 0x004F1674
		public int cache_size { get; set; }

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000142 RID: 322 RVA: 0x004F1680 File Offset: 0x004F1680
		// (set) Token: 0x06000143 RID: 323 RVA: 0x004F1688 File Offset: 0x004F1688
		public string cache_directory { get; set; } = "";

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000144 RID: 324 RVA: 0x004F1694 File Offset: 0x004F1694
		// (set) Token: 0x06000145 RID: 325 RVA: 0x004F169C File Offset: 0x004F169C
		public int fpv_steadycam_fov { get; set; }

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000146 RID: 326 RVA: 0x004F16A8 File Offset: 0x004F16A8
		// (set) Token: 0x06000147 RID: 327 RVA: 0x004F16B0 File Offset: 0x004F16B0
		public int camera_res_height { get; set; }

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000148 RID: 328 RVA: 0x004F16BC File Offset: 0x004F16BC
		// (set) Token: 0x06000149 RID: 329 RVA: 0x004F16C4 File Offset: 0x004F16C4
		public int camera_res_width { get; set; }

		// Token: 0x0600014A RID: 330 RVA: 0x004F16D0 File Offset: 0x004F16D0
		public static string GetVRChatPath()
		{
			return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "..\\LocalLow\\VRChat\\VRChat\\");
		}

		// Token: 0x0600014B RID: 331 RVA: 0x004F16E4 File Offset: 0x004F16E4
		public static VRCConfig GetVRChatConfig()
		{
			return JsonConvert.DeserializeObject<VRCConfig>(File.ReadAllText(VRCConfig.GetVRChatPath() + "config.json"));
		}

		// Token: 0x0600014C RID: 332 RVA: 0x004F1700 File Offset: 0x004F1700
		public static string GetContentCachePath()
		{
			string result;
			try
			{
				result = Path.Combine(VRCConfig.GetVRChatConfig().cache_directory, "Cache-WindowsPlayer");
			}
			catch
			{
				result = Path.Combine(VRCConfig.GetVRChatPath(), "Cache-WindowsPlayer\\");
			}
			return result;
		}
	}
}
