#ifndef VISUALIZER_HPP
#define VISUALIZER_HPP

#ifdef __cplusplus
#define EXTERN_C extern "C"
#else
#define EXTERN_C
#endif

#include <vector>

EXTERN_C struct Frame
{
    int *array;
    int size;
    std::vector<int> accessed;
};

EXTERN_C Frame newFrame(int arr[], int size, std::vector<int> accessed = {});

#endif