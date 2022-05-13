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
    public partial class Tabel_Batter : UserControl
    {
        private BaseBallGame baseBallGame = new BaseBallGame();
        public Tabel_Batter()
        {
            InitializeComponent();
            initBatterPanel();
        }
        private void initBatterPanel()
        {
            for (int column = 0; column < tableLayoutPanel_Batter.ColumnCount; column++)
                for (int row = 0; row < tableLayoutPanel_Batter.RowCount; row++)
                {
                    var buttonBatter = new Button() { BackColor = Color.Green, Dock = DockStyle.Fill };
                    buttonBatter.Click += ButtonBatter_Click;
                    tableLayoutPanel_Batter.Controls.Add(buttonBatter);
                }
        }

        private void ButtonBatter_Click(object sender, EventArgs e)
        {
            baseBallGame.BatterBallPos = (tableLayoutPanel_Batter.GetColumn(sender as Control), tableLayoutPanel_Batter.GetRow(sender as Control));
        }
    }
}
