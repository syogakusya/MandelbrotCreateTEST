using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MandelbrotTEST02
{
    public partial class MandelbrotForm : Form
    {
        Bitmap Bitmap = new Bitmap(1980, 1080);
        double ParDot = 0.0025;
        double CenterX = -0.5;
        double CenterY = 0;
        //int NumberOfTrials = 512;//発散試行回数
        int NumberOfTrials = 64;

        double RealMin
        {
            get { return CenterX - ParDot * Bitmap.Width / 2; }
        }

        double ImaginarylMin
        {
            get { return CenterY - ParDot * Bitmap.Height / 2; }
        }

        public MandelbrotForm()
        {
            CreateMandelbrotSetBitmap();
            ClientSize = new Size(Bitmap.Width, Bitmap.Height);
        }

        Color CreatColorFromInt(int count)
        {
            int r, g, b;
            int k = 32;

            int d = (count % k) * 256 / k;
            int m = (int)(d / 42.667);

            switch (m)
            {

                case 0: r = 0; g = 6 * d; b = 255; break;

                case 1: r = 0; g = 255; b = 255 - 6 * (d - 43); break;

                case 2: r = 6 * (d - 86); g = 255; b = 0; break;

                case 3: r = 255; g = 255 - 6 * (d - 129); b = 0; break;

                case 4: r = 255; g = 0; b = 6 * (d - 171); break;

                case 5: r = 255 - 6 * (d - 214); g = 0; b = 255; break;
                default: r = 0; g = 0; b = 0; break;
            }

            return Color.FromArgb(r, g, b);
        }
        bool IsMandelbrotSet(Complex c, ref int count)
        {
            Complex z = Complex.Zero;
            for (int i = 0; i < NumberOfTrials; i++)
            {
                z = z * z + c;
                if (z.Magnitude > 2)
                {
                    count = i;
                    return false;
                }
            }
            count = -1;
            return true;
        }

        void CreateMandelbrotSetBitmap()
        {
            Graphics graphics = Graphics.FromImage(Bitmap);
            graphics.Clear(Color.Black);
            graphics.Dispose();

            for (int x = 0; x < Bitmap.Width; x++)
            {
                for (int y = 0; y < Bitmap.Height; y++)
                {
                    int count = 0;
                    Complex c = new Complex(
                        RealMin + x * ParDot,
                        ImaginarylMin + y * ParDot
                    );
                    if (IsMandelbrotSet(c, ref count))
                    {
                        Bitmap.SetPixel(x, y, Color.Black);
                    }
                    else
                    {
                        Bitmap.SetPixel(x, y, CreatColorFromInt(count));
                        //Bitmap.SetPixel(x, y, Color.LightSkyBlue);
                    }
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Bitmap clone = (Bitmap)Bitmap.Clone();
            clone.RotateFlip(RotateFlipType.RotateNoneFlipY);
            e.Graphics.DrawImage(clone, new Point(0, 0));
            clone.Dispose();
            Text = "Form1";

            base.OnPaint(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            double x = RealMin - e.X * ParDot;
            double y = ImaginarylMin + (Bitmap.Height - e.Y) * ParDot;
            Text = $"X = {x}, Y = {y} ParDot = {ParDot}, NumberOfTrials = {NumberOfTrials}";

            base.OnMouseMove(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            CenterX = e.X * ParDot + RealMin;
            CenterY = (Bitmap.Height - e.Y) * ParDot + ImaginarylMin;

            if (e.Button == MouseButtons.Left)
            {
                ParDot *= 0.5;
                NumberOfTrials += 10;
            }
            if (e.Button == MouseButtons.Right && NumberOfTrials > 30)
            {
                ParDot /= 0.5;
                // NumberOfTrials -= 10;
            }

            CreateMandelbrotSetBitmap();
            Invalidate();//再描画

            base.OnMouseDown(e);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            //if (e.Control)
            //{
            //    NumberOfTrials += 10;
            //}
            //if (e.Shift && NumberOfTrials > 30)
            //{
            //    NumberOfTrials -= 10;
            //}

            if (e.Control)
            {

                int num = 1;
                bool exist = false;
                Bitmap clone = (Bitmap)Bitmap.Clone();
                clone.RotateFlip(RotateFlipType.RotateNoneFlipY);

                string folderPath = Application.StartupPath + "\\img";
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);
                string filePath = folderPath + "\\" + "Mandelbrot" + num + ".png";
                while (File.Exists(filePath))
                {
                    num++;
                    filePath = folderPath + "\\" + "Mandelbrot" + num + ".png";
                }
                clone.Save(filePath, System.Drawing.Imaging.ImageFormat.Png);
                clone.Dispose();
            }

            base.OnKeyDown(e);
        }


    }
}
