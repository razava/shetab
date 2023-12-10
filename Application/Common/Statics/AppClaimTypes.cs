namespace Application.Common.Statics;

public static class AppClaimTypes
{
    public static class Report
    {
        public static string Prefix = "Report-";
        public static string CreateCitizen = $"{Prefix}CreateCitizen";
        public static string CreateOperator = $"{Prefix}CreateOperator";
        public static class Read
        {
            public static string Prefix = Report.Prefix + "Read-";
            public static string All = $"{Prefix}All";
            public static string Self = $"{Prefix}Self";

        }
    }
}
