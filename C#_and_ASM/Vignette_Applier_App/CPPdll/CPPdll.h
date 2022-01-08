#pragma once

extern "C" double dist(double first_point_x, double first_point_y, double second_point_x, double second_point_y);

extern "C" double getMaxDistFromCenter(double size_x, double size_y, double center_x, double center_y);

extern "C" double calculateMaskValue(double maskCenterX, double maskCenterY, double col, double row, double stdMultiplier, double maskPower);