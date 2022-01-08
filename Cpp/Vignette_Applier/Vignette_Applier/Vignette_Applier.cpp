#define _USE_MATH_DEFINES
#include <math.h>

#include <vector>

#include <opencv2/opencv.hpp>
#include <opencv2/highgui.hpp>
#include <opencv2/imgproc/types_c.h>


// Helper function to calculate the distance between 2 points.
double dist(double first_point_x, double first_point_y, double second_point_x, double second_point_y)
{
	return sqrt(pow((first_point_x - second_point_x), 2) + pow((first_point_y - second_point_y), 2));
}

// Helper function that computes the longest distance from the edge to the center point.
double getMaxDistFromCenter(double size_x, double size_y, double center_x, double center_y) {
	// given a rect and a line
	// get which corner of rect is farthest from the line
	std::vector<std::pair<double,double>> corners(4);
	corners[0] = std::pair<double,double>(0, 0);
	corners[1] = std::pair<double, double>(size_x, 0);
	corners[2] = std::pair<double, double>(0, size_y);
	corners[3] = std::pair<double, double>(size_x, size_y);
	double maxDis = 0;
	for (int i = 0; i < 4; ++i) {
		double dis = dist(center_x, center_y, corners[i].first,corners[i].second);
		if (maxDis < dis)
			maxDis = dis;
	}
	return maxDis;
}

// Helper function that creates a gradient image.
void generateGradient(cv::Mat& mask) {
	const double mask_center_x = mask.cols / 2;	//wyznaczenie środka winiety
	const double mask_center_y = mask.rows / 2;
	const double mask_size_x = mask.cols;			//wyznaczenie rozmiaru winiety
	const double mask_size_y = mask.rows;
	const double power = 2.0;						//moc winiety
	const double maxDistFromCenter = getMaxDistFromCenter(mask_size_x, mask_size_y, mask_center_x, mask_center_y);	//odległość najdalszego punktu od środka winiety
	const double std_multiplier = M_PI_2 / maxDistFromCenter;
	double temp;
	for (int row = 0; row < mask.rows; row++) {
		for (int col = 0; col < mask.cols; col++) {
			temp = dist(mask_center_x, mask_center_y, col, row);	//generowanie winiety
			temp *= std_multiplier;
			temp = cos(temp);
			temp = pow(temp, power);
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

	cv::imwrite("gradient.jpg", mask_image);
	
	//cv::namedWindow("Mask", cv::WINDOW_NORMAL);
	//cv::resizeWindow("Mask", mask_image.cols/2, mask_image.rows/2);
	//cv::imshow("Mask", mask_image);
	//cv::waitKey();
	

	cv::Mat output_image(input_image);
	for (int row = 0; row < output_image.rows; row++) {
		for (int col = 0; col < output_image.cols; col++) {
			output_image.at<cv::Vec3b>(row, col)[0] *= mask_image.at<double>(row, col);
			output_image.at<cv::Vec3b>(row, col)[1] *= mask_image.at<double>(row, col);
			output_image.at<cv::Vec3b>(row, col)[2] *= mask_image.at<double>(row, col);
		}
	}

	cv::imwrite("vignette_cpp.jpg", output_image);

	cv::namedWindow("Vignette", cv::WINDOW_NORMAL);
	cv::resizeWindow("Vignette", output_image.cols/2, output_image.rows/2);
	cv::imshow("Vignette", output_image);
	cv::waitKey();

	return 0;
}