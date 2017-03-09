using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace $safeprojectname$.Validation
{
    public class StringValidationAttribute : ValidationAttribute
    {
        public StringValidationAttribute()
        {
            MaxLength = 256;
        }

        public int MaxLength { get; set; }
        public bool IsNulOrEmptyAllowed { get; set; }
        public bool IsAlphaNumeric { get; set; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var str = value as string;

            var isValid = IsNulOrEmptyAllowed || !string.IsNullOrEmpty(str);
            if (!isValid)
            {
                return new ValidationResult(string.Format("{0} should not be empty.", validationContext.ObjectInstance));
            }

            if (str != null)
            {
                var length = str.Length;
                isValid = length <= MaxLength;
                if (!isValid)
                {
                    return new ValidationResult(string.Format("Provided string is {0} chars length. Maximal length allowed is {1}.", length, MaxLength));
                }

                isValid = !IsAlphaNumeric || str.Replace("-", "").All(char.IsLetterOrDigit);
                if (!isValid)
                {
                    return new ValidationResult("Only alphanumeric characters or '-' are allowed.");
                }
            }

            return ValidationResult.Success;
        }
    }
}