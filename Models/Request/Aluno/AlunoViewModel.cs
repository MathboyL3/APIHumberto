using APIHumberto.Models.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIHumberto.Models.Request.Aluno
{
	public class AlunoViewModel
	{
		[RAValidation]
		public string RA { get; set; }
		public string Nome { get; set; }
		[CPFValidation]
		public string CPF { get; set; }
		public string Email { get; set; }
		public bool Ativo { get; set; }
	}
}
