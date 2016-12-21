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