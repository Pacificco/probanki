$(document).ready(function () {

    var options = {
        dataType: "json",
        beforeSubmit: function (data) {
            $('#preview').attr('src', '/Content/system/spinning.gif');
        },
        success: function (data) {
            $('#preview').attr('src', '/Image/ById/' + data.Id);
        }
    };

    // Грузим асинхронно
    $('#upload').ajaxForm(options);

    // Выбор файла при клике на изображении    
    $('#preview').click(function () {
        $('#file').click();
    });

    // Auto popup file select dialog
    //$('#file').click();
});