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
        private BaseBallGame ballGame = new BaseBallGame();
        public event EventHandler<BattingStandbyArgs> RaiseBattingStandbyEvent;

        public Tabel_Batter()
        {
            InitializeComponent();
            initBatterPanel();
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
            RaiseBattingStandbyEvent += ballGame.HandleBattingStandbyEvent;
        }
        private void initBatterPanel()
        {
            for (int column = 0; column < tableLayoutPanel_Batter.ColumnCount; column++)
                for (int row = 0; row < tableLayoutPanel_Batter.RowCount; row++)
                {
                    var buttonBatter = new Button() { BackColor = Color.Green, Dock = DockStyle.Fill, Text = column.ToString() + row.ToString() };
                    buttonBatter.Click += ButtonBatter_Click;
                    tableLayoutPanel_Batter.Controls.Add(buttonBatter, column, row);
                }
        }

        private void ButtonBatter_Click(object sender, EventArgs e)
        {
            OnRaiseBattingStandbyEvent(new BattingStandbyArgs((tableLayoutPanel_Batter.GetColumn(sender as Control), tableLayoutPanel_Batter.GetRow(sender as Control))));
        }

        protected virtual void OnRaiseBattingStandbyEvent(BattingStandbyArgs e)
        {
            EventHandler<BattingStandbyArgs> raiseEvent = RaiseBattingStandbyEvent;
            if (raiseEvent != null)
            {
                raiseEvent(this, e);
            }
        }
    }
}
