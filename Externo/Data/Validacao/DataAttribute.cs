using Externo.Util;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Externo.Data.Validacao
{
    public class MyDataAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {

            if (DateTime.TryParseExact(value?.ToString(), "yyyy-MM",
            CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime consulta))
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(Erro.DataMsg);
        }
    }
}
