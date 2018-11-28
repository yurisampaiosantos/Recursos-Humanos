using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Negocio;

namespace GoliathAD
{
    public partial class ExtracaoAD : Form
    {
        public ExtracaoAD()
        {
            InitializeComponent();
            Executar.Enabled = true;
        }

        private void Executar_Tick(object sender, EventArgs e)
        {
            Executar.Enabled = false;
            PBI_OracleNeg pBI_OracleNeg = new PBI_OracleNeg();
            pBI_OracleNeg.AtualizarPBI();
            Close();
        }
    }
}
