using Android.App;
using Android.Widget;
using Android.OS;
using Android.Support.V7.App;
using Android.Support.V4.Widget;
using Android.Support.Design.Widget;
using Android.Content;
using Auth0.OidcClient;
using IdentityModel.OidcClient;

namespace Gomando.Activities
{
    [Activity(Label = "Gomando")]
    public class BaseActivity : AppCompatActivity
    {
        protected DrawerLayout mDrawer;
        protected NavigationView mNavigationView;
        protected Android.Support.V7.Widget.Toolbar mToolbar;

        //Auth0 authorization properties
        internal Auth0Client client;
        internal AuthorizeState authorizeState;

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
        private async void NavigationView_NavigationItemSelected(object sender, NavigationView.NavigationItemSelectedEventArgs e)
        {
            e.MenuItem.SetChecked(true);
           
            mDrawer.CloseDrawers();
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
                    client = new Auth0Client(new Auth0ClientOptions
                    {
                        Domain = "gomando.eu.auth0.com",
                        ClientId = "ipNMZdU7KW6acYYEbQLTMqGR5BP4FheO",
                        Activity = this
                    });


                    authorizeState = await client.PrepareLoginAsync();

                    var uri = Android.Net.Uri.Parse(authorizeState.StartUrl);
                    var intent = new Intent(Intent.ActionView, uri);
                    intent.AddFlags(ActivityFlags.NoHistory);
                    StartActivity(intent);
                    break;

            }
        }
    }
}

////TODO:
//- po n klikniecia znikaja jednopstki
//- w prawym dolnym rogu dwa przyciski przy usuwaniu zamiast z prawej i lewej