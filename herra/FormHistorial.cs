using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static proyecto_final__respaldo_.Biblioteca;

namespace proyecto_final__respaldo_
{
    public partial class FormHistorial : Form
    {
        private List<Movimiento> movimientos;

        public FormHistorial(List<Movimiento> movimientos)
        {
            InitializeComponent();
            this.movimientos = movimientos;
            ConfigurarDataGridView();
            MostrarHistorial();
        }

        private void ConfigurarDataGridView()
        {
            dataGridViewHistorial.Columns.Clear();

            dataGridViewHistorial.Columns.Add("Fecha", "Fecha");
            dataGridViewHistorial.Columns.Add("Tipo", "Tipo de Movimiento");
            dataGridViewHistorial.Columns.Add("Persona", "Persona");
            dataGridViewHistorial.Columns.Add("Titulo", "Título del Material");
        }

        private void MostrarHistorial()
        {
            dataGridViewHistorial.Rows.Clear();

            foreach (var movimiento in movimientos)
            {
                string tipoMovimiento = movimiento.Tip.ToString();  
                string nombrePersona = movimiento.Persona.Nombre;
                string tituloMaterial = movimiento.Material.Titulo;

                dataGridViewHistorial.Rows.Add(
                    movimiento.Fecha_movimiento.ToShortDateString(),  
                    tipoMovimiento,                        
                    nombrePersona,                         
                    tituloMaterial                         
                );
            }
        }
    }

}
