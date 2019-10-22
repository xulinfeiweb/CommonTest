$(function () {
    //IE兼容�?
    if (window["context"] == undefined) {
        if (!window.location.origin) {
            window.location.origin = window.location.protocol + "//" + window.location.hostname + (window.location.port ? ':' + window.location.port : '');
        }
        window["context"] = location.origin + "/V6.0";
    }

    changeFooterPosition();
    //导航滑块hover
    $('.nav').moveline({
        'zIndex': '-1'
    });

    window.onresize = window.onscroll = function () {
        changeFooterPosition();
        //导航滑块hover
        $('.nav').moveline({
            'zIndex': '-1'
        });
    }
    //页面登录权限控制
    isJudgingLogin();
});

function isJudgingLogin() {
    var href = window.location.href;
    var isJudgingLoginStatus = true;	//true为需在这判断是否登录
    var reg;
    var withoutJudgingLoginList = ['/pages/aftersales/', '/pages/service/', '/pages/dengta/', 'collectIntention.html'];	//需登录权限页面列表
    for (var n in withoutJudgingLoginList) {
        reg = new RegExp(withoutJudgingLoginList[n]);
        if (href.search(reg) == -1) {
            isJudgingLoginStatus = true;
        } else {
            isJudgingLoginStatus = false;
            return;
        }
    }
    isLogin();	//是否登录
}

function isLogin() {
    $.ajax({
        type: "post",
        url: window.location.origin + "/loginUser.pub",
        success: function (res) {
            if (res.success) {
                $('.header .header-right').hide();
                var name = res.accountInfo.accountType == 5 ? res.accountInfo.mobile : res.accountInfo.loginName;
				$('.header .user-center').show().find('a').html(name);
                hasNewMessage()   //是否有新消息
            } else {
                $('.header .header-right').show();
                $('.header .user-center').hide()
                var login = new Login();
                var userAgent = navigator.userAgent.toLowerCase();
                login.getBrowser();
            }
        }
    })
}

function changeFooterPosition() {
    var windowHeight = $(window).height();
    var windowWidth = $(window).width();
    var scrollTop = $(document).scrollTop();
    var minHeight = windowHeight > 775 ? windowHeight : 775;
    $('#main-wrap').css('min-height', minHeight);
    var leftNavHeight = $('#main-wrap').height() - scrollTop;
    var left = $('.main').css('margin-left');
    $('#header-wrap').css('position', 'fixed');
    $('.left-nav').css({
        left: left,
        height: leftNavHeight,
        display: 'block',
        position: 'fixed'
    })
    if (windowWidth < 1050) {
        var mainHeight = $('#main-wrap').height();
        $('#header-wrap').css('position', 'absolute');
        $('.left-nav').css({
            height: mainHeight,
            position: 'absolute'
        })
    }
    $('#footer-wrap').css('visibility', 'visible');
}

function hasNewMessage() {    //是否有新消息
    $.ajax({
        type: "post",
        url: '/msg/queryMsg.pvt',
        success: function (res) {
            if (res.success && res.accountMsgNum != 0) {
                $('.user-center').find('.red-dot').show();
                $('.user-center').find('.msg').attr('title', '你有新的消息');
            }
            $('.user-center').find('.msg').click(function () {
                var url = window.location.origin + '/message.html';
                window.location.href = url;
            })
        }
    });


}

function Login(returnUrl) {
    this.isRemember = false;
    this.userID = false;
    this.password = false;
    this.imgCode = false;
    this.isDengTa = false;
    this.returnUrl = returnUrl || null;
    this.origin = window.location.origin;
    this.userIp = "";
    this.browser = "";
    this.loginArea = "";
    this.isFill = false;
    this.verifyType = 1; //1 为滑动验�? �?2为图片验�?
	
	this.wechatSwitch();  //微信扫码开�?
    this.getCaptchaType();	//获取验证码类�?
    this.showLoginPage();  //显示登录弹窗
    this.cancalLogin();    //取消登录
    this.checkIsRemember();  //判断是否有记住密�?
    this.viewPassword();     //查看密码
    this.toForgetPassword(); //忘记密码
    this.deliverPassword();  //密码框间值同�?
    this.switchLoginMold();  //切换账户类型
    this.judgeUserID();      //账号验证
    this.checkPassword();  //密码验证
    this.toLogin();        //点击登录

}

Login.prototype.wechatSwitch = function(){
	var oSelf = this;
	$.ajax({
            type: "post",
            url: '/weChat/switch.pub',
            success: function (res) {
            	if(res.success){ //开
            		$('.login-title .commonTitle').css('display','inline-block');
            		$('.active-line').show();
            		oSelf.switchLoginTab();  //扫码登录与账号密码登录切�?
            		var loginContent = $('.login .wxlogin-content');
            		if(loginContent.length != 0){
            			oSelf.wechatAuth();  //微信授权二维码扫码登�?
            		}
            	}else{	//�?
            		$('.wxlogin-content').remove();
            		$('.login-title .wx-title, .active-line').remove();
            		$('.login-title .title').css({
            			'display': 'inline-block',
            			'width': '100%'
            		});
            	}
            }
        }
    )
}

Login.prototype.switchLoginTab = function(){
	$(".login .login-title .title").click(function () {
	    $(".login-title .active-line").animate({left: '72.5'}, "fast", function () {
	        $(".login-content").show();
	        $(".wxlogin-content").hide();
	    });
	});
	$(".login .login-title .wx-title").click(function () {
	    $(".login-title .active-line").animate({left: '397.5'}, "fast", function () {
	        $(".login-content").hide();
	        $(".wxlogin-content").show();
	    });
	});
}

Login.prototype.wechatAuth = function(){ // 微信扫码登陆
	var origin = location.protocol + "//" + location.hostname + (location.port ? ':' + location.port : '');
	var returnUrl;
	if(location.pathname == "/pages/account/login.html"){
		if(document.referrer.indexOf("/pages/account/login.html") == -1){
			returnUrl = document.referrer || origin;
		}else{
			returnUrl = origin;
		}
	}else{
		returnUrl = origin + location.pathname;
	}
	var redirectUrl = encodeURIComponent(origin + "/weChat/wechatLogin.pub?redirectUrl=" + returnUrl);
	var state = (new Date()).getTime();
	var obj = new WxLogin({
	    self_redirect: false,
	    id: "qrCode-content",
	    appid: "wxee14ecd016ff4808",
	    scope: "snsapi_login",
	    redirect_uri:redirectUrl,
	    state: state,
	    style: "",
	    href: ""
	});
}

Login.prototype.getCaptchaType = function () {
    var oSelf = this;
    $.ajax({
            type: "post",
            url: '/verifyCodeType.pub',
            success: function (res) {
                if (res.success) {
                    oSelf.verifyType = res.verifyCode;
                    if (oSelf.verifyType == 1) {
                        $('.login .imgCode').hide();
                        $('.login .login-content').css('padding-top', '50px');
                        $('.login').height('500px');
                        oSelf.addCaptchaDiv();
                    } else {
                        $('.login .imgCode').show();
                        $('.login').height('562px');
                        oSelf.getImgCode();     //更换图片验证�?
                        oSelf.checkImgCode();   //图片验证码验�?
                    }
                }
            }
        }
    )
};

Login.prototype.getCaptcha = function (data) { //获取腾讯云验证码
    var self = this;
    $.ajax({
        type: 'post',
        url: window.location.origin + '/verifyCode/webCodeJsUrl.pub',
        success: function (res) {
            $('#captcha-wrap').show();
            if (!document.getElementById('scriptE1')) {
                var script = document.createElement("script");
                script.type = "text/javascript";
                script.src = res.jsUrl;
                script.id = 'scriptE1';
                document.getElementsByTagName("head")[0].appendChild(script);
                script.onload = script.onreadystatechange = function () {
                    self.beginMethod(data);
                }
            } else {
                self.beginMethod(data);
            }
        }
    })
};
Login.prototype.beginMethod = function (data) {
    var self = this;
    var capOption = {callback: cbfn, showHeader: true, type: "popup",};
    capDestroy();
    capInit(document.getElementById("TCaptcha"), capOption);
    //回调函数：验证码页面关闭时回�?
    function cbfn(retJson) {
        $('#captcha-wrap').hide();
        if (retJson.ret == 0) {
            // 用户验证成功
            data.verifyCode = retJson.ticket;
            self.merchantLogin(data);
        } else {
            //用户关闭验证码页面，没有验证
            data.verifyCode.imgCode = false;
        }
    }
};

Login.prototype.addCaptchaDiv = function () { //添加滑动验证码控�?
    if (!document.getElementById("captcha-wrap")) {
        var divString = '<div id="captcha-wrap">' + '<div id="TCaptcha" style="width: 300px; height: 40px;"></div>' + '</div>';
        $("body").append(divString);
    }
}

Login.prototype.showLoginPage = function () {//显示登录弹窗
    var oSelf = this;
    if (location.href.search(/login.html/) != -1) {
        oSelf.autoShowLogin();
        $("body").css("overflow", "auto");
    }
};
Login.prototype.autoShowLogin = function () {
    var oSelf = this;
    $('#login-wrap').show();
    window.scrollTo(0, 0);  //滚动条归�?
    oSelf.randomImgSrc();
    this.userID = false;
    this.password = false;
    this.imgCode = false;
    this.isDengTa = false;
    this.isFill = true;
    $('.login-content input').val('');
    $('#showPassword').hide();
    $('#password').show();
    $('.userPassword .viewPassword').attr({
        'data-pass': 'off',
        'title': '查看密码'
    }).css('background-image', 'url(' + oSelf.origin + '/images/hidePwd.png)');
    $('.userID #userID').val('').css('background-color', 'none');
    $('#password').val('').css('background-color', 'none');
    $('.login-content .login-err').hide();
    oSelf.checkIsRemember();
    $("body").css("overflow", "hidden");
    $('.left-nav').css('left', $('.main').css('margin-left'));
}

Login.prototype.randomImgSrc = function () {
    var url = this.origin + '/verifyCode.pub';
    var random = parseInt(Math.random() * 1000);
    $('.imgCode img').attr('src', url + '?aid=' + random);
    this.imgCode = false;

}

Login.prototype.cancalLogin = function () {
    var oSelf = this;
    $('#login-wrap .cancal,#login-wrap').unbind('click').click(function () {
        $('#login-wrap').hide();
        $('#userID').val('').removeClass('false').siblings('p').hide();
        $('#password').val('').removeClass('false').siblings('p').hide();
        $('#imgCode').val('').removeClass('false').siblings('p').hide();
        oSelf.userID = false;
        oSelf.password = false;
        oSelf.imgCode = false;
        oSelf.isFill = false;
        $('body').css('overflow-y', 'auto');
        $('.left-nav').css('left', $('.main').css('margin-left'));
    })
    $('#login-wrap .login').unbind('click').click(function (e) {
        e.stopPropagation();
    })
}

Login.prototype.viewPassword = function () {  //查看密码
    var oSelf = this;
    $('.userPassword .viewPassword').on('click', function () {
        var passValue = $(this).attr('data-pass');
        $('#password').toggle();
        $('#showPassword').toggle();
        if (passValue == 'on') {
            $(this).attr({
                'data-pass': 'off',
                'title': '查看密码'
            }).css('background-image', 'url(' + oSelf.origin + '/images/hidePwd.png)');
            $('#password').focus();
        } else if (passValue == 'off') {
            $(this).attr({
                'data-pass': 'on',
                'title': '隐藏密码'
            }).css('background-image', 'url(' + oSelf.origin + '/images/showPwd.png)');
            $('#showPassword').focus();
        }
    })
}

Login.prototype.toForgetPassword = function () { //忘记密码
    $('.forgetPassword').click(function () {
        window.location.href = "/pages/findPassword/index.html";
    })
}

Login.prototype.deliverPassword = function () {   //密码框间值传递同�?
    $('#showPassword').on('focus keyup blur', function () {
        $('#password').val($(this).val());
    })
    $('#password').on('focus keyup blur', function () {
        $('#showPassword').val($(this).val());
    })
}

Login.prototype.switchLoginMold = function () { //灯塔和丰桥账号切�?
    var oSelf = this;
    $('.login-type .dt').unbind('click').click(function () {
        var tempStr1 = "数据灯塔账户登录".tran();
        $('.login .title').text(tempStr1);
        var tempStr2 = "数据灯塔用户名或手机�?".tran();
        $('#userID').attr('placeholder', tempStr2);
        var tempStr3 = "请使用数据灯塔账户登录，或点击图片切�?".tran();
        $('.login-content .sub-title span').text(tempStr3);
        $(this).hide().siblings("li").show();
        oSelf.isDengTa = true;
    })
    $('.login-type .qiao').unbind('click').click(function () {
        var tempStr1 = "丰桥账号登录".tran();
        $('.login .title').text(tempStr1);
        var tempStr2 = "用户名或手机�?".tran();
        $('#userID').attr('placeholder', tempStr2);
        var tempStr3 = "请使用丰桥账户登录，或点击图片切�?".tran();
        $('.login-content .sub-title span').text(tempStr3);
        $(this).hide().siblings("li").show();
        oSelf.isDengTa = false;
    })
}

Login.prototype.checkIsRemember = function () {      //判断是否记住账户类型
    var oSelf = this;
    if ($.cookie('csimRememberName') != undefined) {
        var userInfo = JSON.parse($.cookie('csimRememberName'))
        oSelf.userID = userInfo.rememberName;
        oSelf.isDengTa = userInfo.isDengTa;
        $('.userID #userID').val(oSelf.userID).removeClass('false').siblings('p').hide();
        $('#password').focus();
        if (oSelf.isDengTa) {
            $('.login-type .dt').hide();
            $('.login-type .qiao').show();
            var tempStr1 = "数据灯塔账户登录".tran();
            $('.login .title').text(tempStr1);
            var tempStr2 = "数据灯塔用户名或手机�?".tran();
            $('#userID').attr('placeholder', tempStr2);
            var tempStr3 = "请使用数据灯塔账户登录，或点击图片切�?".tran();
            $('.login-content .sub-title span').text(tempStr3);
        } else {
            $('.login-type .dt').show();
            $('.login-type .qiao').hide();
            var tempStr1 = "丰桥账号登录".tran();
            $('.login .title').text(tempStr1);
            var tempStr2 = "用户名或手机�?".tran();
            $('#userID').attr('placeholder', tempStr2);
            var tempStr3 = "请使用丰桥账户登录，或点击图片切�?".tran();
            $('.login-content .sub-title span').text(tempStr3);
        }
    } else {
        $('.login-type .dt').show();
        $('.login-type .qiao').hide();
        var tempStr1 = "丰桥账号登录".tran();
        $('.login .title').text(tempStr1);
        var tempStr2 = "用户名或手机�?".tran();
        $('#userID').attr('placeholder', tempStr2);
        var tempStr3 = "请使用丰桥账户登录，或点击图片切�?".tran();
        $('.login-content .sub-title span').text(tempStr3);
        $('.userID #userID').focus();
    }
}

Login.prototype.hideErrInfo = function (el) { //隐藏错误提示
    el.attr('class', '');
    el.siblings('p').hide();
}

Login.prototype.judgeUserID = function () {

    var oSelf = this;
    $('.userID #userID').blur(function () {
        var tempStr = $(this).val().trim();
        if (tempStr.length != 0) {
            var qiaoReg = /^[a-z0-9][a-z0-9@-_.]{5,19}$/i;	//丰桥用户名正�?
            var dengTaReg = /^([a-zA-z]*[0-9]*){2,15}$/;	//灯塔用户名正�?
            if (!qiaoReg.test(tempStr) && !dengTaReg.test(tempStr)) {
                oSelf.userID = false;
                $('.userID #userID').siblings('p').show().html('用户名或手机号输入不合法');
            } else {
                $('.userID #userID').siblings('p').hide();
                oSelf.userID = tempStr;
            }

        } else {
            oSelf.userID = false;
            oSelf.hideErrInfo($('.userID #userID'));
        }
    })

}

Login.prototype.checkPassword = function () {
    var oSelf = this;
    $('#password,#showPassword').blur(function () {
        var tempStr = $('#password').val().trim();
        if (tempStr.length != 0) {
            oSelf.password = tempStr;
        } else {
            oSelf.password = false;
        }

    })
}

Login.prototype.encrypt = function (word) {
    var PUBLIC_KEY = "MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCqwfA7Pre0IkwdwnOR6cbgX9M5nAD4hd5n0iY0hEZE8K/IFi4lqHEm8ofJBWW+LFdtyHz3Lp8i77ZXRtN/qXP/b5tkQvgRmcXE3Zjfppm3kjzct+41fS3RMMSb8hrY5kidUH+ANBAjmZP1gpWW2Hy6Ns+OernenUilvoe4fsYTpQIDAQAB";
    var encrypt = new JSEncrypt();
    encrypt.setPublicKey(PUBLIC_KEY);
    var encrypted = encrypt.encrypt(word) || '';
    return encrypted.replace(/\+/g, '-').replace(/\//g, '_').replace(/\=/g, '');
}

Login.prototype.getImgCode = function () {
    var oSelf = this;
    $('.imgCode img').unbind('click').click(function () {
        oSelf.randomImgSrc();
        $('#imgCode').val('').removeAttr('disabled').focus();
        $('.imgCode').find('p').show().html('图片验证码已更换');
    })
}

Login.prototype.checkImgCode = function () {
    var oSelf = this;
    $('#imgCode').blur(function () {
        var str = $('.imgCode #imgCode').val().trim();
        if (str.length != 4 && str.length != 0) {
            $('.imgCode p').show().html('请输�?4位图片验证码');
            oSelf.imgCode = false;
        } else if (str.length == 0) {
            $('.imgCode p').hide();
            oSelf.imgCode = false;
        } else {
            $('.imgCode p').hide();
            oSelf.imgCode = str;
        }
    })
}

Login.prototype.toLogin = function () {  //登录
    var oSelf = this;
    $('.btn-group .toLogin').unbind('click').click(function () {
        oSelf.submitUserInfo();
    })
    $("body").unbind('keydown').keydown(function (event) {
        if (event.keyCode == 13 && oSelf.isFill) {
            event.preventDefault();
            $('input').blur();
            if (!oSelf.userID) {
                $('.userID #userID').focus();
            } else if (!oSelf.password) {
                $('.userPassword #password').focus();
            } else if (oSelf.verifyType == 1) {
                oSelf.submitUserInfo();
            } else if (!oSelf.imgCode) {
                $('.imgCode #imgCode').focus();
            } else {
                oSelf.submitUserInfo();
            }
            return false;
        }
    })
}

Login.prototype.submitUserInfo = function () {
    var oSelf = this;
    oSelf.getIP(function () {
        if (!oSelf.userID) {
            $('.userID #userID').focus();
            $('.userID .reminder').show().html('用户名或手机号输入不合法');
            $('.imgCode #imgCode').val('');
            oSelf.randomImgSrc();
        } else if (!oSelf.password) {
            var passValue = $(this).attr('data-pass');
            if (passValue == 'on') {
                $('.userPassword #showPassword').focus();
            } else if (passValue == 'off') {
                $('.userPassword #password').focus();
            }
            $('.userPassword p').show().html('密码未输�?');
            $('.imgCode #imgCode').val('');
            oSelf.randomImgSrc();
        } else if (oSelf.verifyType == 1) {
            $('.captcha-wrap').show();
            $('.userID .reminder').hide();
            $('.userPassword p').hide();
            var passwordStr = oSelf.encrypt(oSelf.password);
            var data = {
                loginName: oSelf.userID,
                loginPwd: passwordStr,
                userIp: oSelf.userIp,
                browser: oSelf.browser,
                loginArea: oSelf.loginArea
            };
            oSelf.getCaptcha(data);
        }
        else if (oSelf.verifyType == 2 && !oSelf.imgCode) {
            $('.imgCode #imgCode').focus();
            $('.imgCode p').show().html('验证码未输入/输入错误');
            $('.imgCode #imgCode').val('');
            oSelf.randomImgSrc();
        } else {
            $('.userID .reminder').hide();
            $('.userPassword p').hide();
            var passwordStr = oSelf.encrypt(oSelf.password);
            var data = {
                loginName: oSelf.userID,
                loginPwd: passwordStr,
                verifyCode: oSelf.imgCode,
                userIp: oSelf.userIp,
                browser: oSelf.browser,
                loginArea: oSelf.loginArea
            };
            oSelf.merchantLogin(data);
        }
    });

}

Login.prototype.merchantLogin = function (data) {  //登录
    var oSelf = this;
    var reqUrl = location.origin + '/merchantLogin.pub';
    var reqData = data;
    oSelf.isDengTa ? reqData.loginMold = 4 : reqData.loginMold = 1;
    $.ajax({
        type: 'post',
        url: reqUrl,
        data: reqData,
        dataType: 'json',
        success: function (res) {
            if (res.loginSucc) { //登录成功
                oSelf.loginSuccess(res);
            } else if (res.success) {            //登录失败
                if (res.mappedErrorMsgs[0] == "verifyCode.error") {  //验证码错�?
                    oSelf.verifyCodeErr();
                } else if (res.mappedErrorMsgs[0] == "Authorize.error.password" || res.mappedErrorMsgs[0] == "Authorize.not.exist.user") {    //账号密码错误
                    oSelf.userInfoErr();
                } else if (res.mappedErrorMsgs[0] == "Authorize.invalid.user") {       //账户冻结失效
                    oSelf.blockingUser();
                } else if (res.mappedErrorMsgs[0] == "Authorize.domain.connFailed") {	//请求灯塔失败
                    $('.login-content .login-err').show().html('登录验证失败,请确认用户类�?');
                    $('.imgCode #imgCode').val('');
                    oSelf.randomImgSrc();
                } else if (res.mappedErrorMsgs[0] == "Authorize.domain.exception") {
                    $('.login-content .login-err').show().html('请求异常,登录失败');
                    $('.imgCode #imgCode').val('');
                    oSelf.randomImgSrc();
                } else if (res.mappedErrorMsgs[0] == "verifyCode.timeout") {    //验证码超�?
                    oSelf.timeout();
                } else {
                    $('.login-content .login-err').show().html('出现未知错误,登录失败');
                    $('.imgCode #imgCode').val('');
                    oSelf.randomImgSrc();
                }
            } else {
                $('.login-content .login-err').show().html('无法登录，请稍后再试');
                $('.imgCode #imgCode').val('');
                oSelf.randomImgSrc();
            }
        }
    })
}

Login.prototype.loginSuccess = function (res) {  //登录成功
    var oSelf = this;
    $('.header .header-right').hide();
    $('.header .user-center').show().find('a').html(res.username);
    $('.login-content .login-err').hide();
    $('#login-wrap').hide();  // 关闭弹窗
    oSelf.isFill = false;
    $('body').css('overflow-y', 'auto');
    $('.left-nav').css('left', $('.main').css('margin-left'));
    //记住账号
    var csimRememberName = {
        rememberName: oSelf.userID,
        isDengTa: oSelf.isDengTa
    }
    $.cookie('csimRememberName', JSON.stringify(csimRememberName), {expires: 36500, path: '/'});
    //跳转
    if (location.href.search(/login.html/) == -1) {
        oSelf.returnUrl != null ? window.open(oSelf.returnUrl) : window.location.reload();
    } else if (document.referrer && document.referrer.search(/register|findPassword|login/) == -1) {
        window.location.replace(document.referrer);
    } else {
        window.location.replace(location.origin);
    }
}

Login.prototype.userInfoErr = function () {	//账号密码错误
    var oSelf = this;
    $('.userPassword #password').val('');
    $('.userPassword #showPassword').val('');
    oSelf.password = false;
    $('.login-content .login-err').hide();
    $('.userID #userID').focus();
    oSelf.userID = false;
    $('.userID p').show().html('账号或密码错误，请重新输�?');
    $('#imgCode').val('');
    oSelf.randomImgSrc();
}

Login.prototype.verifyCodeErr = function () {  //验证码错�?
    var oSelf = this;
    $('#imgCode').val('').focus();
    oSelf.randomImgSrc();
    $('.login-content .login-err').hide();
    $('.imgCode p').show().html('图片验证码错误，请重新输�?');
};

Login.prototype.blockingUser = function () {	//账号冻结不可�?
    var oSelf = this;
    $('.userPassword #password').val('');
    $('.userPassword #showPassword').val('');
    oSelf.password = false;
    $('.login-content .login-err').show().html('该账号已冻结');
    $('.userID #userID').focus().val('');
    oSelf.userID = false;
    $('#imgCode').val('');
    oSelf.randomImgSrc();
}

Login.prototype.timeout = function () {	//验证码超�?
    var oSelf = this;
    $('.imgCode #imgCode').focus().val('');
    oSelf.randomImgSrc();
    $('.login-content .login-err').hide();
    $('.imgCode p').show().html('图片验证码超时，请重新输�?');
}

Login.prototype.getBrowser = function () {
    var userBrowser = "";
    var userAgent = navigator.userAgent.toLowerCase(); //取得浏览器的userAgent字符�?
    var regStr_ie = /trident\/[\d.]+;/gi;
    var regStr_ff = /firefox\/[\d.]+/gi;
    var regStr_chrome = /chrome\/[\d.]+/gi;
    var regStr_saf = /safari\/[\d.]+/gi;
    var regStr_opr = /opr\/[\d.]+/gi;
    var regStr_qq = /qqbrowser\/[\d.]+/gi;
    var regStr_metasr = /metasr [\d.]+/gi;
    var regStr_ubrowser = /ubrowser\/[\d.]+/gi;
    if (userAgent.indexOf("trident") > 0) {
        var version = '';
        if (userAgent.indexOf("msie") > 0) {
            version = userAgent.match(/msie [\d.]+;/gi)[0].replace('msie ', '');
        } else {
            version = "11.0";
        }
        userBrowser = "IE浏览�?/" + version;//IE
    } else if (userAgent.indexOf("qqbrowser") > 0) {
        userBrowser = userAgent.match(regStr_qq)[0].replace("qqbrowser", "QQ浏览�?");//QQ
    } else if (userAgent.indexOf("opr") > 0) {
        userBrowser = userAgent.match(regStr_opr)[0].replace("opr", "欧朋浏览�?");//欧朋
    } else if (userAgent.indexOf("metasr") > 0) {
        userBrowser = userAgent.match(regStr_metasr)[0].replace("metasr", "搜狗浏览�?");//搜狗
    } else if (userAgent.indexOf("ubrowser") > 0) {
        userBrowser = userAgent.match(regStr_ubrowser)[0].replace("ubrowser", "UC浏览�?");//搜狗
    } else if (userAgent.indexOf("firefox") > 0) {
        userBrowser = userAgent.match(regStr_ff)[0].replace("firefox", "火狐浏览�?");//firefox
    } else if (userAgent.indexOf("safari") > 0 && userAgent.indexOf("chrome") < 0) {
        userBrowser = userAgent.match(regStr_saf)[0].replace("safari", "苹果浏览�?");//Safari
    } else if (userAgent.indexOf("chrome") > 0) {
        userBrowser = userAgent.match(regStr_chrome)[0].replace("chrome", "谷歌浏览�?");//chrome
    } else {
        userBrowser = "未知浏览�?";
    }
    this.browser = userBrowser;
}

//获取用户真实Ip
Login.prototype.getIP = function (callback) {
    var oSelf = this;
    if (oSelf.userIp.length == 0) {
        var data = {
            key: "PJSBZ-3VYRU-NTDVZ-4AVLB-K6U76-3ZBTI"
        }
        var url = "https://apis.map.qq.com/ws/location/v1/ip";
        data.output = "jsonp";
        $.ajax({
            type: "get",
            dataType: 'jsonp',
            data: data,
            jsonp: "callback",
            jsonpCallback: "QQmap",
            url: url,
            success: function (res) {
                if (res.status == 0) {
                    oSelf.userIp = res.result.ip;
                    var province = res.result.ad_info.province || '';
                    var city = res.result.ad_info.city || '';
                    var district = res.result.ad_info.district || '';
                    oSelf.loginArea = province + city + district;
                }
                if (callback) {
                    callback();
                }
            },
            error: function (err) {
                if (callback) {
                    callback();
                }
            }
        });
    } else {
        if (callback) {
            callback();
        }
    }

}

