using System;
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
                distanceText = $"Dystans: {trainings[position].Distance.ToString()} kilometrów";
            else
                distanceText = $"Dystans: {(trainings[position].Distance * 0.62).ToString()} mil";

            trainingHistoryHolder.Date.Text = $"{trainings[position].StartDate.ToShortDateString()}, {trainings[position].StartDate.ToLongTimeString()}";
            trainingHistoryHolder.Time.Text = $"Czas: {trainings[position].Time.ToString()} sekund";
            trainingHistoryHolder.Distance.Text = distanceText;
        }

        // Return the number of photos available in the photo album:
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
    }
}