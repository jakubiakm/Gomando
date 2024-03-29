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

namespace Gomando.Adapters
{
    public class TrainingHistoryHolder : RecyclerView.ViewHolder
    {
        public TextView Distance { get; private set; }
        public TextView Time { get; private set; }
        public TextView Date { get; private set; }
        public TextView Velocity { get; private set; }

        // Get references to the views defined in the CardView layout.
        public TrainingHistoryHolder(View itemView, Action<int> listener) : base(itemView)
        {
            Date = itemView.FindViewById<TextView>(Resource.Id.trainingHistoryDateTextView);
            Distance = itemView.FindViewById<TextView>(Resource.Id.trainingHistoryDistanceTextView);
            Time = itemView.FindViewById<TextView>(Resource.Id.trainingHistoryTimeTextView);
            Velocity = itemView.FindViewById<TextView>(Resource.Id.trainingHistoryVelocityTextView);
            
            // Detect user clicks on the item view and report which item
            // was clicked (by layout position) to the listener:
            itemView.Click += (sender, e) => listener(base.LayoutPosition);
        }
    }
}