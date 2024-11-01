using System;
using System.Runtime.CompilerServices;

namespace VRCHub.Models
{
	// Token: 0x02000035 RID: 53
	[NullableContext(1)]
	[Nullable(0)]
	public class VRCW
	{
		// Token: 0x1700002B RID: 43
		// (get) Token: 0x0600014E RID: 334 RVA: 0x004F175C File Offset: 0x004F175C
		// (set) Token: 0x0600014F RID: 335 RVA: 0x004F1764 File Offset: 0x004F1764
		public string authorId { get; set; } = "";

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000150 RID: 336 RVA: 0x004F1770 File Offset: 0x004F1770
		// (set) Token: 0x06000151 RID: 337 RVA: 0x004F1778 File Offset: 0x004F1778
		public string authorName { get; set; } = "";

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000152 RID: 338 RVA: 0x004F1784 File Offset: 0x004F1784
		// (set) Token: 0x06000153 RID: 339 RVA: 0x004F178C File Offset: 0x004F178C
		public string description { get; set; } = "";

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000154 RID: 340 RVA: 0x004F1798 File Offset: 0x004F1798
		// (set) Token: 0x06000155 RID: 341 RVA: 0x004F17A0 File Offset: 0x004F17A0
		public string id { get; set; } = "";

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x06000156 RID: 342 RVA: 0x004F17AC File Offset: 0x004F17AC
		// (set) Token: 0x06000157 RID: 343 RVA: 0x004F17B4 File Offset: 0x004F17B4
		public string name { get; set; } = "";

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x06000158 RID: 344 RVA: 0x004F17C0 File Offset: 0x004F17C0
		// (set) Token: 0x06000159 RID: 345 RVA: 0x004F17C8 File Offset: 0x004F17C8
		public string thumbnailImageUrl { get; set; } = "";

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x0600015A RID: 346 RVA: 0x004F17D4 File Offset: 0x004F17D4
		// (set) Token: 0x0600015B RID: 347 RVA: 0x004F17DC File Offset: 0x004F17DC
		public int version { get; set; }

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x0600015C RID: 348 RVA: 0x004F17E8 File Offset: 0x004F17E8
		// (set) Token: 0x0600015D RID: 349 RVA: 0x004F17F0 File Offset: 0x004F17F0
		public string updated_at { get; set; } = "";

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x0600015E RID: 350 RVA: 0x004F17FC File Offset: 0x004F17FC
		// (set) Token: 0x0600015F RID: 351 RVA: 0x004F1804 File Offset: 0x004F1804
		public int visits { get; set; }
	}
}
