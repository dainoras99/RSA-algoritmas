using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Numerics;
using System.IO;

namespace RSA_algoritmas
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void encryptButton_Click(object sender, EventArgs e)
        {
            try
            {
                string text = xTextBox.Text;
                int p = Int32.Parse(pTextBox.Text);
                int q = Int32.Parse(qTextBox.Text);
                Validation(p, q);
                int n = p * q;

                File.WriteAllText(@"D:\KOLEGIJOS MEDZIAGA\INFORMACIJOS SAUGUMAS\RSA algoritmas\file.txt", n.ToString());
                StreamWriter streamWriter = new StreamWriter(@"D:\KOLEGIJOS MEDZIAGA\INFORMACIJOS SAUGUMAS\RSA algoritmas\file.txt", true);

                int fn = (p - 1) * (q - 1);
                int exponent = GetEValue(fn);
                IntWriteToFile(exponent, streamWriter);
                yTextBox.Text = Encryption(exponent, n, CreateAsciiDecimals(text), streamWriter);
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }
        private void IntWriteToFile(int text, StreamWriter streamWriter)
        {
            streamWriter.WriteLine("\n" + text.ToString());
        }
        private void StringWriteToFile(string text, StreamWriter streamWriter)
        {
            streamWriter.WriteLine(text);
            streamWriter.Close();
        }
        private void Validation(int p, int q)
        {
            for (int i = 2; i < p; i++)
                if (p % i == 0 && i != p)
                    throw new Exception("p must be a prime number");

            for (int i = 2; i < q; i++)
                if (q % i == 0 && i != q)
                    throw new Exception("q must be a prime number");
            if (p < 10 || q < 10)
                throw new Exception("q and p cannot have a value below 10");
        }
        private int GetEValue(int fn)
        {
            for (int e = 2; e < fn; e++)
                if (GCD(e, fn) == 1)
                {
                    int exponent = e;
                    if (exponent > 1)
                        return exponent;
                }
            throw new Exception("e not found");
        }
        private int GCD(int a, int b)
        {
            int temp = 0;
            while (b != 0)
            {
                temp = a % b;
                a = b;
                b = temp;
            }
            return a;
        }
        private List<int> CreateAsciiDecimals(string text)
        {
            List<int> AsciiDecimals = new List<int>();
            foreach (char a in text)
                AsciiDecimals.Add(a);
            return AsciiDecimals;
        }
        private string Encryption(int exponent, int n, List<int> AsciiDecimals, StreamWriter streamWriter) 
        {
            string encryptedText = "";
            foreach (char a in AsciiDecimals)
            { 
                BigInteger poweredByE = BigInteger.Pow(a, exponent);
                BigInteger encryptedChar = poweredByE % n;
                encryptedText += (char)(encryptedChar);
            }
            StringWriteToFile(encryptedText, streamWriter);
            return encryptedText;
        }
         
        private int privateKeyValue (int fn, int exponent)
        {
            int d = 1;
            while (d * exponent % fn != 1)
            {
                d++;
            }
            return d;
        }

        private string Decryption(int d, int n, List<int> AsciiDecimals)
        {
            string decryptedText = "";
            foreach (char a in AsciiDecimals)
            {
                BigInteger poweredByD = BigInteger.Pow(a, d);
                BigInteger decryptedChar = poweredByD % n;
                decryptedText += (char)(decryptedChar);
            }
            return decryptedText;
        }

        private void decryptButton_Click(object sender, EventArgs e)
        {
            string text = yTextBox.Text;
            int p = Int32.Parse(pTextBox.Text);
            int q = Int32.Parse(qTextBox.Text);
            Validation(p, q);
            int n = p * q;
            int fn = (p - 1) * (q - 1);
            int exponent = GetEValue(fn);
            int d = privateKeyValue(fn, exponent);
            xTextBox.Text = Decryption(d, n, CreateAsciiDecimals(text));
        }

       

        private void DecryptFromText_Click(object sender, EventArgs e)
        {
            string line;
            List<string> parts = new List<string>();
            StreamReader file = new StreamReader(@"D:\KOLEGIJOS MEDZIAGA\INFORMACIJOS SAUGUMAS\RSA algoritmas\file.txt");
            while ((line = file.ReadLine()) != null)
            {
                parts.Add(line);
            }
            yTextBox.Text = parts[2];
        }

        private void cleanButton_Click(object sender, EventArgs e)
        {
            pTextBox.Text = "";
            qTextBox.Text = "";
            xTextBox.Text = "";
            yTextBox.Text = "";
        }
    }
}
