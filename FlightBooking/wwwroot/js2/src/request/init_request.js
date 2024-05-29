define(['jquery', 'underscore', 'backbone', 'src/request/view/request'],

    function ($, _, backbone, requestview) {
        return function (requestel) {
            var requestView = new requestview();
            requestView.render();

        }
    });
