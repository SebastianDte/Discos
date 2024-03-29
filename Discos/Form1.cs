﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using dominio;
using business;
using System.IO;
using System.Threading;
using System.Runtime.InteropServices;
namespace Discos
{
    public partial class FrmDisco : Form
    {
        private HelpersView helpView = new HelpersView();
        private List<Disco> diskList;
        public FrmDisco()
        {
            InitializeComponent();
            helpView.customizeDesing(ref panelSubMenuDiscos, panelSubMenuBuscar);
        }

        [DllImport("user32.dll", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.dll", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);
        private void FrmDisco_Load(object sender, EventArgs e)
        {
            helpView.loadData(ref diskList, ref dgvDisco);
            helpView.hideColumns(ref dgvDisco);
            cboCampo.Items.Add("Artista");
            cboCampo.Items.Add("Titulo");
            cboCampo.Items.Add("Cantidad de Canciones");
            cboCampo.Items.Add("Estilo");
        }
        private void btnAgregar_Click_1(object sender, EventArgs e)
        {
            frmNewDisk newDisk = new frmNewDisk();
            newDisk.ShowDialog();
            helpView.loadData(ref diskList, ref dgvDisco);
        }
        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (dgvDisco.CurrentRow != null)
            {
                Disco select;
                select = (Disco)dgvDisco.CurrentRow.DataBoundItem;
                frmNewDisk updateDisk = new frmNewDisk(select);
                updateDisk.ShowDialog();
                helpView.loadData(ref diskList, ref dgvDisco);
            }
            else { MessageBox.Show("Seleccione un elemento de la lista"); }
        }
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            DiscoNegocio discoNegocio = new DiscoNegocio();
            Disco select;
            try
            {
                if (dgvDisco.CurrentRow != null)
                {
                    DialogResult respuesta = MessageBox.Show("¿Seguro que quieres eliminarlo?", "Eliminando...", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (respuesta == DialogResult.Yes)
                    {
                        select = (Disco)dgvDisco.CurrentRow.DataBoundItem;
                        string rutaImagen = select.UrlImagenTapa;
                        discoNegocio.delete(select.Id);
                        helpView.eliminarImagenLocal(rutaImagen);
                        helpView.loadData(ref diskList, ref dgvDisco);

                    }
                }
                else { MessageBox.Show("Seleccione un elemento de la Lista"); }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }
        private void dgvDisco_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvDisco.CurrentRow != null)
            {
                Disco selected = (Disco)dgvDisco.CurrentRow.DataBoundItem;
                helpView.uploadImage(selected.UrlImagenTapa, pxbDiscos);
            }
        }
        private void btnBuscar_Click(object sender, EventArgs e)
        {
            DiscoNegocio negocio = new DiscoNegocio();
            try
            {
                if (helpView.validarFiltro(ref cboCampo, cboCriterio, txtFiltroAvanzado))
                    return;
                string campo = cboCampo.SelectedItem.ToString();
                string criterio = cboCriterio.SelectedItem.ToString();
                string filtro = txtFiltroAvanzado.Text;
                dgvDisco.DataSource = negocio.filtrar(campo, criterio, filtro);

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }
        private void txtFiltro_TextChanged(object sender, EventArgs e)
        {
            List<Disco> listFilter;
            string filter = txtFiltro.Text.ToUpper();
            if (filter.Length >= 2)
            {
                listFilter = diskList.FindAll(x =>
                x.Titulo.ToUpper().Contains(filter) || 
                x.Artista.ToUpper().Contains(filter) 
                );
            }
            else
            {
                listFilter = diskList;
            }
            dgvDisco.DataSource = null;
            dgvDisco.DataSource = listFilter;
            helpView.hideColumns(ref dgvDisco);
        }
        private void cboCampo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboCampo.SelectedItem != null)
            {
                string option = cboCampo.SelectedItem.ToString();
                if (option == "Cantidad de Canciones")
                {
                    cboCriterio.Items.Clear();
                    cboCriterio.Items.Add("Mayor a");
                    cboCriterio.Items.Add("Menor a");
                    cboCriterio.Items.Add("Igual a");
                }
                else
                {
                    cboCriterio.Items.Clear();
                    cboCriterio.Items.Add("Comienza con");
                    cboCriterio.Items.Add("Termina con");
                    cboCriterio.Items.Add("Contiene");
                }
            }
        }
        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void btnMaximizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            btnMaximizar.Visible = false;
            btnRestaurar.Visible = true;
        }
        private void btnRestaurar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            btnRestaurar.Visible = false;
            btnMaximizar.Visible = true;
        }
        private void btnMinimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        private void btnDiscosPanel_Click(object sender, EventArgs e)
        {
            helpView.showSubMenu(ref panelSubMenuDiscos);
        }
        private void btnBuscarMenu_Click_1(object sender, EventArgs e)
        {
            helpView.showSubMenu(ref panelSubMenuBuscar);
        }
        private void panelContenedor_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }
        private void barraTitulo_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }
        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            if (cboCampo.SelectedItem != null && cboCriterio.SelectedItem != null && txtFiltroAvanzado != null)
            {
                cboCampo.SelectedItem = null;
                cboCriterio.SelectedItem = null;
                txtFiltroAvanzado.Text = string.Empty;
                helpView.loadData(ref diskList, ref dgvDisco);
            }
        }
    }
}








