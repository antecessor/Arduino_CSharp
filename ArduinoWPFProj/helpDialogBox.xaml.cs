using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.IO.Ports;
using System.Windows.Controls;

namespace ArduinoWPFProj
{
    public partial class helpDialogBox : Window
    {
        SerialPort port;
        public helpDialogBox(MainWindow originalWindow)
        {
            port = originalWindow.arduinoPort;
            InitializeComponent();
            helpText.Text = " با سلام.  برای کار با برنامه ،ابتدا دستگاه را به کامپیوتر وصل نموده و تمامی تنظیمات خود را انتخاب کنید سپس برروی کلید تایید تنظیمات کلیک کنید.جال آماده شوید و بروی شروع کلیک کنید.در پایان میتوانید نتایج خود را به صورت فایل متنی ذخیره نمایید ";
            if (originalWindow.isConnected == true)
            {
                baudRateBox.IsEnabled = false;
            }

        }


        private void close(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void initBaudRate(object sender, EventArgs e)
        {
            ComboBox box = (ComboBox)sender;
            int[] bauds = new int[9] { 1200, 2400, 4800, 9600, 14400, 19200, 38400, 57600, 115200 };
            for (int i = 0; i < bauds.Length; i++)
            {
                object baud = bauds[i].ToString();
                box.Items.Add(baud);
                if (baud.Equals(port.BaudRate.ToString()))
                {
                    box.SelectedItem = baud.ToString();
                }
            }
        }

        private void changeBaud(object sender, SelectionChangedEventArgs e)
        {
            ComboBox box = (ComboBox)sender;
            int newBaud = int.Parse((string)box.SelectedItem);
            Console.WriteLine("Setting Baud to " + newBaud.ToString());
            try
            {
                port.BaudRate = newBaud;
            }
            catch (NullReferenceException) { }
        }
    }
}
