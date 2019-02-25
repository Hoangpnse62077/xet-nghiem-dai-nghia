  $('.map')
      .gmap3({
          center: [48.8620722, 2.352047],
          zoom: 13,
          scrollwheel: false,
          mapTypeControl: false,
          streetViewControl: false,
          mapTypeId: "shadeOfGrey", // to select it directly
          mapTypeControlOptions: {
              mapTypeIds: [google.maps.MapTypeId.ROADMAP, "shadeOfGrey"]
          }
      })
      .marker([
          { position: [48.8588377,2.2770206], icon: "assets/img/marker.png" },
          { address: "48.8548302,2.3027162", icon: "assets/img/marker.png" },
          { address: "48.8600687,2.3727792", icon: "assets/img/marker.png" },
          { address: "48.8625697,2.3505492", icon: "assets/img/marker.png" }
      ])
      .styledmaptype(
          "shadeOfGrey", [
              { "featureType": "all", "elementType": "labels.text.fill", "stylers": [{ "saturation": 36 }, { "color": "#5B5652" }, { "lightness": 40 }] },
              { "featureType": "all", "elementType": "labels.text.stroke", "stylers": [{ "visibility": "on" }, { "color": "#ffffff" }, { "lightness": 16 }] },
              { "featureType": "all", "elementType": "labels.icon", "stylers": [{ "visibility": "off" }] },
              { "featureType": "administrative", "elementType": "geometry.fill", "stylers": [{ "color": "#000000" }, { "lightness": 20 }] },
              { "featureType": "administrative", "elementType": "geometry.stroke", "stylers": [{ "color": "#ffffff" }, { "lightness": 17 }, { "weight": 1.2 }] },
              { "featureType": "landscape", "elementType": "geometry", "stylers": [{ "color": "#ffffff" }, { "lightness": 20 }] },
              { "featureType": "poi", "elementType": "geometry", "stylers": [{ "color": "#ffffff" }, { "lightness": 21 }] },
              { "featureType": "road.highway", "elementType": "geometry.fill", "stylers": [{ "color": "#EDEDED" }, { "lightness": 17 }] },
              { "featureType": "road.highway", "elementType": "geometry.stroke", "stylers": [{ "color": "#EDEDED" }, { "lightness": 29 }, { "weight": 0.2 }] },
              { "featureType": "road.arterial", "elementType": "geometry", "stylers": [{ "color": "#F1F1F1" }, { "lightness": 18 }] },
              { "featureType": "road.local", "elementType": "geometry", "stylers": [{ "color": "#F1F1F1" }, { "lightness": 16 }] },
              { "featureType": "transit", "elementType": "geometry", "stylers": [{ "color": "#F1F1F1" }, { "lightness": 19 }] },
              { "featureType": "water", "elementType": "geometry", "stylers": [{ "color": "#C5D6D0" }, { "lightness": 17 }] }
          ], { name: "Shades of Grey" }
      );
