define(['jquery', 'underscore', 'backbone',

    //templates
    'text!src/home/temp/Index.html',
    'text!src/home/temp/searchFlights.html',
    'text!src/home/temp/searchDetail.html',
    'text!src/home/temp/economyDetails.html',
    'text!src/home/temp/returnFlight.html',
    'text!src/home/temp/booking.html',
    'text!src/home/temp/returnOption.html',
    'text!src/home/temp/payment.html',
    'text!src/home/temp/passengerDetails.html',
    'text!src/home/temp/itineraryPage.html',
    'text!src/home/temp/searchBookings.html',
    'text!src/home/temp/requestform.html',
    'text!src/home/temp/managereq.html',
    'text!src/home/temp/paymentrequest.html',


    //plug-ins
    //https://unpkg.com/sweetalert/dist/sweetalert.min.js',
    'ext/plugin2/jquery.dataTables.min',
    'ext/plugin2/dataTables.bootstrap4.min',
    'https://code.jquery.com/jquery-1.12.4.js',
    'https://code.jquery.com/ui/1.12.1/jquery-ui.js',
    'https://cdnjs.cloudflare.com/ajax/libs/sweetalert/2.1.0/sweetalert.min.js',
    'ext/validate/jquery.validate',



],

    function ($, _, Backbone, homeview) {

        var flightData;
        var ecoDetail;
        var selectedFlightOption = {};
        var selectedReturnOption = {};
        var returnflightData;
        return Backbone.View.extend({
            el: '#homeDiv',

            templates: {

                homepage: require('text!src/home/temp/Index.html'),
                searchFlights: require('text!src/home/temp/searchFlights.html'),
                searchDetail: require('text!src/home/temp/searchDetail.html'),
                economyDetails: require('text!src/home/temp/economyDetails.html'),
                returnFlights: require('text!src/home/temp/returnFlight.html'),
                booking: require('text!src/home/temp/booking.html'),
                returnOption: require('text!src/home/temp/returnOption.html'),
                paymentPage: require('text!src/home/temp/payment.html'),
                passengerDetails: require('text!src/home/temp/passengerDetails.html'),
                /*bookingPage: require('text!src/home/temp/bookingPage.html'),*/
                itinerary: require('text!src/home/temp/itineraryPage.html'),
                searchBookings: require('text!src/home/temp/searchBookings.html'),
                requestform: require('text!src/home/temp/requestform.html'),
                managereq: require('text!src/home/temp/managereq.html'),
                paymentrequest: require('text!src/home/temp/paymentrequest.html'),
                
            },
            events: {
                /*  'change #fromAirport': 'fetchlocation',*/
                'click #showflights': 'showflights',
                'click #ecobtn': 'economyDetails',
                'click #nextecoBtn': 'returnflights',
                'click #bookingbtn': 'bookingPage',
                'click #returnecobtn': 'returnOption',
                'click #passengerDetails': 'isUserLoggedIn',
                /* 'click #confirmpass': 'savePassengerDetails',*/
                'click #confirmpass': 'handleFormSubmit',
                'click #proceedToPayment': 'proceedToPayment',
                'click #backBtn': 'backBtn',
                'click #makePayment': 'makePayment',
                'click #viewbooking': 'ViewBooking',
                'click #savebook': 'saveBookings',
                'click #manageBook': 'getsearchform',
                'click #searchbookin': 'searchbooking',
                'click #requestupgrade': 'requestupgradeform',
                'click #submitUpgradeRequest': 'submitrequest',
                'click #managerequest': 'managerequest',
                'click #makePaymentBtn': 'requestPayment',
                'click #confirmPayment': 'confirmPayment',

            },


            render: function () {
                var view = this;
                view.$el.html("");
                view.$el.append(_.template(this.templates.homepage));
                view.fetchlocation();

            },

            backBtn: function () {
                window.history.back()
            },

            getsearchform: function () {
                var view = this;
                view.$el.html("");
                view.$el.append(_.template(view.templates.searchBookings));

            },

            //request status pending payment
            requestPayment: function (e) {
                var view = this;
                view.$el.html("");
                view.$el.append(_.template(this.templates.paymentrequest));
                view.validateform('#paymentForm');
            },

            confirmPayment: function (event) {
                var view = this;
                event.preventDefault();
                if ($('#paymentForm').valid()) {
                    var paymentData = {
                        cardNumber: $('#cardNumber').val(),
                        cardName: $('#cardName').val(),
                        expiryMonth: $('#expiryMonth').val(),
                        expiryYear: $('#expiryYear').val(),
                        confirmationNumber: $('#confirmationNumber').val(),
                        newPaymentAmount: $('#newPaymentAmount').val(),
                        cvv: $('#cvv').val(),
                    };
                    $.ajax({
                        type: "POST",
                        url: "/Requests/CompletePaymentAndUpdateRecords",
                        data: JSON.stringify(paymentData), // Send requestData as JSON
                        contentType: "application/json; charset=utf-8",
                        success: function (response) {
                            if (response == true) {
                                Swal.fire({
                                    icon: 'success',
                                    title: 'Success!',
                                    text: 'Payment successful.'
                                });
                                window.location.reload();
                            } else {
                                Swal.fire({
                                    icon: 'error',
                                    title: 'Error!',
                                    text: 'Failed. Please try again later.'
                                });
                            }
                        },
                        error: function (xhr, status, error) {
                            Swal.fire({
                                icon: 'error',
                                title: 'Error!',
                                text: 'An error occurred while processing your request. Please try again later.'
                            });
                        }
                    });
                }
            },

            managerequest: function () {
                var view = this;
                view.$el.html("");
                view.$el.append(_.template(view.templates.managereq));
            },  
            submitrequest: function (event) {
                var view = this;
                event.preventDefault(); // Prevent default form submission

                // Create a JSON object with form data
                var requestData = {
                    ConfirmationNumber: $('#confirmationNumber').val(),
                    Action: $('#action').val(),
                    Comment: $('#comment').val(),
                    DocumentBase64: null
                };

                // Check if document upload is required
                if ($('#action').val() == '5') {
                    var file = $('#document')[0].files[0];
                    if (!file) {
                        // No file selected, show error message
                        Swal.fire({
                            icon: 'error',
                            title: 'Error!',
                            text: 'Please select a file to upload.'
                        });
                        return; // Exit the function
                    }

                    // Convert file to Base64
                    var reader = new FileReader();
                    reader.onload = function (e) {
                        requestData.DocumentBase64 = e.target.result.split(',')[1]; // Get the Base64 string
                        sendRequest(requestData); // Send the request after file is read
                    };
                    reader.readAsDataURL(file);
                } else {
                    view.sendRequest(requestData); // Send the request if no file upload
                }
            },

            sendRequest: function (requestData) {
                // Log requestData to console for debugging
                console.log('Request Data:', requestData);

                // Make the AJAX call to submit the JSON data
                $.ajax({
                    type: "POST",
                    url: "/Requests/SubmitRequest",
                    data: JSON.stringify(requestData), // Send requestData as JSON
                    contentType: "application/json; charset=utf-8",
                    success: function (response) {
                        if (response.success) {
                            Swal.fire({
                                icon: 'success',
                                title: 'Success!',
                                text: 'Your request has been submitted successfully.'
                            });
                            window.location.reload();
                        } else {
                            Swal.fire({
                                icon: 'error',
                                title: 'Error!',
                                text: 'Failed to submit your request. Please try again later.'
                            });
                        }
                    },
                    error: function (xhr, status, error) {
                        Swal.fire({
                            icon: 'error',
                            title: 'Error!',
                            text: 'An error occurred while processing your request. Please try again later.'
                        });
                    }
                });
            },







            searchbooking: function () {
                var view = this;
                $('#booking-form').on('submit', function (event) {
                    event.preventDefault();
                    const confirmationNumber = $('#confirmationNumber').val();
                    /* const lastName = $('#lastName').val();*/

                    $.ajax({
                        url: '/Home/GetBookingByReference',
                        type: 'GET',
                        data: {
                            confirmationNumber: confirmationNumber,
                            // lastName: lastName
                        },
                        success: function (data) {
                            if (data) {
                                //window.location.href = '/Home/BookingDetails/' + data.bookingId;
                                view.booking();
                            } else {
                                $('#error-message').text('No booking found with the provided details.');
                            }
                        },
                        error: function () {
                            $('#error-message').text('An error occurred while searching for the booking.');
                        }
                    });
                });
            },


            fetchlocation: function () {
                var view = this;
                var departureDropdown = $('#fromAirport');
                var arrivalDropdown = $('#toAirport');
                departureDropdown.empty().append('<option value="">Loading...</option>');
                arrivalDropdown.empty().append('<option value="">Loading...</option>');
                $.ajax({
                    url: '/DropdownAirport',
                    type: 'GET',
                    dataType: 'json',
                    success: function (airports) {
                        departureDropdown.empty();
                        arrivalDropdown.empty();
                        departureDropdown.append('<option value="">Select Departure Airport</option>');
                        arrivalDropdown.append('<option value="">Select Arrival Airport</option>');
                        airports.forEach(function (airport) {
                            departureDropdown.append($('<option></option>').attr('value', airport.value).text(airport.text));
                            arrivalDropdown.append($('<option></option>').attr('value', airport.value).text(airport.text));
                        });
                    },
                    error: function (xhr, status, error) {
                        console.error('Error loading airports:', error);
                        departureDropdown.empty().append('<option value="">Error loading departure airports</option>');
                        arrivalDropdown.empty().append('<option value="">Error loading arrival airports</option>');
                    }
                });
            },




            showflights: function () {
                var view = this;

                $('#bookingForm').submit(function (event) {
                    event.preventDefault();

                    view.validateform("#bookingForm");
                    var fromAirport = $('#fromAirport').val();
                    var toAirport = $('#toAirport').val();

                    $.ajax({
                        type: 'GET',
                        url: '/Admin/ScheduledFlights',
                        data: {
                            departureAirportId: fromAirport,
                            arrivalAirportId: toAirport
                        },
                        success: function (response) {
                            flightData = response;
                            view.$el.html(""); // Clear the existing content

                            if (response && response.length > 0) {
                                view.$el.append(_.template(view.templates.searchFlights)); // Add searchFlights template

                                response.forEach(function (flight) {
                                    var departureTime = new Date(flight.departureTime).toLocaleString();
                                    flight.departureTime = departureTime;

                                    var arrivalTime = new Date(flight.arrivalTime).toLocaleString();
                                    flight.arrivalTime = arrivalTime;

                                    var template = _.template(view.templates.searchDetail);
                                    var html = template(flight);
                                    view.$el.append(html);
                                });

                                // Fetch and display return flights
                                //view.returnflights(fromAirport, toAirport);
                            } else {
                                Swal.fire({
                                    title: "Error",
                                    text: "No flights available for the selected dates. Please select another date",
                                    icon: "error"
                                });
                            }
                        },
                        error: function (xhr, status, error) {
                            console.error(xhr.responseText);
                            Swal.fire({
                                title: "Error",
                                text: "An error occurred while fetching flights.",
                                icon: "error"
                            });
                        }
                    });
                });
            },

            economyDetails: function () {
                var view = this;

                if (flightData && flightData.length > 0) {
                    var flightId = flightData[0].flightId;

                    // Make an AJAX request to retrieve flight details by ID
                    $.ajax({
                        url: '/Admin/GetFlightId/' + flightId, // Pass the flightId in the URL
                        method: 'GET',
                        success: function (response) {
                            // Check if flight data is received successfully
                            if (response) {

                                var economyPrice = response.economyPrice;
                                var valuePrice = economyPrice + 100;

                                // Format departure and arrival times
                                var departureTime = new Intl.DateTimeFormat('en-US', {
                                    year: 'numeric', month: 'short', day: 'numeric',
                                    hour: 'numeric', minute: 'numeric', hour12: true
                                }).format(new Date(response.departureTime));

                                var arrivalTime = new Intl.DateTimeFormat('en-US', {
                                    year: 'numeric', month: 'short', day: 'numeric',
                                    hour: 'numeric', minute: 'numeric', hour12: true
                                }).format(new Date(response.arrivalTime));



                                view.selectedFlightOption = {
                                    flightId: flightId,
                                    economyPrice: economyPrice,
                                    valuePrice: valuePrice,
                                    departureAirport: response.departureAirport,
                                    arrivalAirport: response.arrivalAirport,
                                    departureTime: departureTime,
                                    arrivalTime: arrivalTime,
                                    selectedOption: 'Lite',
                                    price: economyPrice,
                                    departureAirportId: response.departureAirportId,
                                    arrivalAirportId: response.arrivalAirportId
                                };

                                $('#litePrice').text(economyPrice);
                                $('#valuePrice').text(valuePrice);

                                if (view.$('#ecoDetail').length === 0) {
                                    view.$el.append(_.template(view.templates.economyDetails)({
                                        economyPrice: economyPrice,
                                        valuePrice: valuePrice
                                    }));
                                } else {
                                    // After appending the template, check again for the presence of #ecoDetail
                                    // and then toggle its visibility
                                    if (view.$('#ecoDetail').length > 0) {
                                        view.$('#ecoDetail').toggle();
                                    } else {
                                        console.error("#ecoDetail element not found after appending template.");
                                    }
                                }

                                // Add event listener for radio button selection
                                $('input[name="flightOption"]').change(function () {
                                    var selectedOption = $('input[name="flightOption"]:checked').val();
                                    view.selectedFlightOption.selectedOption = selectedOption;
                                    view.selectedFlightOption.price = selectedOption === 'Lite' ? economyPrice : valuePrice;
                                });
                            } else {
                                // Handle case where flight data is not received
                                Swal.fire({
                                    title: "Error",
                                    text: "Failed to retrieve flight details.",
                                    icon: "error"
                                });
                            }
                        },
                        error: function (xhr, status, error) {
                            // Handle AJAX errors
                            Swal.fire({
                                title: "Error",
                                text: "Failed to retrieve flight details: " + error,
                                icon: "error"
                            });
                        }
                    });
                } else {
                    // Handle case where flight data is not available or empty
                    Swal.fire({
                        title: "Error",
                        text: "No flight data available.",
                        icon: "error"
                    });
                }
            },

            //return flight view
            returnflights: function () {
                var view = this;

                if (!view.selectedFlightOption || !view.selectedFlightOption.departureAirport || !view.selectedFlightOption.arrivalAirport) {
                    Swal.fire({
                        title: "Error",
                        text: "Please select a flight option first.",
                        icon: "error"
                    });
                    return;
                }

                var fromAirport = view.selectedFlightOption.arrivalAirportId;
                var toAirport = view.selectedFlightOption.departureAirportId;

                $.ajax({
                    type: 'GET',
                    url: '/Admin/ScheduledFlights',
                    data: {
                        departureAirportId: fromAirport,
                        arrivalAirportId: toAirport
                    },
                    success: function (response) {
                        if (response && response.length > 0) {
                            view.$el.html(""); // Clear the previous content
                            returnflightData = response;
                            response.forEach(function (flight) {
                                var departureTime = new Intl.DateTimeFormat('en-US', {
                                    year: 'numeric', month: 'short', day: 'numeric',
                                    hour: 'numeric', minute: 'numeric', hour12: true
                                }).format(new Date(flight.departureTime));

                                var arrivalTime = new Intl.DateTimeFormat('en-US', {
                                    year: 'numeric', month: 'short', day: 'numeric',
                                    hour: 'numeric', minute: 'numeric', hour12: true
                                }).format(new Date(flight.arrivalTime));

                                flight.departureTime = departureTime;
                                flight.arrivalTime = arrivalTime;


                                var template = _.template(view.templates.returnFlights);
                                var html = template(flight);
                                view.$el.append(html);

                                // Add event listener for selecting a return flight option
                                $('input[name="returnFlightOption"]').change(function () {
                                    var selectedOption = $('input[name="returnFlightOption"]:checked').val();
                                    view.selectedReturnOption.selectedOption = selectedOption;
                                    view.selectedReturnOption.price = selectedOption === 'Lite' ? economyPrice : valuePrice;

                                });
                            });
                        } else {
                            Swal.fire({
                                title: "Error",
                                text: "No return flights available for the selected dates. Please select another date",
                                icon: "error"
                            });
                        }
                    },
                    error: function (xhr, status, error) {
                        console.error(xhr.responseText);
                        Swal.fire({
                            title: "Error",
                            text: "An error occurred while fetching return flights.",
                            icon: "error"
                        });
                    }
                });
            },

            //appends the lite and value option for return flight
            returnOption: function () {
                var view = this;

                if (returnflightData && returnflightData.length > 0) {
                    var flightId = returnflightData[0].flightId;

                    // Make an AJAX request to retrieve flight details by ID
                    $.ajax({
                        url: '/Admin/GetFlightId/' + flightId, // Pass the flightId in the URL
                        method: 'GET',
                        success: function (response) {
                            // Check if flight data is received successfully
                            if (response) {

                                var economyPrice = response.economyPrice;
                                var valuePrice = economyPrice + 100;

                                // Format departure and arrival times
                                var departureTime = new Intl.DateTimeFormat('en-US', {
                                    year: 'numeric', month: 'short', day: 'numeric',
                                    hour: 'numeric', minute: 'numeric', hour12: true
                                }).format(new Date(response.departureTime));

                                var arrivalTime = new Intl.DateTimeFormat('en-US', {
                                    year: 'numeric', month: 'short', day: 'numeric',
                                    hour: 'numeric', minute: 'numeric', hour12: true
                                }).format(new Date(response.arrivalTime));

                                view.selectedReturnOption = {
                                    flightId: flightId,
                                    economyPrice: economyPrice,
                                    valuePrice: valuePrice,
                                    departureAirport: response.departureAirport,
                                    arrivalAirport: response.arrivalAirport,
                                    departureTime: departureTime,
                                    arrivalTime: arrivalTime,
                                    selectedOption: 'Lite',
                                    price: economyPrice,
                                    departureAirportId: response.departureAirportId,
                                    arrivalAirportId: response.arrivalAirportId,
                                    specialRequests: 'Lite',
                                };

                                $('#litePrice').text(economyPrice);
                                $('#valuePrice').text(valuePrice);

                                if (view.$('#ecoDetail').length === 0) {
                                    view.$el.append(_.template(view.templates.returnOption)({
                                        economyPrice: economyPrice,
                                        valuePrice: valuePrice
                                    }));
                                } else {
                                    if (view.$('#ecoDetail').length > 0) {
                                        view.$('#ecoDetail').toggle();
                                    } else {
                                        console.error("#ecoDetail element not found after appending template.");
                                    }
                                }

                                // Add event listener for radio button selection
                                $('input[name="flightOption"]').change(function () {
                                    var selectedOption = $('input[name="flightOption"]:checked').val();
                                    view.selectedReturnOption.selectedOption = selectedOption;
                                    view.selectedReturnOption.price = selectedOption === 'Lite' ? economyPrice : valuePrice;
                                });
                                $('input[name="flightOption"]').change(function () {
                                    var specialRequests = $('input[name="flightOption"]:checked').val();
                                    view.selectedReturnOption.specialRequests = specialRequests;
                                    view.selectedReturnOption.price = specialRequests === 'Lite' ? economyPrice : valuePrice;
                                });
                            } else {
                                // Handle case where flight data is not received
                                Swal.fire({
                                    title: "Error",
                                    text: "Failed to retrieve flight details.",
                                    icon: "error"
                                });
                            }
                        },
                        error: function (xhr, status, error) {
                            // Handle AJAX errors
                            Swal.fire({
                                title: "Error",
                                text: "Failed to retrieve flight details: " + error,
                                icon: "error"
                            });
                        }
                    });
                } else {
                    // Handle case where flight data is not available or empty
                    Swal.fire({
                        title: "Error",
                        text: "No flight data available.",
                        icon: "error"
                    });
                }

            },


            bookingPage: function () {
                var view = this;

                // Ensure both options are selected
                if (!view.selectedFlightOption.selectedOption || !view.selectedReturnOption.selectedOption) {
                    Swal.fire({
                        title: "Error",
                        text: "Please select both an outgoing and return flight option before proceeding to payment.",
                        icon: "error"
                    });
                    return;
                }

                // Store selected flight options in local storage
                localStorage.setItem('selectedFlightOption', JSON.stringify(view.selectedFlightOption));
                localStorage.setItem('selectedReturnOption', JSON.stringify(view.selectedReturnOption));


                // Combine the flight details
                var combinedFlightDetails = {
                    outgoingFlight: view.selectedFlightOption,
                    returnFlight: view.selectedReturnOption
                };

                var totalPrice = view.selectedFlightOption.price + view.selectedReturnOption.price;
                combinedFlightDetails.totalPrice = totalPrice;

                // Display selected flight details
                var flightDetailsTemplate = _.template(view.templates.booking);
                var flightDetailsHtml = flightDetailsTemplate(combinedFlightDetails);
                view.$el.html(flightDetailsHtml);



            },



            isUserLoggedIn: function () {
                var view = this;
                var loggedIn = false;

                $.ajax({
                    type: 'GET',
                    url: '/Admin/CheckLoginStatus',
                    success: function (response) {
                        loggedIn = response.isLoggedIn;
                        if (loggedIn) {
                            view.passengerDetails();
                        } else {
                            Swal.fire({
                                title: "Login Required",
                                text: "Please log in to proceed to the passenger details and checkout page.",
                                icon: "info",
                                showCancelButton: true,
                                confirmButtonText: "Login",
                                cancelButtonText: "Cancel"
                            }).then((result) => {
                                if (result.isConfirmed) {
                                    window.location.href = "/Identity/Account/Login";
                                }
                            });
                        }
                    },
                    error: function () {
                        console.error('Error checking login status');
                    }
                });
            },


            passengerDetails: function () {
                var view = this;
                view.$el.html(_.template(view.templates.passengerDetails));
                $("#firstName").val("");
                $("#lastName").val("");
                $("#gender").val("");
                $("#loyaltyCode").val("");
                $("#dob").val("");
                $("#nationality").val("");
                $("#passportNumber").val("");
                $("#expiryDate").val("");
                $("#issuingCountry").val("");
                $("#city").val("");
                $("#country").val("");
                $("#email").val("");
                $("#phone").val("");

                view.validatePassengerForm('#passengerForm');
            },

            handleFormSubmit: function (event) {
                event.preventDefault();
                this.savePassengerDetails();
            },

            validatePassengerForm: function (formName) {
                // Custom method to validate expiry date
                $.validator.addMethod("validExpiryDate", function (value, element) {
                    const today = new Date();
                    const enteredDate = new Date(value);
                    const minDate = new Date(today.getFullYear(), today.getMonth() + 1, today.getDate());
                    return this.optional(element) || enteredDate >= minDate;
                }, "Expiry date should be at least a month later than the current date.");

                // Custom method to validate date of birth
                $.validator.addMethod("validDOB", function (value, element) {
                    const today = new Date();
                    const enteredDate = new Date(value);
                    const minDate = new Date(1950, 0, 1); // January 1, 1950
                    const maxDate = new Date(today.getFullYear(), today.getMonth() - 1, today.getDate());
                    return this.optional(element) || (enteredDate >= minDate && enteredDate <= maxDate);
                }, "Date of birth should be at least a month before the current date and not before 1950.");

                $(formName).validate({
                    rules: {
                        firstName: {
                            required: true,
                            minlength: 3
                        },
                        lastName: {
                            required: true,
                            minlength: 2
                        },
                        loyaltyCode: {
                            required: true,
                            minlength: 4,
                            maxlength: 4,
                            digits: true
                        },
                        expiryMonth: {
                            required: true,
                            digits: true,
                            min: 1,
                            max: 12
                        },
                        dob: {
                            required: true,
                            date: true,
                            validDOB: true
                        },
                        expiryDate: {
                            required: true,
                            date: true,
                            validExpiryDate: true
                        },
                        email: {
                            required: true,
                            email: true
                        },
                        phone: {
                            required: true,
                            digits: true,
                            minlength: 7,
                            maxlength: 7
                        }
                    },
                    messages: {
                        firstName: {
                            required: "Please enter your first name",
                            minlength: "First name must be at least 3 characters long"
                        },
                        lastName: {
                            required: "Please enter your last name",
                            minlength: "Last name must be at least 2 characters long"
                        },
                        loyaltyCode: {
                            required: "Please enter your loyalty code",
                            minlength: "Loyalty code must be 4 digits long",
                            maxlength: "Loyalty code must be 4 digits long",
                            digits: "Loyalty code must be digits only"
                        },
                        expiryMonth: {
                            required: "Please enter the expiry month",
                            digits: "Please enter a valid month",
                            min: "Month must be between 1 and 12",
                            max: "Month must be between 1 and 12"
                        },
                        dob: {
                            required: "Please enter your date of birth",
                            date: "Please enter a valid date",
                            validDOB: "Date of birth should be at least a month before the current date and not before 1950."
                        },
                        expiryDate: {
                            required: "Please enter the expiry date",
                            date: "Please enter a valid date",
                            validExpiryDate: "Expiry date should be at least a month later than the current date."
                        },
                        email: {
                            required: "Please enter your email",
                            email: "Please enter a valid email address"
                        },
                        phone: {
                            required: "Please enter your phone number",
                            digits: "Please enter a valid phone number",
                            minlength: "Phone number must be 7 digits long",
                            maxlength: "Phone number must be 7 digits long"
                        }
                    },
                    errorClass: 'text-danger',
                    errorPlacement: function (error, element) {
                        error.insertBefore(element.parent());
                    }
                });
            },




            savePassengerDetails: function () {
                var view = this;
                if ($('#passengerForm').valid()) {
                    var firstName = $("#firstName").val();
                    var lastName = $("#lastName").val();
                    var gender = $("#gender").val();
                    var loyaltyCode = $("#loyaltyCode").val();
                    var dob = $("#dob").val();
                    var nationality = $("#nationality").val();
                    var passportNumber = $("#passportNumber").val();
                    var expiryDate = $("#expiryDate").val();
                    var issuingCountry = $("#issuingCountry").val();
                    var city = $("#city").val();
                    var country = $("#country").val();
                    var email = $("#email").val();
                    var phone = $("#phone").val();

                    var addPassengers = {
                        FirstName: firstName,
                        LastName: lastName,
                        Gender: gender,
                        DOB: dob,
                        LoyalityPoints: loyaltyCode,
                        Nationality: nationality,
                        PassportNumber: passportNumber,
                        ExpiryDate: expiryDate,
                        IssueCountry: issuingCountry,
                        City: city,
                        Country: country,
                        Email: email,
                        PhoneContact: phone,
                    };

                    $.ajax({
                        type: 'POST',
                        url: '/Home/AddPassengerDetails',
                        contentType: 'application/json',
                        data: JSON.stringify(addPassengers),
                        success: function (response) {
                            if (!response) {
                                alert("Error adding passenger details");
                            } else {
                                // Show success modal
                                view.bookingPage();
                                /* $('#successModal').modal('show');*/
                                var passengerId = response;

                                // Saving passenger ID in selected options
                                var selectedFlightOption = JSON.parse(localStorage.getItem('selectedFlightOption'));
                                var selectedReturnOption = JSON.parse(localStorage.getItem('selectedReturnOption'));

                                // Saving passenger ID in selected flight option
                                selectedFlightOption.passengerId = passengerId;
                                localStorage.setItem('selectedFlightOption', JSON.stringify(selectedFlightOption));

                                // Saving passenger ID in selected return option if applicable
                                if (selectedReturnOption) {
                                    selectedReturnOption.passengerId = passengerId;
                                    localStorage.setItem('selectedReturnOption', JSON.stringify(selectedReturnOption));
                                }


                            }
                        },
                        error: function () {
                            console.error('Error');
                        }
                    });
                }
            },


            saveBookings: function () {
                var view = this;

                // Retrieve selected flight details from localStorage
                var selectedFlightOption = JSON.parse(localStorage.getItem('selectedFlightOption'));
                var selectedReturnOption = JSON.parse(localStorage.getItem('selectedReturnOption'));

                // Ensure selected options are available
                if (!selectedFlightOption) {
                    console.error('Selected flight option not found.');
                    return;
                }
                // Combine booking details
                var bookingData = {
                    PassengerId: selectedFlightOption.passengerId,
                    OutboundFlightId: selectedFlightOption.flightId,
                    OutboundDepartTime: new Date(selectedFlightOption.departureTime).toISOString(),
                    OutboundArriveTime: new Date(selectedFlightOption.arrivalTime).toISOString(),
                    OutboundDepartAirport: selectedFlightOption.departureAirport,
                    OutboundArriveAirport: selectedFlightOption.arrivalAirport,
                    ReturnFlightId: selectedReturnOption ? selectedReturnOption.flightId : null,
                    ReturnDepartTime: selectedReturnOption ? new Date(selectedReturnOption.departureTime).toISOString() : null,
                    ReturnArriveTime: selectedReturnOption ? new Date(selectedReturnOption.arrivalTime).toISOString() : null,
                    ReturnDepartAirport: selectedReturnOption ? selectedReturnOption.departureAirport : null,
                    ReturnArriveAirport: selectedReturnOption ? selectedReturnOption.arrivalAirport : null,
                    TotalAmount: selectedFlightOption.price + (selectedReturnOption ? selectedReturnOption.price : 0),
                    PaymentStatus: "Successful", // Initial status, may be updated by backend after payment
                    SpecialRequests: selectedReturnOption ? selectedReturnOption.specialRequests : null,
                    OutboundPriceType: selectedFlightOption.selectedOption,
                    OutboundPrice: selectedFlightOption.price,
                    ReturnPriceType: selectedReturnOption.selectedOption,
                    ReturnPrice: selectedReturnOption.price,

                };

                // Make an AJAX request to save booking details
                $.ajax({
                    type: 'POST',
                    url: '/Home/SaveBookingDetails', // Adjust URL as necessary
                    contentType: 'application/json',
                    data: JSON.stringify(bookingData),
                    success: function (response) {
                        if (!response) {
                            alert("Error adding passenger details");
                        } else {
                            // Show success modal
                            view.proceedToPayment();
                            var bookingId = response;

                            // Saving passenger ID in selected options
                            var selectedFlightOption = JSON.parse(localStorage.getItem('selectedFlightOption'));
                            var selectedReturnOption = JSON.parse(localStorage.getItem('selectedReturnOption'));

                            // Saving passenger ID in selected flight option
                            selectedFlightOption.bookingId = bookingId;
                            localStorage.setItem('selectedFlightOption', JSON.stringify(selectedFlightOption));

                            // Saving passenger ID in selected return option if applicable
                            if (selectedReturnOption) {
                                selectedReturnOption.bookingId = bookingId;
                                localStorage.setItem('selectedReturnOption', JSON.stringify(selectedReturnOption));
                            }

                        }
                    },
                    error: function () {
                        console.error('Error saving booking details');
                        swal("Error saving booking details", { icon: "error" });
                    }
                });
            },

            validateform: function (formName) {
                $(formName).validate({
                    rules: {
                        cardNumber: {
                            required: true,
                            minlength: 6,
                            maxlength: 6
                        },
                        cardName: {
                            required: true,
                            minlength: 2
                        },
                        expiryMonth: {
                            required: true,
                            digits: true,
                            min: 1,
                            max: 12
                        },
                        expiryYear: {
                            required: true,
                            digits: true,
                            min: new Date().getFullYear()
                        },
                        cvv: {
                            required: true,
                            digits: true,
                            minlength: 3,
                            maxlength: 4
                        }
                    },
                    messages: {
                        cardNumber: {
                            required: "Please enter your card number",
                            creditcard: "Please enter a valid card number",
                            minlength: "Card number must be 6 digits long",
                            maxlength: "Card number must be 6 digits long"
                        },
                        cardName: {
                            required: "Please enter the name on your card",
                            minlength: "Name on card must be at least 2 characters long"
                        },
                        expiryMonth: {
                            required: "Please enter the expiry month",
                            digits: "Please enter a valid month",
                            min: "Month must be between 1 and 12",
                            max: "Month must be between 1 and 12"
                        },
                        expiryYear: {
                            required: "Please enter the expiry year",
                            digits: "Please enter a valid year",
                            min: "Year must be the current year or later"
                        },
                        cvv: {
                            required: "Please enter the CVV",
                            digits: "Please enter a valid CVV",
                            minlength: "CVV must be at least 3 digits long",
                            maxlength: "CVV must be no more than 4 digits long"
                        }
                    },
                    errorClass: 'text-danger',
                    errorPlacement: function (error, element) {
                        error.insertBefore(element.parent());
                    }
                });
            },


            proceedToPayment: function () {
                $('#successModal').modal('hide');
                this.paymentPage();
            },

            paymentPage: function () {
                var view = this;
                var selectedFlightOption = JSON.parse(localStorage.getItem('selectedFlightOption'));
                var selectedReturnOption = JSON.parse(localStorage.getItem('selectedReturnOption'));

                var combinedFlightDetails = {
                    outgoingFlight: selectedFlightOption,
                    returnFlight: selectedReturnOption,
                    totalPrice: selectedFlightOption.price + selectedReturnOption.price
                };

                var paymentPageTemplate = _.template(this.templates.paymentPage);
                var paymentPageHtml = paymentPageTemplate(combinedFlightDetails);
                this.$el.html(paymentPageHtml);
                view.validateform("#paymentForm");
            },


            makePayment: function () {
                event.preventDefault();
                var view = this;
                if ($('#paymentForm').valid()) {
                    var cardNumber = $("#cardNumber").val();
                    var cardName = $("#cardName").val();
                    var expiryMonth = $("#expiryMonth").val();
                    var expiryYear = $("#expiryYear").val();
                    var cvv = $("#cvv").val();

                    // Retrieve selected flight details from localStorage
                    var selectedFlightOption = JSON.parse(localStorage.getItem('selectedFlightOption'));
                    var selectedReturnOption = JSON.parse(localStorage.getItem('selectedReturnOption'));

                    // Construct payment data
                    var paymentData = {
                        BookingId: selectedFlightOption.bookingId,
                        Amount: selectedFlightOption.price + (selectedReturnOption ? selectedReturnOption.price : 0),
                        PaymentDate: new Date(),
                        PaymentStatus: "Successful",
                        CardNumber: cardNumber,
                        CVV: cvv,
                        NameOnCard: cardName,
                        ExpiryYear: expiryYear,
                        ExpiryMonth: expiryMonth,
                    };

                    $.ajax({
                        type: 'POST',
                        url: '/Home/PaymentProcess',
                        contentType: 'application/json',
                        data: JSON.stringify(paymentData),
                        success: function (response) {
                            if (!response) {
                                alert("Error processing payment");
                            } else {
                                // Show success modal
                                view.itineraryPage();
                            }
                        },
                        error: function (xhr, status, error) {
                            console.error('Error processing payment:', status, error);
                            swal("Error processing payment", { icon: "error" });
                        }

                    });
                }
            },


            itineraryPage: function () {
                var view = this;

                // Retrieve the selected flight option from localStorage
                var selectedFlightOption = JSON.parse(localStorage.getItem('selectedFlightOption'));

                // Check if selectedFlightOption exists and has a bookingId
                if (selectedFlightOption && selectedFlightOption.bookingId) {
                    var bookingId = selectedFlightOption.bookingId;

                    // Make an AJAX request to get itinerary details using the booking ID
                    $.ajax({
                        url: '/Home/GetItinerary/' + bookingId, // Use bookingId in the URL
                        type: 'GET',
                        dataType: 'json',
                        success: function (data) {
                            var confirmationNumber = data;

                            // Saving passenger ID in selected options
                            var selectedFlightOption = JSON.parse(localStorage.getItem('selectedFlightOption'));
                            var selectedReturnOption = JSON.parse(localStorage.getItem('selectedReturnOption'));

                            // Saving passenger ID in selected flight option
                            selectedFlightOption.confirmationNumber = confirmationNumber;
                            localStorage.setItem('selectedFlightOption', JSON.stringify(selectedFlightOption));


                            var itineraryHtml = _.template(view.templates.itinerary)(data);
                            view.$el.html(itineraryHtml);
                        },
                        error: function () {
                            alert('Error loading itinerary details.');
                        }
                    });
                } else {
                    // Handle the case where the booking ID is not available
                    alert('Booking ID not found.');
                }
            }








        })
    })







