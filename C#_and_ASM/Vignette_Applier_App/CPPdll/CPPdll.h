#pragma once

extern "C" double calculateDistance(double first_point_x, double first_point_y, double second_point_x, double second_point_y);

extern "C" double calculateMaskValueCpp(double maskCenterX, double maskCenterY, double col, double row, double stdMultiplier, int maskPower);