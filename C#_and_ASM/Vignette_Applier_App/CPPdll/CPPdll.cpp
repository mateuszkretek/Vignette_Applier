#include "pch.h"
#include "CPPdll.h"
#include <math.h>

double calculateDistance(double first_point_x, double first_point_y, double second_point_x, double second_point_y)
{
    return sqrt(pow((first_point_x - second_point_x), 2) + pow((first_point_y - second_point_y), 2));
}

double calculateMaskValue(double maskCenterX, double maskCenterY, double col, double row, double stdMultiplier, double maskPower)
{
	double temp;
	temp = calculateDistance(maskCenterX, maskCenterY, col, row);    //generowanie winiety
	temp *= stdMultiplier;
	temp = cos(temp);
	temp = pow(temp, maskPower);
	return temp;
}
