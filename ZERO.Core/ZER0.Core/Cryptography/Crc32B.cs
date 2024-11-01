using System;
using System.IO;
using System.Text;

namespace ZER0.Core.Cryptography
{
	/// <summary>
	/// Provides functionality to compute CRC32B (CRC32/BZIP2) checksums.
	/// </summary>
	// Token: 0x02000008 RID: 8
	public class Crc32B : IDisposable
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="T:ZER0.Core.Cryptography.Crc32B" /> class.
		/// </summary>
		// Token: 0x06000029 RID: 41 RVA: 0x00005350 File Offset: 0x00005350
		public Crc32B()
		{
			this.InitializeTable();
		}

		/// <summary>
		/// Initializes the CRC32 table for fast computation.
		/// </summary>
		// Token: 0x0600002A RID: 42 RVA: 0x00005370 File Offset: 0x00005370
		private void InitializeTable()
		{
			for (uint num = 0U; num < 256U; num += 1U)
			{
				uint num2 = num;
				for (int i = 8; i > 0; i--)
				{
					num2 = (((num2 & 1U) == 1U) ? (num2 >> 1 ^ 3988292384U) : (num2 >> 1));
				}
				this.table[(int)num] = num2;
			}
		}

		/// <summary>
		/// Computes the CRC32B checksum for the specified file.
		/// </summary>
		/// <param name="filename">The path of the file to compute the checksum for.</param>
		/// <returns>The CRC32B checksum as a <see cref="T:System.UInt32" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">Thrown when the <paramref name="filename" /> is null.</exception>
		/// <exception cref="T:System.IO.FileNotFoundException">Thrown when the file does not exist.</exception>
		// Token: 0x0600002B RID: 43 RVA: 0x000053BC File Offset: 0x000053BC
		public uint ComputeFile(string filename)
		{
			if (filename == null)
			{
				throw new ArgumentNullException("filename");
			}
			if (!File.Exists(filename))
			{
				throw new FileNotFoundException("File not found.", filename);
			}
			uint result;
			using (FileStream fileStream = File.OpenRead(filename))
			{
				result = this.ComputeChecksum(fileStream);
			}
			return result;
		}

		/// <summary>
		/// Computes the CRC32B checksum for the specified string data.
		/// </summary>
		/// <param name="data">The string data to compute the checksum for.</param>
		/// <returns>The CRC32B checksum as a <see cref="T:System.UInt32" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">Thrown when the <paramref name="data" /> string is null.</exception>
		// Token: 0x0600002C RID: 44 RVA: 0x00005418 File Offset: 0x00005418
		public uint ComputeChecksum(string data)
		{
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			return this.ComputeChecksum(Encoding.UTF8.GetBytes(data));
		}

		/// <summary>
		/// Computes the CRC32B checksum for the specified byte array.
		/// </summary>
		/// <param name="data">The byte array to compute the checksum for.</param>
		/// <returns>The CRC32B checksum as a <see cref="T:System.UInt32" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">Thrown when the <paramref name="data" /> array is null.</exception>
		// Token: 0x0600002D RID: 45 RVA: 0x0000543C File Offset: 0x0000543C
		public uint ComputeChecksum(byte[] data)
		{
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			uint num = uint.MaxValue;
			foreach (byte b in data)
			{
				byte b2 = (byte)((num ^ (uint)b) & 255U);
				num = (num >> 8 ^ this.table[(int)b2]);
			}
			return num ^ uint.MaxValue;
		}

		/// <summary>
		/// Computes the CRC32B checksum for the specified stream.
		/// </summary>
		/// <param name="stream">The stream to compute the checksum for.</param>
		/// <returns>The CRC32B checksum as a <see cref="T:System.UInt32" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">Thrown when the <paramref name="stream" /> is null.</exception>
		// Token: 0x0600002E RID: 46 RVA: 0x0000548C File Offset: 0x0000548C
		public uint ComputeChecksum(Stream stream)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			uint num = uint.MaxValue;
			int num2;
			while ((num2 = stream.ReadByte()) != -1)
			{
				byte b = (byte)(((ulong)num ^ (ulong)((long)num2)) & 255UL);
				num = (num >> 8 ^ this.table[(int)b]);
			}
			return num ^ uint.MaxValue;
		}

		/// <summary>
		/// Releases all resources used by the <see cref="T:ZER0.Core.Cryptography.Crc32B" /> class.
		/// </summary>
		// Token: 0x0600002F RID: 47 RVA: 0x000054D4 File Offset: 0x000054D4
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Releases the unmanaged resources used by the <see cref="T:ZER0.Core.Cryptography.Crc32B" /> class
		/// and optionally releases the managed resources.
		/// </summary>
		/// <param name="disposing">True to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
		// Token: 0x06000030 RID: 48 RVA: 0x000054E4 File Offset: 0x000054E4
		protected virtual void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				if (disposing)
				{
					Array.Clear(this.table, 0, this.table.Length);
				}
				this.disposed = true;
			}
		}

		/// <summary>
		/// Finalizes an instance of the <see cref="T:ZER0.Core.Cryptography.Crc32B" /> class.
		/// </summary>
		// Token: 0x06000031 RID: 49 RVA: 0x0000550C File Offset: 0x0000550C
		~Crc32B()
		{
			this.Dispose(false);
		}

		// Token: 0x0400000A RID: 10
		private const uint Polynomial = 3988292384U;

		// Token: 0x0400000B RID: 11
		private const uint DefaultSeed = 4294967295U;

		// Token: 0x0400000C RID: 12
		private readonly uint[] table = new uint[256];

		// Token: 0x0400000D RID: 13
		private bool disposed;
	}
}
