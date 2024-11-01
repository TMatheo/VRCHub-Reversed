using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;
using System.Threading;
using System.Threading.Tasks;
using ZER0.Core;
using ZER0.Core.Models;

namespace ZER0.VRChat.Patch
{
	/// <summary>
	/// Provides functionality to patch the VRChat game.
	/// </summary>
	// Token: 0x02000002 RID: 2
	[NullableContext(2)]
	[Nullable(0)]
	[SupportedOSPlatform("Windows")]
	public static class VRCPatch
	{
		/// <summary>
		/// Indicates whether the patcher is authorized to perform operations.
		/// </summary>
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000001 RID: 1 RVA: 0x00005264 File Offset: 0x00005264
		// (set) Token: 0x06000002 RID: 2 RVA: 0x0000526C File Offset: 0x0000526C
		public static bool Authorized { get; internal set; }

		/// <summary>
		/// The path to the VRChat installation directory.
		/// </summary>
		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000003 RID: 3 RVA: 0x00005274 File Offset: 0x00005274
		// (set) Token: 0x06000004 RID: 4 RVA: 0x0000527C File Offset: 0x0000527C
		public static string VRCPath { get; set; }

		/// <summary>
		/// Represents the current download percentage of the patch.
		/// </summary>
		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000005 RID: 5 RVA: 0x00005284 File Offset: 0x00005284
		// (set) Token: 0x06000006 RID: 6 RVA: 0x0000528C File Offset: 0x0000528C
		public static Half DownloadPercent { get; internal set; }

		/// <summary>
		/// Represents the current download speed in mbps.
		/// </summary>
		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000007 RID: 7 RVA: 0x00005294 File Offset: 0x00005294
		// (set) Token: 0x06000008 RID: 8 RVA: 0x0000529C File Offset: 0x0000529C
		public static Half DownloadSpeed { get; internal set; }

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000009 RID: 9 RVA: 0x000052A4 File Offset: 0x000052A4
		// (remove) Token: 0x0600000A RID: 10 RVA: 0x000052D8 File Offset: 0x000052D8
		public static event Action OnGamePatchStart;

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x0600000B RID: 11 RVA: 0x0000530C File Offset: 0x0000530C
		// (remove) Token: 0x0600000C RID: 12 RVA: 0x00005340 File Offset: 0x00005340
		public static event Action OnPatchKeyStarted;

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x0600000D RID: 13 RVA: 0x00005374 File Offset: 0x00005374
		// (remove) Token: 0x0600000E RID: 14 RVA: 0x000053A8 File Offset: 0x000053A8
		public static event Action OnPatchKeySuccess;

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x0600000F RID: 15 RVA: 0x000053DC File Offset: 0x000053DC
		// (remove) Token: 0x06000010 RID: 16 RVA: 0x00005410 File Offset: 0x00005410
		public static event Action OnPatchKeyFail;

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x06000011 RID: 17 RVA: 0x00005444 File Offset: 0x00005444
		// (remove) Token: 0x06000012 RID: 18 RVA: 0x00005478 File Offset: 0x00005478
		public static event Action OnPatchDownloadStarted;

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x06000013 RID: 19 RVA: 0x000054AC File Offset: 0x000054AC
		// (remove) Token: 0x06000014 RID: 20 RVA: 0x000054E0 File Offset: 0x000054E0
		public static event Action OnPatchDownloaded;

		// Token: 0x14000007 RID: 7
		// (add) Token: 0x06000015 RID: 21 RVA: 0x00005514 File Offset: 0x00005514
		// (remove) Token: 0x06000016 RID: 22 RVA: 0x00005548 File Offset: 0x00005548
		public static event Action OnPatchInstall;

		/// <summary>
		/// Reads the stored authorization key from the key store.
		/// </summary>
		/// <returns>Returns the stored authorization key if available, null otherwise.</returns>
		// Token: 0x06000017 RID: 23 RVA: 0x0000557C File Offset: 0x0000557C
		public static string ReadKey()
		{
			KeyContent storedKey = VRCPatch._keyStore.GetStoredKey();
			if (storedKey == null)
			{
				return null;
			}
			return storedKey.Key;
		}

		/// <summary>
		/// Stores the given authorization key in the key store.
		/// </summary>
		/// <param name="Key">The authorization key to store.</param>
		/// <returns>True if the key was stored successfully, false otherwise.</returns>
		// Token: 0x06000018 RID: 24 RVA: 0x00005594 File Offset: 0x00005594
		[NullableContext(1)]
		public static bool StoreKey(string Key)
		{
			return VRCPatch._keyStore.StoreKey(Key, "Lite");
		}

		/// <summary>
		/// Fetches the public IP address from ipinfo.io and caches it.
		/// </summary>
		/// <returns>Public IP as a string</returns>
		// Token: 0x06000019 RID: 25 RVA: 0x000055A8 File Offset: 0x000055A8
		[NullableContext(1)]
		private static string GetPublicIP()
		{
			if (VRCPatch.CachedPublicIP != null)
			{
				return VRCPatch.CachedPublicIP;
			}
			string cachedPublicIP;
			using (HttpClient client = new HttpClient())
			{
				HttpResponseMessage response = client.GetAsync("https://ipinfo.io/ip").GetAwaiter().GetResult();
				if (response.IsSuccessStatusCode)
				{
					VRCPatch.CachedPublicIP = response.Content.ReadAsStringAsync().GetAwaiter().GetResult().Trim();
				}
				else
				{
					VRCPatch.CachedPublicIP = "UnknownIP";
				}
				cachedPublicIP = VRCPatch.CachedPublicIP;
			}
			return cachedPublicIP;
		}

		/// <summary>
		/// Fetches the public IP address asynchronously from ipinfo.io and caches it.
		/// </summary>
		/// <returns>Public IP as a string</returns>
		// Token: 0x0600001A RID: 26 RVA: 0x0000563C File Offset: 0x0000563C
		[NullableContext(1)]
		private static Task<string> GetPublicIPAsync()
		{
			VRCPatch.<GetPublicIPAsync>d__44 <GetPublicIPAsync>d__;
			<GetPublicIPAsync>d__.<>t__builder = AsyncTaskMethodBuilder<string>.Create();
			<GetPublicIPAsync>d__.<>1__state = -1;
			<GetPublicIPAsync>d__.<>t__builder.Start<VRCPatch.<GetPublicIPAsync>d__44>(ref <GetPublicIPAsync>d__);
			return <GetPublicIPAsync>d__.<>t__builder.Task;
		}

		/// <summary>
		/// Authorizes the user by validating the provided key synchronously.
		/// </summary>
		/// <param name="key">The authorization key to validate.</param>
		/// <returns>True if the authorization was successful, false otherwise.</returns>
		// Token: 0x0600001B RID: 27 RVA: 0x00005678 File Offset: 0x00005678
		[NullableContext(1)]
		public static bool Authorize(string key)
		{
			VRCPatch.Key = key;
			if (string.IsNullOrWhiteSpace(VRCPatch.Key) || string.IsNullOrWhiteSpace(VRCPatch.VRCPath))
			{
				return false;
			}
			Action onGamePatchStart = VRCPatch.OnGamePatchStart;
			if (onGamePatchStart != null)
			{
				onGamePatchStart();
			}
			Thread.Sleep(50);
			Action onPatchKeyStarted = VRCPatch.OnPatchKeyStarted;
			if (onPatchKeyStarted != null)
			{
				onPatchKeyStarted();
			}
			string publicIP = VRCPatch.GetPublicIP();
			VRCPatch.HTTP.DefaultRequestHeaders.Clear();
			VRCPatch.HTTP.DefaultRequestHeaders.Add("Authorization", VRCPatch.Key);
			VRCPatch.HTTP.DefaultRequestHeaders.Add("HostIP", publicIP);
			VRCPatch.HTTP.DefaultRequestHeaders.Add("MachineName", Environment.MachineName);
			VRCPatch.HTTP.DefaultRequestHeaders.Add("HashedFootprint", ZER0Key.ComputerFootprint());
			bool result;
			using (HttpResponseMessage message = VRCPatch.HTTP.GetAsync("https://software.vrchub.site/GamePatch/Status.php").GetAwaiter().GetResult())
			{
				if (!message.IsSuccessStatusCode)
				{
					Action onPatchKeyFail = VRCPatch.OnPatchKeyFail;
					if (onPatchKeyFail != null)
					{
						onPatchKeyFail();
					}
					VRCPatch.Authorized = false;
					result = false;
				}
				else
				{
					VRCPatch.Authorized = true;
					Action onPatchKeySuccess = VRCPatch.OnPatchKeySuccess;
					if (onPatchKeySuccess != null)
					{
						onPatchKeySuccess();
					}
					result = true;
				}
			}
			return result;
		}

		/// <summary>
		/// Asynchronously authorizes the user by validating the provided key.
		/// </summary>
		/// <param name="key">The authorization key to validate.</param>
		/// <returns>A task that represents the asynchronous operation. The task result is true if the authorization was successful, false otherwise.</returns>
		// Token: 0x0600001C RID: 28 RVA: 0x000057B8 File Offset: 0x000057B8
		[NullableContext(1)]
		public static Task<bool> AuthorizeAsync(string key)
		{
			VRCPatch.<AuthorizeAsync>d__46 <AuthorizeAsync>d__;
			<AuthorizeAsync>d__.<>t__builder = AsyncTaskMethodBuilder<bool>.Create();
			<AuthorizeAsync>d__.key = key;
			<AuthorizeAsync>d__.<>1__state = -1;
			<AuthorizeAsync>d__.<>t__builder.Start<VRCPatch.<AuthorizeAsync>d__46>(ref <AuthorizeAsync>d__);
			return <AuthorizeAsync>d__.<>t__builder.Task;
		}

		/// <summary>
		/// Patches the game synchronously.
		/// </summary>
		/// <returns>True if the patch was applied successfully, false otherwise.</returns>
		// Token: 0x0600001D RID: 29 RVA: 0x000057FC File Offset: 0x000057FC
		public static bool PatchGame()
		{
			Action onPatchDownloadStarted = VRCPatch.OnPatchDownloadStarted;
			if (onPatchDownloadStarted != null)
			{
				onPatchDownloadStarted();
			}
			if (!VRCPatch.Authorized || string.IsNullOrWhiteSpace(VRCPatch.VRCPath))
			{
				return false;
			}
			byte[] PatchData;
			using (HttpResponseMessage downloadMessage = VRCPatch.HTTP.GetAsync("https://software.vrchub.site/GamePatch/Download.php", HttpCompletionOption.ResponseHeadersRead).GetAwaiter().GetResult())
			{
				downloadMessage.EnsureSuccessStatusCode();
				long totalBytes = downloadMessage.Content.Headers.ContentLength.GetValueOrDefault(1L);
				using (Stream downloadStream = downloadMessage.Content.ReadAsStream())
				{
					using (MemoryStream memoryStream = new MemoryStream())
					{
						byte[] buffer = new byte[2048];
						long totalRead = 0L;
						Stopwatch stopwatch = new Stopwatch();
						stopwatch.Start();
						int bytesRead;
						while ((bytesRead = downloadStream.Read(buffer, 0, buffer.Length)) > 0)
						{
							memoryStream.Write(buffer, 0, bytesRead);
							totalRead += (long)bytesRead;
							VRCPatch.DownloadPercent = (Half)((double)totalRead / (double)totalBytes * 100.0);
							double elapsedSeconds = stopwatch.Elapsed.TotalSeconds;
							if (elapsedSeconds > 0.0)
							{
								VRCPatch.DownloadSpeed = (Half)((double)((float)totalRead / 1024f / 1024f) / elapsedSeconds);
							}
						}
						PatchData = memoryStream.ToArray();
						stopwatch.Stop();
					}
				}
			}
			Action onPatchDownloaded = VRCPatch.OnPatchDownloaded;
			if (onPatchDownloaded != null)
			{
				onPatchDownloaded();
			}
			Thread.Sleep(50);
			bool result;
			using (MemoryStream Data = new MemoryStream(PatchData))
			{
				ZipFile.ExtractToDirectory(Data, VRCPatch.VRCPath, true);
				Action onPatchInstall = VRCPatch.OnPatchInstall;
				if (onPatchInstall != null)
				{
					onPatchInstall();
				}
				result = true;
			}
			return result;
		}

		/// <summary>
		/// Asynchronously patches the game.
		/// </summary>
		/// <returns>A task that represents the asynchronous operation. The task result is true if the patch was applied successfully, false otherwise.</returns>
		// Token: 0x0600001E RID: 30 RVA: 0x00005A08 File Offset: 0x00005A08
		[NullableContext(1)]
		public static Task<bool> PatchGameAsync()
		{
			VRCPatch.<PatchGameAsync>d__48 <PatchGameAsync>d__;
			<PatchGameAsync>d__.<>t__builder = AsyncTaskMethodBuilder<bool>.Create();
			<PatchGameAsync>d__.<>1__state = -1;
			<PatchGameAsync>d__.<>t__builder.Start<VRCPatch.<PatchGameAsync>d__48>(ref <PatchGameAsync>d__);
			return <PatchGameAsync>d__.<>t__builder.Task;
		}

		// Token: 0x04000003 RID: 3
		[Nullable(1)]
		private static readonly KeyStore _keyStore = new KeyStore("Patch");

		// Token: 0x04000004 RID: 4
		[Nullable(1)]
		private static readonly HttpClient HTTP = new HttpClient();

		// Token: 0x04000005 RID: 5
		[Nullable(1)]
		private static string Key = "";

		// Token: 0x04000006 RID: 6
		private static string CachedPublicIP = null;
	}
}
