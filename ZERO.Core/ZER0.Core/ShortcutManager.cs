using System;
using System.Diagnostics;
using System.IO;

namespace ZER0.Core
{
	/// <summary>
	/// Provides methods for creating, deleting, checking existence, and retrieving the target path of Windows shortcuts (.lnk files).
	/// </summary>
	// Token: 0x02000004 RID: 4
	public static class ShortcutManager
	{
		/// <summary>
		/// Executes a PowerShell command.
		/// </summary>
		/// <param name="command">The PowerShell command to execute.</param>
		// Token: 0x06000008 RID: 8 RVA: 0x00004BC0 File Offset: 0x00004BC0
		private static string RunPowerShellCommand(string command)
		{
			string result = string.Empty;
			try
			{
				using (Process process = Process.Start(new ProcessStartInfo
				{
					FileName = "powershell.exe",
					Arguments = "-NoProfile -ExecutionPolicy Bypass -Command \"" + command + "\"",
					RedirectStandardOutput = true,
					RedirectStandardError = true,
					UseShellExecute = false,
					CreateNoWindow = true
				}))
				{
					result = process.StandardOutput.ReadToEnd();
					string arg = process.StandardError.ReadToEnd();
					process.WaitForExit();
					if (process.ExitCode != 0)
					{
						throw new Exception(string.Format("PowerShell command failed with exit code {0}: {1}", process.ExitCode, arg));
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("Failed to execute PowerShell command: " + ex.Message);
			}
			return result;
		}

		/// <summary>
		/// Creates a Windows shortcut (.lnk file) at the specified path.
		/// </summary>
		/// <param name="shortcutPath">The full path where the shortcut will be created, For example, "C:\\path\\to\\shortcut".</param>
		/// <param name="targetPath">The path to the target executable or file the shortcut will point to. For example, "C:\\path\\to\\target.exe".</param>
		/// <param name="workingDirectory">The working directory for the shortcut. This can be an empty string if no specific working directory is needed.</param>
		/// <param name="arguments">Any arguments to pass to the target application. This can be an empty string if no arguments are needed.</param>
		/// <param name="iconPath">The path to the icon to use for the shortcut, including the icon index. This can be an empty string if the default icon should be used.</param>
		/// <param name="description">A description for the shortcut. This can be an empty string if no description is needed.</param>
		// Token: 0x06000009 RID: 9 RVA: 0x00004CA0 File Offset: 0x00004CA0
		public static void CreateShortcut(string shortcutPath, string targetPath, string workingDirectory = null, string arguments = null, string iconPath = null, string description = null)
		{
			if (!shortcutPath.EndsWith(".lnk", StringComparison.OrdinalIgnoreCase))
			{
				shortcutPath += ".lnk";
			}
			ShortcutManager.RunPowerShellCommand(string.Concat(new string[]
			{
				"\r\n$wshell = New-Object -ComObject WScript.Shell;\r\n$shortcut = $wshell.CreateShortcut('",
				ShortcutManager.EscapePath(shortcutPath),
				"');\r\n$shortcut.TargetPath = '",
				ShortcutManager.EscapePath(Path.GetFullPath(targetPath)),
				"';\r\n$shortcut.Description = '",
				ShortcutManager.EscapeString(description),
				"';\r\n$shortcut.Arguments = '",
				ShortcutManager.EscapeString(arguments),
				"';\r\nif ('",
				ShortcutManager.EscapePath(iconPath),
				"' -ne '') {\r\n    $shortcut.IconLocation = '",
				ShortcutManager.EscapePath(iconPath),
				"';\r\n}\r\nif ('",
				ShortcutManager.EscapePath(workingDirectory),
				"' -ne '') {\r\n    $shortcut.WorkingDirectory = '",
				ShortcutManager.EscapePath(workingDirectory),
				"';\r\n}\r\n$shortcut.Save();\r\n"
			}));
		}

		/// <summary>
		/// Deletes a shortcut at the specified path.
		/// </summary>
		/// <param name="shortcutPath">The full path of the shortcut to delete.</param>
		/// <exception cref="T:System.IO.FileNotFoundException"></exception>
		// Token: 0x0600000A RID: 10 RVA: 0x00004D7C File Offset: 0x00004D7C
		public static void DeleteShortcut(string shortcutPath)
		{
			if (File.Exists(shortcutPath) && Path.GetExtension(shortcutPath).Equals(".lnk", StringComparison.OrdinalIgnoreCase))
			{
				File.Delete(shortcutPath);
				return;
			}
			throw new FileNotFoundException("Shortcut not found.", shortcutPath);
		}

		/// <summary>
		/// Checks if a shortcut exists at the specified path.
		/// </summary>
		/// <param name="shortcutPath">The full path of the shortcut to check.</param>
		/// <returns>True if the shortcut exists, otherwise false.</returns>
		// Token: 0x0600000B RID: 11 RVA: 0x00004DAC File Offset: 0x00004DAC
		public static bool ShortcutExists(string shortcutPath)
		{
			return File.Exists(shortcutPath) && Path.GetExtension(shortcutPath).Equals(".lnk", StringComparison.OrdinalIgnoreCase);
		}

		/// <summary>
		/// Gets the target path of a Windows shortcut (.lnk file).
		/// </summary>
		/// <param name="shortcutPath">The full path of the shortcut file.</param>
		/// <returns>The target path of the shortcut if valid; otherwise, null.</returns>
		/// <exception cref="T:System.IO.FileNotFoundException">Thrown when the shortcut is not found.</exception>
		// Token: 0x0600000C RID: 12 RVA: 0x00004DCC File Offset: 0x00004DCC
		public static string GetPath(string shortcutPath)
		{
			string text = ShortcutManager.EscapePath(shortcutPath);
			string str = string.Concat(new string[]
			{
				"\r\nif (Test-Path '",
				text,
				"') {\r\n    if ([System.IO.Path]::GetExtension('",
				text,
				"').Equals('.lnk', [StringComparison]::OrdinalIgnoreCase)) {\r\n        $wshell = New-Object -ComObject WScript.Shell\r\n        $shortcut = $wshell.CreateShortcut('",
				text,
				"')\r\n        echo $shortcut.TargetPath\r\n    } else {\r\n        throw 'The file is not a .lnk shortcut: ",
				text,
				"'\r\n    }\r\n} else {\r\n    throw 'Shortcut not found: ",
				text,
				"'\r\n}\r\n"
			});
			string result;
			try
			{
				using (Process process = Process.Start(new ProcessStartInfo
				{
					FileName = "powershell.exe",
					Arguments = "-NoProfile -ExecutionPolicy Bypass -Command \"" + str + "\"",
					RedirectStandardOutput = true,
					RedirectStandardError = true,
					UseShellExecute = false,
					CreateNoWindow = true
				}))
				{
					string text2 = process.StandardOutput.ReadToEnd();
					string str2 = process.StandardError.ReadToEnd();
					process.WaitForExit();
					if (process.ExitCode != 0)
					{
						Console.WriteLine("Failed to execute PowerShell command: " + str2);
						result = null;
					}
					else
					{
						result = text2.Trim();
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("Failed to get shortcut target path: " + ex.Message);
				result = null;
			}
			return result;
		}

		/// <summary>
		/// Escapes single quotes in a path to prevent issues with PowerShell command parsing.
		/// </summary>
		/// <param name="path">The path to escape.</param>
		/// <returns>The escaped path.</returns>
		// Token: 0x0600000D RID: 13 RVA: 0x00004F08 File Offset: 0x00004F08
		private static string EscapePath(string path)
		{
			if (!string.IsNullOrEmpty(path))
			{
				return path.Replace("'", "''");
			}
			return "";
		}

		/// <summary>
		/// Escapes single quotes in a string to prevent issues with PowerShell command parsing.
		/// </summary>
		/// <param name="str">The string to escape.</param>
		/// <returns>The escaped string.</returns>
		// Token: 0x0600000E RID: 14 RVA: 0x00004F28 File Offset: 0x00004F28
		private static string EscapeString(string str)
		{
			if (!string.IsNullOrEmpty(str))
			{
				return str.Replace("'", "''");
			}
			return "";
		}
	}
}
