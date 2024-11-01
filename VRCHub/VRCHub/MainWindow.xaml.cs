using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Microsoft.Win32;
using Segment;
using ZER0.VRChat.Patch;

namespace VRCHub
{
	// Token: 0x0200000B RID: 11
	public partial class MainWindow : Window
	{
		// Token: 0x0600003D RID: 61 RVA: 0x004EC228 File Offset: 0x004EC228
		[NullableContext(1)]
		private void PauseButton(System.Windows.Controls.Button button, [Nullable(2)] string SetText = null)
		{
			System.Windows.Application.Current.Dispatcher.Invoke(delegate()
			{
				if (!this.ButtonToggles.ContainsKey(button))
				{
					this.ButtonToggles.Add(button, new MainWindow.ButtonData((string)button.Content, true));
				}
				if (SetText != null)
				{
					button.Content = SetText;
				}
			});
		}

		// Token: 0x0600003E RID: 62 RVA: 0x004EC26C File Offset: 0x004EC26C
		[NullableContext(1)]
		private bool ButtonPaused(System.Windows.Controls.Button button)
		{
			MainWindow.ButtonData buttonData;
			return this.ButtonToggles.TryGetValue(button, out buttonData) && buttonData.Paused;
		}

		// Token: 0x0600003F RID: 63 RVA: 0x004EC294 File Offset: 0x004EC294
		[NullableContext(1)]
		private void ResumeButon(System.Windows.Controls.Button button)
		{
			System.Windows.Application.Current.Dispatcher.Invoke(delegate()
			{
				MainWindow.ButtonData buttonData;
				if (!this.ButtonToggles.TryGetValue(button, out buttonData))
				{
					this.ButtonToggles.Add(button, new MainWindow.ButtonData((string)button.Content, false));
				}
				else
				{
					buttonData.Paused = false;
				}
				button.Content = this.ButtonToggles[button].BaseText;
			});
		}

		// Token: 0x06000040 RID: 64 RVA: 0x004EC2D0 File Offset: 0x004EC2D0
		private static void NotifyDownloadStarted()
		{
			NotifyIcon Notify = new NotifyIcon
			{
				Visible = true,
				Icon = SystemIcons.Warning,
				BalloonTipTitle = "Update Started",
				BalloonTipText = "This May Take Some Time Depending On Your System Specs"
			};
			Notify.ShowBalloonTip(3000);
			Notify.BalloonTipClosed += delegate([Nullable(2)] object sender, EventArgs args)
			{
				Notify.Dispose();
			};
		}

		// Token: 0x06000041 RID: 65 RVA: 0x004EC340 File Offset: 0x004EC340
		public MainWindow()
		{
			MainWindow.SetupConsole(Environment.GetCommandLineArgs());
			Common.StartAnalytics();
			base.Hide();
			this.InitilizeServerAPI();
			this.InitializeSplashScreen();
			this.InitializeMainWindowAsync();
			this.SetupEvents();
		}

		// Token: 0x06000042 RID: 66 RVA: 0x004EC394 File Offset: 0x004EC394
		[NullableContext(1)]
		private static void SetupConsole(string[] args)
		{
			if (args.Contains("--console") || args.Contains("-v") || args.Contains("--verbose") || Debugger.IsAttached)
			{
				KERNAL32.AllocConsole();
				Console.SetOut(new StreamWriter(Console.OpenStandardOutput())
				{
					AutoFlush = true
				});
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000043 RID: 67 RVA: 0x004EC3EC File Offset: 0x004EC3EC
		[Nullable(1)]
		internal static ServerAPI API
		{
			[NullableContext(1)]
			get
			{
				return MainWindow.api;
			}
		}

		// Token: 0x06000044 RID: 68 RVA: 0x004EC3F4 File Offset: 0x004EC3F4
		[NullableContext(1)]
		internal static string GetServer(string server)
		{
			return ServerAPI.GetServer(server);
		}

		// Token: 0x06000045 RID: 69 RVA: 0x004EC3FC File Offset: 0x004EC3FC
		private void InitilizeServerAPI()
		{
			MainWindow.api = new ServerAPI();
		}

		// Token: 0x06000046 RID: 70 RVA: 0x004EC408 File Offset: 0x004EC408
		private void InitializeSplashScreen()
		{
			MainWindow.<InitializeSplashScreen>d__24 <InitializeSplashScreen>d__;
			<InitializeSplashScreen>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
			<InitializeSplashScreen>d__.<>4__this = this;
			<InitializeSplashScreen>d__.<>1__state = -1;
			<InitializeSplashScreen>d__.<>t__builder.Start<MainWindow.<InitializeSplashScreen>d__24>(ref <InitializeSplashScreen>d__);
		}

		// Token: 0x06000047 RID: 71 RVA: 0x004EC440 File Offset: 0x004EC440
		[NullableContext(1)]
		private void CheckServer(string server, byte index)
		{
			if (MainWindow.API.CheckServer(server))
			{
				return;
			}
			System.Windows.Application.Current.Dispatcher.Invoke(new Action(this._splashScreen.StartFadeOut));
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(134, 2);
			defaultInterpolatedStringHandler.AppendLiteral("Failed To Check Server Status On Server ");
			defaultInterpolatedStringHandler.AppendFormatted<byte>(index);
			defaultInterpolatedStringHandler.AppendLiteral("/");
			defaultInterpolatedStringHandler.AppendFormatted<int>(ServerAPI.Servers.Length);
			defaultInterpolatedStringHandler.AppendLiteral("\r\nDo You Want To Use Our Server Proxy?");
			defaultInterpolatedStringHandler.AppendLiteral("\r\nIf This Continues To Happen Please Try And Use A VPN!");
			if (System.Windows.MessageBox.Show(defaultInterpolatedStringHandler.ToStringAndClear(), "ServerAPI - Status", MessageBoxButton.OKCancel, MessageBoxImage.Exclamation) == MessageBoxResult.OK)
			{
				this.CheckProxyServer();
				return;
			}
			Environment.Exit(2);
		}

		// Token: 0x06000048 RID: 72 RVA: 0x004EC4F8 File Offset: 0x004EC4F8
		private void CheckProxyServer()
		{
			ServerAPI.usingProxy = true;
			if (MainWindow.API.CheckServer("https://magmamc.dev/ServerProxy/vrchub/api/2/Status"))
			{
				System.Windows.Application.Current.Dispatcher.Invoke(new Action(this._splashScreen.StartFadeIn));
				return;
			}
			if (System.Windows.Forms.MessageBox.Show("Failed To Check Server Status On Server Proxy\r\nIf This Continues To Happen Please Try And Use A VPN!", "ServerAPI - Proxy Status", MessageBoxButtons.RetryCancel, MessageBoxIcon.Exclamation) == System.Windows.Forms.DialogResult.Retry)
			{
				System.Windows.Forms.Application.Restart();
				Environment.Exit(3);
				return;
			}
			Environment.Exit(2);
		}

		// Token: 0x06000049 RID: 73 RVA: 0x004EC564 File Offset: 0x004EC564
		private void InitializeMainWindowAsync()
		{
			MainWindow.<InitializeMainWindowAsync>d__27 <InitializeMainWindowAsync>d__;
			<InitializeMainWindowAsync>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
			<InitializeMainWindowAsync>d__.<>4__this = this;
			<InitializeMainWindowAsync>d__.<>1__state = -1;
			<InitializeMainWindowAsync>d__.<>t__builder.Start<MainWindow.<InitializeMainWindowAsync>d__27>(ref <InitializeMainWindowAsync>d__);
		}

		// Token: 0x0600004A RID: 74 RVA: 0x004EC59C File Offset: 0x004EC59C
		[NullableContext(1)]
		private Task LoadDataPacksAsync()
		{
			MainWindow.<LoadDataPacksAsync>d__28 <LoadDataPacksAsync>d__;
			<LoadDataPacksAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<LoadDataPacksAsync>d__.<>4__this = this;
			<LoadDataPacksAsync>d__.<>1__state = -1;
			<LoadDataPacksAsync>d__.<>t__builder.Start<MainWindow.<LoadDataPacksAsync>d__28>(ref <LoadDataPacksAsync>d__);
			return <LoadDataPacksAsync>d__.<>t__builder.Task;
		}

		// Token: 0x0600004B RID: 75 RVA: 0x004EC5E0 File Offset: 0x004EC5E0
		[NullableContext(1)]
		private void VRCFXButton_Click(object sender, [Nullable(2)] RoutedEventArgs e)
		{
			if (Config.SendAnalytics)
			{
				Analytics.Client.Page(Environment.MachineName, "VRCFX Viewed");
			}
			this.VRCFX_Panel.Visibility = Visibility.Visible;
			this.VRCSpoofer_Panel.Visibility = Visibility.Collapsed;
			this.Datapacks_Panel.Visibility = Visibility.Collapsed;
			this.Splashscreen_Panel.Visibility = Visibility.Collapsed;
			this.Settings_Panel.Visibility = Visibility.Collapsed;
			this.DatapackCreator_Panel.Visibility = Visibility.Collapsed;
			this.Patch_Panel.Visibility = Visibility.Collapsed;
			this.MelonLoader_Panel.Visibility = Visibility.Collapsed;
		}

		// Token: 0x0600004C RID: 76 RVA: 0x004EC668 File Offset: 0x004EC668
		[NullableContext(1)]
		private void VRCFX_DownloadButton_Click(object sender, [Nullable(2)] RoutedEventArgs e)
		{
			MainWindow.<VRCFX_DownloadButton_Click>d__30 <VRCFX_DownloadButton_Click>d__;
			<VRCFX_DownloadButton_Click>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
			<VRCFX_DownloadButton_Click>d__.<>4__this = this;
			<VRCFX_DownloadButton_Click>d__.<>1__state = -1;
			<VRCFX_DownloadButton_Click>d__.<>t__builder.Start<MainWindow.<VRCFX_DownloadButton_Click>d__30>(ref <VRCFX_DownloadButton_Click>d__);
		}

		// Token: 0x0600004D RID: 77 RVA: 0x004EC6A0 File Offset: 0x004EC6A0
		[NullableContext(1)]
		private void VRCFX_GetLicenseButton_Click(object sender, [Nullable(2)] RoutedEventArgs e)
		{
			if (Config.SendAnalytics)
			{
				Analytics.Client.Track(Environment.MachineName, "VRCFX Get License");
			}
			MainWindow.OpenURL("https://hdzero.mysellix.io/pay/9b069c-20bb91bd74-877091");
		}

		// Token: 0x0600004E RID: 78 RVA: 0x004EC6C8 File Offset: 0x004EC6C8
		[NullableContext(1)]
		private void VRCSpooferButton_Click(object sender, [Nullable(2)] RoutedEventArgs e)
		{
			if (Config.SendAnalytics)
			{
				Analytics.Client.Page(Environment.MachineName, "VRCSpoofer Viewed");
			}
			this.VRCFX_Panel.Visibility = Visibility.Collapsed;
			this.VRCSpoofer_Panel.Visibility = Visibility.Visible;
			this.Datapacks_Panel.Visibility = Visibility.Collapsed;
			this.Splashscreen_Panel.Visibility = Visibility.Collapsed;
			this.Settings_Panel.Visibility = Visibility.Collapsed;
			this.DatapackCreator_Panel.Visibility = Visibility.Collapsed;
			this.Patch_Panel.Visibility = Visibility.Collapsed;
			this.MelonLoader_Panel.Visibility = Visibility.Collapsed;
		}

		// Token: 0x0600004F RID: 79 RVA: 0x004EC750 File Offset: 0x004EC750
		[NullableContext(1)]
		private void VRCSpoofer_GetLicenseButton_Click(object sender, [Nullable(2)] RoutedEventArgs e)
		{
			if (Config.SendAnalytics)
			{
				Analytics.Client.Track(Environment.MachineName, "VRCSpoofer Get License");
			}
			MainWindow.OpenURL("https://hdzero.mysellix.io/pay/005854-950d2e0567-b6a173");
		}

		// Token: 0x06000050 RID: 80 RVA: 0x004EC778 File Offset: 0x004EC778
		[NullableContext(1)]
		private void VRCSpoofer_DownloadButton_Click(object sender, [Nullable(2)] RoutedEventArgs e)
		{
			MainWindow.<VRCSpoofer_DownloadButton_Click>d__34 <VRCSpoofer_DownloadButton_Click>d__;
			<VRCSpoofer_DownloadButton_Click>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
			<VRCSpoofer_DownloadButton_Click>d__.<>4__this = this;
			<VRCSpoofer_DownloadButton_Click>d__.<>1__state = -1;
			<VRCSpoofer_DownloadButton_Click>d__.<>t__builder.Start<MainWindow.<VRCSpoofer_DownloadButton_Click>d__34>(ref <VRCSpoofer_DownloadButton_Click>d__);
		}

		// Token: 0x06000051 RID: 81 RVA: 0x004EC7B0 File Offset: 0x004EC7B0
		[NullableContext(1)]
		private void Datapacks_Click(object sender, [Nullable(2)] RoutedEventArgs e)
		{
			if (Config.SendAnalytics)
			{
				Analytics.Client.Page(Environment.MachineName, "Datapacks Viewed");
			}
			this.VRCFX_Panel.Visibility = Visibility.Collapsed;
			this.VRCSpoofer_Panel.Visibility = Visibility.Collapsed;
			this.Datapacks_Panel.Visibility = Visibility.Visible;
			this.Splashscreen_Panel.Visibility = Visibility.Collapsed;
			this.Settings_Panel.Visibility = Visibility.Collapsed;
			this.DatapackCreator_Panel.Visibility = Visibility.Collapsed;
			this.Patch_Panel.Visibility = Visibility.Collapsed;
			this.MelonLoader_Panel.Visibility = Visibility.Collapsed;
		}

		// Token: 0x06000052 RID: 82 RVA: 0x004EC838 File Offset: 0x004EC838
		[NullableContext(1)]
		private void QuickLauncherButton_Click(object sender, RoutedEventArgs e)
		{
			if (this.VRCQuickLauncher == null || this.VRCQuickLauncher.HasExited)
			{
				ProcessStartInfo startInfo = new ProcessStartInfo(Path.Combine(Path.GetTempPath(), "VRC Quick Launcher.exe"));
				if (!File.Exists(Path.Combine(Path.GetTempPath(), "VRC Quick Launcher.exe")))
				{
					File.WriteAllBytes(Path.Combine(Path.GetTempPath(), "VRC Quick Launcher.exe"), AppResources.VRC_Quick_Launcher);
				}
				this.VRCQuickLauncher = Process.Start(startInfo);
				return;
			}
			KERNAL32.SetForegroundWindow(this.VRCQuickLauncher.Handle);
		}

		// Token: 0x06000053 RID: 83 RVA: 0x004EC8BC File Offset: 0x004EC8BC
		public void UpdateSplashScreen()
		{
			try
			{
				this.Splashscreen_CurrentImage.Source = Common.GetImageSource(File.ReadAllBytes(SplashscreenEditor.SplashScreenPath));
			}
			catch
			{
			}
		}

		// Token: 0x06000054 RID: 84 RVA: 0x004EC8F8 File Offset: 0x004EC8F8
		[NullableContext(1)]
		private void SplashScreenButton_Click(object sender, RoutedEventArgs e)
		{
			if (Config.SendAnalytics)
			{
				Analytics.Client.Page(Environment.MachineName, "SplashScreens Viewed");
			}
			this.VRCFX_Panel.Visibility = Visibility.Collapsed;
			this.VRCSpoofer_Panel.Visibility = Visibility.Collapsed;
			this.Datapacks_Panel.Visibility = Visibility.Collapsed;
			this.Splashscreen_Panel.Visibility = Visibility.Visible;
			this.Settings_Panel.Visibility = Visibility.Collapsed;
			this.DatapackCreator_Panel.Visibility = Visibility.Collapsed;
			this.Patch_Panel.Visibility = Visibility.Collapsed;
			this.MelonLoader_Panel.Visibility = Visibility.Collapsed;
		}

		// Token: 0x06000055 RID: 85 RVA: 0x004EC980 File Offset: 0x004EC980
		[NullableContext(1)]
		private void SplashScreenResetButton_Click(object sender, RoutedEventArgs e)
		{
			SplashscreenEditor.SaveImage(Common.GetImageSource(AppResources.SplashScreen));
			this.UpdateSplashScreen();
		}

		// Token: 0x06000056 RID: 86 RVA: 0x004EC998 File Offset: 0x004EC998
		[NullableContext(1)]
		private void SplashScreenChangeButton_Clicked(object sender, RoutedEventArgs e)
		{
			BitmapImage bitmapImage = SplashscreenEditor.SelectImageFromExplorer();
			if (bitmapImage == null)
			{
				return;
			}
			SplashscreenEditor.ScaleImage(bitmapImage, out bitmapImage);
			SplashscreenEditor.SaveImage(bitmapImage);
			this.UpdateSplashScreen();
		}

		// Token: 0x06000057 RID: 87 RVA: 0x004EC9C4 File Offset: 0x004EC9C4
		[NullableContext(1)]
		private void SettingsButton_Click(object sender, [Nullable(2)] RoutedEventArgs e)
		{
			this.VRCFX_Panel.Visibility = Visibility.Collapsed;
			this.VRCSpoofer_Panel.Visibility = Visibility.Collapsed;
			this.Datapacks_Panel.Visibility = Visibility.Collapsed;
			this.Splashscreen_Panel.Visibility = Visibility.Collapsed;
			this.Settings_Panel.Visibility = Visibility.Visible;
			this.DatapackCreator_Panel.Visibility = Visibility.Collapsed;
			this.Patch_Panel.Visibility = Visibility.Collapsed;
			this.MelonLoader_Panel.Visibility = Visibility.Collapsed;
		}

		// Token: 0x06000058 RID: 88 RVA: 0x004ECA34 File Offset: 0x004ECA34
		[NullableContext(1)]
		private void Settings_VRCPath_TextChanged(object sender, TextChangedEventArgs e)
		{
			Config.VRC_Path = this.Settings_VRCPath.Text;
			bool flag = File.Exists(Config.VRC_Path);
			this.VRCFXButton.IsEnabled = flag;
			this.VRCSpooferButton.IsEnabled = flag;
			this.DatapacksButton.IsEnabled = flag;
			this.SplashScreenButton.IsEnabled = flag;
			this.QuickLauncherButton.IsEnabled = flag;
			if (flag)
			{
				this.UpdateSplashScreen();
			}
			Config.SaveConfig();
		}

		// Token: 0x06000059 RID: 89 RVA: 0x004ECAA8 File Offset: 0x004ECAA8
		[NullableContext(1)]
		private void Settings_VRCPath_DoubleClicked(object sender, MouseButtonEventArgs e)
		{
			Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog
			{
				Filter = "VRChat (VRChat.exe)|VRChat.exe"
			};
			if (openFileDialog.ShowDialog().GetValueOrDefault())
			{
				this.Settings_VRCPath.Text = openFileDialog.FileName;
			}
		}

		// Token: 0x0600005A RID: 90 RVA: 0x004ECAE8 File Offset: 0x004ECAE8
		[NullableContext(1)]
		private void Settings_SendAnalytics_Clicked(object sender, RoutedEventArgs e)
		{
			Config.SendAnalytics = this.Settings_SendAnalytics.IsChecked.GetValueOrDefault(true);
			Config.SaveConfig();
		}

		// Token: 0x0600005B RID: 91 RVA: 0x004ECB14 File Offset: 0x004ECB14
		[NullableContext(1)]
		private void BackupVRChatConfig(object sender, RoutedEventArgs e)
		{
			string[] source = new string[]
			{
				"RECENTLY_VISITED_",
				"_LastExpiredSubscription_"
			};
			List<RegKey> list = new List<RegKey>();
			try
			{
				using (RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\VRChat\\VRChat"))
				{
					if (registryKey != null)
					{
						string[] valueNames = registryKey.GetValueNames();
						for (int i = 0; i < valueNames.Length; i++)
						{
							string valueName = valueNames[i];
							if (!source.Any((string prefix) => valueName.StartsWith(prefix)))
							{
								object value = registryKey.GetValue(valueName);
								list.Add(new RegKey
								{
									ObjectName = valueName,
									Value = value
								});
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				System.Windows.MessageBox.Show("Error reading registry: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
				return;
			}
			Config.VRChatRegBackup = list.ToArray();
			Config.SaveConfig();
			System.Windows.MessageBox.Show("Backup completed successfully.");
		}

		// Token: 0x0600005C RID: 92 RVA: 0x004ECC28 File Offset: 0x004ECC28
		[NullableContext(1)]
		private void LoadVRChatConfig(object sender, RoutedEventArgs e)
		{
			try
			{
				if (Config.VRChatRegBackup != null)
				{
					foreach (RegKey regKey in Config.VRChatRegBackup)
					{
						using (RegistryKey registryKey = Registry.CurrentUser.CreateSubKey("Software\\VRChat\\VRChat"))
						{
							if (registryKey != null)
							{
								registryKey.SetValue(regKey.ObjectName, regKey.Value);
							}
						}
					}
					System.Windows.MessageBox.Show("Configuration loaded successfully.");
				}
			}
			catch (Exception ex)
			{
				System.Windows.MessageBox.Show("Error writing to registry: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
			}
		}

		// Token: 0x0600005D RID: 93 RVA: 0x004ECCD4 File Offset: 0x004ECCD4
		[NullableContext(1)]
		private void ClearVRChatConfigBackup(object sender, RoutedEventArgs e)
		{
			try
			{
				Config.VRChatRegBackup = null;
				Config.SaveConfig();
				System.Windows.MessageBox.Show("Configuration cleared successfully.");
			}
			catch (Exception ex)
			{
				System.Windows.MessageBox.Show("Error clearing Backup: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
			}
		}

		// Token: 0x0600005E RID: 94 RVA: 0x004ECD2C File Offset: 0x004ECD2C
		[NullableContext(1)]
		private void DiscordButton_Click(object sender, [Nullable(2)] RoutedEventArgs e)
		{
			if (Config.SendAnalytics)
			{
				Analytics.Client.Track(Environment.MachineName, "DiscordButton_Click");
			}
			MainWindow.OpenURL("https://dc.vrchub.site");
		}

		// Token: 0x0600005F RID: 95 RVA: 0x004ECD54 File Offset: 0x004ECD54
		[NullableContext(1)]
		public static void OpenURL(string URL)
		{
			Process.Start(new ProcessStartInfo
			{
				FileName = URL,
				UseShellExecute = true
			});
		}

		// Token: 0x06000060 RID: 96 RVA: 0x004ECD70 File Offset: 0x004ECD70
		[NullableContext(1)]
		private void DatapackCreator_Click(object sender, RoutedEventArgs e)
		{
			this.VRCFX_Panel.Visibility = Visibility.Collapsed;
			this.VRCSpoofer_Panel.Visibility = Visibility.Collapsed;
			this.Datapacks_Panel.Visibility = Visibility.Collapsed;
			this.Splashscreen_Panel.Visibility = Visibility.Collapsed;
			this.Settings_Panel.Visibility = Visibility.Collapsed;
			this.DatapackCreator_Panel.Visibility = Visibility.Visible;
			this.Patch_Panel.Visibility = Visibility.Collapsed;
			this.MelonLoader_Panel.Visibility = Visibility.Collapsed;
		}

		// Token: 0x06000061 RID: 97 RVA: 0x004ECDE0 File Offset: 0x004ECDE0
		[NullableContext(1)]
		private void Datapack_InputPath_DoubleClicked(object sender, MouseButtonEventArgs e)
		{
			Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog
			{
				Filter = "Asset Bundle (__data, *.vrcw)|__data;*.vrcw|All files (*.*)|*.*"
			};
			if (openFileDialog.ShowDialog().GetValueOrDefault())
			{
				this.Datapack_InputPath.Text = openFileDialog.FileName;
			}
		}

		// Token: 0x06000062 RID: 98 RVA: 0x004ECE20 File Offset: 0x004ECE20
		[NullableContext(1)]
		private void Datapack_OutputPath_DoubleClicked(object sender, MouseButtonEventArgs e)
		{
			Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog
			{
				Filter = "Asset Pack (*.dp)|*.dp",
				DefaultExt = ".dp"
			};
			if (saveFileDialog.ShowDialog().GetValueOrDefault())
			{
				this.Datapack_OutputFile.Text = saveFileDialog.FileName;
			}
		}

		// Token: 0x06000063 RID: 99 RVA: 0x004ECE6C File Offset: 0x004ECE6C
		[NullableContext(1)]
		private void CreateDatapackButton_Clicked(object sender, RoutedEventArgs e)
		{
			ProcessStartInfo PackCreator = new ProcessStartInfo("DatapackCreator.exe")
			{
				UseShellExecute = true,
				WindowStyle = ProcessWindowStyle.Hidden,
				CreateNoWindow = true
			};
			string InputFile = this.Datapack_InputPath.Text.Replace("\"", "");
			string OutputFile = this.Datapack_OutputFile.Text.Replace("\"", "");
			string WorldID = this.Datapack_WorldID.Text.Replace("\"", "");
			string WorldName = this.Datapack_WorldName.Text.Replace("\"", "");
			string WorldHash = this.Datapack_WorldHash.Text.Replace("\"", "");
			string Version = string.Concat(new string[]
			{
				this.Datapack_Version_Major.Text,
				".",
				this.Datapack_Version_Min.Text,
				".",
				this.Datapack_Version_Patch.Text
			});
			string Author = this.Datapack_Author.Text.Replace("\"", "");
			this.CreateDatapack_Button.IsEnabled = false;
			Func<bool> <>9__1;
			new Thread(delegate()
			{
				ProcessStartInfo packCreator = PackCreator;
				packCreator.Arguments = packCreator.Arguments + "\"" + InputFile + "\" ";
				ProcessStartInfo packCreator2 = PackCreator;
				packCreator2.Arguments = packCreator2.Arguments + "\"" + OutputFile + "\" ";
				ProcessStartInfo packCreator3 = PackCreator;
				packCreator3.Arguments = packCreator3.Arguments + "\"" + WorldID + "\" ";
				ProcessStartInfo packCreator4 = PackCreator;
				packCreator4.Arguments = packCreator4.Arguments + "\"" + WorldName + "\" ";
				ProcessStartInfo packCreator5 = PackCreator;
				packCreator5.Arguments = packCreator5.Arguments + "\"" + WorldHash + "\" ";
				ProcessStartInfo packCreator6 = PackCreator;
				packCreator6.Arguments = packCreator6.Arguments + "\"" + Version + "\" ";
				ProcessStartInfo packCreator7 = PackCreator;
				packCreator7.Arguments = packCreator7.Arguments + "\"" + Author + "\" ";
				Process process = Process.Start(PackCreator);
				if (process != null)
				{
					process.WaitForExit();
				}
				Process.Start(new ProcessStartInfo
				{
					FileName = "explorer.exe",
					Arguments = "/select,\"" + OutputFile + "\"",
					UseShellExecute = true
				});
				Dispatcher dispatcher = System.Windows.Application.Current.Dispatcher;
				Func<bool> callback;
				if ((callback = <>9__1) == null)
				{
					callback = (<>9__1 = (() => this.CreateDatapack_Button.IsEnabled = true));
				}
				dispatcher.Invoke<bool>(callback);
			})
			{
				IsBackground = true,
				Priority = ThreadPriority.BelowNormal
			}.Start();
		}

		// Token: 0x06000064 RID: 100 RVA: 0x004ECFE8 File Offset: 0x004ECFE8
		[NullableContext(1)]
		private void PatchGame_Click(object sender, RoutedEventArgs e)
		{
			this.VRCFX_Panel.Visibility = Visibility.Collapsed;
			this.VRCSpoofer_Panel.Visibility = Visibility.Collapsed;
			this.Datapacks_Panel.Visibility = Visibility.Collapsed;
			this.Splashscreen_Panel.Visibility = Visibility.Collapsed;
			this.Settings_Panel.Visibility = Visibility.Collapsed;
			this.DatapackCreator_Panel.Visibility = Visibility.Collapsed;
			this.Patch_Panel.Visibility = Visibility.Visible;
			this.MelonLoader_Panel.Visibility = Visibility.Collapsed;
		}

		// Token: 0x06000065 RID: 101 RVA: 0x004ED058 File Offset: 0x004ED058
		private void SetupEvents()
		{
			VRCPatch.OnPatchKeyStarted += delegate()
			{
				System.Windows.Application.Current.Dispatcher.Invoke(delegate()
				{
					this.Patch_Status.Content = "Validating Key";
					this.Patch_Key.IsEnabled = false;
					this.PatchGame_Button.IsEnabled = false;
					this.UnpatchGame_Button.IsEnabled = false;
				});
			};
			VRCPatch.OnPatchKeySuccess += delegate()
			{
				System.Windows.Application.Current.Dispatcher.Invoke(delegate()
				{
					this.Patch_Status.Content = "Valid Key";
				});
			};
			VRCPatch.OnPatchDownloadStarted += delegate()
			{
				System.Windows.Application.Current.Dispatcher.Invoke(delegate()
				{
					this.Patch_Status.Content = "Downloading Patch";
				});
			};
			VRCPatch.OnPatchKeyFail += delegate()
			{
				System.Windows.Application.Current.Dispatcher.Invoke(delegate()
				{
					this.Patch_Status.Content = "Invalid Key";
					this.Patch_Key.IsEnabled = true;
				});
			};
			VRCPatch.OnPatchDownloaded += delegate()
			{
				System.Windows.Application.Current.Dispatcher.Invoke(delegate()
				{
					this.Patch_Status.Content = "Installing Patch";
				});
			};
			VRCPatch.OnPatchInstall += delegate()
			{
				System.Windows.Application.Current.Dispatcher.Invoke(delegate()
				{
					this.Patch_Status.Content = "Patch Installed";
					this.Patch_Key.IsEnabled = true;
					this.PatchGame_Button.IsEnabled = true;
					this.UnpatchGame_Button.IsEnabled = true;
				});
			};
		}

		// Token: 0x06000066 RID: 102 RVA: 0x004ED0CC File Offset: 0x004ED0CC
		[NullableContext(1)]
		private void PatchGame_Button_Click(object sender, RoutedEventArgs e)
		{
			MainWindow.<>c__DisplayClass57_0 CS$<>8__locals1 = new MainWindow.<>c__DisplayClass57_0();
			CS$<>8__locals1.<>4__this = this;
			this.PatchGame_Button.IsEnabled = false;
			CS$<>8__locals1.key = this.Patch_Key.Text.Trim();
			if (string.IsNullOrEmpty(CS$<>8__locals1.key))
			{
				return;
			}
			new Thread(delegate()
			{
				MainWindow.<>c__DisplayClass57_0.<<PatchGame_Button_Click>b__0>d <<PatchGame_Button_Click>b__0>d;
				<<PatchGame_Button_Click>b__0>d.<>t__builder = AsyncVoidMethodBuilder.Create();
				<<PatchGame_Button_Click>b__0>d.<>4__this = CS$<>8__locals1;
				<<PatchGame_Button_Click>b__0>d.<>1__state = -1;
				<<PatchGame_Button_Click>b__0>d.<>t__builder.Start<MainWindow.<>c__DisplayClass57_0.<<PatchGame_Button_Click>b__0>d>(ref <<PatchGame_Button_Click>b__0>d);
			}).Start();
		}

		// Token: 0x06000067 RID: 103 RVA: 0x004ED12C File Offset: 0x004ED12C
		[NullableContext(1)]
		private void UnpatchGame_Button_Click(object sender, RoutedEventArgs e)
		{
		}

		// Token: 0x06000068 RID: 104 RVA: 0x004ED130 File Offset: 0x004ED130
		[NullableContext(1)]
		private void Patch_Key_TextChanged(object sender, TextChangedEventArgs e)
		{
			this.PatchGame_Button.IsEnabled = true;
			this.UnpatchGame_Button.IsEnabled = true;
			this.Patch_Status.Content = "";
		}

		// Token: 0x06000069 RID: 105 RVA: 0x004ED15C File Offset: 0x004ED15C
		[NullableContext(1)]
		private void MelonLoader_Uninstall_Button_Click(object sender, RoutedEventArgs e)
		{
			this.MelonLoader_Button.IsEnabled = false;
			this.MelonLoaderUninstall_Button.IsEnabled = false;
			new Thread(delegate()
			{
				List<Process> list = new List<Process>();
				list.AddRange(Process.GetProcessesByName("VRChat"));
				list.AddRange(Process.GetProcessesByName("start_protected_game"));
				foreach (Process process in list)
				{
					try
					{
						process.Kill();
					}
					catch
					{
					}
					Thread.Sleep(150);
				}
				Thread.Sleep(100);
				try
				{
					File.Delete(Path.Combine(new FileInfo(Config.VRC_Path).Directory.FullName, "Version.dll"));
					System.Windows.MessageBox.Show("Uninstalled Melon Loader Sucessfully");
				}
				catch
				{
				}
				System.Windows.Application.Current.Dispatcher.Invoke<bool>(() => this.MelonLoader_Button.IsEnabled = true);
				System.Windows.Application.Current.Dispatcher.Invoke<bool>(() => this.MelonLoaderUninstall_Button.IsEnabled = true);
			}).Start();
		}

		// Token: 0x0600006A RID: 106 RVA: 0x004ED18C File Offset: 0x004ED18C
		[NullableContext(1)]
		private void MelonLoader_Page_Click(object sender, RoutedEventArgs e)
		{
			this.VRCFX_Panel.Visibility = Visibility.Collapsed;
			this.VRCSpoofer_Panel.Visibility = Visibility.Collapsed;
			this.Datapacks_Panel.Visibility = Visibility.Collapsed;
			this.Splashscreen_Panel.Visibility = Visibility.Collapsed;
			this.Settings_Panel.Visibility = Visibility.Collapsed;
			this.DatapackCreator_Panel.Visibility = Visibility.Collapsed;
			this.Patch_Panel.Visibility = Visibility.Collapsed;
			this.MelonLoader_Panel.Visibility = Visibility.Visible;
			new Thread(delegate()
			{
				if (!this.MelonDebounce)
				{
					return;
				}
				this.MelonDebounce = false;
				System.Windows.Application.Current.Dispatcher.Invoke<object>(() => this.MelonLoaderStatus.Content = "Loading..");
				string Status = new HttpClient().GetStringAsync(MainWindow.GetServer("https://software.vrchub.site/VRCMelon/Status")).GetAwaiter().GetResult();
				System.Windows.Application.Current.Dispatcher.Invoke<object>(() => this.MelonLoaderStatus.Content = Status);
				Thread.Sleep(1500);
				this.MelonDebounce = true;
			}).Start();
		}

		// Token: 0x0600006B RID: 107 RVA: 0x004ED210 File Offset: 0x004ED210
		[NullableContext(1)]
		private void MelonLoader_Button_Click(object sender, RoutedEventArgs e)
		{
			new Thread(delegate()
			{
				MainWindow.<>c.<<MelonLoader_Button_Click>b__63_0>d <<MelonLoader_Button_Click>b__63_0>d;
				<<MelonLoader_Button_Click>b__63_0>d.<>t__builder = AsyncVoidMethodBuilder.Create();
				<<MelonLoader_Button_Click>b__63_0>d.<>1__state = -1;
				<<MelonLoader_Button_Click>b__63_0>d.<>t__builder.Start<MainWindow.<>c.<<MelonLoader_Button_Click>b__63_0>d>(ref <<MelonLoader_Button_Click>b__63_0>d);
			}).Start();
		}

		// Token: 0x0400001D RID: 29
		[Nullable(2)]
		private Process VRCQuickLauncher;

		// Token: 0x0400001E RID: 30
		[Nullable(1)]
		private static readonly Random random = new Random();

		// Token: 0x0400001F RID: 31
		private const ushort controlHeight = 195;

		// Token: 0x04000020 RID: 32
		private const ushort controlWidth = 250;

		// Token: 0x04000021 RID: 33
		private const ushort verticalSpacing = 10;

		// Token: 0x04000022 RID: 34
		private const ushort horizontalSpacing = 5;

		// Token: 0x04000023 RID: 35
		private const ushort initialTop = 20;

		// Token: 0x04000024 RID: 36
		private const ushort initialLeft = 10;

		// Token: 0x04000025 RID: 37
		private const byte controlsPerRow = 3;

		// Token: 0x04000026 RID: 38
		[Nullable(2)]
		private SplashScreen _splashScreen;

		// Token: 0x04000027 RID: 39
		[Nullable(1)]
		private readonly Dictionary<System.Windows.Controls.Button, MainWindow.ButtonData> ButtonToggles = new Dictionary<System.Windows.Controls.Button, MainWindow.ButtonData>();

		// Token: 0x04000028 RID: 40
		[Nullable(2)]
		private static ServerAPI api;

		// Token: 0x04000029 RID: 41
		private bool ServerInitilized;

		// Token: 0x0400002A RID: 42
		private bool MelonDebounce = true;

		// Token: 0x0200000C RID: 12
		[NullableContext(1)]
		[Nullable(0)]
		private class ButtonData
		{
			// Token: 0x06000082 RID: 130 RVA: 0x004EDDE4 File Offset: 0x004EDDE4
			public ButtonData(string text, bool paused)
			{
			}

			// Token: 0x04000066 RID: 102
			public string BaseText = text;

			// Token: 0x04000067 RID: 103
			public bool Paused = paused;
		}

		// Token: 0x0200000D RID: 13
		[NullableContext(1)]
		[Nullable(0)]
		public class PackageJson
		{
			// Token: 0x17000013 RID: 19
			// (get) Token: 0x06000083 RID: 131 RVA: 0x004EDDFC File Offset: 0x004EDDFC
			// (set) Token: 0x06000084 RID: 132 RVA: 0x004EDE04 File Offset: 0x004EDE04
			public string Name { get; set; } = "";

			// Token: 0x17000014 RID: 20
			// (get) Token: 0x06000085 RID: 133 RVA: 0x004EDE10 File Offset: 0x004EDE10
			// (set) Token: 0x06000086 RID: 134 RVA: 0x004EDE18 File Offset: 0x004EDE18
			public string Version { get; set; } = "";

			// Token: 0x17000015 RID: 21
			// (get) Token: 0x06000087 RID: 135 RVA: 0x004EDE24 File Offset: 0x004EDE24
			// (set) Token: 0x06000088 RID: 136 RVA: 0x004EDE2C File Offset: 0x004EDE2C
			public bool Active { get; set; }
		}
	}
}
