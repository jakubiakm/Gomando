using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

using Android.App;
using Android.Content;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;

using Gomando.Logic;
using Gomando.Model.Models;
using Plugin.FacebookClient;
using Plugin.FacebookClient.Abstractions;

namespace Gomando.Activities
{
    [Activity(Label = "Szczegóły treningu")]
    public class TrainingDetailsActivity : BaseActivity, IOnMapReadyCallback
    {
        TrainingDetailsLogic trainingDetailsLogic = new TrainingDetailsLogic();

        public FloatingActionButton EditTrainingButton { get; private set; }
        public FloatingActionButton DeleteTrainingButton { get; private set; }
        public FloatingActionButton ShareOnFacebookButton { get; private set; }
        public TextView DistanceTextView { get; private set; }
        public TextView TimeTextView { get; private set; }

        public Training Training { get; private set; }

        public void OnMapReady(GoogleMap googleMap)
        {
            List<List<Localization>> Localizations = new List<List<Localization>>();
            LatLngBounds.Builder builder = new LatLngBounds.Builder();
            if (Training.SerializedLocalizations != null && Training.SerializedLocalizations.Length > 0)
            {
                IFormatter formatter = new BinaryFormatter();
                using (MemoryStream stream = new MemoryStream(Training.SerializedLocalizations))
                {
                    Localizations = formatter.Deserialize(stream) as List<List<Localization>>;
                }
            }
            foreach (var path in Localizations)
            {
                PolylineOptions options = new PolylineOptions()
                    .InvokeColor((new Color(105, 121, 176, 200).ToArgb()))
                    .InvokeWidth(20);
                foreach (var point in path)
                {
                    options.Add(new LatLng(point.Latitude, point.Longitude));
                    builder.Include(new LatLng(point.Latitude, point.Longitude));
                }
                googleMap.AddPolyline(options);
            }
            LatLngBounds bounds = builder.Build();
            // begin new code:
            int width = Resources.DisplayMetrics.WidthPixels;
            int height = Resources.DisplayMetrics.HeightPixels;
            int padding = (int)(width * 0.05);

            CameraUpdate cameraUpdate = CameraUpdateFactory.NewLatLngBounds(bounds, width, height, padding);
            googleMap.AnimateCamera(cameraUpdate);
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            FacebookClientManager.Initialize(this);

            LayoutInflater inflater = (LayoutInflater)this.GetSystemService(Context.LayoutInflaterService);
            View contentView = inflater.Inflate(Resource.Layout.training_details_layout, null, false);
            mDrawer.AddView(contentView, 0);

            DeleteTrainingButton = FindViewById<FloatingActionButton>(Resource.Id.trainingDetailsDeleteTrainingButton);
            ShareOnFacebookButton = FindViewById<FloatingActionButton>(Resource.Id.trainingDetailsShareOnFacebook);
            int trainingId = Intent.GetIntExtra("TrainingId", 0);
            Training = trainingDetailsLogic.GetTraining(trainingId);

            DeleteTrainingButton.Click += DeleteTrainingButton_Click;
            ShareOnFacebookButton.Click += ShareOnFacebookButton_Click;

            var mapFragment = FragmentManager.FindFragmentById(Resource.Id.fragment_training_map) as MapFragment;
            mapFragment.GetMapAsync(this);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent intent)
        {
            base.OnActivityResult(requestCode, resultCode, intent);
            FacebookClientManager.OnActivityResult(requestCode, resultCode, intent);
        }

        private async void ShareOnFacebookButton_Click(object sender, EventArgs e)
        {
            await CrossFacebookClient.Current.LoginAsync(new string[] { "email", "user_posts" });
        }

        private void DeleteTrainingButton_Click(object sender, EventArgs e)
        {
            var builder = new AlertDialog.Builder(this);
            builder.SetMessage("Czy na pewno chcesz usunąć trening?");
            builder.SetPositiveButton("Tak", (s, ew) =>
            {
                trainingDetailsLogic.DeleteTraining(Training.Id);
                Finish();
            });
            builder.SetNegativeButton("Nie", (s, ew) => { });
            builder.Create().Show();

        }

        private void EditTrainingButton_Click(object sender, EventArgs e)
        {
            double distance, time;
            if (string.IsNullOrWhiteSpace(DistanceTextView.Text) || string.IsNullOrWhiteSpace(TimeTextView.Text))
                return;
            if (!double.TryParse(DistanceTextView.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out distance) || !double.TryParse(TimeTextView.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out time))
                return;

            Training.Distance = distance;
            Training.Time = time;
            trainingDetailsLogic.EditTraining(Training);
            Finish();
        }
    }
}