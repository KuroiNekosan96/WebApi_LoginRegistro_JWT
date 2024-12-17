using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiCortPas.Models;

namespace WebApiCortPas.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]//Aqui ya digo que solo ingresa usuarios autorizados cuando hay varios metodos
    /*[Authorize] cuando solo es un metodo*/

    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly SisPaseCortesiaContext _sisPaseCortesiaContext;

        public UsuarioController(SisPaseCortesiaContext sisPaseCortesiaContext)
        {
            _sisPaseCortesiaContext = sisPaseCortesiaContext;
        }

        [HttpGet]
        [Route("Pase")]
        public async Task<IActionResult> Lista()
        {
            var lista = await _sisPaseCortesiaContext.Usuarios.ToListAsync();
            return StatusCode(StatusCodes.Status200OK, new { value = lista });
        }




    }
}
