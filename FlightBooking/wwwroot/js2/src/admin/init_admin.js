define(['jquery', 'underscore', 'backbone', 'src/admin/view/admin'],

    function ($, _, backbone, adminview) {
        return function (adminel) {
            var adminView = new adminview();
            adminView.render();

        }
    });