using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
namespace ICS.Lamp
{
    public class Onoffbutton : Window
    {
        public Image Lampimg, Onoffimg;
        public byte[] Oncode, Offcode;
        public string Errormessage;

        public bool LightState;
       
        public void Onoffmain()
        {
            var portState = Global.ADAM4150Provider.CheckSerialPort(Global.ADAM4150Provider.ADAM4017Provider);
            if (portState.Status == RunStatus.Failure)
                MessageBox.Show(portState.ResultMessage);
            else
            {
                if (Global.ADAM4150Provider.OnOff(LightState ? Offcode : Oncode))
                {
                    LightState = !LightState;
                    Setimages();
                }
                else
                    MessageBox.Show(Errormessage);
            }
        }

        void Quickimg(string lamp, string button)
        {
            Lampimg.Source = new BitmapImage(new Uri(lamp, UriKind.Relative));
            Onoffimg.Source = new BitmapImage(new Uri(button, UriKind.Relative));
        }

        void Setimages()
        {
            if (LightState)
                Quickimg("Resources/lamp_on.png", "Resources/btn_switch_on.png");
            else
                Quickimg("Resources/lamp_off.png", "Resources/btn_switch_off.png");
        }
    }

    public partial class MainWindow : Window
    {
      
        Onoffbutton cbutton;
        Onoffbutton sbutton;

        byte[] con = { 0x01, 0x05, 0x00, 0x11, 0xFF, 0x00, 0xDC, 0x3F };
        byte[] coff = { 0x01, 0x05, 0x00, 0x11, 0x00, 0x00, 0x9D, 0xCF };
        byte[] son = { 0x01, 0x05, 0x00, 0x12, 0xFF, 0x00, 0x2C, 0x3F };
        byte[] soff = { 0x01, 0x05, 0x00, 0x12, 0x00, 0x00, 0x6D, 0xCF };

        public MainWindow()
        {
            InitializeComponent();
            cbutton = new Onoffbutton {Lampimg = imgc, Onoffimg = imgcs, Oncode = con, Offcode = coff,Errormessage = "操作楼道灯失败"};
            sbutton = new Onoffbutton {Lampimg = imgs, Onoffimg = imgss, Oncode = son, Offcode = soff, Errormessage = "操作街道等失败" };
        }


        private void Image_MouseLeftButtonUp_1(object sender, MouseButtonEventArgs e) { sbutton.Onoffmain(); }
        private void Image_MouseLeftButtonUp_2(object sender, MouseButtonEventArgs e) { sbutton.Onoffmain(); }
        private void Image_MouseLeftButtonUp_3(object sender, MouseButtonEventArgs e) { cbutton.Onoffmain(); }
        private void Image_MouseLeftButtonUp_4(object sender, MouseButtonEventArgs e) { cbutton.Onoffmain(); }

        private void Button_Click_1(object sender, RoutedEventArgs e){WindowState = WindowState.Minimized;}
        private void Button_Click_2(object sender, RoutedEventArgs e){Close();}
    }

}