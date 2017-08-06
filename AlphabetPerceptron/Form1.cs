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
        Random rnd;
        double[] wts;
        double[] inputs;
        double bias;
        double learning_rate = 0.005;
        int output;
        byte[] desired;
        byte[,] patterns;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            loadLettersText();

            //initialize the data
            int patternLength = allAlphabet[0].pattern.Length;
            rnd = new Random();
            //MessageBox.Show(allAlphabet[0].pattern.Length+"");
            patterns = new byte[allAlphabet.Count, patternLength];
            desired = new byte[allAlphabet.Count];
            bias = rnd.NextDouble();
            wts = new double[patternLength];

            //initialize the pattern, bias, and desired output
            for (int x=0; x<allAlphabet.Count; x++)
            {
                for (int y=0; y<patternLength; y++)
                {
                    patterns[x, y] = (byte)Char.GetNumericValue(allAlphabet[x].pattern[y]);
                }
                desired[x] = 0; //set all letters to consonant
                if (allAlphabet[x].type == "Vowel")
                {
                    desired[x] = 1;
                }
                //MessageBox.Show("Letter: " + allAlphabet[x].letter + "\nType: " + desired[x]);
            }

            
            for (int x=0; x<wts.Length; x++)
            {
                wts[x] = rnd.NextDouble();
            }

            highlightColor = Color.CadetBlue;
            picBoxes = new bool[7, 5];
            for (int x=0; x<7; x++)
            {
                for (int y=0; y<5; y++)
                {
                    picBoxes[x, y] = false;
                }
            }
        }

        private void trainBtn_Click(object sender, EventArgs e)
        {
            int length = allAlphabet.Count;
            int patternLength = allAlphabet[0].pattern.Length;
            double v;
            double delta;
            int max_epoch = 100000, epochs = 0;
            int error = 10;
            int pn; //represents the pattern number
            inputs = new double[patternLength];
            int[] pat_used = new int[length];

            while (error > 0 && epochs < max_epoch)
            {
                //set all pat_used to 0 every epoch also error
                error = 0;
                for (int x=0; x<length; x++)
                {
                    pat_used[x] = 0;
                }

                for (int x=0; x<length; x++)
                {
                    pn = rnd.Next(length);
                    while (pat_used[pn]==1)
                    {
                        pn = rnd.Next(length);
                    }
                    for (int y=0; y<patternLength; y++)
                    {
                        //MessageBox.Show("At pattern " + pn + " and index " + y);
                        inputs[y] = patterns[pn, y];
                    }
                    pat_used[pn] = 1; //mark this pattern that we've used it

                    v = 0.0;
                    for (int j=0; j<patternLength; j++)
                    {
                        v += inputs[j] * wts[j];
                    }
                    v += bias;
                    if (v >= 0) output = 1;
                    else output = 0;

                    delta = desired[pn] - output;
                    if (delta!=0)
                    {
                        for (int i = 0; i < patternLength; i++)
                        {
                            wts[i] += learning_rate * delta * inputs[i];
                        }
                        bias += learning_rate * delta;
                        //MessageBox.Show("Changing Bias: " + bias);
                    }
                    error += Math.Abs((int)delta);
                }
                epochs++;
            }
            MessageBox.Show("Finished!\nEpochs: " + epochs + "\nErrors: " + error);
        }

        //test button
        private void testBtn_Click(object sender, EventArgs e)
        {
            double v = 0;
            int[] allInputs = new int[wts.Length];
            int counter = 0;

            //get all inputs for v
            for (int x=0; x<7; x++)
            {
                for (int y=0; y<5; y++)
                {
                    allInputs[counter] = ((picBoxes[x,y]) ? 1 : 0);
                    counter++;
                }
            }
            
            //calculate for v
            for (int x=0; x<allInputs.Length; x++)
            {
                v += allInputs[x] * wts[x];
            }
            v += bias;

            if (v>=0)
            {
                MessageBox.Show("1 or VOWEL");
            }
            else
            {
                MessageBox.Show("1 or CONSONANT");
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
            //MessageBox.Show("Total lines: " + lines.Length);

            for (int x=0; x<=lines.Length; x++)
            {
                if (count == 8)
                {
                    tempAlphabet = new Alphabet(tempLetter, tempType, tempPattern);
                    allAlphabet.Add(tempAlphabet);
                    //MessageBox.Show("" + tempAlphabet);
                    tempPattern = "";
                    count = 0;
                    if (lines.Length == allAlphabet.Count * 8) break;
                }
                if (count == 0)
                {
                    tempLetter = lines[x][0];
                    tempType = lines[x].Substring(2);
                }
                else
                {
                    tempPattern += lines[x];
                }
                count++;
            }
        }

        private Color changeColor(int x, int y)
        {
            x--;
            y--;
            if (picBoxes[x, y])
            {
                picBoxes[x, y] = false;
                return Color.White;
            }
            else
            {
                picBoxes[x, y] = true;
                return highlightColor;
            }
        }

        private void p11_Click(object sender, EventArgs e)
        {
            p11.BackColor = changeColor(1, 1);
        }

        private void p12_Click(object sender, EventArgs e)
        {
            p12.BackColor = changeColor(1, 2);
        }

        private void p13_Click(object sender, EventArgs e)
        {
            p13.BackColor = changeColor(1, 3);
        }

        private void p14_Click(object sender, EventArgs e)
        {
            p14.BackColor = changeColor(1, 4);
        }

        private void p15_Click(object sender, EventArgs e)
        {
            p15.BackColor = changeColor(1, 5);
        }

        private void p21_Click(object sender, EventArgs e)
        {
            p21.BackColor = changeColor(2, 1);
        }

        private void p22_Click(object sender, EventArgs e)
        {
            p22.BackColor = changeColor(2, 2);
        }

        private void p23_Click(object sender, EventArgs e)
        {
            p23.BackColor = changeColor(2, 3);
        }

        private void p24_Click(object sender, EventArgs e)
        {
            p24.BackColor = changeColor(2, 4);
        }

        private void p25_Click(object sender, EventArgs e)
        {
            p25.BackColor = changeColor(2, 5);
        }

        private void p31_Click(object sender, EventArgs e)
        {
            p31.BackColor = changeColor(3, 1);
        }

        private void p32_Click(object sender, EventArgs e)
        {
            p32.BackColor = changeColor(3, 2);
        }

        private void p33_Click(object sender, EventArgs e)
        {
            p33.BackColor = changeColor(3, 3);
        }

        private void p34_Click(object sender, EventArgs e)
        {
            p34.BackColor = changeColor(3, 4);
        }

        private void p35_Click(object sender, EventArgs e)
        {
            p35.BackColor = changeColor(3, 5);
        }

        private void p41_Click(object sender, EventArgs e)
        {
            p41.BackColor = changeColor(4, 1);
        }

        private void p42_Click(object sender, EventArgs e)
        {
            p42.BackColor = changeColor(4, 2);
        }

        private void p43_Click(object sender, EventArgs e)
        {
            p43.BackColor = changeColor(4, 3);
        }

        private void p44_Click(object sender, EventArgs e)
        {
            p44.BackColor = changeColor(4, 4);
        }

        private void p45_Click(object sender, EventArgs e)
        {
            p45.BackColor = changeColor(4, 5);
        }

        private void p51_Click(object sender, EventArgs e)
        {
            p51.BackColor = changeColor(5, 1);
        }

        private void p52_Click(object sender, EventArgs e)
        {
            p52.BackColor = changeColor(5, 2);
        }

        private void p53_Click(object sender, EventArgs e)
        {
            p53.BackColor = changeColor(5, 3);
        }

        private void p54_Click(object sender, EventArgs e)
        {
            p54.BackColor = changeColor(5, 4);
        }

        private void p55_Click(object sender, EventArgs e)
        {
            p55.BackColor = changeColor(5, 5);
        }

        private void p61_Click(object sender, EventArgs e)
        {
            p61.BackColor = changeColor(6, 1);
        }

        private void p62_Click(object sender, EventArgs e)
        {
            p62.BackColor = changeColor(6, 2);
        }

        private void p63_Click(object sender, EventArgs e)
        {
            p63.BackColor = changeColor(6, 3);
        }

        private void p64_Click(object sender, EventArgs e)
        {
            p64.BackColor = changeColor(6, 4);
        }

        private void p65_Click(object sender, EventArgs e)
        {
            p65.BackColor = changeColor(6, 5);
        }

        private void p71_Click(object sender, EventArgs e)
        {
            p71.BackColor = changeColor(7, 1);
        }

        private void p72_Click(object sender, EventArgs e)
        {
            p72.BackColor = changeColor(7, 2);
        }

        private void p73_Click(object sender, EventArgs e)
        {
            p73.BackColor = changeColor(7, 3);
        }

        private void p74_Click(object sender, EventArgs e)
        {
            p74.BackColor = changeColor(7, 4);
        }

        private void p75_Click(object sender, EventArgs e)
        {
            p75.BackColor = changeColor(7, 5);
        }

        private void resetBtn_Click(object sender, EventArgs e)
        {
            for (int x=0; x<7; x++)
            {
                for (int y=0; y<5; y++)
                {
                    picBoxes[x, y] = false;
                }
            }
            p11.BackColor = Color.White;
            p12.BackColor = Color.White;
            p13.BackColor = Color.White;
            p14.BackColor = Color.White;
            p15.BackColor = Color.White;
            p21.BackColor = Color.White;
            p22.BackColor = Color.White;
            p23.BackColor = Color.White;
            p24.BackColor = Color.White;
            p25.BackColor = Color.White;
            p31.BackColor = Color.White;
            p32.BackColor = Color.White;
            p33.BackColor = Color.White;
            p34.BackColor = Color.White;
            p35.BackColor = Color.White;
            p41.BackColor = Color.White;
            p42.BackColor = Color.White;
            p43.BackColor = Color.White;
            p44.BackColor = Color.White;
            p45.BackColor = Color.White;
            p51.BackColor = Color.White;
            p52.BackColor = Color.White;
            p53.BackColor = Color.White;
            p54.BackColor = Color.White;
            p55.BackColor = Color.White;
            p61.BackColor = Color.White;
            p62.BackColor = Color.White;
            p63.BackColor = Color.White;
            p64.BackColor = Color.White;
            p65.BackColor = Color.White;
            p71.BackColor = Color.White;
            p72.BackColor = Color.White;
            p73.BackColor = Color.White;
            p74.BackColor = Color.White;
            p75.BackColor = Color.White;
        }

        private void drawBtn_Click(object sender, EventArgs e)
        {
            char selectedLetter;
            string selectedPattern = "";
            if (letterInput.Text.Length==1)
            {
                selectedLetter = Convert.ToChar(letterInput.Text);
                for (int x=0; x<allAlphabet.Count; x++)
                {
                    if (Char.ToLower(selectedLetter) == Char.ToLower(allAlphabet[x].letter))
                    {
                        selectedPattern = allAlphabet[x].pattern;
                        x = allAlphabet.Count;
                    }
                }
                if (selectedPattern.Length!=0)
                {
                    //column 1
                    if (selectedPattern[0] == '1') { p11.BackColor = highlightColor; picBoxes[0, 0] = true; }
                    else { p11.BackColor = Color.White; picBoxes[0, 0] = false; }
                    if (selectedPattern[1] == '1') { p12.BackColor = highlightColor; picBoxes[0, 1] = true; }
                    else { p12.BackColor = Color.White; picBoxes[0, 1] = false; }
                    if (selectedPattern[2] == '1') { p13.BackColor = highlightColor; picBoxes[0, 2] = true; }
                    else { p13.BackColor = Color.White; picBoxes[0, 2] = false; }
                    if (selectedPattern[3] == '1') { p14.BackColor = highlightColor; picBoxes[0, 3] = true; }
                    else { p14.BackColor = Color.White; picBoxes[0, 3] = false; }
                    if (selectedPattern[4] == '1') { p15.BackColor = highlightColor; picBoxes[0, 4] = true; }
                    else { p15.BackColor = Color.White; picBoxes[0, 4] = false; }
                    //column 2
                    if (selectedPattern[5] == '1') { p21.BackColor = highlightColor; picBoxes[1, 0] = true; }
                    else { p21.BackColor = Color.White; picBoxes[1, 0] = false; }
                    if (selectedPattern[6] == '1') { p22.BackColor = highlightColor; picBoxes[1, 1] = true; }
                    else { p22.BackColor = Color.White; picBoxes[1, 1] = false; }
                    if (selectedPattern[7] == '1') { p23.BackColor = highlightColor; picBoxes[1, 2] = true; }
                    else { p23.BackColor = Color.White; picBoxes[1, 2] = false; }
                    if (selectedPattern[8] == '1') { p24.BackColor = highlightColor; picBoxes[1, 3] = true; }
                    else { p24.BackColor = Color.White; picBoxes[1, 3] = false; }
                    if (selectedPattern[9] == '1') { p25.BackColor = highlightColor; picBoxes[1, 4] = true; }
                    else { p25.BackColor = Color.White; picBoxes[1, 4] = false; }
                    //column 3
                    if (selectedPattern[10] == '1') { p31.BackColor = highlightColor; picBoxes[2, 0] = true; }
                    else { p31.BackColor = Color.White; picBoxes[2, 0] = false; }
                    if (selectedPattern[11] == '1') { p32.BackColor = highlightColor; picBoxes[2, 1] = true; }
                    else { p32.BackColor = Color.White; picBoxes[2, 1] = false; }
                    if (selectedPattern[12] == '1') { p33.BackColor = highlightColor; picBoxes[2, 2] = true; }
                    else { p33.BackColor = Color.White; picBoxes[2, 2] = false; }
                    if (selectedPattern[13] == '1') { p34.BackColor = highlightColor; picBoxes[2, 3] = true; }
                    else { p34.BackColor = Color.White; picBoxes[2, 3] = false; }
                    if (selectedPattern[14] == '1') { p35.BackColor = highlightColor; picBoxes[2, 4] = true; }
                    else { p35.BackColor = Color.White; picBoxes[2, 4] = false; }
                    //column 4
                    if (selectedPattern[15] == '1') { p41.BackColor = highlightColor; picBoxes[3, 0] = true; }
                    else { p41.BackColor = Color.White; picBoxes[3, 0] = false; }
                    if (selectedPattern[16] == '1') { p42.BackColor = highlightColor; picBoxes[3, 1] = true; }
                    else { p42.BackColor = Color.White; picBoxes[3, 1] = false; }
                    if (selectedPattern[17] == '1') { p43.BackColor = highlightColor; picBoxes[3, 2] = true; }
                    else { p43.BackColor = Color.White; picBoxes[3, 2] = false; }
                    if (selectedPattern[18] == '1') { p44.BackColor = highlightColor; picBoxes[3, 3] = true; }
                    else { p44.BackColor = Color.White; picBoxes[3, 3] = false; }
                    if (selectedPattern[19] == '1') { p45.BackColor = highlightColor; picBoxes[3, 4] = true; }
                    else { p45.BackColor = Color.White; picBoxes[3, 4] = false; }
                    //column 5
                    if (selectedPattern[20] == '1') { p51.BackColor = highlightColor; picBoxes[4, 0] = true; }
                    else { p51.BackColor = Color.White; picBoxes[4, 0] = false; }
                    if (selectedPattern[21] == '1') { p52.BackColor = highlightColor; picBoxes[4, 1] = true; }
                    else { p52.BackColor = Color.White; picBoxes[4, 1] = false; }
                    if (selectedPattern[22] == '1') { p53.BackColor = highlightColor; picBoxes[4, 2] = true; }
                    else { p53.BackColor = Color.White; picBoxes[4, 2] = false; }
                    if (selectedPattern[23] == '1') { p54.BackColor = highlightColor; picBoxes[4, 3] = true; }
                    else { p54.BackColor = Color.White; picBoxes[4, 3] = false; }
                    if (selectedPattern[24] == '1') { p55.BackColor = highlightColor; picBoxes[4, 4] = true; }
                    else { p55.BackColor = Color.White; picBoxes[4, 4] = false; }
                    //column 6
                    if (selectedPattern[25] == '1') { p61.BackColor = highlightColor; picBoxes[5, 0] = true; }
                    else { p61.BackColor = Color.White; picBoxes[5, 0] = false; }
                    if (selectedPattern[26] == '1') { p62.BackColor = highlightColor; picBoxes[5, 1] = true; }
                    else { p62.BackColor = Color.White; picBoxes[5, 1] = false; }
                    if (selectedPattern[27] == '1') { p63.BackColor = highlightColor; picBoxes[5, 2] = true; }
                    else { p63.BackColor = Color.White; picBoxes[5, 2] = false; }
                    if (selectedPattern[28] == '1') { p64.BackColor = highlightColor; picBoxes[5, 3] = true; }
                    else { p64.BackColor = Color.White; picBoxes[5, 3] = false; }
                    if (selectedPattern[29] == '1') { p65.BackColor = highlightColor; picBoxes[5, 4] = true; }
                    else { p65.BackColor = Color.White; picBoxes[5, 4] = false; }
                    //column 7
                    if (selectedPattern[30] == '1') { p71.BackColor = highlightColor; picBoxes[6, 0] = true; }
                    else { p71.BackColor = Color.White; picBoxes[6, 0] = false; }
                    if (selectedPattern[31] == '1') { p72.BackColor = highlightColor; picBoxes[6, 1] = true; }
                    else { p72.BackColor = Color.White; picBoxes[6, 1] = false; }
                    if (selectedPattern[32] == '1') { p73.BackColor = highlightColor; picBoxes[6, 2] = true; }
                    else { p73.BackColor = Color.White; picBoxes[6, 2] = false; }
                    if (selectedPattern[33] == '1') { p74.BackColor = highlightColor; picBoxes[6, 3] = true; }
                    else { p74.BackColor = Color.White; picBoxes[6, 3] = false; }
                    if (selectedPattern[34] == '1') { p75.BackColor = highlightColor; picBoxes[6, 4] = true; }
                    else { p75.BackColor = Color.White; picBoxes[6, 4] = false; }
                }
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
