namespace FluentValidation.Results
{
    public static class FluentValidationResultExtensions
    {
        public static string ErrorMessage(this ValidationResult result)
        {
            if (result.IsValid)
            {
                return string.Empty;
            }
            return string.Join("\n", result.Errors);
        }
    }
}
