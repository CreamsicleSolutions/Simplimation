using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
using AForge.Video.VFW;
using AviFile;

namespace Simplimation
{
    class AviWriter
    {
        // private variables for Avi
        private List<double> del = new List<double>();
        private List<Bitmap> bitmaps = new List<Bitmap>();
        private double min;
        private int height = 480;
        private int width = 848;
        private double opfr;

        // constructor
        public AviWriter(List<frame> Frame, String filename, int w, int h)
        {
            width = w;
            height = h;
            Console.WriteLine("Frame.Count: {0}", Frame.Count);
            string path = Environment.CurrentDirectory + "\\Images\\169.png";
            Image blank = Image.FromFile(path);
            Bitmap blankbuf = new Bitmap(blank, width, height);

            bitmaps.Add(blankbuf);

            del.Add(1);

            for (int i = 0; i < Frame.Count; i++)
            {
                del.Add((((double)Frame[i].delay) / 1000.00));
                string FileName  = Frame[i].src;
                Image img = Image.FromFile(FileName);
                Bitmap bitmap = new Bitmap(img, width, height);
                bitmaps.Add(bitmap);

                Console.WriteLine("Frame: {0}", i, " delay: {1}", ((double)Frame[i].delay / 1000.00), "src: {2}", Frame[i].src);
            }

            min = del.Min();
            opfr = 1000 / (min * 1000);
            AviManager aviManager = new AviManager(filename, false);
            VideoStream aviStream = aviManager.AddVideoStream(false, opfr, bitmaps[0]);
            Console.WriteLine("min: {0}", min);
            

            


            for(int i = 0; i < bitmaps.Count; i++)
            {

                if (del[i] > min)
                {
                    double j = del[i];
                    while ( j > min)
                    {
                        Bitmap bit = new Bitmap(bitmaps[i], width, height);
                        aviStream.AddFrame(bit);
                        j = j - min;
                    }

                }
                else
                {
                    aviStream.AddFrame(bitmaps[i]);
                }

                
                //Console.WriteLine("Frame added");
             /*   
                for(double j = min; j <= del[i];)
                {
                    aviStream.AddFrame(bitmaps[i]);
                    j = j + min;

                    Console.WriteLine("j: {0}", j);
                }
              */
            }

            aviManager.Close();
            del.Clear();
        }

        // methods
        double FindMin(List<double> values) { return values.Min(); }

        bool MilliSecondsToSeconds(ref List<double> ms)
        {
            return true;
        }

        double MilliSecondsToSecond(ref double ms) { return (ms / 1000); }

    }
}
