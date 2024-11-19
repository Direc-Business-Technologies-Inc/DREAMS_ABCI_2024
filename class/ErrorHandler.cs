namespace ABROWN_DREAMS
{
    public static class ErrorHandler
    {
        static SAPHanaAccess _hana = _hana ?? new SAPHanaAccess();
        static string _conStr = _hana?.GetConnection("SAOHana");
        /// <summary>
        /// Log all the error that encounter to the whole system
        /// </summary>
        /// <param name="userId">(int)Session["UserID"]</param>
        /// <param name="logModule">Module encountered.</param>
        /// <param name="errorSeverity">"Low", "Medium", "High", or "Critical".</param>
        /// <param name="errorMessage">This is assumed to be a string and can be up to 5000 characters long.</param>
        /// <param name="additionalInfo">This could include things like the function where the error occurred, the input that caused the error, etc.</param>
        /// <returns></returns>
        public static bool Log(string userId, string logModule, string errorSeverity, string errorMessage, string additionalInfo)
        {//INSERT INTO ErrorLog (User, ErrorSeverity, ErrorMessage, AdditionalInfo) VALUES (@User, @ErrorSeverity, @ErrorMessage, @AdditionalInfo)
            return _hana.Execute($"INSERT INTO LOGS (USERID,LOGMODULE, ERRORSEVERITY, ERRORMESSAGE, ADDITIONALINFO) VALUES ({(userId == null ? "0" : userId)},'{logModule}','{errorSeverity}', '{errorMessage}', '{additionalInfo}')", _conStr);
        }
    }

}