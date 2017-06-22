using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Text;
using System.Threading;
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
using Lego.Ev3.Core;
using Lego.Ev3.Desktop;

namespace EV3_Test
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        Brick brick = null;
        string le = "B";
        string ri = "C";
        int fo_po = 60;
        int back_po = -60;
        Vector joystick_pos = new Vector();

        public MainWindow()
        {
            InitializeComponent();
            Left.IsEnabled = false;
            Right.IsEnabled = false;
            Top.IsEnabled = false;
            Bot.IsEnabled = false;
        }

        private async void Left_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            brick.BatchCommand.TurnMotorAtPower((OutputPort)Enum.Parse(typeof(OutputPort), le), back_po);
            brick.BatchCommand.TurnMotorAtPower((OutputPort)Enum.Parse(typeof(OutputPort), ri), fo_po);
            await brick.BatchCommand.SendCommandAsync();
        }
        
        private async void Left_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            await brick.DirectCommand.StopMotorAsync(OutputPort.All, false);
        }

        private async void Right_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            brick.BatchCommand.TurnMotorAtPower((OutputPort)Enum.Parse(typeof(OutputPort), ri), back_po);
            brick.BatchCommand.TurnMotorAtPower((OutputPort)Enum.Parse(typeof(OutputPort), le), fo_po);
            await brick.BatchCommand.SendCommandAsync();
        }

        private async void Right_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            await brick.DirectCommand.StopMotorAsync(OutputPort.All, false);
        }

        private async void Top_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            await brick.DirectCommand.TurnMotorAtPowerAsync(
                (OutputPort)Enum.Parse(typeof(OutputPort), le) | (OutputPort)Enum.Parse(typeof(OutputPort), ri), fo_po);
        }

        private async void Top_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            await brick.DirectCommand.StopMotorAsync(OutputPort.All, false);
        }

        private async void Bot_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            await brick.DirectCommand.TurnMotorAtPowerAsync(
                (OutputPort)Enum.Parse(typeof(OutputPort), le) | (OutputPort)Enum.Parse(typeof(OutputPort), ri), back_po);
        }
        
        private async void Bot_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            await brick.DirectCommand.StopMotorAsync(OutputPort.All, false);
        }

        private async void Connection_Blue_Click(object sender, RoutedEventArgs e)
        {
            string result = Microsoft.VisualBasic.Interaction.InputBox("EV3의 COM 포트를 입력하세요.", "블루투스 연결");
            try
            {
                brick = new Brick(new BluetoothCommunication(result));
                Left.IsEnabled = true;
                Right.IsEnabled = true;
                Top.IsEnabled = true;
                Bot.IsEnabled = true;
                await brick.ConnectAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "블루투스 연결");
                MessageBox.Show("연결 실패", "블루투스 연결");
            }

        }

        private async void Connection_Usb_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                brick = new Brick(new UsbCommunication());
                Left.IsEnabled = true;
                Right.IsEnabled = true;
                Top.IsEnabled = true;
                Bot.IsEnabled = true;
                await brick.ConnectAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "USB 연결");
                MessageBox.Show("연결 실패", "USB 연결");
            }
        }
        
        private void Debug_Click(object sender, RoutedEventArgs e)
        {
            string res = Microsoft.VisualBasic.Interaction.InputBox
                ("디버그 명령어를 입력하십시오.", "디버그 관리자", $"Left:{le},Right:{ri},FoPower:{fo_po},BackPower:{back_po}");
            if (res != "")
            {
                string[] highvalue = res.Replace(" ", "").Split(',');
                Dictionary<string, string> realvalue = new Dictionary<string, string>();
                highvalue.ToList().ForEach(x => realvalue.Add(x.Split(':')[0], x.Split(':')[1]));
                le = realvalue["Left"];
                ri = realvalue["Right"];
                fo_po = Convert.ToInt32(realvalue["FoPower"]);
                back_po = Convert.ToInt32(realvalue["BackPower"]);
            }
        }
    }
}