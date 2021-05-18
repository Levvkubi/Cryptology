using Crypto_1_Cezar.Cyphers;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for DH_Ex.xaml
    /// </summary>
    public partial class DH_Ex : Window
    {
        DH_Exschange alise;
        DH_Exschange bob;
        public DH_Ex()
        {
            InitializeComponent();
        }
        void getRandPG(out long p,out long g)
        {
            Random random = new Random();
            string[] dict;
            using (StreamReader sr = new StreamReader(@"D:\Programming\С#\3Curs_2\Cruptology\Crypto_1_Cezar\Crypto_1_Cezar\SimpleNumbers.txt"))
            {
                dict = sr.ReadToEnd().Split("\r\n");
            }
            int indexP = random.Next(3,dict.Length);
            p = long.Parse(dict[indexP]);
            g = long.Parse(dict[random.Next(indexP-1)]);
        }

        private void generateButt_Click(object sender, RoutedEventArgs e)
        {
            long p, g;
            getRandPG(out p, out g);

            PField.Text = p.ToString();
            GField.Text = g.ToString();

            alise = new DH_Exschange(p, g);
            bob = new DH_Exschange(p, g);

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
