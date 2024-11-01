using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace VRCHub
{
	// Token: 0x02000003 RID: 3
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
	[DebuggerNonUserCode]
	[CompilerGenerated]
	internal class AppResources
	{
		// Token: 0x06000005 RID: 5 RVA: 0x004EB71C File Offset: 0x004EB71C
		internal AppResources()
		{
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000006 RID: 6 RVA: 0x004EB724 File Offset: 0x004EB724
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static ResourceManager ResourceManager
		{
			get
			{
				if (AppResources.resourceMan == null)
				{
					AppResources.resourceMan = new ResourceManager("VRCHub.AppResources", typeof(AppResources).Assembly);
				}
				return AppResources.resourceMan;
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000007 RID: 7 RVA: 0x004EB750 File Offset: 0x004EB750
		// (set) Token: 0x06000008 RID: 8 RVA: 0x004EB758 File Offset: 0x004EB758
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static CultureInfo Culture
		{
			get
			{
				return AppResources.resourceCulture;
			}
			set
			{
				AppResources.resourceCulture = value;
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000009 RID: 9 RVA: 0x004EB760 File Offset: 0x004EB760
		internal static byte[] SplashScreen
		{
			get
			{
				return (byte[])AppResources.ResourceManager.GetObject("SplashScreen", AppResources.resourceCulture);
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600000A RID: 10 RVA: 0x004EB77C File Offset: 0x004EB77C
		internal static byte[] VRC_Quick_Launcher
		{
			get
			{
				return (byte[])AppResources.ResourceManager.GetObject("VRC_Quick_Launcher", AppResources.resourceCulture);
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600000B RID: 11 RVA: 0x004EB798 File Offset: 0x004EB798
		internal static byte[] VRCFX_Example1
		{
			get
			{
				return (byte[])AppResources.ResourceManager.GetObject("VRCFX_Example1", AppResources.resourceCulture);
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600000C RID: 12 RVA: 0x004EB7B4 File Offset: 0x004EB7B4
		internal static byte[] VRCFX_Example2
		{
			get
			{
				return (byte[])AppResources.ResourceManager.GetObject("VRCFX_Example2", AppResources.resourceCulture);
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600000D RID: 13 RVA: 0x004EB7D0 File Offset: 0x004EB7D0
		internal static byte[] VRCSpoofer_Example1
		{
			get
			{
				return (byte[])AppResources.ResourceManager.GetObject("VRCSpoofer_Example1", AppResources.resourceCulture);
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600000E RID: 14 RVA: 0x004EB7EC File Offset: 0x004EB7EC
		internal static byte[] ZER0_Certificates
		{
			get
			{
				return (byte[])AppResources.ResourceManager.GetObject("ZER0_Certificates", AppResources.resourceCulture);
			}
		}

		// Token: 0x04000001 RID: 1
		private static ResourceManager resourceMan;

		// Token: 0x04000002 RID: 2
		private static CultureInfo resourceCulture;
	}
}
