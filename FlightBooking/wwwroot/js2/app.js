

/**
 * Set up require.js configration information and
 * initialize paths and shims.
 */
(function () {
    requirejs.config({
        baseUrl: '/js2',
        paths: {
            model: 'model',
            lib: 'lib',
            view: 'view',
            ext: 'ext',

            // Vendor libraries.
            moment: 'moment',
            deepmodel: 'ext/deepmodel/deepmodel',
            dropzone: 'ext/dropzone/dropzone',
            pikaday: 'ext/pikaday/pikaday',
            timepicker: 'ext/timepicker/timepicker',
            isotope: 'ext/isotope/isotope',
            bloodhound: 'ext/bloodhound/bloodhound',
            typeahead: 'ext/typeahead/typeahead',
            leaflet: 'leaflet',
            leaflet_ajax: 'leaflet_ajax',
            leafletSleep: 'leafletSleep',
            easy_button: 'easy_button',
            // Require.js plugins
            text: 'text',

            // Just a short cut so we templates can be put outside js dir
            tmpl: 'tmpl',

            jquery: 'jquery',
            
            bootstrap: 'ext/bootstrap/bootstrap',
            multiselect: 'ext/bootstrap-multiselect/bootstrap-multiselect',
            ckeditor: 'ext/ckeditor/ckeditor',
            shim: {
                'pikaday': {
                    deps: ['moment']
                },
                'ext/typeahead/typeahead': {
                    deps: ['jquery'],
                    exports: 'typeahead'
                },
                'ext/bloodhound/bloodhound': {
                    deps: ['jquery'],
                    exports: 'Bloodhound'
                },
                'ext/jwplayer/jwplayer': {
                    exports: 'jwplayer'
                },
                'bootstrap': {
                    deps: ['jquery'],
                    exports: 'bootstrap'
                },
                'ext/bootstrap-multiselect/bootstrap-multiselect': {
                    deps: ['jquery']
                },
                'leaflet':{
                    deps: ['jquery'],
                    exports: 'leaflet'

                },
                'leaflet_ajax':{
                    deps: ['jquery', 'leaflet'],
                    exports: 'leaflet_ajax'
                },
                'leafletSleep': {
                    deps: ['jquery', 'leaflet'],
                    exports: 'leafletSleep'
                },
                'easy_button': {
                    deps: ['jquery', 'leaflet'],
                    exports: 'easy_button'
                }

            }
        },
        waitSeconds: 200
    });
}());

/** 
 * Set global variable for application if required.
 * This is where all application config is set. asd d asd asdsa das
 */
var App = App || {};
App.config = {
    APP_NAME: 'Guardian'
}
App.stats = {
    requestStartTime: null,
    requestEndTime: null,
    requestSeconds: 0,
    requestStart: function () {
        this.requestStartTime = new Date().getTime();
    },
    requestEnd: function () {
        this.requestEndTime = new Date().getTime();
        this.requestSeconds = (this.requestEndTime - this.requestStartTime) / 1000;
    }
}

define(['jquery', 'underscore', 'backbone', 'eventbus', 'loadpod', 'bootstrap', 'extensions', 'leaflet'],
	/**
	 * Set up the application.
	 * @exports app
	 */
	function ($, _, Backbone, eventbus, loadpod) {
	    $(document).ready(function () {
	        initialize();
	    });

	    /**
         * Initialize the application by determining whether the code
         * is running on a mobile device.
         */
	    function initialize() {

	        // Set up global AJAX settings for stats, logging etc.
	        $(document).on("ajaxStart", function (e, xhr, settings, exception) {
	            App.stats.requestStart();
	        });
	        $(document).on("ajaxComplete", function (e, xhr, settings, exception) {
	            App.stats.requestEnd();
	        });
	    };
	});