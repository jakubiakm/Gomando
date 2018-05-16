using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Graphics;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Gomando.Helpers
{
    public class TrainingHelper
    {
        public GoogleMap Map { get; set; }
        public Location CurrentLocation { get; set; }
        public bool TrainingPaused { get; set; } = true;
        public bool LastLocationAdded { get; private set; } = false;
        public List<List<LatLng>> AllTrainingLocations { get; private set; } = new List<List<LatLng>>();
        public List<LatLng> CurrentTrainingLocations { get; private set; } = new List<LatLng>();

        public Marker CurrentLocationMarker { get; private set; } = null;
        public Circle CurrentLocationAccuracy { get; private set; } = null;

        public TrainingHelper(GoogleMap map) => Map = map;



        public void ChangeCurrentLocation(Location location)
        {
            if (CurrentLocation != null)
            {
                if (TrainingPaused)
                {
                    if (LastLocationAdded)
                    {
                        LastLocationAdded = false;
                        AllTrainingLocations.Add(CurrentTrainingLocations);
                        CurrentTrainingLocations = new List<LatLng>();
                    }
                }
                else
                {
                    CurrentTrainingLocations.Add(new LatLng(location.Latitude, location.Longitude));
                    LastLocationAdded = true;
                }
            }
            AnimateToCurrentLocation();
            SetCurrentLocationMarker();
            CurrentLocation = location;
        }

        public void AnimateToCurrentLocation()
        {
            if (Map != null)
            {
                if (CurrentLocation == null)
                {
                    Map.MoveCamera(CameraUpdateFactory.NewLatLngZoom(new LatLng(52.216819, 21.014766), 14));
                }
                else
                {
                    Map.AnimateCamera(CameraUpdateFactory.NewLatLng(new LatLng(CurrentLocation.Latitude, CurrentLocation.Longitude)));
                }
            }
        }

        public void SetCurrentLocationMarker()
        {
            if (Map != null)
            {
                if (CurrentLocation != null)
                {
                    if(CurrentLocationMarker == null)
                    {
                        MarkerOptions markerOpt = new MarkerOptions()
                            .SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.ic_current_location_marker))
                            .SetPosition(new LatLng(CurrentLocation.Latitude, CurrentLocation.Longitude));
                        markerOpt.Anchor((float)0.5, (float)0.5);
                        CurrentLocationMarker = Map.AddMarker(markerOpt);

                        CircleOptions circleOpt = new CircleOptions()
                            .InvokeRadius(6)
                            .InvokeCenter(new LatLng(0, 0))
                            .InvokeStrokeColor((new Color(0, 0, 0, 255)).ToArgb())
                            .InvokeStrokeWidth((float)0.5)
                            .InvokeFillColor((new Color(0, 191, 255, 100).ToArgb()));
                        CurrentLocationAccuracy = Map.AddCircle(circleOpt);

                    }
                    CurrentLocationMarker.Position = new LatLng(CurrentLocation.Latitude, CurrentLocation.Longitude);
                    CurrentLocationAccuracy.Radius = CurrentLocation.Accuracy;
                    CurrentLocationAccuracy.Center = new LatLng(CurrentLocation.Latitude, CurrentLocation.Longitude);
                }
            }
        }
    }
}