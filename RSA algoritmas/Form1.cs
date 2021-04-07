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

namespace RSA_algoritmas
{
    public partial class Form1 : Form
    {
        Encoding encoding = Encoding.GetEncoding("437");
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
                int fn = (p - 1) * (q - 1);
                int exponent = GetEValue(fn);
                Console.WriteLine("n " + n);
                Console.WriteLine("fn " + fn);
                Console.WriteLine("e " + exponent);
                Console.WriteLine("encrypted: " + Encryption(exponent, n, CreateXAsciiDecimals(text)));

            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }
        private void Validation(int p, int q)
        {
            for (int i = 2; i < p; i++)
                if (p % i == 0 && i != p)
                    throw new Exception("p must be a prime number");

            for (int i = 2; i < q; i++)
                if (q % i == 0 && i != q)
                    throw new Exception("q must be a prime number");
        }
        private int GetEValue(int fn)
        {
            for (int e = 2; e < fn; e++)
                if (GCD(e, fn) == 1)
                    if (e > 1)
                        return e;
            throw new Exception("e not found");
        }
        private int GCD(int a, int b)
        {
            while (b != 0)
            {
                int temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }
        private List<int> CreateXAsciiDecimals(string text)
        {
            List<int> xAsciiDecimals = new List<int>();
            foreach (char a in text)
                xAsciiDecimals.Add(a);
            return xAsciiDecimals;
        }
        private string Encryption(int exponent, int n, List<int> xAsciiDecimals) 
        {
            string encryptedText = "";
            foreach (char a in xAsciiDecimals)
            { 
                BigInteger poweredE = BigInteger.Pow(a, exponent);
                BigInteger decryptedChar = poweredE % n;
                encryptedText += (char)(decryptedChar);
            }
            return encryptedText;
        }
         
        private int privateKeyValue (int fn, int exponent)
        {
            int d = 2;
            while (d * exponent % fn != 1)
            {
                d++;
            }
            return d;
        }

    }
}
