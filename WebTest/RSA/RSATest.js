$(function () {
    $("#login").on('click', _login);
})

var _getRsaPublicKey = function () {
    var publickey = "";
    $.ajax({
        url: "/RSA/RSATest.ashx",
        async: false,
        method: 'POST',
        dataType: 'json',
        data: {
            Action: "getRasPublicKey",
        },
        success: function (resp) {
            publickey = resp;
        },
        error: function (ex) {
            ex = ex;
        }
    });
    return publickey;
}

var _login = function () {
    var data = _getUserData();
    $.ajax({
        url: "/RSA/RSATest.ashx",
        method: 'POST',
        dataType: 'json',
        data: data,
        success: function (resp) {
            alert('加解密成功，后台返回\r\n' + "用户名：" + resp.username + "\r\n密码：" + resp.passwd);
        },
        error: function (ex) {

        }
    });
}

var _getUserData = function () {
    var publickey = _getRsaPublicKey();
    var username = $("#user").val();
    var passwd = $("#pwd").val();
    setMaxDigits(129);
    var key = new RSAKeyPair(publickey.Exponent, "", publickey.Modulus);
    var passwdScuri = encryptedString(key, passwd);
    var usernameScuri = encryptedString(key, username);
    return {
        Action: "Login",
        username: usernameScuri,
        passwd: passwdScuri
    }
}