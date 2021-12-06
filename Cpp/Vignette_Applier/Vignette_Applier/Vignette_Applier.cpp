#include <math.h>

#include <vector>

#include <opencv2/opencv.hpp>
#include <opencv2/highgui.hpp>
#include <opencv2/imgproc/types_c.h>


// Helper function to calculate the distance between 2 points.
double dist(double first_point_x, double first_point_y, double second_point_x, double second_point_y)
{
	return sqrt(pow((double)(first_point_x - second_point_x), 2) + pow((double)(first_point_y - second_point_y), 2));
}

// Helper function that computes the longest distance from the edge to the center point.
double getMaxDisFromCorners(double size_x, double size_y, double center_x, double center_y)
{
	// given a rect and a line
	// get which corner of rect is farthest from the line

	std::vector<std::pair<double,double>> corners(4);
	corners[0] = std::pair<double,double>(0, 0);
	corners[1] = std::pair<double, double>(size_x, 0);
	corners[2] = std::pair<double, double>(0, size_y);
	corners[3] = std::pair<double, double>(size_x, size_y);

	double maxDis = 0;
	for (int i = 0; i < 4; ++i)
	{
		double dis = dist(center_x, center_y, corners[i].first,corners[i].second);
		if (maxDis < dis)
			maxDis = dis;
	}

	return maxDis;
}

// Helper function that creates a gradient image.
void generateGradient(cv::Mat& mask)
{
	double mask_center_x = mask.cols / 2;
	double mask_center_y = mask.rows / 2;

	double mask_size_x = mask.cols;
	double mask_size_y = mask.rows;

	double radius = 1.0;
	double power = 1.0;
	double maxImageRad = radius * getMaxDisFromCorners(mask_size_x, mask_size_y, mask_center_x, mask_center_y);
	double temp;

	for (int row = 0; row < mask.rows; row++) {
		for (int col = 0; col < mask.cols; col++) {
			temp = dist(mask_center_x, mask_center_y, col, row) / maxImageRad * power;
			temp = pow(cos(temp), 16);
			mask.at<double>(row, col) = temp;
		}
	}

}

// This is where the fun starts!
int main() {
	cv::Mat input_image = cv::imread("E:/Projects/Projects/Vignette_Applier/turtle.jpg");
	if (input_image.empty()) {
		std::cout << "!!! Failed imread\n";
		return -1;
	}

	/*
	cv::namedWindow("Original", cv::WINDOW_NORMAL);
	cv::resizeWindow("Original", img.size().width/2, img.size().height/2);
	cv::imshow("Original", img);
	*/

	cv::Mat mask_image(input_image.size(), CV_64F);
	//cv::Mat mask_image(400, 600, CV_64F);
	generateGradient(mask_image);

	//cv::imshow("gradient.png", mask_image);
	

	cv::Mat output_image(input_image);
	for (int row = 0; row < output_image.rows; row++) {
		for (int col = 0; col < output_image.cols; col++) {
			output_image.at<cv::Vec3b>(row, col)[0] *= mask_image.at<double>(row, col);
			output_image.at<cv::Vec3b>(row, col)[1] *= mask_image.at<double>(row, col);
			output_image.at<cv::Vec3b>(row, col)[2] *= mask_image.at<double>(row, col);
		}
	}
	cv::namedWindow("Vignette", cv::WINDOW_NORMAL);
	cv::resizeWindow("Vignette", output_image.cols/2, output_image.rows/2);
	cv::imshow("Vignette", output_image);
	cv::waitKey();

	return 0;
}