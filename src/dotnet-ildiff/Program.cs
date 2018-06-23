using System;
using System.Diagnostics;
using System.IO;

namespace DotNet.Ildiff
{
    class Program
    {
        static void Main(string[] args)
        {
            if (!IsDotnetIldasmInstalled())
            {
                Console.WriteLine("This tool depends on dotnet-ildasm. Please execute 'dotnet tool install -g dotnet-ildasm' first.");
                return;
            }

            if (!IsGitdiffInstalled())
            {
                Console.WriteLine("This tool depends on git diff. Please make sure that is installed in your system.");
                return;
            }

            new CommandHandler(Execute);
        }
        
        static int Execute(CommandArgument argument)
        {
            var targetFile1 = Path.GetTempFileName();
            var targetFile2 = Path.GetTempFileName();

            ExecuteCommand("dotnet", BuildIldasmCommand(argument.Assembly1, targetFile1, argument.Item));
            ExecuteCommand("dotnet", BuildIldasmCommand(argument.Assembly2, targetFile2, argument.Item));
            
            var result = ExecuteCommand("git", $"diff {targetFile1} {targetFile2}");
            
            if (!string.IsNullOrEmpty(argument.OutputFile))
                File.WriteAllText(argument.OutputFile, result);

            return 0;
        }

        static string BuildIldasmCommand(string assemblyPath, string tempFilePath, string item)
        {
            string command = $"ildasm {assemblyPath} -o {tempFilePath} --force";
            
            if (!string.IsNullOrEmpty(item))
                command += " -i " + item;

            return command;
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

        public static string ExecuteCommand(string command, string arguments)
        {
            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = command,
                    Arguments = arguments,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };
            
            Console.WriteLine($"{command} {arguments}");
            process.Start();
            
            string result = process.StandardOutput.ReadToEnd();
            
            process.WaitForExit();
            
            Console.WriteLine(result);
            
            return result;
        }
    }
}
