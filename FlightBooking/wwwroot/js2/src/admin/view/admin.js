define(['jquery', 'underscore', 'backbone',

    //templates
    'text!src/admin/temp/addAirport.html',
    'text!src/admin/temp/airporttbl.html',
    'text!src/admin/temp/airportrow.html',
    'text!src/admin/temp/admin.html',
    'text!src/admin/temp/addFlight.html',
    'text!src/admin/temp/flightrow.html',

    //plug-ins
    //https://unpkg.com/sweetalert/dist/sweetalert.min.js',
    'ext/plugin2/jquery.dataTables.min',
    'ext/plugin2/dataTables.bootstrap4.min',
    'https://code.jquery.com/jquery-1.12.4.js',
    'https://code.jquery.com/ui/1.12.1/jquery-ui.js',
    'https://cdnjs.cloudflare.com/ajax/libs/sweetalert/2.1.0/sweetalert.min.js',
    'ext/validate/jquery.validate',



],

    function ($, _, Backbone, adminview) {
        return Backbone.View.extend({

            el: '#admin_container',

            templates: {

                addform: require('text!src/admin/temp/addAirport.html'),
                airporttbl: require('text!src/admin/temp/airporttbl.html'),
                airportrow: require('text!src/admin/temp/airportrow.html'),
                admin: require('text!src/admin/temp/admin.html'),
                flightForm: require('text!src/admin/temp/addFlight.html'),
                flightrow: require('text!src/admin/temp/flightrow.html'),
            },
            events: {
                'click #addbtn': 'addAirport',
                'click #btnAddAirport': 'saveAirport',
                'click #btn_DelAir': 'delAirport',
                'click #addflight': 'flightaddform',
                'click #btnAddFlight': 'saveFlight',
                'click #flights-tab': 'flightstab',
                'click #btn_EditFlight': 'editFlights',
            },


            render: function () {
                var view = this;
                view.$el.html("");
                view.$el.append(_.template(this.templates.airporttbl));
                view.airporttableview();

            },
            //airports table
            airporttableview: function (e) {
                var view = this;
                view.$el.find("#airportbody").html('');
                $.ajax({
                    url: '/Admin/GetAirports',
                    type: 'GET',
                    success: function (data) {
                       // console.log("Successfully fetched data:", data);
                        // Assume data is an array of airport objects
                        if (data && data.length > 0) {
                            _.each(data, function (airport) {
                                var row = _.template(view.templates.airportrow, airport);
                                view.$el.find("#airportbody").append(row);
                            });
                            // Optionally, you can re-initialize any JavaScript components that interact with the table
                            view.renderDataTable("#airporttbl", true, 0, "asc");
                        } else {
                            view.$el.find('#airportbody').html('<tr><td colspan="3">No airports found.</td></tr>');
                        }
                    },
                    error: function (xhr, status, error) {
                        console.error("Error fetching data: ", xhr.responseText);
                        view.$el.find('#airportbody').html('<tr><td colspan="3">Failed to load data.</td></tr>');
                        // Show user-friendly error message, optionally you can use Bootstrap modals or alerts
                        alert("Failed to load airports. Please try again.");
                    }
                });
            },

            //add new airport button function
            addAirport: function (e) {
                var view = this;
                $("#name").val("");
                $("#location").val("");
                $('#addmodal').remove();
                view.$el.append(_.template(view.templates.addform)),
                    $('#addmodal').modal('show');
                view.validateform("#addformAirport");


                $('#addmodal').on('hidden.bs.modal', function (e) {
                    $('#addmodal').remove();
                });

            },
            //save new airport 
            saveAirport: function (e) {
                var view = this;
                if ($('#addformAirport').valid()) {
                    var name = $("#name").val();
                    var location = $("#location").val();

                    var addAir = {
                        name: name,
                        location: location,
                    };

                    $.post("/AddAirport", addAir).done(function (res) {
                        if (res == false) {
                            swal("Airport with similar name already exists");
                        } else {
                            $('#addmodal').modal('hide');  // Hide modal after successful addition
                            swal("Airport added successfully!", { icon: "success" });

                            // Reload the data (refresh the page or just the table content)
                            window.location.reload();  
                            // view.airporttableview(); // Refresh just the table if full reload isn't desired
                        }
                    }).fail(function (jqXHR, textStatus, errorThrown) {
                        swal("Failed to add airport: " + textStatus);
                    });
                }
            },

            // form validation
            validateform: function (formName) {
                $(formName).validate({
                    rules: {
                        name: "required",
                        location: "required",
                    },
                    messages: {
                        name: { required: " Please enter name" },
                        location: { required: " Please enter location" },

                    },
                    errorClass: 'text-danger',
                    errorPlacement: function (error, element) {
                        error.insertBefore(element.parent());
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


            //delete records
            delAirport: function (e) {
                var view = this;
                var id = $(e.currentTarget).data('id');

                // Confirm deletion with the user
                if (confirm("Are you sure you want to delete this record?")) {
                    $.ajax({
                        url: "/DeleteAirport",
                        type: "POST",
                        data: { id: id },
                        success: function (res) {
                            if (res == true) {
                                // Assuming 'res' includes a 'success' flag and the new data array
                                alert(" Successfully deleted");  
                                window.location.reload();
                            } else {
                                alert("Failed to delete record: " + res.message);
                            }
                        },
                        error: function (xhr, status, error) {
                            alert("Error deleting record: " + xhr.responseText);
                        }
                    });
                }
            },

            // General form validation function
            validateForm1: function (formSelector, rules, messages) {
                $(formSelector).validate({
                    rules: rules,
                    messages: messages,
                    errorClass: 'text-danger',
                    errorPlacement: function (error, element) {
                        error.insertBefore(element.parent());
                    }
                });
            },

            // Add flight button functionality
            flightaddform: function (e) {
                var view = this;
                $("#fligname").val("");
                $("#flignum").val("");
                $("#economyPrice").val("");
                $("#businessPrice").val("");
                $("#arrivalTime").val("");
                $("#departureTime").val("");
                $("#status").val("");

                $('#addFlimodal').remove();

                view.$el.append(_.template(view.templates.flightForm));
                $('#addFlimodal').modal('show');
                $.ajax({
                    url: '/DropdownAirport',
                    type: 'GET',
                    dataType: 'json',
                    success: function (airports) {
                        var departureDropdown = $('#departureAirport');
                        var arrivalDropdown = $('#arrivalAirport');
                        departureDropdown.empty();
                        arrivalDropdown.empty();
                        departureDropdown.append('<option value="">Select Departure Airport</option>');
                        arrivalDropdown.append('<option value="">Select Arrival Airport</option>');
                        airports.forEach(function (airport) {
                            departureDropdown.append($('<option></option>').attr('value', airport.value).text(airport.text));
                            arrivalDropdown.append($('<option></option>').attr('value', airport.value).text(airport.text));
                        });
                    },
                    error: function () {
                        alert('Error loading airports.');
                    }
                });
                $('#addFlimodal').on('hidden.bs.modal', function (e) {
                    $('#addFlimodal').remove();
                });
                this.validateForm1("#addformFlight", {
                    fligname: "required",
                    flignum: "required",
                    departureAirport: "required",
                    arrivalAirport: "required",
                    economyPrice: "required",
                    businessPrice: "required",
                    departureTime: "required",
                    arrivalTime: "required",
                    status: "required",
                }, {
                    fligname: { required: "Please enter the flight name" },
                    flignum: { required: "Please enter the flight number" },
                    departureAirport: { required: "Please select a departure airport" },
                    arrivalAirport: { required: "Please select an arrival airport" },
                    economyPrice: { required: "Please enter price" },
                    businessPrice: { required: "Please enter price" },
                    departureTime: {require: "Please add departure time"},
                    arrivalTime: { require: "Please add arrivalTime time"},
                    status: { require: "Please select flight status"},

                });
            },


            //save flight 
            saveFlight: function (e) {
                var view = this;
                if ($('#addformFlight').valid()) {
                    var fligname = $("#fligname").val();
                    var flignum = $("#flignum").val();
                    var economyPrice = $("#economyPrice").val();
                    var businessPrice = $("#businessPrice").val();
                    var departureAirport = $("#departureAirport").val();
                    var arrivalAirport = $("#arrivalAirport").val();
                    var departureTime = $("#departureTime").val();
                    var arrivalTime = $("#arrivalTime").val();
                    var status = $("#status").val();

                    var addFlight = {
                        FlightName: fligname,
                        FlightNumber: flignum,
                        EconomyPrice: parseFloat(economyPrice),
                        BusinessPrice: parseFloat(businessPrice),
                        DepartureAirportId: parseInt(departureAirport),
                        ArrivalAirportId: parseInt(arrivalAirport),
                        DepartureTime: departureTime,
                        ArrivalTime: arrivalTime,
                        Status: status,
                    };


                    $.ajax({
                        url: '/AddFlight',
                        type: 'POST',
                        contentType: 'application/json',
                        data: JSON.stringify(addFlight), 
                        success: function (res) {
                            if (!res) {
                                swal("Flight with similar name already exists.");
                            } else {
                                swal("Flight added successfully.", { icon: "success" }).then(function () {
                                    $('#addFlimodal').modal('hide');
                                    view.renderDataTable("#flight_tbl", true, 0, "asc"); // Assume view context is handled
                                });
                                view.renderDataTable("#flight_tbl", true, 0, "asc");
                                view.flightstab();
                                
                            }
                        },
                        error: function (xhr, status, error) {
                            console.error("Error adding flight:", error);
                        }
                    });

                }
            },


            //flights table
            flightstab: function (e) {
                var view = this;
                view.$el.find("#flightbody").html('');
                $.ajax({
                    url: '/Admin/GetFlights',
                    type: 'GET',
                    success: function (data) {
                       // console.log("Successfully fetched data:", data);
                        if (data && data.length > 0) {
                            _.each(data, function (flight) {
                                var row = _.template(view.templates.flightrow, flight);
                                view.$el.find("#flightbody").append(row);
                            });
                            view.renderDataTable("#flight_tbl", true, 0, "asc");
                        } else {
                            view.$el.find('#flightbody').html('<tr><td colspan="3">No data found.</td></tr>');
                        }
                    },
                    error: function (xhr, status, error) {
                       // console.error("Error fetching data: ", xhr.responseText);
                        view.$el.find('#flightbody').html('<tr><td colspan="3">Failed to load data.</td></tr>');
                        alert("Failed to load. Please try again.");
                    }
                });
            },

            //edit flights
           
            editFlights: function (e) {
                var view = this;
                var id = $(e.currentTarget).data("id"); // Retrieve the flight ID from data-id attribute
                $.ajax({
                    url: '/Admin/GetFlightId/' + id,
                    type: 'GET',
                    dataType: 'json',
                    success: function (flight) {
                        view.$el.append(_.template(view.templates.flightForm, { id: id }));
                        $("#flightName").val(flight.flightName);
                        $("#flightNumber").val(flight.flightNumber);
                        $("#economyPrice").val(flight.economyPrice);
                        $("#businessPrice").val(flight.businessPrice);
                        $("#departureAirport").val(flight.departureAirport);
                        $("#arrivalAirport").val(flight.arrivalAirport);
                        $("#departureTime").val(flight.departureTime);
                        $("#arrivalTime").val(flight.arrivalTime);
                        $("#status").val(flight.status);

                        //$('#addFlimodal').remove();

                        $("#addFlimodal .modal-title").text("Edit Flight");
                        $('#addFlimodal').modal('show');
                    },
                    error: function () {
                        alert('Error loading flight details.');
                    }
                });
            },


            // Function to handle saving edited flight
            saveEditedFlight: function (e) {
                var view = this;
                if ($('#addformFlight').valid()) {
                    var fligname = $("#fligname").val();
                    var flignum = $("#flignum").val();
                    var economyPrice = $("#economyPrice").val();
                    var businessPrice = $("#businessPrice").val();
                    var departureAirport = $("#departureAirport").val();
                    var arrivalAirport = $("#arrivalAirport").val();
                    var departureTime = $("#departureTime").val();
                    var arrivalTime = $("#arrivalTime").val();
                    var status = $("#status").val();

                    var editedFlight = {
                        FlightName: fligname,
                        FlightNumber: flignum,
                        EconomyPrice: parseFloat(economyPrice),
                        BusinessPrice: parseFloat(businessPrice),
                        DepartureAirportId: parseInt(departureAirport),
                        ArrivalAirportId: parseInt(arrivalAirport),
                        DepartureTime: departureTime,
                        ArrivalTime: arrivalTime,
                        Status: status,
                    };

                   
                    $.ajax({
                        url: '/UpdateFlight/' + flightId, 
                        type: 'POST',
                        contentType: 'application/json',
                        data: JSON.stringify(editedFlight),
                        success: function (res) {
                            if (!res) {
                                swal("Flight with similar name already exists.");
                            } else {
                                swal("Flight updated successfully.", { icon: "success" }).then(function () {
                                    $('#addFlimodal').modal('hide');
                                    view.renderDataTable("#flight_tbl", true, 0, "asc"); 
                                });
                                view.renderDataTable("#flight_tbl", true, 0, "asc");
                                view.flightstab();
                            }
                        },
                        error: function (xhr, status, error) {
                            console.error("Error updating flight:", error);
                        }
                    });
                }
            },


           







        })
    })


        
  



