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
		int threads;
		int power;

		public MainWindow()
		{
			InitializeComponent();
			inputImage = new Bitmap("E:/Projects/Projects/Vignette_Applier/C#_and_ASM/Vignette_Applier_App/images/turtle.jpg");
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
		}
		private void Picture_Button_Click(object sender, RoutedEventArgs e)
		{
			Tuple<Bitmap,double> result = VignetteApplier.ApplyVignette(inputImage, inputImage.Width / 2, inputImage.Height / 2, power, threads);
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
			text1.Content = result.Item2;
		}

		private void ThreadsSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			threads = (int)ThreadsSlider.Value;
		}

		private void VignettePowerSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			power = (int)VignettePowerSlider.Value;
		}
	}
}
