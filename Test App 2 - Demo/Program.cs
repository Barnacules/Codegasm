using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Added references
// System.Web.Dll

// Added namespaces
using System.Web;
using System.Threading;

namespace TestApp2Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            // We want to save the current console color so we can restore it later
            ConsoleColor oldColor = Console.ForegroundColor;

            // Set the new console color to Green
            Console.ForegroundColor = ConsoleColor.Green;
            
            // Tell everyone what this program is when it starts
            Console.WriteLine("Welcome to #Codegasm episode 2 - Magic 8 ball app");

            Random random = new Random();
            while (true)
            {
                Console.Write("Ask any question: ");
                string question = Console.ReadLine();
                if( question.ToLower() == "quit")
                {
                    break;
                }
                Console.Write("Answer: ");
                switch( random.Next(5) )
                {
                    case 0:
                        Console.WriteLine("Yes!");
                        break;
                    case 1:
                        Console.WriteLine("Hell No!");
                        break;
                    case 2:
                        Console.WriteLine("Probably.");
                        break;
                    case 3:
                        Console.WriteLine("WTF do you think?");
                        break;
                    case 4:
                        Console.WriteLine("I don't understand the question?!?!");
                        break;
                }
            }

            // Restore the old foreground color
            Console.ForegroundColor = oldColor;
        }
    }
}
