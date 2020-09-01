namespace NakedBank.Shared
{
    public class BusinessError
    {
        public BusinessError(string fieldName, string message)
        {
            FieldName = fieldName;
            Message = message;
        }

        public string FieldName { get; set; }

        public string Message { get; set; }
    }
}
