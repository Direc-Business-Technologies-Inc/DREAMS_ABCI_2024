﻿window.setTimeout(function () {
    $(".alert").fadeTo(1000, 0).slideUp(1000, function () {
        $(this).remove();
    });
}, 4000);


$(".validuntil").inputmask({ "mask": "99/99" });