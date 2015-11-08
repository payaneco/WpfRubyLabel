using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// このカスタム コントロールを XAML ファイルで使用するには、手順 1a または 1b の後、手順 2 に従います。
    ///
    /// 手順 1a) 現在のプロジェクトに存在する XAML ファイルでこのカスタム コントロールを使用する場合
    /// この XmlNamespace 属性を使用場所であるマークアップ ファイルのルート要素に
    /// 追加します:
    ///
    ///     xmlns:MyNamespace="clr-namespace:WpfRubyLabel"
    ///
    ///
    /// 手順 1b) 異なるプロジェクトに存在する XAML ファイルでこのカスタム コントロールを使用する場合
    /// この XmlNamespace 属性を使用場所であるマークアップ ファイルのルート要素に
    /// 追加します:
    ///
    ///     xmlns:MyNamespace="clr-namespace:WpfRubyLabel;assembly=WpfRubyLabel"
    ///
    /// また、XAML ファイルのあるプロジェクトからこのプロジェクトへのプロジェクト参照を追加し、
    /// リビルドして、コンパイル エラーを防ぐ必要があります:
    ///
    ///     ソリューション エクスプローラーで対象のプロジェクトを右クリックし、
    ///     [参照の追加] の [プロジェクト] を選択してから、このプロジェクトを参照し、選択します。
    ///
    ///
    /// 手順 2)
    /// コントロールを XAML ファイルで使用します。
    ///
    ///     <MyNamespace:WpfRubyLabelControl/>
    ///
    /// </summary>
    public class WpfRubyLabelControl : Control, INotifyPropertyChanged
    {

        public string WholeHtml
        {
            get { return _WholeHtml; }
            set
            {
                _WholeHtml = value;
                NotifyPropertyChanged("WholeHtml");
            }
        }
        private string _WholeHtml;

        public string Text
        {
            set
            {
                var fmt = @"<html lang=""ja"">
<head>
    <meta http-equiv = ""Content-Type"" content = ""text/html; charset=utf-8"">
    <style type=""text/css"">
    <!--
        body {{
            overflow: hidden;
        }}
        rt {{
            font-size: 9px;
            color: #ff0000;
        }}
    -->
    </style>
</head>
<body>{0}</body>
</html>";
                WholeHtml = string.Format(fmt, value);
            }
        }

        static WpfRubyLabelControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WpfRubyLabelControl), new FrameworkPropertyMetadata(typeof(WpfRubyLabelControl)));
        }

        public WpfRubyLabelControl()
        {
            Loaded += WpfRubyLabelControl_Loaded;
        }

        private void WpfRubyLabelControl_Loaded(object sender, RoutedEventArgs e)
        {
            (GetTemplateChild("RubyBrowser") as WebBrowser).DataContext = this;
        }

        public void SetText()
        {
            var fmt = @"<html lang=""ja"">
<head>
    <meta http-equiv = ""Content-Type"" content = ""text/html; charset=utf-8"">
    <style type=""text/css"">
    <!--
        body {{
            overflow: hidden;
        }}
        rt {{
            font-size: 9px;
            color: #ff0000;
        }}
    -->
    </style>
</head>
<body>{0}</body>
</html>";
            WholeHtml = string.Format(fmt, "<ruby>本気<rt>マジ</rt></ruby>ああああああああああああああああああああああああああああああああああああああああああああああああああああああああああああああああああああああああ");
            //(GetTemplateChild("RubyBrowser") as WebBrowser).NavigateToString(string.Format(fmt, "<ruby>本気<rt>マジ</rt></ruby>ああああああああああああああああああああああああああああああああああああああああああああああああああああああああああああああああああああああああ"));
        }

        public static readonly DependencyProperty HtmlProperty = DependencyProperty.RegisterAttached(
                "Html",
                typeof(string),
                typeof(WpfRubyLabelControl),
                new FrameworkPropertyMetadata(OnHtmlChanged));

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        [AttachedPropertyBrowsableForType(typeof(WebBrowser))]
        public static string GetHtml(WebBrowser d)
        {
            return (string)d.GetValue(HtmlProperty);
        }

        public static void SetHtml(WebBrowser d, string value)
        {
            d.SetValue(HtmlProperty, value);
        }

        static void OnHtmlChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            WebBrowser webBrowser = dependencyObject as WebBrowser;
            if (webBrowser != null)
            {
                webBrowser.NavigateToString(e.NewValue as string ?? "&nbsp;");
            }
        }
    }
}
