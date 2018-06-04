using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gomando.Logic.Helpers
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

        public static double HaversineDistance(double lat1, double long1, double lat2, double long2)
        {
            double R = 6371e3;

            var lat = (lat2 - lat1).ToRadian();
            var lng = (long2 - long1).ToRadian();

            var h1 = Math.Sin(lat / 2) * Math.Sin(lat / 2) +
                     Math.Cos(lat1.ToRadian()) * Math.Cos(lat2.ToRadian()) *
                     Math.Sin(lng / 2) * Math.Sin(lng / 2);

            var h2 = 2 * Math.Atan2(Math.Sqrt(h1), Math.Sqrt(1 - h1));

            return HaversineInKiloMeters(lat1, long1, lat2, long2);
        }
        static double _eQuatorialEarthRadius = 6378.1370D;
        static double _d2r = (Math.PI / 180D);

        private static double HaversineInKiloMeters(double lat1, double long1, double lat2, double long2)
        {
            double dlong = (long2 - long1) * _d2r;
            double dlat = (lat2 - lat1) * _d2r;
            double a = Math.Pow(Math.Sin(dlat / 2D), 2D) + Math.Cos(lat1 * _d2r) * Math.Cos(lat2 * _d2r) * Math.Pow(Math.Sin(dlong / 2D), 2D);
            double c = 2D * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1D - a));
            double d = _eQuatorialEarthRadius * c;

            return d;
        }

        public static double ToRadian(this double value)
        {
            return (Math.PI / 180) * value;
        }
    }
}
