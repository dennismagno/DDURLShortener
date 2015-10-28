function createShortUrl() {
    var _url = $("#get-url").val()
    var longUrl = $("#Url").val()
    $.ajax({
        url: _url,
        dataType: "json",
        async: true,
        jsonp: false,
        type: "POST",
        data: {
            Url: longUrl,
            ts: new Date().getTime()
        },
        error: function (xhr, textStatus, errorThrown) {
            $('#urlResult').append(errorThrown);
        },
        success: function (data) {
            $('#urlResult').text("");
            if (data.status == true) {
                $('#urlResult').append('<a href="' + data.url.ShortUrl + '" target="_blank">' + data.url.ShortUrl + '</a>');
                var qrcode = new QRCode("qrcode");
                qrcode.makeCode(data.url.ShortUrl);
            } else {
                $('#urlResult').append(data.message);
            }
            $("#short-url").show();
        }
    });
}