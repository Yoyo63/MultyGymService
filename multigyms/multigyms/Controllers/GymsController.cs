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
using System.Device.Location;
using NotificacionPush;
using RestSharp;

namespace multigyms.Controllers
{
    public class GymsController : ApiController
    {
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [Route("api/gyms/getciudades")]
        [HttpPost]
        public IHttpActionResult getciudades()
        {
            try
            {
                MultigymEntities1 context = new MultigymEntities1();
                List<MG_Ciudades> ciudades = new List<MG_Ciudades>();
                ciudades = (from x in context.MG_Ciudades
                            select x).ToList();

                List<ECiudad> lista = new List<ECiudad>();
                foreach (var c in ciudades)
                {
                    var ciu = new ECiudad();
                    ciu.idciudad = c.Id_Ciudad;
                    ciu.nombre = c.Ciudad;
                    ciu.codigopais = c.CodigoPais;
                    lista.Add(ciu);
                }
                return Ok(RespuestaApi<List<ECiudad>>.createRespuestaSuccess(lista));
            }
            catch (Exception ex)
            {
                return Ok(RespuestaApi<string>.createRespuestaError(ex.Message));
            }
        }
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [Route("api/gyms/getdisciplinas")]
        [HttpPost]
        public IHttpActionResult getdisciplinas()
        {
            try
            {
                MultigymEntities1 context = new MultigymEntities1();
                List<MG_Disciplinas> disciplinas = new List<MG_Disciplinas>();
                disciplinas = (from x in context.MG_Disciplinas
                         select x).OrderBy(x => x.Nombre).ToList();

                List<EDisciplina> lista = new List<EDisciplina>();
                foreach (var d in disciplinas)
                {
                    var di = new EDisciplina();
                    di.iddisciplina = d.Id;
                    di.nombre = d.Nombre;
                    di.rutaimg = d.Imagen;
                    lista.Add(di);
                }
                return Ok(RespuestaApi<List<EDisciplina>>.createRespuestaSuccess(lista));
            }
            catch (Exception ex)
            {
                return Ok(RespuestaApi<string>.createRespuestaError(ex.Message));
            }
        }
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [Route("api/gyms/getgymbynombre")]
        [HttpPost]
        public IHttpActionResult getgymbynombre([FromBody] getdata data)
        {
            try
            {
                MultigymEntities1 context = new MultigymEntities1();
                List<MG_Gym> gyms = new List<MG_Gym>();
                gyms = (from x in context.MG_Gym
                        where x.Activo == true
                        && x.Nombre.Contains(data.nombregym)
                        select x).ToList();

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
        [Route("api/gyms/getgymbynombrefiltro")]
        [HttpPost]
        public IHttpActionResult getgymbynombrefiltro([FromBody] getdata data)
        {
            try
            {
                MultigymEntities1 context = new MultigymEntities1();
                List<MG_Gym> gyms = new List<MG_Gym>();
                gyms = (from x in context.MG_Gym
                        where x.Activo == true
                        select x).ToList();
                if (!string.IsNullOrEmpty(data.nombregym))
                {
                    gyms = (from x in gyms
                            where x.Nombre.Contains(data.nombregym)
                            select x).ToList();
                }

                if (!string.IsNullOrEmpty(data.fcreditos.ToString()) && data.fcreditos!=0)
                {
                    gyms = (from x in gyms
                            where x.Creditos <= data.fcreditos
                            select x).ToList();
                }

                if (!string.IsNullOrEmpty(data.fcategoria.ToString()) && data.fcategoria != 0)
                {
                    gyms = (from x in gyms
                            where x.MG_Gym_Disc.Any(y => y.Id_Disciplina == data.fcategoria)
                            select x).ToList();
                }
                

                if (!string.IsNullOrEmpty(data.fcalificacion.ToString()) && data.fcalificacion != 0)
                {
                    gyms = (from x in gyms
                            where (int)x.ReviewAverage >= data.fcalificacion
                            select x).ToList();
                }

                if (!string.IsNullOrEmpty(data.fciudad.ToString()) && data.fciudad != 0)
                {
                    gyms = (from x in gyms
                            where x.Id_Ciudad==data.fciudad
                            select x).ToList();
                }


                List<EGym> lista = new List<EGym>();
                foreach (var g in gyms)
                {
                    var control = false;
                    double distance = -1;
                    if (!string.IsNullOrEmpty(data.fdistancia.ToString()) && data.fdistancia != 0)
                    {
                        if (!string.IsNullOrEmpty(data.lat) && !string.IsNullOrEmpty(data.lon))
                        {
                            double latD = ConvertCoordStrToDouble(g.Lat.ToString());
                            double lngD = ConvertCoordStrToDouble(g.Lon.ToString());
                            distance = GetDistance(ConvertCoordStrToDouble(data.lat), ConvertCoordStrToDouble(data.lon), latD, lngD);
                        }
                        else
                        {
                            control = true;
                        }
                    }else
                    {
                        control = true;
                    }
                    if (distance <= (data.fdistancia * 1000) && distance != -1)
                    {
                        control = true;
                    }
                    if (control == true)
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
                        gy.distancia = distance.ToString();
                        gy.horariod = g.HorarioDF;
                        gy.reviewaverage = g.ReviewAverage.ToString();
                        gy.reviewcount = g.ReviewCount.ToString();
                        lista.Add(gy);
                    }
                }
                return Ok(RespuestaApi<List<EGym>>.createRespuestaSuccess(lista));
            }
            catch (Exception ex)
            {
                return Ok(RespuestaApi<string>.createRespuestaError(ex.Message));
            }
        }
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [Route("api/gyms/getplanes")]
        [HttpPost]
        public IHttpActionResult getplanes([FromBody] getdata data)
        {
            try
            {
                MultigymEntities1 context = new MultigymEntities1();
                List<MG_Planes> plans = new List<MG_Planes>();
                plans = (from x in context.MG_Planes
                         where x.Activo==true
                        select x).ToList();

                List<EPlan> lista = new List<EPlan>();
                foreach (var p in plans)
                {
                    var pl = new EPlan();
                    pl.idplan = p.Id;
                    pl.plan = p.Nombre;
                    pl.descripcion = p.Descripcion;
                    pl.precio = p.Creditos;
                    pl.creditos = p.Creditos;
                    pl.activo = p.Activo;
                    lista.Add(pl);
                }
                return Ok(RespuestaApi<List<EPlan>>.createRespuestaSuccess(lista));
            }
            catch (Exception ex)
            {
                return Ok(RespuestaApi<string>.createRespuestaError(ex.Message));
            }
        }
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
                            where x.Activo.Equals(true)
                            && x.Id_TipoEntidad == (int)tipoEntidad.gymnasio
                            select x).ToList();
                }
                else
                {
                    gyms = (from x in context.MG_Gym_Disc
                            where x.Id_Disciplina == data.iddisc
                            && x.MG_Gym.Activo.Equals(true)
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
        // confirmacion de monto Gym
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
                               && x.Id_TipoEntidad == (int)tipoEntidad.gymnasio
                               && x.Activo == true
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
        //pedido de monto snack
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [Route("api/gyms/checkinscanother")]
        [HttpPost]
        public IHttpActionResult checkinscanother([FromBody] getdata data)
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
                           && x.Id_TipoEntidad == (int)tipoEntidad.comercio
                           && x.Activo == true
                           select x).First();
                }
                if (res != "ok")
                {
                    return Ok(RespuestaApi<string>.createRespuestaError(res));
                }
                else
                {
                    res = "Ingrese el monto a pagar en " + gym.Nombre;
                    return Ok(RespuestaApi<string>.createRespuestaSuccess(res));
                }
            }
            catch (Exception ex)
            {
                return Ok(RespuestaApi<string>.createRespuestaError(ex.Message));
            }
        }
        //confirmacion de monto Snack
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [Route("api/gyms/checkinscanother2")]
        [HttpPost]
        public IHttpActionResult checkinscanother2([FromBody] getdata data)
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
                           && x.Id_TipoEntidad == (int)tipoEntidad.comercio
                           && x.Activo == true
                           select x).First();
                    res = check_persona(data.idusuario, data.montoapagar, gym.Nombre, true);
                }
                if (res != "ok")
                {
                    return Ok(RespuestaApi<string>.createRespuestaError(res));
                }
                else
                {
                    res = "Desea pagar " + data.montoapagar + " creditos a " + gym.Nombre;
                    return Ok(RespuestaApi<string>.createRespuestaSuccess(res));
                }
            }
            catch (Exception ex)
            {
                return Ok(RespuestaApi<string>.createRespuestaError(ex.Message));
            }
        }
        //pedido de monto taxi
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [Route("api/gyms/checkinscantaxis")]
        [HttpPost]
        public IHttpActionResult checkinscantaxis([FromBody] getdata data)
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
                           && x.Id_TipoEntidad == (int)tipoEntidad.taxi
                           && x.Activo == true
                           select x).First();
                }
                if (res != "ok")
                {
                    return Ok(RespuestaApi<string>.createRespuestaError(res));
                }
                else
                {
                    res = "Ingrese el monto a pagar en " + gym.Nombre;
                    return Ok(RespuestaApi<string>.createRespuestaSuccess(res));
                }
            }
            catch (Exception ex)
            {
                return Ok(RespuestaApi<string>.createRespuestaError(ex.Message));
            }
        }
        //confirmacion de monto taxi
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [Route("api/gyms/checkinscantaxis2")]
        [HttpPost]
        public IHttpActionResult checkinscantaxis2([FromBody] getdata data)
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
                           && x.Id_TipoEntidad == (int)tipoEntidad.taxi
                           && x.Activo == true
                           select x).First();
                    res = check_persona(data.idusuario, data.montoapagar, gym.Nombre, true);
                }
                if (res != "ok")
                {
                    return Ok(RespuestaApi<string>.createRespuestaError(res));
                }
                else
                {
                    res = "Desea pagar " + data.montoapagar + " creditos a " + gym.Nombre;
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
                return Ok(RespuestaApi<string>.createRespuestaSuccess("Transaccion exitosa " + u.Nombre + " te restan " + u.CredDisponible + " creditos|"+g.ImgLogo+"|"+u.Nombre +" "+ u.Apellido + "|" + Now1.ToString("dd/MM/yyyy HH:mm")));
            }
            catch (Exception ex)
            {
                return Ok(RespuestaApi<string>.createRespuestaError(ex.Message));
            }
        }
        //checkin gym
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [Route("api/gyms/checkin2020")]
        [HttpPost]
        public IHttpActionResult checkin2020([FromBody] getdata data)
        {
            try
            {
                MultigymEntities1 context = new MultigymEntities1();
                var g = (from x in context.MG_Gym
                         where x.ID == data.idgym
                         && x.Id_TipoEntidad == (int)tipoEntidad.gymnasio
                         && x.Activo == true
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
                var res = new ERandomResponse();
                res.mensaje = "Transaccion exitosa " + u.Nombre + " te restan " + u.CredDisponible + " creditos|" + g.ImgLogo + "|" + u.Nombre + " " + u.Apellido + "|" + Now1.ToString("dd/MM/yyyy HH:mm");
                res.creditos = u.CredDisponible.ToString();
                notificationonesignal(g.PushID, "Nuevo Ingreso Registrado", u.Nombre + " ha ingresado a " + g.Nombre, v.Id, u.Nombre,v.CredUsado, Convert.ToDateTime(v.FecVisita));
                return Ok(RespuestaApi<ERandomResponse>.createRespuestaSuccess(res));
            }
            catch (Exception ex)
            {
                return Ok(RespuestaApi<string>.createRespuestaError(ex.Message));
            }
        }
        //checkin Snack
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [Route("api/gyms/checkin2020other")]
        [HttpPost]
        public IHttpActionResult checkin2020other([FromBody] getdata data)
        {
            try
            {
                MultigymEntities1 context = new MultigymEntities1();
                var g = (from x in context.MG_Gym
                         where x.ID == data.idgym
                         && x.Id_TipoEntidad == (int)tipoEntidad.comercio
                         && x.Activo == true
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
                v.CredUsado = Convert.ToInt16(data.montoapagar);
                context.MG_Visitas.Add(v);
                context.SaveChanges();
                var res = new ERandomResponse();
                res.mensaje = "Transaccion exitosa " + u.Nombre + " te restan " + u.CredDisponible + " creditos|" + g.ImgLogo + "|" + u.Nombre + " " + u.Apellido + "|" + Now1.ToString("dd/MM/yyyy HH:mm");
                res.creditos = u.CredDisponible.ToString();
                notificationonesignal(g.PushID, "Nuevo Pago Registrado", u.Nombre + " ha pagado " + v.CredUsado + " Creditos", v.Id, u.Nombre, v.CredUsado, Convert.ToDateTime(v.FecVisita));
                return Ok(RespuestaApi<ERandomResponse>.createRespuestaSuccess(res));
            }
            catch (Exception ex)
            {
                return Ok(RespuestaApi<string>.createRespuestaError(ex.Message));
            }
        }
        //checkin Micros
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [Route("api/gyms/checkin2020micros")]
        [HttpPost]
        public IHttpActionResult checkin2020micros([FromBody] getdata data)
        {
            try
            {
                MultigymEntities1 context = new MultigymEntities1();
                var g = (from x in context.MG_Gym
                         where x.ID == data.idgym
                         && x.Id_TipoEntidad == (int)tipoEntidad.micros
                         && x.Activo == true
                         select x).First();
                var u = (from x in context.MG_Persona
                         where x.Id == data.idusuario
                         select x).First();
                var resm = check_persona(u.Id, data.montoapagar, g.Nombre, true);
                if(resm != "ok")
                {
                    return Ok(RespuestaApi<string>.createRespuestaError(resm));
                }
                u.CredDisponible = Convert.ToInt16(u.CredDisponible - g.Creditos);
                context.SaveChanges();
                var v = new MG_Visitas();
                v.Id_Gym = g.ID;
                v.Id_Persona = u.Id;
                v.FecVisita = Now1;
                v.CredUsado = Convert.ToInt16(data.montoapagar);
                context.MG_Visitas.Add(v);
                context.SaveChanges();
                var res = new ERandomResponse();
                res.mensaje = "Transaccion exitosa " + u.Nombre + " te restan " + u.CredDisponible + " creditos|" + g.ImgLogo + "|" + u.Nombre + " " + u.Apellido + "|" + Now1.ToString("dd/MM/yyyy HH:mm");
                res.creditos = u.CredDisponible.ToString();
                notificationonesignal(g.PushID, "Nuevo Pago Registrado", u.Nombre + " ha pagado " + v.CredUsado + " Creditos", v.Id, u.Nombre, v.CredUsado, Convert.ToDateTime(v.FecVisita));
                return Ok(RespuestaApi<ERandomResponse>.createRespuestaSuccess(res));
            }
            catch (Exception ex)
            {
                return Ok(RespuestaApi<string>.createRespuestaError(ex.Message));
            }
        }
        //checkin Micro
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [Route("api/gyms/checkin2020taxis")]
        [HttpPost]
        public IHttpActionResult checkin2020taxis([FromBody] getdata data)
        {
            try
            {
                MultigymEntities1 context = new MultigymEntities1();
                var g = (from x in context.MG_Gym
                         where x.ID == data.idgym
                         && x.Id_TipoEntidad == (int)tipoEntidad.taxi
                         && x.Activo == true
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
                v.CredUsado = Convert.ToInt16(data.montoapagar);
                context.MG_Visitas.Add(v);
                context.SaveChanges();
                var res = new ERandomResponse();
                res.mensaje = "Transaccion exitosa " + u.Nombre + " te restan " + u.CredDisponible + " creditos|" + g.ImgLogo + "|" + u.Nombre + " " + u.Apellido + "|" + Now1.ToString("dd/MM/yyyy HH:mm");
                res.creditos = u.CredDisponible.ToString();
                notificationonesignal(g.PushID, "Nuevo Pago Registrado", u.Nombre + " ha pagado " + v.CredUsado + " Creditos", v.Id, u.Nombre, v.CredUsado, Convert.ToDateTime(v.FecVisita));
                return Ok(RespuestaApi<ERandomResponse>.createRespuestaSuccess(res));
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
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [Route("api/gyms/login")]
        [HttpPost]
        public IHttpActionResult login([FromBody] getdata data)
        {
            try
            {
                MultigymEntities1 context = new MultigymEntities1();
                var user = (from x in context.MG_Gym
                            where x.Email == data.usuariomail
                            && x.Passw == data.password
                            select x).ToList();
                if (user.Count() != 0)
                {
                    //if (user.First().Activo == true)
                    //{
                        var per = new EPersona();
                        per.id = user.First().ID;
                        per.nombre = user.First().Nombre;
                        //per.apellido = user.First().Apellido;
                        per.apellido = "";
                        per.email = user.First().Email;
                        per.celular = user.First().Celular;
                        //per.fechaingreso = user.First().FecIngreso;
                        per.creditos = user.First().Creditos;
                        //per.token = user.First().Token;
                        return Ok(RespuestaApi<EPersona>.createRespuestaSuccess(per));
                    //}
                    //else
                    //{
                    //    return Ok(RespuestaApi<string>.createRespuestaError("Usuario inactivo, Ponga en contacto con el soporte tecnico"));
                    //}
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
        [Route("api/gyms/historialcheckin")]
        [HttpPost]
        public IHttpActionResult historialcheckin([FromBody] getdata data)
        {
            try
            {
                MultigymEntities1 context = new MultigymEntities1();
                var user = (from x in context.MG_Gym
                            where x.ID == data.idgym
                            select x).First();
                if (user != null)
                {
                    var his = (from x in context.MG_Visitas
                               where x.Id_Gym == data.idgym
                               select x).OrderByDescending(x => x.FecVisita).ToList().Take(30);
                    if (data.total == false)
                    {
                        var stingf = data.fechafin.ToString().Split(' ')[0] + " 23:59";
                        var ff = Convert.ToDateTime(stingf);
                        his = (from x in his
                                where (x.FecVisita.Value >= data.fechainicio && x.FecVisita.Value <= ff)
                                select x).OrderByDescending(x => x.FecVisita).ToList().Take(30);
                    }
                    var result = new EResVisitas();
                    var res = new List<Evisita>();
                    foreach (var v in his)
                    {
                        var vi = new Evisita();
                        vi.idvisita = v.Id;
                        vi.idusuario = user.ID;
                        vi.nombregym = v.MG_Persona.Nombre;
                        //vi.fechayhora = parcedatetime(Convert.ToDateTime(v.FecVisita));
                        vi.fechayhora = Convert.ToDateTime(v.FecVisita);
                        vi.creditousados = v.CredUsado;
                        vi.idgym = v.Id_Gym;
                        res.Add(vi);
                    }
                    result.visitas = res;
                    result.creditosganados = res.Sum(x=>x.creditousados).ToString();
                    result.visitastotales = res.Count().ToString();
                    return Ok(RespuestaApi<EResVisitas>.createRespuestaSuccess(result));
                }
                else
                {
                    return Ok(RespuestaApi<string>.createRespuestaError("no se enontro un Usuario con este Id"));
                }

            }
            catch (Exception ex)
            {
                return Ok(RespuestaApi<string>.createRespuestaError(ex.ToString()));
            }
        }
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [Route("api/gyms/registrodispositivo")]
        [HttpPost]
        public IHttpActionResult registrodispositivo([FromBody] getdata data)
        {
            try
            {
                MultigymEntities1 context = new MultigymEntities1();
                var gym = (from x in context.MG_Gym
                            where x.ID == data.idgym
                            select x).ToList();
                if (gym.Count() != 0)
                {
                    gym.First().IMEI = data.imei;
                    gym.First().PushID = data.pushid;
                    gym.First().CelTypeID = data.tipoapp==1?"Android":"Ios";
                    context.SaveChanges();
                    return Ok(RespuestaApi<string>.createRespuestaSuccess("ok"));
                }
                else
                {
                    return Ok(RespuestaApi<string>.createRespuestaError("no se pudo encontrar un gym con este id"));
                }
            }
            catch (Exception ex)
            {
                return Ok(RespuestaApi<string>.createRespuestaError(ex.Message));
            }
        }
        public void notificationonesignal(string pushid, string titulo, string mensaje, int idvisita, string nombreuser, short creditosusados, DateTime fechahora)
        {
            var client = new RestClient("https://onesignal.com/api/v1/notifications");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", "Basic ZTUxNzY3MDktYTdlZi00ODg2LTkzN2YtZjg0MTJlYTRmN2Ni");
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Cookie", "__cfduid=dce83d71651cc829f9453344945906aad1587949040");
            request.AddParameter("application/json", "{\n\t\"app_id\":\"0a91eaef-c570-491e-aa94-cf18b055bfb9\",\n\t\"include_player_ids\": [\""+ pushid + "\"],\n\t\"data\": {\n\t\t\"idvisita\": " + idvisita + ",\n\t\t\"nombregym\": \"" + nombreuser + "\",\n\t\t\"creditousados\": " + creditosusados + ",\n\t\t\"fechayhora\": \"" + fechahora + "\"\n\t},\n\t\"contents\": {\n\t\t\"en\": \"" + mensaje + "\",\n\t\t\"es\": \"" + mensaje + "\"\n\t},\n\t\"headings\": {\n\t\t\"en\": \"" + titulo + "\",\n\t\t\"es\": \"" + titulo + "\"\n\t},\n\t\"small_icon\": \"ic_stat_onesignal_default\",\n\t\"priority\": \"10\",\n\t\"android_visibility\": \"1\",\n\t\"big_picture\": \"https://multigym.fit/images/MULTIGYM-S%20Letras%20N.png\"\n}", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
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
                throw new Exception("Error");
            }
        }
        public string check_persona(int idpersona, int creditos, string ngym, bool flag=false)
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
                        if (flag == false)
                        {
                            return "Su Saldo es: <span style='color:red;'>" + persona.CredDisponible + "</span> es Insuficiente para " + ngym;
                        }
                        else
                        {
                            return "Su Saldo es: <span style='color:red;'>" + persona.CredDisponible + "</span> es Insuficiente para pagar " + creditos + " creditos.";
                        }
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
        public static double ConvertCoordStrToDouble(string coordenada)
        {
            double NewCoord = Convert.ToDouble(coordenada.Replace(".", ","));
            if (NewCoord < -200 || NewCoord > 200)
            {
                NewCoord = Convert.ToDouble(coordenada.Replace(",", "."));
            }

            if (NewCoord < -200 || NewCoord > 200)
            {
                NewCoord = double.Parse(coordenada);
            }
            return NewCoord;
        }
        public static double GetDistance(double latO, double lngO, double latD, double lngD)
        {
            var sCoord = new GeoCoordinate(latO, lngO);
            var eCoord = new GeoCoordinate(latD, lngD);
            return sCoord.GetDistanceTo(eCoord);
        }
        public static DateTime parcedatetime(DateTime date)
        {
            return TimeZoneInfo.ConvertTime(date, TimeZoneInfo.FindSystemTimeZoneById("SA Western Standard Time"));
        }
//public void CreateNotificationPushs(MultigymEntities1 esquema, TipoNotificacion tipo, long contenidoId, string contenido, List<long> usuariosIds = null, string titulo = "")
        //{
        //    // Generar las notificaciones push
        //    Notificacion notificacion = new Notificacion();
        //    notificacion.FechaHora = Now1;
        //    notificacion.ContenidoId = contenidoId;
        //    notificacion.Texto = contenido;
        //    notificacion.Tipo = (int)tipo;

        //    var notificaciones = new List<NotificacionPush.Entidades.Notificacion>();
        //    usuariosIds = usuariosIds.Distinct().ToList();

        //    var dispositivos = (from x in esquema.MG_Persona
        //                        where usuariosIds.Contains(x.Id)
        //                        && x.Activo == true
        //                        && x.dispositivoId != null
        //                        && x.DispositivoMovil.Estado == 1
        //                        select new
        //                        {
        //                            PushId = x.DispositivoMovil.PushId,
        //                            TipoOS = x.DispositivoMovil.TipoOS,
        //                            TipoApp = x.DispositivoMovil.TipoAplicacion
        //                        }).ToList();
        //    foreach (var x in dispositivos)
        //    {
        //        NotificacionPush.Entidades.Notificacion notipush = new NotificacionPush.Entidades.Notificacion(contenidoId, x.TipoOS, x.PushId, 0, x.TipoApp);
        //        notificaciones.Add(notipush);
        //    }

        //    NotificacionPsh.EnviarNotificacion(string.IsNullOrEmpty(titulo) ? ObtenerTituloWithTipoNotificacion(tipo) : titulo, contenido, "", (int)tipo + "", notificaciones);
        //}   
        public void CreateNotificationPushsprueba(string imei, int tipo_app, string pushid, TipoNotificacion tipo, string contenido, string titulo = "")
        {
            // Generar las notificaciones push
            Notificacion notificacion = new Notificacion();
            notificacion.FechaHora = Now1;
            notificacion.ContenidoId = 0;
            notificacion.Texto = contenido;
            notificacion.Tipo = (int)tipo;

            var notificaciones = new List<NotificacionPush.Entidades.Notificacion>();

                NotificacionPush.Entidades.Notificacion notipush = new NotificacionPush.Entidades.Notificacion(0, "Android", pushid, 0, tipo_app);
                notificaciones.Add(notipush);


            NotificacionPsh.EnviarNotificacion(string.IsNullOrEmpty(titulo) ? ObtenerTituloWithTipoNotificacion(tipo) : titulo, contenido, "", (int)tipo + "", notificaciones);
        }
        private string ObtenerTituloWithTipoNotificacion(TipoNotificacion tipo)
        {
            switch (tipo)
            {
                case TipoNotificacion.General: return "Nuevas novedades..";
                case TipoNotificacion.SolicitudNueva: return "Nueva solicitud Yaigo.";
                case TipoNotificacion.SolicitudAceptadoPorConductor: return "Solicitud aceptada por conductor.";
                case TipoNotificacion.SolicitudAceptadoPorComercio: return "Solicitud aceptada por comercio.";
                case TipoNotificacion.SolicitudCanceladaPorComercio: return "Solicitud cancelada por conductor.";
                case TipoNotificacion.OrdenLista: return "Orden lista.";
                case TipoNotificacion.OrdenEnCamino: return "La orden esta en camino.";
                case TipoNotificacion.OrdenEnDestino: return "La orden ha llegado al destino.";
                case TipoNotificacion.OrdenEntragada: return "La orden ha sido entregada al conductor.";
                case TipoNotificacion.OrdenEntragadacliente: return "La orden ha sido entregada al cliente.";
                case TipoNotificacion.PedidoCalificacion: return "Calificación del servicio.";
                case TipoNotificacion.SolicitudRechazadaPorComercio: return "Solicitud Rechazada por Comercio";
                default: return "YAIGO";
            }
        }
    }
    public class Notificacion
    {
        public string NotificacionId { get; set; }
        public System.DateTime FechaHora { get; set; }
        public string Texto { get; set; }
        public int Tipo { get; set; }
        public Nullable<long> ContenidoId { get; set; }
    }
}
