// ImageSegmentation.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include "opencv2/opencv.hpp"
#include <string>

typedef unsigned char byte;

using namespace cv;
using namespace std;

Mat bytesToMat(byte* bytes, int width, int height, bool isGrayScale)
{
	Mat image = Mat(height, width, isGrayScale ? CV_8UC1 : CV_8UC3, bytes).clone(); // make a copy	
	return image;
}

byte * matToBytes(Mat image)
{
	int total = image.total();
	int elementSize = image.elemSize();
	int size = total * elementSize;
	byte * bytes = new byte[size];  // you will have to delete[] that later
	std::memcpy(bytes, image.data, size * sizeof(byte));
	return bytes;
}

extern "C" {
	__declspec(dllexport) unsigned char* otsuBinarization(unsigned char* bytes, int width, int height, int blur = 5, bool revert = false)
	{
		Mat image = bytesToMat(bytes, width, height, false);
		Mat gray;
		Mat gaus;
		Mat output;

		cvtColor(image, gray, cv::COLOR_BGR2GRAY);
		cv::GaussianBlur(gray, gaus, cv::Size(blur, blur), 0);
		cv::threshold(gaus, output, 0, 255, revert ? (CV_THRESH_BINARY_INV | CV_THRESH_OTSU) : (CV_THRESH_BINARY | CV_THRESH_OTSU));

		return matToBytes(output);
	}

	__declspec(dllexport) unsigned char* gaussianBlur(unsigned char* bytes, int width, int height, int blur = 5)
	{
		Mat image = bytesToMat(bytes, width, height, false);
		Mat gray;
		Mat output;

		cvtColor(image, gray, cv::COLOR_BGR2GRAY);
		cv::GaussianBlur(gray, output, cv::Size(blur, blur), 0);

		return matToBytes(output);
	}

	__declspec(dllexport) unsigned char* grayScale(unsigned char* bytes, int width, int height)
	{
		Mat image = bytesToMat(bytes, width, height, false);
		Mat gray;
		Mat output;

		cvtColor(image, gray, cv::COLOR_BGR2GRAY);

		return matToBytes(gray);
	}
}

int main()
{
	return 0;
}

