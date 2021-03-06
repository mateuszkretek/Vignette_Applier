using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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

namespace Vignette_Applier_App
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		Bitmap inputImage;
		int dllParam;
		double horizontalCenterMultiplier;
		double verticalCenterMultiplier;
		int threads = Environment.ProcessorCount;
		int power;

		public MainWindow()
		{
			InitializeComponent();
			inputImage = new Bitmap(@"../../../../images/turtle.jpg");
			BitmapImage inputBitmapImage = new BitmapImage();
			using (MemoryStream memory = new MemoryStream())
			{
				inputImage.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
				memory.Position = 0;
				inputBitmapImage.BeginInit();
				inputBitmapImage.StreamSource = memory;
				inputBitmapImage.CacheOption = BitmapCacheOption.OnLoad;
				inputBitmapImage.EndInit();
			}
			ImageInput.Source = null;
			ImageInput.Source = inputBitmapImage;
			ThreadsSlider.Value = threads;
		}
		private void Run_Button_Click(object sender, RoutedEventArgs e)
		{
			Tuple<Bitmap,double> result = VignetteApplier.ApplyVignette(inputImage, inputImage.Width * horizontalCenterMultiplier, inputImage.Height * verticalCenterMultiplier, power, threads, dllParam);
			BitmapImage outputBitmapImage = new BitmapImage();
			using (MemoryStream memory = new MemoryStream())
			{
				result.Item1.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
				memory.Position = 0;
				outputBitmapImage.BeginInit();
				outputBitmapImage.StreamSource = memory;
				outputBitmapImage.CacheOption = BitmapCacheOption.OnLoad;
				outputBitmapImage.EndInit();
			}
			ImageOutput.Source = outputBitmapImage;
			TimeLabel.Content = "Time: " + result.Item2 + " ms";
		}

		private void ThreadsSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			threads = (int)ThreadsSlider.Value;
			ThreadsLabel.Content = "Threads: " + threads;
		}

		private void VignettePowerSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			power = (int)VignettePowerSlider.Value;
			VignettePowerLabel.Content = "Power: " + power;
		}

        private void AsmRadioButton_Checked(object sender, RoutedEventArgs e)
        {
			dllParam = 0;
		}

        private void CppRadioButton_Checked(object sender, RoutedEventArgs e)
        {
			dllParam = 1;
        }

        private void HorizontalCenterSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
			horizontalCenterMultiplier = Math.Round((HorizontalCenterSlider.Value / 100),2);
			HorizontalCenterLabel.Content = "Horizontal: " + horizontalCenterMultiplier;
        }

        private void VerticalCenterSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
			verticalCenterMultiplier = Math.Round((VerticalCenterSlider.Value / 100),2);
			VerticalCenterLabel.Content = "Vertical: " + verticalCenterMultiplier;
        }
    }
}
