using Datos;
using Entidades;
using System;
using System.Data;
using System.Windows.Forms;

namespace Vista
{
    public partial class ClientesForm : Syncfusion.Windows.Forms.Office2010Form
    {
        public ClientesForm()
        {
            InitializeComponent();
        }

        string tipoOperacion;

        DataTable dt = new DataTable();
        ClienteDB ClienteDB = new ClienteDB();
        Cliente cliente = new Cliente();

        private void NuevoButton_Click(object sender, System.EventArgs e)
        {
            NIdentidadTextBox.Focus();
            HabilitarControles();
            tipoOperacion = "Nuevo";
        }

        private void HabilitarControles()
        {
            NIdentidadTextBox.Enabled = true;
            NombreTextBox.Enabled = true;
            TelefonoTextBox.Enabled = true;
            CorreoTextBox.Enabled = true;
            DireccionTextBox.Enabled = true;
            NacimientoDateTimePicker.Enabled = true;
            EstaActivoCheckBox.Enabled = true;
            CancelarButton.Enabled = true;
            ModificarButton.Enabled = false;
        }


        private void ModificarButton_Click(object sender, System.EventArgs e)
        {
            tipoOperacion = "Modificar";

            if (ClientesDataGridView.SelectedRows.Count > 0)
            {
                NIdentidadTextBox.Text = ClientesDataGridView.CurrentRow.Cells["Identidad"].Value.ToString();
                NombreTextBox.Text = ClientesDataGridView.CurrentRow.Cells["Nombre"].Value.ToString();
                CorreoTextBox.Text = ClientesDataGridView.CurrentRow.Cells["Correo"].Value.ToString();
                DireccionTextBox.Text = ClientesDataGridView.CurrentRow.Cells["Direccion"].Value.ToString();
                NacimientoDateTimePicker.Value = Convert.ToDateTime(ClientesDataGridView.CurrentRow.Cells["FechaNacimiento"]);
                EstaActivoCheckBox.Checked = Convert.ToBoolean(ClientesDataGridView.CurrentRow.Cells["EstaActivo"].Value);

                HabilitarControles();
            }
            else
            {
                MessageBox.Show("Seleccione un registro");
            }
        }


        private void CancelarButton_Click(object sender, System.EventArgs e)
        {
            DeshabilitarControles();
            LimpiarControles();
        }

        private void DeshabilitarControles()
        {
            NIdentidadTextBox.Enabled = false;
            NIdentidadTextBox.Enabled = false;
            NombreTextBox.Enabled = false;
            TelefonoTextBox.Enabled = false;
            CorreoTextBox.Enabled = false;
            DireccionTextBox.Enabled = false;
            NacimientoDateTimePicker.Enabled = false;
            EstaActivoCheckBox.Enabled = false;
            CancelarButton.Enabled = false;
            ModificarButton.Enabled = true;
        }

        private void LimpiarControles()
        {
            NIdentidadTextBox.Clear();
            NIdentidadTextBox.Clear();
            NombreTextBox.Clear();
            TelefonoTextBox.Clear();
            CorreoTextBox.Clear();
            DireccionTextBox.Clear();
            NacimientoDateTimePicker = null;
            EstaActivoCheckBox.Enabled = false;
        }

        private void GuardarButton_Click(object sender, System.EventArgs e)
        {

            if (tipoOperacion == "Nuevo")
            {
                if (string.IsNullOrEmpty(NIdentidadTextBox.Text))
                {
                    errorProvider1.SetError(NIdentidadTextBox, "Ingrese el número de identidad");
                    NIdentidadTextBox.Focus();
                    return;
                }
                errorProvider1.Clear();

                if (string.IsNullOrEmpty(NombreTextBox.Text))
                {
                    errorProvider1.SetError(NombreTextBox, "Ingrese el nombre del cliente");
                    NombreTextBox.Focus();
                    return;
                }
                errorProvider1.Clear();


                cliente.Identidad = NIdentidadTextBox.Text;
                cliente.Nombre = NombreTextBox.Text;
                cliente.Telefono = TelefonoTextBox.Text;
                cliente.Correo = CorreoTextBox.Text;
                cliente.Direccion = DireccionTextBox.Text;
                cliente.FechaNacimiento = NacimientoDateTimePicker.Value;
                cliente.EstaActivo = EstaActivoCheckBox.Checked;


                //INSERTAR EN LA BASE DE DATOS

                bool inserto = ClienteDB.InsertarCliente(cliente);

                if (inserto)
                {
                    LimpiarControles();
                    DeshabilitarControles();
                    TraerClientes();
                    MessageBox.Show("Registro del Cliente Guardado");

                }
                else
                {
                    MessageBox.Show("No se pudo guardar el registro del cliente");
                }
            }
            else
                if (tipoOperacion == "Modificar")
            {
                cliente.Identidad = NIdentidadTextBox.Text;
                cliente.Nombre = NombreTextBox.Text;
                cliente.Telefono = TelefonoTextBox.Text;
                cliente.Correo = CorreoTextBox.Text;
                cliente.Direccion = DireccionTextBox.Text;
                cliente.FechaNacimiento = NacimientoDateTimePicker.Value;
                cliente.EstaActivo = EstaActivoCheckBox.Checked;

                bool modifico = ClienteDB.EditarCliente(cliente);

                if (modifico)
                {
                    LimpiarControles();
                    DeshabilitarControles();
                    TraerClientes();
                    MessageBox.Show("El registro se actualizó correctamente");
                }
                else
                {
                    MessageBox.Show("No se pudo actualizar el registro");
                }
            }

        }

        private void ClientesForm_Load(object sender, EventArgs e)
        {
            TraerClientes();
        }

        private void TraerClientes()
        {
            dt = ClienteDB.DevolverClientes();

            ClientesDataGridView.DataSource = dt;
        }

        private void EliminarButton_Click(object sender, EventArgs e)
        {
            if (ClientesDataGridView.SelectedRows.Count > 0)
            {
                bool elimino = ClienteDB.EliminarCliente(ClientesDataGridView.CurrentRow.Cells["Identidad"].Value.ToString());

                if (elimino)
                {
                    LimpiarControles();
                    DeshabilitarControles();
                    TraerClientes();
                    MessageBox.Show("Registro eliminado");
                }
                else
                {
                    MessageBox.Show("No se pudo eliminar del registro");
                }
            }
            else
            {
                MessageBox.Show("Seleccione un registro para eliminar");
            }

        }
    }


}
