import cv2
import numpy as np
from matplotlib import pyplot as plt

img = cv2.imread('temp.jpg', 0)
rows, cols = img.shape

a = cv2.getGaussianKernel(cols, 300)
b = cv2.getGaussianKernel(rows, 300)
c = b*a.T
d = c/c.max()
e = img*d

cv2.imwrite('vig2.png', e)
