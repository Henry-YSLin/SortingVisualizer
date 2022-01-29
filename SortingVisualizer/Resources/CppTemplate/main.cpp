#include <vector>
#include "visualizer.hpp"
#include "algorithm.hpp"

#define EXPORT __declspec(dllexport)

EXTERN_C struct EXPORT OutFrame
{
    int *Array;
    int ArrayLength;
    int *Accessed;
    int AccessedLength;
};

EXTERN_C struct EXPORT OutVisualization
{
    int Length;
    OutFrame *Frames;
};

EXTERN_C EXPORT OutVisualization _cdecl run(int arr[], int size)
{
    std::vector<Frame> ret = runAlgorithm(arr, size);

    OutVisualization vis;
    vis.Length = ret.size();
    vis.Frames = new OutFrame[ret.size()];
    for (int i = 0; i < ret.size(); i++)
    {
        vis.Frames[i].Array = ret[i].array;
        vis.Frames[i].ArrayLength = ret[i].size;
        vis.Frames[i].Accessed = new int[ret[i].accessed.size()];
        vis.Frames[i].AccessedLength = ret[i].accessed.size();
        for (int j = 0; j < ret[i].accessed.size(); j++)
        {
            vis.Frames[i].Accessed[j] = ret[i].accessed[j];
        }
        std::vector<int>().swap(ret[i].accessed);
    }

    std::vector<Frame>().swap(ret);

    return vis;
}

EXTERN_C EXPORT void _cdecl freeVisualization(OutVisualization vis)
{
    for (int i = 0; i < vis.Length; i++)
    {
        delete[] vis.Frames[i].Array;
        delete[] vis.Frames[i].Accessed;
    }
    delete[] vis.Frames;
}

EXTERN_C EXPORT char *_cdecl getAlgorithmName()
{
    return getName();
}