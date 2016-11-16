$(function () {
    loadCartInfo();

    $('#toSettlement').click(function () {
        bindToSettlement();
    });

    $(".submitorder").live("click", function () {
        var ShoppingCartsId = $(this).attr('id');

        $.dialog.confirm('确定提交订单？', function () {
            window.location.href = "/Order/Submit?cartItemId=" + ShoppingCartsId;
        });
    });

    $("#submitorderall").click(function () {
        var memberid = $("#hduserid").val();
        $.dialog.confirm('确定提交订单？', function () {
            window.location.href = "/Order/Submit?cartItemId=" + memberid;
        });
    });
});

var data = {
};

function loadCartInfo() {

    $.post('/cart/GetCartProducts', {
    }, function (cart) {
        data = {
        };
        $.each(cart.products, function (i, e) {
            if (data[e.shopId]) {
                if (!data[e.shopId]['name']) {
                    data[e.shopId]['name'] = e.shopName;
                    data[e.shopId]['sid'] = e.shopId;
                }
                data[e.shopId]['shop'].push(e);
            } else {
                data[e.shopId] = {
                };
                data[e.shopId]['shop'] = [];
                data[e.shopId]['name'] = e.shopName;
                data[e.shopId]['sid'] = e.shopId;
                data[e.shopId]['status'] = e.productstatus;
                data[e.shopId]['shop'].push(e);
            }
        });
        var strproductstatus = $("#hidSaleStatus").val();
        var strproductauditstatus = $("#hidAuditStatus").val();
        var str = '';
        var memberId = $.cookie('ChemCloud-User');
        if (memberId) {
            $('.cart-inner .message').find('.unLogin').hide();
        }
        if (cart.products.length == 0) {
            $('.cart-inner').addClass('cart-empty');
        } else {
            $.each(data, function (i, e) {
                str += '\
                  <div class="cart-toolbar cl">\
                    <span class="column t-checkbox form">\
                      <input type="checkbox" style="margin: 15px 5px 0 9px;" class="shopSelect" value="' + i + '" name="checkShop" checked="">\
                      <label for=""><span>' + e.name + '-' + e.sid + '</span></label>\
                    </span>\
                    <input type="button" value="提交订单" class="submitorder" id="'+ e.sid + '" style="float:right;margin-top:17px;margin-right:16px;background: #3498DB;border:none 0;color:#fff;height:23px;width:66px; display:none;" />\
                  </div>';
                $.each(e.shop, function (j, product) {
                    if (product.productstatus != strproductstatus) {
                        str += '\
                        <div class="item item_disabled ">\
                          <div class="item_form cl">\
                            <div class="cell p-checkbox">\
                              <span status=' + product.productstatus + ' name="checkItem" class="checkbox">失效</span>\
                            </div>'
                    } else {
                        if (product.productauditstatus != strproductauditstatus) {
                            str += '\
                            <div class="item item_disabled">\
                              <div class="item_form cl">\
                                <div class="cell p-checkbox">\
                                  <span status=' + product.productauditstatus + ' name="checkItem" class="checkbox">失效</span>\
                                </div>'
                        } else {
                            str += '\
                            <div class="item item_selected ">\
                              <div class="item_form cl">\
                                <div class="cell p-checkbox">\
                                  <input class="checkbox" disabled="disabled" type="checkbox" data-cartid="' + product.cartItemId + '" name="checkItem" checked="" value="' + product.shopId + '" sku="' + product.cartItemId + '" />\
                                </div>'
                        }
                    }

                    str += '<div class="cell p-goods">\
                  <div class="p-img"><a href="javascript:void(0)"><img src="' + product.imgUrl + '"/></a></div>\
                  <div class="p-name"><span>' + product.name + '</span><br>' + (product.productstatus != 1 || product.productauditstatus == 4 ? "[已停售]" : "") + '</div>'
                    if (product.productcode.length > 0) {
                        str += '<div class="p-code">    ：' + product.productcode + ' &nbsp;包装规格：' + product.PackingUnit + ' &nbsp;等级：' + product.SpecLevel + ' &nbsp;纯度：' + product.Purity + '</div>'
                    }
                    str += '</div>\
                <div class="cell p-quantity">\
                  <div>\
                        <span class="num">' + product.Quantity + '</span>\
                  </div>\
                </div>\
                <div class="cell p-price" style="text-align:center;"><span class="price">' + product.ProductTotalAmount.toFixed(2) + '&nbsp;' + product.CoinType + '</span></div>\
                <div class="cell p-remove"><a class="cart-remove" href="javascript:removeFromCart(\'' + product.cartItemId + '\')">删除</a></div>\
              </div>\
            </div>';
                });
            });
            $('#product-list').html(str);

            $('#selectedCount').html(cart.totalCount);

            $('#finalPrice').html(cart.amount.toFixed(2));

            bindAddAndReduceBtn();
            bindBatchRemove();
            bindSelectAll();
        }
    });


}

function bindToSettlement() {
    var memberId = $.cookie('ChemCloud-User');
    var arr = [], str = '';
    $('input[name="checkItem"]').each(function (i, e) {
        if ($(e).attr('checked')) {
            arr.push($(e).attr('data-cartid'));
        }
    });

    str = (arr && arr.join(','));

    if (memberId) {
        if (str != "")
            location.href = '/order/submit?' + 'cartItemIds=' + str;
        else
            $.dialog.errorTips("没有可结算的产品！");
    }
    else {
        $.fn.login({
        }, function () {
            location.href = '/order/submit';
        }, '', '', '/Login/Login');
    }
}

function bindSelectAll() {
    $('input[name="checkAll"]').change(function () {
        var checked = $(this).attr('checked');
        checked = checked ? true : false;
        if (checked) {
            $('#product-list input[type="checkbox"]').attr('checked', checked);
            $('input[name="checkAll"]').attr('checked', checked);
            var total = getCheckProductPrice();
            $('#finalPrice').html('' + total);
        }
        else {
            $('#product-list input[type="checkbox"]').removeAttr('checked');
            $('input[name="checkAll"]').removeAttr('checked');
            $('#finalPrice').html('' + "0.00");

        }
        $('#selectedCount').html(getCheckProductCount());
    });

    $('input[name="checkShop"]').click(function () {
        var checked = $(this).attr('checked'),
            v = $(this).val();
        checked = checked ? true : false;



        $('#product-list input[type="checkbox"]').each(function (i, e) {
            var a = $(e).val();
            if (a == v) {
                $(e).attr('checked', checked);
            }
        });
        var allShopChecked = true;
        $('#product-list input[type="checkbox"]').each(function (i, e) {
            if (!$(this).attr('checked')) {
                allShopChecked = false;
            }
        });
        if (allShopChecked)
            $('input[name="checkAll"]').attr('checked', checked);
        else
            $('input[name="checkAll"]').removeAttr('checked');
        $('#selectedCount').html(getCheckProductCount());

        var t = 0;


        $.each($('input[name="checkItem"]:checked'), function () {
            var a = $(this).parent().parent().find('.price').html();
            t += parseFloat(a);
        })

        $('#finalPrice').html(t.toFixed(2));

    });

    $('input[name="checkItem"]').change(function () {
        var checked = $(this).attr('checked'),
            v = $(this).val();
        checked = checked ? true : false;
        if (checked) {
            $(this).attr('checked', checked);
            $(this).parents('.item').addClass('item_selected')
        } else {
            $(this).removeAttr('checked');
            $(this).parents('.item').removeClass('item_selected')
        }

        //判断供应商下的所有产品是否全选中
        var allProductChecked = false;
        $('input[name="checkItem"]').eachenqi9ch(function (i, e) {
            if ($(e).val() == v) {
                if ($(e).attr('checked'))
                    allProductChecked = true;
            }
        });
        if (allProductChecked) {
            $('input[name="checkShop"]').each(function () {
                if ($(this).val() == v)
                    $(this).attr('checked', checked);
            });

            $('input[name="checkItem"]').each(function () {
                if ($(this).val() == v)
                    $(this).attr('checked', checked);
            });
        }
        else {
            $('input[name="checkShop"]').each(function () {
                if ($(this).val() == v)
                    $(this).removeAttr('checked');
            });
        }

        //判断所有供应商是否都选中了
        var allShopChecked = true;
        $('#product-list input[type="checkbox"]').each(function (i, e) {
            if (!$(this).attr('checked')) {
                allShopChecked = false;
            }
        });
        if (allShopChecked)
            $('input[name="checkAll"]').attr('checked', checked);
        else
            $('input[name="checkAll"]').removeAttr('checked');


        $('#finalPrice').html(getCheckProductPrice());
        $('#selectedCount').html(getCheckProductCount());
    });

}

function removeFromCart(skuId) {
    var loading = showLoading();
    $.post('/cart/RemoveFromCart', {
        skuId: skuId
    }, function (result) {
        loading.close();
        if (result.success)
            loadCartInfo();
        else
            $.dialog.errorTips(result.msg);
    });
}

function bindAddAndReduceBtn() {
    $('a.decrement').click(function () {
        var skuId = $(this).attr('sku');
        var textBox = $('input[name="count"][sku="' + skuId + '"]');
        var count = parseInt(textBox.val());
        if (count > 1) {
            count -= 1;
            textBox.val(count);
            updateCartItem(skuId, count);
            if ($(this).parent().parent().parent().find('input[name="checkItem"]').is(":checked")) {
                $('#finalPrice').html(getCheckProductPrice());
                $('#selectedCount').html(getCheckProductCount());
            }
        }
    });

    $('a.increment').click(function () {
        var skuId = $(this).attr('sku');
        var textBox = $('input[name="count"][sku="' + skuId + '"]');
        var count = parseInt(textBox.val());

        if (count > 0 && count < 100) {
            count += 1;
            textBox.val(count);
            updateCartItem(skuId, count);
            if ($(this).parent().parent().parent().find('input[name="checkItem"]').is(":checked")) {
                $('#finalPrice').html(getCheckProductPrice());
                $('#selectedCount').html(getCheckProductCount());
            }
        } else {
            $.dialog.errorTips('最多不能大于 100 件');
            textBox.val(100);
        }
    });

    $('input[name="count"]').keyup(function () {
        var skuId = $(this).attr('sku');
        var count = parseInt($(this).val());
        if (count > 0 && count < 100) {
            updateCartItem(skuId, count);
            if ($(this).parent().parent().parent().find('input[name="checkItem"]').is(":checked")) {
                $('#finalPrice').html(getCheckProductPrice());
                $('#selectedCount').html(getCheckProductCount());
            }
        }
        else {
            $(this).val('1');
            updateCartItem(skuId, 1);
            $('#finalPrice').html(getCheckProductPrice());
        }
    });
}

function updateCartItem(skuId, count) {
    var loading = showLoading();
    $.post('/cart/UpdateCartItem', {
        skuId: skuId, count: count
    }, function (result) {
        loading.close();
        if (result.success)
            ;
        else
            $.dialog.errorTips(result.msg);
    });
}

function bindBatchRemove() {
    $('#remove-batch').click(function () {
        var skuIds = [];
        $.each($('#product-list input[type="checkbox"]:checked'), function (i, checkBox) {
            skuIds.push($(checkBox).attr('sku'));
        });
        if (skuIds.length < 1) {
            $.dialog.errorTips("请选择要删除的产品！");
            return;
        }
        var loading = showLoading();
        $.post('/cart/BatchRemoveFromCart', {
            skuIds: skuIds.toString()
        }, function (result) {
            loading.close();
            if (result.success)
                loadCartInfo();
            else
                $.dialog.errorTips(result.msg);
        });
    });
}
function priceAll(tag, bool, checked) {

    var t = 0;
    if (bool) {
        $(tag).parent().parent().parent().find('.item_form').each(function (i, e) {
            var a = $(this).find('.price').html();

            t += parseFloat(a);
        });
        return t.toFixed(2);
    }
    if (typeof tag == 'string') {
        $(tag).each(function (i, e) {
            if ($(this).find(".checkbox").attr("status") == $("#hidSaleStatus").val() && $(this).find(".checkbox").attr("status") != $("#hidAuditStatus").val()) {
                var a = $(this).find('.price').html();

                t += parseFloat(a);
            }
        });
    } else {
        if (checked) {
            $(tag).parent().parent().parent().find('input[name="checkItem"]').not("input:checked").each(function (i, e) {
                if ($(tag).val() == $(e).val()) {
                    var a = $(this).find('.price').html();

                    t += parseFloat(a);
                }
            });
        }
        else {
            $(tag).parent().parent().parent().find('input[name="checkItem"]:checked').each(function (i, e) {
                if ($(tag).val() == $(e).val()) {
                    var a = $(this).find('.price').html();

                    t += parseFloat(a);
                }
            });
        }
        return t.toFixed(2);
    }
    return t.toFixed(2);
}

function getCheckProductPrice() {
    var t = 0;
    $.each($('input[name="checkItem"]:checked'), function () {
        var a = $(this).parent().parent().find('.price').html();

        t += parseFloat(a);
    })
    return t.toFixed(2);
}


function getCheckProductCount() {
    //var t = 0;
    //$.each($('input[name="checkShop"]:checked'), function () {
    //    var a = 1;
    //    t += a;
    //})
    //return t;

    var t = 0;
    $.each($('input[name="checkItem"]:checked'), function () {
        var a = $(this).parent().parent().find('.num').html();

        t += parseFloat(a);
    })
    return t.toFixed(0);
}


