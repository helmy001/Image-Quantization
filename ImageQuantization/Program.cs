using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ImageQuantization
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            /*Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());*/

            Min_Heap pq = new Min_Heap();
            edge e1=new edge(0,1,10);
            edge e2= new edge(0, 2, 1);
            edge e3 = new edge(0, 3, 4);
            edge e4 = new edge(2, 1, 3);


            if (pq.isEmpty())
            {
                Console.WriteLine("Empty works well");
            }
            else
            {
                Console.WriteLine("err");
            }

            //trying min heap 
            
            pq.insert(e1);
            pq.insert(e2);
            pq.insert(e3);
            Console.WriteLine(pq.GetMin().end);
            Console.WriteLine(pq.GetMin().end);
            Console.WriteLine(pq.GetMin().end);
            Console.WriteLine(pq.GetMin().end);






        }
    }
}