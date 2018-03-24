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
using Android.Support.V7.Widget;

using Gomando.Model.Models;
using Gomando.Logic;
using Gomando.Adapters;

namespace Gomando.Activities
{
    [Activity(Label = "Historia treningów")]
    public class TrainingHistoryActivity : BaseActivity
    {
        TrainingHistoryLogic trainingHistoryLogic = new TrainingHistoryLogic();

        RecyclerView mRecyclerView;
        RecyclerView.LayoutManager mLayoutManager;
        TrainingHistoryAdapter mAdapter;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            LayoutInflater inflater = (LayoutInflater)this.GetSystemService(Context.LayoutInflaterService);
            View contentView = inflater.Inflate(Resource.Layout.training_history_layout, null, false);
            mDrawer.AddView(contentView, 0);

            List<Training> trainings = trainingHistoryLogic.GetAllTrainings();

            mRecyclerView = FindViewById<RecyclerView>(Resource.Id.trainingHistoryRecyclerView);
            mLayoutManager = new LinearLayoutManager(this);
            mRecyclerView.SetLayoutManager(mLayoutManager);
            mAdapter = new TrainingHistoryAdapter(trainings);
            mAdapter.ItemClick += OnItemClick;
            mRecyclerView.SetAdapter(mAdapter);
        }

        // Handler for the item click event:
        void OnItemClick(object sender, int position)
        {
            // Display a toast that briefly shows the enumeration of the selected photo:
            int photoNum = position + 1;
            Toast.MakeText(this, "This is photo number " + photoNum, ToastLength.Short).Show();
            mAdapter.trainings.Add(new Training() {  Time = (new Random()).Next()});
            mAdapter.NotifyDataSetChanged();
        }
    }
}