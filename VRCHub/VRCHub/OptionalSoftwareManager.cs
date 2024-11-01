using System;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using ZER0.Core;
using ZER0.Core.Cryptography;

namespace VRCHub
{
	// Token: 0x02000028 RID: 40
	[NullableContext(1)]
	[Nullable(0)]
	public static class OptionalSoftwareManager
	{
		// Token: 0x060000C5 RID: 197 RVA: 0x004EFF7C File Offset: 0x004EFF7C
		public static string GetFileHash(string fileName)
		{
			return OptionalSoftwareManager.GetFileHash(File.ReadAllBytes(fileName));
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x004EFF8C File Offset: 0x004EFF8C
		public static string GetFileHash(byte[] data)
		{
			return new Crc32B().ComputeChecksum(data).ToString("X8");
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x004EFFB4 File Offset: 0x004EFFB4
		public static bool SoftwareInstalled(string fileName, string name)
		{
			bool result;
			try
			{
				string text = Path.Combine(OptionalSoftwareManager.GetSoftwarePath(name), fileName);
				result = (File.Exists(text) && OptionalSoftwareManager.CheckSoftwareHashAsync(OptionalSoftwareManager.GetFileHash(text), fileName).GetAwaiter().GetResult());
			}
			catch
			{
				result = false;
			}
			return result;
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x004F000C File Offset: 0x004F000C
		public static void DownloadSoftware(string filename, string name)
		{
			string path = Path.Combine(OptionalSoftwareManager.GetSoftwarePath(name), filename);
			Path.GetExtension(path);
			if (!OptionalSoftwareManager.SoftwareInstalled(filename, name))
			{
				File.WriteAllBytes(path, OptionalSoftwareManager._httpClient.GetByteArrayAsync(ServerAPI.GetServer("https://software.vrchub.site/" + filename)).GetAwaiter().GetResult());
			}
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x004F0064 File Offset: 0x004F0064
		public static void DownloadSoftware(string filename, string name, string MainExecutable)
		{
			string text = Path.Combine(OptionalSoftwareManager.GetSoftwarePath(name), filename);
			Path.GetExtension(text);
			if (!OptionalSoftwareManager.SoftwareInstalled(filename, name))
			{
				OptionalSoftwareManager.DeleteSoftware(name);
				string tempFileName = Path.GetTempFileName();
				File.WriteAllBytes(tempFileName, OptionalSoftwareManager._httpClient.GetByteArrayAsync(ServerAPI.GetServer("https://software.vrchub.site/" + filename)).GetAwaiter().GetResult());
				ZipFile.ExtractToDirectory(tempFileName, text, true);
				File.Delete(tempFileName);
			}
		}

		// Token: 0x060000CA RID: 202 RVA: 0x004F00D4 File Offset: 0x004F00D4
		public static void InstallSoftware(string fileName, string name)
		{
			string text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.StartMenu), "Programs", "VRCHub", name);
			Directory.CreateDirectory(Path.GetDirectoryName(text));
			ShortcutManager.CreateShortcut(text, Path.Combine(OptionalSoftwareManager.GetSoftwarePath(name), fileName), OptionalSoftwareManager.GetSoftwarePath(name), null, null, null);
		}

		// Token: 0x060000CB RID: 203 RVA: 0x004F0114 File Offset: 0x004F0114
		public static void UninstallSoftware(string name)
		{
			ShortcutManager.DeleteShortcut(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.StartMenu), "Programs", "VRCHub", name));
		}

		// Token: 0x060000CC RID: 204 RVA: 0x004F0134 File Offset: 0x004F0134
		public static void DeleteSoftware(string name)
		{
			string softwarePath = OptionalSoftwareManager.GetSoftwarePath(name);
			try
			{
				Directory.Delete(softwarePath);
			}
			catch
			{
			}
		}

		// Token: 0x060000CD RID: 205 RVA: 0x004F0164 File Offset: 0x004F0164
		public static Task<bool> CheckSoftwareHashAsync(string hash, string filename)
		{
			OptionalSoftwareManager.<CheckSoftwareHashAsync>d__9 <CheckSoftwareHashAsync>d__;
			<CheckSoftwareHashAsync>d__.<>t__builder = AsyncTaskMethodBuilder<bool>.Create();
			<CheckSoftwareHashAsync>d__.hash = hash;
			<CheckSoftwareHashAsync>d__.filename = filename;
			<CheckSoftwareHashAsync>d__.<>1__state = -1;
			<CheckSoftwareHashAsync>d__.<>t__builder.Start<OptionalSoftwareManager.<CheckSoftwareHashAsync>d__9>(ref <CheckSoftwareHashAsync>d__);
			return <CheckSoftwareHashAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060000CE RID: 206 RVA: 0x004F01B0 File Offset: 0x004F01B0
		public static string GetSoftwarePath(string name)
		{
			string text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "VRCHub", name);
			Directory.CreateDirectory(text);
			return text;
		}

		// Token: 0x040000CF RID: 207
		private static readonly HttpClient _httpClient = new HttpClient();
	}
}
