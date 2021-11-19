using FluentValidation.Results;
using System;

namespace FitPro.Common
{
    public class ValidationErrorException : Exception
    {
        public readonly ValidationResult ValidationResult;
        public readonly object Model;

        public ValidationErrorException(ValidationResult result, object model)
        {
            ValidationResult = result;
            Model = model;
        }

        public ValidationErrorException(ValidationResult result)
        {
            ValidationResult = result;
        }
    }
}
