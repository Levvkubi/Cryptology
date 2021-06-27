using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Crypto_1_Cezar
{
    /// <summary>
    /// Interaction logic for InputWindow.xaml
    /// </summary>
    public partial class InputWindow : Window
    {
        MainWindow mainWindow;
        public InputWindow(MainWindow MainWindow)
        {
            InitializeComponent();
            mainWindow = MainWindow;
        }

        private void okButt_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.HackByDecryptMessege(DecryptBox.Text, (bool)UseGaslo.IsChecked);
            this.Close();
        }
    }
}
