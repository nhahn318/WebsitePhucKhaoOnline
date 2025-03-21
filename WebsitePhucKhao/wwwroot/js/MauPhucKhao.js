jQuery(document).foundation();

(function ($) {
    $(function () {

        // Code here for active or inactive the error text.

        $('.numeric').keypress(function (event) {
            var charCode = (event.which) ? event.which : event.keyCode
            return !(charCode > 31 && (charCode < 48 || charCode > 57));
        });

        $(".allownumericwithoutdecimal").on("keypress keyup blur", function (event) {
            jQuery(this).val($(this).val().replace(/[^\d].+/, ""));
            if ((event.which < 48 || event.which > 57)) {
                event.preventDefault();
            }
        });

    });
})(jQuery);

