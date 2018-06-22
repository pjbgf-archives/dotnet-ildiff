using System;
using Microsoft.Extensions.CommandLineUtils;

namespace DotNet.Ildiff
{
    public class ArgumentHandler
    {
        private readonly Func<IldiffArguments, int> _executor;
        private CommandLineApplication _commandLineApplication;
        private Func<int> _showHelp;

        public ArgumentHandler(Func<IldiffArguments, int> executor, Func<int> showHelp = null)
        {
            Init();
            
            _executor = executor;
            _showHelp = showHelp;
        }

        void Init()
        {
            _commandLineApplication = new CommandLineApplication();

            _commandLineApplication.FullName = "dotnet ildiff";
            _commandLineApplication.Name = "dotnet ildiff";
            _commandLineApplication.Description = "Compare the IL difference between two .NET assemblies.";
            _commandLineApplication.HelpOption("-?|-h|--help");

            var assembly1 = _commandLineApplication.Argument("assembly1", "First assembly file.", false);
            var assembly2 = _commandLineApplication.Argument("assembly2", "Second assembly file.", false);
            
            var output = _commandLineApplication.Option("-o|--output",
                "Save output to file.",
                CommandOptionType.SingleValue);
            
            var item = _commandLineApplication.Option("-i|--item",
                "Select a method or class to be compared.",
                CommandOptionType.SingleValue);

            _commandLineApplication.OnExecute(() =>
            {
                if (!string.IsNullOrEmpty(assembly1.Value) && !string.IsNullOrEmpty(assembly2.Value))
                {
                    var arguments = new IldiffArguments();
                    arguments.Assembly1 = assembly1.Value;
                    arguments.Assembly2 = assembly2.Value;

                    if (item.HasValue())
                        arguments.Item = item.Value();

                    if (output.HasValue())
                        arguments.OutputFile = output.Value();
                    
                    _executor.Invoke(arguments);
                    return 0;
                }

                if (_showHelp != null)
                    _showHelp();
                
                _commandLineApplication.ShowHelp();
                return -1;
            });
        }

        public void Handle(string[] rawArguments)
        {
            _commandLineApplication.Execute(rawArguments);
        }
    }
}