using System.Windows;
using System.Windows.Media;

namespace SimpleWPFWork.WPFUI
{
    public partial class ModernMessageBox : Window
    {
        public enum MessageType
        {
            Success,
            Error,
            Warning,
            Info
        }

        public ModernMessageBox(string message, string title, MessageType type)
        {
            InitializeComponent();

            MessageText.Text = message;
            TitleText.Text = title;

            // Set colors and icons based on type
            switch (type)
            {
                case MessageType.Success:
                    HeaderBorder.Background = new SolidColorBrush(Color.FromRgb(0, 217, 163)); // #00D9A3
                    IconText.Text = "✅";
                    OkButton.Background = new SolidColorBrush(Color.FromRgb(0, 217, 163));
                    break;

                case MessageType.Error:
                    HeaderBorder.Background = new SolidColorBrush(Color.FromRgb(231, 76, 60)); // #E74C3C
                    IconText.Text = "❌";
                    OkButton.Background = new SolidColorBrush(Color.FromRgb(231, 76, 60));
                    break;

                case MessageType.Warning:
                    HeaderBorder.Background = new SolidColorBrush(Color.FromRgb(241, 196, 15)); // #F1C40F
                    IconText.Text = "⚠️";
                    OkButton.Background = new SolidColorBrush(Color.FromRgb(241, 196, 15));
                    break;

                case MessageType.Info:
                    HeaderBorder.Background = new SolidColorBrush(Color.FromRgb(52, 152, 219)); // #3498DB
                    IconText.Text = "ℹ️";
                    OkButton.Background = new SolidColorBrush(Color.FromRgb(52, 152, 219));
                    break;
            }
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        public static void Show(string message, string title = "Message", MessageType type = MessageType.Info)
        {
            var messageBox = new ModernMessageBox(message, title, type);
            messageBox.Owner = Application.Current.MainWindow;
            messageBox.ShowDialog();
        }
    }
}