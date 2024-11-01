using System;
using System.Runtime.CompilerServices;

namespace VRCHub.Models
{
	// Token: 0x02000033 RID: 51
	[NullableContext(1)]
	[Nullable(0)]
	public class DataPackage
	{
		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000131 RID: 305 RVA: 0x004F1588 File Offset: 0x004F1588
		// (set) Token: 0x06000132 RID: 306 RVA: 0x004F1590 File Offset: 0x004F1590
		public string WorldName { get; set; } = "";

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000133 RID: 307 RVA: 0x004F159C File Offset: 0x004F159C
		// (set) Token: 0x06000134 RID: 308 RVA: 0x004F15A4 File Offset: 0x004F15A4
		public string WorldHash { get; set; } = "";

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000135 RID: 309 RVA: 0x004F15B0 File Offset: 0x004F15B0
		// (set) Token: 0x06000136 RID: 310 RVA: 0x004F15B8 File Offset: 0x004F15B8
		public string WorldID { get; set; } = "";

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000137 RID: 311 RVA: 0x004F15C4 File Offset: 0x004F15C4
		// (set) Token: 0x06000138 RID: 312 RVA: 0x004F15CC File Offset: 0x004F15CC
		public string Version { get; set; } = "";

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000139 RID: 313 RVA: 0x004F15D8 File Offset: 0x004F15D8
		// (set) Token: 0x0600013A RID: 314 RVA: 0x004F15E0 File Offset: 0x004F15E0
		public string Author { get; set; } = "";

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x0600013B RID: 315 RVA: 0x004F15EC File Offset: 0x004F15EC
		// (set) Token: 0x0600013C RID: 316 RVA: 0x004F15F4 File Offset: 0x004F15F4
		public string Discord { get; set; } = "";
	}
}
