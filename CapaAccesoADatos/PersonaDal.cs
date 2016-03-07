using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;
/*
 * TABLA AFECTADA POR ESTA CLASE DAL(DATA ACCESS LAYER)
 * 
 * create table personas(
	idpersona int primary key identity,
	nombre_persona varchar(40),
	edad_persona integer)
 */
namespace CapaAccesoADatos
{
    public class PersonaDal{
   
       
        /// <summary>
        /// Devuelve un listado de todas las personas existentes en la bbdd
        /// </summary>
        /// <returns>El listado de personas.</returns>
        public List<Persona> GetListado()
        {
            //creo el listado que voy a rellenar y devolver
            List<Persona> listado = new List<Persona>();
            
            //creo el command que voy a ejecutar
            SqlCommand comando = new SqlCommand("select idpersona,nombre_persona, edad_persona from personas");
            //creo el objeto helper que me servira para obtener los datos
            DataBaseHelper helper = new DataBaseHelper();

            //Bloque con sensibilidad de lanzar excepcion en caso de perdida de la conectividad o alguna otra razon relacionada con la bbdd.
            try
            {
                //obtengo los datos.
                DataTable datos = helper.getDatos(comando);
            
            //creo un helper que me servira para obtener las mascotas de cada persona.
            MascotaDal helperMascotas=new MascotaDal();
            
            //Por cada fila en la tabla personas...
            foreach (DataRow row in datos.Rows)
            {
                
                //creo un objeto persona
                Persona persona = new Persona(Convert.ToInt32(row["idpersona"]), row["nombre_persona"].ToString(), Convert.ToInt32(row["edad_persona"]));
                //obtengo la lista de mascotas de esa persona
                List<Mascota> mascotas=helperMascotas.GetListadoPorPersona(persona.IdPersona);
                //asigno la lista de mascotas al objeto persona
                persona.ListaMascotas=mascotas;
                //añado el objeto persona a la lista
                listado.Add(persona);
            }
                }
            catch(SqlException e){
                throw e;
            }
            //devuelvo la lista.
            return listado;

        }

        
        /// <summary>
        /// Metodo que obtiene una Persona por un id
        /// </summary>
        /// <param name="id">id de la persona buscada</param>
        /// <returns>Persona buscada</returns>
        public Persona GetPersonaPorId(int id)
        {
            //Creo el obteno persona que devolveré
            Persona persona = new Persona();

            //comando que ejecutaré
            SqlCommand comando = new SqlCommand("select idpersona,nombre_persona, edad_persona from personas where idpersona=@persona");
            
            //parametros del comando
            SqlParameter parametro = new SqlParameter("@persona", id);
            comando.Parameters.Add(parametro);
            
            //creo el objeto helper que me permitira obtener los datos
            DataBaseHelper helper = new DataBaseHelper();

            //Bloque con sensibilidad de lanzar excepcion en caso de perdida de la conectividad o alguna otra razon relacionada con la bbdd.
            try
            {
                //Obtengo los datos
                DataTable datos = helper.getDatos(comando);

                //Si hay datos
                if (datos.Rows[0] != null)
                {
                    //obtengo la primera fila, ya que solo habrá una fila
                    DataRow row = datos.Rows[0];
                    //recreo el objeto persona con los datos obtenidos de la bbdd
                    persona = new Persona(Convert.ToInt32(row["idpersona"]), row["nombre_persona"].ToString(), Convert.ToInt32(row["edad_persona"]));

                    //Creo un helper de mascotas para obtener la lista de mascotas de esta persona.
                    MascotaDal helperMascotas = new MascotaDal();
                    //obtengo la lista de mascotas o lista vacia si no hubiera
                    List<Mascota> mascotas = helperMascotas.GetListadoPorPersona(persona.IdPersona);
                    //asigno la lista de mascotas
                    persona.ListaMascotas = mascotas;
                }
            }
            catch (SqlException e)
            {
                throw e;
            }
            //devuelvo la persona buscada.
            return persona;
        }


        /// <summary>
        /// Metodo que se encarga de almacenar una persona en la bbdd.
        /// </summary>
        /// <param name="p">Objeto persona a almacenar.</param>
        /// <returns>true si la operacion fue correcta, false en caso contrario</returns>
        public Boolean GuardarPersona(Persona p)
        {
            // Creo na variable booleana que al final contendrá si la operacion fue correcta, o no.
            Boolean opCorrecta = false;

            //creo el comando de insertar
            SqlCommand comando = new SqlCommand("insert into personas(nombre_persona,edad_persona) values(@nombre,@edad)");
            //Establezco los parametros del comando. Nota: la id no la establezco ya que la generará la bbdd automaticamente.
            SqlParameter parametro1 = new SqlParameter("@nombre", p.Nombre);
            SqlParameter parametro2 = new SqlParameter("@edad", p.Edad);
            comando.Parameters.Add(parametro1);
            comando.Parameters.Add(parametro2);

            //Como siempre, creo el objeto helper que me servira para ejecutar la instruccion
            DataBaseHelper helper = new DataBaseHelper();

            //Bloque con sensibilidad de lanzar excepcion en caso de perdida de la conectividad o alguna otra razon relacionada con la bbdd.
            try
            {
                //Ejecuto la instruccion y recojo el valor devuelto.
                opCorrecta = helper.EjecutaNoQuery(comando);
            }
            catch (SqlException e)
            {
                throw e;
            }
            //Devuelve si la operacion fue correcta
            return opCorrecta;
        }


        /// <summary>
        /// Metodo que modifica los datos de una persona en la bbdd
        /// </summary>
        /// <param name="p">Objeto persona a modificar ya con los nuevos datos</param>
        /// <returns>true si la operacion fue correcta, false en caso contrario</returns>
        public Boolean ModificarPersona(Persona p)
        {
            Boolean opCorrecta = false;

            //Creo el objeto comand con la instrucción sql
            SqlCommand comando = new SqlCommand("update personas set nombre_persona=@nombre, edad_persona=@edad where idpersona=@id");

            //Establezco los parametros de la instrucción
            SqlParameter parametro1 = new SqlParameter("@nombre", p.Nombre);
            SqlParameter parametro2 = new SqlParameter("@edad", p.Edad);
            SqlParameter parametro3 = new SqlParameter("@id", p.IdPersona);
            comando.Parameters.Add(parametro1);
            comando.Parameters.Add(parametro2);
            comando.Parameters.Add(parametro3);

            //Objeto helper que me servira pa ejecutar la instruccion
            DataBaseHelper helper = new DataBaseHelper();

            //Bloque con sensibilidad de lanzar excepcion en caso de perdida de la conectividad o alguna otra razon relacionada con la bbdd.
            try
            {
                //Ejecuto la instruccion y recogo el resultado de hacerlo
                opCorrecta = helper.EjecutaNoQuery(comando);
            }
            catch (SqlException e)
            {
                throw e;
            }
            return opCorrecta;
        }

        /// <summary>
        /// Metodo que elimina a una persona y a todas sus mascotas de la bbdd. Habria que hacerlo con transaccion(Si falla eliminar alguna masota, ninguna accion debe realizarse),
        /// pero dado el sentido del ejercicio, no lo complicaremos.
        /// </summary>
        /// <param name="persona">id de la persona a eliminar de la bbdd</param>
        /// <returns>true si la operacion fue correcta, false en caso contrario</returns>
        public Boolean EliminarPersona(int persona)
        {
            Boolean opCorrecta = false;
            //Creo un helper de mascotas para eliminar todas las mascotas de esa persona.
            MascotaDal helperMascotas = new MascotaDal();
            try
            {
                //obtengo la persona de la bbdd, para obtener sus mascotas
                Persona p = GetPersonaPorId(persona);
                foreach (Mascota m in p.ListaMascotas)
                {
                    //Voy eliminando las mascotas de la tabla mascotas que pertencen a esa persona
                    helperMascotas.EliminarMascota(m.IdMascota);
                }

                //Creo el comando para eliminar a la persona
                SqlCommand comando = new SqlCommand("delete from personas where idpersona=@id");
                
                //Establezco el parametro id del comando.
                SqlParameter parametro1 = new SqlParameter("@id", persona);
                parametro1.SqlDbType = SqlDbType.Int;
                comando.Parameters.Add(parametro1);

                DataBaseHelper helper = new DataBaseHelper();

                //Ejecuto la instruccion
                opCorrecta = helper.EjecutaNoQuery(comando);
            }
            catch (SqlException e)
            {
                throw e;
            }

            //devuelvo el resultado de la operacion
            return opCorrecta;

        }

    }
}
