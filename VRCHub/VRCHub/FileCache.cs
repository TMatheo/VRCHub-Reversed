using System;
using System.Runtime.CompilerServices;

namespace VRCHub
{
	// Token: 0x0200002A RID: 42
	[NullableContext(1)]
	[Nullable(0)]
	internal class FileCache
	{
		// Token: 0x17000016 RID: 22
		// (get) Token: 0x060000D2 RID: 210 RVA: 0x004F0340 File Offset: 0x004F0340
		// (set) Token: 0x060000D3 RID: 211 RVA: 0x004F0348 File Offset: 0x004F0348
		public string filename { get; set; } = "";

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x060000D4 RID: 212 RVA: 0x004F0354 File Offset: 0x004F0354
		// (set) Token: 0x060000D5 RID: 213 RVA: 0x004F035C File Offset: 0x004F035C
		public string hash { get; set; } = "";

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x060000D6 RID: 214 RVA: 0x004F0368 File Offset: 0x004F0368
		// (set) Token: 0x060000D7 RID: 215 RVA: 0x004F0370 File Offset: 0x004F0370
		public ulong last_modified { get; set; }
	}
}
