using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using multigyms.Models;
using System.Web.Http.Cors;


namespace multigyms.Controllers
{
    public class UsuarioController : ApiController
    {
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [Route("api/user/perfil")]
        [HttpPost]

        public IHttpActionResult getperfil([FromBody] getdata data)
        {
            try
            {
                MultigymEntities1 context = new MultigymEntities1();
                var user = (from x in context.MG_Persona
                            where x.Id == data.idusuario
                            select x).First();
                if (user != null)
                {
                    var per = new EPersona();
                    per.id = user.Id;
                    per.nombre = user.Nombre;
                    per.apellido = user.Apellido;
                    per.email = user.Email;
                    per.celular = user.Celular;
                    per.fechaingreso = user.FecIngreso;
                    per.creditos = user.CredDisponible;
                    per.token = user.Token;
                    return Ok(RespuestaApi<EPersona>.createRespuestaSuccess(per));
                }
                else
                {
                    return Ok(RespuestaApi<string>.createRespuestaError("no se enontro un Usuario con este item"));
                }
            }
            catch (Exception ex)
            {
                return Ok(RespuestaApi<string>.createRespuestaError(ex.Message));
            }
        }
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [Route("api/user/login")]
        [HttpPost]
        public IHttpActionResult login([FromBody] getdata data)
        {
            try
            {
                MultigymEntities1 context = new MultigymEntities1();
                var user = (from x in context.MG_Persona
                            where x.Email == data.usuariomail
                            && x.Passw == data.password
                            select x).ToList();
                if (user.Count()!=0)
                {
                    if (user.First().Activo == true)
                    {
                        var per = new EPersona();
                        per.id = user.First().Id;
                        per.nombre = user.First().Nombre;
                        per.apellido = user.First().Apellido;
                        per.email = user.First().Email;
                        per.celular = user.First().Celular;
                        per.fechaingreso = user.First().FecIngreso;
                        per.creditos = user.First().CredDisponible;
                        per.token = user.First().Token;
                        return Ok(RespuestaApi<EPersona>.createRespuestaSuccess(per));
                    }
                    else
                    {
                        return Ok(RespuestaApi<string>.createRespuestaError("Usuario inactivo, Ponga en contacto con el soporte tecnico"));
                    }
                    
                }
                else
                {
                    return Ok(RespuestaApi<string>.createRespuestaError("Nombre o contraseña Incorrectos"));
                }
            }
            catch (Exception ex)
            {
                return Ok(RespuestaApi<string>.createRespuestaError(ex.Message));
            }
        }

        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [Route("api/user/registro")]
        [HttpPost]
        public IHttpActionResult register([FromBody] getdata data)
        {
            try
            {
                MultigymEntities1 context = new MultigymEntities1();
                var user = (from x in context.MG_Persona
                            where x.Email == data.email
                            && x.Passw == data.password
                            select x).ToList();
                if (user.Count()==0)
                {
                    var per = new MG_Persona();
                    per.Nombre = data.nombres;
                    per.Apellido = data.apellidos;
                    per.Celular = data.celular;
                    per.FecNacimiento = Convert.ToDateTime(data.fechanacimiento);
                    per.Email = data.email;
                    per.Passw = data.password;
                    per.Activo = true;
                    per.Id_Plan = data.idplan;
                    context.MG_Persona.Add(per);
                    context.SaveChanges();
                    return Ok(RespuestaApi<MG_Persona>.createRespuestaSuccess(per));
                }
                else
                {
                    return Ok(RespuestaApi<string>.createRespuestaError("Ya existe un usuario registrado con este correo."));
                }
            }
            catch (Exception ex)
            {
                return Ok(RespuestaApi<string>.createRespuestaError(ex.Message));
            }
        }
    }
}
