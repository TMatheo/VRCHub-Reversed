using System;
using System.IO;
using System.Text;
using ZER0.Core;

namespace _Test
{
	// Token: 0x02000002 RID: 2
	internal class Net472_Test
	{
		// Token: 0x06000001 RID: 1 RVA: 0x000049CC File Offset: 0x000049CC
		internal static void Main(string[] args)
		{
			Console.OutputEncoding = Encoding.UTF8;
			Console.WriteLine("Username Hash: " + KeyStore.GetUserHash());
			Console.WriteLine();
			Console.ForegroundColor = ConsoleColor.Red;
			ShortcutManager.CreateShortcut("example.lnk", AppDomain.CurrentDomain.FriendlyName + ".exe", Directory.GetCurrentDirectory(), null, null, null);
			Console.ResetColor();
			Console.WriteLine("Shortcut Target: " + ShortcutManager.GetPath("example.lnk"));
		}
	}
}
