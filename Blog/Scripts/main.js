//Модальное окно Регистрация/Авторизация
$(document).ready(function () {
    $('a[name=modal]').click(function (e) {
        e.preventDefault();
        var id = $(this).attr('href');
        //var maskHeight = $(document).height();
        //var maskWidth = $(window).width();
        //$('#mask').css({ 'width': maskWidth, 'height': maskHeight });
        //$('#mask').fadeIn(1000);
        //$('#mask').fadeTo("slow", 0.8);
        //var winH = $(window).height();
        var winW = $(window).width();
        $(id).css('top', 90);
        $(id).css('left', winW / 2 - $(id).width() / 2 + 425);
        $(id).fadeIn(1000);
    });
    //Закрытие диалоговых окон
    $('.window .close').click(function (e) {
        e.preventDefault();
        $('#mask, .window').hide();
    });
    
    //$('#mask').click(function () {
    //    $(this).hide();
    //    $('.window').hide();
    //});    
});
//Успешная авторизация
function auth_success(data) {    
    //Закрытие диалоговых окон
    $('.window .close').click(function (e) {
        e.preventDefault();
        $('#mask, .window').hide();
    });

    var id = $('#auth-success-mes');
    if (id != null) {
        location.reload();
    }

    //alert('OK!');
};
//Успешное пополнение баланса пользователя
function add_balance_success(data) {
    var id = $('#add-balance-success-mes');
    if (id != null) {
        location.reload();
    }

    //alert('OK!');
};
//Фиксирование главного меню в верху страницы
jQuery(function ($) {
    $(window).scroll(function () {
        if ($(this).scrollTop() > 140) {
            $('#top-menu-wrapper').addClass('fixed');
        }
        else if ($(this).scrollTop() < 140) {
            $('#top-menu-wrapper').removeClass('fixed');
        }
    });
});