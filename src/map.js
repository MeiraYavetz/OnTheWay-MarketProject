import React, { useEffect,useState} from 'react';
import Box from '@mui/material/Box';

import { useSelector } from 'react-redux';
//, DirectionsRenderer
import { Map, Marker, GoogleApiWrapper, Polyline , DirectionsRenderer} from 'google-maps-react';
import CircularProgress from '@mui/material/CircularProgress';
//import { GoogleMap, LoadScript, MarkerF, DirectionsService, DirectionsRenderer } from '@react-google-maps/api';
const DogAddressesMap = ({ google }) => {
  const listMap = useSelector(state => state.products.listMap);
  const [routeCoordinates, setRouteCoordinates] = useState([]);

  useEffect(() => {
    if(listMap.length>0)
      calculateRoute();
  }, [listMap]);

  const calculateRoute = () => {
    debugger
    const dogAddresses = listMap.map(product => ({
      lat: product.geoX,
      lng: product.geoY,
      name: product.nameOfStore,
      list: product.products
    }));
console.log(listMap)
console.log("!!!")
console.log(dogAddresses)

      debugger
console.log(dogAddresses[0].lat)
    const waypoints = dogAddresses.map(address => ({
      location: new google.maps.LatLng(address.lat, address.lng),
      stopover: true
    }));

    const directionsService = new google.maps.DirectionsService();
    debugger
    directionsService.route(
      {
        origin: waypoints[0].location,
        destination: waypoints[waypoints.length-1].location,
        waypoints: [waypoints[1]],
        optimizeWaypoints: false,
        travelMode: google.maps.TravelMode.WALKING
      },
      (result, status) => {debugger
        if (status === google.maps.DirectionsStatus.OK) {
          const route = result.routes[0];
          const routeCoordinates = [];
          for (let i = 0; i < route.legs.length; i++) {
            const leg = route.legs[i];
            console.log(leg);
            for (let j = 0; j < leg.steps.length; j++) {
              const step = leg.steps[j];
              console.log(step);
              for (let k = 0; k < step.path.length; k++) {
                const point = step.path[k];
                console.log(point);
                routeCoordinates.push({ lat: point.lat(), lng: point.lng() });
              }
            }
          }
          setRouteCoordinates(routeCoordinates);
        }
      }
    );
  }

  const dogAddresses = listMap.map(product => ({
    lat: product.geoX,
    lng: product.geoY,
    name: product.nameOfStore,
    list: product.products
  }));

  const mapOptions = {
    mapTypeId: google.maps.MapTypeId.ROADMAP,
    language: 'he',
    zoomControl: true,
    mapTypeControl: false,
    scaleControl: true,
    streetViewControl: false,
    rotateControl: false,
    fullscreenControl: false
  };

  return (
    <>
   {
    listMap.length>0?
    <div style={{ height: '100vh', width: '100vw',  position: 'fixed',
    top: '0',
    left:' 0',
    zIndex:'8'
   }}>
    
      <Map
        google={google}
        zoom={18}
        initialCenter={dogAddresses[0]}
        options={mapOptions}
        language="he" 
        region="IL" // Add this line to display the map for Israel
      >
        
        {dogAddresses.map(address => (
          <Marker
            label={address.name}
            key={address.name}
            position={{ lat: address.lat, lng: address.lng }}
            title={address.list.join(", ")}
          />
        ))}
        {routeCoordinates.length > 0 && (
          <Marker
            position={{
              lat: routeCoordinates[1].lat,
              lng: routeCoordinates[1].lng
            }}
            icon={{
              url: 'https://maps.google.com/mapfiles/kml/paddle/red-circle.png',
              anchor: new google.maps.Point(10, 10),
              scaledSize: new google.maps.Size(20, 20)
            }}
          />
        )}
        {routeCoordinates.length > 0 && (
          <Polyline
            path={routeCoordinates}
            options={{
              strokeColor: "#00BFFF",
              strokeOpacity: 0.7,
              strokeWeight: 10
            }}
          />
        )}
      </Map>
    </div>:
    <Box sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center', height: '100vh' }}>
    <CircularProgress />
  </Box>
  }
    </>
  );
}

export default GoogleApiWrapper({
  apiKey: 'AIzaSyAd-0M-alwppdecG0L2WQMS97hXbRNRTjA'
})(DogAddressesMap);
