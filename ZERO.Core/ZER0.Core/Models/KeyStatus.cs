using System;

namespace ZER0.Core.Models
{
	/// <summary>
	/// Represents the status of a key authentication.
	/// </summary>
	// Token: 0x02000007 RID: 7
	public class KeyStatus
	{
		/// <summary>
		/// Gets or sets a value indicating whether the user is authenticated.
		/// </summary>
		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000022 RID: 34 RVA: 0x0000530C File Offset: 0x0000530C
		// (set) Token: 0x06000023 RID: 35 RVA: 0x00005314 File Offset: 0x00005314
		public bool IsAuthed { get; internal set; }

		/// <summary>
		/// Gets or sets a value indicating whether there was a failure in footprint (HWID) authentication.
		/// </summary>
		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000024 RID: 36 RVA: 0x00005320 File Offset: 0x00005320
		// (set) Token: 0x06000025 RID: 37 RVA: 0x00005328 File Offset: 0x00005328
		public bool FootprintFail { get; internal set; }

		/// <summary>
		/// Gets or sets a message describing the authentication status.
		/// </summary>
		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000026 RID: 38 RVA: 0x00005334 File Offset: 0x00005334
		// (set) Token: 0x06000027 RID: 39 RVA: 0x0000533C File Offset: 0x0000533C
		public string Message { get; internal set; }
	}
}
