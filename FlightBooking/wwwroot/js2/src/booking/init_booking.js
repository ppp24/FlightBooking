define(['jquery', 'underscore', 'backbone', 'src/booking/view/booking'],

    function ($, _, backbone, bookingview) {
        return function (bookingel) {
            var bookingView = new bookingview();
            bookingView.render();

        }
    });
