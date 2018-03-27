using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Preferences;
using Android.Runtime;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;
using Gomando.Activities;

namespace Gomando.Fragments
{
    class PrefsFragment : PreferenceFragment
    {
        TrainingHistoryActivity Parent;
        public PrefsFragment(TrainingHistoryActivity parent)
        {
            Parent = parent;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Load the preferences from an XML resource
            AddPreferencesFromResource(Resource.Layout.preference_fragment);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = base.OnCreateView(inflater, container, savedInstanceState);
            view.SetBackgroundColor(new Android.Graphics.Color(120,120,120,255));
            return view;
        }


        public override bool OnPreferenceTreeClick(PreferenceScreen preferenceScreen, Preference preference)
        {
            Parent.RefreshAdapter();
            return base.OnPreferenceTreeClick(preferenceScreen, preference);
        }


    }
}