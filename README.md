# WpfRubyLabel

## これは何？

WPFのWebBrowserコントロールを利用して作成した、ルビを表示するラベルのようなもの。
これはルビ表示できることを立証するための実験用コードに過ぎず、実用に耐える品質ではないのであしからず。

## どう使うの？

ソリューションをビルドすると `WpfRubyLabel\WpfRubyLabel\bin\Debug\WpfRubyLabelControl.dll` ができるので画面に組み込む。
同封されてる `WpfRubyLabelSample` がWPFで使用するときのサンプルで、 `WpfRubyLabelTest` がユニットテスト。

いろいろ足りてないけれど、ルビ付きのコントロールを作る手掛かりになるかもしれない。…ならないかもしれない。

## ルビを表示するためのコーディング

1. Appendメソッドを呼ぶ

    var label = new WpfRubyLabelControl();
    //地の文のみ
    label.Append("ああ");
    //地の文＋コメント
    label.Append("幸せ", "ハッピー");
    //Appendを連結させることもできる
    label.Append("。なんて").Append("家庭の味", "ボルシチ").Append("なのだろう！");
    //Textプロパティで取得
    Assert.AreEqual("ああ｜幸せ《ハッピー》。なんて｜家庭の味《ボルシチ》なのだろう！", label.Text);

2. 直接入力

    var label = new WpfRubyLabelControl();
    //Textプロパティで入力
    label.Text = "茶碗｜一杯《いっぱい》の雪《ゆき》";
    //HtmlBodyプロパティでrubyタグ変換後のデータを取得できる
    Assert.AreEqual("茶碗<ruby>一杯<rt>いっぱい</rt></ruby>の<ruby>雪<rt>ゆき</rt></ruby>", label.HtmlBody);

## 既知の不具合

1. バインディング機構が弱い。
2. 青空文庫風の記法だと改行が反映されない。
3. 「｜」のエスケープが未実装。
4. ベースはWebBrowserなので、通常のラベルよりも幅や高さの調整が難しい。
5. バグ超大盛。
