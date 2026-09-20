using System;
using System.Collections.Generic;
using System.Text;

namespace PM.Entities
{
    public class REGISTRARJORNADA
    {   
        public int IdRegistrarJornada { get; set; }
        public DateTime FechaJor { get; set; }
        public int CantidadPresupuestada { get; set; }
        public int CantidadDespachada { get; set; }
        public int IdProducto { get; set; }
        public int IdUsuario { get; set; }

        public REGISTRARJORNADA() { }

        public REGISTRARJORNADA(int idRegistrarJornada,
                                DateTime fechajor,
                                int cantidadPresupuestada, 
                                int cantidadDespachada, 
                                int idProducto, 
                                int idUsuario)
        
        {
            IdRegistrarJornada = idRegistrarJornada;
            FechaJor = fechajor;
            CantidadPresupuestada = cantidadPresupuestada;
            CantidadDespachada = cantidadDespachada;
            IdProducto = idProducto;
            IdUsuario = idUsuario;
        }
    }
}
