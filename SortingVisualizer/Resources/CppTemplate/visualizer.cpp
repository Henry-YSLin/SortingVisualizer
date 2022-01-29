#include "visualizer.hpp"

Frame newFrame(int arr[], int size, std::vector<int> accessed)
{
    Frame frame;
    int *newArr = new int[size];
    for (int i = 0; i < size; i++)
    {
        newArr[i] = arr[i];
    }
    frame.array = newArr;
    frame.size = size;
    frame.accessed = accessed;
    return frame;
}