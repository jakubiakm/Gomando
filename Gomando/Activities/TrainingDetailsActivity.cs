﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
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

        public Button EditTrainingButton { get; private set; }
        public Button DeleteTrainingButton { get; private set; }
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
            EditTrainingButton = FindViewById<Button>(Resource.Id.trainingDetailsEditTrainingButton);
            DeleteTrainingButton = FindViewById<Button>(Resource.Id.trainingDetailsDeleteTrainingButton);

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
            Training.Distance = double.Parse(DistanceTextView.Text);
            Training.Time = double.Parse(TimeTextView.Text);
            trainingDetailsLogic.EditTraining(Training);
            Finish();
        }
    }
}