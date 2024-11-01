using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Media.Imaging;

namespace VRCHub
{
	// Token: 0x02000004 RID: 4
	[NullableContext(1)]
	[Nullable(0)]
	internal static class Common
	{
		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600000F RID: 15 RVA: 0x004EB808 File Offset: 0x004EB808
		public static byte[] NonCollisionBytes
		{
			get
			{
				Common.NoCollision += 1UL;
				if (Common.NoCollision == 18446744073709551615UL)
				{
					Common.NoCollision = 0UL;
				}
				return Encoding.ASCII.GetBytes(Common.random.Next(10000000, 99999999).ToString() + Common.NoCollision.ToString());
			}
		}

		// Token: 0x06000010 RID: 16 RVA: 0x004EB868 File Offset: 0x004EB868
		public static string MD5Hash()
		{
			byte[] array = Common.md5.ComputeHash(Common.NonCollisionBytes);
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < array.Length; i++)
			{
				stringBuilder.Append(array[i].ToString("X2"));
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000011 RID: 17 RVA: 0x004EB8B8 File Offset: 0x004EB8B8
		public static BitmapImage GetImageSource(byte[] imageBytes)
		{
			BitmapImage result;
			using (MemoryStream memoryStream = new MemoryStream(imageBytes))
			{
				BitmapImage bitmapImage = new BitmapImage();
				bitmapImage.BeginInit();
				bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
				bitmapImage.StreamSource = memoryStream;
				bitmapImage.EndInit();
				result = bitmapImage;
			}
			return result;
		}

		// Token: 0x06000012 RID: 18 RVA: 0x004EB90C File Offset: 0x004EB90C
		internal static void StartAnalytics()
		{
			if (File.Exists("EasyAnalytics.dll"))
			{
				string softwarePath = OptionalSoftwareManager.GetSoftwarePath("EasyAnalytics");
				try
				{
					File.Copy("libcurl.dll", Path.Combine(softwarePath, "libcurl.dll"), true);
					File.Copy("libssh2.dll", Path.Combine(softwarePath, "libssh2.dll"), true);
					File.Copy("libcrypto-3-x64.dll", Path.Combine(softwarePath, "libcrypto-3-x64.dll"), true);
					File.Copy("zlib1.dll", Path.Combine(softwarePath, "zlib1.dll"), true);
					File.Copy("jsoncpp.dll", Path.Combine(softwarePath, "jsoncpp.dll"), true);
					File.Copy("EasyAnalytics.dll", Path.Combine(softwarePath, "EasyAnalytics.dll"), true);
				}
				catch
				{
				}
				if (File.Exists(Path.Combine(softwarePath, "EasyAnalytics.dll")))
				{
					Process.Start(new ProcessStartInfo
					{
						FileName = "rundll32.exe",
						Arguments = "EasyAnalytics.dll,OpenAnalyticsPort",
						WorkingDirectory = softwarePath,
						UseShellExecute = true,
						WindowStyle = ProcessWindowStyle.Hidden,
						CreateNoWindow = true
					});
					return;
				}
			}
			else
			{
				Config.SendAnalytics = false;
			}
		}

		// Token: 0x04000003 RID: 3
		public static Version VERSION = new Version("0.5.0");

		// Token: 0x04000004 RID: 4
		private static readonly Random random = new Random();

		// Token: 0x04000005 RID: 5
		private static readonly MD5 md5 = MD5.Create();

		// Token: 0x04000006 RID: 6
		private static ulong NoCollision = 0UL;
	}
}
