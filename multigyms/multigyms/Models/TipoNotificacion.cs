using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace multigyms.Models
{
    public enum TipoNotificacion
    {
        General = 0,
        /// <summary>
        /// Notifica Cliente: Conductor, Empresa
        /// </summary>
        SolicitudNueva = 1,  
        /// <summary>
        /// Notifica Conductor: Cliente, Conductores, Empresa
        /// </summary>
        SolicitudAceptadoPorConductor = 2,
        /// <summary>
        /// Notifica Comercio: Cliente, Conductor
        /// </summary>
        SolicitudAceptadoPorComercio = 3,
        /// <summary>
        /// Notifica Comercio: Cliente, Conductor
        /// </summary>
        SolicitudCanceladaPorComercio = 4,
        /// <summary>
        /// Notifica Comercio: CLiente, Conductor 
        /// </summary>
        OrdenLista = 5,
        /// <summary>
        /// Notifica Comercio o Conductor: Cliente
        /// </summary>
        OrdenEnCamino = 6,
        /// <summary>
        /// Notifica Conductor: Cliente
        /// </summary>
        OrdenEnDestino = 7,
        /// <summary>
        /// Notifica Conductor: Cliente
        /// </summary>
        OrdenEntragada = 8,
        /// <summary>
        /// Notifica Conductor: Cliente, Empresa
        /// </summary>
        OrdenEntragadacliente = 9,
        /// <summary>
        /// Notifica Sistema: Cliente
        /// </summary>
        PedidoCalificacion = 10,
        /// <summary>
        /// Notifica Comercio: Cliente, Conductor
        /// </summary>
        SolicitudRechazadaPorComercio = 11,
        /// <summary>
        /// Notifica Comercio: Cliente, Conductor
        /// </summary>
        SolicitudReasigadaAotroConductor = 12,
        /// <summary>
        /// Notifica sistema: Cliente, Conductor, Empresa
        /// </summary>
        SolicitudCancelada
    }
}