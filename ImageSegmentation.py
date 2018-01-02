import numpy as np
import cv2

# Load an color image in grayscale
img = cv2.imread('./water_coins.jpg')
gray = cv2.cvtColor(img,cv2.COLOR_BGR2GRAY)
ret, thresh = cv2.threshold(gray,0,255, cv2.THRESH_BINARY_INV+cv2.THRESH_OTSU )

cv2.imshow('bg',thresh)
cv2.waitKey(0)
cv2.destroyAllWindows()