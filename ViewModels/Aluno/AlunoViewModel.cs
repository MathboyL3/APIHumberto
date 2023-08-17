using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIHumberto.ViewModels.Aluno
{
	public class AlunoViewModel
	{
		public string RA { get; set; }
		public string Nome { get; set; }
		public string CPF { get; set; }
		public string Email { get; set; }
		public bool Ativo { get; set; }
	}
}
