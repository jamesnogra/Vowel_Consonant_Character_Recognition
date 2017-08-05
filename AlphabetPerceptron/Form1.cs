using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AlphabetPerceptron
{
    public partial class Form1 : Form
    {
        List<Alphabet> allAlphabet;
        bool[,] picBoxes;
        Color highlightColor;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            loadLettersText();
            highlightColor = Color.Red;
            picBoxes = new bool[7, 5];
            for (int x=0; x<7; x++)
            {
                for (int y=0; y<5; y++)
                {
                    picBoxes[x, y] = false;
                }
            }
        }

        //load the letters Text
        private void loadLettersText()
        {
            string[] lines = System.IO.File.ReadAllLines(@"Letters.txt");
            char tempLetter = ' ';
            string tempType = "", tempPattern = "";
            byte count = 0;
            Alphabet tempAlphabet;
            allAlphabet = new List<Alphabet>();

            foreach (string line in lines)
            {
                if (count == 8)
                {
                    tempAlphabet = new Alphabet(tempLetter, tempType, tempPattern);
                    allAlphabet.Add(tempAlphabet);
                    tempPattern = "";
                    count = 0;
                }
                if (count == 0)
                {
                    tempLetter = line[0];
                    tempType = line.Substring(2);
                }
                else
                {
                    tempPattern += line;
                }
                count++;
            }
        }

        private void p11_Click(object sender, EventArgs e)
        {
            if (picBoxes[1, 1])
            {
                p11.BackColor = Color.White;
                picBoxes[1, 1] = false;
            }
            else
            {
                p11.BackColor = highlightColor;
                picBoxes[1, 1] = true;
            }
        }
    }

    public class Alphabet
    {
        public char letter;
        public string type, pattern;
        public Alphabet(char letter, string type, string pattern)
        {
            this.letter = letter;
            this.type = type;
            this.pattern = pattern;
        }
        public override string ToString()
        {
            string temp = "";
            temp += "Letter: " + this.letter + "\nType: " + this.type + "\nPattern: " + this.pattern;
            return temp;
        }
    }
}
