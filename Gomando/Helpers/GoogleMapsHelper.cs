using System;
using System.Collections.Generic;

using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Graphics;
using Android.Locations;
using Android.Views;
using Gomando.Logic;
using Gomando.Model.Enums;
using Gomando.Model.Models;
namespace Gomando.Helpers
{
    public class TrainingHelper
    {
        public TrainingLogic logic = new TrainingLogic();
        public GoogleMap Map { get; set; }
        public Location CurrentLocation { get; set; }
        public TrainingState CurrentTrainingState { get; set; } = TrainingState.NotStarted;
        public bool LastLocationAdded { get; private set; } = false;
        public bool MapsInitialized { get; private set; } = false;
        public List<List<Localization>> AllTrainingLocalizations { get; private set; } = new List<List<Localization>>();
        public List<Localization> CurrentTrainingLocalizations { get; private set; } = new List<Localization>();

        public Marker CurrentLocationMarker { get; private set; } = null;
        public Circle CurrentLocationAccuracy { get; private set; } = null;
        public Polyline MapRoadPolyLine { get; private set; } = null;
        public List<Polyline> PolyLines { get; private set; } = new List<Polyline>();

        public TrainingHelper(GoogleMap map, TrainingState state)
        {
            Map = map;
            CurrentTrainingState = state;
        }

        public void Reset()
        {
            LastLocationAdded = false;
            AllTrainingLocalizations = new List<List<Localization>>();
            CurrentTrainingLocalizations = new List<Localization>();
            PolyLines.ForEach(polyline => polyline.Remove());
            PolyLines = new List<Polyline>();
            MapRoadPolyLine = null;
        }

        public void SaveTraining(TrainingType type, DateTime start, double distance, double time)
        {
            if (CurrentTrainingLocalizations.Count != 0)
                AllTrainingLocalizations.Add(CurrentTrainingLocalizations);
            Training training = new Training()
            {
                Distance = distance,
                Time = time,
                StartDate = start,
                EndDate = DateTime.Now,
                Type = type
            };
            logic.SaveTraining(training, AllTrainingLocalizations);
        }


        public (double, double) ChangeCurrentLocation(Location location)
        {
            double elapsedTime = -1, distance = -1;
            if (MapsInitialized)
            {
                if (CurrentTrainingState != TrainingState.Started)
                {
                    CurrentLocation = location;
                    if (LastLocationAdded)
                    {
                        LastLocationAdded = false;
                        AllTrainingLocalizations.Add(CurrentTrainingLocalizations);
                        CurrentTrainingLocalizations = new List<Localization>();
                    }
                }
                else
                {
                    if (!LastLocationAdded)
                    {
                        AddLocationLineOnMap();
                        CurrentTrainingLocalizations.Add(new Localization
                        {
                            Date = DateTime.Now,
                            Latitude = CurrentLocation.Latitude,
                            Longitude = CurrentLocation.Longitude
                        });
                        LastLocationAdded = true;
                    }
                    else
                    {
                        distance = MathHelper.HaversineDistance(location.Latitude, location.Longitude, CurrentLocation.Latitude, CurrentLocation.Longitude);
                        elapsedTime = (location.Time - CurrentLocation.Time);
                    }
                    CurrentLocation = location;
                    AddLocationLineOnMap();
                    CurrentTrainingLocalizations.Add(new Localization
                    {
                        Date = DateTime.Now,
                        Latitude = CurrentLocation.Latitude,
                        Longitude = CurrentLocation.Longitude
                    });
                    LastLocationAdded = true;
                }
                AnimateToCurrentLocation();
                SetCurrentLocationMarker();
            }
            else
            {
                AnimateToCurrentLocation();
                SetCurrentLocationMarker();
                CurrentLocation = location;
                MapsInitialized = true;
            }
            return (elapsedTime, distance);
        }

        private void AddLocationLineOnMap()
        {
            if (Map != null)
            {
                if (CurrentLocation != null)
                {
                    if (MapRoadPolyLine == null || !LastLocationAdded)
                    {
                        PolylineOptions options = new PolylineOptions()
                            .InvokeColor((new Color(105, 121, 176, 200).ToArgb()))
                            .InvokeWidth(20);
                        options.Add(new LatLng(CurrentLocation.Latitude, CurrentLocation.Longitude));
                        MapRoadPolyLine = Map.AddPolyline(options);
                        PolyLines.Add(MapRoadPolyLine);
                    }
                    else
                    {
                        IList<LatLng> points = MapRoadPolyLine.Points;
                        points.Add(new LatLng(CurrentLocation.Latitude, CurrentLocation.Longitude));
                        MapRoadPolyLine.Points = points;
                    }
                }
            }
        }

        public void AnimateToCurrentLocation()
        {
            if (Map != null)
            {
                if (CurrentLocation == null)
                {
                    Map.MoveCamera(CameraUpdateFactory.NewLatLngZoom(new LatLng(52.216819, 21.014766), 17));
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
                    if (CurrentLocationMarker == null)
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