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
using System.IO.Ports;
using System.Windows.Threading;
using System.Text.RegularExpressions;
using Microsoft.Win32;
using System.IO;
using System.Threading;
using System.ComponentModel;

namespace ArduinoWPFProj
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string[] ports;
        public bool isConnected = false;
        public SerialPort arduinoPort;
        private SignalCollection signals;
        public ViewParams view;
        public ScaleTransform scale;
        public TranslateTransform pan;
        private Point outdragPoint;
        private Point indragPoint;
        private Point bardragPoint;
        private int barPos, inPos, outPos;
     

        public MainWindow()
        {
            
           
            view = new ViewParams();
            InitializeComponent();
            arduinoPort = new SerialPort();
            arduinoPort.DataBits = 8;
            arduinoPort.BaudRate = 9600;
            arduinoPort.Parity = Parity.None;
            arduinoPort.ReadTimeout = 200;
            if (comportList.Items.Count > 0)
            {
                arduinoPort.PortName = comportList.Items[0].ToString();
            }
            else
            {
                Console.WriteLine("No ports available");
                connectButton.IsEnabled = false;
            }
            scale = new ScaleTransform(1, 1, 0, 0);
            pan = new TranslateTransform(0, plot.Height);
            signals = new SignalCollection(this);
            signals.scaleSignalStrokes(scale);
            signals.updateLabels();
            resetTransform();
            cleargraphBtn_Click();
        }
        public double anchorIn, anchorOut = 0;
        public void resetTransform(Boolean useSlider = false)
        {
            Rect rectangleBounds = new Rect();
            rectangleBounds = plot.RenderTransform.TransformBounds(new Rect(0, 0, plot.Width, plot.Height));


            //Add transformgroup to plot
            double yscale = plot.Height / view.YMAX; //YMAX is maximum plot value received
            double xscale = plot.Width / view.XMAX; //XMAX is total ammount of plotted points
            Matrix m = new Matrix(1, 0, 0, 1, 0, 0);
            if (useSlider)
            {
                double maxVal = zoomBar.ActualWidth - outPoint.Width;
                double outP = Canvas.GetLeft(outPoint); //points position relative to the scrollbar
                double inP = Canvas.GetLeft(inPoint);
                double delta = (outP - inP);
                double factor = (maxVal / delta) * xscale;
                double mappedinP = (inP / maxVal) * view.XMAX;

                anchorOut = (outP / maxVal) * view.XMAX;
                anchorIn = (inP / maxVal) * view.XMAX;
                double center = (anchorOut + anchorIn) / 2;

                m.Translate(-anchorIn, 0); //Move graph to inpoint
                m.ScaleAt(factor, -yscale, 0, 0); //scale around the inpoint, with a factor so that outpoint is 600px further away
                m.Translate(0, plot.Height); //to compensate the flipped graph, move it back down
            }
            scale = new ScaleTransform(m.M11, m.M22, 0, 0); //save scale factors in a scaletransform for reference
            signals.scaleSignalStrokes(scale); //Scale the plotlines to compensate for canvas scaling
            MatrixTransform matrixTrans = new MatrixTransform(m); //Create matrixtransform
            plot.RenderTransform = matrixTrans; //Apply to canvas
            
        }

        private void cleargraphBtn_Click()
        {
            foreach (Signal signal in signals.signals)
            {
                signal.clear();
                
            }
        }

       

        private void RefreshPorts(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < comportList.Items.Count; i++)
            {
                comportList.Items.RemoveAt(i);
            }
            ports = SerialPort.GetPortNames();
            Console.WriteLine("ports:");
            foreach (string port in ports)
            {
                comportList.Items.Add(port.ToString());
                Console.WriteLine(port);
            }
        }

        private void zoomBar_Bar_Initialized(object sender, EventArgs e)
        { //Make rectangle stretch between in and out points
            Rectangle bar = (Rectangle)sender;
            Canvas.SetLeft(bar, inPoint.Width);
            bar.Width = Canvas.GetLeft(outPoint) - Canvas.GetLeft(inPoint) - inPoint.Width;
        }



        private void ChangePort(object sender,SelectionChangedEventArgs e) {
            if (comportList.SelectedItem != null) {
                arduinoPort.PortName = comportList.SelectedItem.ToString();
            }
            connectButton.IsEnabled=true;
        
        }



        public int ZarbeR=10;
        public int Takhir=1;
        public string Direc="L";

        public void Setset(object sender, RoutedEventArgs e)
        {


            


                    if (Rad1.IsChecked == true) { ZarbeR = 10; }
                    else if (Rad2.IsChecked == true) { ZarbeR = 20; }
                    else if (Rad3.IsChecked == true) { ZarbeR = 30; }

                    if (Radt1.IsChecked == true) { Takhir = 1; }
                    else if (Radt2.IsChecked == true) { Takhir = 2; }
                    else if (Radt3.IsChecked == true) { Takhir = 3; }

                    if (RadL.IsChecked == true) { Direc = "L"; }
                    else if (RadR.IsChecked == true) { Direc = "R"; }
                    else if (RadM.IsChecked == true) { Direc = "M"; }
                    else if (RadN.IsChecked == true) { Direc = "N"; }
                    Console.WriteLine(Direc + "\n Takhir:" + Takhir.ToString() + "\n Zarbe:" + ZarbeR.ToString());




                }






        public List<double> values1;
        public List<double> values2;
        public List<double> values3;

        public List<double> max1;
        public List<double> max2;
        public List<double> max3;


        double m1, m2, m3;
        int c1=0,c2=0,c3=0;bool flagrast=false,flagchap=false,flagcenter=false;bool flagincorrect=false;
        int c4, c5, c6; /*signal1=l    signal2=m   signal3=r   like m*/
       
        private void DispatcherTimer_Tick1(object sender, EventArgs e)
        {
            try
            {

                c4 = c1;
                c5 = c2;
                c6 = c3;
                c1 = signals.signals[1].values.Count();
                c2 = signals.signals[2].values.Count();
                c3 = signals.signals[3].values.Count();

                values1 = signals.signals[1].values.GetRange(c4, c1 - c4);
                values2 = signals.signals[2].values.GetRange(c5, c2 - c5);
                values3 = signals.signals[3].values.GetRange(c6, c3 - c6);


                
                

                m1 = int.Parse(values1.Max().ToString());
                m2 = int.Parse(values2.Max().ToString());
                m3 = int.Parse(values3.Max().ToString());



                if (flagkhord == false)
                {

                    flagchap = false;
                    flagcenter = false;
                    flagrast = false;

                    if (m1 > thresold && m1 > m2 && m1 > m3)
                    {
                        flagzarbchap = true;
                        max1.Add(m1);
                        flagkhord = true;

                    }
                    if (m2 > thresold && m2 > m1 && m2 > m3)
                    {
                        flagzarbcenter = true;
                        max2.Add(m2);
                        flagkhord = true;
                    }
                    if (m3 > thresold && m3 > m1 && m3 > m2)
                    {
                        flagzarbrast = true;
                        max3.Add(m3);
                        flagkhord = true;
                    }

                    if (c == "l" && flagzarbchap == true)
                    {
                        DateTime end = DateTime.Now;

                        sec = (end - start).Milliseconds;
                        Amalkardtime[trn] = double.Parse(sec.ToString());

                        flagzarbchap = false;
                        flagzarbrast = false;
                        flagzarbcenter = false;
                        flagkhord = true;
                        flagchap = true;


                    }


                    else if (c == "m" && flagzarbcenter == true)
                    {
                        DateTime end = DateTime.Now;

                        sec = (end - start).Milliseconds;
                        Amalkardtime[trn] = double.Parse(sec.ToString());

                        flagzarbchap = false;
                        flagzarbrast = false;
                        flagzarbcenter = false;
                        flagkhord = true;
                        flagcenter = true;
                    }


                    else if (c == "r" && flagzarbrast == true)
                    {
                        DateTime end = DateTime.Now;

                        sec = (end - start).Milliseconds;
                        Amalkardtime[trn] = double.Parse(sec.ToString());

                        flagzarbrast = false;
                        flagkhord = true;
                        flagrast = true;
                        flagzarbchap = false;

                        flagzarbcenter = false;
                    }
                    else if ((flagzarbrast && c != "r") || (flagzarbchap && c != "l") || (flagzarbcenter && c != "m"))
                    {
                        flagzarbrast = false;
                        flagzarbchap = false;
                        flagzarbcenter = false;
                        flagkhord = true;
                        flagincorrect = true;
                        DateTime end = DateTime.Now;

                        sec = (end - start).Milliseconds;
                        Amalkardtime[trn] = double.Parse(sec.ToString());

                    }
                }







                values1.Clear();
                values2.Clear();
                values3.Clear();
                Console.WriteLine(m1 + "\n" + m2 + "\n" + m3);
            }
            catch (InvalidOperationException)
            {

                MessageBox.Show("لطفا برنامه را مجددا ببندید و باز کنید-اتصال با برد برقرار نیست");
                th.Abort();

            }
            catch (ArgumentOutOfRangeException) {

                System.Diagnostics.Process.Start(AppDomain.CurrentDomain.BaseDirectory + "\\ArduinoWPFProj.exe");
                Application.Current.Shutdown();
            }
        }
        DateTime start; string c = "";
        double sec; int nazade = 0; bool flagzarbrast = false, flagzarbchap = false, flagzarbcenter = false; bool flagkhord = false;
        double[] Amalkardtime=new double[30];
        int[] amalrasttrue=new int[30];
        int[] amalchaptrue=new int[30];
        int[] amalvasattrue = new int[30];
        int[] amalfalse = new int[30];
        float[] domain = new float[30];
        public double thresold=100;
        public void Pulse() {
           c= anbar[trn].ToString();
            
            
            arduinoPort.WriteLine(c);
            start = DateTime.Now;
            
            

           
        
        }


        private int trn=0;
        private void DispatcherTimer_Tick2(object sender, EventArgs e)
        {

            
            Pulse();
            if (flagkhord)
            {
                if (flagrast)
                {
                    amalrasttrue[trn] = 1;
                    domain[trn] = int.Parse(max3.Max().ToString());
                }
                else if (flagcenter)
                {
                    amalvasattrue[trn] = 1;
                    domain[trn] = int.Parse(max2.Max().ToString());
                }
                else if (flagchap)
                {
                    amalchaptrue[trn] = 1;
                    domain[trn] = int.Parse(max1.Max().ToString());
                }

                else if (flagincorrect)
                {
                    amalfalse[trn] = 1;

                }
                flagkhord = false;
            }
            
            flagchap = false;
            flagrast = false;
            flagcenter = false;
            flagincorrect = false;
            try
            {
                max1.Clear();
                max2.Clear();
                max3.Clear();
            }
            catch(NullReferenceException) { }
            rightlbl.Content = amalrasttrue.Sum().ToString();
            leftlbl.Content = amalchaptrue.Sum().ToString();
            centerlbl.Content = amalvasattrue.Sum().ToString();
            falselbl.Content = amalfalse.Sum().ToString();
            trn++;
            

            if (trn == ZarbeR) {
                th.Abort();
                DispatcherTimer1.Stop();
                DispatcherTimer2.Stop();
                while (th.IsAlive) { };
               
                arduinoPort.WriteLine("n");







                

                    isConnected = false;
                savefiletext += "    ";
                foreach (double d in Amalkardtime)
                {
                    savefiletext += d.ToString() +"     ";
                }
                savefiletext += "\n Chap   ";
                foreach (double d in amalchaptrue)
                {
                    savefiletext += d.ToString() + "     ";

                }
                savefiletext += "\n RAST  ";
                foreach (double d in amalrasttrue)
                {
                    savefiletext += d.ToString() + "     ";
                }
                savefiletext += "\n VASAT  ";
                foreach (double d in amalvasattrue)
                {
                    savefiletext += d.ToString() + "     ";
                }
                savefiletext += "\n Incorrect  ";
                foreach (double d in amalfalse)
                {
                    savefiletext += d.ToString() + "     ";
                }
                savefiletext += "\n Domain  ";
                foreach (double d in domain)
                {
                    savefiletext += d.ToString() + "     ";
                }


                
                savefilee();

                cleargraphBtn_Click();

               
               

                settingbtn.IsEnabled = true;
                ExportBtn.IsEnabled = true;
                MessageBox.Show("عملیات با موفقیت انجام شد");

                
                
                
                resultshow();
                
                
            }
        }

        string anbar;
        public void Calckindofzarbe(int Z,string D) {
            
           
            switch(D){
                case "L":
                  
                    anbar = "lrmlrllrmmlrlmlrlmllmlrllrmlml";
                   
                    break;
                    
                case "R": 
                     anbar = "rlmrrlrlmrlrlrmrlmrlrlrlrrmrmr";
                     
                     break;
                case "M":
                     anbar = "mlmrlmlmrlrlrmrlmrlmlmrlrrmrmr";
                    
                    break;
                case "N": 
                     anbar = "lrmmrlrlmrmlmlrrlmlmrlrmlrmmrlm";
                 
                    break;
            }
            
            
        }

        
        public  BackgroundWorker worker = new BackgroundWorker();
        DispatcherTimer DispatcherTimer1 = new System.Windows.Threading.DispatcherTimer();
        DispatcherTimer DispatcherTimer2 = new System.Windows.Threading.DispatcherTimer();
        Thread th;
        public void ArduinoConnect(object sender, RoutedEventArgs e)
        {
            flagkhord = false;
            c1 = 0; c2 = 0; c3 = 0;
            max1=new List<double>();
            max2=new List<double>();
            max3 = new List<double>();


            try
            {
                thresold = int.Parse(threshtxt.Text.ToString());
            }
            catch (FormatException)
            {
            }
            Calckindofzarbe(ZarbeR,Direc);
         
            DispatcherTimer1.Tick += new EventHandler(DispatcherTimer_Tick1);
            DispatcherTimer1.Interval = new TimeSpan(0, 0, 0, 0, 100);
            DispatcherTimer1.Start();

            DispatcherTimer2.Tick += new EventHandler(DispatcherTimer_Tick2);
            DispatcherTimer2.Interval = new TimeSpan(0, 0, Takhir+1);
            DispatcherTimer2.Start();


            settingbtn.IsEnabled = false;
            ExportBtn.IsEnabled = false;


            th = new Thread(new ThreadStart(this.Graph));
           // worker.DoWork += Graph;
            //worker.WorkerSupportsCancellation = true;
            //worker.RunWorkerAsync();
            th.Start();
            
            

        }
        public void Graph()
        {
            
            
            try
            {
                
                
                    
                    signals.arduinoPort = arduinoPort;
                    signals.openPort();
                    arduinoPort.DiscardInBuffer();
                    
                    isConnected = true;
                   
                

            }



            catch (IOException)
            {
                
                connectButton.IsEnabled = true;
                comportList.IsEnabled = true;
                isConnected = false;
            }


        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            arduinoPort.Close();
            Main.Close();
        }
        private void MinWindow(object sender, RoutedEventArgs e)
        {
            Main.WindowState = System.Windows.WindowState.Minimized;
            Button btn = (Button)sender;
        }

        void DragWindow(object sender, MouseButtonEventArgs e)
        {
           
            this.DragMove();

        }
        private void MoreHelp(object sender, RoutedEventArgs e)
        {
            // Instantiate the dialog box
            helpDialogBox dlg = new helpDialogBox(this);

            // Configure the dialog box
            dlg.Owner = this;

            // Open the dialog box modally
            dlg.Show();
        }
        string savefiletext="";
        private void ExportData(object sender, RoutedEventArgs e)
        {
            savefilee();

        }

        public void savefilee(){
            savefiletext += "\n name:" + Name.Text+"       "+"Age : " + age.Text+"  tall : "+tall.Text+"  weight: "+weight.Text;
            FileStream fs;
            // Displays a SaveFileDialog so the user can save the export
            SaveFileDialog exportDialog = new SaveFileDialog();
            exportDialog.Filter = "please write .txt|*.txt";
            exportDialog.Title = "ذخیره اطلاعات";
            exportDialog.ShowDialog();
            if (exportDialog.FileName != "")
            {
                // Saves the export via a FileStream created by the OpenFile method.
                
                File.WriteAllText(exportDialog.FileName, savefiletext);
               
            }
            }
        private void disableDraw(object sender, MouseEventArgs e)
        {
            signals.drawmouse = false;
        }

        private void resultshow()
        {
            
       
            PLOT pl = new PLOT();
            pl.Owner = this;
            pl.Show();
            pl.andaze = domain;
            pl.rasttrue = amalrasttrue.Sum();
            pl.chaptrue = amalchaptrue.Sum();
            pl.vasattrue = amalvasattrue.Sum();
            pl.falsezarbe = amalfalse.Sum();
            pl.takhir = Amalkardtime;
            pl.meanamalkard = Amalkardtime.Sum() / Amalkardtime.Count();
            pl.showchart();

           
           
        

            
            


            
            
           
        }

    
    }

}
