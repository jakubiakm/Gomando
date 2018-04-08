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

namespace Gomando.Activities
{
    [Activity(Label = "Trening", MainLauncher = true)]
    public class TrainingActivity : BaseActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            mNavigationView.SetCheckedItem(Resource.Id.menu_navigation_training);

            LayoutInflater inflater = (LayoutInflater)this.GetSystemService(Context.LayoutInflaterService);
            View contentView = inflater.Inflate(Resource.Layout.training_layout, null, false);
            mDrawer.AddView(contentView, 0);


        }

        protected override void OnResume()
        {
            base.OnResume();
            mNavigationView.SetCheckedItem(Resource.Id.menu_navigation_training);
        }
    }
}