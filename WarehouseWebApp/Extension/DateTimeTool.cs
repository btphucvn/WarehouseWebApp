namespace EcommerceWebsite.Extension
{
    public static class DateTimeTool
    {
        public static double GetTimeStampNow()
        {
             return (double)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;

        }
    }
}
