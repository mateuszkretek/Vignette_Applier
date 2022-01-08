#include "pch.h"
#include "CPPdll.h"
#include <math.h>
#include <vector>

double dist(double first_point_x, double first_point_y, double second_point_x, double second_point_y)
{
    return sqrt(pow((first_point_x - second_point_x), 2) + pow((first_point_y - second_point_y), 2));
}

double getMaxDistFromCenter(double size_x, double size_y, double center_x, double center_y)
{
	std::vector<std::pair<double, double>> corners(4);
	corners[0] = std::pair<double, double>(0, 0);
	corners[1] = std::pair<double, double>(size_x, 0);
	corners[2] = std::pair<double, double>(0, size_y);
	corners[3] = std::pair<double, double>(size_x, size_y);
	double maxDis = 0;
	for (int i = 0; i < 4; ++i) {
		double dis = dist(center_x, center_y, corners[i].first, corners[i].second);
		if (maxDis < dis)
			maxDis = dis;
	}
	return maxDis;
}

double calculateMaskValue(double maskCenterX, double maskCenterY, double col, double row, double stdMultiplier, double maskPower)
{
	double temp;
	temp = dist(maskCenterX, maskCenterY, col, row);    //generowanie winiety
	temp *= stdMultiplier;
	temp = cos(temp);
	temp = pow(temp, maskPower);
	return temp;
}
