using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Vignette_Applier_App
{
	class Apply_Vignette
	{
		//[DllImport("C:/Users/mateu/Desktop/Vignette_Applier_App/x64/Debug/ASMdll.dll")]
		//[DllImport("C:/Users/mateu/Desktop/Vignette_Applier_App/x64/Debug/CPPdll.dll")]
		//private static extern double dist(double first_point_x, double first_point_y, double second_point_x, double second_point_y);
		
		//[DllImport("C:/Users/mateu/Desktop/Vignette_Applier_App/x64/Debug/ASMdll.dll")]
		[DllImport("C:/Users/mateu/Desktop/Vignette_Applier_App/x64/Debug/CPPdll.dll")]
		private static extern double getMaxDistFromCenter(double size_x, double size_y, double center_x, double center_y);

		//[DllImport("C:/Users/mateu/Desktop/Vignette_Applier_App/x64/Debug/ASMdll.dll")]
		[DllImport("C:/Users/mateu/Desktop/Vignette_Applier_App/x64/Debug/CPPdll.dll")]
		private static extern double calculateMaskValue(double maskCenterX, double maskCenterY, double col, double row, double stdMultiplier, double maskPower);

			public static Bitmap ApplyVignette(Bitmap inputImage, double maskCenterX, double maskCenterY, double maskPower) {
			double maxDistFromCenter = getMaxDistFromCenter(inputImage.Width, inputImage.Height, maskCenterX, maskCenterY);
			double stdMultiplier = (Math.PI / 2) / maxDistFromCenter;
			double temp;
			Bitmap outputImage = new Bitmap(inputImage);
			List<List<double>> mask = new List<List<double>>();
			for (int row = 0; row < outputImage.Height; row++) {
				mask.Add(new List<double>());
				for (int col = 0; col < outputImage.Width; col++) {
					temp = calculateMaskValue(maskCenterX, maskCenterY, col, row, stdMultiplier, maskPower);
					mask[row].Add(temp);
				}
			}

			for (int row = 0; row < outputImage.Height; row++)
			{
				for (int col = 0; col < outputImage.Width; col++)
				{
					Color outputColor = outputImage.GetPixel(col, row);
					outputImage.SetPixel(col, row, Color.FromArgb(255,
																(byte)(outputColor.R * mask[row][col]),
																(byte)(outputColor.G * mask[row][col]),
																(byte)(outputColor.B * mask[row][col])));
				}
			}

			return outputImage;
		}
	}
}
