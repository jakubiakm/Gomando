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
        Button mButtonAddManualTraining;

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.action_preferences:

                    Toast.MakeText(this, "no wreszcie", ToastLength.Long).Show();
                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.actions_menu, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            LayoutInflater inflater = (LayoutInflater)this.GetSystemService(Context.LayoutInflaterService);
            View contentView = inflater.Inflate(Resource.Layout.training_history_layout, null, false);
            mDrawer.AddView(contentView, 0);

            List<Training> trainings = trainingHistoryLogic.GetAllTrainings();

            mButtonAddManualTraining = FindViewById<Button>(Resource.Id.trainingHistoryAddManualTrainingButton);
            mButtonAddManualTraining.Click += MButtonAddManualTraining_Click;
            mRecyclerView = FindViewById<RecyclerView>(Resource.Id.trainingHistoryRecyclerView);
            mLayoutManager = new LinearLayoutManager(this);
            mRecyclerView.SetLayoutManager(mLayoutManager);
            mAdapter = new TrainingHistoryAdapter(trainings);
            mAdapter.ItemClick += OnItemClick;
            mRecyclerView.SetAdapter(mAdapter);
        }

        private void MButtonAddManualTraining_Click(object sender, EventArgs e)
        {
            var addManualTraining = new Intent(this, typeof(AddManualTrainingActivity));
            StartActivityForResult(addManualTraining, 0);
        }

        // Handler for the item click event:
        void OnItemClick(object sender, int position)
        {
            int photoNum = position + 1;
            var trainingDetails = new Intent(this, typeof(TrainingDetailsActivity));
            trainingDetails.PutExtra("TrainingId", mAdapter.trainings[position].Id);
            StartActivityForResult(trainingDetails, 0);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            mAdapter.trainings = trainingHistoryLogic.GetAllTrainings();
            mAdapter.NotifyDataSetChanged();
        }
    }
}