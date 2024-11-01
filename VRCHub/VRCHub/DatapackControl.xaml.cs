using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

namespace VRCHub
{
	// Token: 0x02000008 RID: 8
	public partial class DatapackControl : UserControl
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000029 RID: 41 RVA: 0x004EBC8C File Offset: 0x004EBC8C
		// (remove) Token: 0x0600002A RID: 42 RVA: 0x004EBCC4 File Offset: 0x004EBCC4
		[Nullable(2)]
		[method: NullableContext(2)]
		[Nullable(2)]
		public event Action InstallClicked;

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x0600002B RID: 43 RVA: 0x004EBCFC File Offset: 0x004EBCFC
		// (remove) Token: 0x0600002C RID: 44 RVA: 0x004EBD34 File Offset: 0x004EBD34
		[Nullable(2)]
		[method: NullableContext(2)]
		[Nullable(2)]
		public event Action UninstallClicked;

		// Token: 0x0600002D RID: 45 RVA: 0x004EBD6C File Offset: 0x004EBD6C
		public DatapackControl()
		{
			this.InitializeComponent();
			this.Datapack_Install.Click += delegate(object s, RoutedEventArgs e)
			{
				Action installClicked = this.InstallClicked;
				if (installClicked == null)
				{
					return;
				}
				installClicked();
			};
			this.Datapack_Uninstall.Click += delegate(object s, RoutedEventArgs e)
			{
				Action uninstallClicked = this.UninstallClicked;
				if (uninstallClicked == null)
				{
					return;
				}
				uninstallClicked();
			};
		}

		// Token: 0x0600002E RID: 46 RVA: 0x004EBDA8 File Offset: 0x004EBDA8
		[NullableContext(1)]
		public void SetImage(ImageSource image)
		{
			this.Datapack_Image.Source = image;
		}

		// Token: 0x0600002F RID: 47 RVA: 0x004EBDB8 File Offset: 0x004EBDB8
		[NullableContext(1)]
		public void SetText(string text)
		{
			this.Datapack_Name.Content = text;
		}
	}
}
