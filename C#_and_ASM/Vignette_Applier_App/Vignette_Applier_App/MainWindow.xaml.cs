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
		public MainWindow()
		{
			InitializeComponent();
		}

		private void ASM_Button_Click(object sender, RoutedEventArgs e)
		{
		}

		private void Picture_Button_Click(object sender, RoutedEventArgs e)
		{
			Bitmap inputImage = new Bitmap("E:/Projects/Projects/Vignette_Applier/C#_and_ASM/Vignette_Applier_App/images/turtle.jpg");
			Tuple<Bitmap,double> result = Apply_Vignette.ApplyVignette(inputImage, inputImage.Width / 2, inputImage.Height / 2, 4, 6);
			BitmapImage bitmapimage = new BitmapImage();
			using (MemoryStream memory = new MemoryStream())
			{
				result.Item1.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
				memory.Position = 0;

				bitmapimage.BeginInit();
				bitmapimage.StreamSource = memory;
				bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
				bitmapimage.EndInit();
			}
			Image_Output.Source = bitmapimage;
			text1.Content = result.Item2;
		}
	}
}
