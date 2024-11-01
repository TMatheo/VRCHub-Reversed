using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace VRCHub
{
	// Token: 0x0200002B RID: 43
	[NullableContext(1)]
	[Nullable(0)]
	public class ServerAPI : IDisposable
	{
		// Token: 0x060000D9 RID: 217 RVA: 0x004F039C File Offset: 0x004F039C
		public static string GetServer(string url)
		{
			if (ServerAPI.usingProxy)
			{
				if (url.Contains("api.vrchub.site"))
				{
					url = url.Replace("api.vrchub.site", "magmamc.dev/ServerProxy/vrchub");
				}
				else if (url.Contains("datapacks.vrchub.site"))
				{
					url = url.Replace("datapacks.vrchub.site", "magmamc.dev/ServerProxy/vrchub/datapacks");
				}
				else if (url.Contains("software.vrchub.site"))
				{
					url = url.Replace("software.vrchub.site", "magmamc.dev/ServerProxy/vrchub/software");
				}
				Console.WriteLine("[HTTP PROXY] " + url);
			}
			else
			{
				Console.WriteLine("[HTTP] " + url);
			}
			return url;
		}

		// Token: 0x060000DA RID: 218 RVA: 0x004F0434 File Offset: 0x004F0434
		public ServerAPI()
		{
			this.HTTP = new HttpClient();
			this.HTTP.DefaultRequestHeaders.Add("Connection", "keep-alive");
			this.HTTP.DefaultRequestHeaders.Add("Cache-Control", "no-cache");
			this.HTTP.DefaultRequestHeaders.Add("Pragma", "no-cache");
			this.HTTP.DefaultRequestHeaders.Add("Upgrade-Insecure-Requests", "1");
			this.HTTP.DefaultRequestHeaders.Add("Accept", "application/json, application/zip, application/octet-stream, text/plain, image/*, */*");
			this.HTTP.DefaultRequestHeaders.Add("User-Agent", "HttpClient/9.0 EasyServiceAPI/1.2");
			this.HTTP.Timeout = TimeSpan.FromSeconds(10.0);
		}

		// Token: 0x060000DB RID: 219 RVA: 0x004F0508 File Offset: 0x004F0508
		public static HttpClient CreateByteDownloader(bool cache = false)
		{
			HttpClient httpClient = new HttpClient();
			httpClient.DefaultRequestHeaders.Add("Connection", "keep-alive");
			if (!cache)
			{
				httpClient.DefaultRequestHeaders.Add("Cache-Control", "no-cache");
				httpClient.DefaultRequestHeaders.Add("Pragma", "no-cache");
			}
			else
			{
				httpClient.DefaultRequestHeaders.Add("Cache-Control", "public, max-age=86400");
			}
			httpClient.DefaultRequestHeaders.Add("Upgrade-Insecure-Requests", "1");
			httpClient.DefaultRequestHeaders.Add("Accept", "application/json, application/zip, application/octet-stream, image/*");
			httpClient.DefaultRequestHeaders.Add("User-Agent", "HttpClient/9.0 EasyServiceAPI/1.2");
			httpClient.Timeout = TimeSpan.FromMinutes(5.0);
			return httpClient;
		}

		// Token: 0x060000DC RID: 220 RVA: 0x004F05C8 File Offset: 0x004F05C8
		public Task<string> GetStringAsync(string url)
		{
			return this.HTTP.GetStringAsync(url);
		}

		// Token: 0x060000DD RID: 221 RVA: 0x004F05D8 File Offset: 0x004F05D8
		public Task<byte[]> GetByteArrayAsync(string url)
		{
			return this.HTTP.GetByteArrayAsync(url);
		}

		// Token: 0x060000DE RID: 222 RVA: 0x004F05E8 File Offset: 0x004F05E8
		[return: Nullable(new byte[]
		{
			1,
			2
		})]
		public Task<T> GetFromJsonAsync<[Nullable(2)] T>(string url)
		{
			return this.HTTP.GetFromJsonAsync(url, default(CancellationToken));
		}

		// Token: 0x060000DF RID: 223 RVA: 0x004F060C File Offset: 0x004F060C
		public bool CheckServer(string server)
		{
			bool result = false;
			try
			{
				using (HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Head, ServerAPI.GetServer(server)))
				{
					using (HttpResponseMessage result2 = this.HTTP.SendAsync(httpRequestMessage).GetAwaiter().GetResult())
					{
						if (result2.IsSuccessStatusCode)
						{
							result = true;
						}
					}
				}
			}
			catch
			{
			}
			return result;
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x004F0698 File Offset: 0x004F0698
		public void Dispose()
		{
			object httplock = ServerAPI.HTTPLock;
			lock (httplock)
			{
				HttpClient http = this.HTTP;
				if (http != null)
				{
					http.Dispose();
				}
				this.HTTP = null;
			}
		}

		// Token: 0x040000D8 RID: 216
		public static readonly string[] Servers = new string[]
		{
			"https://vrchub.site",
			"https://api.vrchub.site/API/2/Status",
			"https://datapacks.vrchub.site/List.php",
			"https://software.vrchub.site/Hash/"
		};

		// Token: 0x040000D9 RID: 217
		public static bool usingProxy = false;

		// Token: 0x040000DA RID: 218
		[Nullable(2)]
		public HttpClient HTTP;

		// Token: 0x040000DB RID: 219
		private static readonly object HTTPLock = new object();
	}
}
