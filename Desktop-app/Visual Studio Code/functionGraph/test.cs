
using System;

namespace SimpleTestNamespace
{
    public class Calculator
    {
        public int Value;

        // Konstruktor
        public Calculator(int initialValue)
        {
            Value = initialValue;
            Console.WriteLine("Kalkulátor létrehozva");
        }

        // Void metódus
        public void Add(int number)
        {
            Value += number;
            Console.WriteLine("Hozzáadva: " + number);
        }

        // Nem-void metódus
        public int Multiply(int number)
        {
            return Value * number;
        }

        // Statikus metódus
        public static void PrintMessage()
        {
            Console.WriteLine("Ez egy statikus üzenet");
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            Calculator calc = new Calculator(5);
            calc.Add(10);
            int result = calc.Multiply(2);
            Console.WriteLine("Eredmény: " + result);
            Calculator.PrintMessage();
        }
    }
}
