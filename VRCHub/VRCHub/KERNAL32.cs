using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using MagmaMc.JEF;

namespace VRCHub
{
	// Token: 0x02000031 RID: 49
	[NullableContext(1)]
	[Nullable(0)]
	[SupportedOSPlatform("Windows")]
	public struct KERNAL32
	{
		// Token: 0x06000116 RID: 278
		[DllImport("KERNEL32.DLL", CallingConvention = 3, EntryPoint = "SetProcessWorkingSetSize", SetLastError = true)]
		public static extern bool SetProcessWorkingSetSize32Bit(IntPtr pProcess, int dwMinimumWorkingSetSize, int dwMaximumWorkingSetSize);

		// Token: 0x06000117 RID: 279
		[DllImport("KERNEL32.DLL", CallingConvention = 3, EntryPoint = "SetProcessWorkingSetSize", SetLastError = true)]
		public static extern bool SetProcessWorkingSetSize64Bit(IntPtr pProcess, long dwMinimumWorkingSetSize, long dwMaximumWorkingSetSize);

		// Token: 0x06000118 RID: 280
		[DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true)]
		public static extern IntPtr OpenProcess(int processAccess, bool bInheritHandle, int processId);

		// Token: 0x06000119 RID: 281
		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern bool CloseHandle(IntPtr hObject);

		// Token: 0x0600011A RID: 282
		[DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true)]
		public static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, uint flAllocationType, uint flProtect);

		// Token: 0x0600011B RID: 283
		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, uint nSize, out UIntPtr lpNumberOfBytesWritten);

		// Token: 0x0600011C RID: 284
		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern IntPtr CreateRemoteThread(IntPtr hProcess, IntPtr lpThreadAttributes, uint dwStackSize, IntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, out IntPtr lpThreadId);

		// Token: 0x0600011D RID: 285
		[DllImport("kernel32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
		public static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);

		// Token: 0x0600011E RID: 286
		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern IntPtr GetModuleHandle(string lpModuleName);

		// Token: 0x0600011F RID: 287
		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern IntPtr GetConsoleWindow();

		// Token: 0x06000120 RID: 288
		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern bool AllocConsole();

		// Token: 0x06000121 RID: 289
		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern bool FreeConsole();

		// Token: 0x06000122 RID: 290
		[DllImport("user32.dll")]
		public static extern bool SetForegroundWindow(IntPtr hWnd);

		// Token: 0x06000123 RID: 291
		[DllImport("user32.dll", SetLastError = true)]
		public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

		// Token: 0x06000124 RID: 292
		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

		// Token: 0x06000125 RID: 293
		[DllImport("user32.dll")]
		private static extern bool SystemParametersInfo(uint uiAction, uint uiParam, out KERNAL32.RECT pvParam, uint fWinIni);

		// Token: 0x06000126 RID: 294
		[DllImport("user32.dll")]
		public static extern IntPtr GetDesktopWindow();

		// Token: 0x06000127 RID: 295
		[DllImport("user32.dll")]
		public static extern bool SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

		// Token: 0x06000128 RID: 296
		[DllImport("user32.dll")]
		public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

		// Token: 0x06000129 RID: 297
		[DllImport("user32.dll")]
		public static extern bool UpdateWindow(IntPtr hWnd);

		// Token: 0x0600012A RID: 298
		[DllImport("user32.dll")]
		public static extern bool SetLayeredWindowAttributes(IntPtr hWnd, uint crKey, byte bAlpha, uint dwFlags);

		// Token: 0x0600012B RID: 299
		[DllImport("user32.dll")]
		public static extern bool GetWindowRect(IntPtr hWnd, out KERNAL32.RECT rect);

		// Token: 0x0600012C RID: 300 RVA: 0x004F1400 File Offset: 0x004F1400
		public static KERNAL32.RECT GetScrenBounds()
		{
			KERNAL32.RECT result;
			KERNAL32.SystemParametersInfo(48U, 0U, out result, 0U);
			return result;
		}

		// Token: 0x0600012D RID: 301 RVA: 0x004F141C File Offset: 0x004F141C
		public static void SetWindowTransparency(IntPtr hWnd, byte transparency)
		{
			int windowLong = KERNAL32.GetWindowLong(hWnd, -20);
			KERNAL32.SetWindowLong(hWnd, -20, windowLong | 524288 | 8);
			KERNAL32.SetLayeredWindowAttributes(hWnd, 0U, transparency, 2U);
		}

		// Token: 0x0600012E RID: 302 RVA: 0x004F1450 File Offset: 0x004F1450
		public static string Powershell(string command)
		{
			string result;
			using (Process process = Process.Start(new ProcessStartInfo
			{
				FileName = "powershell.exe",
				Arguments = "-NoProfile -Command \"" + command + "\"",
				RedirectStandardOutput = true,
				RedirectStandardError = true,
				UseShellExecute = false,
				CreateNoWindow = true,
				WindowStyle = ProcessWindowStyle.Hidden
			}))
			{
				using (StreamReader standardOutput = process.StandardOutput)
				{
					result = standardOutput.ReadToEnd();
				}
			}
			return result;
		}

		// Token: 0x0600012F RID: 303 RVA: 0x004F14F0 File Offset: 0x004F14F0
		public static bool RequireAdministrator(bool required, string args = "")
		{
			try
			{
				if (!JEF.Administrator.IsElevated() && required)
				{
					ProcessStartInfo processStartInfo = new ProcessStartInfo(AppDomain.CurrentDomain.FriendlyName);
					processStartInfo.Verb = "runas";
					processStartInfo.UseShellExecute = true;
					processStartInfo.Arguments = args;
					processStartInfo.WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory;
					Console.WriteLine(args);
					Process.Start(processStartInfo);
					Environment.Exit(2);
					return true;
				}
				if (!required)
				{
					return true;
				}
			}
			catch
			{
			}
			return false;
		}

		// Token: 0x040000F2 RID: 242
		public const int PROCESS_CREATE_THREAD = 2;

		// Token: 0x040000F3 RID: 243
		public const int PROCESS_QUERY_INFORMATION = 1024;

		// Token: 0x040000F4 RID: 244
		public const int PROCESS_VM_OPERATION = 8;

		// Token: 0x040000F5 RID: 245
		public const int PROCESS_VM_WRITE = 32;

		// Token: 0x040000F6 RID: 246
		public const int PROCESS_VM_READ = 16;

		// Token: 0x040000F7 RID: 247
		public const uint MEM_COMMIT = 4096U;

		// Token: 0x040000F8 RID: 248
		public const uint MEM_RESERVE = 8192U;

		// Token: 0x040000F9 RID: 249
		public const uint PAGE_READWRITE = 4U;

		// Token: 0x040000FA RID: 250
		public static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);

		// Token: 0x040000FB RID: 251
		public const uint SWP_NOMOVE = 2U;

		// Token: 0x040000FC RID: 252
		public const uint SWP_NOSIZE = 1U;

		// Token: 0x040000FD RID: 253
		public const uint SWP_NOACTIVATE = 16U;

		// Token: 0x040000FE RID: 254
		public const int GWL_EXSTYLE = -20;

		// Token: 0x040000FF RID: 255
		public const int WS_EX_LAYERED = 524288;

		// Token: 0x04000100 RID: 256
		public const int WS_EX_TOPMOST = 8;

		// Token: 0x04000101 RID: 257
		public const uint LWA_COLORKEY = 1U;

		// Token: 0x04000102 RID: 258
		public const uint LWA_ALPHA = 2U;

		// Token: 0x02000032 RID: 50
		[NullableContext(0)]
		public struct RECT
		{
			// Token: 0x04000103 RID: 259
			public int Left;

			// Token: 0x04000104 RID: 260
			public int Top;

			// Token: 0x04000105 RID: 261
			public int Right;

			// Token: 0x04000106 RID: 262
			public int Bottom;
		}
	}
}
