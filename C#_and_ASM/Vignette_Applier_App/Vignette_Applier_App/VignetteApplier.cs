using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Vignette_Applier_App {
	class VignetteApplier {
		private class TaskParams
        {
			public double maskCenterX;
			public double maskCenterY;
			public int col;
			public int row;
			public double stdMultiplier;
			public int maskPower;
			public double[] mask;
			public int imageHeight;
		}

		private static double getMaxDistFromCenter(double sizeX, double sizeY, double centerX, double centerY)
		{
			Tuple<double, double>[] corners = new Tuple<double, double>[4];
			corners[0] = new Tuple<double, double>(0, 0);
			corners[1] = new Tuple<double, double>(sizeX, 0);
			corners[2] = new Tuple<double, double>(0, sizeY);
			corners[3] = new Tuple<double, double>(sizeX, sizeY);
			double maxDist = 0;
			for (int i = 0; i < 4; ++i)
			{
				double dist = Math.Sqrt(Math.Pow((centerX- corners[i].Item1),2)+Math.Pow((centerY-corners[i].Item2),2));
				if (maxDist < dist)
					maxDist = dist;
			}
			return maxDist;
		}

		[DllImport(@"../../../../x64/Debug/ASMdll.dll")]
		public static extern double calculateMaskValueAsm(double maskCenterX, double maskCenterY, double col, double row, double stdMultiplier, int maskPower);
		[DllImport(@"../../../../x64/Debug/CPPdll.dll")]
		public static extern double calculateMaskValueCpp(double maskCenterX, double maskCenterY, double col, double row, double stdMultiplier, int maskPower);

		public static Tuple<Bitmap, double> ApplyVignette(Bitmap inputImage, double maskCenterX, double maskCenterY, int maskPower, int threads, int dllParam)
		{
			var countdownEvent = new CountdownEvent(inputImage.Height * inputImage.Width);
			double maxDistFromCenter = VignetteApplier.getMaxDistFromCenter(inputImage.Width, inputImage.Height, maskCenterX, maskCenterY);
			Action<Object> threadFunctionWrapper = null;
            if (dllParam == 0)
            {
				threadFunctionWrapper = threadParams =>
				{
					TaskParams castedParams = (TaskParams)threadParams;
					int temp = castedParams.col * castedParams.imageHeight + castedParams.row;
					castedParams.mask[temp] = calculateMaskValueAsm(castedParams.maskCenterX,
														castedParams.maskCenterY,
														castedParams.col,
														castedParams.row,
														castedParams.stdMultiplier,
														castedParams.maskPower
														);
					countdownEvent.Signal();
				};
			}
            else if(dllParam == 1)
            {
                threadFunctionWrapper = threadParams =>
                {
                    TaskParams castedParams = (TaskParams)threadParams;
                    int temp = castedParams.col * castedParams.imageHeight + castedParams.row;
                    castedParams.mask[temp] = calculateMaskValueCpp(castedParams.maskCenterX,
                                                        castedParams.maskCenterY,
                                                        castedParams.col,
                                                        castedParams.row,
                                                        castedParams.stdMultiplier,
                                                        castedParams.maskPower
                                                        );
                    countdownEvent.Signal();
                };
            }
            Bitmap outputImage = new Bitmap(inputImage);
			double[] mask = new double[inputImage.Width * inputImage.Height];
			ThreadPool.SetMinThreads(threads, threads);
			ThreadPool.SetMaxThreads(threads, threads);
			var watch = System.Diagnostics.Stopwatch.StartNew();
			for (int row = 0; row < outputImage.Height; row++)
			{
				for (int col = 0; col < outputImage.Width; col++)
				{
					TaskParams taskParams = new TaskParams();
					taskParams.maskCenterX = maskCenterX;
					taskParams.maskCenterY = maskCenterY;
					taskParams.stdMultiplier = (Math.PI / 2) / maxDistFromCenter;
					taskParams.maskPower = maskPower;
					taskParams.mask = mask;
					taskParams.imageHeight = inputImage.Height;
					taskParams.col = col;
					taskParams.row = row;
					ThreadPool.QueueUserWorkItem(new WaitCallback(threadFunctionWrapper), taskParams);
                }
			}
			countdownEvent.Wait();
			watch.Stop();
			for (int row = 0; row < outputImage.Height; row++)
			{
				for (int col = 0; col < outputImage.Width; col++)
				{
					Color outputColor = outputImage.GetPixel(col, row);
					int temp = col * outputImage.Height + row;
					outputImage.SetPixel(col, row, Color.FromArgb(255,
																(byte)(outputColor.R * mask[temp]),
																(byte)(outputColor.G * mask[temp]),
																(byte)(outputColor.B * mask[temp])));
				}
			}
			return new Tuple<Bitmap,double>(outputImage, watch.ElapsedMilliseconds);
		}
	}
}
