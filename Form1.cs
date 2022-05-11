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
        }
        private void initPitcherPanel()
        {
            for (int column = 0; column < tableLayoutPanel_Pitcher.ColumnCount; column++)
                for (int row = 0; row < tableLayoutPanel_Pitcher.RowCount; row++)
                {
                    var buttonPitcher = new Button() { Dock = DockStyle.Fill};
                    if(column<1  || row <1 || column > tableLayoutPanel_Pitcher.ColumnCount -1 || row > tableLayoutPanel_Pitcher.RowCount-1)
                        buttonPitcher.BackColor = Color.HotPink;
                    else
                        buttonPitcher.BackColor = Color.Green;
                    buttonPitcher.Click += ButtonPitcher_Click;
                     tableLayoutPanel_Pitcher.Controls.Add(buttonPitcher);
                }
        }

        private void ButtonPitcher_Click(object sender, EventArgs e)
        {
            baseBallGame.PitcherBallPos = (tableLayoutPanel_Pitcher.GetColumn(sender as Control), tableLayoutPanel_Pitcher.GetRow(sender as Control));
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

        private void Form1_Load(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            initPitcherPanel();
            initBatterPanel();
            Cursor = Cursors.Default;
        }
    }
}
