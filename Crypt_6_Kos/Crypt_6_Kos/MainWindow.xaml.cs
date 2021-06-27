using System;
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

namespace Crypt_6_Kos
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        DH_Algorythm alise;
        DH_Algorythm bob;
        void getRandPG(out long p, out long g)
        {
            Random random = new Random();
            string[] dict;
            using (StreamReader sr = new StreamReader(@"D:\Programming\С#\3Curs_2\Cruptology\Crypto_1_Cezar\Crypto_1_Cezar\SimpleNumbers2.txt"))
            {
                dict = sr.ReadToEnd().Split('\n');
            }
            int indexP = random.Next(3, dict.Length);
            p = long.Parse(dict[indexP]);
            g = long.Parse(dict[random.Next(indexP - 1)]);
        }

        private void generateButt_Click(object sender, RoutedEventArgs e)
        {
            long p, g;
            getRandPG(out p, out g);

            PField.Text = p.ToString();
            GField.Text = g.ToString();

            Random rand = new Random();

            alise = new DH_Algorythm(p, g,rand.Next((int)p -1));
            bob = new DH_Algorythm(p, g, rand.Next((int)p - 1));

            ASKey.Text = alise.getSecretKey().ToString();
            BSKey.Text = bob.getSecretKey().ToString();

            long AM, BM;
            AM = alise.getMiddleKey();
            BM = bob.getMiddleKey();

            AMKey.Text = AM.ToString();
            BMKey.Text = BM.ToString();

            long AF, BF;
            AF = alise.getFinKey(BM);
            BF = bob.getFinKey(AM);

            AFKey.Text = AF.ToString();
            BFKey.Text = BF.ToString();
        }

    }
}
