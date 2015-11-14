using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WpfRubyLabel;
using System.Reflection;
using System.IO;
using System.Windows.Media;

namespace WpfRubyLabelTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestBody()
        {
            var label = new WpfRubyLabelControl();
            Assert.AreEqual(string.Empty, label.HtmlBody);
            Assert.AreEqual(GetTextResource("TestBody01.txt"), label.WholeHtml);
            label.HtmlBody = "これは<ruby>螺鈿<rt>らでん</rt></ruby>の<ruby>装飾<rt>そうしょく</rt></ruby>です。";
            Assert.AreEqual("これは<ruby>螺鈿<rt>らでん</rt></ruby>の<ruby>装飾<rt>そうしょく</rt></ruby>です。", label.HtmlBody);
            Assert.AreEqual(GetTextResource("TestBody02.txt"), label.WholeHtml);

        }

        private string GetTextResource(string name)
        {
            var resource = string.Join(".", "WpfRubyLabelTest", "res", name);
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resource))
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        [TestMethod]
        public void TestAppend()
        {
            var label = new WpfRubyLabelControl();
            label.Append("ああ");
            Assert.AreEqual("ああ", label.HtmlBody);
            label.Append("幸せ", "ハッピー");
            Assert.AreEqual("ああ<ruby>幸せ<rt>ハッピー</rt></ruby>", label.HtmlBody);
            label.Append("。なんて").Append("家庭の味", "ボルシチ").Append("なのだろう！");
            Assert.AreEqual("ああ｜幸せ《ハッピー》。なんて｜家庭の味《ボルシチ》なのだろう！", label.Text);
        }

        [TestMethod]
        public void TestText()
        {
            var label = new WpfRubyLabelControl();
            Assert.AreEqual(string.Empty, label.Text);
            label.HtmlBody = "これは<ruby>螺鈿<rt>らでん</rt></ruby>の<ruby>装飾<rt>そうしょく</rt></ruby>です。";
            Assert.AreEqual("これは｜螺鈿《らでん》の｜装飾《そうしょく》です。", label.Text);
            label.RubyNotation = WpfRubyLabelControl.RubyNotationType.Html;
            Assert.AreEqual("これは<ruby>螺鈿<rt>らでん</rt></ruby>の<ruby>装飾<rt>そうしょく</rt></ruby>です。", label.Text);
            //label.RubyNotation = WpfRubyLabelControl.RubyNotationType.Shincho;
            //Assert.AreEqual("これは#螺鈿{らでん}の#装飾{そうしょく}です。", label.Text);
            label.RubyNotation = WpfRubyLabelControl.RubyNotationType.Aozora;
            label.Text = "茶碗";
            Assert.AreEqual("茶碗", label.Text);
            label.Text = "茶碗《ちゃわん》";
            Assert.AreEqual("<ruby>茶碗<rt>ちゃわん</rt></ruby>", label.HtmlBody);
            label.Text = "茶｜碗《ちゃわん》";
            Assert.AreEqual("茶<ruby>碗<rt>ちゃわん</rt></ruby>", label.HtmlBody);
            label.Text = "茶碗|一杯《いっぱい》";
            Assert.AreEqual("茶碗<ruby>一杯<rt>いっぱい</rt></ruby>", label.HtmlBody);
            label.Text = "Ruby on ひらがな《ヒラガナ》";
            Assert.AreEqual("Ruby on <ruby>ひらがな<rt>ヒラガナ</rt></ruby>", label.HtmlBody);
            label.Text = "るびぃ・おんカタカナ《かたかな》";
            Assert.AreEqual("るびぃ・おん<ruby>カタカナ<rt>かたかな</rt></ruby>", label.HtmlBody);
            label.Text = "テスト1AbＺ《1aBz》, 19３《いっきゅーさん》";
            Assert.AreEqual("テスト<ruby>1AbＺ<rt>1aBz</rt></ruby>, <ruby>19３<rt>いっきゅーさん</rt></ruby>", label.HtmlBody);
            label.Text = "茶碗｜一杯《いっぱい》の雪《ゆき》";
            Assert.AreEqual("茶碗<ruby>一杯<rt>いっぱい</rt></ruby>の<ruby>雪<rt>ゆき</rt></ruby>", label.HtmlBody);
            Assert.AreEqual("茶碗｜一杯《いっぱい》の｜雪《ゆき》", label.Text);
            label.Text = @"　今日《きょう》
ぼく《僕》とR2D3《メカ》、猫《タマ》の３人《にん》
で、|CR銀河Train777《パチンコ》をトゥギャザー《ルー》しました。《まる》
《劇終》";
            Assert.AreEqual(@"　<ruby>今日<rt>きょう</rt></ruby><br/>
<ruby>ぼく<rt>僕</rt></ruby>と<ruby>R2D3<rt>メカ</rt></ruby>、<ruby>猫<rt>タマ</rt></ruby>の３<ruby>人<rt>にん</rt></ruby><br/>
で、<ruby>CR銀河Train777<rt>パチンコ</rt></ruby>を<ruby>トゥギャザー<rt>ルー</rt></ruby>しました。《まる》<br/>
《劇終》", label.HtmlBody);
            Assert.AreEqual(@"　｜今日《きょう》
｜ぼく《僕》と｜R2D3《メカ》、｜猫《タマ》の３｜人《にん》
で、｜CR銀河Train777《パチンコ》を｜トゥギャザー《ルー》しました。《まる》
《劇終》", label.Text);
            //todo エスケープ未実装
            //label.Text = @"\｜茶碗《ちゃわん》";
            //Assert.AreEqual("｜<ruby>茶碗<rt>ちゃわん</rt></ruby>", label.HtmlBody);
        }

        [TestMethod]
        public void TestProperty()
        {
            var label = new WpfRubyLabelControl();
            label.TextColor = "#0000ff";
            Assert.AreEqual("#0000ff", label.TextColor);
            label.SetTextColor(Colors.White);
            Assert.AreEqual("#ffffff", label.TextColor);
            label.SetTextColor(Colors.Aquamarine);
            Assert.AreEqual("#7fffd4", label.TextColor);
            label.RubyColor = "#123456";
            Assert.AreEqual("#123456", label.RubyColor);
            label.SetRubyColor(Colors.Aquamarine);
            Assert.AreEqual("#7fffd4", label.RubyColor);
            label.SetRubyColor(Colors.Black);
            Assert.AreEqual("#000000", label.RubyColor);
            label.TextSize = 1;
            Assert.AreEqual(1, label.TextSize);
            label.TextSize = int.MaxValue;
            Assert.AreEqual(int.MaxValue, label.TextSize);
            label.RubySize = 100;
            Assert.AreEqual(100, label.RubySize);
        }
    }
}
