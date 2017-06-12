using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AudioSwitcher.AudioApi.CoreAudio;

namespace Volume_Changer
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		bool isCounting = true;
		int currentVolume;

		System.Timers.Timer timer;
		CoreAudioDevice audioDevice;
		public MainWindow()
		{
			InitializeComponent();
			audioDevice = new CoreAudioController().DefaultPlaybackDevice;
			currentVolume = (int)audioDevice.Volume;
			deadHorse.Play();
			timer = new System.Timers.Timer(33);
			timer.Elapsed += onTimer;
			timer.AutoReset = true;
			timer.Enabled = true;
			timer.Start();
		}

		private void deadHorse_MediaEnded(object sender, RoutedEventArgs e)
		{
			deadHorse.Position = new TimeSpan(0, 0, 1);
			deadHorse.Play();
		}

		private void onTimer(object source, System.Timers.ElapsedEventArgs e)
		{
			currentVolume++;
			currentVolume = currentVolume > 100 ? 0 : currentVolume;
			audioDevice.Volume = currentVolume;
			this.Dispatcher.Invoke(() =>
			{
				label.Content = String.Format("{0}", currentVolume);
			});
		}

		private void button_Click(object sender, RoutedEventArgs e)
		{
			if (isCounting)
			{
				deadHorse.Stop();
				isCounting = false;
				button.Content = "Start";
				timer.Stop();
			}
			else
			{
				//deadHorse.Position = new TimeSpan(0, 0, 1);
				deadHorse.Play();
				isCounting = true;
				button.Content = "Stop";
				timer.Start();
			}
		}
	}
}
