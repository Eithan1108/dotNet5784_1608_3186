using System;
internal class Program
{
    private static void Main(string[] args)
    {
        Welcome1608();
        Console.ReadKey();


    }

    private static void Welcome1608()
    {
        Console.Write("Enter your name: ");
        string name = Console.ReadLine();
        Console.WriteLine($"{name}, welcome to my first console application");
    }
}