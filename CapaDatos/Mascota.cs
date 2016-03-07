using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CapaDatos
{
    public class Mascota
    {
        public Mascota()
        {
        }

        public Mascota(int id, string n, int e)
        {
            IdMascota = id;
            Nombre = n;
            Edad = e;
        }

        public int IdMascota { get; set; }

        [Required]
        public string Nombre { get; set; }
       [Required]
        public int Edad { get; set; }

    }
}
