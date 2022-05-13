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
    public partial class Tabel_Pitcher : UserControl
    {
        private BaseBallGame baseBallGame = new BaseBallGame();
        public Tabel_Pitcher()
        {
            InitializeComponent();
            initPitcherPanel();
        }
        private void initPitcherPanel()
        {
            for (int column = 0; column < tableLayoutPanel_Pitcher.ColumnCount; column++)
                for (int row = 0; row < tableLayoutPanel_Pitcher.RowCount; row++)
                {
                    var buttonPitcher = new Button() { Dock = DockStyle.Fill };
                    if (column < 1 || row < 1 || column > tableLayoutPanel_Pitcher.ColumnCount - 2 || row > tableLayoutPanel_Pitcher.RowCount - 2)
                        buttonPitcher.BackColor = Color.HotPink;
                    else
                        buttonPitcher.BackColor = Color.Green;
                    buttonPitcher.Click += ButtonPitcher_Click;
                    tableLayoutPanel_Pitcher.Controls.Add(buttonPitcher);
                }
        }

        private void ButtonPitcher_Click(object sender, EventArgs e)
        {
            baseBallGame.BatterBallPos = (tableLayoutPanel_Pitcher.GetColumn(sender as Control), tableLayoutPanel_Pitcher.GetRow(sender as Control));
        }
    }
}
