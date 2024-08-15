namespace GestionDesConges.Data.Providers
{
    public static class HolidayProvider
    {
        public static List<DateTime> GetHolidays()
        {
            return new List<DateTime>
        {
            new DateTime(DateTime.Now.Year, 1, 1),  // New Year's Day
            new DateTime(DateTime.Now.Year, 7, 4),  // Independence Day
            // Add other holidays here...
        };
        }
    }
}
