using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
//using System.Web.UI.DataVisualization.Charting;
using System.Windows.Forms.DataVisualization.Charting;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        bool CC = true;
        double FstOprnd;
        double ScndOprnd;
        bool AddFlag = false;
        bool SubFlag = false;
        bool MultFlag = false;
        bool DivFlag = false;
        bool PowFlag = false;
        bool ModFlag = false;
        bool InvsFlag = false;
        bool SktchFlag = false;
        bool NegFlag = false;
        //Point[] pts;
        //e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

        public Form1()
        {
            InitializeComponent();
        }

        public Point[] convertShit(string s, double start, double end, double step)
        {
            Point[] pts = new Point[Math.Abs((int)((end - start + 1) / step))];
            string txtInput = s;
            string temp = "";
            for (double i = start; i <= end; i += step)
            {
                temp = txtInput.Replace("x", Convert.ToString(i));
                double h = calcRPN(toRPN(temp));
                //Console.WriteLine(h);
                pts[(int)i] = new Point((int)i, (int)h);
            }
            return pts;
        }

        public static string toRPN(string s)
        {
            string RPN = "";
            string[] tokens = s.Split(' ');
            Stack<string> stack = new Stack<string>();
            List<string> outputList = new List<string>();
            int n;
            foreach (string c in tokens)
            {
                if (int.TryParse(c.ToString(), out n))
                {
                    outputList.Add(c);
                }
                if (c == "(")
                {
                    stack.Push(c);
                }
                if (c == ")")
                {
                    while (stack.Count != 0 && stack.Peek() != "(")
                    {
                        outputList.Add(stack.Pop());
                    }
                    stack.Pop();
                }
                if (isOperator(c) == true)
                {
                    while (stack.Count != 0 && Priority(stack.Peek()) >= Priority(c))
                    {
                        outputList.Add(stack.Pop());
                    }
                    stack.Push(c);
                }
            }
            while (stack.Count != 0)
            {
                outputList.Add(stack.Pop());
            }

            for (int i = 0; i < outputList.Count; i++)
            {
                RPN = RPN + " " + outputList[i];
            }
            return RPN;
        }
        public static int Priority(string c)
        {
            if (c == "^")
                return 3;
            if (c == "*" || c == "/")
                return 2;
            if (c == "+" || c == "-")
                return 1;
            return 0;
        }

        public static bool isOperator(string c)
        {
            return (c == "+" || c == "-" || c == "*" || c == "/" || c == "^");
        }

        static double calcRPN(string rpn)
        {
            string[] rpnTokens = rpn.Split(' ');
            Stack<double> stack = new Stack<double>();
            double n = 0.0d;

            foreach (string token in rpnTokens)
            {
                if (double.TryParse(token, out n))
                {
                    stack.Push(n);
                }
                else
                {
                    switch (token)
                    {
                        case "^":
                        case "pow":
                            {
                                n = stack.Pop();
                                stack.Push((double)Math.Pow((double)stack.Pop(), (double)n));
                                break;
                            }
                        case "ln":
                            {
                                stack.Push((double)Math.Log((double)stack.Pop(), Math.E));
                                break;
                            }
                        case "sqrt":
                            {
                                stack.Push((double)Math.Sqrt((double)stack.Pop()));
                                break;
                            }
                        case "sin":
                            {
                                stack.Push((double)Math.Sin((double)stack.Pop()));
                                break;
                            }
                        case "tan":
                            {
                                stack.Push((double)Math.Tan((double)stack.Pop()));
                                break;
                            }
                        case "cos":
                            {
                                stack.Push((double)Math.Cos((double)stack.Pop()));
                                break;
                            }
                        case "*":
                            {
                                stack.Push(stack.Pop() * stack.Pop());
                                break;
                            }
                        case "/":
                            {
                                n = stack.Pop();
                                stack.Push(stack.Pop() / n);
                                break;
                            }
                        case "+":
                            {
                                stack.Push(stack.Pop() + stack.Pop());
                                break;
                            }
                        case "-":
                            {
                                n = stack.Pop();
                                stack.Push(stack.Pop() - n);
                                break;
                            }
                        default:
                            //Exception e = new Exception();
                            //MessageBox.Show(e.Message, "Error evaluating RPN expression", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                    }
                }
            }

            return stack.Pop();
        }

        private void calcRes()
        {
            if (txtInput.Text != ".")
            {
                ScndOprnd = Convert.ToDouble(txtInput.Text);
                if (CC == true)
                {
                    FstOprnd = ScndOprnd;
                }
                else if (AddFlag == true)
                {
                    FstOprnd = FstOprnd + ScndOprnd;
                }
                else if (SubFlag == true)
                {
                    FstOprnd = FstOprnd - ScndOprnd;
                }
                else if (MultFlag == true)
                {
                    FstOprnd = FstOprnd * ScndOprnd;
                }
                else if (DivFlag == true)
                {
                    FstOprnd = FstOprnd / ScndOprnd;
                }
                else if (ModFlag == true)
                {

                    ScndOprnd = Convert.ToInt32(txtInput.Text);
                    FstOprnd = Convert.ToInt32(FstOprnd % ScndOprnd);
                }

                else
                {
                    FstOprnd = ScndOprnd;
                }
                txtInput.Text = Convert.ToString(FstOprnd);

            }
        }

        // Inverse Hyperbolic Sin
        public static double Asinh(double x)
        {
            return Math.Log(x + Math.Sqrt(x * x + 1));
        }

        // Inverse Hyperbolic Cos 
        public static double Acosh(double x)
        {
            return Math.Log(x + Math.Sqrt(x * x - 1));
        }

        // Inverse Hyperbolic Tan 
        public static double Atanh(double x)
        {
            return Math.Log((1 + x) / (1 - x)) / 2;
        }

        private void clearFlags()
        {
            AddFlag = false;
            SubFlag = false;
            MultFlag = false;
            DivFlag = false;
            PowFlag = false;
            ModFlag = false;
        }

        //Square Root Button
        private void button11_Click(object sender, EventArgs e)
        {
            double sqrt, res;
            sqrt = Convert.ToDouble(txtInput.Text);
            res = Math.Sqrt(sqrt);
            txtInput.Text = Convert.ToString(res);
        }

        //Zero Button
        private void button37_Click(object sender, EventArgs e)
        {
            if (CC == false)
            {
                txtInput.Text = txtInput.Text + Convert.ToString(0);
            }
            else if (CC == true)
            {
                txtInput.Text = Convert.ToString(0);
                CC = false;
            }
        }

        //7 Button
        private void button43_Click(object sender, EventArgs e)
        {
            if (CC == false)
            {
                txtInput.Text = txtInput.Text + Convert.ToString(7);
            }
            else if (CC == true)
            {
                txtInput.Text = Convert.ToString(7);
                CC = false;
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            txtInput.Clear();
            txtInput.Text = Convert.ToString(0);
            CC = true;
            FstOprnd = 0;
            ScndOprnd = 0;
            clearFlags();
        }

        //Sin Button
        private void button40_Click(object sender, EventArgs e)
        {
            if (InvsFlag == false)
            {
                try
                {
                    if (txtInput.Text.Length != 0)
                    {
                        double res;
                        res = Math.Sin(Convert.ToDouble(txtInput.Text));
                        txtInput.Text = Convert.ToString(res);
                    }
                    CC = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                try
                {
                    if (txtInput.Text.Length != 0)
                    {
                        double res;
                        res = Math.Asin(Convert.ToDouble(txtInput.Text));
                        txtInput.Text = Convert.ToString(res);
                    }
                    CC = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        //Factorial Button
        private void button42_Click(object sender, EventArgs e)
        {
            try
            {
                double fact = 1;
                for (int i = 1; i <= Convert.ToInt32(txtInput.Text); i++)
                {
                    fact = fact * i;
                }
                txtInput.Text = Convert.ToString(fact);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //1 Button
        private void button47_Click(object sender, EventArgs e)
        {
            if (CC == false)
            {
                txtInput.Text = txtInput.Text + Convert.ToString(1);
            }
            else if (CC == true)
            {
                txtInput.Text = Convert.ToString(1);
                CC = false;
            }
        }

        private void txtInput_TextChanged(object sender, EventArgs e)
        {

        }

        //2 Button
        private void button26_Click(object sender, EventArgs e)
        {
            if (CC == false)
            {
                txtInput.Text = txtInput.Text + Convert.ToString(2);
            }
            else if (CC == true)
            {
                txtInput.Text = Convert.ToString(2);
                CC = false;
            }
        }

        //3 Button
        private void button22_Click(object sender, EventArgs e)
        {
            if (CC == false)
            {
                txtInput.Text = txtInput.Text + Convert.ToString(3);
            }
            else if (CC == true)
            {
                txtInput.Text = Convert.ToString(3);
                CC = false;
            }
        }

        //4 Button
        private void button46_Click(object sender, EventArgs e)
        {
            if (CC == false)
            {
                txtInput.Text = txtInput.Text + Convert.ToString(4);
            }
            else if (CC == true)
            {
                txtInput.Text = Convert.ToString(4);
                CC = false;
            }
        }

        //5 Button
        private void button25_Click(object sender, EventArgs e)
        {
            if (CC == false)
            {
                txtInput.Text = txtInput.Text + Convert.ToString(5);
            }
            else if (CC == true)
            {
                txtInput.Text = Convert.ToString(5);
                CC = false;
            }
        }

        //6 Button
        private void button21_Click(object sender, EventArgs e)
        {
            if (CC == false)
            {
                txtInput.Text = txtInput.Text + Convert.ToString(6);
            }
            else if (CC == true)
            {
                txtInput.Text = Convert.ToString(6);
                CC = false;
            }
        }

        //8 Button
        private void button24_Click(object sender, EventArgs e)
        {
            if (CC == false)
            {
                txtInput.Text = txtInput.Text + Convert.ToString(8);
            }
            else if (CC == true)
            {
                txtInput.Text = Convert.ToString(8);
                CC = false;
            }
        }

        //9 Button
        private void button20_Click(object sender, EventArgs e)
        {
            if (CC == false)
            {
                txtInput.Text = txtInput.Text + Convert.ToString(9);
            }
            else if (CC == true)
            {
                txtInput.Text = Convert.ToString(9);
                CC = false;
            }
        }

        //Cos Button
        private void button52_Click(object sender, EventArgs e)
        {
            if (InvsFlag == false)
            {
                try
                {
                    if (txtInput.Text.Length != 0)
                    {
                        double res;
                        res = Math.Cos(Convert.ToDouble(txtInput.Text));
                        txtInput.Text = Convert.ToString(res);
                    }
                    CC = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                try
                {
                    if (txtInput.Text.Length != 0)
                    {
                        double res;
                        res = Math.Acos(Convert.ToDouble(txtInput.Text));
                        txtInput.Text = Convert.ToString(res);
                    }
                    CC = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        //Tan Button
        private void button53_Click(object sender, EventArgs e)
        {
            if (InvsFlag == false)
            {
                try
                {
                    if (txtInput.Text.Length != 0)
                    {
                        double res;
                        res = Math.Tan(Convert.ToDouble(txtInput.Text));
                        txtInput.Text = Convert.ToString(res);
                    }
                    CC = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                try
                {
                    if (txtInput.Text.Length != 0)
                    {
                        double res;
                        res = Math.Atan(Convert.ToDouble(txtInput.Text));
                        txtInput.Text = Convert.ToString(res);
                    }
                    CC = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }

        //Hyperbolic Sin Button
        private void button39_Click(object sender, EventArgs e)
        {
            if (InvsFlag == false)
            {
                try
                {
                    if (txtInput.Text.Length != 0)
                    {
                        double res;
                        res = Math.Sinh(Convert.ToDouble(txtInput.Text));
                        txtInput.Text = Convert.ToString(res);
                    }
                    CC = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                try
                {
                    if (txtInput.Text.Length != 0)
                    {
                        double res;
                        res = Asinh(Convert.ToDouble(txtInput.Text));
                        txtInput.Text = Convert.ToString(res);
                    }
                    CC = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        //Hyperbolic Cos Button
        private void button55_Click(object sender, EventArgs e)
        {
            if (InvsFlag == false)
            {
                try
                {
                    if (txtInput.Text.Length != 0)
                    {
                        double res;
                        res = Math.Cosh(Convert.ToDouble(txtInput.Text));
                        txtInput.Text = Convert.ToString(res);
                    }
                    CC = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                try
                {
                    if (txtInput.Text.Length != 0)
                    {
                        double res;
                        res = Acosh(Convert.ToDouble(txtInput.Text));
                        txtInput.Text = Convert.ToString(res);
                    }
                    CC = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }

        //Tan Hyperbolic Button
        private void button54_Click(object sender, EventArgs e)
        {
            if (InvsFlag == false)
            {
                try
                {
                    if (txtInput.Text.Length != 0)
                    {
                        double res;
                        res = Math.Tanh(Convert.ToDouble(txtInput.Text));
                        txtInput.Text = Convert.ToString(res);
                    }
                    CC = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                try
                {
                    if (txtInput.Text.Length != 0)
                    {
                        double res;
                        res = Atanh(Convert.ToDouble(txtInput.Text));
                        txtInput.Text = Convert.ToString(res);
                    }
                    CC = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        //PI Button
        private void button30_Click(object sender, EventArgs e)
        {
            txtInput.Text = Math.PI.ToString();
            CC = true;
        }

        //Ln Button
        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtInput.Text.Length != 0)
                {
                    double res;
                    res = Math.Log(Convert.ToDouble(txtInput.Text), Math.E);
                    txtInput.Text = Convert.ToString(res);
                }
                CC = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Log Button
        private void button35_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtInput.Text.Length != 0)
                {
                    double res;
                    res = Math.Log(Convert.ToDouble(txtInput.Text));
                    txtInput.Text = Convert.ToString(res);
                }
                CC = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //1/x Button
        private void button13_Click(object sender, EventArgs e)
        {
            txtInput.Text = (1 / Convert.ToDouble(txtInput.Text)).ToString();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            if (txtInput.Text.Length != 0)
            {
                calcRes();
                clearFlags();
                ModFlag = true;
                CC = true;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (CC == false)
            {
                txtInput.Text = txtInput.Text + " " + ")";
            }
            else if (CC == true)
            {
                txtInput.Text = ")" + " ";
                CC = false;
            }
        }

        //Clear Button
        private void button9_Click(object sender, EventArgs e)
        {
            txtInput.Clear();
            txtInput.Text = Convert.ToString(0);
            CC = true;
            FstOprnd = 0;
            ScndOprnd = 0;
            clearFlags();
        }

        //Decimal point button
        private void button23_Click(object sender, EventArgs e)
        {
            int i = 0;
            char chr = '\0';
            bool isDecimal = false;
            int length = txtInput.Text.Length;
            if (CC != true)
            {
                for (i = 0; i < length; i++)
                {
                    chr = txtInput.Text[i];
                    if (chr == '.')
                    {
                        isDecimal = true;
                    }
                }

                if (isDecimal != true)
                {
                    txtInput.Text = txtInput.Text + Convert.ToString(".");
                }
            }
        }

        //Inverse Button
        private void button3_Click(object sender, EventArgs e)
        {
            if (InvsFlag == false)
            {
                button53.Text = "tan-1";
                button54.Text = "tanh-1";
                button40.Text = "sin-1";
                button39.Text = "sinh-1";
                button52.Text = "cos-1";
                button55.Text = "cosh-1";
                InvsFlag = true;
            }
            else
            {
                button53.Text = "tan";
                button54.Text = "tanh";
                button40.Text = "sin";
                button39.Text = "sinh";
                button52.Text = "cos";
                button55.Text = "cosh";
                InvsFlag = false;
            }
        }

        private void button19_Click(object sender, EventArgs e)
        {
            if (txtInput.Text.Length != 0)
            {
                calcRes();
                clearFlags();
                AddFlag = true;
                CC = true;
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            if (txtInput.Text.Length != 0)
            {
                calcRes();
                clearFlags();
            }
            CC = true;
        }

        private void button31_Click(object sender, EventArgs e)
        {
            //string rpn = "3 4 2 * 1 5 - 2 3 ^ ^ / +";
            string rpn = txtInput.Text;
            double res = calcRPN(rpn);
            txtInput.Text = Convert.ToString(res);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        //Bingo Button
        private void button1_Click(object sender, EventArgs e)
        {
            string infix = txtInput.Text;
            string rpn = toRPN(infix);
            double res = calcRPN(rpn);
            txtInput.Text = Convert.ToString(res);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            //panel2.Location = new Point(30, 270);
            //panel2.Size = new Size(532, 216);
            //txtInput.Text = "f(x) = ";
            //this.chart1 = null;
            //chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();

            for (int i = 0; i < 9; i++)
            {
                chart1.Series["test1"].Points.AddXY
                                (i, 0);
            }
            chart1.Series["test1"].Points.AddXY(10, 10);
            //chart1.Series["test1"].Points.Add(0);
            //chart1.Series["test1"].ChartType = SeriesChartType.FastLine;
            chart1.Series["test1"].ChartType = SeriesChartType.Point;
            //SktchFlag = true;
           

        }



        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            /*if (SktchFlag == true)
            {*/
                Pen p = new Pen(Color.Green, 1.5f);
                Graphics g = e.Graphics;
                /*Point[] pts = { new Point(0, 0), new Point(10, 10), new Point(20, 20), new Point(30, 30), 
                                  new Point(40, 40), new Point(40, 50), new Point(40, 60), new Point(40, 70), 
                                  new Point(40, 80), new Point(50, 50), new Point(50, 40), new Point(50, 70),
                                  new Point(60, 50), new Point(60, 40), new Point(60, 70), new Point(60, 50), 
                                  new Point(70, 40), new Point(70, 70), new Point(150, 150), new Point(120, 40), new Point(50, 170) };*/

               /*Point[] pts = convertShit(txtInput.Text, 
                    Convert.ToDouble(textBox1.Text), 
                    Convert.ToDouble(textBox2.Text),
                    Convert.ToDouble(textBox3.Text));*/
                /*Point[] pts = convertShit("3 ^ 4 - ( 11 - ( 3 * x ) ) / 2",
                        0,
                        1000,
                        1);*/
                Point[] pts = convertShit(" x ^ 2  - ( 11 - ( 3 * x ) ) / 2 ",
                            0,
                            1000,
                            1);
                for (int i = 0; i < pts.Length; i++)
                {
                    if (i != 0)
                    {
                        g.DrawLine(p, pts[i - 1], pts[i]);
                        //g.Dispose();
                    }
                    else
                    {
                        g.DrawLine(p, pts[i], pts[i]);
                        //g.DrawLine(p, 10, 20, 20, 30);
                        //g.Dispose();
                    }
                }
            //}
        }

        private void button36_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtInput.Text.Length != 0)
                {
                    double res;
                    res = Math.Pow(10 ,Convert.ToDouble(txtInput.Text));
                    txtInput.Text = Convert.ToString(res);
                }
                CC = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button41_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtInput.Text.Length != 0)
                {
                    double res;
                    res = Math.Pow(Convert.ToDouble(txtInput.Text), 2.0d);
                    txtInput.Text = Convert.ToString(res);
                }
                CC = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button50_Click(object sender, EventArgs e)
        {

        }

        //Exp Button
        private void button34_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtInput.Text.Length != 0)
                {
                    double res;
                    res = Math.Exp(Convert.ToDouble(txtInput.Text));
                    txtInput.Text = Convert.ToString(res);
                }
                CC = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Sketch Button
        private void button28_Click(object sender, EventArgs e)
        {
            /*string infix = txtInput.Text.Replace("x", Convert.ToString(2));
            txtInput.Text = infix;
            string rpn = toRPN(infix);
            double res = calcRPN(rpn);
            txtInput.Text = Convert.ToString(res);*/

            //Pen p = new Pen(Color.Green, 1.5f);
            /*Point[] pts = convertShit(" x ^ 2  - ( 11 - ( 3 * x ) ) / 2 ",
                        0,
                        1000,
                        1);*/
            //this.chart1 = null;
            //chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            //
            /*chartArea1.Name = "ChartArea1";
            chart1.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            chart1.Legends.Add(legend1);
            chart1.Location = new System.Drawing.Point(-38, 0);
            chart1.Name = "chart1";
            chart1.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.SeaGreen;
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.Legend = "Legend1";
            series1.Name = "test1";
            chart1.Series.Add(series1);
            chart1.Size = new System.Drawing.Size(672, 227);
            chart1.TabIndex = 0;
            chart1.Text = "chart1";*/
            // 
            try
            {
                Point[] pts = convertShit(txtInput.Text,
                        Convert.ToDouble(textBox1.Text),
                        Convert.ToDouble(textBox2.Text),
                        Convert.ToDouble(textBox3.Text));

                for (int i = 0; i < pts.Length; i++)
                {
                    chart1.Series["test1"].Points.AddXY
                                    (pts[i].X, pts[i].Y);
                }

                chart1.Series["test1"].ChartType = SeriesChartType.FastLine;
                SktchFlag = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button16_Click(object sender, EventArgs e)
        {
            if (txtInput.Text.Length != 0)
            {
                calcRes();
                clearFlags();
                DivFlag = true;
                CC = true;
            }
        }

        private void button18_Click(object sender, EventArgs e)
        {
            if (txtInput.Text.Length != 0)
            {
                calcRes();
                clearFlags();
                SubFlag = true;
                CC = true;
            }
        }

        private void button17_Click(object sender, EventArgs e)
        {
            if (txtInput.Text.Length != 0)
            {
                calcRes();
                clearFlags();
                MultFlag = true;
                CC = true;
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            string temp = "";
            string strng = txtInput.Text;
            for (int i = 0; i < strng.Length - 1; i++)
            {
                temp += strng[i];
            }
            txtInput.Text = temp;
            temp = "";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (CC == false)
            {
                txtInput.Text = txtInput.Text + " " + ")";
            }
            else if (CC == true)
            {
                txtInput.Text = ")";
                CC = false;
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (CC == false)
            {
                if (NegFlag == false)
                {
                    txtInput.Text = "-" + txtInput.Text;
                    NegFlag = true;
                }
                else{
                    double res = Convert.ToDouble(txtInput.Text);
                    res*=-1;
                    txtInput.Text = Convert.ToString(res);
            }
            /*else if (CC == true)
            {
                txtInput.Text = ")";
                CC = false;
            }*/
        }


    }

    }}

