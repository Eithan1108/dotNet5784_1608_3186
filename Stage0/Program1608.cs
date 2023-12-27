using System;
namespace Stage0
{

    internal partial class Program
    {
        private static void Main(string[] args)
        {
            Welcome1608();
            Welcome3186();
            Console.ReadKey();


        }

        static void Welcome1608()
        {
            Console.Write("Enter your name: ");
            string name = Console.ReadLine();
            Console.WriteLine($"{name}, welcome to my first console application");
        }
        static partial void Welcome3186();
    }
}