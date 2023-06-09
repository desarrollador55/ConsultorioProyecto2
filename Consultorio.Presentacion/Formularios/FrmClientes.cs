﻿using Consultorio.Presentacion.Services;
using Consultorio.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Consultorio.Presentacion.Modelos;

namespace Consultorio.Presentacion.Formularios
{
    public partial class FrmClientes : Form
    {
        private AuthContext _authContext;
        public FrmClientes(AuthContext authContext)
        {
            InitializeComponent();
            _authContext = authContext;
        }
        private async void btnConfirmarCliente_Click(object sender, EventArgs e)
        {
            try
            {
                btnConfirmarCliente.Enabled = false;
                GuardarCliente(
                    tbNombreCliente.Text,
                    tbApellidoCliente.Text,
                    int.Parse(tbEdadCliente.Text),
                    tbDireccionCliente.Text,
                    tbTelefonoCliente.Text
                    );
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }finally
            {
                btnConfirmarCliente.Enabled=true;
            }

        }

        private async Task GuardarCliente(string nombre, string apellido, int edad, string direccion, string telefono)
        {

            var cliente = new Cliente(nombre, apellido, edad, direccion, telefono);

            var clienteService = new ClienteService(_authContext);

            await clienteService.Almacenar(cliente); 

            dtgClientes.DataSource = await clienteService.ConsultarTodos();

        }

       

        private void AlmacenarTxt(Cliente cliente)
        {
            
        }

        private async void FrmClientes_Load(object sender, EventArgs e)
        {
            try
            {
                var clienteService = new ClienteService(_authContext);

                btnConfirmarCliente.Enabled = false;

                dtgClientes.DataSource = await clienteService.ConsultarTodos();

            }
            finally
            {
                btnConfirmarCliente.Enabled = true;

            }
        }
    }
}
