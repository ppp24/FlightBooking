define(['underscore', 'backbone'],
        function (_, Backbone) {
            /*
             * Checks if the provided parameter is null or empty string.
             *
             * @param {string} param The object to compare against null and ""
             */
            Backbone.Model.prototype.nullOrEmpty = function (param) {
                var nullOrEmpty = false;
                if (param == null) {
                    nullOrEmpty = true;
                } else {
                    if (param == "") {
                        nullOrEmpty = true;
                    };
                };

                return nullOrEmpty;
            };

            /*
             * Logs model validation errors to the console
             * or to a specified element. Can accept validation
             * errors with messages or accept a message as a
             * parameter.
             */
            Backbone.Model.prototype.logValidationErrors = function (view, el, message) {

                // Clear all error classes.
                view.$el.find('input').removeClass('has-error');
                view.$el.find('textarea').removeClass('has-error');
                view.$el.find('select').removeClass('has-error');
                view.$el.find('div').removeClass('has-error');

                // If an element is passed in, don't write to the console.
                var useConsole = false,
                    useMessage = false;
                if (typeof el != 'undefined') { useConsole = false; };
                if (typeof message != 'undefined') { useMessage = true; };

                // Get the model validation errors.
                var err = this.validationError;
                if (err.length > 0) {
                    var elsAffected = "",
                        messages = "";

                    // Loop through all of the validation errors.
                    for (var i = 0; i < err.length; i++) {

                        // Check if there is an element id provided
                        // for the validation error.
                        if (err[i].el.length > 0) {

                            // If there are several elements affected,
                            // loop through them to construct the elements affected.
                            if (err[i].el instanceof Array) {
                                for (var x = 0; x < err[i].el.length; x++) {
                                    elsAffected += err[i].el[x] + ",";
                                };
                            } else {

                                // Otherwise just put the only element into the string.
                                elsAffected += err[i].el + ",";
                            };
                        };

                        // Add to the list of messages if !useMessage.
                        if (!useMessage) {
                            messages += err[i].msg + "<br /> ";
                        };
                    };

                    // Get rid of the trailing comma for elements affected.
                    elsAffected = elsAffected.slice(0, -1);

                    // Add spaces to the elements affected commas.
                    elsAffected = elsAffected.replace(/,/g, ', ');

                    // Log to the console if no element, otherwise render
                    // the errors to the page.
                    if (useConsole) {
                        console.log("Elements affected: " + elsAffected + ". Messages: " + message);
                    } else {
                        // Otherwise loop through all the elements, highlight them,
                        // then append the error messages to the page.
                        _.each(err, function (e, i, l) {

                            if (e.el instanceof Array) {
                                _.each(e.el, function (element) {
                                    view.$el.find(element).parent().addClass('has-error');
                                });
                            } else {
                                view.$el.find(e.el).parent().addClass('has-error');
                            };

                        });

                        // Render the messages on the page.
                        message = useMessage ? message : messages.slice(0, -2);
                        view.$el.find(el).show().html(message);
                    };
                } else {
                    // Hide the element if no errors are present.
                    if (!useConsole) {
                        view.$el.find(el).hide();
                    }
                };
            };

            /*
             * Checks to see if an email address conforms to requirements.
             */
            Backbone.Model.prototype.validateEmail = function (email) {
                var re = /[-0-9a-zA-Z.+_]+@[-0-9a-zA-Z.+_]+\.[a-zA-Z]{2,4}/;
                return re.test(email);
            }

            /*
             * Uses a seed from the pod to set all attribute values
             * of the backbone model.
             *
             * @param {object} seed The seed object pulled from the pod with model attributes.
             * @param {array} ignore An array of properties to ignore.
             */
            Backbone.Model.prototype.mergeSeed = function (seed, ignore) {
                for (var prop in seed) {
                    if (ignore != undefined) {
                        if (!_.contains(ignore, prop)) {
                            if (seed.hasOwnProperty(prop)) {
                                this.set(prop, seed[prop]);
                            };
                        };
                    } else {
                        if (seed.hasOwnProperty(prop)) {
                            this.set(prop, seed[prop]);
                        };
                    };
                };
            };

            Backbone.Collection.prototype.save = function (options) {
                return Backbone.sync("create", this, options);
            };
            Backbone.Collection.prototype.update = function (options) {
                return Backbone.sync("update", this, options);
            };
            //using the default attributes of a model, by filtering a response through this function
            //during parse phase, this will strip all unwanted values returned in the response that aren't
            //necessarily always wanted in the backbone view
            //cust is an array of custom rules that will overwrite any default parsing of attributes found in resp,
            //or specific attributes that are not in response.
            //cust object accepts the following properties
            //n: navigation to the object property i.e life.student.fullName.
            //v: value of the backbone property the navigation value will be set to.
            //t: type of custom set, the following are the only values currently accepted:
            //  u: converts the value to UpperCase.
            //  n: checks if value is NaN, returning the opposite - false if NaN, true if number.
            //  i: returns null if the value is 0.
            Backbone.Model.prototype.parseDefaults = function (resp, cust) {
                if (resp === null) return this.defaults;
                r = {};
                _.each(this.defaults, function (v, n) {
                    r[n] = resp[n]
                });
                //parse defaults for object properties aswell ^
                if (cust == undefined) return r;
                if (cust.length == 0) return r;
                for (i = 0; i < cust.length; i++) {
                    t = undefined;
                    tv = cust[i].n.split('.');
                    for (ii = 0; ii < cust[i].n.split('.').length; ii++)
                        if (t == undefined) t = resp[tv[ii]];
                        else t = t[tv[ii]];
                    if (cust[i].t == undefined) r[cust[i].v] = t;
                    if (cust[i].t == 'u') r[cust[i].v] = t.toUpperCase();
                    if (cust[i].t == 'n') r[cust[i].v] = isNaN(t) ? false : true;
                    if (cust[i].t == 'i') r[cust[i].v] = t == 0 ? null : t;
                }
                return r;
            };

            //accepts an array of strings, after calling the el.remove and stopListening events
            //as done in the default view.remove implementation, this method will also turn off
            //any App.events that have been bound within the view
            Backbone.View.prototype.destruct = function (events, removeElement) {

                // Only remove the element if specified.
                if (removeElement != null) {
                    if (removeElement) { this.$el.remove() };
                };

                // Undbind all of the view's events.
                this.stopListening();
                this.undelegateEvents();
                this.unbind();

                // Remove all of the specified app events.
                _.each(events, function (e) {
                    App.events.off(e);
                });
            };

            //sets the opacity css of the view's el to 0.3 and disables all input elements.
            Backbone.View.prototype.initViewMode = function () {
                this.undelegateEvents();
                this.$el.css('opacity', 0.3);
                this.$('input').prop('disabled', true);
                this.$('seleect').prop('disabled', true);
                this.$('button').prop('disabled', true);
            },

            //function merges the told attributes from model parameter to the backbone model called on.
            //attrs is an array of strings containing the attributes of model that are to be merged.
            Backbone.Model.prototype.mergeFromObject = function (model, attrs) {
                var curr = this;
                _.each(attrs, function (name) {
                    curr.set(name, model[name]);
                });
                return this;
            },

            Backbone.Collection.prototype.generateCid = function () {
                this.cid = _.uniqueId('m');
            }

            //using the default attributes of the model, this function unset's all attributes
            //that are not found, with a silent hash that will not trigger a change event.
            Backbone.Model.prototype.unsetNonDefaults = function () {
                _.each(this.attributes, function (v, i) {
                    if (typeof this.defaults[i] == "undefined") this.unset(i, { silent: true });
                }, this);
            }

        });