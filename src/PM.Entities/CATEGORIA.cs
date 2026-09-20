using System;
using System.Collections.Generic;
using System.Text;

namespace PM.Entities
{
    public class CATEGORIA
    {
        public int IdCategoria { get; set; }
        public string NombreCat { get; set; } = string.Empty;
        public string Observacion { get; set; } = string.Empty;
        
        public CATEGORIA() { }

        public CATEGORIA(int idCategoria, string nombrecat, string observacion)
        {
            IdCategoria = idCategoria;
            NombreCat = nombrecat;
            Observacion = observacion;
        }

        public override string ToString() => NombreCat;

       

    }
}
