define(['jquery', 'underscore', 'backbone',

    //templates 
    'text!src/booking/temp/booking.html',
    'text!src/booking/temp/flightDetails.html',

    //plug-ins
    //https://unpkg.com/sweetalert/dist/sweetalert.min.js',
    'ext/plugin2/jquery.dataTables.min',
    'ext/plugin2/dataTables.bootstrap4.min',
    'https://code.jquery.com/jquery-1.12.4.js',
    'https://code.jquery.com/ui/1.12.1/jquery-ui.js',
    'https://cdnjs.cloudflare.com/ajax/libs/sweetalert/2.1.0/sweetalert.min.js',
    'ext/validate/jquery.validate',



],

    function ($, _, Backbone, bookingview) {
        return Backbone.View.extend({

            el: '#flightDetails',

            templates: {

              
                booking: require('text!src/booking/temp/booking.html'),
                flightDetails: require('text!src/booking/temp/flightDetails.html'),
            },
            events: {
                /*  'change #fromAirport': 'fetchlocation',*/
                'click #showflights': 'showflights',
            },


            render: function () {
                var view = this;
                view.$el.html("");
                view.$el.append(_.template(this.templates.booking));
                view.showflights();

            },


            showflights: function () {
                var view = this;

                $('#bookingForm').submit(function (event) {
                    event.preventDefault(); // Prevent default form submission

                    // Fetch flights based on user input
                    var fromAirport = $('#fromAirport').val();
                    var toAirport = $('#toAirport').val();
                    var formData = {
                        fromAirport: fromAirport,
                        toAirport: toAirport
                    };

                    // Perform AJAX request to fetch flights
                    $.ajax({
                        type: 'GET',
                        url: '/Admin/GetFlights',
                        data: formData,
                        success: function (response) {
                            if (response && response.length > 0) {
                                // Clear previous flight details
                                $('#flightCardsContainer').empty();

                                // Load flight card template
                                $.get('/js2/src/booking/temp/flightDetails.html', function (template) {
                                    // Append flight cards
                                    response.forEach(function (flight) {
                                        var flightCard = $(template);
                                        flightCard.find('#departureTime').text(flight.departureTime);
                                        flightCard.find('#departureAirport').text(flight.departureAirport);
                                        flightCard.find('#arrivalTime').text(flight.arrivalTime);
                                        flightCard.find('#arrivalAirport').text(flight.arrivalAirport);
                                        flightCard.find('#economyPrice').text(flight.economyPrice);
                                        flightCard.find('#businessPrice').text(flight.businessPrice);
                                        $('#flightCardsContainer').append(flightCard);
                                    });
                                });
                            } else {
                                view.showNoFlightsMessage();
                            }
                        },
                        error: function (xhr, status, error) {
                            console.error(xhr.responseText); // Log error message
                            view.showNoFlightsMessage();
                        }
                    });
                });
            }



            
       



        })
    })
