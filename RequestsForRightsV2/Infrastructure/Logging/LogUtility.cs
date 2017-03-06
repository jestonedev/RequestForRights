using System;
using System.Web;

namespace RequestsForRights.Web.Infrastructure.Logging
{
    public class LogUtility
    {

        public static string BuildExceptionMessage(Exception x)
        {
            var logException = x;
            if (x.InnerException != null)
                logException = x.InnerException;
            var strErrorMsg = Environment.NewLine + "Error in Path : " + HttpContext.Current.Request.Path;
            strErrorMsg += Environment.NewLine + "Raw Url : " + HttpContext.Current.Request.RawUrl;
            strErrorMsg += Environment.NewLine + "Message : " + logException.Message;
            strErrorMsg += Environment.NewLine + "Source : " + logException.Source;
            strErrorMsg += Environment.NewLine + "Stack Trace : " + logException.StackTrace;
            strErrorMsg += Environment.NewLine + "TargetSite : " + logException.TargetSite;
            return strErrorMsg;
        }
    }
}