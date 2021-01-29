using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyGo.Models
{
    [Table("Usurios")]
    public class Usuario
    {
        [Key]
        [Column("IDUSUARIO")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IdUsuario{ get; set; }

        [Column("NOMBRE")]
        public String Nombre { get; set; }

        [Column("NombreUsuario")]
        public String NombreUsuario { get; set; }


        [Column("Email")]
        public String Email{ get; set; }
       
        [Column("PASSWORD")]
        public byte[] Password{ get; set; }

        [Column("SALT")]
        public String Salt { get; set; }
       
    }
}
