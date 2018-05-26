using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Gms.Common;
using Android.Gms.Location;
using Android.Gms.Maps;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;
using Gomando.Helpers;
using Gomando.Logic;
using Gomando.Model.Enums;

namespace Gomando.Activities
{
    internal class MyLocationCallback : LocationCallback
    {
        public EventHandler<Location> LocationUpdated;
        public override void OnLocationResult(LocationResult result)
        {
            base.OnLocationResult(result);
            LocationUpdated?.Invoke(this, result.LastLocation);
        }
    }

    [Activity(Label = "Trening", MainLauncher = true, LaunchMode = LaunchMode.SingleTask, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation,
    ScreenOrientation = ScreenOrientation.Portrait)]
    [IntentFilter(
    new[] { Intent.ActionView },
    Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
    DataScheme = "gomando.gomando",
    DataHost = "gomando.eu.auth0.com",
    DataPathPrefix = "/android/gomando.gomando/callback")]
    public class TrainingActivity : BaseActivity, IOnMapReadyCallback
    {

        readonly string[] PermissionsLocation =
        {
            Manifest.Permission.AccessCoarseLocation,
            Manifest.Permission.AccessFineLocation
        };

        const int RequestLocationId = 0;
        public int TrainingTime { get; set; } = 0;
        public double TrainingDistance { get; set; } = 0;
        public double TrainingTempo { get; set; } = 0;
        public double TrainingAverageTempo { get; set; } = 0;
        public double TrainingVelocity { get; set; } = 0;
        public double TrainingAverageVelocity { get; set; } = 0;

        public DateTime TrainingStartDate { get; set; } = DateTime.MinValue;

        MyLocationCallback locationCallback;
        FusedLocationProviderClient locationClient;

        public TrainingHelper trainingHelper = new TrainingHelper(null, TrainingState.NotStarted);

        public TrainingType CurrentTrainingType { get; set; } = TrainingType.Running;

        public TrainingState CurrentTrainingState { get; set; } = TrainingState.NotStarted;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            mNavigationView.SetCheckedItem(Resource.Id.menu_navigation_training);

            LayoutInflater inflater = (LayoutInflater)this.GetSystemService(Context.LayoutInflaterService);
            View contentView = inflater.Inflate(Resource.Layout.training_layout, null, false);
            mDrawer.AddView(contentView, 0);

            AddClickEventsToControls();
            GetLocationPermission();
        }

        private void GetGoogleMap()
        {
            var mapFragment = FragmentManager.FindFragmentById(Resource.Id.fragment_training_map) as MapFragment;
            mapFragment.GetMapAsync(this);
        }

        protected override void OnResume()
        {
            base.OnResume();
            mNavigationView.SetCheckedItem(Resource.Id.menu_navigation_training);
        }

        private void AddClickEventsToControls()
        {
            FindViewById(Resource.Id.layout_training_sliding_drawer_training_type).Click += TrainingTypeLayout_Click;
            FindViewById(Resource.Id.button_training_left).Click += LeftTrainingButton_Click;
            FindViewById(Resource.Id.button_training_right).Click += RightTrainingButton_Click;
        }


        private void ChangeTrainingType(TrainingType trainingType)
        {
            CurrentTrainingType = trainingType;

            FindViewById<ImageView>(Resource.Id.image_training_training_type).SetImageResource(MapTrainingTypeToNameIdAndImageSourceId(trainingType).Item2);
            FindViewById<TextView>(Resource.Id.text_training_training_type_name).Text = MapTrainingTypeToNameIdAndImageSourceId(trainingType).Item1;
        }

        private (string, int) MapTrainingTypeToNameIdAndImageSourceId(TrainingType type)
        {
            string trainingName = "";
            int trainingIconId = 0;
            switch (type)
            {
                case TrainingType.Cycling:
                    trainingName = Resources.GetString(Resource.String.training_type_cycling_name);
                    trainingIconId = Resource.Drawable.ic_training_type_cycling;
                    break;
                case TrainingType.Hiking:
                    trainingName = Resources.GetString(Resource.String.training_type_hiking_name);
                    trainingIconId = Resource.Drawable.ic_training_type_walking;
                    break;
                case TrainingType.Running:
                    trainingName = Resources.GetString(Resource.String.training_type_running_name);
                    trainingIconId = Resource.Drawable.ic_training_type_running;
                    break;
                case TrainingType.Skating:
                    trainingName = Resources.GetString(Resource.String.training_type_skating_name);
                    trainingIconId = Resource.Drawable.ic_training_type_skating;
                    break;
                case TrainingType.Walking:
                    trainingName = Resources.GetString(Resource.String.training_type_walking_name);
                    trainingIconId = Resource.Drawable.ic_training_type_walking;
                    break;
                default:
                    throw new Exception("Nie znaleziono typu treningu");
            }
            return (trainingName, trainingIconId);
        }

        public void OnMapReady(GoogleMap googleMap)
        {
            trainingHelper = new TrainingHelper(googleMap, CurrentTrainingState);
        }

        private void SetUpTimers()
        {
            Timer timer = new Timer();
            timer.Interval = 1000;
            timer.Elapsed += new ElapsedEventHandler(TimerElapsedEventHandler);
            timer.Start();
        }
        private void TimerElapsedEventHandler(object sender, EventArgs e)
        {
            if (CurrentTrainingState == TrainingState.Paused || CurrentTrainingState == TrainingState.NotStarted)
                ShowNotification();
            if (CurrentTrainingState == TrainingState.Started)
            {
                TrainingTime++;
            }
            RunOnUiThread(() => FindViewById<TextView>(Resource.Id.text_training_sliding_drawer_time_value).Text = MathHelper.ConvertSecondsToTimeString(TrainingTime));
        }

        private void ShowNotification()
        {
            //Intent pauseReceive = new Intent();
            //pauseReceive.SetAction("Pause");
            //PendingIntent pendingIntentYes = PendingIntent.GetBroadcast(this, 12345, pauseReceive, PendingIntentFlags.UpdateCurrent);
            //var intent = new Intent(this, typeof(TrainingActivity))
            //   .SetFlags(ActivityFlags.ReorderToFront);
            //const int pendingIntentId = 0;
            //PendingIntent pendingIntent = PendingIntent.GetActivity(this, pendingIntentId, intent, PendingIntentFlags.UpdateCurrent);
            //NotificationBuilder.MActions.Clear();
            //NotificationBuilder.SetContentText(string.Format(Resources.GetString(Resource.String.notification_content_text), Time, Distance, Tempo));
            //NotificationBuilder.SetContentIntent(pendingIntent);
            //NotificationBuilder.SetPriority(2);
            //NotificationBuilder.AddAction(new Android.Support.V4.App.NotificationCompat.Action(
            //    Resource.Drawable.ic_launcher_gomando_sport,
            //    CurrentTrainingState == TrainingState.Active ?
            //    Resources.GetString(Resource.String.notification_action_pause_text) :
            //    Resources.GetString(Resource.String.notification_action_resume_text),
            //    pendingIntentYes));
            //NotificationManager.Notify(NotifyID, NotificationBuilder.Build());
        }

        async Task StartLocationUpdatesAsync()
        {
            // Create a callback that will get the location updates
            if (locationCallback == null)
            {
                locationCallback = new MyLocationCallback();
                locationCallback.LocationUpdated += OnLocationResult;
            }

            // Get the current client
            if (locationClient == null)
                locationClient = LocationServices.GetFusedLocationProviderClient(this);

            try
            {
                //Create request and set intervals:
                //Interval: Desired interval for active location updates, it is inexact and you may not receive upates at all if no location servers are available
                //Fastest: Interval is exact and app will never receive updates faster than this value
                var locationRequest = new LocationRequest()
                                          .SetInterval(10000)
                                          .SetFastestInterval(5000)
                                          .SetPriority(LocationRequest.PriorityHighAccuracy);

                await locationClient.RequestLocationUpdatesAsync(locationRequest, locationCallback);
            }
            catch (Exception)
            {
            }
        }

        private void OnLocationResult(object sender, Location e)
        {
            double time, distance;
            (time, distance) = trainingHelper.ChangeCurrentLocation(e);
            if (time > -1 && distance > 0)
            {
                TrainingDistance += distance;
                TrainingTempo = (time / 1000 / 60) / (distance);
                TrainingVelocity = distance / (time / 1000 / 60 / 60);
                TrainingAverageTempo = ((double)TrainingTime / 60) / TrainingDistance;
                TrainingAverageVelocity = TrainingDistance / ((double)TrainingTime / 60 / 60);
                RunOnUiThread(() =>
                {
                    FindViewById<TextView>(Resource.Id.text_training_sliding_drawer_distance_value).Text = string.Format("{0:0.00}", TrainingDistance);
                    FindViewById<TextView>(Resource.Id.text_training_sliding_drawer_training_parameter_1_value).Text = string.Format("{0:0.00}", TrainingDistance);
                    FindViewById<TextView>(Resource.Id.text_training_sliding_drawer_training_parameter_2_value).Text = string.Format("{0:0.00}", TrainingAverageTempo);
                    FindViewById<TextView>(Resource.Id.text_training_sliding_drawer_training_parameter_3_value).Text = string.Format("{0:0.00}", TrainingVelocity);
                    FindViewById<TextView>(Resource.Id.text_training_sliding_drawer_training_parameter_4_value).Text = string.Format("{0:0.00}", TrainingAverageVelocity);
                });
            }
        }

        public void StartTraining()
        {
            TrainingTime = 0;
            TrainingDistance = 0;
            TrainingStartDate = DateTime.Now;

            trainingHelper.Reset();
            CurrentTrainingState = TrainingState.Started;
            trainingHelper.CurrentTrainingState = TrainingState.Started;
            var button = FindViewById(Resource.Id.button_training_left);
            var layoutParams = (RelativeLayout.LayoutParams)button.LayoutParameters;
            layoutParams.AddRule(LayoutRules.CenterInParent, 1);
            button.LayoutParameters = layoutParams;

            FindViewById(Resource.Id.button_training_left).Visibility = ViewStates.Visible;
            FindViewById(Resource.Id.button_training_right).Visibility = ViewStates.Gone;
            FindViewById<ImageButton>(Resource.Id.button_training_left).SetImageResource(Resource.Drawable.btn_training_pause);
        }

        public void ResumeTraining()
        {
            CurrentTrainingState = TrainingState.Started;
            trainingHelper.CurrentTrainingState = TrainingState.Started;
            var button = FindViewById(Resource.Id.button_training_left);
            var layoutParams = (RelativeLayout.LayoutParams)button.LayoutParameters;
            layoutParams.AddRule(LayoutRules.CenterInParent, 1);
            button.LayoutParameters = layoutParams;

            FindViewById(Resource.Id.button_training_left).Visibility = ViewStates.Visible;
            FindViewById(Resource.Id.button_training_right).Visibility = ViewStates.Gone;
            FindViewById<ImageButton>(Resource.Id.button_training_left).SetImageResource(Resource.Drawable.btn_training_pause);
        }

        public void PauseTraining()
        {
            CurrentTrainingState = TrainingState.Paused;
            trainingHelper.CurrentTrainingState = TrainingState.Paused;
            var button = FindViewById(Resource.Id.button_training_left);
            var layoutParams = (RelativeLayout.LayoutParams)button.LayoutParameters;
            layoutParams.AddRule(LayoutRules.CenterInParent, 0);
            button.LayoutParameters = layoutParams;

            FindViewById(Resource.Id.button_training_left).Visibility = ViewStates.Visible;
            FindViewById(Resource.Id.button_training_right).Visibility = ViewStates.Visible;
            FindViewById<ImageButton>(Resource.Id.button_training_left).SetImageResource(Resource.Drawable.btn_training_start);
        }

        public void EndTraining()
        {
            trainingHelper.SaveTraining(CurrentTrainingType, TrainingStartDate, TrainingDistance, TrainingTime);
            trainingHelper.Reset();
            CurrentTrainingState = TrainingState.NotStarted;
            ResetTrainingStatistics();
            trainingHelper.CurrentTrainingState = TrainingState.NotStarted;
            var button = FindViewById(Resource.Id.button_training_left);
            var layoutParams = (RelativeLayout.LayoutParams)button.LayoutParameters;
            layoutParams.AddRule(LayoutRules.CenterInParent, 1);
            button.LayoutParameters = layoutParams;

            FindViewById(Resource.Id.button_training_left).Visibility = ViewStates.Visible;
            FindViewById(Resource.Id.button_training_right).Visibility = ViewStates.Gone;
            FindViewById<ImageButton>(Resource.Id.button_training_left).SetImageResource(Resource.Drawable.btn_training_start);
        }

        private void ResetTrainingStatistics()
        {
            TrainingDistance = 0;
            TrainingTempo = 0;
            TrainingVelocity = 0;
            TrainingAverageTempo = 0;
            TrainingAverageVelocity = 0;
            TrainingTime = 0;

            RunOnUiThread(() =>
            {
                FindViewById<TextView>(Resource.Id.text_training_sliding_drawer_time_value).Text = MathHelper.ConvertSecondsToTimeString(TrainingTime);
                FindViewById<TextView>(Resource.Id.text_training_sliding_drawer_distance_value).Text = string.Format("{0:0.00}", TrainingDistance);
                FindViewById<TextView>(Resource.Id.text_training_sliding_drawer_training_parameter_1_value).Text = string.Format("{0:0.00}", TrainingDistance);
                FindViewById<TextView>(Resource.Id.text_training_sliding_drawer_training_parameter_2_value).Text = string.Format("{0:0.00}", TrainingAverageTempo);
                FindViewById<TextView>(Resource.Id.text_training_sliding_drawer_training_parameter_3_value).Text = string.Format("{0:0.00}", TrainingVelocity);
                FindViewById<TextView>(Resource.Id.text_training_sliding_drawer_training_parameter_4_value).Text = string.Format("{0:0.00}", TrainingAverageVelocity);
            });
        }

        void GetLocationPermission()
        {
            if ((int)Build.VERSION.SdkInt < 23)
                return;
            const string permission = Manifest.Permission.AccessFineLocation;
            if (CheckSelfPermission(permission) == (int)Permission.Granted)
            {
                GetGoogleMap();
                SetUpTimers();
                StartLocationUpdatesAsync();
                ResetTrainingStatistics();
                return;
            }
            RequestPermissions(PermissionsLocation, RequestLocationId);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            switch (requestCode)
            {
                case RequestLocationId:
                    {
                        if (grantResults[0] == Permission.Granted)
                        {
                            GetGoogleMap();
                            SetUpTimers();
                            StartLocationUpdatesAsync();
                            ResetTrainingStatistics();
                        }
                    }
                    break;
            }
        }

        #region UI EVENTS
        protected override async void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);

            if (intent.DataString != null && authorizeState != null)
            {
                var loginResult = await client.ProcessResponseAsync(intent.DataString, authorizeState);
            }
        }

        private void TrainingTypeLayout_Click(object sender, EventArgs e)
        {
            TrainingType[] trainingTypes = Enum.GetValues(typeof(TrainingType)) as TrainingType[];
            Android.Support.V7.App.AlertDialog.Builder builder = new Android.Support.V7.App.AlertDialog.Builder(this);
            builder.SetTitle(Resources.GetString(Resource.String.training_type_select_type_name));
            builder.SetItems(
                trainingTypes.Select(type => MapTrainingTypeToNameIdAndImageSourceId(type).Item1).ToArray(),
                new EventHandler<DialogClickEventArgs>((senderr, ee) => ChangeTrainingType(trainingTypes[ee.Which])));
            Android.Support.V7.App.AlertDialog alert = builder.Create();
            alert.Show();
        }


        private void LeftTrainingButton_Click(object sender, EventArgs e)
        {
            switch (CurrentTrainingState)
            {
                case TrainingState.NotStarted:
                    StartTraining();
                    break;
                case TrainingState.Started:
                    PauseTraining();
                    break;
                case TrainingState.Paused:
                    ResumeTraining();
                    break;
            }
        }

        private void RightTrainingButton_Click(object sender, EventArgs e)
        {
            switch (CurrentTrainingState)
            {
                case TrainingState.Paused:
                    EndTraining();
                    break;
            }
        }
        #endregion

    }
}