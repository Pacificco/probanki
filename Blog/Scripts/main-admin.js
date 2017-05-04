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
function set_tariff_sum() {
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
//Успешное закрытие прогноза
function forecast_closed_success(data) {
    var id = $('#forecast-closed-success-mes');
    if (id != null) {
        location.reload();
    }
};