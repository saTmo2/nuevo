using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static proyecto_final__respaldo_.Biblioteca;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace proyecto_final__respaldo_
{
    public partial class Form1 : Form
    {
        private Biblioteca biblioteca;
        private BibliotecaCatalogo bibliotecaCatalogo = new BibliotecaCatalogo();
        private List<Persona> personas; 


        public Form1()
        {
            InitializeComponent();
            this.biblioteca = new Biblioteca();
            bibliotecaCatalogo.Personas = new List<Persona>();
            bibliotecaCatalogo.Materials = new List<Material>();
            personas = new List<Persona>(); 

            CargarDatosDesdeTxt();
            ActualizarDataGridView();
            MessageBox.Show("Bienvenido");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox3.Text) && string.IsNullOrWhiteSpace(textBox4.Text) && comboBox1.SelectedItem == null)
            {
                MessageBox.Show("Por favor, llene todos los campos.");
                return;
            }

            if (string.IsNullOrWhiteSpace(textBox3.Text))
            {
                MessageBox.Show("Por favor, ingrese su nombre.");
                return;
            }

            if (string.IsNullOrWhiteSpace(textBox4.Text))
            {
                MessageBox.Show("Por favor, ingrese su cédula.");
                return;
            }

            if (comboBox1.SelectedItem == null)
            {
                MessageBox.Show("Por favor, seleccione un rol.");
                return;
            }

            int cedula;

            if (!int.TryParse(textBox4.Text, out cedula))
            {
                MessageBox.Show("La cédula no es válida.");
                return;
            }

            bool cedulaRepetida = false;
            foreach (var persona in bibliotecaCatalogo.Personas)
            {
                if (persona.Cedula == cedula)
                {
                    cedulaRepetida = true;
                    break;
                }
            }

            if (cedulaRepetida)
            {
                MessageBox.Show("La cédula ya está registrada.");
                return;
            }

            string nombre = textBox3.Text;
            string rol = comboBox1.SelectedItem.ToString();

            Persona nuevaPersona = new Persona(nombre, cedula, rol);
            bibliotecaCatalogo.Personas.Add(nuevaPersona);

            MessageBox.Show($"Persona Registrada: \nNombre: {nombre}\nCédula: {cedula}\nRol: {rol}");

            textBox3.Clear();
            textBox4.Clear();
            comboBox1.SelectedItem = null;

            ActualizarDataGridView();
            GuardarDatosEnTxt();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text) && string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show("Por favor, llene todos los campos.");
                return;
            }

            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Por favor, ingrese un identificador.");
                return;
            }

            if (string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show("Por favor, ingrese un título.");
                return;
            }

            int cedula;
            if (!int.TryParse(textBox5.Text, out cedula))
            {
                MessageBox.Show("La cédula no es válida.");
                return;
            }

            bool cedulaRegistrada = false;
            Persona personaRegistrada = null;
            foreach (var persona in bibliotecaCatalogo.Personas)
            {
                if (persona.Cedula == cedula)
                {
                    cedulaRegistrada = true;
                    personaRegistrada = persona;
                    break;
                }
            }

            if (!cedulaRegistrada)
            {
                MessageBox.Show("Por favor registrese.");
                return;
            }

            string identificador = textBox1.Text;

            bool identificadorRepetido = false;
            foreach (var material in bibliotecaCatalogo.Materials)
            {
                if (material.Identificador == identificador)
                {
                    identificadorRepetido = true;
                    break;
                }
            }

            if (identificadorRepetido)
            {
                MessageBox.Show("No se puede repetir identificador.");
                return;
            }

            string titulo = textBox2.Text;
            DateTime fecha = dateTimePicker1.Value;
            int cantidadRegistrada = (int)numericUpDown1.Value;
            int cantidadActual = cantidadRegistrada;

            Material nuevoMaterial = new Material(identificador, titulo, fecha, cantidadRegistrada, cantidadActual);
            bibliotecaCatalogo.Materials.Add(nuevoMaterial);

            personaRegistrada.Materiales.Add(nuevoMaterial);

            MessageBox.Show($"Material registrado \nIdentificador: {identificador}\nTítulo: {titulo}\nFecha: {fecha.ToShortDateString()}\nCantidad: {cantidadRegistrada}\nCédula: {cedula}");

            textBox1.Clear();
            textBox2.Clear();
            textBox5.Clear();
            numericUpDown1.Value = 0;

            ActualizarDataGridView();
            GuardarDatosEnTxt();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            GuardarDatosEnTxt();
        }

        private void CargarDatosDesdeTxt()
        {
            if (!File.Exists("datos.txt"))
                return;

            using (StreamReader reader = new StreamReader("datos.txt"))
            {
                string linea;
                Persona personaActual = null;

                while ((linea = reader.ReadLine()) != null)
                {
                    var partes = linea.Split('|');
                    if (partes[0] == "Persona")
                    {
                        personaActual = new Persona(partes[1], int.Parse(partes[2]), partes[3]);
                        bibliotecaCatalogo.Personas.Add(personaActual);
                    }
                    else if (partes[0] == "Material" && personaActual != null)
                    {
                        Material material = new Material(
                            partes[1],
                            partes[2],
                            DateTime.Parse(partes[3]),
                            int.Parse(partes[4]),
                            int.Parse(partes[4])
                        );
                        personaActual.Materiales.Add(material);
                    }
                }
            }
        }

        private void GuardarDatosEnTxt()
        {
            using (StreamWriter writer = new StreamWriter("datos.txt"))
            {
                foreach (var persona in personas)
                {
                    writer.WriteLine($"Persona|{persona.Nombre}|{persona.Cedula}|{persona.Roles}");

                    foreach (var material in persona.Materiales)
                    {
                        writer.WriteLine($"Material|{material.Identificador}|{material.Titulo}|{material.Fecharegistro:yyyy-MM-dd}|{material.Cantidad_registrada}");
                    }
                }
            }
        }


       

        private void button3_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2(bibliotecaCatalogo.Personas);
            form2.Show();
        }

        private void ActualizarDataGridView()
        {
        }

        private void BtnSalir_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnEliminarPersona_Click(object sender, EventArgs e)
        {
            int cedula;
            if (!int.TryParse(textBox8.Text, out cedula))   
            {
                MessageBox.Show("La cédula no es válida.");
                return;
            }

            Persona personaAEliminar = null;

            foreach (var persona in bibliotecaCatalogo.Personas)
            {
                if (persona.Cedula == cedula)
                {
                    personaAEliminar = persona;
                    break;
                }
            }

            if (personaAEliminar == null)  
            {
                MessageBox.Show("La persona no está registrada.");
                return;
            }

            if (personaAEliminar.Materiales.Count > 0)
            {
                MessageBox.Show("No puedes eliminar a una persona que tenga materiales prestados.");
                return;
            }

            bibliotecaCatalogo.Personas.Remove(personaAEliminar);
            MessageBox.Show($"Persona eliminada: {personaAEliminar.Nombre}");

            ActualizarDataGridView();  
            GuardarDatosEnTxt();  
        }

        private void btnIncrementarCantidad_Click(object sender, EventArgs e)
        {
            string identificador = textBox9.Text;  
            Material materialAActualizar = null;

            foreach (var material in bibliotecaCatalogo.Materials)
            {
                if (material.Identificador == identificador)
                {
                    materialAActualizar = material;
                    break;
                }
            }

            if (materialAActualizar == null)  
            {
                MessageBox.Show("El material no existe.");
                return;
            }

            int cantidadAAgregar = (int)numericUpDown2.Value; 
            materialAActualizar.Cantidad_registrada += cantidadAAgregar;
            materialAActualizar.Cantidad_actual += cantidadAAgregar;

            MessageBox.Show($"Cantidad registrada de {materialAActualizar.Titulo} incrementada en {cantidadAAgregar} unidades.");

            ActualizarDataGridView();  
            GuardarDatosEnTxt();
        }

        private void btnAceptar1_Click(object sender, EventArgs e)
        {
            int cedula;
            if (!int.TryParse(textBox6.Text, out cedula))  
            {
                MessageBox.Show("La cédula no es válida.");
                return;
            }

            string identificador = textBox6.Text;  

            Persona persona = null;
            foreach (var p in bibliotecaCatalogo.Personas)
            {
                if (p.Cedula == cedula)
                {
                    persona = p;
                    break;
                }
            }

            if (persona == null)  
            {
                MessageBox.Show("La persona no está registrada.");
                return;
            }

            Material material = null;
            foreach (var m in bibliotecaCatalogo.Materials)
            {
                if (m.Identificador == identificador)
                {
                    material = m;
                    break;
                }
            }

            if (material == null)  
            {
                MessageBox.Show("El material no está disponible.");
                return;
            }

            int maxPrestamos = (persona.Roles == "Estudiante") ? 5 : (persona.Roles == "Profesor") ? 3 : 1;
            if (persona.Materiales.Count >= maxPrestamos)
            {
                MessageBox.Show($"No puedes prestar más de {maxPrestamos} materiales.");
                return;
            }

            if (material.Cantidad_actual <= 0)
            {
                MessageBox.Show("No hay ejemplares disponibles para prestar.");
                return;
            }

            persona.Materiales.Add(material);
            material.Cantidad_actual--;  

            Movimiento movimientoPrestamo = new Movimiento(material, persona, DateTime.Now, Movimiento.tipo.valorPrestamo);
            bibliotecaCatalogo.Movimiento.Add(movimientoPrestamo);

            MessageBox.Show($"Material prestado: {material.Titulo} a {persona.Nombre}.");

            ActualizarDataGridView();

            GuardarDatosEnTxt();
        }


        private void btnAceptar2_Click(object sender, EventArgs e)
        {
            int cedula;
            if (!int.TryParse(textBox4.Text, out cedula))  
            {
                MessageBox.Show("La cédula no es válida.");
                return;
            }

            string identificador = textBox7.Text; 

            Persona persona = null;
            foreach (var p in bibliotecaCatalogo.Personas)
            {
                if (p.Cedula == cedula)
                {
                    persona = p;
                    break;
                }
            }

            if (persona == null) 
            {
                MessageBox.Show("La persona no está registrada.");
                return;
            }

            Material material = null;
            foreach (var m in persona.Materiales)
            {
                if (m.Identificador == identificador)
                {
                    material = m;
                    break;
                }
            }

            if (material == null)  
            {
                MessageBox.Show("La persona no tiene este material prestado.");
                return;
            }

            persona.Materiales.Remove(material);
            material.Cantidad_actual++;  

            Movimiento movimientoDevolucion = new Movimiento(material, persona, DateTime.Now, Movimiento.tipo.valorDevolucion);
            bibliotecaCatalogo.Movimiento.Add(movimientoDevolucion);

            MessageBox.Show($"Material devuelto: {material.Titulo} por {persona.Nombre}.");
            ActualizarDataGridView();  
            GuardarDatosEnTxt();
        }

        private void btnMostrarHistorial_Click(object sender, EventArgs e)
        {
            FormHistorial formHistorial = new FormHistorial(bibliotecaCatalogo.Movimiento);
            formHistorial.Show();
        }

        private void btnBuscarPersona_Click(object sender, EventArgs e)
        {
            int cedula;
            if (!int.TryParse(textBoxBuscarCedula.Text, out cedula))  
            {
                MessageBox.Show("La cédula no es válida.");
                return;
            }

            Persona personaEncontrada = null;
            foreach (var persona in bibliotecaCatalogo.Personas)
            {
                if (persona.Cedula == cedula)
                {
                    personaEncontrada = persona;
                    break;
                }
            }

            if (personaEncontrada == null)
            {
                MessageBox.Show("Persona no encontrada.");
                return;
            }

            MessageBox.Show($"Nombre: {personaEncontrada.Nombre}\nCedula: {personaEncontrada.Cedula}\nRol: {personaEncontrada.Roles}");
        }

        private void btnEliminarMaterial_Click(object sender, EventArgs e)
        {
            // Obtener el identificador ingresado y eliminar espacios en blanco (también de ambos extremos)
            string identificador = textBoxEliminarMaterial.Text.Trim();

            // Validar que el identificador no esté vacío
            if (string.IsNullOrEmpty(identificador))
            {
                MessageBox.Show("Por favor, ingrese un identificador válido.");
                return;
            }

            // Asegurarse de que la lista de materiales esté inicializada
            if (bibliotecaCatalogo == null || bibliotecaCatalogo.Materials == null || bibliotecaCatalogo.Materials.Count == 0)
            {
                MessageBox.Show("No hay materiales registrados.");
                return;
            }

            // Depuración: Verificar el identificador ingresado
            Console.WriteLine($"Identificador ingresado: '{identificador}'");

            // Buscar el material con el identificador proporcionado
            Material materialEliminar = null;

            foreach (var material in bibliotecaCatalogo.Materials)
            {
                // Depuración: Verificar el identificador de cada material en la colección
                Console.WriteLine($"Comparando: '{material.Identificador.ToString()}' con '{identificador}'");

                // Comparar el identificador como texto, insensible a mayúsculas/minúsculas
                if (string.Equals(material.Identificador.ToString(), identificador, StringComparison.OrdinalIgnoreCase))
                {
                    materialEliminar = material;
                    break; // Salir del bucle una vez que se encuentra el material
                }
            }

            // Si no se encontró el material, mostrar un mensaje
            if (materialEliminar == null)
            {
                MessageBox.Show("El material no está registrado.");
                return;
            }

            // Eliminar el material de la lista
            bibliotecaCatalogo.Materials.Remove(materialEliminar);
            MessageBox.Show($"Material eliminado: {materialEliminar.Identificador}");

            // Limpiar el TextBox
            textBox8.Clear();

            // Actualizar la interfaz gráfica y guardar los datos
            ActualizarDataGridView();
            GuardarDatosEnTxt();
        }

        public void EliminarMaterialYActualizar(int identificadorMaterial)
        {
            foreach (var persona in bibliotecaCatalogo.Personas)
            {
                persona.EliminarMaterial(identificadorMaterial);
            }

            ActualizarDataGridView();

            GuardarDatosEnTxt();

            MessageBox.Show("Material eliminado y datos actualizados.");
        }



        public void EliminarMaterialDeArchivo(int identificadorMaterial)
        {
            List<string> lineas = File.ReadAllLines("personas.txt").ToList();

            var nuevasLineas = lineas.Where(linea =>
            {
                string[] campos = linea.Split(',');
                int identificador = int.Parse(campos[2]);   
                return identificador != identificadorMaterial;
            }).ToList();

            File.WriteAllLines("personas.txt", nuevasLineas);
        }

    }
}
