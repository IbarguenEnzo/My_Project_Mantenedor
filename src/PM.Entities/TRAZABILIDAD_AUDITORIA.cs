using System;
using System.Collections.Generic;
using System.Text;

namespace PM.Entities
{
    public class TRAZABILIDADAUDITORIA
    {
        public int IdTrazabilidadAuditoria { get; set; }
        public int IdUsuario { get; set; }
        public required string Accion { get; set; }
        public DateTime FechaHora { get; set; }
        public required string Detalle { get; set; }
    }
}
