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
        Clustering clustering;

        private void btnOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //Open the browsed image and display it
                string OpenedFilePath = openFileDialog1.FileName;
                ImageMatrix = ImageOperations.OpenImage(OpenedFilePath);
                ImageOperations.DisplayImage(ImageMatrix, pictureBox1);

                //make  object from Clustering class
                clustering = new Clustering();

                //Make array of distinct colors
                clustering.Properties_Colors(ImageMatrix);

                
            }
            txtWidth.Text = ImageOperations.GetWidth(ImageMatrix).ToString();
            txtHeight.Text = ImageOperations.GetHeight(ImageMatrix).ToString();

        }

       /* private void btnGaussSmooth_Click(object sender, EventArgs e)
        {
            double sigma = double.Parse(txtGaussSigma.Text);
            int maskSize = (int)nudMaskSize.Value ;
            ImageMatrix = ImageOperations.GaussianFilter1D(ImageMatrix, maskSize, sigma);
            ImageOperations.DisplayImage(ImageMatrix, pictureBox2);
        }*/

        private void MainForm_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Add("Lazy Prim's ");
            comboBox1.Items.Add("Eager Prim's");
        }

        
        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (txtWidth.TextLength != 0)
            {
                if (comboBox1.SelectedIndex == 0)
                {
                    clustering.Lazy_prims(0);
                }
                else if (comboBox1.SelectedIndex == 1)
                {
                    clustering.Eager_prims(0);
                }
            }
            else
            {
                MessageBox.Show("Open Image First !");
            }
        }
    }
}