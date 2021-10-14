using System;

namespace targilZero
{
    partial class Program
    {
        static void Main(string[] args)
        {
            welcome0938();
        }

        private static void welcome0938()
        {
            Console.WriteLine("enter your name: ");
            string name = Console.ReadLine();
            Console.WriteLine("{0}, welcome to my first console application", name);
        }
    }
}
