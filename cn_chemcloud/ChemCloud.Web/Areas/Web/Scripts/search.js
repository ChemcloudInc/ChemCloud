var totalcount = 0;
var maxcount = 0;
var pagesize = 10;
var pageno = 1;
$(function () {


    var exp_keywords = $("#hdexp_keywords").val();//结构式查询参数

    var keywords = $("#hdkeywords").val();//关键词
    var islike = $("#hdislike").val();//0模糊查询1精确查询
    QueryResultCount(keywords, islike, exp_keywords);
    QueryResult(keywords, islike, exp_keywords);
    $(window).scroll(function () {
        // 当滚动到最底部以上500像素时， 加载新的查询结果
        if ($(document).height() - $(this).scrollTop() - $(this).height() < 500) {
            if (pageno < maxcount) {
                pageno = pageno + 1;
                QueryResult(keywords, islike, exp_keywords);
                $(".list-h li").lazyload();
                return false;
            }
        }
    });
});
function QueryResultCount(keywords, islike, exp_keywords) {
    $.ajax({
        type: 'post',
        url: '/search/SearchResultCount',
        data: {
            "keywords": keywords,
            "islikevalue": islike, "exp_keywords": exp_keywords
        },
        dataType: "json",
        success: function (data) {
            $("#res_count").html(data);
            totalcount = parseInt(data);
            maxcount = totalcount / 10;
        }
    });
};
function QueryResult(keywords, islike, exp_keywords) {
    var loading = showLoading1();
    $.ajax({
        type: 'post',
        url: '/search/SearchResult',
        data: {
            "keywords": keywords,
            "islikevalue": islike,
            "pagesize": pagesize,
            "pageno": pageno, "exp_keywords": exp_keywords
        },
        dataType: "json",
        async: false,
        success: function (data) {
            var html = "";
            if (data == "") {
                html += "<span>很抱歉，没有找到与“<em style=\"color:red;\">" + keywords + "</em>”相关的产品。</span>";
            } else {
                if (data.indexOf("Total") > -1 && data.indexOf("ds") > -1) {
                    var json = $.parseJSON(data);
                    $.each(json.ds, function (i, item) {
                        if (item.CAS == null || item.CAS == "0") {
                            item.CAS = "";
                        }
                        if (item.Record_Title == null || item.Record_Title == "0") {
                            item.Record_Title = "";
                        }
						
						if (item.CHINESE == null || item.CHINESE == "0") {
                            item.CHINESE = "";
                        }

                        if (item.Molecular_Weight == null || item.Molecular_Weight == "0") {
                            item.Molecular_Weight = "";
                        }
                        var oMOLECULAR_FORMULA = "";
                        if (item.Molecular_Formula == null || item.Molecular_Formula == "0") {
                            item.Molecular_Formula = "";
                            oMOLECULAR_FORMULA = item.Molecular_Formula;
                        }
                        var pname = "";
                       
					   
					     if (item.Record_Title != "") {
                            if (item.Record_Title.length > 20) {
                                pname = item.Record_Title.substring(0, 20) + "...";
                            } else {
                                pname = item.Record_Title;
                            }
                        } else if (item.CHINESE != "") {
                            if (item.CHINESE.length > 20) {
                                pname = item.CHINESE.substring(0, 20) + "...";
                            } else {
                                pname = item.CHINESE;
                            }
                        }


                        var imgdataurl = "https://pubchem.ncbi.nlm.nih.gov/image/imgsrv.fcgi?cid=" + item.PUB_CID + "&t=l";
                        if (item.Dataurl != "" && item.Dataurl.length > 10) {
                            imgdataurl = item.Dataurl;
                        }


                        item.Molecular_Formula = item.Molecular_Formula.replace(/(\d+)/g, "<sub>$1</sub>");
                        html += "<li>";
                        html += "   <div class=\"lh-wrap\" style='width:210px;'>";
                        html += "        <div class=\"p-img\"><a alt=\"" + item.Record_Title + ";" + oMOLECULAR_FORMULA + "\" title=\"" + item.Record_Title + ";" + oMOLECULAR_FORMULA + "\" target=\"_blank\" href=\"/Search/Search_Product_Shops?id=" + item.PUB_CID + "\"><img class=\"lazyload\" src=\"" + imgdataurl + "\" data-url=\"" + imgdataurl + "\" ></a></div>";
                        html += "        <div class=\"p-price\"><strong>" + pname + "</strong></div>";
                        html += "        <div class=\"p-price\">CAS#：" + item.CAS + "</div>";
                        html += "        <div class=\"p-price\">分子量：" + item.Molecular_Weight + "</div>";
                        html += "        <div class=\"p-price\"  id=\"fenzishi\"><label style=\"float:left;width:140px;\" id=\"lblShopName_" + item.PUB_CID + "\"></label><label style=\"float:right;color:#3498DB\">分子式：" + item.Molecular_Formula + "</label></div>";
                        html += "   </div>";
                        html += "</li>";
                    });
                } else {
                    html += "<span>很抱歉，没有找到与“<em style=\"color:red;\">" + keywords + "</em>”相关的产品。</span>";
                }
            }
            $(".list-h").append(html);
            loading.close();
        }
    });
};

function showLoading1(msg, delay) {
    /// <param name="msg" type="String">待显示的文本,非必填</param>
    /// <param name="delay" type="Int">延时显示的毫秒数，默认100毫秒显示,非必填</param>
    if (!delay)
        delay = 100;
    var loading = $('<div class="ajax-loading" style="display:none"><table height="100%" width="100%"><tr><td align="center"><p>' + (msg ? msg : '') + '</p></td></tr></table></div>');
    loading.appendTo('body');
    var s = setTimeout(function () {
        if ($(".ajax-loading").length > 0) {
            loading.show();
            $('.container,.login-box').addClass('blur');
        }
    }, delay);
    return {
        close: function () {
            clearTimeout(s);
            loading.remove();
            $('.container,.login-box').removeClass('blur');
        }
    }

}

