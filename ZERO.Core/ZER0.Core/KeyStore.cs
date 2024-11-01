using System;
using System.IO;
using System.Text;
using MagmaMc.MagmaSimpleConfig.Utils;
using ZER0.Core.Cryptography;
using ZER0.Core.Models;

namespace ZER0.Core
{
	/// <summary>
	/// Quick System For Storing Keys
	/// </summary>
	// Token: 0x02000003 RID: 3
	public class KeyStore
	{
		/// <summary>
		/// Initilizer
		/// </summary>
		/// <param name="programName"></param>
		// Token: 0x06000003 RID: 3 RVA: 0x00004A50 File Offset: 0x00004A50
		public KeyStore(string programName)
		{
			this.ProgramName = programName;
		}

		/// <summary>
		///
		/// </summary>
		/// <returns></returns>
		// Token: 0x06000004 RID: 4 RVA: 0x00004A60 File Offset: 0x00004A60
		public KeyContent GetStoredKey()
		{
			KeyContent keyContent = new KeyContent();
			string filePath = this.GetFilePath();
			string userHash = KeyStore.GetUserHash();
			if (File.Exists(filePath))
			{
				try
				{
					byte[] rawData = File.ReadAllBytes(filePath);
					string[] array = Encoding.UTF8.GetString(AES.DecryptData(rawData, userHash)).Split('|', StringSplitOptions.None);
					if (array.Length != 0)
					{
						keyContent.Key = array[0];
						keyContent.Email = array[1];
					}
				}
				catch
				{
					File.Delete(filePath);
				}
			}
			return keyContent;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="key"></param>
		/// <param name="email"></param>
		/// <returns></returns>
		// Token: 0x06000005 RID: 5 RVA: 0x00004AE0 File Offset: 0x00004AE0
		public bool StoreKey(string key, string email)
		{
			bool result = false;
			string filePath = this.GetFilePath();
			string userHash = KeyStore.GetUserHash();
			try
			{
				Convert.FromBase64String(userHash);
				string text = key + "|" + email;
				File.WriteAllBytes(filePath, AES.EncryptData(Encoding.UTF8.GetBytes(text.ToString()), userHash));
				result = true;
			}
			catch
			{
			}
			return result;
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00004B44 File Offset: 0x00004B44
		internal static string GetUserHash()
		{
			string userName = Environment.UserName;
			string result;
			using (ZLHash zlhash = ZLHash.Create())
			{
				byte[] bytes = Encoding.UTF8.GetBytes(userName);
				result = Convert.ToBase64String(zlhash.ComputeHash(bytes));
			}
			return result;
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00004B94 File Offset: 0x00004B94
		private string GetFilePath()
		{
			string text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ZER0Team", this.ProgramName);
			Directory.CreateDirectory(text);
			return Path.Combine(text, "KeyStore.key");
		}

		/// <summary>
		/// Program Name For Directory
		/// </summary>
		// Token: 0x04000001 RID: 1
		public readonly string ProgramName;
	}
}
