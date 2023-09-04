using System.ComponentModel.DataAnnotations;
using System.Data;

namespace APIHumberto.Models.Validation
{
	public class RAValidationAttribute : ValidationAttribute
	{
		protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
		{
			if(value == null)
				return new ValidationResult("RA nulo");

			string ra = (string)value;
			if (ra.Length != 8) 
				return new ValidationResult("Deve ter 8 dígitos");

			if (char.ToLower(ra[0]) != 'r' || char.ToLower(ra[1]) != 'a') 
				return new ValidationResult("O ra deve começar com as siglas RA + XXXXXX.");

			if (!char.IsDigit(ra[2]) ||
				!char.IsDigit(ra[3]) ||
				!char.IsDigit(ra[4]) ||
				!char.IsDigit(ra[5]) ||
				!char.IsDigit(ra[6]) ||
				!char.IsDigit(ra[7]))
				return new ValidationResult("O RA deve ter 6 dígitos após a sigla RA.");

			return ValidationResult.Success;
		}
	}
}
