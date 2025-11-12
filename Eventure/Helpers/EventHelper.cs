namespace Eventure.Helpers
{
    public static class EventHelper
    {
        public static int CalculateRemainingDays(DateTime eventDate)
        {
            var today = DateTime.UtcNow.Date;
            var eventDay = eventDate.Date;

            int remaining = (eventDay - today).Days;
            return remaining < 0 ? 0 : remaining;
        }
    }
}