using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaAccesoADatos
{
    class DataBaseHelper
    {
        /// <summary>
        /// Metodo que ejecuta una select sobre la bbdd y devuelve un dataTable con los datos
        /// </summary>
        /// <param name="consulta">Command con la consulta a ejecutar</param>
        /// <returns>datatable con los datos o datatable vacio si no hay datos</returns>
        public DataTable getDatos(SqlCommand consulta)
        {
            //creo un datatable donde se almacena los datos
            DataTable datos = new DataTable();
            try
            {
                //creo conexion a bbdd
                ConexionBuilder connBuilder = new ConexionBuilder();
                SqlConnection conn = connBuilder.getConexion();

                //asigno la conexion
                consulta.Connection = conn;

                //ejecuto el reader
                SqlDataReader Reader = consulta.ExecuteReader();

                //cargo los datos del reader en el datatable
                datos.Load(Reader);

                //cierro la conexion
                conn.Close();
            }
            //relanzamos las excepciones para controlarla en la vista y poder informar al usuario
            catch (SqlException ex)
            {
                
                throw ex;
            }
            catch (InvalidOperationException ex)
            {
                
                throw ex;
            }

            return datos;
        }


        
       
        /// <summary>
        /// Metodo que ejecuta una instruccion insert, update o delete.
        /// </summary>
        /// <param name="consulta"></param>
        /// <returns>True si la operacion fue correcta, false en caso contrario</returns>
        public Boolean EjecutaNoQuery(SqlCommand instruccion)
        {
            Boolean res = false;
            try
            {
                //creo la coonexion
                ConexionBuilder connBuilder = new ConexionBuilder();
                SqlConnection conn = connBuilder.getConexion();

                //asigno la conexion al command
                instruccion.Connection=conn;
                
                //ejecuto el command y cojo las filas afectas
                int filas = instruccion.ExecuteNonQuery();

                if (filas > 0)
                {
                    res = true;
                }

                //cierro la conexion
                conn.Close();
            }
            //relanzamos las excepciones para controlarla en la vista y poder informar al usuario
            catch (SqlException ex)
            {
               
                throw ex;
            }
            catch (InvalidOperationException ex)
            {
                throw ex;
            }

            return res;
        }


        /// <summary>
        /// Metodo que ejecuta un procedimiento sobre la bbdd. 
        /// </summary>
        /// <pre>El procedimiento debe tener un parametro de salida llamado @res de tipo bit</pre>
        /// <param name="comando">Comando a ejecutar</param>
        /// <returns>True si la operacion fue correcta, false en caso contrario</returns>
        public Boolean executeStoredProcedure(SqlCommand comando)
        {
            Boolean opCorrecta = false;
            try
            {
                ConexionBuilder connBuilder = new ConexionBuilder();
                SqlConnection conn = connBuilder.getConexion();
                //establezco el parametro de salida del procedimiento
                SqlParameter outputParam = new SqlParameter("@res", SqlDbType.Bit)
                {
                    Direction = ParameterDirection.Output
                };
                comando.Parameters.Add(outputParam);
                //Asigno la conexion al comando
                comando.Connection = conn;
                comando.ExecuteNonQuery();
                //recogo el valor del parametro de salida del procedimiento
                opCorrecta = Convert.ToBoolean(outputParam.Value.ToString());
                //cierro la conexion
                conn.Close();
            }
            //relanzamos las excepciones para controlarla en la vista y poder informar al usuario
            catch (SqlException ex)
            {
                throw ex;
            }
            catch (InvalidOperationException ex)
            {
               
                throw ex;
            }
            return opCorrecta;

        }


    }
}
