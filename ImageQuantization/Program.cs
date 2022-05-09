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
            if (pq.isEmpty())
            {
                Console.WriteLine("Empty works well");
            }
            else
            {
                Console.WriteLine("err");
            }

            //trying min heap 
            pq.insert(40);
            pq.insert(30);
            pq.insert(20);
            pq.insert(50);
            pq.GetMin();
            pq.GetMin();
            pq.insert(10);
            pq.insert(4);
            Console.WriteLine(pq.GetMin());
            






        }
    }
}