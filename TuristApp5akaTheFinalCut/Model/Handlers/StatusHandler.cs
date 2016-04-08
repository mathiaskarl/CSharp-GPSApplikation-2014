namespace TuristApp5akaTheFinalCut.Model.Handlers
{
    class StatusHandler
    {
        private static string _returnStatusMessage;
        public static string ReturnStatusMessage(object statusKey)
        {
            RetrieveStatusMessage(statusKey);
            return _returnStatusMessage;
        }

        private static void RetrieveStatusMessage(object statusKey)
        {
            if (statusKey.GetType() != typeof (string))
                statusKey = "INVALID_STATUSMESSAGE_INPUT";
            switch (statusKey.ToString())
            {
                case "INVALID LOCATION SELECTION":
                    _returnStatusMessage = "PLEASE CHOOSE A VALID LOCATION";
                    break;
                case "INVALID_STATUSMESSAGE_INPUT":
                    _returnStatusMessage = "THE RETURNED MESSAGE IS NOT OF THE TYPE STRING";
                    break;
                default:
                    _returnStatusMessage = "STATUSERROR DOES NOT EXIST";
                    break;
            }
        }
    }
}
