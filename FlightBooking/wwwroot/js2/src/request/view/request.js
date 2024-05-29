define(['jquery', 'underscore', 'backbone',

    //templates
    'text!src/request/temp/allrequests.html',
    'text!src/request/temp/pendingrow.html',
    'text!src/request/temp/pendingmodal.html',
    'text!src/request/temp/actionedrow.html',
    'text!src/request/temp/paymentrequest.html',


    //plug-ins
    //https://unpkg.com/sweetalert/dist/sweetalert.min.js',
    'ext/plugin2/jquery.dataTables.min',
    'ext/plugin2/dataTables.bootstrap4.min',
    'https://code.jquery.com/jquery-1.12.4.js',
    'https://code.jquery.com/ui/1.12.1/jquery-ui.js',
    'https://cdnjs.cloudflare.com/ajax/libs/sweetalert/2.1.0/sweetalert.min.js',
    'ext/validate/jquery.validate',



],

    function ($, _, Backbone, requestview) {
        const actionNames = {
            "1": "Change To Value",
            "2": "Change To Lite",
            "4": "Apply For Loyalty",
            "5": "Upload Doc"
        };

        return Backbone.View.extend({

            el: '#request_container',

            templates: {

                allrequest: require('text!src/request/temp/allrequests.html'),
                pendingrow: require('text!src/request/temp/pendingrow.html'),
                pendingmodal: require('text!src/request/temp/pendingmodal.html'),
                actionedrow: require('text!src/request/temp/actionedrow.html'),
                paymentrequest: require('text!src/request/temp/paymentrequest.html'),

            },
            events: {
                // 'click #addbtn': 'addAirport',
                'click #btn_action': 'ViewRequest',
                'click #saveChanges': 'SaveRequestAdmin',
                'click #actioned-tab': 'actionedTab',
                'click #makePaymentBtn': 'requestPayment',
            },


            render: function () {
                var view = this;
                view.$el.html("");
                view.$el.append(_.template(this.templates.allrequest));
                view.requesttblview();

            },

            requesttblview: function () {
                var view = this;
                view.$el.find("#pendingbody").html('');
                $.ajax({
                    url: '/Requests/GetPendingRequests',
                    type: 'GET',
                    success: function (data) {
                        if (data && data.length > 0) {
                            _.each(data, function (response) {
                                // Map the action value to the action name
                                response.action = actionNames[response.action] || response.action; // Default to value if no match found
                                var row = _.template(view.templates.pendingrow, response);
                                view.$el.find("#pendingbody").append(row);
                            });
                            view.renderDataTable("#pendingtbl", true, 0, "asc");
                        } else {
                            view.$el.find('#pendingbody').html('<tr><td colspan="3">No pending requests found.</td></tr>');
                        }
                    },
                    error: function (xhr, status, error) {
                        console.error("Error fetching data: ", xhr.responseText);
                        view.$el.find('#pendingbody').html('<tr><td colspan="3">Failed to load data.</td></tr>');
                        // Show user-friendly error message, optionally you can use Bootstrap modals or alerts
                        alert("Failed to load requests. Please try again.");
                    }
                });

            },

            ////request status pending payment
            //requestPayment: function (e) {
            //    var view = this;
            //    view.$el.html("");
            //    view.$el.append(_.template(this.templates.paymentrequest));
            //},



            ViewRequest: function (e) {
                var view = this;
                var id = $(e.currentTarget).data('id');

                // Make an AJAX request to fetch the request details by ID
                $.ajax({
                    url: '/Requests/GetRequestById/' + id,
                    type: 'GET',
                    dataType: 'json',
                    success: function (data) {
                        // Populate the modal with the fetched data
                        var modalTemplate = _.template(view.templates.pendingmodal, { id: id });
                        var $modal = $(modalTemplate);
                        $modal.find("#confirmationNumber").text("Confirmation Number: " + data.confirmationNumber);
                        var actionDisplayName = actionNames[data.action] || data.action;
                        $modal.find("#action").text("Action: " + actionDisplayName);
                        $modal.find("#uploadDocuments").text("Upload Documents: " + data.uploadDocuments);
                        $modal.find("#requestDate").text("Request Date: " + data.requestDate);
                        $modal.find("#comment").text("Customer Comment: " + data.comment);

                        $('body').append($modal);
                        $modal.modal('show');

                        // Listen for save action event
                        $modal.find('#saveChanges').click(function () {

                            //  var id = data.id;
                            var comment = data.comment
                            var confirmationNumber = data.confirmationNumber;
                            var action = data.action;

                            // Call SaveRequestAdmin function to save the selected action and comment
                            view.SaveRequestAdmin(action, comment, confirmationNumber);

                            // Close the modal after saving changes
                            $modal.modal('hide');
                        });
                    },
                    error: function () {
                        console.error('Failed to fetch request details.');
                    }
                });

                // Remove the modal when it's hidden
                $('#viewpending').on('hidden.bs.modal', function () {
                    $('#viewpending').remove();
                });
            },

            SaveRequestAdmin: function (action, comment, confirmationNumber) {
                var view = this;

                // Make an AJAX request to save the selected action and comment
                $.ajax({
                    url: '/Requests/SaveRequestAdmin',
                    type: 'POST',
                    contentType: 'application/json', // Set content type explicitly
                    dataType: 'json',
                    data: JSON.stringify({
                        action: action,
                        comment: comment,
                        confirmationNumber: confirmationNumber
                    }),
                    success: function (response) {
                        if (!response) {
                            swal("Error approving request");
                        } else {
                            swal("Request actioned", { icon: "success" }).then(function () {
                                $('#viewpending').modal('hide');
                                view.renderDataTable("#pendingtbl", true, 0, "asc"); // Assume view context is handled
                            });
                            view.renderDataTable("#pendingtbl", true, 0, "asc");
                            view.requesttblview();

                        }
                    },
                    error: function () {
                        console.error('Error performing action. Please try later');
                    }
                });
            },


            actionedTab: function () {
                var view = this;
                view.$el.find("#actionedbody").html('');
                $.ajax({
                    url: '/Requests/GetActionedRequests',
                    type: 'GET',
                    success: function (data) {
                        if (data && data.length > 0) {
                            _.each(data, function (response) {
                                // Map the action value to the action name
                                response.action = actionNames[response.action] || response.action; // Default to value if no match found
                                var row = _.template(view.templates.actionedrow, response);
                                view.$el.find("#actionedbody").append(row);
                            });
                            view.renderDataTable("#actionedtbl", true, 0, "asc");
                        } else {
                            view.$el.find('#actionedbody').html('<tr><td colspan="3">No pending requests found.</td></tr>');
                        }
                    },
                    error: function (xhr, status, error) {
                        // console.error("Error fetching data: ", xhr.responseText);
                        view.$el.find('#actionedbody').html('<tr><td colspan="3">Failed to load data.</td></tr>');
                        alert("Failed to load. Please try again.");
                    }
                });
            },





            //datatable 
            renderDataTable: function (tablex, paginate, sort_col, sort_order) {
                if (sort_order === undefined) {
                    sort_order = "asc";
                }
                $(tablex).DataTable({
                    "pagingType": "full_numbers",
                    "destroy": true,
                    "aaSorting": [[sort_col, sort_order]],
                    'pageLength': 5,
                    "lengthMenu": [
                        [10, 25, 50, -1],
                        [10, 25, 50, "All"]
                    ],
                    responsive: true,
                    language: {
                        search: "_INPUT_",
                        searchPlaceholder: "Search records",
                    }
                });
            },


        })
    })







