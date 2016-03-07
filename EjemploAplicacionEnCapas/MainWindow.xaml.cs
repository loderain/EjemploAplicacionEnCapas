using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CapaAccesoADatos;
using CapaDatos;

namespace EjemploAplicacionEnCapas
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        
        
        /// <summary>
        /// Al cargar la ventana, cargamos la lista de personas en el listview de las personas 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CargaPersonas();
        }

        /// <summary>
        /// Metodo que carga la lista de peronas en el ListView LstPersonas ubicado en la vista
        /// </summary>
        private void CargaPersonas()
        {
            //Limpia la lista de personas actual 
            LstPersonas.Items.Clear();

            //Creo el listado de personas que posteriormente mandare al listview para componer la lista
            List<Persona> lista = new List<Persona>();

            //Bloque sensible a las excepciones sql.
            try
            {
                //Obtengo la lista de personas.
                PersonaDal helper = new PersonaDal();
                lista = helper.GetListado();
            }
            catch (SqlException e)
            {
                MessageBox.Show("Problemas al cargar la lista de personas de la bbdd " + e.Message);
            }

            //Añado los objetos persona obtenidos en el listview de la vista.
            foreach (Persona p in lista)
            {
                LstPersonas.Items.Add(p);
            }
        }


        /// <summary>
        /// Metodo que se ejecuta cuando el elimento seleccionado de la lista de personas cambia.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LstPersonas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //limpia los labels de los detalles de la mascota, por si alguna estuviera seleccionada anteriormente
            limpiaCamposMascota();
            //Elimino todos los elementos en la lista de mascotas. Por si hubiera mascotas de la persona que habia seleccionada anteriormente.
            LstMascotas.Items.Clear();
            
            //Obtengo el objeto persona que se a seleccionado en el listview
            Persona p=(Persona)LstPersonas.SelectedItem;

            //si la persona seleccionada no es nula.
            if (p != null)
            {
                //Activo los botones de nueva y eliminar, ya que en caso de haber una persona seleccionada, estas operaciones estan permitidas.
                CambiaEstadoBotones(true);
                
                //Obtengo el listado de mascotas de la persona actualmente seleccionada
                List<Mascota> listamascotas = p.ListaMascotas;
                //Por cada mascota, las voy añadiendo a los items del listview de mascotas.
                foreach (Mascota m in listamascotas)
                {
                    LstMascotas.Items.Add(m);
                }
                
                //Establezco el contenido del label del id  los textbox para el nombre y edad de la persona.
                LblIdPersona.Content = p.IdPersona;
                LblNombrePersona.Text = p.Nombre;
                LblEdadPersona.Text = p.Edad + "";
                
            }
            else
            {
                //Si no hay ninguna persona seleccionada, deshabilito el boton de nueva y eliminar, ya que sin personas seleccionadas, estas operaciones no tienen sentido.
                CambiaEstadoBotones(false);
            }
           

        }

        /// <summary>
        /// Boton que dado un valor true o false, activa o desactiva los botones de eliminar y nueva.
        /// </summary>
        /// <param name="value">true activa, false desactiva</param>
        private void CambiaEstadoBotones(Boolean value)
        {
            BtnNuevaPersona.IsEnabled = value;
            BtnEliminarPersona.IsEnabled =value;
        }


        /// <summary>
        /// Metodo que se ejecuta cuando la seleccion en la lista de mascotas cambie.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LstMascotas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //obtengo la mascota seleccionada en el listado
            Mascota m=(Mascota)LstMascotas.SelectedItem;

            //si no es null, establezco los datos de esa mascota, en caso contrario, limpio los campos.
            if (m != null)
            {
                LblIdMascota.Content = m.IdMascota;
                LblNombreMascota.Content = m.Nombre;
                LblEdadMascota.Content = m.Edad;
            }
            else
            {
                limpiaCamposMascota();
            }
        }

        /// <summary>
        /// Metodo que se ejecuta al hacer clic en nueva
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnNuevaPersona_Click(object sender, RoutedEventArgs e)
        {
            //establece a null el elemento seleccionado de la lista de personas.
            LstPersonas.SelectedItem = null;
            //limpio los campos de la mascota por si alguna fuera seleccionada anteriormente
            limpiaCamposMascota();
            //limpio los campos de persona
            limpiaCamposPersona();
            //cambio el estado de los botones insetar y eliminar.
            CambiaEstadoBotones(false);
        }

        /// <summary>
        /// Metodo que lismpia los campos de la persona
        /// </summary>
        private void limpiaCamposPersona()
        {
            LblNombrePersona.Text = "";
            LblIdPersona.Content = null;
            LblEdadPersona.Text = "";
        }

        /// <summary>
        /// Metodo que lismpia los campos de la mascota
        /// </summary>
        private void limpiaCamposMascota()
        {
            LblNombreMascota.Content = "";
            LblIdMascota.Content = "";
            LblEdadMascota.Content = "";
        }

        
        /// <summary>
        /// Metodo que se ejecuta al hacer clic en el boton guardar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnGuardarPersona_Click(object sender, RoutedEventArgs e)
        {
            //Si el contenido del label id de la persona esta vacio, estoy insertando, en caso contrario, estoy editando
            if (LblIdPersona.Content == null)
            {
                //creo un nuevo objeto persona con los datos indicados por el usuario con los campos de texto
                Persona p = new Persona(-1, LblNombrePersona.Text, Convert.ToInt32(LblEdadPersona.Text));

                PersonaDal helper = new PersonaDal();
                try
                {
                    helper.GuardarPersona(p);
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Error al guardar la persona " + ex.Message);
                }
               
            }
            else
            {
                //obtengo el objeto persona seleccionado en la lista y el cual se modificara.
                Persona p = (Persona)LstPersonas.SelectedItem;
                //pongo el nuevo nombre y edad a la persona
                p.Nombre = LblNombrePersona.Text;
                p.Edad = Convert.ToInt32(LblEdadPersona.Text);
                //Creo un personadal para realizar las operaciones sobre la bbdd
                PersonaDal helper = new PersonaDal();
                try
                {
                    helper.ModificarPersona(p);
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Problemas al editar la persona " + ex.Message);
                }
                
               
            }
            //Vuelvo a recargar la lista de personas con supuestamente la nueva persona si todo fue bien.
            CargaPersonas();
        }


        /// <summary>
        /// Metodo que se ejecuta al pulsar el boton eliminar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnEliminarPersona_Click(object sender, RoutedEventArgs e)
        {
            //Obtengo el objeto seleccionado en la lista de personas
            Persona p = (Persona)LstPersonas.SelectedItem;
            LstPersonas.SelectedItem = null;

            //creo un helper de personas
            PersonaDal helper = new PersonaDal();
            try
            {
                helper.EliminarPersona(p.IdPersona);
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Problemas al eliminar a la persona " + ex.Message);
            }
            //recargo la lista de personas.
           CargaPersonas();
        }

      

        

    }
}
