define(['jquery', 'underscore', 'backbone', 'src/home/view/home'],

    function ($, _, backbone, homeview) {
        return function (homeel) {
            var homeView = new homeview();
            homeView.render();

        }
    });
