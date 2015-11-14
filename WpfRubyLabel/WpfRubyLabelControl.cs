using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        /// <summary>
        /// ルビ記法の列挙型
        /// </summary>
        public enum RubyNotationType
        {
            /// <summary>
            /// 青空文庫風の｜記法《きほう》
            /// </summary>
            Aozora,
            //todo 実装
            ///// <summary>
            ///// 新潮社風の#記法{きほう}
            ///// </summary>
            //Shincho,
            /// <summary>
            /// HTML風の&lt;ruby&rt;記法&lt;rt&rt;きほう&lt;/rt&rt;&lt;/ruby&rt;
            /// </summary>
            Html,
        }

        /// <summary> 地の文のフォントサイズ(px) </summary>
        public int TextSize
        {
            get { return _TextSize; }
            set
            {
                _TextSize = value;
                NotifyPropertyChanged("TextSize");
            }
        }
        /// <summary> 地の文のフォントの色を指定する文字列(css準拠) </summary>
        public string TextColor
        {
            get { return _TextColor; }
            set
            {
                _TextColor = value;
                NotifyPropertyChanged("TextColor");
            }
        }
        /// <summary> ルビのフォントサイズ(px) </summary>
        public int RubySize
        {
            get { return _RubySize; }
            set
            {
                _RubySize = value;
                NotifyPropertyChanged("RubySize");
            }
        }
        /// <summary> ルビのフォントの色を指定する文字列(css準拠) </summary>
        public string RubyColor
        {
            get { return _RubyColor; }
            set
            {
                _RubyColor = value;
                NotifyPropertyChanged("RubyColor");
            }
        }
        /// <summary> html body要素 </summary>
        public string HtmlBody
        {
            get
            {
                return _BodyBuilder.ToString();
            }
            set
            {
                _BodyBuilder.Clear();
                _BodyBuilder.Append(value);
                NotifyPropertyChanged("WholeHtml");
            }
        }
        /// <summary> htmlに変換済みのルビ付き文字列 </summary>
        public string WholeHtml
        {
            get
            {
                var sb = new StringBuilder();
                sb.AppendLine(@"<html lang=""ja"">")
                  .AppendLine(@"<head>")
                  .AppendLine(@"    <meta http-equiv = ""Content-Type"" content = ""text/html; charset=utf-8"">")
                  .AppendLine(@"    <style type=""text/css"">")
                  .AppendLine(@"    <!--")
                  .AppendLine(@"        body {{")
                  .AppendLine(@"            overflow: hidden;")     //スクロールバーを隠す
                  .AppendFormat(@"            font-size: {0}px;", TextSize).AppendLine()
                  .AppendFormat(@"            color: {0};", TextColor).AppendLine()
                  .AppendLine(@"        }}")
                  .AppendLine(@"        rt {{")
                  .AppendFormat(@"            font-size: {0}px;", RubySize).AppendLine()
                  .AppendFormat(@"            color: {0};", RubyColor).AppendLine()
                  .AppendLine(@"        }}")
                  .AppendLine(@"    -->")
                  .AppendLine(@"    </style>")
                  .AppendLine(@"</head>")
                  .AppendFormat(@"    <body>{0}</body>", HtmlBody).AppendLine()
                  .AppendLine(@"</html>");
                return sb.ToString();
            }
        }
        /// <summary> ルビ付きの文字列 </summary>
        public string Text
        {
            get
            {
                string text;
                switch (RubyNotation)
                {
                    case RubyNotationType.Aozora: text = GetAozoraText(HtmlBody); break;
                    //todo 実装
                    //case RubyNotationType.Shincho: text = GetShinchoText(HtmlBody); break;
                    case RubyNotationType.Html:
                    default: text = HtmlBody; break;
                }
                return text;
            }
            set
            {
                string text;
                switch (RubyNotation)
                {
                    case RubyNotationType.Aozora: text = DecodeAozoraText(value); break;
                    //todo 実装
                    //case RubyNotationType.Shincho: text = GetShinchoText(HtmlBody); break;
                    case RubyNotationType.Html:
                    default: text = HtmlBody; break;
                }
                HtmlBody = text;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <remarks>
        /// 正規表現のUnicodeカテゴリ判定は、以下のページを参考にしました。感謝。
        /// http://dobon.net/vb/dotnet/string/ishiragana.html
        /// </remarks>
        private static string DecodeAozoraText(string value)
        {
            var text = value;
            //「|」または「｜」による区切り
            text = Regex.Replace(text, @"\|(.+?)(《(.+?)》|≪(.+?)≫)", "<ruby>$1<rt>$3$4</rt></ruby>");
            text = Regex.Replace(text, @"｜(.+?)(《(.+?)》|≪(.+?)≫)", "<ruby>$1<rt>$3$4</rt></ruby>");
            //連続する漢字
            text = Regex.Replace(text, @"(([\p{IsCJKUnifiedIdeographs}\p{IsCJKCompatibilityIdeographs}\p{IsCJKUnifiedIdeographsExtensionA}]|[\uD840-\uD869][\uDC00-\uDFFF]|\uD869[\uDC00-\uDEDF])+)(《(.+?)》|≪(.+?)≫)",
                "<ruby>$1<rt>$4$5</rt></ruby>");
            //連続するひらがな・カタカナ・アルファベット
            //通常の全角カタカナの他に、カタカナフリガナ拡張、
            //　濁点と半濁点、半角カタカナもカタカナとする
            text = Regex.Replace(text, @"(\p{IsHiragana}+|[\p{IsKatakana}\u31F0-\u31FF\u3099-\u309C\uFF65-\uFF9F]+)(《(.+?)》|≪(.+?)≫)",
                "<ruby>$1<rt>$3$4</rt></ruby>");
            //英数字(全角含む)
            text = Regex.Replace(text, @"([a-zA-Zａ-ｚＡ-Ｚ\d]+)(《(.+?)》|≪(.+?)≫)",
                "<ruby>$1<rt>$3$4</rt></ruby>");
            //その他単語
            text = Regex.Replace(text, @"^(\w+?)(《(.+?)》|≪(.+?)≫)", "<ruby>$1<rt>$3$4</rt></ruby>");
            return text;
        }

        /// <summary>
        /// 青空文庫風記法の文字列を取得する
        /// </summary>
        /// <param name="htmlBody"></param>
        /// <returns></returns>
        private static string GetAozoraText(string htmlBody)
        {
            return GetNotatedText(htmlBody, "｜", string.Empty, "《", "》");
        }

        /// <summary>
        /// 新潮社風記法の文字列を取得する
        /// </summary>
        /// <param name="htmlBody"></param>
        /// <returns></returns>
        private string GetShinchoText(string htmlBody)
        {
            return GetNotatedText(htmlBody, "#", string.Empty, "{", "}");
        }

        /// <summary>
        /// 記法を適用した文字列を取得する
        /// </summary>
        /// <param name="htmlBody"></param>
        /// <param name="repRubyS"></param>
        /// <param name="repRubyE"></param>
        /// <param name="repRtS"></param>
        /// <param name="repRtE"></param>
        /// <returns></returns>
        private static string GetNotatedText(string htmlBody, string repRubyS, string repRubyE, string repRtS, string repRtE)
        {
            return htmlBody
                .Replace("<ruby>", repRubyS)
                .Replace("<rt>", repRtS)
                .Replace("</rt>", repRtE)
                .Replace("</ruby>", repRubyE);
        }

        /// <summary> ルビの記法 </summary>
        public RubyNotationType RubyNotation { get; set; }

        private int _TextSize;
        private string _TextColor;
        private int _RubySize;
        private string _RubyColor;
        private StringBuilder _BodyBuilder;

        /// <summary>
        /// staticコンストラクタ
        /// </summary>
        static WpfRubyLabelControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WpfRubyLabelControl), new FrameworkPropertyMetadata(typeof(WpfRubyLabelControl)));
        }

        /// <summary>
        /// 通常のコンストラクタ
        /// </summary>
        public WpfRubyLabelControl()
        {
            TextSize = 14;
            TextColor = "#000000";
            RubySize = 10;
            RubyColor = "#000000";
            _BodyBuilder = new StringBuilder();
            RubyNotation = RubyNotationType.Aozora;

            Loaded += WpfRubyLabelControl_Loaded;
        }

        /// <summary>
        /// コントロールロード時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WpfRubyLabelControl_Loaded(object sender, RoutedEventArgs e)
        {
            (GetTemplateChild("RubyBrowser") as WebBrowser).DataContext = this;
        }

        /// <summary>
        /// プロパティ登録
        /// </summary>
        public static readonly DependencyProperty HtmlProperty = DependencyProperty.RegisterAttached(
                "Html",
                typeof(string),
                typeof(WpfRubyLabelControl),
                new FrameworkPropertyMetadata(OnHtmlChanged));

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
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

        /// <summary>
        /// 地の文の色をColorで設定する
        /// </summary>
        public void SetTextColor(Color color)
        {
            TextColor = GetColorString(color);
        }

        /// <summary>
        /// 地の文の色をColorで設定する
        /// </summary>
        public void SetRubyColor(Color color)
        {
            RubyColor = GetColorString(color);
        }

        private string GetColorString(Color color)
        {
            return string.Format("#{0:x2}{1:x2}{2:x2}", color.R, color.G, color.B);
        }
    }
}
