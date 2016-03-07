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
 * create table mascotas(
	idmascota int primary key identity,
	nombre_mascota varchar(40) not null,
	edad_mascota integer not null,
	persona integer foreign key references personas(idpersona))
 */
namespace CapaAccesoADatos
{
    public class MascotaDal
    {

        
        /// <summary>
        /// Metodo que obtiene un listado completo de todas las mascotas disponibles en la bbdd
        /// </summary>
        /// <returns>Listado completo de mascotas</returns>
        public List<Mascota> GetListado()
        {
            //Creo el listado que posteriormente rellenare y devolveré
            List<Mascota> listado = new List<Mascota>();

            //Creo el comando select que me permitira obtener todos los datos de las mascotas de la bbdd
            SqlCommand comando = new SqlCommand("select idmascota,nombre_mascota,edad_mascota from mascotas");

            DataBaseHelper helper = new DataBaseHelper();

            //Bloque sensible a lanzar una excepcion debido a problemas con la bbdd
            try
            {
                //obtengo los datos
                DataTable datos = helper.getDatos(comando);

                //Para cada fila  en la tabla mascotas
                foreach (DataRow row in datos.Rows)
                {
                    //creo un objeto mascota y lo añado al listado creado al principio.
                    Mascota mascota = new Mascota(Convert.ToInt32(row["idmascota"]), row["nombre_mascota"].ToString(), Convert.ToInt32(row["edad_mascota"]));
                    listado.Add(mascota);
                }
            }
            catch (SqlException e)
            {
                //relanzamos la excepcion para controlarla en la vista y poder informar al usuario
                throw e;
            }

            //devuelvo el listado de mascotas.
            return listado;

        }

        //NOTAS

        //LAS CLAVES FORANEAS EN LAS TABLAS, SE PRESTAN MUCHO A CREAR ESTE TIPO DE METODOS QUE VIENE A CONTINUACIÓN, YA QUE UNA RELACION EN LA BBDD(TABLA MASCOTAS CON TABLA PERSONA)
        //EN POO SE CONVIERTEN EN RELACIONES ENTRE OBJETOS, EN NUESTRO CASO, UNA PERSONA TIENE UNA LISTA DE MASCOTAS. 
 
        //PODRIAMOS HABER DICHO TB QUE UNA MASCOTA TIENE UN DUEÑO, EN CUYO CASO TENDRIAMOS QUE TENER UN METODO PARA OBTENER UNA PERSONA POR MASCOTA, PERO PARA NO COMPLICAR EL 
        //EJEMPLO QUE TE ESTOY MONTANDO, NO HE QUERIDO REALIZAR RELACIONES BIDIRECCIONALES. SI NO ENTIENDES ALGO DE ESTO, ME PUEDES PREGUNTAR EN CUALQUIER MOMENTO.



        /// <summary>
        /// Metodo que devuelve el listado de mascotas perteneciente a una persona o vacio, en caso de no tener ninguna.
        /// </summary>
        /// <param name="idPersona">id de la persona de la cual queremos sus mascotas</param>
        /// <returns>Lista de maascotas pertenecientes a la persona indicada</returns>
        public List<Mascota> GetListadoPorPersona(int idPersona)
        {
            //Creo el listado que posteriormente rellenare y devolveré
            List<Mascota> listado = new List<Mascota>();

            //Creo la instruccion sql que me obtendra los datos de las mascotas de una persona
            SqlCommand comando = new SqlCommand("select idmascota,nombre_mascota,edad_mascota from mascotas where persona=@persona");

            //Establezco los parametros
            SqlParameter parametro = new SqlParameter("@persona", idPersona);
            comando.Parameters.Add(parametro);

            //Creo el helper que me servirá para ejecutar la instrucción
            DataBaseHelper helper = new DataBaseHelper();

            //Bloque sensible a lanzar una excepcion debido a problemas con la bbdd
            try
            {
                //obtengo los datos de las mascotas de esa persona
                DataTable datos = helper.getDatos(comando);

                foreach (DataRow row in datos.Rows)
                {
                    //Por cada fila obtenida, creo un objeto mascota, y lo añado a la lista creada al principio.
                    Mascota mascota = new Mascota(Convert.ToInt32(row["idmascota"]), row["nombre_mascota"].ToString(), Convert.ToInt32(row["edad_mascota"]));
                    listado.Add(mascota);
                }
            }
            catch (SqlException e)
            {
                //relanzamos la excepcion para controlarla en la vista y poder informar al usuario
                throw e;
            }

            //Devuelvo el listado
            return listado;

        }

        /// <summary>
        /// Metodo que dado el id de una mascota, devuelve esa mascota.
        /// </summary>
        /// <param name="id">id de la mascota a buscar</param>
        /// <returns>Mascota buscada</returns>
        public Mascota GetMascotaPorId(int id)
        {
            //Creo objeto mascota en principio por si algo sale mal devolver un objeto vacio y evitar la null exception
            Mascota m = new Mascota();

            //creo el comando que me obtendra la mascota
            SqlCommand comando = new SqlCommand("select idmascota,nombre_mascota,edad_mascota from mascotas where idmascota=@mascota");
            //Estabezco el parametro del comando
            SqlParameter parametro = new SqlParameter("@mascota", id);
            comando.Parameters.Add(parametro);

            DataBaseHelper helper = new DataBaseHelper();
            try
            {
                DataTable datos = helper.getDatos(comando);

                DataRow row = datos.Rows[0];
                m = new Mascota(Convert.ToInt32(row["idmascota"]), row["nombre_mascota"].ToString(), Convert.ToInt32(row["edad_mascota"]));
            }
            catch (SqlException e)
            {
                //relanzamos la excepcion para controlarla en la vista y poder informar al usuario
                throw e;
            }
            return m;
        }

        /// <summary>
        /// metodo que almacena una mascota asociada a una persona en la base de datos(Ver anotaciones en profundida en la otra clase dal, ya que es mas de lo mismo)
        /// </summary>
        /// <param name="m">Mascota a guardar</param>
        /// <param name="idpersona">id de la persona dueño de la mascota</param>
        /// <returns>true si la operacion fue correcta, false en caso contrario.</returns>
        public Boolean GuardarMascota(Mascota m,int idpersona)
        {
            Boolean opCorrecta = false;
            SqlCommand comando = new SqlCommand("insert into mascotas(nombre_mascota,edad_mascota,persona) values(@nombre,@edad,@persona)");
            SqlParameter parametro1 = new SqlParameter("@nombre", m.Nombre);
            SqlParameter parametro2 = new SqlParameter("@edad", m.Edad);
            SqlParameter parametro3 = new SqlParameter("@persona", idpersona);
            comando.Parameters.Add(parametro1);
            comando.Parameters.Add(parametro2);
            comando.Parameters.Add(parametro3);
            DataBaseHelper helper = new DataBaseHelper();
            opCorrecta= helper.EjecutaNoQuery(comando);

            return opCorrecta;
        }


        /// <summary>
        /// Metodo que recibe una mascota con los datos ya modificados, para modificarlos en la bbdd
        /// </summary>
        /// <param name="m">Mascota a modificar con los nuevos datos</param>
        /// <returns>true si la operacion fue correcta, false en caso contrario.</returns>
        public Boolean ModificarMascota(Mascota m)
        {
            Boolean opCorrecta = false;
            SqlCommand comando = new SqlCommand("update mascotas set nombre_mascota=@nombre, edad_mascota=@edad where idmascota=@id");
            SqlParameter parametro1 = new SqlParameter("@nombre", m.Nombre);
            SqlParameter parametro2 = new SqlParameter("@edad", m.Edad);
            
            SqlParameter parametro4 = new SqlParameter("@id", m.IdMascota);
            comando.Parameters.Add(parametro1);
            comando.Parameters.Add(parametro2);
            
            comando.Parameters.Add(parametro4);
            DataBaseHelper helper = new DataBaseHelper();
            opCorrecta = helper.EjecutaNoQuery(comando);

            return opCorrecta;
        }

        /// <summary>
        /// Metodo que dado el id de una mascota, lo elimina de la bbdd
        /// </summary>
        /// <param name="idMascota">id de la mascota a eliminar</param>
        /// <returns>true si la operacion fue correcta, false en caso contrario.</returns>
        public Boolean EliminarMascota(int idMascota)
        {
            Boolean opCorrecta = false;

            SqlCommand comando = new SqlCommand("delete from mascotas where idmascota=@id");
            SqlParameter parametro = new SqlParameter("@id", idMascota);
            comando.Parameters.Add(parametro);

            DataBaseHelper helper = new DataBaseHelper();
            opCorrecta = helper.EjecutaNoQuery(comando);

            return opCorrecta;

        }
    }
}
