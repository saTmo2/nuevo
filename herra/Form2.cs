using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

using static proyecto_final__respaldo_.Biblioteca;


namespace proyecto_final__respaldo_
{
    public partial class Form2 : Form
    {
        private List<Persona> personas;

        public Form2(List<Persona> personas)
        {
            InitializeComponent();
            this.personas = personas;

            ConfigurarDataGridView();
            ActualizarDataGridView();

            MessageBox.Show("Base de Datos");
        }

        private void dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void ConfigurarDataGridView()
        {
            dataGridView.Columns.Clear();

            dataGridView.Columns.Add("Nombre", "Nombre");
            dataGridView.Columns.Add("Cedula", "Cédula");
            dataGridView.Columns.Add("Identificador", "Identificador");
            dataGridView.Columns.Add("Titulo", "Título");
            dataGridView.Columns.Add("FechaRegistro", "Fecha de Registro");
            dataGridView.Columns.Add("CantidadRegistrada", "Cantidad Registrada");
        }

        public void ActualizarDataGridView()
        {
            dataGridView.Rows.Clear();

            if (personas == null || personas.Count == 0)
            {
                MessageBox.Show("No hay personas registradas.");
                return;
            }

            foreach (var persona in personas)
            {
                bool personaAgregada = false;

                foreach (DataGridViewRow row in dataGridView.Rows)
                {
                    if (row.Cells[1].Value != null && row.Cells[1].Value.ToString() == persona.Cedula.ToString())
                    {
                        personaAgregada = true;
                        break;
                    }
                }

                if (!personaAgregada)
                {
                    var personaFila = new DataGridViewRow();
                    personaFila.CreateCells(dataGridView, persona.Nombre, persona.Cedula.ToString(), "", "", "", "");
                    dataGridView.Rows.Add(personaFila);
                }

                foreach (var material in persona.Materiales)
                {
                    var materialFila = new DataGridViewRow();
                    materialFila.CreateCells(dataGridView,
                        persona.Nombre,
                        persona.Cedula.ToString(),
                        material.Identificador,
                        material.Titulo,
                        material.Fecharegistro.ToShortDateString(),
                        material.Cantidad_registrada);
                    dataGridView.Rows.Add(materialFila);
                }
            }
        }

        public void AgregarPersona(Persona nuevaPersona)
        {
            personas.Add(nuevaPersona);
            ActualizarDataGridView();
        }
    }
}
