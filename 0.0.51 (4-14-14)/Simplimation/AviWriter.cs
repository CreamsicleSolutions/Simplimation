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
        // constructor
        public AviWriter(ref List<frame> Frame, ref String filename)
        {
            for (int i = 0; i < Frame.Count; i++)
            {
                stamps.Add((((double)Frame[i].delay) / 1000.00));
                string FileName  = Frame[i].src;
                Image img = Image.FromFile(FileName);
                Bitmap bitmap = new Bitmap(img, width, height);
                bitmaps.Add(bitmap);
            }

            min = stamps.Min();

            AviManager aviManager = new AviManager(filename, false);
            VideoStream aviStream = aviManager.AddVideoStream(false, min, bitmaps[0]);

            for(int i = 0; i < Frame.Count; i++)
            {
                
                for(double j = min; j < Frame[i].delay;)
                {
                    aviStream.AddFrame(bitmaps[i]);
                    j = j + min;
                }
            }

            aviManager.Close();
            stamps.Clear();
        }

        // methods
        double FindMin(List<double> values) { return values.Min(); }

        bool MilliSecondsToSeconds(ref List<double> ms)
        {
            return true;
        }

        double MilliSecondsToSecond(ref double ms) { return (ms / 1000); }

        // private variables
        private List<double> stamps = new List<double>();
        private List<Bitmap> bitmaps = new List<Bitmap>();
        private double min;
        private int height = 240;
        private int width = 320;
    }
}
