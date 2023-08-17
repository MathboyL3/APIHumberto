using APIHumberto.ViewModels.Aluno;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace APIHumberto.Controllers
{
	[Route("api/[controller]")]
	public class AlunosController : ControllerBase
	{
		private const string CaminhoArquivoAlunos = @".\Data\";
		private const string NomeArquivoAlunos = "alunos.json";

		#region operações de arquivos

		private List<AlunoViewModel> LerAlunosDB()
		{
			List<AlunoViewModel> alunos;
			string caminho_completo = Path.Combine(CaminhoArquivoAlunos, NomeArquivoAlunos);
			FileInfo arquivo_FI = new FileInfo(caminho_completo);

			if (!arquivo_FI.Exists)
				arquivo_FI.Create().Close();

			string json = System.IO.File.ReadAllText(caminho_completo);
			alunos = JsonConvert.DeserializeObject<List<AlunoViewModel>>(json);
			return alunos == null ? new List<AlunoViewModel>() : alunos;
		}

		private void GuardarAlunosDB(List<AlunoViewModel> alunos)
		{
			string json = JsonConvert.SerializeObject(alunos);
			string caminho_completo = Path.Combine(CaminhoArquivoAlunos, NomeArquivoAlunos);

			FileInfo arquivo_FI = new FileInfo(caminho_completo);

			if (!arquivo_FI.Exists)
				arquivo_FI.Create().Close();

			System.IO.File.WriteAllText(caminho_completo, json);
		}

		#endregion

		//CRUD - Create Read Update Delete
		#region operações CRUD

		[HttpGet]
		public IActionResult Get()
		{
			List<AlunoViewModel> alunos = LerAlunosDB();
			return Ok(alunos);
		}

		[HttpGet("{ra}")]
		public IActionResult Get(string ra)
		{
			List<AlunoViewModel> alunos = LerAlunosDB();
			AlunoViewModel aluno_selecionado = alunos.Find(aluno => aluno.RA.Equals(ra));

			return aluno_selecionado == null || string.IsNullOrEmpty(aluno_selecionado.RA) ? NotFound("RA inexistente") : Ok(aluno_selecionado);
		}

		[HttpPost]
		public IActionResult Create([FromBody] AlunoViewModel aluno)
		{
			if (aluno == null || string.IsNullOrEmpty(aluno.RA))
				return BadRequest("Objeto não é válio");

			List<AlunoViewModel> alunos = LerAlunosDB();
			if (alunos.Where(a => a.RA.Equals(aluno.RA)).Count() > 0)
				return BadRequest("Já possui um aluno com essee RA");

			alunos.Add(aluno);
			GuardarAlunosDB(alunos);
			return Ok();
		}

		[HttpPut("{ra}")]
		public IActionResult Put(string ra, [FromBody] AlunoViewModel aluno)
		{
			List<AlunoViewModel> alunos = LerAlunosDB();
			if(alunos.Where(a => a.RA.Equals(ra)).Count() < 1) return NotFound("RA inexistente");
			alunos = alunos.Select(a => a.RA.Equals(ra) ? aluno : a).ToList();
			GuardarAlunosDB(alunos);
			return Ok();
		}

		[HttpDelete("{ra}")]
		public IActionResult Delete(string ra)
		{
			List<AlunoViewModel> alunos = LerAlunosDB();
			for(int i = 0; i < alunos.Count; i++)
			{
				AlunoViewModel aluno = alunos[i];
				if (aluno.RA.Equals(ra))
				{
					alunos.RemoveAt(i);
					GuardarAlunosDB(alunos);
					return NoContent();
				}
			}
			return NotFound();
		}

		#endregion
	}
}
