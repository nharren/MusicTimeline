using Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var provider = new DataProvider();

            foreach (var composer in provider.Composers.ToList())
            {
                Console.Write(composer.ID + " ");
                Console.Write(composer.Name + " ");
                Console.WriteLine(composer.Dates);

                foreach (var era in composer.Eras)
                {
                    Console.WriteLine(era.Name + " " + era.Dates);
                }

                Console.WriteLine();
            }

            Console.ReadKey();
        }
    }
}
