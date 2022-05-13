using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BaseBallGame
{
    public partial class Form1 : Form
    {
        private BaseBallGame baseBallGame = new BaseBallGame();
        public Form1()
        {
            InitializeComponent();
#if DEBUG_SELF
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.Controls.Add(new Tabel_Pitcher(), 0, 0);
            tableLayoutPanel1.Controls.Add(new Tabel_Batter(), 1, 0);
#else
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.ColumnCount = 1;
#endif
        }
    }
}
