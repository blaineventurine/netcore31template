using System;
using System.Text;

namespace Common.Extensions
{
    public static class ExceptionExtensions
    {
        public static string GetErrorDetails(this Exception ex)
        {
            var stb = new StringBuilder();

            while (ex != null)
            {
                stb.Append("Exception Message: ")
                    .Append(ex.Message)
                    .Append(" Exception Type: ")
                    .Append(ex.GetType())
                    .Append(" StackTrace: ")
                    .Append(ex.StackTrace);

                ex = ex.InnerException;
            }

            return stb.ToString();
        }
    }
}
