using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using SortingVisualizer.Resources;

namespace SortingVisualizer.Editor;

// TODO: subject to refactoring
public class CppCodeCompiler
{
    private readonly string code;
    private static readonly string working_directory = Directory.GetCurrentDirectory();

    public CppCodeCompiler(string code)
    {
        this.code = code;
    }

    public async Task<FileInfo?> CompileAsync()
    {
        await File.WriteAllTextAsync(Path.Combine(working_directory, "algorithm.cpp"), code);
        await Task.WhenAll(
            File.WriteAllTextAsync(Path.Combine(working_directory, "algorithm.hpp"), ResourceStore.ALGORITHM_HPP.Value),
            File.WriteAllTextAsync(Path.Combine(working_directory, "main.cpp"), ResourceStore.MAIN_CPP.Value),
            File.WriteAllTextAsync(Path.Combine(working_directory, "visualizer.cpp"), ResourceStore.VISUALIZER_CPP.Value),
            File.WriteAllTextAsync(Path.Combine(working_directory, "visualizer.hpp"), ResourceStore.VISUALIZER_HPP.Value)
        );

        Process cmd = new Process();
        cmd.StartInfo.FileName = "g++.exe";
        cmd.StartInfo.Arguments = "-shared visualizer.cpp algorithm.cpp main.cpp -o algorithm.dll";
        cmd.StartInfo.WorkingDirectory = working_directory;
        cmd.StartInfo.RedirectStandardOutput = true;
        cmd.StartInfo.RedirectStandardError = true;
        cmd.StartInfo.CreateNoWindow = true;
        cmd.StartInfo.UseShellExecute = false;
        cmd.Start();

        await cmd.WaitForExitAsync();

        string tmp = await cmd.StandardOutput.ReadToEndAsync();
        tmp += await cmd.StandardError.ReadToEndAsync();
        Console.WriteLine(tmp);

        if (!File.Exists(Path.Combine(working_directory, "algorithm.dll")))
            return null;

        return new FileInfo(Path.Combine(working_directory, "algorithm.dll"));
    }
}
