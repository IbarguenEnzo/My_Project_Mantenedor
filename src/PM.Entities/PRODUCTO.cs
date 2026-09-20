using System;
using System.Collections.Generic;
using System.Text;

namespace PM.Entities
{
    public  class PRODUCTO

    {
        public int IdProducto { get; set; }
        public string NombreProd { get; set; } = string.Empty;
        public string Unidad { get; set; } = string.Empty;
        public string Observacion { get; set; } = string.Empty;
        public int Precio {  get; set; }
        public int IdCategoria { get; set; }
        public int StockDisponible { get; set; }
        public bool Estado { get; set; }

        public PRODUCTO() { }
        
        public PRODUCTO(int idProducto,
                        string nombreProd,
                        string unidad,
                        string observacion,
                        int precio,
                        int idCategoria,
                        int stockDisponible,
                        bool estado)
        {
            IdProducto = idProducto;
            NombreProd = nombreProd;
            Unidad = unidad;
            Observacion = observacion;
            Precio = precio;
            IdCategoria = idCategoria;
            StockDisponible = stockDisponible;
            Estado = estado;
        }
    }
    
}
