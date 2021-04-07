using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NuGetProyecto.Models
{
    [Table("TRANSACCIONES")]
    public class Transacciones
    {
        [Key]
        [Column("IDTRANSACCION")]
        public int IdTransaccion { get; set; }

        [ForeignKey("IDUSUARIO")]
        [Column("IDUSUARIO")]
        public int IdUsuario { get; set; }


        [Column("CANTIDAD")]
        public Double Cantidad { get; set; }

        [Column("TIPOTRANSACCION")]
        public String TipoTransaccion { get; set; }

        [Column("FECHATRANSACCION")]
        public DateTime FechaTransaccion { get; set; }

        [Column("CONCEPTO")]
        public String Concepto { get; set; }
    }
}
