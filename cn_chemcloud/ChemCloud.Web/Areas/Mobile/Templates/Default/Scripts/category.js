﻿var cate;
$(function () {
    //滚动
    $(".category1,.category2").niceScroll({ cursorwidth: 0,cursorborder:0 });

    //图片延迟加载
 //   $(".lazyload").scrollLoading({ container: $(".category2") });

    $('.category-box').height($(window).height());

    //点击切换2 3级分类
	var array=new Array();
	$('.category1 li').each(function(){ 
		array.push($(this).position().top-56);
	});
	
	$('.category1 li').click(function() {
		var index=$(this).index();
		$('.category1').delay(200).animate({scrollTop:array[index]},300);
		$(this).addClass('cur').siblings().removeClass();
		$('.category2 dl').eq(index).show().siblings().hide();
	});

});



