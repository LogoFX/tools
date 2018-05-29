using System.ComponentModel.DataAnnotations;

namespace $safeprojectname$.Validation
{
    public class NumberValidation : ValidationAttribute
    {
        public NumberValidation()
        {
            Minimum = int.MinValue;
            Maximum = int.MaxValue;
        }

        public int Minimum { get; set; }

        public int Maximum { get; set; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var number = (int) value;

            if (number < Minimum || number > Maximum)
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
}