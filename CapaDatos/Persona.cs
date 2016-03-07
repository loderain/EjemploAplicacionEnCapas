using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class Persona
    {
        public Persona()
        {
        }

        public Persona(int id, string n, int e)
        {
            IdPersona = id;
            Nombre = n;
            Edad = e;
            
        }
        public int IdPersona { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        public int Edad { get; set; }

        public List<Mascota> ListaMascotas{ get; set; }
    }
}
