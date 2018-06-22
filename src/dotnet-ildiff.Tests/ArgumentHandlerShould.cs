using System;
using DotNet.Ildiff;
using NSubstitute;
using Xunit;

namespace DotNet.Ildiff.Tests
{
    public class ArgumentHandlerShould
    {
        private Func<IldiffArguments, int> _executor;
        private Func<int> _showHelp;

        public ArgumentHandlerShould()
        {
            _executor = Substitute.For<Func<IldiffArguments, int>>();
            _showHelp = Substitute.For<Func<int>>();
        }

        [Fact]
        public void Execute_Command_With_Assembly1_And_Assembly2()
        {
            var arguments = new string[] {"assembly1.dll", "assembly2.dll"};
            var handler = new ArgumentHandler(_executor, _showHelp);
            var expected = new IldiffArguments
            {
                Assembly1 = "assembly1.dll",
                Assembly2 = "assembly2.dll"
            };

            handler.Handle(arguments);

            _executor.Received(1).Invoke(Arg.Is<IldiffArguments>(x =>
                x.Assembly1 == expected.Assembly1 && 
                x.Assembly2 == expected.Assembly2));
        }

        [Fact]
        public void Execute_Command_With_Assembly1_Assembly2_With_Output_File()
        {
            var arguments = new string[] {"assembly1.dll", "assembly2.dll", "-o", "output.il"};
            var handler = new ArgumentHandler(_executor, _showHelp);
            var expected = new IldiffArguments
            {
                Assembly1 = "assembly1.dll",
                Assembly2 = "assembly2.dll",
                OutputFile = "output.il"
            };

            handler.Handle(arguments);

            _executor.Received(1).Invoke(Arg.Is<IldiffArguments>(x =>
                x.Assembly1 == expected.Assembly1 && 
                x.Assembly2 == expected.Assembly2 && 
                x.OutputFile == expected.OutputFile));
        }

        [Fact]
        public void Execute_Command_With_Assembly1_Assembly2_And_Item_With_Output_File()
        {
            var arguments = new string[] {"assembly1.dll", "assembly2.dll", "-o", "output.il", "-i", "::Method"};
            var handler = new ArgumentHandler(_executor, _showHelp);
            var expected = new IldiffArguments
            {
                Assembly1 = "assembly1.dll",
                Assembly2 = "assembly2.dll",
                OutputFile = "output.il",
                Item = "::Method"
            };

            handler.Handle(arguments);

            _executor.Received(1).Invoke(Arg.Is<IldiffArguments>(x =>
                x.Assembly1 == expected.Assembly1 && 
                x.Assembly2 == expected.Assembly2 && 
                x.OutputFile == expected.OutputFile && 
                x.Item == expected.Item));
        }

        [Fact]
        public void Execute_Command_With_Assembly1_Assembly2_And_Item()
        {
            var arguments = new string[] {"assembly1.dll", "assembly2.dll", "-i", "::Method"};
            var handler = new ArgumentHandler(_executor, _showHelp);
            var expected = new IldiffArguments
            {
                Assembly1 = "assembly1.dll",
                Assembly2 = "assembly2.dll",
                Item = "::Method"
            };

            handler.Handle(arguments);

            _executor.Received(1).Invoke(Arg.Is<IldiffArguments>(x =>
                x.Assembly1 == expected.Assembly1 && 
                x.Assembly2 == expected.Assembly2 && 
                x.Item == expected.Item));
        }
        
        [Fact]
        public void Print_Help_If_No_Arguments()
        {
            var arguments = new string[] {};
            var handler = new ArgumentHandler(_executor, _showHelp);

            handler.Handle(arguments);

            _showHelp.Received(1).Invoke();
        }
    }
}