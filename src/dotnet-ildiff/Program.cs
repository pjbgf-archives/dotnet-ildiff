using System;
using System.Diagnostics;
using System.IO;

namespace dotnet_ildiff
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Usage: dotnet ildiff assembly1.dll assembly2.dll");
                return;
            }

            Console.WriteLine("=== This is a POC for IL diff which depends on git diff. ===");

            if (!IsDotnetIldasmInstalled())
            {
                Console.WriteLine(
                    "This tool depends on dotnet-ildasm. Please execute 'dotnet tool install -g dotnet-ildasm' first.");
                return;
            }

            if (!IsGitdiffInstalled())
            {
                Console.WriteLine("This tool depends on git diff. Please make sure that is installede in your system.");
                return;
            }

            var sourceFile1 = args[0];
            var sourceFile2 = args[1];
            var targetFile1 = Path.GetTempFileName();
            var targetFile2 = Path.GetTempFileName();

            ExecuteCommand("dotnet", $"ildasm {sourceFile1} -o {targetFile1} --force");
            ExecuteCommand("dotnet", $"ildasm {sourceFile2} -o {targetFile2} --force");
            ExecuteCommand("git", $"diff {targetFile1} {targetFile2}");
        }

        public static bool IsDotnetIldasmInstalled()
        {
            var result = ExecuteCommand("dotnet", "tool list --global");
            return result.IndexOf("dotnet-ildasm") >= 0;
        }

        public static bool IsGitdiffInstalled()
        {
            try
            {
                ExecuteCommand("git", "diff");
            }
            catch
            {
                return false;
            }

            return true;
        }

        public static string ExecuteCommand(string cmd, string args)
        {
            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = cmd,
                    Arguments = args,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };
            
            Console.WriteLine($"{cmd} {args}");
            process.Start();
            
            string result = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            
            Console.WriteLine(result);
            
            return result;
        }
    }
}
