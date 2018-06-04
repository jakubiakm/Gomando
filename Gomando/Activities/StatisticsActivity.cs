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
using Gomando.Logic.Helpers;
using Gomando.Logic;
using Gomando.Model.Models;

namespace Gomando.Activities
{
    [Activity(Label = "Statystyki")]
    public class StatisticsActivity : BaseActivity
    {
        StatisticsLogic statisticsLogic = new StatisticsLogic();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            LayoutInflater inflater = (LayoutInflater)this.GetSystemService(Context.LayoutInflaterService);
            View contentView = inflater.Inflate(Resource.Layout.statistics_layout, null, false);
            mDrawer.AddView(contentView, 0);

            Spinner spinner = FindViewById<Spinner>(Resource.Id.training_type_spinner);

            spinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinner_ItemSelected);
            var adapter = ArrayAdapter.CreateFromResource(
                    this, Resource.Array.training_type_array, Android.Resource.Layout.SimpleSpinnerItem);

            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spinner.Adapter = adapter;
        }

        private void spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spinner = (Spinner)sender;
            TextView textView = (TextView)FindViewById(Resource.Id.training_statistics_content);
            string selectedIitemString = spinner.GetItemAtPosition(e.Position).ToString();
            string statisticString = "";
            TrainingTypeStatistic statistics = new TrainingTypeStatistic();

            if(selectedIitemString == Resources.GetString(Resource.String.training_type_all_name))
            {
                statistics = statisticsLogic.GetTrainingTypeStatistic();
            }
            if (selectedIitemString == Resources.GetString(Resource.String.training_type_cycling_name))
            {
                statistics = statisticsLogic.GetTrainingTypeStatistic(Model.Enums.TrainingType.Cycling);
            }
            if (selectedIitemString == Resources.GetString(Resource.String.training_type_hiking_name))
            {
                statistics = statisticsLogic.GetTrainingTypeStatistic(Model.Enums.TrainingType.Hiking);
            }
            if (selectedIitemString == Resources.GetString(Resource.String.training_type_running_name))
            {
                statistics = statisticsLogic.GetTrainingTypeStatistic(Model.Enums.TrainingType.Running);
            }
            if (selectedIitemString == Resources.GetString(Resource.String.training_type_skating_name))
            {
                statistics = statisticsLogic.GetTrainingTypeStatistic(Model.Enums.TrainingType.Skating);
            }
            if (selectedIitemString == Resources.GetString(Resource.String.training_type_walking_name))
            {
                statistics = statisticsLogic.GetTrainingTypeStatistic(Model.Enums.TrainingType.Walking);
            }
            statisticString += $"Ilość treningów:\t {statistics.Count}";
            statisticString += "\n";
            statisticString += $"Sumaryczny czas:\t {MathHelper.ConvertSecondsToTimeString(statistics.AllTime)}\n";
            statisticString += $"Sumaryczny dystans:\t {string.Format("{0:0.00}", statistics.AllDistance)} km\n";
            statisticString += $"Średni czas treningu:\t {MathHelper.ConvertSecondsToTimeString(statistics.AverageTime)} \n";
            statisticString += $"Średni dystans treningu:\t {string.Format("{0:0.00}", statistics.AverageDistance)} km\n";
            statisticString += "\n";
            statisticString += $"Średnie tempo:\t {string.Format("{0:0.00}", statistics.AverageTempo)} min\\km\n";
            statisticString += $"Średnia prędkość:\t {string.Format("{0:0.00}", statistics.AverageVelocity)} km\\h\n";

            textView.Text = statisticString;
        }
    }
}