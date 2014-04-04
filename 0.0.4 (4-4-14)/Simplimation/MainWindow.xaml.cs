using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.ComponentModel;
using System.IO;

namespace Simplimation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public FrameStack proj = new FrameStack();

        private int index = 0;
        private int thecount = 0;
        private bool playing = false;
        private bool paused = false;
        private bool playfrom = false;
        //timing
        private int tottime = 0;
        private int curtime = 0;
        
        private DispatcherTimer timestamp = new DispatcherTimer()
        {
            Interval = new TimeSpan(0, 0, 0, 0, 10), 
            //set to 10 ms.
        };
        //---
        private int selfordel;
        Grid frame;
        private DispatcherTimer dispatcherTimer = new DispatcherTimer();
        private List<Image> cinsel = new List<Image>();
        
        public MainWindow()
        {
            InitializeComponent();

            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            timestamp.Tick += new EventHandler(TickTock);


        }

        private void TickTock(object sender, EventArgs e)
        {
            int min = 0;
            int sec = 0;
            int MS = 0;
            curtime = curtime + 10;
            if (curtime > 60000)
            {
                min = curtime / 60000;
            }
            if (curtime > 1000)
            {
                sec = curtime - (min * 60000);
            }
            
            MS = sec;
            sec = sec / 1000;

            MS = MS - (sec * 1000);

            TimeShown.Text = min + ":" + sec +"."+ MS;

        }
        

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                remove();   
                e.Handled = true;
            }
        }



        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (thecount < proj.working.Count() - 1)
            {
                playing = true;
                thecount++;
                var switcho = proj.working[thecount].src;
                BitmapImage Changement = new BitmapImage(new Uri(switcho));
                int timing = proj.working[thecount].delay;
                dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, timing); //get's timing from delay;
                Big.Source = Changement; //changes big image

            }
            else
            {
                playing = false;
                dispatcherTimer.Stop();
                timestamp.Stop();
                thecount = -1;
               
                Play_Button.Source = new BitmapImage(new Uri("pack://application:,,,/Simplimation;component/graphics/play_o.png"));
           

            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog fileload = new Microsoft.Win32.OpenFileDialog();
            fileload.FileName = ""; // Default file name
            fileload.DefaultExt = ""; // Default file extension
            fileload.Filter = "PNG Image Files (.png)|*.png"; // Filter files by extension
            fileload.Multiselect = true;
            Nullable<bool> result = fileload.ShowDialog();

            if (result == true)
            {
                // Open document
                int reads = fileload.FileNames.Count();
                int cur = 0;
                foreach (String file in fileload.FileNames)
                {
                    string filename = fileload.FileNames[cur];

                    frame save = new frame()
                    {
                        src = filename,
                        delay = 100,
                    };

                    tottime = tottime + save.delay;

                    proj.append(save);
                    index = proj.working.Count() - 1;
                    add();
                    if (cur < reads)
                    {
                        cur++;
                    }
                }
                /*if (index > 5)
                {
                    index = index - 5;
                }
                else
                {
                    index = 0;
                }
                reconfig_bench();
                 */
                
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog fileload = new Microsoft.Win32.SaveFileDialog();
            fileload.FileName = ""; // Default file name
            fileload.DefaultExt = ""; // Default file extension
            fileload.Filter = "Simplimation Project File (.spf)|*.spf"; // Filter files by extension
            Nullable<bool> result = fileload.ShowDialog();


            if (result == true)
            {
                // Save document
                string filename = fileload.FileName;
                String delwr;
                String srcwr;
                using (StreamWriter sw = new StreamWriter(fileload.FileName))
                {
                    for (int i = 0; i < proj.working.Count(); i++)

                    {
                        delwr=proj.working[i].delay.ToString();
                        srcwr = proj.working[i].src.ToString();
                        sw.Write(i + ",");
                        sw.Write(srcwr+",");
                        sw.WriteLine(delwr);
                    
                    }

                }
                char[] slashsep = new char[] { '\\'};

                string[] titlechg = filename.Split(slashsep, StringSplitOptions.None);

                

                Title = (titlechg.Last() + " - Simplimation");
            }
            
        }

        private void reconfig_bench()
        {


     
        }

        private void add() 
        {
            Grid newFrame = new Grid();
            newFrame.Width = 160;
            newFrame.Height = 160;
            Grid fbg = new Grid();
            fbg.Width = 160;
            fbg.Height = 90;
            fbg.Background = Brushes.Black;
            newFrame.ShowGridLines.Equals(true);
            Image prev = new Image();
            Label delay = new Label();            
            prev.Height = 90;
            prev.Width = 160;
     

            Border brdr = new Border();
            brdr.BorderBrush = Brushes.Black;
            brdr.BorderThickness.Equals("1");
            brdr.Width = 160;
            brdr.Height = 90;
            prev.Source = new BitmapImage(new Uri(proj.working[index].src));
            prev.VerticalAlignment.Equals(VerticalAlignment.Bottom);
            newFrame.Uid = index.ToString();
            delay.Content = "Delay: " + proj.working[index].delay;
            delay.HorizontalContentAlignment.Equals(HorizontalAlignment.Center);
            delay.Foreground = Brushes.White;
            delay.FontStretch.Equals(true);
            newFrame.MouseLeftButtonDown +=new MouseButtonEventHandler(Bench_select);
            newFrame.Children.Add(delay);
            newFrame.Children.Add(fbg);
            newFrame.Children.Add(prev);
            newFrame.Children.Add(brdr);
            CineBench.Children.Add(newFrame);
            
            
        }
       
        private void Add_it_MouseEnter(object sender, MouseEventArgs e)
        {
            Add_it.Source = new BitmapImage(new Uri("pack://application:,,,/Simplimation;component/graphics/new_image_lite.png"));
        }

        private void Add_it_MouseLeave(object sender, MouseEventArgs e)
        {
            Add_it.Source = new BitmapImage(new Uri("pack://application:,,,/Simplimation;component/graphics/new_image.png"));
        }

        private void rem_it_MouseEnter(object sender, MouseEventArgs e)
        {
            rem.Source = new BitmapImage(new Uri("pack://application:,,,/Simplimation;component/graphics/rem_glow.png"));
        }

        private void rem_it_MouseLeave(object sender, MouseEventArgs e)
        {
            rem.Source = new BitmapImage(new Uri("pack://application:,,,/Simplimation;component/graphics/rem.png"));
        }


        private void Play_Button_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Play_Button.Source = new BitmapImage(new Uri("pack://application:,,,/Simplimation;component/graphics/pause_o.png"));
            timestamp.Start();
            dispatcherTimer.Start();
            
            paused = false;
                if (playing == true)
                {
                    timestamp.Stop();
                    dispatcherTimer.Stop();
                    playing = false;
                    paused = true;
                }
        }

        private void Play_Button_MouseEnter(object sender, MouseEventArgs e)
        {

            if (playing == true || paused == true)
            {
                Play_Button.Source = new BitmapImage(new Uri("pack://application:,,,/Simplimation;component/graphics/pause_o.png"));
            }
            else
            {
                Play_Button.Source = new BitmapImage(new Uri("pack://application:,,,/Simplimation;component/graphics/play_o.png")); 
            }
        }

        private void Play_Button_MouseLeave(object sender, MouseEventArgs e)
        {
            if (playing == true || paused == true)
            {
                Play_Button.Source = new BitmapImage(new Uri("pack://application:,,,/Simplimation;component/graphics/pause_i.png"));
            }
            else
            {
                Play_Button.Source = new BitmapImage(new Uri("pack://application:,,,/Simplimation;component/graphics/play_i.png"));
            }
        }


        private void con_beg_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            paused = false;
            if (playing == true)
            {
                dispatcherTimer.Stop();
                timestamp.Stop();
                playing = false;
                Play_Button.Source = new BitmapImage(new Uri("pack://application:,,,/Simplimation;component/graphics/play_o.png")); 
               
            }
            try
            {
                BitmapImage Changement = new BitmapImage(new Uri(proj.working[0].src));
                Big.Source = Changement;
            }
            catch
            {
                //do nothing
            }
                

        }

        private void con_beg_MouseEnter(object sender, MouseEventArgs e)
        {
            con_beg.Source = new BitmapImage(new Uri("pack://application:,,,/Simplimation;component/graphics/rev_hov.png"));
        }

        private void con_beg_MouseLeave(object sender, MouseEventArgs e)
        {
            con_beg.Source = new BitmapImage(new Uri("pack://application:,,,/Simplimation;component/graphics/rev.png"));
        }

        private void rem_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            remove();
        }

        private void Grid_SizeChanged(object sender, SizeChangedEventArgs e)
        {

        }

        private void Bench_select(object sender, MouseButtonEventArgs e)
        {
            Grid sel = (Grid)sender;
            
            List<Grid> turnoff = CineBench.Children.OfType<Grid>().ToList();




            for (int i = 0; i < turnoff.Count; i++ )
            {
                turnoff[i].Background = CineBench.Background;
            }

            sel.Background = Brushes.Gray;


            Big.Source = sel.Children.OfType<Image>().FirstOrDefault().Source;

            Big.Height = Ratio.Height;
            

            DelaySet.IsEnabled = true;

            int getDelay = Convert.ToInt32(sel.Uid);

            selfordel = getDelay;

            

            frame = sel;
            try
            { DelaySet.Text = proj.working[getDelay].delay.ToString(); }
            catch
            { DelaySet.Text = "0"; }
        }

        private void DelaySet_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {

                proj.working[selfordel].delay = Convert.ToInt32(DelaySet.Text);
                frame.Children.OfType<Label>().FirstOrDefault().Content = "Delay: " + Convert.ToInt32(DelaySet.Text);

            }
            catch
            {
                //do nothing
            }
        }

        private void exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void New_Click(object sender, RoutedEventArgs e)
        {
            savechanges();   
        }
        

        private void savechanges()
        {
            string messageBoxText = "Do you wish to save changes to your current project?";
            string caption = "Unsaved Changes";
            MessageBoxButton button = MessageBoxButton.YesNoCancel;
            MessageBoxImage icon = MessageBoxImage.Warning;
            MessageBoxResult result = MessageBox.Show(messageBoxText, caption, button, icon);

            switch (result)
            {
                case MessageBoxResult.Yes:
                    save.RaiseEvent(new RoutedEventArgs(MenuItem.ClickEvent));
                    break;

                case MessageBoxResult.No:
                    proj.working.Clear();
                    CineBench.Children.Clear();
                    Big.Source = new BitmapImage(new Uri("pack://application:,,,/Simplimation;component/graphics/nonbut.png")); 
                    break;

                case MessageBoxResult.Cancel:
                    //do nothing
                    break;
            }

        }

        private void open_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog fileload = new Microsoft.Win32.OpenFileDialog();
            fileload.FileName = ""; // Default file name
            fileload.DefaultExt = ""; // Default file extension
            fileload.Filter = "Simplimation Project Files (.spf)|*.spf"; // Filter files by extension
            Nullable<bool> result = fileload.ShowDialog();

             if (result == true)
            {
                // Open document
                string line;
                // Kill content
                proj.working.Clear();
                CineBench.Children.Clear();
                Big.Source = new BitmapImage(new Uri("pack://application:,,,/Simplimation;component/graphics/nonbut.png")); 
                // read and store
                 using (StreamReader sr = new StreamReader(fileload.FileName))
                {
                    while (sr.EndOfStream.Equals(false))
                    {
                        line = sr.ReadLine();
                        char[] comsep = new char[] { ',' };

                        string[] data = line.Split(comsep, StringSplitOptions.None);

                        string f = data[1];
                        string d = data[2];

                        frame save = new frame()
                        {
                            src = f,
                            delay = Convert.ToInt32(d),
                        };

                        proj.append(save);

                        index = proj.working.Count() - 1;
                        add();
                    }
                }
                 char[] slashsep = new char[] { '\\' };

                 string[] titlechg = fileload.FileName.Split(slashsep, StringSplitOptions.None);



                 Title = (titlechg.Last() + " - Simplimation"); 
             }



        }

        private void About_Click(object sender, RoutedEventArgs e)

            /*
             * This method is used to display the about window
             * when "About Team" is clicked from the menu bar.
             * The about windows includes the current version
             * of the applicaiton, a short description of the 
             * application, and all team members and their roles.
             */

        {
            //create new Window1 object
            Window1 about_window = new Window1();

            //show about window
            about_window.Show();
        }

        private void Moveback_MouseDown(object sender, MouseButtonEventArgs e)
        {
            int sor = CineBench.Children.IndexOf(frame);

            if (sor > 0)
            {

                string itemR = proj.working[sor].src;
                int delR = proj.working[sor].delay;
                string itemL = proj.working[sor - 1].src;
                int delL = proj.working[sor - 1].delay;

                proj.working[sor].src = itemL;
                proj.working[sor].delay = delL;
                proj.working[sor - 1].src = itemR;
                proj.working[sor - 1].delay = delR;

                
                CineBench.Children.RemoveAt(sor); 
                CineBench.Children.Insert(sor - 1, frame);
            }
            else
            {
                //should not work!
            }
            
        }

        private void MoveFor_MouseDown(object sender, MouseButtonEventArgs e)
        {
            int sor = CineBench.Children.IndexOf(frame);

            if (sor < proj.working.Count-1)
            {

                string itemR = proj.working[sor].src;
                int delR = proj.working[sor].delay;
                string itemL = proj.working[sor + 1].src;
                int delL = proj.working[sor + 1].delay;

                proj.working[sor].src = itemL;
                proj.working[sor].delay = delL;
                proj.working[sor + 1].src = itemR;
                proj.working[sor + 1].delay = delR;


                CineBench.Children.RemoveAt(sor);
                CineBench.Children.Insert(sor + 1, frame);
            }
            else
            {
                //should not work!
            }
        }

        private void remove()
        {
            int sor = CineBench.Children.IndexOf(frame);

            try
            {
                proj.working.RemoveAt(sor);
                CineBench.Children.RemoveAt(sor);
                if (sor > 1)
                {
                    Grid newi = (Grid)CineBench.Children[sor];
                    newi.RaiseEvent(new RoutedEventArgs(Grid.MouseLeftButtonDownEvent));
                }
            }
            catch
            {
                //nope
            }
            
        }

        private void Play_Point_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Image change = new Image();



            if (playfrom == false)
            {
                playfrom = true;

            }
            else
            {
                playfrom = false;
            }
        }
    
    }

    

        
}
