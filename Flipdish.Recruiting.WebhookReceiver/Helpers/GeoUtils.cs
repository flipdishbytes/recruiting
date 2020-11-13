
using Flipdish.Recruiting.WebhookReceiver.Models;
using System;
using System.Globalization;

namespace Flipdish.Recruiting.WebhookReceiver.Helpers
{
    public static class GeoUtils
    {
        public static string GetDynamicMapUrl(double centerLatitude, double centerLongitude, int zoom)
        {

            // latitude
            string direction;
            double absoluteValue;
            if (centerLatitude < 0)
            {
                direction = "S";
                absoluteValue = -centerLatitude;
            }
            else
            {
                direction = "N";
                absoluteValue = centerLatitude;
            }

            string dmsLatitude = GetDms(absoluteValue) + direction;

            // longitude
            if (centerLongitude < 0)
            {
                direction = "W";
                absoluteValue = -centerLongitude;
            }
            else
            {
                direction = "E";
                absoluteValue = centerLongitude;
            }

            string dmsLongitude = GetDms(absoluteValue) + direction;

            string url = string.Format("https://www.google.ie/maps/place/{0}+{1}/@{2},{3},{4}z", dmsLatitude, dmsLongitude, centerLatitude, centerLongitude, zoom);
            return url;
        }
        private static string GetDms(double value)
        {
            double decimalDegrees = (double)value;
            double degrees = Math.Floor(decimalDegrees);
            double minutes = (decimalDegrees - Math.Floor(decimalDegrees)) * 60.0;
            double seconds = (minutes - Math.Floor(minutes)) * 60.0;
            double tenths = (seconds - Math.Floor(seconds)) * 1000.0;
            // get rid of fractional part
            minutes = Math.Floor(minutes);
            seconds = Math.Floor(seconds);
            tenths = Math.Floor(tenths);

            string result = string.Format("{0}°{1}'{2}.{3}\"", degrees, minutes, seconds, tenths);

            return result;
        }

        public static string GetStaticMapUrl(double centerLatitude, double centerLongitude, int zoom, double? markerLatitude,  double? markerLongitude, int width = 1200, int height = 1200)
        {

            string googleStaticMapsApiKey = SettingsService.Google_StaticMapsApiKey;

            string keyString = string.IsNullOrWhiteSpace(googleStaticMapsApiKey) ? "" : "&key=" + googleStaticMapsApiKey;
            string markerLatitudeStr = markerLatitude.HasValue ? markerLatitude.Value.ToString(CultureInfo.InvariantCulture) : "0";
            string markerLongitudeStr = markerLongitude.HasValue ? markerLongitude.Value.ToString(CultureInfo.InvariantCulture) : "0";

            const string mapBaseUri = "https://maps.googleapis.com/maps/api/staticmap?center={0},{1}&scale=2&zoom={2}&size={6}x{7}&format=png32&scale=1&maptype=roadmap&markers=size:mid|{3},{4}{5}";

            string mapFullUri = string.Format(mapBaseUri, centerLatitude.ToString(CultureInfo.InvariantCulture), centerLongitude.ToString(CultureInfo.InvariantCulture),
                zoom.ToString(CultureInfo.InvariantCulture), markerLatitudeStr,
                markerLongitudeStr, keyString, width.ToString(CultureInfo.InvariantCulture),
                height.ToString(CultureInfo.InvariantCulture));

            return mapFullUri;
        }

        public static double GetAirDistance(Coordinates aCoords, Coordinates bCoords)
        {
            var lat1 = aCoords.Latitude.Value;
            var lat2 = bCoords.Latitude.Value;
            var lon1 = aCoords.Longitude.Value;
            var lon2 = bCoords.Longitude.Value;
            if ((lat1 == lat2) && (lon1 == lon2))
            {
                return 0;
            }
            else
            {
                double theta = lon1 - lon2;
                double dist = Math.Sin(deg2rad(lat1)) * Math.Sin(deg2rad(lat2)) + Math.Cos(deg2rad(lat1)) * Math.Cos(deg2rad(lat2)) * Math.Cos(deg2rad(theta));
                dist = Math.Acos(dist);
                dist = rad2deg(dist);
                dist = dist * 60 * 1.1515;

                dist = dist * 1.609344;
                /*if (unit == 'K')
                {
                    dist = dist * 1.609344;
                }
                else if (unit == 'N')
                {
                    dist = dist * 0.8684;
                }*/
                return (dist);
            }
        }

        private static double deg2rad(double deg)
        {
            return (deg * Math.PI / 180.0);
        }

        private static double rad2deg(double rad)
        {
            return (rad / Math.PI * 180.0);
        }
    }
}
