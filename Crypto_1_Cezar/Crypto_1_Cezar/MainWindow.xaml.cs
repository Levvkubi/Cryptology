using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Drawing;
using System.Drawing.Imaging;
using Crypto_1_Cezar.Cyphers;

namespace Crypto_1_Cezar
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string input = string.Empty;
        string output = string.Empty;
        Cypher cypher;
        int currCypher;
        //змінна зберігає чи зашифрувати зараз текст чи дешефрувати (якщо true зашифрувати), можливо не найкраще рішення використовувати bool
        bool lastActEncript = true;
        bool useGaslo = false;// для шифру тримеуса
        Action<Func<string, string[], int, string>> crypter;
        System.Windows.Media.Brush defoltButtColor;
        System.Windows.Media.Brush activeButtColor;
        OpenFileDialog openDialog;
        OpenFileDialog openDialogNote;
        SaveFileDialog saveDialog;
        string openedFile;
        string noteKeyText;
        List<StackPanel> inputPanels;
        public MainWindow()
        {
            InitializeComponent();

            inputPanels = new List<StackPanel>();
            inputPanels.Add(CaesarInputKey);
            inputPanels.Add(TrimeusInputKey);
            inputPanels.Add(GammaKeyInputKey);
            inputPanels.Add(GammaNoteInputKey);

            changeCrypt(SelectCrypt.SelectedIndex);

            defoltButtColor = DecryptButt.Background;
            activeButtColor = EncryptButt.Background;

            openDialog = new OpenFileDialog();
            openDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";

            saveDialog = new SaveFileDialog();
            saveDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";

            openDialogNote = new OpenFileDialog();
            openDialogNote.Filter = "txt files (*.txt)|*.txt";
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
        private void updateFields()
        {
            InputTextBox.Text = input;
            OutputTextBox.Text = output;
        }
        private async void updateFieldsAsync()
        {
            //await this.Dispatcher.BeginInvoke(
            //new Action(() =>
            //{
            //    if (input.Length < 1000)
            //    {
            //        this.InputTextBox.Text = input;
            //        this.OutputTextBox.Text = output;
            //    }
            //    else
            //    {
            //        for (int i = 0; i < input.Length; i++)
            //        {
            //            this.InputTextBox.Text += input[i];
            //        }
            //    }
            //}));
            updateFields();
        }
        private void Crypt(bool encrypt)
        {   //Мабуть тому що поле для вводу ключа створюється швидше за UseAlfabet і воно викликає цей метод
            //тут виникає помилка нулл референс в ідеалі з цим бт поглибше розібратись
            if (UseAlfabet == null)
                return;
            Func<string, string[], int, string> crypt;
            if (encrypt)
                crypt = cypher.Encrypt;
            else
                crypt = cypher.Decrypt;

            crypter(crypt);
            updateFieldsAsync();
        }
        private void CaesarsCrypt(Func<string, string[], int, string> crypt)
        {
            if (cypher.IsValidKey(new string[] { KeyBox.Text }))
            {
               output = crypt(
                    input,
                    new string[] { KeyBox.Text },
                    getLangState()
                    );
            }
        }
        private void TrimeusCrypt(Func<string, string[], int, string> crypt)
        {
            if (AGasBox.Text == string.Empty)
                return;// помилка валідації мала б оброблятись тут
            string[] args;
            if(int.TryParse( AGasBox.Text,out int a))// мені самому боляче дивитись на цей код але час піджимає(
            {
                if(Bkey.Text != string.Empty)
                {
                    if (Ckey.Text != string.Empty)
                        args = new string[] { AGasBox.Text, Bkey.Text, Ckey.Text };
                    else
                        args = new string[] { AGasBox.Text, Bkey.Text };
                }
                else
                        args = new string[] { AGasBox.Text };
            }
            else
                args = new string[] { AGasBox.Text };
            if (cypher.IsValidKey(args))
            {
                output = crypt(
                    input,
                    args,
                    getLangState()
                    );
            }
        }
        private void GammaKeyCrypt(Func<string, string[], int, string> crypt)
        {
            if(GenerateCheckBox.IsChecked == true)
            {
                if (GammaKeyBox.Text.Length < input.Length)
                {
                    Gamma_code gamma = (Gamma_code)cypher;
                    GammaKeyBox.Text = gamma.GenerateKey(input.Length, getLangState());
                }
            }
            if(!cypher.IsValidKey(new string[] { GammaKeyBox.Text }))
            {
                MessageBox.Show("key is invalid");
                return;
            }
            output = crypt(
                    input,
                    new string[] { GammaKeyBox.Text },
                    getLangState()
                    );
        }
        private void GammaNoteCrypt(Func<string, string[], int, string> crypt)
        {
            if (noteKeyText == null)
                return;

            if (!cypher.IsValidKey(new string[] { noteKeyText }))
            {
                MessageBox.Show("Note is invalid");
                return;
            }
            output = crypt(
                    input,
                    new string[] { noteKeyText },
                    getLangState()
                    );
        }

        private void Swap_Click(object sender, RoutedEventArgs e)
        {
            string currText = input;
            input = output;
            output = currText;

            updateFieldsAsync();

            Crypt(lastActEncript);
        }

        private void InputTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            input = InputTextBox.Text;
            Crypt(lastActEncript);
            InputChastotTextBox.Text = cypher.GetFrequency(input);
        }

        private void OutputTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            OutputChastotTextBox.Text = cypher.GetFrequency(output);
        }

        private void ChangeLangEvent(object sender, RoutedEventArgs e)
        {
            Crypt(lastActEncript);
        }
        private void BruetForseAuto(object sender, RoutedEventArgs e)
        {
            string[] keys;
            cypher.BroutForseAuto(input,out keys, getLangState());
            if(currCypher == 0)
                KeyBox.Text = keys[0];
            else if(currCypher == 1)
            {
                if (!int.TryParse(keys[0], out int a))
                    AGasBox.Text = keys[0];
                else
                {
                    if (keys[0] == "0")
                    {
                        if (keys[1] == "0")
                            AGasBox.Text = keys[2];
                        else
                        {
                            AGasBox.Text = keys[1];
                            Bkey.Text = keys[2];
                        }
                    }
                    else
                    {
                        AGasBox.Text = keys[0];
                        Bkey.Text = keys[1];
                        Ckey.Text = keys[2];
                    }
}
            }
            buttonClick(false);
        }
        private void BruetForseManualy(object sender, RoutedEventArgs e)
        {
            output = cypher.BroutForseManual(input, getLangState());
            updateFieldsAsync();
        }
        private void HackByFreguency(object sender, RoutedEventArgs e)//caesar 
        {
            OutputTextBox.Text = cypher.HackByFreguency(input, getLangState()).ToString();
            buttonClick(false);//імітується нажаття на кнопку розшифрувати для зміни стану в інтерфейсі
                               //і щоб в вихіднопу полі було розшифроване повідомлення
            if (currCypher == 1)
                OutputTextBox.Text = cypher.HackByFreguency(input, getLangState()).ToString();
        }
        private void HackByDecryptMess(object sender, RoutedEventArgs e)
        {
            if (currCypher == 0)
                return;

            InputWindow inputWindow = new InputWindow(this);
            inputWindow.Show();
        }
        public void HackByDecryptMessege(string decrypted,bool usegaslo)
        {
            if(decrypted.Length != input.Length)
            {
                MessageBox.Show("Invalid lenght");
                return;
            }    
            string[] args;
            if (usegaslo)
                args = new string[] { "0" };
            else
                args = new string[] { "1" };

            cypher.HuckByEnDePair(input, decrypted,ref args, getLangState());
            if (usegaslo)
            {
                AGasBox.Text = args[0];
            }
            else
            {
                if (args[0] == "0")
                {
                    if (args[1] == "0")
                        AGasBox.Text = args[2];
                    else
                    {
                        AGasBox.Text = args[1];
                        Bkey.Text = args[2];
                    }
                }
                else
                {
                    AGasBox.Text = args[0];
                    Bkey.Text = args[1];
                    Ckey.Text = args[2];
                }
            }
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
                        input = Convert.ToBase64String(File.ReadAllBytes(openedFile));
                    }
                    else
                        input = sr.ReadToEnd();
                }
                updateFieldsAsync();
                SaveButton.IsEnabled = true;//оскільки файл вже відкритий то в нього тепер можна зберігати данні
            }
        }
        private void SaveFile(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!openedFile.Contains(".txt"))
                {
                    byte[] bytes = Convert.FromBase64String(output);
                    var imageMemoryStream = new MemoryStream(bytes);
                    System.Drawing.Image imageFromStream = System.Drawing.Image.FromStream(imageMemoryStream);
                    imageFromStream.Save(openedFile, ImageFormat.Jpeg);
                }
                else
                {
                    byte[] txtBytes = Encoding.ASCII.GetBytes(output);
                    File.WriteAllBytes(openedFile, txtBytes);
                }
                File.WriteAllText(openedFile, output);
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
                    if (!saveDialog.FileName.Contains(".txt"))
                    {
                        byte[] bytes = Convert.FromBase64String(output);
                        var imageMemoryStream = new MemoryStream(bytes);
                        System.Drawing.Image imageFromStream = System.Drawing.Image.FromStream(imageMemoryStream);
                        imageFromStream.Save(saveDialog.FileName, ImageFormat.Jpeg);
                    }
                    else
                    {
                        byte[] txtBytes = Encoding.ASCII.GetBytes(output);
                        File.WriteAllBytes(saveDialog.FileName, txtBytes);
                    }

                    //File.WriteAllText(saveDialog.FileName, output);
                    //openedFile = saveDialog.FileName;
                    //SaveButton.IsEnabled = true;//оскільки файл вже відкритий то в нього тепер можна зберігати данні
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

        private void SelectCrypt_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            changeCrypt(SelectCrypt.SelectedIndex);
            Crypt(lastActEncript);
        }
        private void changeCrypt(int ind)
        {
            switch (SelectCrypt.SelectedIndex)
            {
                case 0:
                    cypher = new Caesars_code();
                    crypter = CaesarsCrypt;
                    break;
                case 1:
                    cypher = new Trimeus_code();
                    crypter = TrimeusCrypt;
                    break;
                case 2:
                    cypher = new Gamma_code();
                    crypter = GammaKeyCrypt;
                    break;
                case 3:
                    cypher = new Gamma_code();
                    crypter = GammaNoteCrypt;
                    break;
                default:
                    break;
            }
            // при старті чомусь викликається метод змінени стану комбобокса а масив на той момент ще не ініціалізований
            if (inputPanels!= null)
                changePanel(ind);

            currCypher = ind;
        }
        private void changePanel(int ind)
        {
            foreach (var item in inputPanels)
            {
                item.Visibility = Visibility.Collapsed;
            }
            inputPanels[ind].Visibility = Visibility.Visible;
        }

        private void OpenNote_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)openDialogNote.ShowDialog())
            {
                using (StreamReader sr = new StreamReader(openDialogNote.FileName))
                {
                        noteKeyText = sr.ReadToEnd();
                }
                if (noteKeyText.Length <= 0)
                {
                    MessageBox.Show("File is empty!");
                    return;
                }
                GammaNoteBox.Text = openDialogNote.FileName;
                updateFieldsAsync();
            }
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            if(GenerateCheckBox.IsChecked == true)
                GammaKeyBox.IsReadOnly = true;
            else
                GammaKeyBox.IsReadOnly = false;
            Crypt(lastActEncript);
        }
    }
}
