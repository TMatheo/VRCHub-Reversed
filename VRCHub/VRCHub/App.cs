using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Windows;

namespace VRCHub
{
	// Token: 0x02000002 RID: 2
	public class App : Application
	{
		// Token: 0x06000002 RID: 2 RVA: 0x004EB6EC File Offset: 0x004EB6EC
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "9.0.0.0")]
		public void InitializeComponent()
		{
			base.StartupUri = new Uri("MainWindow.xaml", UriKind.Relative);
		}

		// Token: 0x06000003 RID: 3 RVA: 0x004EB700 File Offset: 0x004EB700
		[STAThread]
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "9.0.0.0")]
		public static void Main()
		{
			App app = new App();
			app.InitializeComponent();
			app.Run();
		}
	}
}
