using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Microsoft.Office.Interop.Excel;
using static System.Net.WebRequestMethods;
using objExcel = Microsoft.Office.Interop.Excel;
using System.IO;
using System.Reflection;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Color = System.Drawing.Color;

namespace Hospital
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public void CleanFind()
        {
            txtNombre.Text = string.Empty;
            txtCodigo.Text = string.Empty;
            txtCedula.Text = string.Empty;
            txtProfesion.Text = string.Empty;
            txtHorasExtra.Text = string.Empty;
            txtSalario.Text = string.Empty;
            cboEstado.Text = string.Empty;
            cboSexo.Text = string.Empty;
            cboCargo.Text = string.Empty;

            txtNombre.Focus();

        }
        string ruta = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        private void btnAgregar_Click(object sender, EventArgs e)
        {
            string Nombre, sexo, Cargo, profesion, estado, cedula;
            int codigo;
            double Salario;
            double tasair1;
            double tasaIR = 0;
            double irx12 = 0;

            double horasExtr, totalDevengado, DeduccionINSS, salarioIR;


            Nombre = txtNombre.Text;
            sexo = cboSexo.Text;
            Cargo = cboCargo.Text;
            profesion = txtProfesion.Text;
            estado = cboEstado.Text;
            codigo = int.Parse(txtCodigo.Text);
            Salario = double.Parse(txtSalario.Text);
            horasExtr = double.Parse(txtHorasExtra.Text);
            cedula = txtCedula.Text;
            double totalHorasExtr = ((Salario / 240) * 2) * horasExtr;
            totalDevengado = Salario + totalHorasExtr;
            DeduccionINSS = (totalDevengado * 6.5) / 100;
            salarioIR = (Salario - DeduccionINSS) * 12;
            double salarioNeto = totalDevengado - DeduccionINSS - tasaIR;
            if (salarioIR == 0 || salarioIR <= 100000)
            {


                tasaIR = 0;
            }
            else if (salarioIR == 100000.01 || salarioIR <= 200000)
            {

                tasair1 = ((salarioIR - 100000) * 15) / 100;
                tasaIR = (tasair1 + 0) / 12;

            }
            else if (salarioIR == 200000.01 || salarioIR <= 350000)
            {
                tasair1 = ((salarioIR - 200000) * 20) / 100;
                tasaIR = (tasair1 + 15000) / 12;
            }
            else if (salarioIR == 350000.01 || salarioIR <= 500000)
            {
                tasair1 = ((salarioIR - 350000) * 25) / 100;
                tasaIR = (tasair1 + 45000) / 12;
            }
            else if (salarioIR == 500000.01 || salarioIR > 500000.01)
            {
                tasair1 = ((salarioIR - 500000) * 30) / 100;
                tasaIR = (tasair1 + 82500) / 12;
            }



            string[] Datos = new string[15];
            Datos[0] = codigo.ToString();
            Datos[1] = Nombre;
            Datos[2] = sexo.ToString();
            Datos[3] = cedula;
            Datos[4] = profesion;
            Datos[5] = Cargo;
            Datos[6] = Salario.ToString();
            Datos[7] = horasExtr.ToString();
            Datos[8] = totalHorasExtr.ToString();
            Datos[9] = totalDevengado.ToString();
            Datos[10] = DeduccionINSS.ToString();
            Datos[11] = tasaIR.ToString();
            Datos[12] = salarioNeto.ToString();
            Datos[13] = estado;

            RegistroDatos.Rows.Add(Datos);
            CleanFind();

        }

        int strFila = 0;

        public void clean()
        {

            int f = RegistroDatos.RowCount;
            for (int i = f - 1; i >= 0; i--)
            {
                RegistroDatos.Rows[i].DefaultCellStyle.BackColor = Color.White;
            }
            for (int i = f - 1; i >= 0; i--)
            {
                this.RegistroDatos.CurrentCell = null;
                this.RegistroDatos.Rows[i].Visible = true;
            }
        }




        private void btnGuardar_Click(object sender, EventArgs e)
        {

            objExcel.Application objAplicacion = new objExcel.Application();
            Workbook objLibro = objAplicacion.Workbooks.Add(XlSheetType.xlWorksheet);
            Worksheet objHoja = (Worksheet)objAplicacion.ActiveSheet;

            objAplicacion.Visible = false;



            foreach (DataGridViewColumn columna in RegistroDatos.Columns)
            {
                objHoja.Cells[1, columna.Index + 1] = columna.HeaderText;
                foreach (DataGridViewRow fila in RegistroDatos.Rows)
                {
                    objHoja.Cells[fila.Index + 2, columna.Index + 1] = fila.Cells[columna.Index].Value;
                }
            }

            objLibro.SaveAs(ruta + "\\Nomina.xlsx");
            objLibro.Close();
            MessageBox.Show("Guardado Exitoso");

        }

        private void btnBorrarTodo_Click(object sender, EventArgs e)
        {
            RegistroDatos.Rows.Clear();
            CleanFind();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {

            DialogResult r = MessageBox.Show("𝓔𝓢𝓣𝓐 𝓢𝓔𝓖𝓤𝓡𝓞 𝓓𝓔 𝓢𝓐𝓛𝓘𝓡?",
                            "Nomina Hospital",
                         MessageBoxButtons.YesNo,
                         MessageBoxIcon.Question);
            if (r == DialogResult.Yes)
                this.Close();
        }

        private void txtNombre_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(txtNombre.Text, "[^a-zA-Z]"))
            {
                MessageBox.Show("Solo ingrese letras!!");
                txtNombre.Text = txtNombre.Text.Remove(txtNombre.Text.Length - 1);
            }
        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtSalario_TextChanged(object sender, EventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(txtSalario.Text, "^[0-9]*$"))
            {
                MessageBox.Show("Por favor solo ingrese numeros.");
                txtSalario.Text = txtSalario.Text.Remove(txtSalario.Text.Length - 1);
            }

        }

        private void btnPdf_Click(object sender, EventArgs e)
        {

            PdfPTable pdfTable = new PdfPTable(RegistroDatos.ColumnCount);
            pdfTable.DefaultCell.Padding = 3;
            pdfTable.WidthPercentage = 70;
            pdfTable.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfTable.DefaultCell.BorderWidth = 1;


            foreach (DataGridViewColumn column in RegistroDatos.Columns)
            {
                PdfPCell cell = new PdfPCell(new Phrase(column.HeaderText));
                cell.BackgroundColor = new iTextSharp.text.BaseColor(240, 240, 240);
                pdfTable.AddCell(cell);
            }


            foreach (DataGridViewRow row in RegistroDatos.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    if (cell.Value == null)
                    {

                    }
                    else
                    {
                        pdfTable.AddCell(cell.Value.ToString());
                    }
                }
            }


            string folderPath = "D:\\";
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            using (FileStream stream = new FileStream(folderPath + "Nomina.pdf", FileMode.Create))
            {
                Document pdfDoc = new Document(PageSize.A2, 10f, 10f, 10f, 0f);
                PdfWriter.GetInstance(pdfDoc, stream);
                pdfDoc.Open();
                pdfDoc.Add(pdfTable);
                pdfDoc.Close();
                stream.Close();
                MessageBox.Show("Guardado Exitoso");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            RegistroDatos.AllowUserToAddRows = false;
            txtBuscar.Enabled = false;
        }
    

        private void txtBuscar_TextChanged_1(object sender, EventArgs e)
        {
            
        }

        private void chBuscar_CheckedChanged(object sender, EventArgs e)
        {
            if (chBuscar.Checked == true)
            {
                txtBuscar.Enabled = true;
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            if (txtBuscar.Text == "")
            {
                RegistroDatos.DefaultCellStyle.BackColor = Color.White;
            }
            else
            {
                foreach (DataGridViewRow Row in RegistroDatos.Rows)
                {


                    strFila = Convert.ToInt32(Row.Index.ToString());
                    string valor = Convert.ToString(Row.Cells["Codigo"].Value);
                    string valor2 = Convert.ToString(Row.Cells["Cedula"].Value);
                    if (valor == this.txtBuscar.Text || valor2 == this.txtBuscar.Text)
                    {
                        this.RegistroDatos.CurrentCell = null;

                        int f = RegistroDatos.RowCount;
                        for (int i = f - 1; i >= 0; i--)
                        {
                            this.RegistroDatos.CurrentCell = null;
                            this.RegistroDatos.Rows[i].Visible = false;
                            this.RegistroDatos.Rows[strFila].Visible = true;
                        }
                        RegistroDatos.Rows[strFila].DefaultCellStyle.BackColor = Color.Green;
                        chBuscar.Checked = false;
                        txtBuscar.Text = String.Empty;
                        txtBuscar.Enabled = false;
                    }
                }
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            clean();
        }
    }


}

    
    
