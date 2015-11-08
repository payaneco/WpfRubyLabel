using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
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

namespace WpfRubyLabelSample
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {

        public int TextSize
        {
            get { return _TextSize; }
            set
            {
                _TextSize = value;
                OnPropertyChanged("TextSize");
            }
        }

        public string TextColor
        {
            get { return _TextColor; }
            set
            {
                _TextColor = value;
                OnPropertyChanged("TextColor");
            }
        }

        public int RubySize
        {
            get { return _RubySize; }
            set
            {
                _RubySize = value;
                OnPropertyChanged("RubySize");
            }
        }

        public string RubyColor
        {
            get { return _RubyColor; }
            set
            {
                _RubyColor = value;
                OnPropertyChanged("RubyColor");
            }
        }

        private int _TextSize;
        private string _TextColor;
        private int _RubySize;
        private string _RubyColor;

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                LblTest.HtmlBody = EdtTest.Text;
                LblTest.TextSize = TextSize;
                LblTest.TextColor = TextColor;
                LblTest.RubySize = RubySize;
                LblTest.RubyColor = RubyColor;

                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        public MainWindow()
        {
            InitializeComponent();

            Loaded += MainWindow_Loaded;
            BtnTest.Click += BtnTest_Click;
        }

        private void BtnTest_Click(object sender, RoutedEventArgs e)
        {
            LblTest.Text = EdtTest.Text;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            EdtTest.Text = "<ruby>螺鈿<rt>らでん</rt></ruby>の<ruby>装飾<rt>そうしょく</rt></ruby>";
            TextSize = LblTest.TextSize;
            TextColor = LblTest.TextColor;
            RubySize = LblTest.RubySize;
            RubyColor = LblTest.RubyColor;

            PnlProperty.DataContext = this;
        }
    }

    public class SizeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int v = (int)value;
            return v.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string s = value.ToString();
            int v;
            if (int.TryParse(s, out v))
            {
                return Math.Max(1, v);
            }
            return 10;
        }
    }
}
