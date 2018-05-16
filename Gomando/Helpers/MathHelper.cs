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

namespace Gomando.Helpers
{
    public static class MathHelper
    {
        public static string ConvertSecondsToTimeString(int seconds)
        {
            TimeSpan ts = new TimeSpan(0, 0, seconds);
            if (ts.Hours > 0)
                return string.Format("{0:0}:{1:00}:{2:00}", ts.Hours, ts.Minutes, ts.Seconds);
            if (ts.Minutes > 0)
                return string.Format("{0:0}:{1:00}", ts.Minutes, ts.Seconds);
            return string.Format("{0:0}", ts.Seconds);
        }
    }
}