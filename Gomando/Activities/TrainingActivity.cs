using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using Gomando.Model.Enums;

namespace Gomando.Activities
{
    [Activity(Label = "Trening", MainLauncher = true, LaunchMode = LaunchMode.SingleTask)]
    [IntentFilter(
    new[] { Intent.ActionView },
    Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
    DataScheme = "gomando.gomando",
    DataHost = "gomando.eu.auth0.com",
    DataPathPrefix = "/android/gomando.gomando/callback")]
    public class TrainingActivity : BaseActivity
    {
        public TrainingType CurrentTrainingType { get; set; } = TrainingType.Running;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            mNavigationView.SetCheckedItem(Resource.Id.menu_navigation_training);

            LayoutInflater inflater = (LayoutInflater)this.GetSystemService(Context.LayoutInflaterService);
            View contentView = inflater.Inflate(Resource.Layout.training_layout, null, false);
            mDrawer.AddView(contentView, 0);

            AddClickEventsToControls();
        }

        protected override void OnResume()
        {
            base.OnResume();
            mNavigationView.SetCheckedItem(Resource.Id.menu_navigation_training);
        }

        private void AddClickEventsToControls()
        {
            FindViewById(Resource.Id.layout_training_sliding_drawer_training_type).Click += TrainingTypeLayout_Click;
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

        #region UI EVENTS
        protected override async void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);

            if (intent.DataString != null)
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
        #endregion
    }
}