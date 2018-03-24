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

        public TrainingHistoryAdapter(List<Training> trainings)
        {
            this.trainings = trainings;
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

            trainingHistoryHolder.Time.Text = trainings[position].Time.ToString();
            trainingHistoryHolder.Distance.Text = trainings[position].Distance.ToString();
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