using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Gomando.Helpers
{
    public class GoogleMapsHelper
    {
        public GoogleMap Map { get; set; }

        public LatLng CurrentLocation { get; set; } = new LatLng(52.216819, 21.014766);

        public GoogleMapsHelper(GoogleMap map) => Map = map;

        public void ZoomToCurrentLocation()
        {
            if (Map != null)
            {
                CameraPosition.Builder builder = CameraPosition.InvokeBuilder();
                builder.Target(CurrentLocation);
                builder.Zoom(11);
                CameraPosition cameraPosition = builder.Build();
                CameraUpdate cameraUpdate = CameraUpdateFactory.NewCameraPosition(cameraPosition);
                Map.MoveCamera(cameraUpdate);
            }
        }
    }
}