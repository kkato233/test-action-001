﻿Title: お問い合わせ
---
## お問い合わせフォーム

タブレットとスマホ環境では、サブブロックが非表示になります。その他のページでも非表示にしたい場合、
html側の `<body>` タグを `<body class="s-n">` として下さい。
もしclass指定が２つ以上ある場合は半角スペースで区切って入力すればOKです。
例：`<body class="s-n sample">`


### お問い合わせは、下記のフォームからお願いいたします。

<div id="contact-form">
<form action="/api/Contact" class="large" method="post">                    
<table class="ta1">
<tr>
<th colspan="2" class="tamidashi">※マークは入力必須です</th>
</tr>
<tr>
<th>お名前※</th>
<td><input type="text" name="お名前" size="30" class="ws"></td>
</tr>
<tr>
<th>メールアドレス※</th>
<td>
    <input class="ws form-control" data-val="true" data-val-email="メールアドレスを正しく入力してください。" data-val-required="メールアドレスを入力してください" id="Email" name="Email" type="text" value="" />
    <span class="field-validation-valid text-danger" data-valmsg-for="Email" data-valmsg-replace="true"></span>
</td>
</tr>
<tr>
<th>ご住所(都道府県)</th>
<td>
<select name="ご住所(都道府県)">
<option value="" selected="selected">都道府県選択</option>
<option value="北海道">北海道</option>
<option value="青森県">青森県</option>
<option value="岩手県">岩手県</option>
<option value="宮城県">宮城県</option>
<option value="秋田県">秋田県</option>
<option value="山形県">山形県</option>
<option value="福島県">福島県</option>
<option value="茨城県">茨城県</option>
<option value="栃木県">栃木県</option>
<option value="群馬県">群馬県</option>
<option value="埼玉県">埼玉県</option>
<option value="千葉県">千葉県</option>
<option value="東京都">東京都</option>
<option value="神奈川県">神奈川県</option>
<option value="新潟県">新潟県</option>
<option value="富山県">富山県</option>
<option value="石川県">石川県</option>
<option value="福井県">福井県</option>
<option value="山梨県">山梨県</option>
<option value="長野県">長野県</option>
<option value="岐阜県">岐阜県</option>
<option value="静岡県">静岡県</option>
<option value="愛知県">愛知県</option>
<option value="三重県">三重県</option>
<option value="滋賀県">滋賀県</option>
<option value="京都府">京都府</option>
<option value="大阪府">大阪府</option>
<option value="兵庫県">兵庫県</option>
<option value="奈良県">奈良県</option>
<option value="和歌山県">和歌山県</option>
<option value="鳥取県">鳥取県</option>
<option value="島根県">島根県</option>
<option value="岡山県">岡山県</option>
<option value="広島県">広島県</option>
<option value="山口県">山口県</option>
<option value="徳島県">徳島県</option>
<option value="香川県">香川県</option>
<option value="愛媛県">愛媛県</option>
<option value="高知県">高知県</option>
<option value="福岡県">福岡県</option>
<option value="佐賀県">佐賀県</option>
<option value="長崎県">長崎県</option>
<option value="熊本県">熊本県</option>
<option value="大分県">大分県</option>
<option value="宮崎県">宮崎県</option>
<option value="鹿児島県">鹿児島県</option>
<option value="沖縄県">沖縄県</option>
</select></td>
</tr>
<tr>
<th>ご住所(市区町村以下)</th>
<td><input type="text" name="ご住所(市区町村以下)" size="30" class="wl"></td>
</tr>
<tr>
<th>お問い合わせ詳細※</th>
<td><textarea name="お問い合わせ詳細" cols="30" rows="10" class="wl"></textarea></td>
</tr>
</table>
<p class="c">
<input type="submit" class="btn btn-primary btn-lg" value="メールを送信する">
</p>
</form>
</div>
</div>
<script
  src="http://code.jquery.com/jquery-3.5.1.min.js"
  integrity="sha256-9/aliU8dGd2tb6OSsuzixeV4y/faTqgFtohetphbbj0="
  crossorigin="anonymous"></script>
<script src="https://cdn.jsdelivr.net/npm/jquery-validation@1.19.3/dist/jquery.validate.min.js" type="text/javascript"></script>
<script src="https://cdnjs.cloudflare.com/ajax/libs/jquery-validation-unobtrusive/3.2.11/jquery.validate.unobtrusive.min.js" type="text/javascript"></script>
<script>
    var validateOk = false;
    $.validator.unobtrusive.options = {
        invalidHandler: function () {
            validateOk = false;
        },
        success: function () {
            validateOk = true;
        }
    };
</script>
<script>
    jQuery(function ($) {
        $('#contact-form-item').submit(function (event) {
            if (validateOk == false) return;
            // HTMLでの送信をキャンセル
            event.preventDefault();
            // 操作対象のフォーム要素を取得
            var $form = $(this);
            // 送信ボタンを取得
            // （後で使う: 二重送信を防止する。）
            var $button = $form.find("input[type='submit']");
            var JSONdata = {
                Name: $("#Mail_UserName").val(),
                Email: $("#Mail_Email").val(),
                Message: $("#Mail_Message").val(),
            };
			if (confirm('下記の内容で送信します。よろしいですか？')) {
            // 送信
            $.ajax({
                url: "/api/Contact",
                type: "post",
                data: JSON.stringify(JSONdata), // フォームの内容
                contentType: 'application/JSON',
                timeout: 10000,  // 単位はミリ秒
                // 送信前
                beforeSend: function (xhr, settings) {
                    // ボタンを無効化し、二重送信を防止
                    $button.attr('disabled', true);
                },
                // 応答後
                complete: function (xhr, textStatus) {
                    // ボタンを有効化し、再送信を許可
                    $button.attr('disabled', false);
                },
                // 通信成功時の処理
                success: function (result, textStatus, xhr) {
                    // 入力値を初期化
                    $form[0].reset();
                    alert("お問い合わせありがとうございました。後日担当から連絡差し上げます。");
                    window.location.href = "/Contact?submit=true";
                },
                // 通信失敗時の処理
                error: function (xhr, textStatus, error) { 
                	alert("データ送信エラー。別の方法でお問い合わせください。" );
                }
            });
            }
        });
    });
</script>
