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

namespace WpfRubyLabel
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Loaded += MainWindow_Loaded;
            BtnTest.Click += BtnTest_Click;
        }

        private void BtnTest_Click(object sender, RoutedEventArgs e)
        {
            LblTest.Text = @"<ruby>月光円舞曲<rt>ムーンライトセレナーデ</rt>！</ruby>";
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LblTest.SetText();
        }
    }
}
