using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ImageQuantization
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        RGBPixel[,] ImageMatrix;
        

        private void btnOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //Open the browsed image and display it
                string OpenedFilePath = openFileDialog1.FileName;
                ImageMatrix = ImageOperations.OpenImage(OpenedFilePath);
                ImageOperations.DisplayImage(ImageMatrix, pictureBox1);
                
            }
            txtWidth.Text = ImageOperations.GetWidth(ImageMatrix).ToString();
            txtHeight.Text = ImageOperations.GetHeight(ImageMatrix).ToString();

        }


        private void MainForm_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Add("Eager Prim's");
        }


        private void button1_Click(object sender, EventArgs e)
        {
            var watch = new System.Diagnostics.Stopwatch();

            watch.Start();

            //make  object from Clustering class
            Clustering clustering = new Clustering();

            //Make array of distinct colors
            dis_colors.Text = clustering.Properties_Colors(ImageMatrix).ToString();
            
            if (txtWidth.TextLength != 0)
            {
                if (comboBox1.SelectedIndex == 0)
                {

                    //apply Eager Prims algorithm 
                    float mst_cost = clustering.Eager_prims(0);
                    mst_txt_box.Text = mst_cost.ToString();

                    //K clustering
                    if (K_txt_box.Text.Length == 0)
                        MessageBox.Show("Enter number Of K First");
                    else
                        clustering.K_Clusters(Int16.Parse(K_txt_box.Text.ToString()));


                    //Replace the old colors with the new colors and display it
                    ImageOperations.DisplayImage(clustering.Quantization(ImageMatrix), pictureBox2);

                    watch.Stop();
                    time_lbl.Text = (watch.ElapsedMilliseconds / 1000).ToString()+" sec";
                    Console.WriteLine($"Execution Time: {watch.ElapsedMilliseconds} ms  , {watch.ElapsedMilliseconds/1000} sec");
                }
            }
            else
            {
                MessageBox.Show("Open Image First !");
            }
            return;
        }
    }
}