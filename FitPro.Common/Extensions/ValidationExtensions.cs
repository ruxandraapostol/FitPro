using FluentValidation.Results;

namespace FitPro.Common
{
    public static class ValidationExtensions
    {
        public static void ThenThrow(this ValidationResult result)
        {
            if (!result.IsValid)
            {
                throw new ValidationErrorException(result);
            }
        }

        public static void ThenThrow(this ValidationResult result, object model)
        {
            if (!result.IsValid)
            {
                throw new ValidationErrorException(result, model);
            }
        }
    }
}
