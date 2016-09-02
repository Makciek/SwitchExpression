using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SwitchExpression;

namespace ExpTester
{
    class Program
    {
        private static ExpressionGenerator<int, string> ExpGen;

        static void Main(string[] args)
        {
            ExpGen = new ExpressionGenerator<int, string>();
            Console.WriteLine("Start");

            ExpGen.AddCase(1, (x) => Method1(x));
            var compiled = ExpGen.Generate();
            compiled.Invoke(1, "1Test!");

            Console.WriteLine("The End");
            Console.Read();
        }

        public static void Method1(string methdo)
        {
            Console.WriteLine($"Method1: {methdo}");
        }
    }
}
