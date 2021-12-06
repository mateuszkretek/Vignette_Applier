import cv2
import numpy as np
#from matplotlib import pyplot as plt


img = cv2.imread('../turtle.jpg')
rows, cols = img.shape[:2]
zeros = np.copy(img)
zeros[:, :, :] = 0
a = cv2.getGaussianKernel(cols, 300)
b = cv2.getGaussianKernel(rows, 300)
c = b*a.T
d = c/c.max()
zeros[:, :, 0] = img[:, :, 0]*d
zeros[:, :, 1] = img[:, :, 1]*d
zeros[:, :, 2] = img[:, :, 2]*d
cv2.imwrite('../vig_python.png', zeros)
	

