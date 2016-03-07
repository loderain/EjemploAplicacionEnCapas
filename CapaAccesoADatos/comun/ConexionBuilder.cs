using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaAccesoADatos
{
    public class ConexionBuilder
    {
        //AQUI LOS DATOS DE CONEXION A TU SERVIDOR SQL SERVER
        public String server = "localhost";
        public String bbdd = "ejemplo_capas";
        public String usuario = "Usuario de tu bbdd";
        public String pass = "password de tu bbdd";

        public ConexionBuilder() { }

        /// <summary>
        /// Metodo que devuelve una conexion a la bbdd.
        /// </summary>
        /// <returns>Conexion a la bbdd</returns>
        public SqlConnection getConexion()
        {
            //Crea el objeto de conexion a la bbdd, realiza un open y lo devuelve
            SqlConnection conexion = new SqlConnection();
            conexion.ConnectionString = "Data Source=" + server + ";Initial Catalog=" + bbdd + ";user id=" + usuario + ";password=" + pass;
            try
            {
                conexion.Open();
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            return conexion;
        }
    }
}
