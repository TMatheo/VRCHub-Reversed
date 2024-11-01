using System;

namespace ZER0.Core.Models
{
	/// <summary>
	/// Represents Contents of key authentication.
	/// </summary>
	// Token: 0x02000006 RID: 6
	public class KeyContent
	{
		/// <summary>
		/// Main Program Key
		/// </summary>
		// Token: 0x17000002 RID: 2
		// (get) Token: 0x0600001D RID: 29 RVA: 0x000052DC File Offset: 0x000052DC
		// (set) Token: 0x0600001E RID: 30 RVA: 0x000052E4 File Offset: 0x000052E4
		public string Key { get; internal set; }

		/// <summary>
		/// Email For Key Author
		/// </summary>
		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600001F RID: 31 RVA: 0x000052F0 File Offset: 0x000052F0
		// (set) Token: 0x06000020 RID: 32 RVA: 0x000052F8 File Offset: 0x000052F8
		public string Email { get; internal set; }
	}
}
