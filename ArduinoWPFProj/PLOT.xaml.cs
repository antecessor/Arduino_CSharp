using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using OxyPlot;
using OxyPlot.Series;

namespace ArduinoWPFProj
{
    /// <summary>
    /// Interaction logic for PLOT.xaml
    /// </summary>
    public partial class PLOT : Window
    {
        public float[] andaze; public double[] takhir; public double meanamalkard;
        public int rasttrue,chaptrue,vasattrue,falsezarbe;
        public PLOT()
        {
           
            InitializeComponent();
            
        }

        public void showchart() {
            
            var items1 = new List<KeyValuePair<string, double>>();
            var items2 = new List<KeyValuePair<string, double>>();
            var items3 = new List<KeyValuePair<string, double>>();

            for (int i = 0; i < 15;i++ ){
                items1.Add(new KeyValuePair<string, double>("ضربه"+(i+1).ToString()+"تاخیر:"+takhir[i].ToString(), andaze[i]));
            
        }
            for (int i = 15; i < 30; i++)
            {
                items2.Add(new KeyValuePair<string, double>("ضربه" + (i + 1).ToString() + "تاخیر:" + takhir[i].ToString(), andaze[i]));

            }
           
                items3.Add(new KeyValuePair<string, double>("ضربه راست درست", rasttrue));
                items3.Add(new KeyValuePair<string, double>("ضربه چپ درست", chaptrue));
                items3.Add(new KeyValuePair<string, double>("ضربه وست درست", vasattrue));
                items3.Add(new KeyValuePair<string, double>("ضربه نادرست", falsezarbe));

           

            cchar1.DataContext = items1;
           
            cchar2.DataContext = items2;
            cchar3.DataContext = items3;

            meanamallbl.Content = meanamalkard.ToString();

        
        }

        private void exit(object sender, RoutedEventArgs e)
        {
          System.Diagnostics.Process.Start(AppDomain.CurrentDomain.BaseDirectory + "\\ArduinoWPFProj.exe");
            Application.Current.Shutdown();
        }

        private void Printbtn(object sender, RoutedEventArgs e)
        {
            PrintDialog pdg = new System.Windows.Controls.PrintDialog();
            pdg.PrintVisual(this, "نتایج");
        }

      
    }
}
