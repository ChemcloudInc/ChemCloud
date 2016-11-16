
var categoryId;
var lastType;
var curType;

$(function () {

    /*查询*/
    $('#searchButton').click(function (e) {
        search();
    });

    search();

    /*特价*/
    $('#list').on('click', '.good-tj', function () {
        var name = $(this).parent().find('.thisName').val();
        var ids = $(this).parent().find('.thisId').val();
        $.dialog.confirm('您确定要申请特价？', function () {
            var loading = showLoading();
            $.post('UpdateSpecicalOffer', { ids: ids.toString(), status: "3" }, function (result) {
                loading.close();
                if (result.success) {
                    $.dialog.tips('申请成功');
                    var pageNo = $("#list").hiMallDatagrid('options').pageNumber;
                    reload(pageNo);
                }
                else {
                    $.dialog.alert('申请失败!' + result.msg);
                    setTimeout(5000);
                    loading.close();
                }
            });
        });
    });


    /*下架*/
    $('#list').on('click', '.good-down', function () {
        var name = $(this).parent().find('.thisName').val();
        var ids = $(this).parent().find('.thisId').val();
        $.dialog.confirm('您确定要下架' + (name ? ' “' + name + '” ' : ('这' + ($.isArray(ids) ? ids.length : 1) + '件产品')) + '吗？', function () {
            var loading = showLoading();
            $.post('batchSaleOff', { ids: ids.toString() }, function (result) {
                loading.close();
                if (result.success) {
                    $.dialog.tips('下架产品成功');
                    var pageNo = $("#list").hiMallDatagrid('options').pageNumber;
                    reload(pageNo);
                }
                else
                    $.dialog.alert('下架产品失败!' + result.msg);
            });
        });

    });
    /*上架*/
    $('#list').on('click', '.good-up', function () {
        var name = $(this).parent().find('.thisName').val();
        var ids = $(this).parent().find('.thisId').val();
        var saleStatus = $(this).parent().find('.salesStatus').val();
        if (saleStatus != 1) {
            $.dialog.confirm('您确定要上架' + (name ? ' “' + name + '” ' : ('这' + ($.isArray(ids) ? ids.length : 1) + '件产品')) + '吗？', function () {
                var loading = showLoading();
                $.post('batchOnSale', { ids: ids.toString() }, function (result) {
                    loading.close();
                    if (result.success) {
                        $.dialog.tips('申请产品上架成功');
                        var pageNo = $("#list").hiMallDatagrid('options').pageNumber;
                        reload(pageNo);
                    }
                    else {
                        $.dialog.alert('申请产品上架失败!' + result.msg);
                        setTimeout(5000);
                        loading.close();
                    }
                });
            });

        }
        else {
            $.dialog.tips('该产品已经处于上架状态');
            setTimeout(5000);
            loading.close();
        }
    });

    /*认证*/
    $('#list').on('click', '.good-author', function () {
        var id = $(this).parent().find(".thisId").val();
        var productAuthor = $(this).parent().find('.thisAuthor').val();
        var loading = showLoading();
        if (productAuthor == 3) {
            $.dialog.tips('该产品已经通过认证');
            setTimeout(5000);
            loading.close();
        }
        else {
            location.href = './ProductAuthenticationInfo?productId=' + id;
        }

    });

    /*删除*/
    $('#list').on('click', '.good-del', function () {
        var name = $(this).parent().find('.thisName').val();
        var ids = $(this).parent().find('.thisId').val();
        $.dialog.confirm('您确定要删除' + (name ? ' “' + name + '” ' : ('这' + ($.isArray(ids) ? ids.length : 1) + '件产品')) + '吗？', function () {
            var loading = showLoading();
            $.post('Delete', { ids: ids.toString() }, function (result) {
                loading.close();
                if (result.success) {
                    $.dialog.tips('删除产品成功');
                    var pageNo = $("#list").hiMallDatagrid('options').pageNumber;
                    reload(pageNo);
                }
                else
                    $.dialog.alert('删除产品失败!' + result.msg);
            });
        });
    });

    $('#list').on('hover', '.good-share', function () {
        $(this).toggleClass('active');
    });

    /*打印*/
    $('#productApply').click(function () {
        var loading = showLoading();
        var ids = getSelectedIds();
        if (ids.length == 1) {
            loading.close();
            window.open("/SellerAdmin/Product/Print?id=" + ids, "_blank");

        } else if (ids.length == 0) {
            $.dialog.tips('请选择需要打印的产品');
            setTimeout(3000);
            loading.close();
        } else {
            $.dialog.tips('请重新选择需要打印的产品');
            setTimeout(3000);
            loading.close();
        }
    })
});

/*删除*/
function deleteProduct(ids) {
    $.dialog.confirm('您确定要删除这些产品吗？', function () {
        var loading = showLoading();
        $.post('Delete', { ids: ids.join(',').toString() }, function (result) {
            loading.close();
            if (result.success) {
                $.dialog.tips('删除产品成功');
                var pageNo = $("#list").hiMallDatagrid('options').pageNumber;
                reload(pageNo);
            }
            else
                $.dialog.alert('删除产品失败!' + result.msg);
        });
    });
}


/*查询*/
function search() {
    var casno = $.trim($('#txtCASNo').val());
    var brandName = $.trim($('#brandBox').val());
    var keyWords = $.trim($('#searchBox').val());
    var ename = $.trim($('#engname').val());
    var productCode = $.trim($('#productId').val());
    var shopName = $.trim($('#shopName').val());
    var productAuthor = $('select[name="saleState"]').val();
    var saleStatus = $('#productStatus').val();
    $("#list").hiMallDatagrid({
        url: "./List",
        nowrap: false,
        rownumbers: true,
        border: false,
        fit: true,
        fitColumns: true,
        pagination: true,
        NoDataMsg: '没有找到符合条件的数据',
        idField: "Id",
        pageSize: 15,
        pageNumber: 1,
        queryParams: {
            brandName: brandName, keyWords: keyWords,ename:ename,
            categoryId: categoryId, productCode: productCode, shopName: shopName,
            productAuthor: productAuthor, CASNo: casno,
            saleStatus: saleStatus
        }, toolbar: "#shopToolBar",
        columns:
        [[
            { checkbox: true, width: 39 },
            { field: "IsLimitTimeBuy", hidden: true, width: 39 },
            { field: "Id", title: '产品编号', hidden: true, width: 50 },
            { field: "ProductCode", title: '产品货号', width: 70 },
             { field: "CASNo", title: 'CAS #', width: 80 },
             {
                 field: "Name", title: '产品名称', formatter: function (value, row, index) {
                     var html = "";
                     
                     html = "<div title=" + row.Name + " style='text-align: center;overflow: hidden;white-space: nowrap;text-overflow: ellipsis;width: 80px;'>" + row.Name + "</div>";
                     
                     return html;
                 }
             },
                  {
                      field: "EProductName", title: '产品英文名称', formatter: function (value, row, index) {
                          var html = "";

                          html = "<div title=" + row.EProductName + " style='text-align: center;overflow: hidden;white-space: nowrap;text-overflow: ellipsis;width: 80px;'>" + row.EProductName + "</div>";

                          return html;
                      }
                  },
             { field: "Purity", title: '纯度', width: 80 },
             {
                 field: "Price", title: "价格", width: 50, align: "left",
                 formatter: function (value, row, index) {
                     return value.toFixed(2);
                 }
             },
             { field: "PublishTime", title: "发布时间", width: 120, align: "left" },
             {
                 field: "SaleState", title: "状态", formatter: function (value, row, index) {
                     var html = "";
                     var dtype = row.SaleState;
                     if (dtype == "1") {
                         html = "<span style=\"color:green;\">销售中</span>";
                     } else {
                         html = "<span style=\"color:red;\">未上架</span>";
                     }
                     return html;
                 }
             },
            {
                field: "s", title: "操作", align: "center",
                formatter: function (value, row, index) {
                    html = '<span class="btn-a"><input class="thisId" type="hidden" value="' + row.Id + '"/><input class="salesStatus" type="hidden" value="' + row.SaleState + '"/><input class="thisName" type="hidden" value="' + row.Name + '"/>';
                    var edit = "edit";
                    if (row.CASNo == null || row.CASNo == "") {
                        var edit = "edit";
                        html += '<a class="good-check" href="PublicStepThree?productId=' + row.Id + '">编辑</a>';

                    }
                    else {
                        html += '<a class="good-check" href="PublicStepTwo?productId=' + row.Id + '">编辑</a>';
                    }
                    html += '<a class="good-del">删除</a>';
                    if (row.SaleState == 1) {
                        html += '<a class="good-down">下架</a>';
                    } else {
                        html += '<a class="good-up">上架</a>';
                    }
                    html += '<a class="good-tj">申请特价</a>';
                    return html;
                }
            },
            {
                field: "ProductAuth", title: "产品认证操作", align: "center",
                formatter: function (value, row, index) {

                    var html = "";
                    html += '<span class="btn-a"><input class="thisAuthor" type="hidden" value="'
                        + row.ProductAuth + '"/><input class="thisId" type="hidden" value="'
                        + row.Id + '"/></span>';

                    if (row.ProductAuth == 3) {
                        html += '<span style="color:green;">已认证</span>';
                    } else if (row.ProductAuth == 0) {
                        html += '<a class= "good-author" style="cursor:pointer">申请产品认证</a>';
                    } else {
                        html += '<a class= "good-author" style="cursor:pointer">查看进度</a>';
                    }
                    return html;
                }
            },
        ]],
        onLoadSuccess: function () {
            initBatchBtnShow();
        }
    });
}

/*刷新*/
function reload(pageNo) {

    $("#list").hiMallDatagrid('reload', { pageNumber: pageNo });
}

/*选中*/
function getSelectedIds() {
    var selecteds = $("#list").hiMallDatagrid('getSelections');
    var ids = [];
    $.each(selecteds, function () {
        ids.push(this.Id);
    });
    return ids;
}

/*下架*/
function saleOff(ids) {
    $.dialog.confirm('您确定要下架这些产品吗？', function () {
        var loading = showLoading();
        $.post('batchSaleOff', { ids: ids.join(',').toString() }, function (result) {
            loading.close();
            if (result.success) {
                $.dialog.tips('下架产品成功');
                var pageNo = $("#list").hiMallDatagrid('options').pageNumber;
                reload(pageNo);
            }
            else
                $.dialog.alert('下架产品失败!' + result.msg);
        });
    });
}

/*上架*/
function onSale(ids) {
    $.dialog.confirm('您确定要上架这些产品吗？', function () {
        var loading = showLoading();
        $.post('batchOnSale', { ids: ids.join(',').toString() }, function (result) {
            loading.close();
            if (result.success) {
                $.dialog.tips('上架产品成功');
                var pageNo = $("#list").hiMallDatagrid('options').pageNumber;
                reload(pageNo);
            }
            else
                $.dialog.alert('上架产品失败!' + result.msg);
        });
    });
}


/*初始化事件*/
function initBatchBtnShow() {
    /*批量下架*/
    $('#batchSaleOff')
        .show()
        .unbind('click')
        .click(function () {
            var ids = getSelectedIds();
            if (ids.length > 0)
                saleOff(ids);
            else
                $.dialog.tips('请至少选择一件产品');
        });


    /*批量上架*/
    $('#batchOnSale')
       .show()
       .unbind('click')
       .click(function () {
           var ids = getSelectedIds();
           if (ids.length > 0)
               onSale(ids);
           else
               $.dialog.tips('请至少选择一件产品');
       });


    /*批量删除*/
    $('#batchDelete')
        .unbind('click')
        .click(function () {
            var ids = getSelectedIds();
            if (ids.length > 0)
                deleteProduct(ids);
            else
                $.dialog.tips('请至少选择一件产品');
        });

}





