var totalcount = 0;
var skey = "";
$(function () {
    var keywords = $("#hdkeywords").val();//关键词
    skey = keywords;
    var pageNo = getUrlParam("pageNo");//页面跳转页码
    if (pageNo == null || pageNo == "") {
        pageNo = 1;
    }
    QueryCount(keywords, pageNo);
    Query(keywords, pageNo);
});
function Query(keywords, pageNo) {
    var loading = showLoading();
    $.ajax({
        type: 'post',
        url: '/search/SearchCOA',
        data: { "keywords": keywords, "pageNo": pageNo },
        dataType: "json",
        success: function (data) {
            $(".list-h").html("");
            var html = "";
            if (data != null) {
                $("#res_count").html("0");
                if (data.rows.length > 0) {
                    $.each(data.rows, function (i, item) {
                        html += "<li>";
                        html += "   <div class=\"lh-wrap\">";
                        html += "        <div class=\"p-img\" style=\"height:26px;\"><a href=\"javasript:void(0)\"><strong style=\"color:#3498DB;\">CertificateNumber:" + item.CertificateNumber + "</strong></a></div>";
                        html += "        <div class=\"p-name\" style=\"height:26px;\"><strong>ProductCode:" + item.ProductCode + "</strong></div>";
                        html += "        <div class=\"p-name\"><strong>ProductName:" + item.ProductName + "</strong></div>";
                        html += "        <div class=\"p-name\"><strong>Supplier:" + item.Supplier + "</strong></div>";
                        html += "        <div class=\"p-name\"><strong>ManufacturersBatchNo:" + item.ManufacturersBatchNo + "</strong></div>";
                        html += "        <div class=\"p-name\"><strong>SydcosLabBatchNo:" + item.SydcosLabBatchNo + "</strong></div>";
                        html += "        <div class=\"p-name\"><strong>ExpiryDate:" + item.strExpiryDate + "</strong></div>";
                        html += "        <div class=\"p-name\"><strong>DateofManufacture:" + item.strDateofManufacture + "</strong></div>";
                        html += "        <div class=\"p-name\"><strong>LABORATORYMANAGER:" + item.LABORATORYMANAGER + "</strong></div>";
                        html += "   </div>";
                        html += "</li>";
                    });

                    $("#res_count").html(data.rows.length);
                } else { html += "<span style=\"font-size: 15px;\">抱歉，没有找到与“<em>" + keywords + "</em>”相关的COA报告</span>"; }
                $(".list-h").html(html);
            } else {
                html += "<span style=\"font-size: 15px;\">抱歉，没有找到与“<em>" + keywords + "</em>”相关的COA报告</span>";
            }
            loading.close();
        }
    });
};
//CertificateNumber
//ProductCode
//ProductName
//Supplier
//ManufacturersBatchNo
//SydcosLabBatchNo
//ExpiryDate
//DateofManufacture
//LABORATORYMANAGER

function QueryCount(keywords, pageNo) {
    $.ajax({
        type: 'post',
        url: '/search/SearchCOACount',
        data: { "keywords": keywords, "pageNo": pageNo },
        dataType: "json",
        success: function (data) {
            if (data != "") {
                totalcount = parseInt(data);
                $("#res_count").text(data);
                $("#Pagination").pagination(totalcount, {
                    prev_text: "« 上一页",
                    next_text: "下一页 »",
                    num_edge_entries: 1, //边缘页数
                    num_display_entries: 4, //主体页数
                    callback: pageselectCallback,
                    items_per_page: 60 //每页显示1项
                });
            }
        }
    });
};


function pageselectCallback(page_index, jq) {
    if (skey != "") {
        Query(skey, page_index + 1);
    }
}
function getUrlParam(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]); return null;
};
