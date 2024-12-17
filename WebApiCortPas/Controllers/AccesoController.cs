using Microsoft.AspNetCore.Http;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiLoginReg.Custom;
using WebApiLoginReg.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using WebApiCortPas.Models;



namespace WebApiLoginReg.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]//Esta api va acceder sin que el usuario se encuentre autorizado
    [ApiController]
    public class AccesoController : ControllerBase
    {
        private readonly SisPaseCortesiaContext _sisPaseCortesiaContext;
        private readonly Utilidades _utilidades;

        public AccesoController(SisPaseCortesiaContext sisPaseCortesiaContext, Utilidades utilidades)
        {

            _sisPaseCortesiaContext = sisPaseCortesiaContext;
            _utilidades = utilidades;

        }

        //Registro de usuario
        [HttpPost]
        [Route("Registrarse")]

        public async Task<IActionResult> Registrarse(UsuarioDTO objeto)
        {

            var modeloUsuario = new Usuario
            {
                Nombre = objeto.Nombre,
                Email = objeto.Email,
                Telefono = objeto.Telefono,
                Rol = objeto.Rol,
                Estado = objeto.Estado,
                PasswordHash = _utilidades.encriptarSHA256(objeto.PasswordHash)
             
            };

            await _sisPaseCortesiaContext.Usuarios.AddAsync(modeloUsuario);
            await _sisPaseCortesiaContext.SaveChangesAsync();

            if (modeloUsuario.IdUsuario != 0)
            {
                return StatusCode(StatusCodes.Status200OK, new { isSuccess = true });
            }
            else
            {
                return StatusCode(StatusCodes.Status200OK, new { isSuccess = false });
            }

        }


        //Para hacer login
        [HttpPost]
        [Route("Login")]

        public async Task<IActionResult> Login(LoginDTO objeto)
        {

            var usuarioEncontrado = await _sisPaseCortesiaContext.Usuarios
                                    .Where(u =>
                                        u.Email == objeto.Email &&
                                        u.PasswordHash == _utilidades.encriptarSHA256(objeto.PasswordHash)
                                    ).FirstOrDefaultAsync();

            if (usuarioEncontrado == null)
                return StatusCode(StatusCodes.Status200OK, new { isSuccess = false, token = "" });
            else
                return StatusCode(StatusCodes.Status200OK, new { isSuccess = true, token = _utilidades.generarJWT(usuarioEncontrado) });

        }

        //Metodo de validar token
        [HttpGet]
        [Route("ValidarToken")]

        public IActionResult ValidarToken([FromQuery]string token)
        {
            bool respuesta = _utilidades.validarToken(token);
           
              return StatusCode(StatusCodes.Status200OK, new {isSuccess=respuesta});

        }


    }
}
