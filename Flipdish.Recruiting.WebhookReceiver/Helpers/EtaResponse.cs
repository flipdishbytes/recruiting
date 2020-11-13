using System;

namespace Flipdish.Recruiting.WebhookReceiver.Helpers
{
    public enum EtaResponse
    {
        None = 0,
        InMinutes = 1,
        TodayAt = 2,
        TomorrowAt = 3,
        OnDayAt = 4,
        AtDateTime = 5,
        OnDay = 6
    }

    public static class EtaResponseMethods
    {

        // 19 Feb
        // Feb 19
        public static string GetDateString(DateTime requestedTime)
        {
            return requestedTime.ToString($"dd MMM");
        }

        // 14:00
        // 2:00 PM
        public static string GetClocksToString(DateTime requestedTime)
        {
            return requestedTime.ToString("HH:mm");
        }
    }
}
