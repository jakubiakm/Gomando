using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;

using Gomando.Logic;
using Gomando.Model.Models;

namespace Gomando.Activities
{
    [Activity(Label = "Szczegóły treningu")]
    public class TrainingDetailsActivity : BaseActivity
    {
        TrainingDetailsLogic trainingDetailsLogic = new TrainingDetailsLogic();

        public FloatingActionButton EditTrainingButton { get; private set; }
        public FloatingActionButton DeleteTrainingButton { get; private set; }
        public TextView DistanceTextView { get; private set; }
        public TextView TimeTextView { get; private set; }

        public Training Training { get; private set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            LayoutInflater inflater = (LayoutInflater)this.GetSystemService(Context.LayoutInflaterService);
            View contentView = inflater.Inflate(Resource.Layout.training_details_layout, null, false);
            mDrawer.AddView(contentView, 0);

            DistanceTextView = FindViewById<EditText>(Resource.Id.trainingDetailsDistanceEditText);
            TimeTextView = FindViewById<EditText>(Resource.Id.trainingDetailsTimeEditText);
            EditTrainingButton = FindViewById<FloatingActionButton>(Resource.Id.trainingDetailsEditTrainingButton);
            DeleteTrainingButton = FindViewById<FloatingActionButton>(Resource.Id.trainingDetailsDeleteTrainingButton);



            int trainingId = Intent.GetIntExtra("TrainingId", 0);
            Training = trainingDetailsLogic.GetTraining(trainingId);

            DistanceTextView.Text = Training.Distance.ToString();
            TimeTextView.Text = Training.Time.ToString();

            EditTrainingButton.Click += EditTrainingButton_Click;
            DeleteTrainingButton.Click += DeleteTrainingButton_Click;
        }

        private void DeleteTrainingButton_Click(object sender, EventArgs e)
        {
            trainingDetailsLogic.DeleteTraining(Training.Id);
            Finish();
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