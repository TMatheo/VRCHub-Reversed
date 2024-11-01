using System;
using System.Security.Cryptography;
using System.Text;

namespace ZER0.Core.Cryptography
{
	/// <summary>
	/// Custom hash algorithm that combines SHA-512 and SHA-256 of bit-shifted MD5.
	/// </summary>
	// Token: 0x02000009 RID: 9
	public class ZLHash : IDisposable
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="T:ZER0.Core.Cryptography.ZLHash" /> class.
		/// </summary>
		// Token: 0x06000032 RID: 50 RVA: 0x0000553C File Offset: 0x0000553C
		protected ZLHash()
		{
			this._md5 = MD5.Create();
			this._sha256 = SHA256.Create();
			this._sha512 = SHA512.Create();
		}

		/// <summary>
		/// Creates a new instance of the <see cref="T:ZER0.Core.Cryptography.ZLHash" /> class.
		/// </summary>
		/// <returns>A new instance of the <see cref="T:ZER0.Core.Cryptography.ZLHash" /> class.</returns>
		// Token: 0x06000033 RID: 51 RVA: 0x00005568 File Offset: 0x00005568
		public static ZLHash Create()
		{
			return new ZLHash();
		}

		/// <summary>
		/// Computes the custom ZLHash for the specified input string.
		/// </summary>
		/// <param name="input">The input string to hash.</param>
		/// <returns>The combined hash as a byte array.</returns>
		// Token: 0x06000034 RID: 52 RVA: 0x00005570 File Offset: 0x00005570
		public byte[] ComputeHash(string input)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(input);
			return this.ComputeHash(bytes);
		}

		/// <summary>
		/// Computes the custom ZLHash for the specified input bytes.
		/// </summary>
		/// <param name="input">The input byte array to hash.</param>
		/// <returns>The combined hash as a byte array.</returns>
		// Token: 0x06000035 RID: 53 RVA: 0x00005590 File Offset: 0x00005590
		public byte[] ComputeHash(byte[] input)
		{
			byte[] array = this.ComputeSHA512(input);
			byte[] array2 = this.ComputeSHA256FromMD5WithBitShift(input);
			byte[] array3 = new byte[array.Length + array2.Length];
			Buffer.BlockCopy(array, 0, array3, 0, array.Length);
			Buffer.BlockCopy(array2, 0, array3, array.Length, array2.Length);
			return array3;
		}

		// Token: 0x06000036 RID: 54 RVA: 0x000055D8 File Offset: 0x000055D8
		private byte[] ComputeSHA512(byte[] data)
		{
			return this._sha512.ComputeHash(data);
		}

		// Token: 0x06000037 RID: 55 RVA: 0x000055E8 File Offset: 0x000055E8
		private byte[] ComputeSHA256FromMD5WithBitShift(byte[] data)
		{
			byte[] data2 = this._md5.ComputeHash(data);
			byte[] buffer = this.BitShiftRight(data2, 2);
			return this._sha256.ComputeHash(buffer);
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00005618 File Offset: 0x00005618
		private byte[] BitShiftRight(byte[] data, int bits)
		{
			int num = bits / 8;
			int num2 = bits % 8;
			int num3 = data.Length;
			byte[] array = new byte[num3];
			for (int i = 0; i < num3; i++)
			{
				if (i - num >= 0)
				{
					array[i] = (byte)(data[i - num] >> num2 | ((i - num - 1 >= 0) ? ((int)data[i - num - 1] << 8 - num2) : 0));
				}
			}
			return array;
		}

		/// <summary>
		/// Releases the resources used by the <see cref="T:ZER0.Core.Cryptography.ZLHash" /> class.
		/// </summary>
		// Token: 0x06000039 RID: 57 RVA: 0x0000567C File Offset: 0x0000567C
		public void Dispose()
		{
			this._md5.Dispose();
			this._sha256.Dispose();
			this._sha512.Dispose();
			GC.SuppressFinalize(this);
		}

		// Token: 0x0400000E RID: 14
		private readonly MD5 _md5;

		// Token: 0x0400000F RID: 15
		private readonly SHA256 _sha256;

		// Token: 0x04000010 RID: 16
		private readonly SHA512 _sha512;
	}
}
