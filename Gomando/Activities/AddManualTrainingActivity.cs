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
    [Activity(Label = "Dodaj trening manualnie")]
    public class AddManualTrainingActivity : BaseActivity
    {
        AddManualTrainingLogic addManualTrainingLogic = new AddManualTrainingLogic();

        public Button AddTrainingButton { get; private set; }
        public TextView DistanceTextView { get; private set; }
        public TextView TimeTextView { get; private set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            LayoutInflater inflater = (LayoutInflater)this.GetSystemService(Context.LayoutInflaterService);
            View contentView = inflater.Inflate(Resource.Layout.add_manual_training_layout, null, false);
            mDrawer.AddView(contentView, 0);

            AddTrainingButton = FindViewById<Button>(Resource.Id.addManualTrainingAddTrainingButton);
            DistanceTextView = FindViewById<EditText>(Resource.Id.addManualTrainingDistanceEditText);
            TimeTextView = FindViewById<EditText>(Resource.Id.addManualTrainingTimeEditText);

            AddTrainingButton.Click += MButtonAddManualTraining_Click;
        }

        private void MButtonAddManualTraining_Click(object sender, EventArgs e)
        {
            Training training = GetTraining();
            if(training != null)
            {
                addManualTrainingLogic.SaveTraining(training);
            }
            Finish();
        }

        private Training GetTraining()
        {
            double result;
            if (string.IsNullOrWhiteSpace(DistanceTextView.Text) || string.IsNullOrWhiteSpace(TimeTextView.Text))
                return null;
            if (!double.TryParse(DistanceTextView.Text, out result) || !double.TryParse(TimeTextView.Text, out result))
                return null;
            Training training = new Training()
            {
                Distance = double.Parse(DistanceTextView.Text),
                Time = double.Parse(TimeTextView.Text),
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddMinutes(55)
            };
            return training; 
        }
    }
}