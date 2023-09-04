using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace APIHumberto.Models.Validation
{
	public class CPFValidationAttribute : ValidationAttribute
	{
		protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
		{
			string cpf_no_dots = value.ToString().Replace(".", "").Replace("-", "");

			foreach(var digit in cpf_no_dots)
				if (!char.IsDigit(digit)) return new ValidationResult("O cpf deve possuir apenas digitos");

			if (cpf_no_dots.Length != 11) return new ValidationResult("Confirme se o cpf foi digitado corretamente, ele deve ter 11 digitos");

			return ValidationResult.Success;
		}

	}
}
