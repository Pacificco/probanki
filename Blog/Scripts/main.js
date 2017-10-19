//Модальное окно Регистрация/Авторизация
$(document).ready(function () {
    //alert(location.host);
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

    $('#currencies-body').mouseenter(function () {
        $(this).css('cursor', 'pointer');
        //$(this).css('background-color', '#47608a');
    }).mouseleave(function () {
        //$(this).css('background', 'none');
    }).click(function () {
        window.location = 'currency';
    });
        
    //Биржа должников
    $('#debtors-list .read-more').click(function (e) {       
        e.preventDefault();        
        showDebtorDetails();
    })
});
//Успешная авторизация
function auth_success(data) {
    //Закрытие диалоговых окон
    $('.window .close').click(function (e) {
        e.preventDefault();
        $('#mask, .window').hide();
    });

    //var id = $('#auth-success-mes');
    if ($("#auth-success-mes").length){
    //if (id != null) {
        //alert("Успешная авторизация!");        
        var new_location = "/club/forecasts";
        window.location = new_location;
    }
};
//Успешное пополнение баланса пользователя
function add_balance_success(data) {
    var id = $('#add-balance-success-mes');
    if (id != null) {
        location.reload();
    }
};
//Расчет тарифа
jQuery(function ($) {
    $('#form-add-balance #TariffId').on('change', function () {
        set_tariff_sum();
    });
    $('#form-add-balance #PeriodId').on('change', function () {
        set_tariff_sum();
    });    
});
function set_tariff_sum()
{
    var tariff = $('#form-add-balance #TariffId').val();
    var period = $('#form-add-balance #PeriodId').val();
    
    if (tariff == 0) {
        $('#form-add-balance #Sum').val(0);
        return;
    }
    if (period == 0) {
        $('#form-add-balance #Sum').val(0);
        return;
    }

    var sum = 0;

    if (tariff == 1) {
        sum = 1199;
    }
    else if (tariff == 2) {
        sum = 999;
    }
    else if (tariff == 3) {
        sum = 699;
    }
    else if (tariff == 4) {
        sum = 499;
    }
    else {
        sum = 0;
    }

    if (period == 1) {
        sum = 1 * sum;
    }
    else if (period == 2) {
        sum = 3 * sum;
    }
    else if (period == 3) {
        sum = sum * 6;
    }
    else if (period == 4) {
        sum = sum * 12;
    }
    else {
        sum = 0;
    }

    $('#form-add-balance #Sum').val(sum);
};
//Успешно принятый прогноз
function add_user_to_forecast_success(data) {
    var id = $('#add-user-to-forecast-success');
    if (id != null) {
        location.reload();
    }
};
//Маски ввода для прогнозов
jQuery(function ($) {
    $.mask.definitions['~'] = '[+-]';
    $('.eur #form-add-user-to-forecast-ajax #Value').mask("99,9999");
    $('.usd  #form-add-user-to-forecast-ajax #Value').mask("99.9999");
    $('.oil  #form-add-user-to-forecast-ajax #Value').mask("999.9999");
    $('.sberbank  #form-add-user-to-forecast-ajax #Value').mask("999.9999");

    $('#form-debtor #ContactPhone').mask("+7(999) 99-99-999");
    $('#form-debtor #DopPhone').mask("+7(999) 99-99-999");
    //$('#form-debtor #DebtAmount').maskMoney();
});
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

//Биржа должников
function showDebtorDetails() {
    var debtor_id = $('#debtors-list .read-more').attr('data-debtor-id');    
    var d = $('#debtors-list .debtor-details-' + debtor_id);
    if (d != null) {
        if (d.css("display") == "none")  // Показываем подробности
        {
            d.css("display", "table-row");
            $('#debtors-list .read-more').text('Скрыть');
            $('#debtors-list .debtor-list-item-' + debtor_id).css('border-left', '8px solid #00aded');
        }
        else  // Скрываем подробности
        {
            d.css("display", "none");
            $('#debtors-list .read-more').text('Подробнее');
            $('#debtors-list .debtor-list-item-' + debtor_id).css('border-left', 'none');
        }
    }
};