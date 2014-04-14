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
        private bool loopon = false;
        //timing
        private int tottime = 0;
        private int curtime = 0;
        // hold about window in case of closure of main window.
        Window holdforclose;
        //---
        private int selfordel;
        private Grid selection;
        Grid frame;
        private DispatcherTimer dispatcherTimer = new DispatcherTimer();
        private List<Image> cinsel = new List<Image>();
        
        public MainWindow()
        {
            InitializeComponent();

            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);


        }

        //updates timer.

        private void TickTock()
        {
            //
            int min = 0;
            int sec = 0;
            int MS = 0;
            //leading zeros
            string mfill = "";
            string secfill = "";
            string msfill = "";
            if (curtime > 60000)
            {
                min = curtime / 60000;
            }
            if (curtime > 1000)
            {
                sec = curtime - (min * 60000);
            }

          
          

            sec = sec / 1000;
            MS = curtime - (min * 60000);
            MS = MS - (sec * 1000);

        
            if (MS == 1000)
            {
                sec++;
                MS -= 1000;
            }
            
     
            if (MS < 10)
            {
                msfill = "00";
            }
            else if (MS < 100)
            {
                msfill = "0";
            }
           

       

            if (sec < 10)
            {
                secfill = "0";
            }

            if (min < 10)
            {
                mfill = "0";
            }

            TimeShown.Text = min + mfill + ":" + secfill + sec + "." + msfill + MS;


        }
        

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                remove();   
                e.Handled = true;
            }
        }

        /* Function: dispatcherTimer_Tick
         * Parameters: N/A
         * Purpose: Used for setting delays and keeping track of timing between images.
         * 
         */


        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (thecount < proj.working.Count() - 1)
            {
                TickTock();
                playing = true;
                thecount++;
                var switcho = proj.working[thecount].src;
                BitmapImage Changement = new BitmapImage(new Uri(switcho));
                int timing = proj.working[thecount].delay;
                curtime = curtime + timing;
               
                dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, timing); //get's timing from delay;
                Big.Source = Changement; //changes big image after delay time has passed, creating animation!
                
               

            }
            else if (loopon == true)
            {
                playing = false;
                dispatcherTimer.Stop();
                curtime = 0;
                TimeShown.Text = "00:00.000";
                thecount = -1;
                dispatcherTimer.Start();
            }
            else
            {
                playing = false;
                dispatcherTimer.Stop();
                curtime = 0;
                TimeShown.Text = "00:00.000";
                thecount = -1;
               
                Play_Button.Source = new BitmapImage(new Uri("pack://application:,,,/Simplimation;component/graphics/play_i.png"));


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
                    add();  //invokes add function to create UI element.
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


        /* Function: add
         * Parameters: N/A
         * Purpose: This is used to create new items based on frame objects. These items are connected to a grid, then added
         * to the storyboard as one object. This is also used when loading a file.
         */

        private void add() 
        {
            //newFrame hold everything
            Grid newFrame = new Grid();
            newFrame.Width = 160;
            newFrame.Height = 160;
            //fbg is the frame's background, in case on non-16:9 images.
            Grid fbg = new Grid();
            fbg.Width = 160;
            fbg.Height = 90;
            fbg.Background = Brushes.Black;
            newFrame.ShowGridLines.Equals(true);
            //prev/delay correspond to frame object that was just created.
            Image prev = new Image();
            Label delay = new Label();            
            prev.Height = 90;
            prev.Width = 160;
     
            //this is for a border, not sure if it works.
            Border brdr = new Border();
            brdr.BorderBrush = Brushes.Black;
            brdr.BorderThickness.Equals("1");
            brdr.Width = 160;
            brdr.Height = 90;
            //this places image in grid
            prev.Source = new BitmapImage(new Uri(proj.working[index].src));
            prev.VerticalAlignment.Equals(VerticalAlignment.Bottom);
            newFrame.Uid = index.ToString();
            //this places frame delay, added ms.
            delay.Content = "Delay: " + proj.working[index].delay + " ms";
            delay.HorizontalContentAlignment.Equals(HorizontalAlignment.Center);
            delay.Foreground = Brushes.White;
            delay.FontStretch.Equals(true);
            //this makes a new event for clicking it, also make things into children of newframe.
            newFrame.MouseLeftButtonDown +=new MouseButtonEventHandler(Bench_select);
            newFrame.MouseEnter += new MouseEventHandler(Bench_hover);
            newFrame.MouseLeave += new MouseEventHandler(Bench_unhover);
            newFrame.Children.Add(delay);
            newFrame.Children.Add(fbg);
            newFrame.Children.Add(prev);
            newFrame.Children.Add(brdr);
            //newFrame is added as a child to CineBench, which is code for storyboard
            CineBench.Children.Add(newFrame);
            
            
        }

        private void Bench_unhover(object sender, MouseEventArgs e)
        {
           
            Grid sel = (Grid)sender;

            if (sel != selection)
            {
                sel.Background = CineBench.Background;
            }
        }

        private void Bench_hover(object sender, MouseEventArgs e)
        {
            Grid sel = (Grid)sender;
           
            if (sel != selection)
            {
                sel.Background = Brushes.DimGray;
            }
        }
       
        // the following code is only used for giving the buttons a highlighting effect when moused-over.

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


        /* Function: Play_button
         * Parameters: N/A
         * Purpose: This is the event that plays the storyboard
         * 
         */

        private void Play_Button_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Play_Button.Source = new BitmapImage(new Uri("pack://application:,,,/Simplimation;component/graphics/pause_o.png"));
            dispatcherTimer.Start();
            
            paused = false;
                if (playing == true)
                {
                    dispatcherTimer.Stop();
                    playing = false;
                    paused = true;
                }
        }

        private void Loop_button_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
           

            if (loopon == true)
            {
                Loop_button.Source = new BitmapImage(new Uri("pack://application:,,,/Simplimation;component/graphics/looper1.png"));
                loopon = false;
            }
            else
            {
                Loop_button.Source = new BitmapImage(new Uri("pack://application:,,,/Simplimation;component/graphics/looper3.png"));
                loopon = true;
            }
           
        }

        //these events are for mouse-over graphics

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


        //this is for playing from beginning

        private void con_beg_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            paused = false;
            TimeShown.Text = "00:00.000";
            if (playing == true)
            {
                dispatcherTimer.Stop();
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
        private void con_end_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            int maxframe = proj.working.Count() - 1;
            paused = false;

            thecount = maxframe;
            curtime = 0;

            for (int i = 0; i < thecount; i++)
            {
                curtime = curtime + proj.working[i].delay;
            }

            if (playing == true)
            {
                dispatcherTimer.Stop();
                playing = false;


                Play_Button.Source = new BitmapImage(new Uri("pack://application:,,,/Simplimation;component/graphics/play_o.png"));

            }
            try
            {
                BitmapImage Changement = new BitmapImage(new Uri(proj.working[maxframe].src));
                Big.Source = Changement;
            }
            catch
            {
                //do nothing
            }
        }


       

       

        //more mouse over

        private void con_beg_MouseEnter(object sender, MouseEventArgs e)
        {
            con_beg.Source = new BitmapImage(new Uri("pack://application:,,,/Simplimation;component/graphics/rev_hov.png"));
        }

        private void con_beg_MouseLeave(object sender, MouseEventArgs e)
        {
            con_beg.Source = new BitmapImage(new Uri("pack://application:,,,/Simplimation;component/graphics/rev.png"));
        }

        private void con_end_MouseEnter(object sender, MouseEventArgs e)
        {
            con_end.Source = new BitmapImage(new Uri("pack://application:,,,/Simplimation;component/graphics/for_hov.png"));
        }

        private void con_end_MouseLeave(object sender, MouseEventArgs e)
        {
            con_end.Source = new BitmapImage(new Uri("pack://application:,,,/Simplimation;component/graphics/for.png"));
        }

        private void play_beg_MouseEnter(object sender, MouseEventArgs e)
        {
            Play_Point.Source = new BitmapImage(new Uri("pack://application:,,,/Simplimation;component/graphics/playbeg_o.png"));
        }

        private void play_beg_MouseLeave(object sender, MouseEventArgs e)
        {
            Play_Point.Source = new BitmapImage(new Uri("pack://application:,,,/Simplimation;component/graphics/playbeg.png"));
        }

        private void Loop_button_MouseEnter(object sender, MouseEventArgs e)
        {
            if (loopon == false)
            {
                Loop_button.Source = new BitmapImage(new Uri("pack://application:,,,/Simplimation;component/graphics/looper1.png"));
            }
            else
            {
                Loop_button.Source = new BitmapImage(new Uri("pack://application:,,,/Simplimation;component/graphics/looper3.png"));
            }
        }

        private void Loop_button_MouseLeave(object sender, MouseEventArgs e)
        {
            if (loopon == false)
            {
                Loop_button.Source = new BitmapImage(new Uri("pack://application:,,,/Simplimation;component/graphics/looper0.png"));
            }
            else
            {
                Loop_button.Source = new BitmapImage(new Uri("pack://application:,,,/Simplimation;component/graphics/looper2.png"));
            }
        }

        private void Moveback_MouseEnter(object sender, MouseEventArgs e)
        {
            Moveback.Source = new BitmapImage(new Uri("pack://application:,,,/Simplimation;component/graphics/frame_b2x.png"));
        }

        private void Moveback_MouseLeave(object sender, MouseEventArgs e)
        {
            Moveback.Source = new BitmapImage(new Uri("pack://application:,,,/Simplimation;component/graphics/frame_b1.png"));
        }

        private void forward_MouseEnter(object sender, MouseEventArgs e)
        {
            forward.Source = new BitmapImage(new Uri("pack://application:,,,/Simplimation;component/graphics/frame_f2.png"));
        }

        private void forward_MouseLeave(object sender, MouseEventArgs e)
        {
            forward.Source = new BitmapImage(new Uri("pack://application:,,,/Simplimation;component/graphics/frame_f1.png"));
        }

        private void rem_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            remove();
        }

        private void Grid_SizeChanged(object sender, SizeChangedEventArgs e)
        {

        }

        private void new_docu_MouseEnter(object sender, MouseEventArgs e)
        {
            new_docu.Source = new BitmapImage(new Uri("pack://application:,,,/Simplimation;component/graphics/new_docu1.png"));

        }


        private void new_docu_MouseLeave(object sender, MouseEventArgs e)
        {
            new_docu.Source = new BitmapImage(new Uri("pack://application:,,,/Simplimation;component/graphics/new_docu.png"));
        }


        private void Open_proj_MouseEnter(object sender, MouseEventArgs e)
        {
            Open_proj.Source = new BitmapImage(new Uri("pack://application:,,,/Simplimation;component/graphics/open_docu1.png"));

        }


        private void Open_proj_MouseLeave(object sender, MouseEventArgs e)
        {
            Open_proj.Source = new BitmapImage(new Uri("pack://application:,,,/Simplimation;component/graphics/open_docu.png"));
        }



        private void Save_proj_MouseEnter(object sender, MouseEventArgs e)
        {
            Save_proj.Source = new BitmapImage(new Uri("pack://application:,,,/Simplimation;component/graphics/save_docu1.png"));

        }


        private void Save_proj_MouseLeave(object sender, MouseEventArgs e)
        {
            Save_proj.Source = new BitmapImage(new Uri("pack://application:,,,/Simplimation;component/graphics/save_docu.png"));
        }
    

        /* Function: Bench_Select
         * Parameters: N/A
         * Purpose: This event is used to select the item on the storyboard the user wants.
         * 
         */

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

            int getDelay = Convert.ToInt32(CineBench.Children.IndexOf(sel));

            selfordel = getDelay;

            selection = sel;

            frame = sel;
            try
            { DelaySet.Text = proj.working[getDelay].delay.ToString(); }
            catch
            { DelaySet.Text = "0"; }
        }

        /* Function: DelaySet
         * Parameters: N/A
         * Purpose: Sets delay to a new delay.
         * 
         */

        private void DelaySet_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                proj.working[selfordel].delay = Convert.ToInt32(DelaySet.Text);
                frame.Children.OfType<Label>().FirstOrDefault().Content = "Delay: " + Convert.ToInt32(DelaySet.Text) + " ms";

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
            holdforclose = about_window;
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
                Big.Source = new BitmapImage(new Uri("pack://application:,,,/Simplimation;component/graphics/nonbut.png"));
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

            thecount = Convert.ToInt32(CineBench.Children.IndexOf(selection)-1);

            curtime = 0;

            for (int i = 0; i < thecount; i++ )
            {
                curtime = curtime + proj.working[i].delay;
            }

            dispatcherTimer.Start();

            paused = false;

        }


        /*  The following function is performed when the main window is closed, prevents about window from being persistant.
         * 
         * 
         */

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            try { holdforclose.Close(); }
            catch { //no about, do nothing
            }
        }

        

        

     

       

      

      
    }

    

        
}
