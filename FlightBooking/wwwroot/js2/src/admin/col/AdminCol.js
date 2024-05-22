define(['jquery', 'underscore', 'backbone', 'src/admin/mod/AdminModel'],
    function ($, _, Backbone, model) {
        model: model,
            initialize: function (models, options) {
                if (typeof options == "object") {
                    this.attributes = options;                }
        },

        //url: function() {
        //    return ''
        //}

        //getPort: function(e) {
        //    this.url = '/Admin/GetAirports';
        //}
    }








);