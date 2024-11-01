using System;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using ZER0.Core.Models;

namespace ZER0.Core
{
	/// <summary>
	///
	/// </summary>
	// Token: 0x02000005 RID: 5
	public class ZER0Key : IDisposable
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="T:ZER0.Core.ZER0Key" /> class.
		/// </summary>
		// Token: 0x0600000F RID: 15 RVA: 0x00004F48 File Offset: 0x00004F48
		public ZER0Key()
		{
			this._client = new HttpClient();
			this._client.BaseAddress = new Uri("https://api.vrchub.site/API/1/");
			this._client.Timeout = TimeSpan.FromSeconds(10.0);
			this._client.DefaultRequestHeaders.Add("UserAgent", "ZER0Key-Library-3.1.0");
		}

		/// <summary>
		/// Synchronously requests an HWID reset using the provided email, key, and old HWID.
		/// </summary>
		/// <param name="email">The email associated with the account.</param>
		/// <param name="key">The key for the HWID reset request.</param>
		/// <param name="Old">The old HWID that needs to be reset.</param>
		/// <returns>Returns true if the request was successful, otherwise false.</returns>
		// Token: 0x06000010 RID: 16 RVA: 0x00004FB0 File Offset: 0x00004FB0
		public bool RequestHWIDReset(string email, string key, string Old)
		{
			return this.RequestHWIDResetAsync(email, key, Old).GetAwaiter().GetResult();
		}

		/// <summary>
		/// Synchronously requests an HWID reset using a KeyContent object and old HWID.
		/// </summary>
		/// <param name="content">The KeyContent object containing the email and key.</param>
		/// <param name="Old">The old HWID that needs to be reset.</param>
		/// <returns>Returns true if the request was successful, otherwise false.</returns>
		// Token: 0x06000011 RID: 17 RVA: 0x00004FD4 File Offset: 0x00004FD4
		public bool RequestHWIDReset(KeyContent content, string Old)
		{
			return this.RequestHWIDResetAsync(content.Email, content.Key, Old).GetAwaiter().GetResult();
		}

		/// <summary>
		/// Asynchronously requests an HWID reset using a KeyContent object and old HWID.
		/// </summary>
		/// <param name="content">The KeyContent object containing the email and key.</param>
		/// <param name="Old">The old HWID that needs to be reset.</param>
		/// <returns>A task that represents the asynchronous operation, containing a boolean result.</returns>
		// Token: 0x06000012 RID: 18 RVA: 0x00005004 File Offset: 0x00005004
		public Task<bool> RequestHWIDResetAsync(KeyContent content, string Old)
		{
			ZER0Key.<RequestHWIDResetAsync>d__5 <RequestHWIDResetAsync>d__;
			<RequestHWIDResetAsync>d__.<>t__builder = AsyncTaskMethodBuilder<bool>.Create();
			<RequestHWIDResetAsync>d__.<>4__this = this;
			<RequestHWIDResetAsync>d__.content = content;
			<RequestHWIDResetAsync>d__.Old = Old;
			<RequestHWIDResetAsync>d__.<>1__state = -1;
			<RequestHWIDResetAsync>d__.<>t__builder.Start<ZER0Key.<RequestHWIDResetAsync>d__5>(ref <RequestHWIDResetAsync>d__);
			return <RequestHWIDResetAsync>d__.<>t__builder.Task;
		}

		/// <summary>
		/// Asynchronously requests an HWID reset using the provided email, key, and old HWID.
		/// </summary>
		/// <param name="email">The email associated with the account.</param>
		/// <param name="key">The key for the HWID reset request.</param>
		/// <param name="Old">The old HWID that needs to be reset.</param>
		/// <returns>A task that represents the asynchronous operation, containing a boolean result.</returns>
		// Token: 0x06000013 RID: 19 RVA: 0x00005058 File Offset: 0x00005058
		public Task<bool> RequestHWIDResetAsync(string email, string key, string Old)
		{
			ZER0Key.<RequestHWIDResetAsync>d__6 <RequestHWIDResetAsync>d__;
			<RequestHWIDResetAsync>d__.<>t__builder = AsyncTaskMethodBuilder<bool>.Create();
			<RequestHWIDResetAsync>d__.<>4__this = this;
			<RequestHWIDResetAsync>d__.email = email;
			<RequestHWIDResetAsync>d__.key = key;
			<RequestHWIDResetAsync>d__.Old = Old;
			<RequestHWIDResetAsync>d__.<>1__state = -1;
			<RequestHWIDResetAsync>d__.<>t__builder.Start<ZER0Key.<RequestHWIDResetAsync>d__6>(ref <RequestHWIDResetAsync>d__);
			return <RequestHWIDResetAsync>d__.<>t__builder.Task;
		}

		/// <summary>
		/// Authenticates the user synchronously.
		/// </summary>
		/// <param name="content">The Key Content To Authorize With</param>
		/// <returns>A <see cref="T:ZER0.Core.Models.KeyStatus" /> object representing the authentication status.</returns>
		/// <exception cref="T:System.ObjectDisposedException">Thrown if the object has been disposed.</exception>
		// Token: 0x06000014 RID: 20 RVA: 0x000050B4 File Offset: 0x000050B4
		public KeyStatus Authenticate(KeyContent content)
		{
			return this.AuthenticateAsync(content.Email, content.Key).GetAwaiter().GetResult();
		}

		/// <summary>
		/// Authenticates the user synchronously.
		/// </summary>
		/// <param name="email">The email of the user.</param>
		/// <param name="key">The key associated with the user.</param>
		/// <returns>A <see cref="T:ZER0.Core.Models.KeyStatus" /> object representing the authentication status.</returns>
		/// <exception cref="T:System.ObjectDisposedException">Thrown if the object has been disposed.</exception>
		// Token: 0x06000015 RID: 21 RVA: 0x000050E0 File Offset: 0x000050E0
		public KeyStatus Authenticate(string email, string key)
		{
			return this.AuthenticateAsync(email, key).GetAwaiter().GetResult();
		}

		/// <summary>
		/// Authenticates the user asynchronously.
		/// </summary>
		/// <param name="email">The email of the user.</param>
		/// <param name="key">The key associated with the user.</param>
		/// <returns>A <see cref="T:ZER0.Core.Models.KeyStatus" /> object representing the authentication status.</returns>
		/// <exception cref="T:System.ObjectDisposedException">Thrown if the object has been disposed.</exception>
		// Token: 0x06000016 RID: 22 RVA: 0x00005104 File Offset: 0x00005104
		public Task<KeyStatus> AuthenticateAsync(string email, string key)
		{
			ZER0Key.<AuthenticateAsync>d__9 <AuthenticateAsync>d__;
			<AuthenticateAsync>d__.<>t__builder = AsyncTaskMethodBuilder<KeyStatus>.Create();
			<AuthenticateAsync>d__.<>4__this = this;
			<AuthenticateAsync>d__.email = email;
			<AuthenticateAsync>d__.key = key;
			<AuthenticateAsync>d__.<>1__state = -1;
			<AuthenticateAsync>d__.<>t__builder.Start<ZER0Key.<AuthenticateAsync>d__9>(ref <AuthenticateAsync>d__);
			return <AuthenticateAsync>d__.<>t__builder.Task;
		}

		/// <summary>
		/// Authenticates the user asynchronously.
		/// </summary>
		/// <param name="content">The Key Content To Authorize With</param>
		/// <returns>A <see cref="T:ZER0.Core.Models.KeyStatus" /> object representing the authentication status.</returns>
		/// <exception cref="T:System.ObjectDisposedException">Thrown if the object has been disposed.</exception>
		// Token: 0x06000017 RID: 23 RVA: 0x00005158 File Offset: 0x00005158
		public Task<KeyStatus> AuthenticateAsync(KeyContent content)
		{
			ZER0Key.<AuthenticateAsync>d__10 <AuthenticateAsync>d__;
			<AuthenticateAsync>d__.<>t__builder = AsyncTaskMethodBuilder<KeyStatus>.Create();
			<AuthenticateAsync>d__.<>4__this = this;
			<AuthenticateAsync>d__.content = content;
			<AuthenticateAsync>d__.<>1__state = -1;
			<AuthenticateAsync>d__.<>t__builder.Start<ZER0Key.<AuthenticateAsync>d__10>(ref <AuthenticateAsync>d__);
			return <AuthenticateAsync>d__.<>t__builder.Task;
		}

		/// <summary>
		/// Gets a value indicating whether the object has been disposed.
		/// </summary>
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000018 RID: 24 RVA: 0x000051A4 File Offset: 0x000051A4
		public bool Disposed
		{
			get
			{
				return this.disposedValue;
			}
		}

		/// <summary>
		/// Releases the unmanaged resources used by the <see cref="T:ZER0.Core.ZER0Key" /> and optionally releases the managed resources.
		/// </summary>
		/// <param name="disposing">If true, releases both managed and unmanaged resources; otherwise, releases only unmanaged resources.</param>
		// Token: 0x06000019 RID: 25 RVA: 0x000051AC File Offset: 0x000051AC
		protected virtual void Dispose(bool disposing)
		{
			if (!this.disposedValue)
			{
				if (disposing)
				{
					this._client.Dispose();
				}
				this.disposedValue = true;
			}
		}

		/// <summary>
		/// Releases all resources used by the current instance of the <see cref="T:ZER0.Core.ZER0Key" /> class.
		/// </summary>
		// Token: 0x0600001A RID: 26 RVA: 0x000051CC File Offset: 0x000051CC
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Generates a unique computer footprint to identify a user.
		/// <exception cref="T:System.InvalidOperationException">Thrown when unable to retrieve necessary registry values or identity information.</exception>
		/// </summary>
		/// <returns>A string representing the hardware ID (HWID).</returns>
		// Token: 0x0600001B RID: 27 RVA: 0x000051DC File Offset: 0x000051DC
		public static string ComputerFootprint()
		{
			string registryValue = ZER0Key.GetRegistryValue("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion", "ProductId");
			string name = WindowsIdentity.GetCurrent().Name;
			if (string.IsNullOrEmpty(registryValue) || string.IsNullOrEmpty(name))
			{
				throw new InvalidOperationException("Failed to retrieve system identifiers.");
			}
			string s = string.Concat(new string[]
			{
				"[",
				registryValue,
				"]-[",
				name,
				"]"
			});
			SHA256 sha = SHA256.Create();
			byte[] value = sha.ComputeHash(Encoding.UTF8.GetBytes(s));
			sha.Dispose();
			return BitConverter.ToString(value).Replace("-", "");
		}

		// Token: 0x0600001C RID: 28 RVA: 0x0000527C File Offset: 0x0000527C
		private static string GetRegistryValue(string keyPath, string valueName)
		{
			object value = Registry.GetValue("HKEY_LOCAL_MACHINE\\" + keyPath, valueName, null);
			string text = (value != null) ? value.ToString() : null;
			if (text == null)
			{
				throw new InvalidOperationException(string.Concat(new string[]
				{
					"Registry key '",
					keyPath,
					"\\",
					valueName,
					"' not found."
				}));
			}
			return text;
		}

		/// <summary>
		/// Indicates whether the user is authenticated.
		/// </summary>
		// Token: 0x04000002 RID: 2
		public bool IsAuthed;

		// Token: 0x04000003 RID: 3
		private readonly HttpClient _client;

		// Token: 0x04000004 RID: 4
		private bool disposedValue;
	}
}
