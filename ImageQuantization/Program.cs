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
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());

            /*Clustering clustering = new Clustering();
            clustering.Distinct_arr.Add(new RGBPixel(1, 1, 1));
            clustering.Distinct_arr.Add(new RGBPixel(10, 10, 10));
            clustering.Distinct_arr.Add(new RGBPixel(20, 20, 20));
            clustering.Show_Distinct_arr();*/
        




        }
    }
}