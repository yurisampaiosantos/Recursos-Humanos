using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Entities;
using BUSINESS;

namespace VIEW
{
    public partial class FrmMain : Form
    {

        

        public FrmMain(string login)
        {
            
            InitializeComponent();
            
            FrmLogin frmLogin = new FrmLogin();
            Employee employeeVw = new Employee();
            
            EmployeeBLL employeeBll = new EmployeeBLL();
            employeeVw = employeeBll.GetEmployee(login);

            lblNome.Text = employeeVw.Name.ToString();
            lblSapNumber.Text = employeeVw.Sap_number.ToString();
            lblemail.Text = employeeVw.Email.ToString();
            lblLogin.Text = employeeVw.Login.ToString();
            lblCpf.Text = employeeVw.Cpf.ToString();

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {

        }

        
        



    }
}
