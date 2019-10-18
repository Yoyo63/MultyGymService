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
    public class GymsController : ApiController
    {
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [Route("api/gyms/getgymsbydisc")]
        [HttpPost]
        public IHttpActionResult getgymsbydisc([FromBody] getdata data)
        {
            try
            {
                MultigymEntities1 context = new MultigymEntities1();
                List<MG_Gym> gyms = new List<MG_Gym>(); 
                if (data.iddisc == 0)
                {
                    gyms = (from x in context.MG_Gym
                            select x).ToList();
                }
                else
                {
                    gyms = (from x in context.MG_Gym_Disc
                            where x.Id_Disciplina == data.iddisc
                            select x.MG_Gym).ToList();
                }
                
                List<EGym> lista = new List<EGym>();
                foreach (var g in gyms)
                {
                    var gy = new EGym();
                    gy.idgym = g.ID;
                    gy.nombregym = g.Nombre;
                    gy.img = g.ImgLogo;
                    gy.direccion = g.Direccion;
                    gy.telefono = g.Telefono;
                    gy.creditos = g.Creditos;
                    gy.lat = g.Lat.ToString();
                    gy.lon = g.Lon.ToString();
                    gy.horariolv = g.HorarioLV;
                    gy.horarios = g.HorarioS;
                    gy.horariod = g.HorarioDF;
                    gy.reviewaverage = g.ReviewAverage.ToString();
                    gy.reviewcount = g.ReviewCount.ToString();
                    lista.Add(gy);
                }
                return Ok(RespuestaApi<List<EGym>>.createRespuestaSuccess(lista));
            }
            catch (Exception ex)
            {
                return Ok(RespuestaApi<string>.createRespuestaError(ex.Message));
            }
        }

        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [Route("api/gyms/checkinscan")]
        [HttpPost]
        public IHttpActionResult checkinscan([FromBody] getdata data)
        {
            try
            {
                MultigymEntities1 context = new MultigymEntities1();
                string res = "";
                res = check_gym(data.idgym);
                MG_Gym gym = new MG_Gym();
                if (res == "ok")
                {
                    gym = (from x in context.MG_Gym
                               where x.ID == data.idgym
                               select x).First();
                    res = check_persona(data.idusuario, gym.Creditos, gym.Nombre);
                }
                if (res != "ok")
                {
                    return Ok(RespuestaApi<string>.createRespuestaError(res));
                }else
                {
                    res = "Desea gastar " + gym.Creditos + " creditos para ingresar a " + gym.Nombre;
                    return Ok(RespuestaApi<string>.createRespuestaSuccess(res));
                }
            }
            catch (Exception ex)
            {
                return Ok(RespuestaApi<string>.createRespuestaError(ex.Message));
            }
        }

        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [Route("api/gyms/checkin")]
        [HttpPost]
        public IHttpActionResult checkin([FromBody] getdata data)
        {
            try
            {
                MultigymEntities1 context = new MultigymEntities1();
                var g = (from x in context.MG_Gym
                         where x.ID == data.idgym
                         select x).First();
                var u = (from x in context.MG_Persona
                         where x.Id == data.idusuario
                         select x).First();
                u.CredDisponible = Convert.ToInt16(u.CredDisponible - g.Creditos);
                context.SaveChanges();
                var v = new MG_Visitas();
                v.Id_Gym = g.ID;
                v.Id_Persona = u.Id;
                v.FecVisita = Now1;
                v.CredUsado = g.Creditos;
                context.MG_Visitas.Add(v);
                context.SaveChanges();
                return Ok(RespuestaApi<string>.createRespuestaSuccess("Transaccion exitosa " + u.Nombre + " te restan " + u.CredDisponible + " creditos"));
            }
            catch (Exception ex)
            {
                return Ok(RespuestaApi<string>.createRespuestaError(ex.Message));
            }
        }

        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [Route("api/gyms/now1")]
        [HttpPost]
        public IHttpActionResult now([FromBody] getdata data)
        {
            try
            {
                MultigymEntities1 context = new MultigymEntities1();
                var a = Now1;
                return Ok(RespuestaApi<DateTime>.createRespuestaSuccess(a));
            }
            catch (Exception ex)
            {
                return Ok(RespuestaApi<string>.createRespuestaError(ex.Message));
            }
        }

        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [Route("api/gyms/now2")]
        [HttpPost]
        public IHttpActionResult now2([FromBody] getdata data)
        {
            try
            {
                MultigymEntities1 context = new MultigymEntities1();
                var a = DateTime.Now;
                return Ok(RespuestaApi<DateTime>.createRespuestaSuccess(a));
            }
            catch (Exception ex)
            {
                return Ok(RespuestaApi<string>.createRespuestaError(ex.Message));
            }
        }
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [Route("api/gyms/gym")]
        [HttpPost]
        public IHttpActionResult gym([FromBody] getdata data)
        {
            try
            {
                MultigymEntities1 context = new MultigymEntities1();
                var res = (from x in context.MG_Gym
                           where x.ID == data.idgym
                           select x).First();
                var result = new EGym();
                result.nombregym = res.Nombre;
                result.img = res.ImgLogo;
                result.creditos = res.Creditos;
                result.direccion = res.Direccion;
                result.telefono = res.Telefono;
                result.horariolv = res.HorarioLV;
                result.horarios = res.HorarioS;
                result.horariod = res.HorarioDF;
                return Ok(RespuestaApi<EGym>.createRespuestaSuccess(result));
            }
            catch (Exception ex)
            {
                return Ok(RespuestaApi<string>.createRespuestaError(ex.Message));
            }
        }

        public string check_gym(int idgym)
        {
            try
            {
                MultigymEntities1 context = new MultigymEntities1();
                var gym = (from x in context.MG_Gym
                           where x.ID == idgym
                           select x).First();
                if (gym.Activo == true)
                {
                    return "ok";
                }
                else
                {
                    return "Gimnasio Inactivo";
                }
            }
            catch(Exception ex)
            {
                return "Error";
            }
        }

        public string check_persona(int idpersona, int creditos, string ngym)
        {
            try
            {
                MultigymEntities1 context = new MultigymEntities1();
                var persona = (from x in context.MG_Persona
                           where x.Id == idpersona
                           select x).First();
                if (persona.Activo == true)
                {
                    if (persona.CredDisponible >= creditos)
                    {
                        return "ok";
                    }
                    else
                    {
                        return "Su Saldo es: <span style='color:red;'>" + persona.CredDisponible + "</span> es Insuficiente para " + ngym;
                    }
                }
                else
                {
                    return "Persona Inactiva";
                }
            }
            catch (Exception ex)
            {
                return "Error";
            }
        }

        public static DateTime Now1
        {
            get { return TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("SA Western Standard Time")); }
        }
    }
}
