using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using SortingVisualizer.Utilities;
using SortingVisualizer.Visualizer;

namespace SortingVisualizer.Algorithms;

public class NativeLibraryAlgorithm : IVisualizable, IDisposable
{
    private readonly UnmanagedLibrary lib;
    private RunDelegate runFunction;
    private FreeDelegate freeFunction;
    private NameDelegate nameFunction;
    private string name = "Native Algorithm";
    public string Name => name;

    private delegate NativeVisualization RunDelegate(IntPtr arr, int size);

    private delegate void FreeDelegate(NativeVisualization visualization);

    private delegate IntPtr NameDelegate();

    public NativeLibraryAlgorithm(string dllFileName)
    {
        lib = new UnmanagedLibrary("algorithm.dll");
        runFunction = lib.GetUnmanagedFunction<RunDelegate>("run") ?? throw new Exception("Run function not found in the DLL.");
        freeFunction = lib.GetUnmanagedFunction<FreeDelegate>("freeVisualization") ?? throw new Exception("Free function not found in the DLL.");
        nameFunction = lib.GetUnmanagedFunction<NameDelegate>("getAlgorithmName") ?? throw new Exception("Name function not found in the DLL.");
        IntPtr namePtr = nameFunction();
        name = Marshal.PtrToStringAnsi(namePtr) ?? name;
    }

    private static T[] convertToArray<T>(IntPtr source, int size)
    {
        byte[] temp = new byte[Marshal.SizeOf<T>() * size];
        GCHandle tempHandle = GCHandle.Alloc(temp, GCHandleType.Pinned);
        T[] destination = new T[size];
        GCHandle destHandle = GCHandle.Alloc(destination, GCHandleType.Pinned);

        try
        {
            Marshal.Copy(source, temp, 0, temp.Length);
            IntPtr destPtr = destHandle.AddrOfPinnedObject();
            Marshal.Copy(temp, 0, destPtr, temp.Length);
            return destination;
        }
        finally
        {
            if (tempHandle.IsAllocated)
                tempHandle.Free();
            if (destHandle.IsAllocated)
                destHandle.Free();
        }
    }

    public IEnumerable<VisualizationFrame> Run(int[] array, IVisualizer visualizer)
    {
        GCHandle handle = GCHandle.Alloc(array, GCHandleType.Pinned);
        try
        {
            List<VisualizationFrame> frames = new();

            IntPtr arrayPtr = handle.AddrOfPinnedObject();
            var result = runFunction(arrayPtr, array.Length);
            NativeFrame[] rawFrames = convertToArray<NativeFrame>(result.Frames, result.Length);
            foreach (var frame in rawFrames)
            {
                int[] frameArray = new int[array.Length];
                Marshal.Copy(frame.Array, frameArray, 0, array.Length);

                int[] frameAccessed = new int[frame.AccessedLength];
                Marshal.Copy(frame.Accessed, frameAccessed, 0, frame.AccessedLength);

                frames.Add(new VisualizationFrame(frameArray, frameAccessed.ToList()));
            }

            freeFunction(result);
            return frames;
        }
        finally
        {
            if (handle.IsAllocated)
                handle.Free();
        }
    }

    public void Dispose()
    {
        lib.Dispose();
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct NativeFrame
    {
        public IntPtr Array;
        public IntPtr Accessed;
        public int AccessedLength;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct NativeVisualization
    {
        public int Length;
        public IntPtr Frames;
    }
}
