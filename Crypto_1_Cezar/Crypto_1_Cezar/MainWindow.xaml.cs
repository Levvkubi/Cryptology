using Microsoft.Win32;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.IO;
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

namespace Crypto_1_Cezar
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //змінна зберігає чи зашифрувати зараз текст чи дешефрувати (якщо true зашифрувати), можливо не найкраще рішення використовувати bool
        bool lastActEncript = true;
        Brush defoltButtColor;
        Brush activeButtColor;
        OpenFileDialog openDialog;
        SaveFileDialog saveDialog;
        string openedFile;
        public MainWindow()
        {
            InitializeComponent();
            defoltButtColor = DecryptButt.Background;
            activeButtColor = EncryptButt.Background;

            openDialog = new OpenFileDialog();
            openDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";

            saveDialog = new SaveFileDialog();
            saveDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
        }
        private void buttonClick(bool encrypt)
        {
            lastActEncript = encrypt;
            Crypt(encrypt);
            if (encrypt)
            {
                DecryptButt.Background = defoltButtColor;
                EncryptButt.Background = activeButtColor;
            }
            else
            {
                DecryptButt.Background = activeButtColor;
                EncryptButt.Background = defoltButtColor;
            }
        }
        private void Encript_Button_Click(object sender, RoutedEventArgs e)
        {
            buttonClick(true);
        }
        private void Decript_Button_Click(object sender, RoutedEventArgs e)
        {
            buttonClick(false);
        }
        //цей метод перешифровує повідомлення і перераховує частоти, викликається часто
        private void Crypt(bool encrypt)
        {   //Мабуть тому що поле для вводу ключа створюється швидше за UseAlfabet і воно викликає цей метод
            //тут виникає помилка нулл референс в ідеалі з цим бт поглибше розібратись
            if (UseAlfabet == null)
                return;
            Func<string, int, int, string> crypt;
            if (encrypt)
                crypt = Caesars_code.Encrypt;
            else
                crypt = Caesars_code.Decrypt;

            if (Caesars_code.IsValidKey(KeyBox.Text))
            {
                OutputTextBox.Text = crypt(
                    InputTextBox.Text, 
                    int.Parse(KeyBox.Text),
                    getLangState()
                    );
            }
        }

        private void Swap_Click(object sender, RoutedEventArgs e)
        {
            string currText = InputTextBox.Text;
            InputTextBox.Text = OutputTextBox.Text;
            OutputTextBox.Text = currText;

            Crypt(lastActEncript);
        }

        private void InputTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Crypt(lastActEncript);
            InputChastotTextBox.Text = Caesars_code.GetFrequency(InputTextBox.Text);
        }

        private void OutputTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            OutputChastotTextBox.Text = Caesars_code.GetFrequency(OutputTextBox.Text);
        }

        private void ChangeLangEvent(object sender, RoutedEventArgs e)
        {
            Crypt(lastActEncript);
        }
        private void BruetForseAuto(object sender, RoutedEventArgs e)
        {
            int key;
            Caesars_code.BroutForseAuto(InputTextBox.Text,out key, getLangState());
            KeyBox.Text = key.ToString();
            buttonClick(false);
        }
        private void BruetForseManualy(object sender, RoutedEventArgs e)
        {
            OutputTextBox.Text = Caesars_code.BroutForseManual(InputTextBox.Text, getLangState());
            //тут би асинхронно добавляти тект до поля
        }
        private void HackByFreguency(object sender, RoutedEventArgs e)
        {
            KeyBox.Text = Caesars_code.HackByFreguency(InputTextBox.Text, getLangState()).ToString();
            buttonClick(false);//імітується нажаття на кнопку розшифрувати для зміни стану в інтерфейсі
                               //і щоб в вихіднопу полі було розшифроване повідомлення
        }
        private int getLangState()
        {
            if (!(bool)UseAlfabet.IsChecked)
                return 0;
            else if ((bool)UseEn.IsChecked)
                return 1;
            else
                return 2;
        }
        private void OpenFile(object sender, RoutedEventArgs e)
        {
            if((bool) openDialog.ShowDialog())
            {
                openedFile = openDialog.FileName;
                using (StreamReader sr = new StreamReader(openedFile))
                {
                    if (!openDialog.FileName.EndsWith(".txt"))
                    {
                        InputTextBox.Text = Convert.ToBase64String(File.ReadAllBytes(openedFile));
                    }
                    else
                        InputTextBox.Text = sr.ReadToEnd();
                }
                SaveButton.IsEnabled = true;//оскільки файл вже відкритий то в нього тепер можна зберігати данні
            }
        }
        private void SaveFile(object sender, RoutedEventArgs e)
        {
            try
            {
                File.WriteAllText(openedFile, OutputTextBox.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("error");
            } 
        }
        private void SaveAs(object sender, RoutedEventArgs e)
        {
            if ((bool)saveDialog.ShowDialog())
            {
                try
                {
                    //if (!saveDialog.FileName.EndsWith(".txt"))
                    //{
                    //    byte[] bytes = Convert.FromBase64String(OutputTextBox.Text);
                    //    var imageMemoryStream = new MemoryStream(bytes);
                    //    Image imageFromStream = Image.FromStream(imageMemoryStream);
                    //    imageFromStream.Save(filename, ImageFormat.Jpeg);
                    //}

                    File.WriteAllText(saveDialog.FileName, OutputTextBox.Text);
                    openedFile = saveDialog.FileName;
                    SaveButton.IsEnabled = true;//оскільки файл вже відкритий то в нього тепер можна зберігати данні
                }
                catch (Exception)
                {
                    MessageBox.Show("error");
                }
            }
        }
        private void InfoAbout(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Developed by\nМАМИН ХАЦКЕР corp");
        }
    }
}
