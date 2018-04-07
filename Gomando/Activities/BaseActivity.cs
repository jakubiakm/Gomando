using Android.App;
using Android.Widget;
using Android.OS;
using Android.Support.V7.App;
using Android.Support.V4.Widget;
using Android.Support.Design.Widget;
using Android.Content;

namespace Gomando.Activities
{
    [Activity(Label = "Gomando")]
    public class BaseActivity : AppCompatActivity
    {
        protected DrawerLayout mDrawer;
        protected NavigationView mNavigationView;
        protected Android.Support.V7.Widget.Toolbar mToolbar;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.navigation_drawer_base_layout);
            //SupportActionBar.SetHomeAsUpIndicator(Resource.Drawable.ic_menu_navigation_drawer);
            mToolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            mDrawer = FindViewById<DrawerLayout>(Resource.Id.navigation_drawer_layout);
            mNavigationView = FindViewById<NavigationView>(Resource.Id.navigation_view);
            mNavigationView.NavigationItemSelected += NavigationView_NavigationItemSelected;

            SetSupportActionBar(mToolbar);
        }
        private void NavigationView_NavigationItemSelected(object sender, NavigationView.NavigationItemSelectedEventArgs e)
        {
            e.MenuItem.SetChecked(true);
            switch (e.MenuItem.ItemId)
            {
                case (Resource.Id.menu_navigation_training):
                    var trainingIntent = new Intent(this, typeof(TrainingActivity))
                   .SetFlags(ActivityFlags.ReorderToFront);
                    StartActivity(trainingIntent);
                    break;
                case (Resource.Id.menu_navigation_training_history):
                    var trainingHistoryIntent = new Intent(this, typeof(TrainingHistoryActivity))
                   .SetFlags(ActivityFlags.ReorderToFront);
                    StartActivity(trainingHistoryIntent);
                    break;
                case (Resource.Id.menu_navigation_profile):
                    break;

            }
            mDrawer.CloseDrawers();
        }
    }
}

