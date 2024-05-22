$(document).ready(function () {
	



    
	/*******jQuerry for Displaying Open Shelter Number **********/
    $.ajax({
        url: "/Feeds/Evacs.json",
        dataType: 'json',
        success: function (data) {
            getTopTabsInfo(data,evacTab);
        },
        error: function (e) {
            alert("error : " + e);
        }
    });
	/*******jQuerry for Displaying Power Outage Number **********/
    $.ajax({
        url: "Feeds/Roads.json",
        dataType: 'json',
        success: function (data) {
            getTopTabsInfo(data, powerTab);
        },
        error: function (e) {
            alert("error : " + e);
        }
    });

    /*******jQuerry for mapping **********/
	
		
 
 
			
              
              
 
              var tmr_SpecialEvents = new L.GeoJSON.AJAX("http://131940.qld.gov.au/api/json/v1/events/specialevent?state=qld",      
             {
                pointToLayer: function(feature, latlng) {
                    return new L.Marker(latlng, { icon: iconSpecialEvent });
                },
                                      onEachFeature: function (feature, layer) {
                                      //var $html = $(L.HTMLUtils.buildTable(feature.properties.event));
 
                                      layer.bindPopup(feature.properties.event.description.replace(/\r\n|\n|\r/g, '<br /><br />'),{
                                                minWidth: 320,
                                                maxWidth: 320
                                                                                                }
                             )}
                             }).addTo(map);
 
              var tmr_Cams = new L.GeoJSON.AJAX("http://131940.qld.gov.au/api/json/v1/poi/general/webcams",
            {
                pointToLayer: function(feature, latlng) {
                    return new L.Marker(latlng, { icon: iconCamera });
                },
                onEachFeature: function (feature, layer){
                    layer.bindPopup(feature.properties.event.description
                    + '<br/>'
                    + '<div class="photothumb">'
                    + '<a rel="lightbox" href="'
                    + feature.properties.metadata.url
                    + '" />'
                    + '<img src="'
                    + feature.properties.metadata.url
                    + '" />'
                    + '</a>'
                    + '</div>'
                    ,{
                                        minWidth: 320,
                                        maxWidth: 320
                                                                                                }
                             )}
                             }).addTo(map);
							 
		 
		  var LGArea = new L.GeoJSON([LGData], {
                
				style: {					
					color:"purple",
					weight: 2,
					fill: "none",
					opacity: 0.8,
					fillOpacity: 0,
					clickable: false
                }
            }).addTo(map);
			
			var suburbInfo = new L.GeoJSON.AJAX("./Feeds/Localities_Ipswich.geojson",{
				style: {					
					color:"purple",
					weight: 2,
					fill: "none",
					opacity: 0.8,
					fillOpacity: 0.5,
					clickable: false
                }
            });
			var suburbBoundry = new L.GeoJSON.AJAX("./Feeds/Locality_Boundaries_Ipswich.geojson",{
				style: {					
					color:"blue",
					weight: 2,
					fill: "none",
					opacity: 0.8,
					fillOpacity: 0,
					clickable: false
                }
            }).addTo(map);
		//parsing the xml file to get the suburbs list 	
		$.ajax({
			url: "https://www.energex.com.au/static/Energex/planned_outages.xml",
			dataType: 'xml',
			success: function (xml) {
				var newsItem;
				
			},
			error: function (e) {
				alert("error : " + e);
			}
		});	
			
            

    /*******jQuerry for Displaying News Feeds **********/
    $.ajax({
        url: "./Feeds/Bulletins.json",
        dataType: 'json',
        success: function (data) {
            var newsItem;
            $.each(data.features, function (i, feature) {
                var dateTime = getDateTime(feature.temporal.lastupdated);
                newsItem = '<div class="contentItem"><div class="contentTitle"><a href="#">' + feature.properties.title +
                '</a></div><div class="contentDetail">' + feature.properties.description +
                '</div><div class="updateInfo">Published on ' + dateTime + '</div></div>';
                $("#newsContent").append(newsItem);
            });
            newsItem = '<div class="viewMore">View More</div>';
            $("#newsContent").append(newsItem);
        },
        error: function (e) {
            alert("error : " + e);
        }
    });

    /*******jQuerry for Displaying BOM Warning Feeds **********/
    $.ajax({
        url: "./Feeds/Bulletins.json",
        dataType: 'json',
        success: function (data) {
            var warningItem;
            $.each(data.features, function (i, feature) {
                var dateTime = getDateTime(feature.temporal.lastupdated);                
                warningItem = '<div class="contentItem"><div class="contentTitle"><a href="#">' + feature.properties.title +
                '</a></div><div class="contentDetail">' + feature.properties.description +
                '</div><div class="updateInfo">Published on ' + dateTime + '</div></div>';
                $("#weatherContent").append(warningItem);
            });
            warningItem = '<div class="viewMore">View More</div>';
            $("#weatherContent").append(warningItem);
        },
        error: function (e) {
            alert("error : " + e);
        }
    });
    
});
function getDateTime(dateFromJSON){
	var date = dateFromJSON.substr(0, 10);
	var time = dateFromJSON.substr(11, 5);
	return(date + ' at ' + time);
}
function getTopTabsInfo(data,roadTab){
	var numberOfElements = data.features.length;
	var dateTime = getDateTime(data.published);
	var updateInfo = 'Published on ' + dateTime ;
	$(roadTab).find(' .infoNumbers').html(numberOfElements);
	$(roadTab).find(' .updateInfo').html(updateInfo);
}