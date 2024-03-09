using System.Globalization;

namespace SharedKernel.ExtensionMethods;

public static class DateTimeExtensionMethods
{
    public static string GregorianToPersian(this DateTime dateTime)
    {
        dateTime = dateTime.ToLocalTime();
        PersianCalendar pc = new PersianCalendar();
        return string.Format("{0}/{1}/{2}", pc.GetYear(dateTime), pc.GetMonth(dateTime), pc.GetDayOfMonth(dateTime));
    }
}
