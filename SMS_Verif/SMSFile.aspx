<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SMSFile.aspx.cs" Inherits="SMS_Verif.SMSFile" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="renderer" content="webkit" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, minimum-scale=1.0, maximum-scale=1.0, user-scalable=0" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>SMS</title>
    <link href="static/css/layui-extension.css" rel="stylesheet" />
    <link href="static/css/layui.css" rel="stylesheet" />
    <link href="static/css/flowplugin.css" rel="stylesheet" />
    <script src="static/jquery-1.11.3.min.js"></script>
    <script src="static/flowplugin.js"></script>
    <script src="static/layui.js"></script>
    <style>
        .layadmin-user-login-footer {
            position: absolute;
            left: 0;
            bottom: 0;
            width: 100%;
            line-height: 30px;
            padding: 20px;
            text-align: center;
            box-sizing: border-box;
            color: rgba(0,0,0,.5);
        }

            .layadmin-user-login-footer span {
                padding: 0 5px;
            }

            .layadmin-user-login-footer a {
                padding: 0 5px;
                color: rgba(0,0,0,.5);
            }

                .layadmin-user-login-footer a:hover {
                    color: rgba(0,0,0,1);
                }
        /*蒙版*/
        .loading-mask {
            width: 100%;
            height: 100%;
            position: fixed;
            top: 0;
            left: 0;
            right: 0;
            bottom: 0;
            background-color: #fff;
            opacity: 0.3;
            filter: alpha(opacity=30);
            z-index: 10000;
        }

        /*动态加载图片*/
        .loading {
            position: fixed;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            background: url(/static/img/loading.gif) 50% 50% no-repeat;
            z-index: 10010;
        }
    </style>
</head>
<body style="background-color: rgba(246, 240, 240, 0.50)">
    <div class="layui-fluid" style="margin-top: 50px;">
        <div class="layui-col-md4 layui-col-md-offset4">
            <fieldset class="layui-elem-field layui-field-title" style="margin-top: 20px;">
                <legend>手机验证</legend>
            </fieldset>
            <div class="layui-form-item">
                <label class="layui-form-label">手机号：</label>
                <div class="layui-input-block">
                    <input type="text" id="iphone" placeholder="请输入手机号" style="width: 65%;" autocomplete="off" class="layui-input" />
                </div>
            </div>
            <div class="layui-form-item">
                <label class="layui-form-label">验证码：</label>
                <div class="layui-input-block">
                    <input type="text" id="vercode" placeholder="请输入图片验证码" style="width: 40%;" class="layui-input" />
                    <img src="http://www.jointac.com/LogisticesServices/AppWeb/ValidateCode.aspx" style="float: left; height: 30px; margin-top: -33px; margin-left: 42%;" class="layadmin-user-login-codeimg" />
                </div>
            </div>
            <div class="layui-form-item">
                <div class="layui-input-block">
                    <input type="text" id="smsid" placeholder="请输入短信验证码" style="width: 40%;" class="layui-input" />
                    <button class="layui-btn layui-btn-sm layui-btn-primary" id="btn-code" style="float: left; height: 38px; line-height: 38px; margin-top: -38px; width: 110px; color: coral; margin-left: 42%; text-align: center;">发送验证码</button><%--<i class="layui-icon"></i>--%>
                </div>
            </div>
            <div class="layui-form-item">
                <div class="layui-input-block">
                    <button class="layui-btn" style="width: 40%;" id="CheckTel">提&nbsp;&nbsp;&nbsp;交</button>
                </div>
            </div>
        </div>
        <div class="layui-col-md6 layui-col-md-offset3">
            <fieldset class="layui-elem-field layui-field-title" style="margin-top: 20px;">
                <legend>信息推送</legend>
            </fieldset>
            <table id="express" lay-filter="express"></table>
        </div>
    </div>
    <div class="layui-trans layadmin-user-login-footer" style="color: #000;">
        <p><span>Copyright © 展通国际物流 2010-2020</span></p>
    </div>
    <div id="showdetail" style="display: none; margin: 20px 20px 10px 10px;">
        <div class="flowtest"></div>
    </div>
</body>
<script type="text/html" id="barDemo">
    <a class="layui-btn layui-btn-primary layui-btn-xs" lay-event="detail">查看</a>
    <a class="layui-btn layui-btn-xs" lay-event="puth">推送</a>
</script>
<script>
    var url = "../Ashx/SMSPuth.ashx";
    var vscodeurl = "http://www.jointac.com/LogisticesServices/AppWeb/ValidateCode.aspx";
    layui.use(['table', 'layer', 'laypage', 'form'], function () {
        var table = layui.table;
        var layer = layui.layer;
        var form = layui.form;
        var laypage = layui.laypage;
        var data = [];
        var code = "";
        var postData = {
            pageNumber: 1,
            pageSize: 10
        };
        // 表单需要的变量
        var infoOptions = {
            elem: '#express',
            width: 850
            , limits: [10]
            , cols: [[
             { field: 'ExpressCompany', title: '运单公司', width: 120 }
            , { field: 'WaybillNumber', title: '运单编号', width: 150 }
            , { field: 'StatusText', title: '状态', width: 90 }
            , { field: 'Sender', title: '发件人', width: 90 }
            , { field: 'ShippingCity', title: '发件地址', width: 90 }
            , { field: 'Receiver', title: '收件人', width: 90 }
            , { field: 'ReceivingCity', title: '收件地址', width: 90 }
            //, { fixed: 'CreateDate', title: '发货日期', width: 120 }
            //, {fixed: 'EstimatedServiceTime', title: '预计到达时间', width: 120}
            , { fixed: 'right', title: '操作', width: 120, align: 'center', toolbar: '#barDemo' }
            ]]
            , data: data
            , id: 'table_sms'
            , page: true
            , even: true
            , height: $(document).height() - $('#express').offset().top - 70
        };
        // 表格初始化 ------------------
        function init() {
            // 完成表格数据
            $.extend(infoOptions, { data: data });
            infoOptions.page = {
                curr: 1
            }
            table.render(infoOptions);
            data = null;
        };
        // 页面初始化---------------------------------
        init(data);
        var maxSecond = 299;
        $("#btn-code").click(function () {
            var $_this = $(this);
            var phone = $("#iphone").val();
            if (phone && checkPhone(phone)) {
                var vscode = $("#vercode").val();
                if (vscode == "") {
                    AlertMsg("请输入图片验证码！");
                    return false;
                }
                $_this.attr("disabled", true);
                $_this.addClass("layui-disabled");
                var val = maxSecond;
                var timer = setInterval(function () {
                    val = maxSecond--;
                    $_this.text(val > 0 ? "(" + val + ")发送验证码" : "发送验证码");
                    $_this.text() == "发送验证码" ? $_this.css({ "color": "coral" }) : $_this.css({ "color": "coral" });
                    if (val == -1) {
                        clearInterval(timer);
                        $_this.attr("disabled", false);
                        $_this.removeClass("layui-disabled");
                        maxSecond = 299;
                    }
                }, 1000);
                var PostData = {
                    param: "sendsms",
                    Miphone: $("#iphone").val()
                }
                $.ajax({
                    url: url,
                    type: 'POST',
                    dataType: 'json',
                    data: PostData,
                    success: function (result) {
                        if (result.code == 0) {
                            AlertMsg("发送成功。");
                        }
                        else {
                            clearInterval(timer);
                            $_this.text("发送验证码");
                            $_this.attr("disabled", false);
                            $_this.removeClass("layui-disabled");
                            maxSecond = 299;
                            $(".layadmin-user-login-codeimg")[0].src = vscodeurl + "?" + Math.random();
                            AlertMsg(result.msg);
                        }
                    }
                });
            }
            else {
                AlertMsg("请输入正确的手机号！");
            }
            return false;
        });
        //验证手机并查询数据
        $("#CheckTel").click(function () {
            var phone = $("#iphone").val();
            if (phone && checkPhone(phone)) {
                var vscode = $("#vercode").val();
                if (vscode == "") {
                    AlertMsg("请输入图片验证码！");
                    return false;
                }
                var sms = $("#smsid").val();
                if (sms == "") {
                    AlertMsg("请输入短信验证码！");
                    return false;
                }
                var PostData = {
                    param: "chkIphone",
                    Miphone: phone,
                    smsCode: $("#smsid").val()
                }
                $.ajax({
                    url: url,
                    type: 'POST',
                    dataType: 'json',
                    data: PostData,
                    success: function (result) {
                        if (result.code == 0) {
                            data = result.data;
                            init();
                        }
                        else if (result.code == -1) {
                            AlertMsg(result.msg);
                        }
                        else {
                            AlertMsg("该手机号下无运单信息！");
                        }
                    }
                });
            }
            else {
                AlertMsg("请输入正确的手机号！");
            }
        });
        //监听工具条
        table.on('tool(express)', function (obj) {
            var data = obj.data;
            if (obj.event === 'detail') {
                //loadingShow(false);
                var mask = layer.load(1, { shade: [0.3, '#eee'], time: 5000 });
                var postdata = {
                    param: "gettrack", WaybillNumber: data.WaybillNumber, ExpressCompany: data.ExpressCompany
                }
                $.ajax({
                    url: url,
                    type: 'POST',
                    dataType: 'json',
                    data: postdata,
                    success: function (result) {
                        //loadingHide();
                        layer.close(mask);
                        if (result.code == 0) {
                            $(".flowtest").html("");
                            var testDate = { "data": [] };
                            var postdata = result.data;
                            if (postdata.length == 0) {
                                AlertMsg("抱歉，运单：" + data.WaybillNumber + "下无跟踪信息，无法查看。");
                                return false;
                            }
                            $.each(postdata, function (index, item) {
                                var data = {
                                    "status": item.describe,
                                    "createDate": item.datetime,
                                    "jobName": item.scan
                                };
                                testDate.data.push(data);
                            });
                            var opt = {
                                "jsonDate": testDate,//json数据
                                "imgSrcStart": "../static/img/f1.png",//最新流程节点图片（第一个节点）
                                "imgSrcOther": "../static/img/f2.png",//其它流程节点图片
                                "imgWidth": "20px",//图片宽  设置第一个图片的宽度
                                "imgHeight": "20px"//图片高  设置第一个图片的高度
                            };
                            $(".flowtest").flowplugin(opt);
                            //弹框
                            var index = layer.open({
                                id: 'detailid',
                                type: 1,
                                area: ['450px', '700px'],
                                title: '快递信息-' + data.WaybillNumber,
                                fixed: false, //不固定
                                maxmin: false,
                                btn: ['关闭'],
                                content: $("#showdetail"),
                                cancel: function (index, olayer) {
                                    layer.close(index);
                                }
                            });
                        }
                        else if (result.code == 400) {
                            AlertMsg("暂无跟踪信息");
                        }
                        else {
                            AlertMsg(result.msg);
                        }
                    }
                });
            } else if (obj.event === 'puth') {
                var mask = layer.load(1, { shade: [0.3, '#eee'], time: 5000 });
                var obj = {
                    param: "puthmsg",
                    ExpressCompany: data.ExpressCompany,
                    WaybillNumber: data.WaybillNumber,
                    Miphone: $("#iphone").val()
                };
                $.ajax({
                    url: url,
                    type: 'POST',
                    dataType: 'json',
                    data: obj,
                    success: function (result) {
                        layer.close(mask);
                        if (result.code == 0) {
                            AlertMsg("发送成功。");
                        }
                        else {
                            AlertMsg(result.msg);
                        }
                    }
                });
            }
        });
        //alert
        function AlertMsg(msg) {
            //配置一个透明的询问框
            layer.msg(msg, {
                time: 3000 //3s后自动关闭
            });
        }

    });
    //刷新验证码
    $(".layadmin-user-login-codeimg").click(function () {
        $(this)[0].src = vscodeurl + "?" + Math.random();
    });
    //增加showdetail遮罩
    function maskElement(maskLevel) {
        if (!maskLevel) {
            maskLevel = '0';
        }
        if ($(".mask-level-" + maskLevel).length == 0) {
            $("#showdetail").append('<div class="loading-mask mask-level-' + maskLevel + '"></div>');
        }
    }
    //取消showdetail遮罩
    function unmaskElement(maskLevel) {
        if (!maskLevel) {
            maskLevel = '0';
        }
        $(".mask-level-" + maskLevel).remove();
    }

    function loadingShow(isMask) {
        if (isMask) {
            maskElement("0");
        }
        if ($(".loading").length == 0) {
            $("#showdetail").append('<div class="loading"></div>');
        }
    }

    function loadingHide() {
        unmaskElement("0");
        $(".loading").remove();
    }

    //验证手机号
    function checkPhone(phone) {
        var regx = new RegExp("^((13[0-9])|(14[5,7])|(15[0-3,5-9])|(17[0,3,5-8])|(18[0-9])|166|198|199|(147))\\d{8}$");
        return regx.test(phone);
    }
</script>
</html>

