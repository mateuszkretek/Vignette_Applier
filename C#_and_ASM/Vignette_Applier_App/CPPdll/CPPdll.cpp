#include "pch.h"
#include "CPPdll.h"
#include <math.h>

double calculateDistance(double first_point_x, double first_point_y, double second_point_x, double second_point_y)
{
    return sqrt(pow((first_point_x - second_point_x), 2) + pow((first_point_y - second_point_y), 2));
}

double calculateMaskValueCpp(double maskCenterX, double maskCenterY, double col, double row, double stdMultiplier, int maskPower)
{
	double temp;
	temp = calculateDistance(maskCenterX, maskCenterY, col, row);   //obliczanie odległości danego punktu od centrum winiety
	temp *= stdMultiplier;											//zmiana wartości na należącą do przedziału <0,Pi/2>
	temp = cos(temp);												//obliczenie cosinusa (wlaściwe generowanie winiety)
	temp = pow(temp, maskPower);									//ustawienie mocy winiety
	return temp;
}

