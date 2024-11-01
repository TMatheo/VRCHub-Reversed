using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Loader;
using System.Threading;

namespace Costura
{
	// Token: 0x02000036 RID: 54
	[CompilerGenerated]
	internal static class AssemblyLoader
	{
		// Token: 0x06000161 RID: 353 RVA: 0x004F1870 File Offset: 0x004F1870
		private static string CultureToString(CultureInfo culture)
		{
			if (culture == null)
			{
				return string.Empty;
			}
			return culture.Name;
		}

		// Token: 0x06000162 RID: 354 RVA: 0x004F1884 File Offset: 0x004F1884
		private static Assembly ReadExistingAssembly(AssemblyName name)
		{
			AppDomain currentDomain = AppDomain.CurrentDomain;
			Assembly[] assemblies = currentDomain.GetAssemblies();
			foreach (Assembly assembly in assemblies)
			{
				AssemblyName name2 = assembly.GetName();
				if (string.Equals(name2.Name, name.Name, StringComparison.InvariantCultureIgnoreCase) && string.Equals(AssemblyLoader.CultureToString(name2.CultureInfo), AssemblyLoader.CultureToString(name.CultureInfo), StringComparison.InvariantCultureIgnoreCase))
				{
					return assembly;
				}
			}
			return null;
		}

		// Token: 0x06000163 RID: 355 RVA: 0x004F18F4 File Offset: 0x004F18F4
		private static string GetAssemblyResourceName(AssemblyName requestedAssemblyName)
		{
			string text = requestedAssemblyName.Name.ToLowerInvariant();
			if (requestedAssemblyName.CultureInfo != null && !string.IsNullOrEmpty(requestedAssemblyName.CultureInfo.Name))
			{
				text = (AssemblyLoader.CultureToString(requestedAssemblyName.CultureInfo) + "." + text).ToLowerInvariant();
			}
			return text;
		}

		// Token: 0x06000164 RID: 356 RVA: 0x004F1944 File Offset: 0x004F1944
		private static void CopyTo(Stream source, Stream destination)
		{
			byte[] array = new byte[81920];
			int count;
			while ((count = source.Read(array, 0, array.Length)) != 0)
			{
				destination.Write(array, 0, count);
			}
		}

		// Token: 0x06000165 RID: 357 RVA: 0x004F1978 File Offset: 0x004F1978
		private static Stream LoadStream(string fullName)
		{
			Assembly executingAssembly = Assembly.GetExecutingAssembly();
			if (fullName.EndsWith(".compressed"))
			{
				using (Stream manifestResourceStream = executingAssembly.GetManifestResourceStream(fullName))
				{
					using (DeflateStream deflateStream = new DeflateStream(manifestResourceStream, CompressionMode.Decompress))
					{
						MemoryStream memoryStream = new MemoryStream();
						AssemblyLoader.CopyTo(deflateStream, memoryStream);
						memoryStream.Position = 0L;
						return memoryStream;
					}
				}
			}
			return executingAssembly.GetManifestResourceStream(fullName);
		}

		// Token: 0x06000166 RID: 358 RVA: 0x004F19FC File Offset: 0x004F19FC
		private static Stream LoadStream(Dictionary<string, string> resourceNames, string name)
		{
			string fullName;
			if (resourceNames.TryGetValue(name, out fullName))
			{
				return AssemblyLoader.LoadStream(fullName);
			}
			return null;
		}

		// Token: 0x06000167 RID: 359 RVA: 0x004F1A1C File Offset: 0x004F1A1C
		private static byte[] ReadStream(Stream stream)
		{
			byte[] array = new byte[stream.Length];
			stream.Read(array, 0, array.Length);
			return array;
		}

		// Token: 0x06000168 RID: 360 RVA: 0x004F1A44 File Offset: 0x004F1A44
		private static Assembly ReadFromEmbeddedResources(Dictionary<string, string> assemblyNames, Dictionary<string, string> symbolNames, AssemblyName requestedAssemblyName)
		{
			string assemblyResourceName = AssemblyLoader.GetAssemblyResourceName(requestedAssemblyName);
			byte[] rawAssembly;
			using (Stream stream = AssemblyLoader.LoadStream(assemblyNames, assemblyResourceName))
			{
				if (stream == null)
				{
					return null;
				}
				rawAssembly = AssemblyLoader.ReadStream(stream);
			}
			using (Stream stream2 = AssemblyLoader.LoadStream(symbolNames, assemblyResourceName))
			{
				if (stream2 != null)
				{
					byte[] rawSymbolStore = AssemblyLoader.ReadStream(stream2);
					return Assembly.Load(rawAssembly, rawSymbolStore);
				}
			}
			return Assembly.Load(rawAssembly);
		}

		// Token: 0x06000169 RID: 361 RVA: 0x004F1AD0 File Offset: 0x004F1AD0
		public static Assembly ResolveAssembly(AssemblyLoadContext assemblyLoadContext, AssemblyName assemblyName)
		{
			string name = assemblyName.Name;
			object obj = AssemblyLoader.nullCacheLock;
			lock (obj)
			{
				if (AssemblyLoader.nullCache.ContainsKey(name))
				{
					return null;
				}
			}
			Assembly assembly = AssemblyLoader.ReadExistingAssembly(assemblyName);
			if (assembly != null)
			{
				return assembly;
			}
			assembly = AssemblyLoader.ReadFromEmbeddedResources(AssemblyLoader.assemblyNames, AssemblyLoader.symbolNames, assemblyName);
			if (assembly == null)
			{
				object obj2 = AssemblyLoader.nullCacheLock;
				lock (obj2)
				{
					AssemblyLoader.nullCache[name] = true;
				}
				if ((assemblyName.Flags & AssemblyNameFlags.Retargetable) != AssemblyNameFlags.None)
				{
					assembly = Assembly.Load(assemblyName);
				}
			}
			return assembly;
		}

		// Token: 0x0600016A RID: 362 RVA: 0x004F1B94 File Offset: 0x004F1B94
		// Note: this type is marked as 'beforefieldinit'.
		static AssemblyLoader()
		{
			AssemblyLoader.assemblyNames.Add("analytics", "costura.analytics.dll.compressed");
			AssemblyLoader.assemblyNames.Add("costura", "costura.costura.dll.compressed");
			AssemblyLoader.symbolNames.Add("costura", "costura.costura.pdb.compressed");
			AssemblyLoader.assemblyNames.Add("hardcodet.notifyicon.wpf", "costura.hardcodet.notifyicon.wpf.dll.compressed");
			AssemblyLoader.assemblyNames.Add("magmamc.jef", "costura.magmamc.jef.dll.compressed");
			AssemblyLoader.assemblyNames.Add("magmamc.magmasimpleconfig", "costura.magmamc.magmasimpleconfig.exe.compressed");
			AssemblyLoader.assemblyNames.Add("microsoft.extensions.dependencyinjection.abstractions", "costura.microsoft.extensions.dependencyinjection.abstractions.dll.compressed");
			AssemblyLoader.assemblyNames.Add("newtonsoft.json", "costura.newtonsoft.json.dll.compressed");
			AssemblyLoader.assemblyNames.Add("spectre.console", "costura.spectre.console.dll.compressed");
			AssemblyLoader.assemblyNames.Add("system.drawing.common", "costura.system.drawing.common.dll.compressed");
			AssemblyLoader.symbolNames.Add("system.drawing.common", "costura.system.drawing.common.pdb.compressed");
			AssemblyLoader.assemblyNames.Add("system.private.windows.core", "costura.system.private.windows.core.dll.compressed");
			AssemblyLoader.assemblyNames.Add("zer0.core", "costura.zer0.core.dll.compressed");
			AssemblyLoader.assemblyNames.Add("zer0.vrchat.patch", "costura.zer0.vrchat.patch.dll.compressed");
			AssemblyLoader.symbolNames.Add("zer0.vrchat.patch", "costura.zer0.vrchat.patch.pdb.compressed");
		}

		// Token: 0x0600016B RID: 363 RVA: 0x004F1CF8 File Offset: 0x004F1CF8
		public static void Attach(bool subscribe)
		{
			if (Interlocked.Exchange(ref AssemblyLoader.isAttached, 1) == 1)
			{
				return;
			}
			if (subscribe)
			{
				AssemblyLoadContext @default = AssemblyLoadContext.Default;
				Func<AssemblyLoadContext, AssemblyName, Assembly> value;
				if ((value = AssemblyLoader.<>O.<0>__ResolveAssembly) == null)
				{
					value = (AssemblyLoader.<>O.<0>__ResolveAssembly = new Func<AssemblyLoadContext, AssemblyName, Assembly>(AssemblyLoader.ResolveAssembly));
				}
				@default.Resolving += value;
			}
		}

		// Token: 0x0400011C RID: 284
		private static object nullCacheLock = new object();

		// Token: 0x0400011D RID: 285
		private static Dictionary<string, bool> nullCache = new Dictionary<string, bool>();

		// Token: 0x0400011E RID: 286
		private static Dictionary<string, string> assemblyNames = new Dictionary<string, string>();

		// Token: 0x0400011F RID: 287
		private static Dictionary<string, string> symbolNames = new Dictionary<string, string>();

		// Token: 0x04000120 RID: 288
		private static int isAttached;

		// Token: 0x02000037 RID: 55
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x04000121 RID: 289
			public static Func<AssemblyLoadContext, AssemblyName, Assembly> <0>__ResolveAssembly;
		}
	}
}
