using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyGo.Models
{
    [Table("PAGINARTRANSACCIONES")]
    public class VistaPaginacion
    {

        [Column("POSICION")]
        public int Posicion { get; set; }

        [Key]
        [Column("IDTRANSACCION")]
        public int IdTransaccion { get; set; }

        [Column("IDUSUARIO")]
        public int IdUsuario { get; set; }

        [Column("CANTIDAD")]
        public int Cantidad{ get; set; }

        [Column("TIPOTRANSACCION")]
        public String TipoTransaccion { get; set; }

        [Column("FECHATRANSACCION")]
        public DateTime FechaTransaccion { get; set; }

        [Column("CONCEPTO")]
        public String Concepto { get; set; }
    }
}
