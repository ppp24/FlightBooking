define(['jquery', 'underscore', 'backbone'],
        function ($, _, Backbone) {
            if (App.events == undefined) App.events = _.extend({}, Backbone.Events);
        });