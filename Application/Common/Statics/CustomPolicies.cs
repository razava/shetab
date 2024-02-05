using Microsoft.AspNetCore.Authorization;

namespace Application.Common.Statics;   

public static class CustomPolicies
{
    public static void AddPolicies(AuthorizationOptions options)
    {
        string claimStr;

        //Report
        claimStr = AppClaimTypes.Report.Create.ByCitizen;
        options.AddPolicy(claimStr, policy => policy.RequireClaim(claimStr));
        claimStr = AppClaimTypes.Report.Create.ByOperator;
        options.AddPolicy(claimStr, policy => policy.RequireClaim(claimStr));

        claimStr = AppClaimTypes.Report.Read.All;
        options.AddPolicy(claimStr, policy => policy.RequireClaim(claimStr));
        claimStr = AppClaimTypes.Report.Read.Details;
        options.AddPolicy(claimStr, policy => policy.RequireClaim(claimStr));
        claimStr = AppClaimTypes.Report.Read.History;
        options.AddPolicy(claimStr, policy => policy.RequireClaim(claimStr));
        claimStr = AppClaimTypes.Report.Read.Comments;
        options.AddPolicy(claimStr, policy => policy.RequireClaim(claimStr));
        claimStr = AppClaimTypes.Report.Read.Self;
        options.AddPolicy(claimStr, policy => policy.RequireClaim(claimStr));
        claimStr = AppClaimTypes.Report.Read.Recent;
        options.AddPolicy(claimStr, policy => policy.RequireClaim(claimStr));
        claimStr = AppClaimTypes.Report.Read.Nearest;
        options.AddPolicy(claimStr, policy => policy.RequireClaim(claimStr));

        claimStr = AppClaimTypes.Report.Update;
        options.AddPolicy(claimStr, policy => policy.RequireClaim(claimStr));
        claimStr = AppClaimTypes.Report.Like;
        options.AddPolicy(claimStr, policy => policy.RequireClaim(claimStr));
        claimStr = AppClaimTypes.Report.Accept;
        options.AddPolicy(claimStr, policy => policy.RequireClaim(claimStr));


        //Task
        claimStr = AppClaimTypes.Task.Read;
        options.AddPolicy(claimStr, policy => policy.RequireClaim(claimStr));
        claimStr = AppClaimTypes.Task.MakeTransition;
        options.AddPolicy(claimStr, policy => policy.RequireClaim(claimStr));
        claimStr = AppClaimTypes.Task.Review;
        options.AddPolicy(claimStr, policy => policy.RequireClaim(claimStr));
        claimStr = AppClaimTypes.Task.MessageToCitizen;
        options.AddPolicy(claimStr, policy => policy.RequireClaim(claimStr));


        //Comments
        claimStr = AppClaimTypes.Comments.Read;
        options.AddPolicy(claimStr, policy => policy.RequireClaim(claimStr));
        claimStr = AppClaimTypes.Comments.Reply;
        options.AddPolicy(claimStr, policy => policy.RequireClaim(claimStr));
        claimStr = AppClaimTypes.Comments.Update;
        options.AddPolicy(claimStr, policy => policy.RequireClaim(claimStr));
        claimStr = AppClaimTypes.Comments.Delete; ;
        options.AddPolicy(claimStr, policy => policy.RequireClaim(claimStr));
        claimStr = AppClaimTypes.Comments.Create;
        options.AddPolicy(claimStr, policy => policy.RequireClaim(claimStr));


        //Account
        claimStr = AppClaimTypes.Account.Get;
        options.AddPolicy(claimStr, policy => policy.RequireClaim(claimStr));

        claimStr = AppClaimTypes.Account.Update.Profile;
        options.AddPolicy(claimStr, policy => policy.RequireClaim(claimStr));
        claimStr = AppClaimTypes.Account.Update.Avatear;
        options.AddPolicy(claimStr, policy => policy.RequireClaim(claimStr));
        claimStr = AppClaimTypes.Account.Update.Password;
        options.AddPolicy(claimStr, policy => policy.RequireClaim(claimStr));


        //User
        claimStr = AppClaimTypes.User.Create;
        options.AddPolicy(claimStr, policy => policy.RequireClaim(claimStr));

        claimStr = AppClaimTypes.User.Read.All;
        options.AddPolicy(claimStr, policy => policy.RequireClaim(claimStr));
        claimStr = AppClaimTypes.User.Read.Roles;
        options.AddPolicy(claimStr, policy => policy.RequireClaim(claimStr));
        claimStr = AppClaimTypes.User.Read.Regions;
        options.AddPolicy(claimStr, policy => policy.RequireClaim(claimStr));
        claimStr = AppClaimTypes.User.Read.Contractors;
        options.AddPolicy(claimStr, policy => policy.RequireClaim(claimStr));

        claimStr = AppClaimTypes.User.Update.Details;
        options.AddPolicy(claimStr, policy => policy.RequireClaim(claimStr));
        claimStr = AppClaimTypes.User.Update.Password;
        options.AddPolicy(claimStr, policy => policy.RequireClaim(claimStr));
        claimStr = AppClaimTypes.User.Update.Roles;
        options.AddPolicy(claimStr, policy => policy.RequireClaim(claimStr));
        claimStr = AppClaimTypes.User.Update.Regions;
        options.AddPolicy(claimStr, policy => policy.RequireClaim(claimStr));


        //Contractor
        claimStr = AppClaimTypes.Contractor.Create;
        options.AddPolicy(claimStr, policy => policy.RequireClaim(claimStr));
        claimStr = AppClaimTypes.Contractor.Get;
        options.AddPolicy(claimStr, policy => policy.RequireClaim(claimStr));


        //Info
        claimStr = AppClaimTypes.Info.Read.Charts;
        options.AddPolicy(claimStr, policy => policy.RequireClaim(claimStr));
        claimStr = AppClaimTypes.Info.Read.Summary;
        options.AddPolicy(claimStr, policy => policy.RequireClaim(claimStr));
        claimStr = AppClaimTypes.Info.Read.Locations;
        options.AddPolicy(claimStr, policy => policy.RequireClaim(claimStr));
        claimStr = AppClaimTypes.Info.Read.Excel;
        options.AddPolicy(claimStr, policy => policy.RequireClaim(claimStr));


        //Common
        claimStr = AppClaimTypes.Common.Read.Executives;
        options.AddPolicy(claimStr, policy => policy.RequireClaim(claimStr));


        //Category
        claimStr = AppClaimTypes.Category.Read;
        options.AddPolicy(claimStr, policy => policy.RequireClaim(claimStr));
        claimStr = AppClaimTypes.Category.Manage;
        options.AddPolicy(claimStr, policy => policy.RequireClaim(claimStr));


        //Process
        claimStr = AppClaimTypes.Processes.Read.All;
        options.AddPolicy(claimStr, policy => policy.RequireClaim(claimStr));
        claimStr = AppClaimTypes.Processes.Read.Details;
        options.AddPolicy(claimStr, policy => policy.RequireClaim(claimStr));

        claimStr = AppClaimTypes.Processes.Create;
        options.AddPolicy(claimStr, policy => policy.RequireClaim(claimStr));
        claimStr = AppClaimTypes.Processes.Update;
        options.AddPolicy(claimStr, policy => policy.RequireClaim(claimStr));


        //Organizational Unit
        claimStr = AppClaimTypes.OrganizationalUnit.Read.All;
        options.AddPolicy(claimStr, policy => policy.RequireClaim(claimStr));
        claimStr = AppClaimTypes.OrganizationalUnit.Read.Self;
        options.AddPolicy(claimStr, policy => policy.RequireClaim(claimStr));
        claimStr = AppClaimTypes.OrganizationalUnit.Read.Details;
        options.AddPolicy(claimStr, policy => policy.RequireClaim(claimStr));

        claimStr = AppClaimTypes.OrganizationalUnit.Create;
        options.AddPolicy(claimStr, policy => policy.RequireClaim(claimStr));
        claimStr = AppClaimTypes.OrganizationalUnit.Update;
        options.AddPolicy(claimStr, policy => policy.RequireClaim(claimStr));


        //File
        claimStr = AppClaimTypes.File.Upload;
        options.AddPolicy(claimStr, policy => policy.RequireClaim(claimStr));


        //QuickAccess
        claimStr = AppClaimTypes.QuickAccess.Read.ByAdmin;
        options.AddPolicy(claimStr, policy => policy.RequireClaim(claimStr));
        claimStr = AppClaimTypes.QuickAccess.Read.ByCitizen;
        options.AddPolicy(claimStr, policy => policy.RequireClaim(claimStr));

        claimStr = AppClaimTypes.QuickAccess.Manage;
        options.AddPolicy(claimStr, policy => policy.RequireClaim(claimStr));


        //News
        claimStr = AppClaimTypes.News.Read.ByAdmin;
        options.AddPolicy(claimStr, policy => policy.RequireClaim(claimStr));
        claimStr = AppClaimTypes.News.Read.ByCitizen;
        options.AddPolicy(claimStr, policy => policy.RequireClaim(claimStr));

        claimStr = AppClaimTypes.News.Manage;
        options.AddPolicy(claimStr, policy => policy.RequireClaim(claimStr));


        //Polls
        claimStr = AppClaimTypes.Polls.Manage;
        options.AddPolicy(claimStr, policy => policy.RequireClaim(claimStr));

        claimStr = AppClaimTypes.Polls.Read.ByAdmin;
        options.AddPolicy(claimStr, policy => policy.RequireClaim(claimStr));
        claimStr = AppClaimTypes.Polls.Read.ByCitizen;
        options.AddPolicy(claimStr, policy => policy.RequireClaim(claimStr));
        claimStr = AppClaimTypes.Polls.Read.Summary;
        options.AddPolicy(claimStr, policy => policy.RequireClaim(claimStr));

        claimStr = AppClaimTypes.Polls.Answer;
        options.AddPolicy(claimStr, policy => policy.RequireClaim(claimStr));


        //Messages
        claimStr = AppClaimTypes.Messages.Read.All;
        options.AddPolicy(claimStr, policy => policy.RequireClaim(claimStr));
        claimStr = AppClaimTypes.Messages.Read.Count;
        options.AddPolicy(claimStr, policy => policy.RequireClaim(claimStr));


        //Map
        claimStr = AppClaimTypes.Map.Read.Forward;
        options.AddPolicy(claimStr, policy => policy.RequireClaim(claimStr));
        claimStr = AppClaimTypes.Map.Read.Backward;
        options.AddPolicy(claimStr, policy => policy.RequireClaim(claimStr));


        //Feedback
        claimStr = AppClaimTypes.Feedback.Create;
        options.AddPolicy(claimStr, policy => policy.RequireClaim(claimStr));


        //Satisfaction
        claimStr = AppClaimTypes.Satisfaction.Update;
        options.AddPolicy(claimStr, policy => policy.RequireClaim(claimStr));


        //Violation
        claimStr = AppClaimTypes.Violation.Read;
        options.AddPolicy(claimStr, policy => policy.RequireClaim(claimStr));
        claimStr = AppClaimTypes.Violation.Update;
        options.AddPolicy(claimStr, policy => policy.RequireClaim(claimStr));

        claimStr = AppClaimTypes.Violation.Create.ForReport;
        options.AddPolicy(claimStr, policy => policy.RequireClaim(claimStr));
        claimStr = AppClaimTypes.Violation.Create.ForComment;
        options.AddPolicy(claimStr, policy => policy.RequireClaim(claimStr));

    }
}
