using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Win32;

namespace VRCHub
{
	// Token: 0x0200002F RID: 47
	[NullableContext(1)]
	[Nullable(0)]
	public static class SplashscreenEditor
	{
		// Token: 0x060000F7 RID: 247 RVA: 0x004F0D4C File Offset: 0x004F0D4C
		[NullableContext(2)]
		public static BitmapImage SelectImageFromExplorer()
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "Image files (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg";
			if (openFileDialog.ShowDialog().GetValueOrDefault())
			{
				BitmapImage bitmapImage = new BitmapImage();
				bitmapImage.BeginInit();
				bitmapImage.UriSource = new Uri(openFileDialog.FileName);
				bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
				bitmapImage.EndInit();
				return bitmapImage;
			}
			return null;
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x004F0DA8 File Offset: 0x004F0DA8
		public static void ScaleImage(BitmapImage image, out BitmapImage imageout)
		{
			int num = 800;
			int num2 = 450;
			BitmapSource source = new TransformedBitmap(image, new ScaleTransform((double)num / (double)image.PixelWidth, (double)num2 / (double)image.PixelHeight));
			PngBitmapEncoder pngBitmapEncoder = new PngBitmapEncoder();
			pngBitmapEncoder.Frames.Add(BitmapFrame.Create(source));
			using (MemoryStream memoryStream = new MemoryStream())
			{
				pngBitmapEncoder.Save(memoryStream);
				memoryStream.Seek(0L, SeekOrigin.Begin);
				BitmapImage bitmapImage = new BitmapImage();
				bitmapImage.BeginInit();
				bitmapImage.StreamSource = memoryStream;
				bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
				bitmapImage.EndInit();
				imageout = bitmapImage;
			}
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x004F0E5C File Offset: 0x004F0E5C
		public static BitmapImage ScaleImage(BitmapImage image)
		{
			BitmapImage result;
			SplashscreenEditor.ScaleImage(image, out result);
			return result;
		}

		// Token: 0x060000FA RID: 250 RVA: 0x004F0E74 File Offset: 0x004F0E74
		public static bool SaveImage(BitmapImage image)
		{
			bool result;
			try
			{
				PngBitmapEncoder pngBitmapEncoder = new PngBitmapEncoder();
				pngBitmapEncoder.Frames.Add(BitmapFrame.Create(image));
				using (FileStream fileStream = new FileStream(SplashscreenEditor.SplashScreenPath, FileMode.Create))
				{
					pngBitmapEncoder.Save(fileStream);
					result = true;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error saving image: " + ex.Message);
				result = false;
			}
			return result;
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x060000FB RID: 251 RVA: 0x004F0EF0 File Offset: 0x004F0EF0
		public static string SplashScreenPath
		{
			get
			{
				return Path.Combine(new FileInfo(Config.VRC_Path).Directory.FullName, "EasyAntiCheat\\SplashScreen.png");
			}
		}
	}
}
