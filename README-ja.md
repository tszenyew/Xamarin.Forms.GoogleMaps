## ![logo](logo.png) Xamarin.Forms.GoogleMaps 
![](https://img.shields.io/nuget/v/Xamarin.Forms.GoogleMaps.svg) ![](https://img.shields.io/nuget/dt/Xamarin.Forms.GoogleMaps.svg) [![Gitter chat](https://badges.gitter.im/gitterHQ/gitter.png)](http://gitter.im/Xamarin-Forms-GoogleMaps/public) [![donate/kyash](https://img.shields.io/badge/donate-kyash-orange.svg)](#寄付)

[English README is here！](README.md)

Xamarin.Forms 用の Googleマップライブラリです。

[Xamarin.Forms.Maps](https://github.com/xamarin/Xamarin.Forms) をフォークして作っているので、使い方はほとんど同じです。


## デモアプリ

このライブラリの全ての機能が試せるデモアプリを以下より配信しています。このアプリのソースは [XFGoogleMapSample](https://github.com/amay077/Xamarin.Forms.GoogleMaps/tree/master/XFGoogleMapSample) です。

* [Android DEMO Apps](https://install.mobile.azure.com/users/okuokuoku/apps/xfgooglemapsample/distribution_groups/trial) - このリンクをAndroidのブラウザで開いてください
* iOS DEMO Apps - [Gitter](https://gitter.im/Xamarin-Forms-GoogleMaps/public) かなにかでリクエストしてください（デバイスのUUIDを教えてもらう必要があります）

![screenshot](screenshot01.png)

## なぜこれを作った

Xamarin公式の地図ライブラリ [Xamarin.Forms.Maps](https://developer.xamarin.com/guides/xamarin-forms/user-interface/map/) は、非常に機能が少ないです（Googleマップ、Appleマップ、Bingマップで機能を共通化するのはとても難しいのでしょう）。

特に感じるのは、BingマップSDKがとてもチープなことです。ベクトルタイルでもないし、マーカーの吹き出し(InfoWindow
)も無いようです。モバイルアプリの市場のほとんどは Android と iOS なのに、Bingマップのサポートは必要ないように感じます。また、iOS でも Appleマップ よりも Googleマップ を使用した方が、 Android/iOS で共通化しやすいと感じます。

このライブラリは、メインターゲットを iOS と Android だけに限定し、Xamarin.Forms.Maps でできない機能を実現するために作りました。

異なる地図SDKで実現可能な最小限の機能しか持たない Xamarin.Forms.Maps に対して、 **同じ Google Maps で多くの共通機能を実現できるのが Xamarin.Forms.GoogleMaps です。**

## Xamarin.Forms.Maps との比較

|機能|X.F.Maps|X.F.GoogleMaps|
| ------------------- | :-----------: | :-----------: |
|地図の種類|Yes|Yes|
|渋滞状況地図|-|Yes|
|地図イベント|-|Yes|
|地図の移動(アニメーション付き)|Yes|Yes|
|地図の移動(アニメーション無し)|-|Yes|
|ピン|Yes|Yes|
|カスタムピン|-|Yes|
|ピンのドラッグ&ドロップ|-|Yes|
|ポリゴン|-|Yes|
|ライン|-|Yes|
|円|-|Yes|
|カスタム地図タイル|-|Yes|


詳しくは、[Xamarin.Forms.Maps との比較](https://github.com/amay077/Xamarin.Forms.GoogleMaps/wiki/Xamarin.Forms.Maps との比較) を見て下さい。

## セットアップ

* Available on NuGet: https://www.nuget.org/packages/Xamarin.Forms.GoogleMaps/ [![NuGet](https://img.shields.io/nuget/v/Xam.Plugin.Geolocator.svg?label=NuGet)](https://www.nuget.org/packages/Xamarin.Forms.GoogleMaps/)
* PCLプロジェクトと各プラットフォームプロジェクトにインストールしてください

## サポートするプラットフォーム

|Platform|Supported|
| ------------------- | :-----------: |
|iOS Unified|Yes|
|Android|Yes|
|Windows 10 UWP|No|
|その他|No|

## 使い方

* [Map Control - Xamarin](https://developer.xamarin.com/guides/xamarin-forms/user-interface/map/)
* [Xamarin.Formsで地図を表示するには？（Xamarin.Forms.Maps使用） - Build Insider](http://www.buildinsider.net/mobile/xamarintips/0039)

とほぼ同じです。
初期化メソッドが ``Xamarin.Forms.Maps.Init()`` から ``Xamarin.Forms.GoogleMaps.Init()`` に変更になっています。

iOS の場合、 [Google Maps API for iOS](https://developers.google.com/maps/documentation/ios-sdk/) の API キーを取得し、``AppDelegate.cs`` にて ``Init`` に渡してください。

```csharp
[Register("AppDelegate")]
public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
{
    public override bool FinishedLaunching(UIApplication app, NSDictionary options)
    {
        global::Xamarin.Forms.Forms.Init();
        Xamarin.FormsGoogleMaps.Init("your_api_key");
        LoadApplication(new App());

        return base.FinishedLaunching(app, options);
    }
}
``` 

既定の名前空間が ``Xamarin.Forms.Maps`` から ``Xamarin.Forms.GoogleMaps`` に変更されています。他のAPIはすべて同じです。

サンプルプログラムが、

* [XFGoogleMapSample](https://github.com/amay077/Xamarin.Forms.GoogleMaps/tree/master/XFGoogleMapSample)

にあります。

## 採用事例

Xamarin.Forms.GoogleMaps が使われているアプリを紹介します(他にもあったら [教えてください](https://github.com/amay077/Xamarin.Forms.GoogleMaps/issues/512))。

<table>
  <tr>
    <td align="center">
      <h3><a target="_blank" href="https://www.herenow.city/">HereNow</a></h3>
      <img src="showcase_herenow.jpg" width="200" width="200" style="max-width:100%;">
      <p>by <a target="_blank" href="https://www.cinra.co.jp/">CINRA, Inc.</a></p>
    </td>
    <td align="center">
      <h3><a target="_new" href="https://www.citybee.lt/">CityBee</a></h3>
      <img src="showcase_citybee.png" width="200" width="200" style="max-width:100%;">
      <p>&nbsp;</p>
    </td>
    <td align="center">
      <h3><a target="_new" href="https://itunes.apple.com/tr/app/rentacarss-ara%C3%A7-takip/id1276280125">Rentacarss Araç Takip</a></h3>
      <img src="showcase_rentacarss.jpg" width="200" width="200" style="max-width:100%;">
      <p>&nbsp;</p>
    </td>
    <td align="center">
      <h3>yakala.co</h3>
      <a target="_blank" href="https://apps.apple.com/tr/app/yakala-co/id834961121?l=tr">iOS</a>
      &nbsp;/&nbsp;<a target="_blank" href="https://play.google.com/store/apps/details?id=com.mobven.yakalaco">Android</a>
      <img src="showcase_yakala.png" width="200" width="200" style="max-width:100%;">
      <p>by <a target="_blank" href="http://www.dakicksoft.com/">Dakicksoft</a></p>
    </td>
  </tr>
  <tr>
    <td align="center">
      <h3><a target="_blank" href="https://itunes.apple.com/us/app/transantiagomaster/id541341697?mt=8">TransantiagoMaster</a></h3>
      <img src="https://user-images.githubusercontent.com/1848210/47026824-b3d31c00-d13c-11e8-926a-d7e68403e856.png" width="200" width="200" style="max-width:100%;">
    </td>
    <td align="center">
    </td>
    <td align="center">
      <h3>CmsApp</h3>
      <a target="_blank" href="https://itunes.apple.com/us/app/cmsmobileapp/id1151248489?ls=1&mt=8">iOS</a>
      &nbsp;/&nbsp;<a target="_blank" href="https://play.google.com/store/apps/details?id=net.winsir.cms.CmsMobile">Android</a>
      <img src="https://user-images.githubusercontent.com/20931876/64476046-8540a400-d147-11e9-9b62-22894d5e2ffd.png" width="200" width="200" style="max-width:100%;">
      <p>by <a target="_blank" href="http://cms.winsir.net/">Ruben Carreon</a></p>
    </td>
    <td align="center">
      <h3>UsynligO</h3>
      <a target="_blank" href="https://apps.apple.com/us/app/usynligo/id1306699569">iOS</a>
      &nbsp;/&nbsp;<a target="_blank" href="https://play.google.com/store/apps/details?id=com.Benum.UsynligO">Android</a>
      <img src="https://lh3.googleusercontent.com/fqVdiOUQTz7oBwCccvmgq8z8tmV0Ip6tLBI6SCEDVHiKcVGGZWwUrEufJ-iOmUZhxu8=s180-rw" width="200" width="200" style="max-width:100%;">
      <p>by <a target="_blank" href="https://play.google.com/store/apps/developer?id=Trond+Benum">Trond Benum</a></p>
    </td>
  </tr>
  <tr>
    <td align="center">
      <h3>Taiwan-AskFaceMask (問口罩)</h3>
      <a target="_blank" href="https://apps.apple.com/tw/app/id1498868646">iOS</a>
      &nbsp;/&nbsp;<a target="_blank" href="https://play.google.com/store/apps/details?id=tw.goodjob.askfacemaskapp">Android</a>
      <img src="showcase_taiwan_askfacemask.gif" width="200" width="200" style="max-width:100%;">
      <p>by <a target="_blank" href="https://github.com/JamestsaiTW">JamestsaiTW</a></p>
    </td>
    <td align="center">
      <h3>Bipbip Navigation GPS</h3>
      <a target="_blank" href="https://apps.apple.com/app/id1588690430">iOS</a>
      &nbsp;/&nbsp;<a target="_blank" href="https://play.google.com/store/apps/details?id=com.Beepbip">Android</a>
      <img src="showcase_bipbip.png" width="200" width="200" style="max-width:100%;">
      <p>by <a target="_blank" href="https://bipbip.tv/">Bipbip</a></p>
    </td>
    <td align="center">
    </td>
    <td align="center">
    </td>
  </tr>  
</table>

## リリースノーツ

[Releases](https://github.com/amay077/Xamarin.Forms.GoogleMaps/releases) または [RELEASE_NOTES](RELEASE_NOTES.md) を見てください。

## 今後の予定

なるべく Xamarin.Forms.Maps の API に準じ、Google Maps固有の機能のみ API を追加するつもりです。 

機能要望は、 [@amay077](https://twitter.com/amay077) または、ISSUE やプルリクください！
追加機能案は以下の通りです。

* ~~Pin の InfoWindow の Visible プロパティ~~ v1.0.0で対応
* ~~Pin のタップ＆ホールドによる移動~~ v1.5.0で対応
* ~~Polygon, Polyline, Circle の描画サポート~~ v1.1.0で対応
* [その他の機能改善リスト](https://github.com/amay077/Xamarin.Forms.GoogleMaps/labels/enhancement)

Windows 10 UWP 対応は v5.0.0 から削除しました。
Android/iOS のみ対応します。

## CONTRIBUTION

私たちは、Xamarin.Forms.GoogleMaps への、あなたの貢献に大変感謝します。

開発に参加して頂ける方は、[コントリビューション ガイドライン](CONTRIBUTING-ja.md) を読んで下さい。

## 寄付

Xamarin.Forms.GoogleMaps 開発の継続のため、寄付を募集しています。

**Gumroad**

* [Gumroad](https://amay077.gumroad.com/)

あなたの寄付で開発者のモチベーションが上がります、どうかよろしくおねがいします :sushi:

## ライセンス

[LICENSE](LICENSE.txt) をみて下さい。

logo.png by [alecive](http://www.iconarchive.com/show/flatwoken-icons-by-alecive.html) - [CC BY-SA 4.0](https://creativecommons.org/licenses/by-sa/4.0/deed)
