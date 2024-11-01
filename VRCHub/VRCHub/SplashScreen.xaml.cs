using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Threading;

namespace VRCHub
{
	// Token: 0x0200002C RID: 44
	[NullableContext(1)]
	[Nullable(0)]
	public partial class SplashScreen : Window, IDisposable
	{
		// Token: 0x060000E2 RID: 226 RVA: 0x004F072C File Offset: 0x004F072C
		private SplashScreen()
		{
			base.Hide();
			this.InitializeComponent();
			base.BringIntoView();
			base.Topmost = true;
			this._textAnimationTimer = new DispatcherTimer
			{
				Interval = TimeSpan.FromMilliseconds(300.0)
			};
			this._textAnimationTimer.Tick += this.OnTextAnimationTick;
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x004F07B8 File Offset: 0x004F07B8
		public static SplashScreen Create()
		{
			object @lock = SplashScreen._lock;
			SplashScreen instance;
			lock (@lock)
			{
				if (SplashScreen._instance != null)
				{
					instance = SplashScreen._instance;
				}
				else
				{
					SplashScreen._instance = new SplashScreen();
					if (File.Exists(Config.VRC_Path))
					{
						SplashScreen._instance.Splash.Source = Common.GetImageSource(File.ReadAllBytes(SplashscreenEditor.SplashScreenPath));
					}
					else
					{
						SplashScreen._instance.Splash.Source = Common.GetImageSource(AppResources.SplashScreen);
					}
					SplashScreen._instance.MainText.Text = SplashScreen.BaseText;
					SplashScreen._instance.StartFadeIn();
					SplashScreen._instance.Show();
					instance = SplashScreen._instance;
				}
			}
			return instance;
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x004F0884 File Offset: 0x004F0884
		public Task EndAsync()
		{
			SplashScreen.<EndAsync>d__9 <EndAsync>d__;
			<EndAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<EndAsync>d__.<>4__this = this;
			<EndAsync>d__.<>1__state = -1;
			<EndAsync>d__.<>t__builder.Start<SplashScreen.<EndAsync>d__9>(ref <EndAsync>d__);
			return <EndAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x004F08C8 File Offset: 0x004F08C8
		public void StartFadeIn()
		{
			DoubleAnimation doubleAnimation = new DoubleAnimation
			{
				From = new double?(0.0),
				To = new double?((double)1),
				Duration = this._fadeInDuration
			};
			doubleAnimation.Completed += delegate([Nullable(2)] object s, EventArgs e)
			{
				this.OnFadeInCompleted();
			};
			base.BeginAnimation(UIElement.OpacityProperty, doubleAnimation);
			DoubleAnimation animation = new DoubleAnimation
			{
				From = new double?((double)20),
				To = new double?(0.0),
				Duration = TimeSpan.FromSeconds(1.0)
			};
			this.blureffect.BeginAnimation(BlurEffect.RadiusProperty, animation);
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x004F0980 File Offset: 0x004F0980
		private void OnFadeInCompleted()
		{
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x004F0984 File Offset: 0x004F0984
		public void StartFadeOut()
		{
			DoubleAnimation animation = new DoubleAnimation
			{
				From = new double?((double)1),
				To = new double?(0.0),
				Duration = this._fadeOutDuration
			};
			base.BeginAnimation(UIElement.OpacityProperty, animation);
			DoubleAnimation animation2 = new DoubleAnimation
			{
				From = new double?(0.0),
				To = new double?((double)30),
				Duration = TimeSpan.FromSeconds(0.5)
			};
			this.blureffect.BeginAnimation(BlurEffect.RadiusProperty, animation2);
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x004F0A28 File Offset: 0x004F0A28
		public void SetText(string Text)
		{
			Application.Current.Dispatcher.Invoke<string>(() => this.MainText.Text = Text);
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x004F0A68 File Offset: 0x004F0A68
		public void StartTextAnimation()
		{
			this._textAnimationState = 0;
			this._textAnimationTimer.Start();
		}

		// Token: 0x060000EA RID: 234 RVA: 0x004F0A7C File Offset: 0x004F0A7C
		public void EndTextAnimation()
		{
			this._textAnimationTimer.Stop();
			this.MainText.Text = SplashScreen.BaseText;
		}

		// Token: 0x060000EB RID: 235 RVA: 0x004F0A9C File Offset: 0x004F0A9C
		private void OnTextAnimationTick([Nullable(2)] object sender, EventArgs e)
		{
			this._textAnimationState = (this._textAnimationState + 1) % 4;
			this.MainText.Text = SplashScreen.BaseText + new string('.', this._textAnimationState);
		}

		// Token: 0x060000EC RID: 236 RVA: 0x004F0AD0 File Offset: 0x004F0AD0
		private void SplashScreen_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.ChangedButton == MouseButton.Left)
			{
				base.DragMove();
			}
		}

		// Token: 0x060000ED RID: 237 RVA: 0x004F0AE0 File Offset: 0x004F0AE0
		public void Dispose()
		{
			if (SplashScreen._instance != null)
			{
				SplashScreen._instance.Close();
				SplashScreen._instance = null;
			}
		}

		// Token: 0x060000EE RID: 238 RVA: 0x004F0AFC File Offset: 0x004F0AFC
		void IDisposable.Dispose()
		{
			this.Dispose();
		}

		// Token: 0x040000DC RID: 220
		[Nullable(2)]
		private static SplashScreen _instance;

		// Token: 0x040000DD RID: 221
		private static readonly object _lock = new object();

		// Token: 0x040000DE RID: 222
		private readonly TimeSpan _fadeInDuration = TimeSpan.FromSeconds(0.75);

		// Token: 0x040000DF RID: 223
		private readonly TimeSpan _fadeOutDuration = TimeSpan.FromSeconds(0.5);

		// Token: 0x040000E0 RID: 224
		private readonly DispatcherTimer _textAnimationTimer;

		// Token: 0x040000E1 RID: 225
		private int _textAnimationState;

		// Token: 0x040000E2 RID: 226
		internal static string BaseText = "INITIALIZING COMPONENTS";
	}
}
