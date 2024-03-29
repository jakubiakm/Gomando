﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Gomando.Model.Enums;
using Gomando.Model.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Gomando.Helpers;
using Gomando.Logic;
using Gomando.Model.Models;


namespace Gomando.Adapters
{
    public class TrainingHistoryAdapter : RecyclerView.Adapter
    {
        public event EventHandler<int> ItemClick;

        public List<Training> trainings;

        bool ShowKilometerDistanceUnit;

        public TrainingHistoryAdapter(List<Training> trainings, bool showKilometerDistanceUnit, bool SortDescending)
        {
            this.trainings = trainings;
            if (SortDescending)
            {
                this.trainings = trainings.OrderByDescending(t => t.StartDate).ToList();
            }
            else
            {
                this.trainings = trainings.OrderBy(t => t.StartDate).ToList();
            }
            ShowKilometerDistanceUnit = showKilometerDistanceUnit;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            // Inflate the CardView for the photo:
            View itemView = LayoutInflater.From(parent.Context).
                        Inflate(Resource.Layout.training_history_view, parent, false);

            // Create a ViewHolder to find and hold these view references, and 
            // register OnClick with the view holder:
            TrainingHistoryHolder trainingHistoryHolder = new TrainingHistoryHolder(itemView, OnClick);
            return trainingHistoryHolder;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            TrainingHistoryHolder trainingHistoryHolder = holder as TrainingHistoryHolder;
            string distanceText = "";
            if (ShowKilometerDistanceUnit)
                distanceText = $"Dystans: {string.Format("{0:0.00}", trainings[position].Distance)} km";
            else
                distanceText = $"Dystans: {string.Format("{0:0.00}", (trainings[position].Distance * 0.62))} mil";

            trainingHistoryHolder.Date.Text = $"{ConvertTrainingTypeToName(trainings[position].Type)}\n{trainings[position].StartDate.ToShortDateString()}, {trainings[position].StartDate.ToLongTimeString()}";
            trainingHistoryHolder.Time.Text = $"Czas: {TimeSpan.FromSeconds(trainings[position].Time).ToString(@"hh\h\:mm\m\:ss\s")}";
            trainingHistoryHolder.Distance.Text = distanceText;
            trainingHistoryHolder.Velocity.Text = $"Średnia prędkość: {string.Format("{0:0.00}", trainings[position].Distance / (trainings[position].Time / 3600))} km/h";
        }

        public override int ItemCount
        {
            get { return trainings.Count(); }
        }

        // Raise an event when the item-click takes place:
        void OnClick(int position)
        {
            if (ItemClick != null)
                ItemClick(this, position);
        }

        private string ConvertTrainingTypeToName(TrainingType type)
        {
            string trainingName = "";
            switch (type)
            {
                case TrainingType.Cycling:
                    trainingName = "Kolarstwo";
                    break;
                case TrainingType.Hiking:
                    trainingName = "Wspinaczka górska";
                    break;
                case TrainingType.Running:
                    trainingName = "Bieganie";
                    break;
                case TrainingType.Skating:
                    trainingName = "Jazda na rolkach";
                    break;
                case TrainingType.Walking:
                    trainingName = "Chodzenie";
                    break;
                default:
                    throw new Exception("Nie znaleziono typu treningu");
            }
            return trainingName;
        }
    }
}