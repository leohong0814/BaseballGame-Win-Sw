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
        private BaseBallGame ballGame = new BaseBallGame();
        public event EventHandler<FieldingStandbyArgs> RaiseFieldingStandbyEvent;

        public Tabel_Pitcher()
        {
            InitializeComponent();
            initPitcherPanel();
            label_Batter.Text = BaseBallGame.battingTeam.Teamplayer[0].Name;
            label_Pitcher.Text = BaseBallGame.fieldingTeam.Teamplayer[0].Name;
            label_Catcher.Text = BaseBallGame.fieldingTeam.Teamplayer[1].Name;
            label_FirstBaseMan.Text = BaseBallGame.fieldingTeam.Teamplayer[2].Name;
            label_SecondBaseMan.Text = BaseBallGame.fieldingTeam.Teamplayer[3].Name;
            label_ThirdBaseMan.Text = BaseBallGame.fieldingTeam.Teamplayer[4].Name;
            label_ShortStop.Text = BaseBallGame.fieldingTeam.Teamplayer[5].Name;
            label_LeftFielder.Text = BaseBallGame.fieldingTeam.Teamplayer[6].Name;
            label_CenterFielder.Text = BaseBallGame.fieldingTeam.Teamplayer[7].Name;
            label_RightFielder.Text = BaseBallGame.fieldingTeam.Teamplayer[8].Name;
            RaiseFieldingStandbyEvent += ballGame.HandleFieldingStandbyEvent;
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
                    tableLayoutPanel_Pitcher.Controls.Add(buttonPitcher, column, row);
                }
        }

        private void ButtonPitcher_Click(object sender, EventArgs e)
        {
            OnRaiseFieldingStandbyEvent(new FieldingStandbyArgs((tableLayoutPanel_Pitcher.GetColumn(sender as Control), tableLayoutPanel_Pitcher.GetRow(sender as Control))));
        }

        protected virtual void OnRaiseFieldingStandbyEvent(FieldingStandbyArgs e)
        {
            EventHandler<FieldingStandbyArgs> raiseEvent = RaiseFieldingStandbyEvent;

            if (raiseEvent != null)
            {
                raiseEvent(this, e);
            }
        }
    }
}
