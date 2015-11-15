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
using System.Windows.Navigation;
using System.Windows.Shapes;
using ICS.Acquisition;
using ICS.Common;
using ICS.Models;
using ICS.Lamp;

namespace ICS.Lamp
{

    public class onoffbutton : Window
    {
        Image lampimg, onoffimg;
        byte[] oncode, offcode;
        string errormessage;
        bool bs = false;

        public onoffbutton(Image lampimg, Image onoffimg, byte[] oncode, byte[] offcode, string errormessage)
        {
            this.            lampimg = lampimg;
            this.onoffimg = onoffimg;
            this.oncode = oncode;
            this.offcode = offcode;
            this.errormessage = errormessage;
        }

        ResultEntity drivestate()
        {
            return Global.ADAM4150Provider.CheckSerialPort(Global.ADAM4150Provider.ADAM4017Provider);
        }

        ADAM4150 adam = Global.ADAM4150Provider;

        public void onoffmain()
        {
            ResultEntity bret = drivestate();
            if (bret.Status == RunStatus.Failure)
                MessageBox.Show(bret.ResultMessage);
            else
            {
                bool adamResult = adam.OnOff(bs ? offcode : oncode); 
                if (adamResult)
                {
                    bs = !bs;
                    setimages();
                }
                else
                    MessageBox.Show(errormessage);
            }
        }

        void quickimg(string imgstringf, string imgstrings)
        {
            lampimg.Source = new BitmapImage(new Uri(imgstringf, UriKind.Relative));
            onoffimg.Source = new BitmapImage(new Uri(imgstrings, UriKind.Relative));
        }

        void setimages()
        {
            if (bs)
                quickimg("Resources/lamp_on.png", "Resources/btn_switch_on.png");
            else
                quickimg("Resources/lamp_off.png", "Resources/btn_switch_off.png");
        }
    }


    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Image_MouseLeftButtonUp_1(object sender, MouseButtonEventArgs e){offonstarts();}
        private void Image_MouseLeftButtonUp_2(object sender, MouseButtonEventArgs e){offonstarts();}
        private void Image_MouseLeftButtonUp_3(object sender, MouseButtonEventArgs e){offonstartc();}
        private void Image_MouseLeftButtonUp_4(object sender, MouseButtonEventArgs e){offonstartc();}

        byte[] con = new byte[] { 0x01, 0x05, 0x00, 0x11, 0xFF, 0x00, 0xDC, 0x3F };
        byte[] coff = new byte[] { 0x01, 0x05, 0x00, 0x11, 0x00, 0x00, 0x9D, 0xCF };
        byte[] son = new byte[] { 0x01, 0x05, 0x00, 0x12, 0xFF, 0x00, 0x2C, 0x3F };
        byte[] soff = new byte[] { 0x01, 0x05, 0x00, 0x12, 0x00, 0x00, 0x6D, 0xCF };

        public void offonstartc(){
        onoffbutton cbutton = new onoffbutton(imgc, imgcs, con,coff,"操作楼道灯失败");
        cbutton.onoffmain();
        }

        public void offonstarts(){
            onoffbutton sbutton = new onoffbutton(imgs, imgss, son, soff, "操作街道等失败");
            sbutton.onoffmain();
        }


        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.WindowState = System.Windows.WindowState.Minimized;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
