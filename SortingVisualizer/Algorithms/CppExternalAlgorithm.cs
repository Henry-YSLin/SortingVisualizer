using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using SortingVisualizer.Utilities;
using SortingVisualizer.Visualizer;

namespace SortingVisualizer.Algorithms;

public class CppExternalAlgorithm : IDisposable
{
    private readonly UnmanagedLibrary lib;
    private RunDelegate runFunction;
    public string Name => "C++ Algorithm";

    private delegate CppVisualization RunDelegate(IntPtr arr, int size);

    public CppExternalAlgorithm(string dllFileName)
    {
        lib = new UnmanagedLibrary("algorithm.dll");
        runFunction = lib.GetUnmanagedFunction<RunDelegate>("run") ?? throw new Exception("Run function not found in the DLL.");
    }

    public unsafe IEnumerable<VisualizationFrame> Run(int[] array, IVisualizer visualizer)
    {
        fixed (int* arrPtr = &array[0])
        {
            var res = runFunction(new IntPtr(arrPtr), array.Length);
            IntPtr[] frames = new IntPtr[res.Length];
            Marshal.Copy(res.Arrays, frames, 0, res.Length);
            List<int[]> managedFrames = new List<int[]>();
            for (int i = 0; i < res.Length; i++)
            {
                int[] frame = new int[array.Length];
                Marshal.Copy(frames[i], frame, 0, array.Length);
                managedFrames.Add(frame);
                Marshal.FreeHGlobal(frames[i]);
            }
            Marshal.FreeHGlobal(res.Arrays);
            Console.WriteLine(managedFrames);
        }

        return Array.Empty<VisualizationFrame>();
    }

    public void Dispose()
    {
        lib.Dispose();
    }

    private struct CppVisualization
    {
        public int Length;
        public IntPtr Arrays;
    }
}
