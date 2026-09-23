using System;
using System.Collections.Generic;
using System.Text;

namespace PM.Entities
{
    public class TRAZABILIDADAUDITORIA
    {
        public int IdTrazabilidadAuditoria { get; set; }
        public int IdUsuario { get; set; }
        public string Accion { get; set; } = string.Empty;
        public DateTime FechaHora { get; set; }
        public string Detalle { get; set; } = string.Empty;

        public TRAZABILIDADAUDITORIA() { }

        public TRAZABILIDADAUDITORIA(int idTrazabilidadAuditoria, int idUsuario, string accion, DateTime fechaHora, string detalle)
        {
            IdTrazabilidadAuditoria = idTrazabilidadAuditoria;
            IdUsuario = idUsuario;
            Accion = accion;
            FechaHora = fechaHora;
            Detalle = detalle;
        }

    }
}     
