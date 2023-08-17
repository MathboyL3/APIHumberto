using APIHumberto.ViewModels.Produto;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IO;

namespace APIHumberto.Controllers
{
    [Route("api/[controller]")]
	[ApiController]
	public class ProdutoController : ControllerBase
	{
		private readonly string CaminhoProdutosJson;

		public ProdutoController()
		{
			CaminhoProdutosJson = Path.Combine(Directory.GetCurrentDirectory(), "Data", "produtos.json");
		}


		#region Metodos Arquivo
		private List<ProdutoViewModel> LerArquivoProdutos()
		{
			FileInfo produtos_json = new FileInfo(CaminhoProdutosJson);
			if (!produtos_json.Exists)
				produtos_json.Create().Close();

			string sjson = System.IO.File.ReadAllText(CaminhoProdutosJson);
			List<ProdutoViewModel> produtos = JsonConvert.DeserializeObject<List<ProdutoViewModel>>(sjson);
			return produtos == null ? new List<ProdutoViewModel>() : produtos;
		}

		private void EscreverProdutosParaArquivo(List<ProdutoViewModel> produtos)
		{
			string json_content = JsonConvert.SerializeObject(produtos);
			System.IO.File.WriteAllText(CaminhoProdutosJson, json_content);
		}

		private int ObterProximoCodigoDisponivel()
		{
			List<ProdutoViewModel> produtos = LerArquivoProdutos();
			if (produtos.Any())
				return produtos.Max(p => p.Codigo) + 1;
			
			return 1;
		}
		#endregion

		#region Metodos CRUD
		[HttpGet]
		public IActionResult Get()
		{
			List<ProdutoViewModel> produtos = LerArquivoProdutos();
			return Ok(produtos);
		}

		[HttpGet("{codigo}")]
		public IActionResult Get(int codigo)
		{
			List<ProdutoViewModel> produtos = LerArquivoProdutos();
			ProdutoViewModel produto = produtos.Find(p => p.Codigo.Equals(codigo));

			if (produto == null)
				return NotFound();
			
			return Ok(produto);
		}

		[HttpPost]
		public IActionResult Post([FromBody] NovoProdutoViewModel produto)
		{
			if(produto == null) return BadRequest();

			List<ProdutoViewModel> produtos = LerArquivoProdutos();
			int novo_codigo = ObterProximoCodigoDisponivel();
			ProdutoViewModel novo_produto = new ProdutoViewModel()
			{
				Nome = produto.Nome,
				Codigo = novo_codigo,
				Descricao = produto.Descricao,
				Valor = produto.Valor
			};

			produtos.Add(novo_produto);
			EscreverProdutosParaArquivo(produtos);
			return CreatedAtAction(nameof(Get), novo_codigo, novo_produto);
		}

		[HttpPut("{codigo}")]
		public IActionResult Put(int codigo, [FromBody] NovoProdutoViewModel produto)
		{
			if (produto == null) return BadRequest();

			List<ProdutoViewModel> produtos = LerArquivoProdutos();
			int index = produtos.FindIndex(p => p.Codigo == codigo);

			if(index == -1) return NotFound();

			ProdutoViewModel produto_editado = new ProdutoViewModel()
			{
				Codigo = codigo,
				Descricao = produto.Descricao,
				Valor = produto.Valor,
				Nome = produto.Nome
			};

			produtos[index] = produto_editado;
			EscreverProdutosParaArquivo(produtos);

			return NoContent();
		}

		[HttpDelete("{codigo}")]
		public IActionResult Delete(int codigo)
		{
			List<ProdutoViewModel> produtos = LerArquivoProdutos();
			int index = produtos.FindIndex(p => p.Codigo == codigo);

			if (index == -1) return NotFound();

			produtos.RemoveAt(index);

			EscreverProdutosParaArquivo(produtos);

			return NoContent();
		}

		#endregion
	}
}