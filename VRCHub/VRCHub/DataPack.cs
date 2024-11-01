using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Windows;
using Newtonsoft.Json;
using VRCHub.Models;

namespace VRCHub
{
	// Token: 0x02000009 RID: 9
	[NullableContext(1)]
	[Nullable(0)]
	public class DataPack
	{
		// Token: 0x06000034 RID: 52 RVA: 0x004EBE94 File Offset: 0x004EBE94
		public DataPack(byte[] packData)
		{
			this._packData = packData;
		}

		// Token: 0x06000035 RID: 53 RVA: 0x004EBEA4 File Offset: 0x004EBEA4
		public static bool ValidPack(byte[] Pack)
		{
			bool result;
			try
			{
				using (MemoryStream memoryStream = new MemoryStream(Pack))
				{
					using (ZipArchive zipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Read))
					{
						bool flag = false;
						bool flag2 = false;
						foreach (ZipArchiveEntry zipArchiveEntry in zipArchive.Entries)
						{
							if (zipArchiveEntry.FullName == "package.json")
							{
								flag = true;
							}
							if (zipArchiveEntry.FullName == "__data")
							{
								flag2 = true;
							}
						}
						result = (flag && flag2);
					}
				}
			}
			catch
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06000036 RID: 54 RVA: 0x004EBF70 File Offset: 0x004EBF70
		public byte[] GetDataBytes()
		{
			using (MemoryStream memoryStream = new MemoryStream(this._packData))
			{
				using (ZipArchive zipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Read))
				{
					ZipArchiveEntry entry = zipArchive.GetEntry("__data");
					if (entry != null)
					{
						using (Stream stream = entry.Open())
						{
							using (MemoryStream memoryStream2 = new MemoryStream())
							{
								stream.CopyTo(memoryStream2);
								return memoryStream2.ToArray();
							}
						}
					}
					throw new Exception("__data entry not found in the ZIP archive.");
				}
			}
			byte[] result;
			return result;
		}

		// Token: 0x06000037 RID: 55 RVA: 0x004EC02C File Offset: 0x004EC02C
		public DataPackage GetDataPackage()
		{
			using (MemoryStream memoryStream = new MemoryStream(this._packData))
			{
				using (ZipArchive zipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Read))
				{
					ZipArchiveEntry entry = zipArchive.GetEntry("package.json");
					if (entry != null)
					{
						using (Stream stream = entry.Open())
						{
							using (StreamReader streamReader = new StreamReader(stream))
							{
								return JsonConvert.DeserializeObject<DataPackage>(streamReader.ReadToEnd());
							}
						}
					}
					throw new Exception("package.json entry not found in the ZIP archive.");
				}
			}
			DataPackage result;
			return result;
		}

		// Token: 0x06000038 RID: 56 RVA: 0x004EC0E8 File Offset: 0x004EC0E8
		public bool Install()
		{
			try
			{
				string contentCachePath = VRCConfig.GetContentCachePath();
				using (HttpClient httpClient = new HttpClient())
				{
					httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:91.0) Gecko/20100101 Firefox/91.0");
					DataPackage dataPackage = this.GetDataPackage();
					FileSystemInfo fileSystemInfo = (from d in new DirectoryInfo(Path.Combine(contentCachePath, dataPackage.WorldHash)).GetDirectories()
					orderby d.LastWriteTime descending
					select d).FirstOrDefault<DirectoryInfo>();
					byte[] dataBytes = this.GetDataBytes();
					File.WriteAllBytes(Path.Combine(fileSystemInfo.FullName, "__data"), dataBytes);
					return true;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}
			return false;
		}

		// Token: 0x06000039 RID: 57 RVA: 0x004EC1B4 File Offset: 0x004EC1B4
		public bool Uninstall()
		{
			try
			{
				string contentCachePath = VRCConfig.GetContentCachePath();
				DataPackage dataPackage = this.GetDataPackage();
				Directory.Delete(Path.Combine(contentCachePath, dataPackage.WorldHash), true);
				return true;
			}
			catch (Exception ex)
			{
				MessageBox.Show("Uninstallation Error", ex.Message);
			}
			return false;
		}

		// Token: 0x0400001A RID: 26
		private readonly byte[] _packData;
	}
}
