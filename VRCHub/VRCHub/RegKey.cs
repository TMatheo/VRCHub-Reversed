using System;
using System.Runtime.CompilerServices;

namespace VRCHub
{
	// Token: 0x02000007 RID: 7
	[NullableContext(1)]
	[Nullable(0)]
	public class RegKey
	{
		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000024 RID: 36 RVA: 0x004EBC50 File Offset: 0x004EBC50
		// (set) Token: 0x06000025 RID: 37 RVA: 0x004EBC58 File Offset: 0x004EBC58
		public string ObjectName { get; set; } = "";

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000026 RID: 38 RVA: 0x004EBC64 File Offset: 0x004EBC64
		// (set) Token: 0x06000027 RID: 39 RVA: 0x004EBC6C File Offset: 0x004EBC6C
		[Nullable(2)]
		public object Value { [NullableContext(2)] get; [NullableContext(2)] set; }
	}
}
