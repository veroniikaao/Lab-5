using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using RestSharp;



namespace WorkWithApi
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            openFileDialog1.Filter = "Image Files(*.BMP;*.JPG;*GIF;*.PNG)|*.BMP;*.JPG;*.GIF;*.PNG|All files (*.*)|*.*";
            comboBox1.Items.AddRange(new string[] { "ara", "bul", "chs", "cht", "hrv","cze", "dan", "dut", "eng", "fin", "fre", "ger", "gre", "hun", "kor", "ita", "jpn", "pol", "por", "rus", "slv", "spa", "swe" });
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    pictureBox1.Image = new Bitmap(openFileDialog1.FileName);

                }
                catch
                {
                    MessageBox.Show("Некорректный формат файла!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
           
            string tokenRegistration = "7fa3ced47088957";
            string pathToFile = openFileDialog1.FileName;
            string url = $"https://api.ocr.space/parse/image/";
            

            var client = new RestClient(url);
            var request = new RestRequest(Method.POST);
            request.AddHeader("apikey", tokenRegistration);
            request.AddFile("url", pathToFile);
            if (comboBox1.Text == "")
            {
                MessageBox.Show("Выберете язык!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                request.AddParameter("language", $"{comboBox1.Text}"); 
            }
            IRestResponse response = client.Execute(request);


           //Предотвращение ошибки
            try
            {
                JObject j = JObject.Parse(response.Content);
                JToken item = j.SelectToken("$.ParsedResults")[0].SelectToken(".ParsedText");
                textBox1.Text = item.ToString();
            }
            catch
            {
                textBox1.Text = "Проблема с ответом от сервера.";
            }
            
        }

    }
}
