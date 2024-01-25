using System.ComponentModel.DataAnnotations;

namespace apiAutores.Validations
{
    public class FirstCharUpperAttribute: ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString())) return ValidationResult.Success;

            var firstChar = value!.ToString()?[0];

            if (firstChar == null) return ValidationResult.Success;

            char secureChar = firstChar.Value;

            if (secureChar != char.ToUpper(secureChar))
            {
                return new ValidationResult("La primera letra debe ser mayúscula");
            }

            return ValidationResult.Success;


        }
    }
}
