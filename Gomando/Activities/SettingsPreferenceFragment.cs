using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Preferences;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Gomando.Activities
{
    [Activity(Label = "SettingsPreferenceFragment")]
    public class SettingsPreferenceFragment : PreferenceFragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            AddPreferencesFromResource(Resource.Layout.training_history_preference);

            var prefs = PreferenceManager.GetDefaultSharedPreferences(this.Activity);
        }
    }
}