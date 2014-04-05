using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows.Controls;

namespace Simplimation
{
    public class frame : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string src
        {
            get;
            set;
        }

        public int delay
        {
            get;
            set;
        }

        public Image prev
        {
            get;
            set;
        }

        protected void OnPropertyChanged(string info)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(info));
            }
        }

    }
     
   
      
   
    
    
}


