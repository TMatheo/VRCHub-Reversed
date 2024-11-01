using System;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace VRCHub
{
	// Token: 0x02000030 RID: 48
	[NullableContext(1)]
	[Nullable(0)]
	public class Version : IComparable<Version>
	{
		// Token: 0x1700001A RID: 26
		// (get) Token: 0x060000FC RID: 252 RVA: 0x004F0F10 File Offset: 0x004F0F10
		// (set) Token: 0x060000FD RID: 253 RVA: 0x004F0F18 File Offset: 0x004F0F18
		public byte major { get; private set; }

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x060000FE RID: 254 RVA: 0x004F0F24 File Offset: 0x004F0F24
		// (set) Token: 0x060000FF RID: 255 RVA: 0x004F0F2C File Offset: 0x004F0F2C
		public byte minor { get; private set; }

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000100 RID: 256 RVA: 0x004F0F38 File Offset: 0x004F0F38
		// (set) Token: 0x06000101 RID: 257 RVA: 0x004F0F40 File Offset: 0x004F0F40
		public byte patch { get; private set; }

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000102 RID: 258 RVA: 0x004F0F4C File Offset: 0x004F0F4C
		// (set) Token: 0x06000103 RID: 259 RVA: 0x004F0F54 File Offset: 0x004F0F54
		public string edition { get; private set; }

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000104 RID: 260 RVA: 0x004F0F60 File Offset: 0x004F0F60
		// (set) Token: 0x06000105 RID: 261 RVA: 0x004F0F68 File Offset: 0x004F0F68
		[Nullable(2)]
		public Uri updateURL { [NullableContext(2)] get; [NullableContext(2)] private set; }

		// Token: 0x06000106 RID: 262 RVA: 0x004F0F74 File Offset: 0x004F0F74
		public Version(byte major, byte minor, byte patch, string edition = "")
		{
			this.major = major;
			this.minor = minor;
			this.patch = patch;
			this.edition = edition;
			this.updateURL = null;
		}

		// Token: 0x06000107 RID: 263 RVA: 0x004F0FA0 File Offset: 0x004F0FA0
		public Version(string versionString)
		{
			string[] array = versionString.Split('-', StringSplitOptions.None);
			string[] array2 = array[0].Split('.', StringSplitOptions.None);
			this.major = byte.Parse(array2[0]);
			this.minor = byte.Parse(array2[1]);
			this.patch = byte.Parse(array2[2]);
			this.edition = ((array.Length > 1) ? array[1] : "");
			this.updateURL = null;
		}

		// Token: 0x06000108 RID: 264 RVA: 0x004F1010 File Offset: 0x004F1010
		public void SetUpdateURL(string url)
		{
			this.updateURL = new Uri(url);
		}

		// Token: 0x06000109 RID: 265 RVA: 0x004F1020 File Offset: 0x004F1020
		public void SetUpdateURL(Uri url)
		{
			this.updateURL = url;
		}

		// Token: 0x0600010A RID: 266 RVA: 0x004F102C File Offset: 0x004F102C
		[NullableContext(2)]
		public Version GetLatestVersion()
		{
			if (this.updateURL == null)
			{
				Console.WriteLine("Update URL is not set.");
				return null;
			}
			Version result;
			try
			{
				using (HttpClient httpClient = new HttpClient())
				{
					httpClient.Timeout = TimeSpan.FromSeconds(8.0);
					JsonDocument jsonDocument = JsonDocument.Parse(httpClient.GetStringAsync(this.updateURL).GetAwaiter().GetResult(), default(JsonDocumentOptions));
					result = new Version(jsonDocument.RootElement.GetProperty("version").GetString())
					{
						updateURL = new Uri(jsonDocument.RootElement.GetProperty("downloadURL").GetString())
					};
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("An error occurred while checking for updates: " + ex.Message);
				result = null;
			}
			return result;
		}

		// Token: 0x0600010B RID: 267 RVA: 0x004F112C File Offset: 0x004F112C
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			if (!(this.edition != ""))
			{
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(2, 3);
				defaultInterpolatedStringHandler.AppendFormatted<byte>(this.major);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				defaultInterpolatedStringHandler.AppendFormatted<byte>(this.minor);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				defaultInterpolatedStringHandler.AppendFormatted<byte>(this.patch);
				return defaultInterpolatedStringHandler.ToStringAndClear();
			}
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(3, 4);
			defaultInterpolatedStringHandler.AppendFormatted<byte>(this.major);
			defaultInterpolatedStringHandler.AppendLiteral(".");
			defaultInterpolatedStringHandler.AppendFormatted<byte>(this.minor);
			defaultInterpolatedStringHandler.AppendLiteral(".");
			defaultInterpolatedStringHandler.AppendFormatted<byte>(this.patch);
			defaultInterpolatedStringHandler.AppendLiteral("-");
			defaultInterpolatedStringHandler.AppendFormatted(this.edition);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x0600010C RID: 268 RVA: 0x004F1204 File Offset: 0x004F1204
		public static Version Parse(string version)
		{
			return new Version(version);
		}

		// Token: 0x0600010D RID: 269 RVA: 0x004F120C File Offset: 0x004F120C
		[NullableContext(2)]
		public int CompareTo(Version other)
		{
			if (other == null)
			{
				return 1;
			}
			if (this.major != other.major)
			{
				return this.major.CompareTo(other.major);
			}
			if (this.minor != other.minor)
			{
				return this.minor.CompareTo(other.minor);
			}
			if (this.patch != other.patch)
			{
				return this.patch.CompareTo(other.patch);
			}
			if (this.edition == "" && other.edition != "")
			{
				return 1;
			}
			if (this.edition != "" && other.edition == "")
			{
				return -1;
			}
			if (this.edition != "" && other.edition != "")
			{
				return string.Compare(this.edition, other.edition, StringComparison.Ordinal);
			}
			return 0;
		}

		// Token: 0x0600010E RID: 270 RVA: 0x004F130C File Offset: 0x004F130C
		[NullableContext(2)]
		public static bool operator ==(Version v1, Version v2)
		{
			return (v1 == null && v2 == null) || (v1 != null && v2 != null && v1.CompareTo(v2) == 0);
		}

		// Token: 0x0600010F RID: 271 RVA: 0x004F1328 File Offset: 0x004F1328
		public static bool operator !=(Version v1, [Nullable(2)] Version v2)
		{
			return v1.CompareTo(v2) != 0;
		}

		// Token: 0x06000110 RID: 272 RVA: 0x004F1334 File Offset: 0x004F1334
		public static bool operator <(Version v1, [Nullable(2)] Version v2)
		{
			return v1.CompareTo(v2) < 0;
		}

		// Token: 0x06000111 RID: 273 RVA: 0x004F1340 File Offset: 0x004F1340
		public static bool operator >(Version v1, [Nullable(2)] Version v2)
		{
			return v1.CompareTo(v2) > 0;
		}

		// Token: 0x06000112 RID: 274 RVA: 0x004F134C File Offset: 0x004F134C
		public static bool operator <=(Version v1, [Nullable(2)] Version v2)
		{
			return v1.CompareTo(v2) <= 0;
		}

		// Token: 0x06000113 RID: 275 RVA: 0x004F135C File Offset: 0x004F135C
		public static bool operator >=(Version v1, [Nullable(2)] Version v2)
		{
			return v1.CompareTo(v2) >= 0;
		}

		// Token: 0x06000114 RID: 276 RVA: 0x004F136C File Offset: 0x004F136C
		[NullableContext(2)]
		public override bool Equals(object obj)
		{
			if (obj == null || base.GetType() != obj.GetType())
			{
				return false;
			}
			Version version = (Version)obj;
			return this.major == version.major && this.minor == version.minor && this.patch == version.patch && this.edition == version.edition;
		}

		// Token: 0x06000115 RID: 277 RVA: 0x004F13D8 File Offset: 0x004F13D8
		public override int GetHashCode()
		{
			return (int)this.major << 16 ^ (int)this.minor << 8 ^ (int)this.patch ^ this.edition.GetHashCode();
		}
	}
}
