namespace Application.Common.Statics;

public static class AppClaimTypes
{
    public static class Report
    {
        public static string Prefix = "Report-";
        public static class Create
        {
            public static string Prefix = Report.Prefix + "Create-";
            public static string ByCitizen = $"{Prefix}ByCitizen";
            public static string ByOperator = $"{Prefix}ByOperator";
        }
        public static class Read
        {
            public static string Prefix = Report.Prefix + "Read-";
            public static string All = $"{Prefix}All";
            public static string Details = $"{Prefix}Details"; //who have this, alse have access to report's citizen's details
            public static string History = $"{Prefix}History";
            public static string Comments = $"{Prefix}Comments";
            public static string Self = $"{Prefix}Self";
            //public static string SelfDetails = $"{Prefix}SelfDetails"; //....self
            //public static string Locations = $"{Prefix}Locations"; //....get nearest and recent
            public static string Recent = $"{Prefix}Recent";
            public static string Nearest = $"{Prefix}Nearest";
        }

        public static string Update = $"{Prefix}Update";
        public static string Like = $"{Prefix}Like";
        public static string Accept = $"{Prefix}Accept";
    }


    public static class Task
    {
        public static string Prefix = "Task-";
        public static string Read = $"{Prefix}Read";
        public static string MakeTransition = $"{Prefix}MakeTransition";
        public static string Review = $"{Prefix}Review";
        public static string MessageToCitizen = $"{Prefix}MessageToCitizen";
    }

    public static class Comments
    {
        public static string Prefix = "Comments-";
        public static string Read = $"{Prefix}Read"; //...All[in system]
        public static string Reply = $"{Prefix}Reply";
        public static string Update = $"{Prefix}Update";
        public static string Delete = $"{Prefix}Delete";
        public static string Create = $"{Prefix}Create";
    }

    public static class Account
    {
        public static string Prefix = "Account-";
        public static string Get = $"{Prefix}Get";
        public static string Update = $"{Prefix}Update";
    }

    public static class User
    {
        public static string Prefix = "User-";

        public static string Create = $"{Prefix}Create";
        public static class Read
        {
            public static string Prefix = User.Prefix + "Read-";
            public static string All = $"{Prefix}All";
            public static string Roles = $"{Prefix}Roles";
            public static string Regions = $"{Prefix}Regions";
            public static string Contractors = $"{Prefix}Contractors"; //....?? in admin 
        }
        public static class Update
        {
            public static string Prefix = User.Prefix + "Update-";
            public static string Details = $"{Prefix}Details";
            public static string Password = $"{Prefix}Password";
            public static string Roles = $"{Prefix}Roles";
            public static string Regions = $"{Prefix}Regions";
        }
    }

    public static class Contractor
    {
        public static string Prefix = "Contractor-";
        public static string Create = $"{Prefix}Crerate";
        public static string Get = $"{Prefix}Get";
    }

    public static class Info
    {
        public static string Prefix = "Info-";
        public static string Read = $"{Prefix}Read";
    }

    public static class Common
    {
        public static string Prefix = "Common-";
        public static class Read
        {
            public static string Prefix = Common.Prefix + "Read-";
            public static string Executives = $"{Prefix}Executives";
        }
    }


    public static class Category
    {
        public static string Prefix = "Category-";
        public static string Read = $"{Prefix}Read";   //need? all users can read categories
        public static string Manage = $"{Prefix}Manage";
    }

    public static class Processes
    {
        public static string Prefix = "Processes-";
        public static string Read = $"{Prefix}Read";
        public static string Manage = $"{Prefix}Manage";
    }

    public static class OrganizationalUnit
    {
        public static string Prefix = "OrganizationalUnit-";
        public static string Read = $"{Prefix}Read";
        public static string Manage = $"{Prefix}MAnage";
    }

    public static class File
    {
        public static string Prefix = "File-";
        public static string Upload = $"{Prefix}Upload";
    }

    public static class QuickAccess
    {
        public static string Prefix = "QuickAccess-";
        public static class Read
        {
            public static string Prefix = QuickAccess.Prefix + "Read-";
            public static string ByAdmin = $"{Prefix}ByAdmin";
            public static string ByCitizen = $"{Prefix}ByCitizen";
        }
        public static string Manage = $"{Prefix}Manage";
    }

    public static class News
    {
        public static string Prefix = "News-";
        public static class Read
        {
            public static string Prefix = QuickAccess.Prefix + "Read-";
            public static string ByAdmin = $"{Prefix}ByAdmin";
            public static string ByCitizen = $"{Prefix}ByCitizen";
        }
        public static string Manage = $"{Prefix}Manage";
    }

    public static class Polls
    {
        public static string Prefix = "Polls-";

        public static string Manage = $"{Prefix}Manage";
        public static class Read
        {
            public static string Prefix = Polls.Prefix + "Read-";
            public static string ByAdmin = $"{Prefix}All";
            public static string ByCitizen = $"{Prefix}Details";
            public static string Summary = $"{Prefix}Summary";
        }
        public static string Answer = $"{Prefix}Answer";
    }


    public static class Messages
    {
        public static string Prefix = "Messages-";
        public static string Read = $"{Prefix}Read";
    }

    public static class Feedback
    {
        public static string Prefix = "Feedback-";
        public static string Create = $"{Prefix}Create";
        public static string Read = $"{Prefix}Read";
    }

    public static class Satisfaction
    {
        public static string Prefix = "Satisfaction-";
        public static string Update = $"{Prefix}Update";
        public static string Read = $"{Prefix}Read";
    }

    public static class Violation
    {
        public static string Prefix = "Violation-";
        public static string Read = $"{Prefix}Read";
        public static string Update = $"{Prefix}Update";
        public static class Create
        {
            public static string Prefix = Violation.Prefix + "Create-";
            public static string ForReport = $"{Prefix}ForReport";
            public static string ForComment = $"{Prefix}ForComment";
        }
    }

}
