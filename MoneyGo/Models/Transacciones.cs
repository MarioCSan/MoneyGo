using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyGo.Models
{
    [Table("TRANSACCIONES")]
    public class Transacciones
    {
        [Key]
        [Column("IDTRANSACCION")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IdTransaccion { get; set; }

        [Column("IDUSUARIO")]
        public int IdUsuario { get; set; }


        [Column("CANTIDAD")]
        public float Cantidad { get; set; }
      
        [Column("TIPOTRANSACCION")]
        public String TipoTransaccion { get; set; }

        [Column("FECHATRANSACCION")]
        public DateTime FechaTransaccion { get; set; }
    }
}
