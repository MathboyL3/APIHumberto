using APIHumberto.Models.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace APIHumberto.Controllers
{
	public class MainController : ControllerBase
	{
		protected IActionResult ApiOkResponse<T>(T data, string message)
		{
			var response = new ApiResponse<T>
			{
				message = message,
				data = data,
				success = true
			};

			return Ok(response);
		}

		protected IActionResult ApiBadRequestResponse(ModelStateDictionary model_state, string message = "Dados inválidos")
		{
			var errors = model_state.Values.SelectMany(e => e.Errors);
			var response = new ApiResponse<object>
			{
				message = message,
				data = errors.Select(n => n.ErrorMessage).ToArray(),
				success = true
			};

			return BadRequest(response);
		}
	}
}
