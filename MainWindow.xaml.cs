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
        bool brakeatstop = false;
        Vector joystick_pos = new Vector();

        public MainWindow()
        {
            InitializeComponent();
            Button_Field.IsEnabled = false;
            Joystick_Field.IsEnabled = false;
            Button_Field.Visibility = Visibility.Visible;
            Joystick_Field.Visibility = Visibility.Hidden;
        }

        private async void Left_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            brick.BatchCommand.TurnMotorAtPower((OutputPort)Enum.Parse(typeof(OutputPort), le), back_po);
            brick.BatchCommand.TurnMotorAtPower((OutputPort)Enum.Parse(typeof(OutputPort), ri), fo_po);
            await brick.BatchCommand.SendCommandAsync();
        }
        
        private async void Left_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            await brick.DirectCommand.StopMotorAsync(OutputPort.All, brakeatstop);
        }

        private async void Right_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            brick.BatchCommand.TurnMotorAtPower((OutputPort)Enum.Parse(typeof(OutputPort), ri), back_po);
            brick.BatchCommand.TurnMotorAtPower((OutputPort)Enum.Parse(typeof(OutputPort), le), fo_po);
            await brick.BatchCommand.SendCommandAsync();
        }

        private async void Right_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            await brick.DirectCommand.StopMotorAsync(OutputPort.All, brakeatstop);
        }

        private async void Top_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            await brick.DirectCommand.TurnMotorAtPowerAsync(
                (OutputPort)Enum.Parse(typeof(OutputPort), le) | (OutputPort)Enum.Parse(typeof(OutputPort), ri), fo_po);
        }

        private async void Top_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            await brick.DirectCommand.StopMotorAsync(OutputPort.All, brakeatstop);
        }

        private async void Bot_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            await brick.DirectCommand.TurnMotorAtPowerAsync(
                (OutputPort)Enum.Parse(typeof(OutputPort), le) | (OutputPort)Enum.Parse(typeof(OutputPort), ri), back_po);
        }
        
        private async void Bot_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            await brick.DirectCommand.StopMotorAsync(OutputPort.All, brakeatstop);
        }

        private async void Left_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if ((Keyboard.GetKeyStates(Key.Left) & KeyStates.Down) > 0)
            {
                brick.BatchCommand.TurnMotorAtPower((OutputPort)Enum.Parse(typeof(OutputPort), le), back_po);
                brick.BatchCommand.TurnMotorAtPower((OutputPort)Enum.Parse(typeof(OutputPort), ri), fo_po);
                await brick.BatchCommand.SendCommandAsync();
            }
        }

        private async void Right_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if ((Keyboard.GetKeyStates(Key.Right) & KeyStates.Down) > 0)
            {
                brick.BatchCommand.TurnMotorAtPower((OutputPort)Enum.Parse(typeof(OutputPort), ri), back_po);
                brick.BatchCommand.TurnMotorAtPower((OutputPort)Enum.Parse(typeof(OutputPort), le), fo_po);
                await brick.BatchCommand.SendCommandAsync();
            }
        }

        private async void Top_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if ((Keyboard.GetKeyStates(Key.Up) & KeyStates.Down) > 0)
                await brick.DirectCommand.TurnMotorAtPowerAsync(
                    (OutputPort)Enum.Parse(typeof(OutputPort), le) | (OutputPort)Enum.Parse(typeof(OutputPort), ri), fo_po);
        }

        private async void Bot_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if ((Keyboard.GetKeyStates(Key.Down) & KeyStates.Down) > 0)
                await brick.DirectCommand.TurnMotorAtPowerAsync(
                    (OutputPort)Enum.Parse(typeof(OutputPort), le) | (OutputPort)Enum.Parse(typeof(OutputPort), ri), back_po);
        }

        private async void Left_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            await brick.DirectCommand.StopMotorAsync(OutputPort.All, brakeatstop);
        }

        private async void Right_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            await brick.DirectCommand.StopMotorAsync(OutputPort.All, brakeatstop);
        }

        private async void Top_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            await brick.DirectCommand.StopMotorAsync(OutputPort.All, brakeatstop);
        }

        private async void Bot_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            await brick.DirectCommand.StopMotorAsync(OutputPort.All, brakeatstop);
        }

        private async void Connection_Blue_Click(object sender, RoutedEventArgs e)
        {
            string result = Microsoft.VisualBasic.Interaction.InputBox("EV3의 COM 포트를 입력하세요.", "블루투스 연결");
            try
            {
                brick = new Brick(new BluetoothCommunication(result));
                LayoutRoot.IsEnabled = false;
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
                LayoutRoot.IsEnabled = false;
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
                ("디버그 명령어를 입력하십시오.", "디버그 관리자", $"Left:{le},Right:{ri},FoPower:{fo_po},BackPower:{back_po},BrakeAtStop:{brakeatstop}");
            if (res != "")
            {
                string[] highvalue = res.Replace(" ", "").Split(',');
                Dictionary<string, string> realvalue = new Dictionary<string, string>();
                highvalue.ToList().ForEach(x => realvalue.Add(x.Split(':')[0], x.Split(':')[1]));
                le = realvalue["Left"];
                ri = realvalue["Right"];
                fo_po = Convert.ToInt32(realvalue["FoPower"]);
                back_po = Convert.ToInt32(realvalue["BackPower"]);
                brakeatstop = Convert.ToBoolean(realvalue["BrakeAtStop"]);
            }
        }

        private async void Ellipse_MouseMove(object sender, MouseEventArgs e)
        {
            double fJoystickRadius = Joystick.Height * 0.5;
            Vector vtJoystickPos = e.GetPosition(Joystick) - new Point(fJoystickRadius, fJoystickRadius);
            vtJoystickPos /= fJoystickRadius;
            if (vtJoystickPos.Length > 1.0)
                vtJoystickPos.Normalize();
            double fTheta = Math.Atan2(vtJoystickPos.Y, vtJoystickPos.X);
            
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                joystick_pos = vtJoystickPos;
                Console.WriteLine($"X:{joystick_pos.X}, Y:{joystick_pos.Y}");
                UpdateKnobPosition();
            }
            else
            {
                Canvas.SetLeft(Knob, (LayoutRoot.ActualWidth - Knob.Width) / 2);
                Canvas.SetTop(Knob, (LayoutRoot.ActualHeight - Knob.Height) / 2);
                //await brick.DirectCommand.StopMotorAsync(OutputPort.All, brakeatstop);
            }
        }

        void UpdateKnobPosition()
        {
            double fJoystickRadius = Joystick.Height * 0.5;
            double fKnobRadius = Knob.Width * 0.5;
            Canvas.SetLeft(Knob, Canvas.GetLeft(Joystick) +
                joystick_pos.X * fJoystickRadius + fJoystickRadius - fKnobRadius);
            Canvas.SetTop(Knob, Canvas.GetTop(Joystick) +
                joystick_pos.Y * fJoystickRadius + fJoystickRadius - fKnobRadius);
        }

        private void Joystick_Radio_Checked(object sender, RoutedEventArgs e)
        {
            Button_Field.IsEnabled = false;
            Joystick_Field.IsEnabled = true;
            Button_Field.Visibility = Visibility.Hidden;
            Joystick_Field.Visibility = Visibility.Visible;
            MainGrid.Focus();
        }

        private void Button_Radio_Checked(object sender, RoutedEventArgs e)
        {
            Button_Field.IsEnabled = true;
            Joystick_Field.IsEnabled = false;
            Joystick_Field.Visibility = Visibility.Hidden;
            Button_Field.Visibility = Visibility.Visible;
            MainGrid.Focus();
        }
    }
}