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
            initLabelText();
            RaiseBattingStandbyEvent += ballGame.HandleBattingStandbyEvent;
        }

        private void initLabelText()
        {
            label_Pitcher.DataBindings.Add("Text", findLinqInPlayer(GarrisonPosition.Pitcher), "Name");
            label_Catcher.DataBindings.Add("Text", findLinqInPlayer(GarrisonPosition.Catcher), "Name");
            label_FirstBaseMan.DataBindings.Add("Text", findLinqInPlayer(GarrisonPosition.FirstBaseMan), "Name");
            label_SecondBaseMan.DataBindings.Add("Text", findLinqInPlayer(GarrisonPosition.SecondBaseMan), "Name");
            label_ThirdBaseMan.DataBindings.Add("Text", findLinqInPlayer(GarrisonPosition.ThirdBaseMan), "Name");
            label_ShortStop.DataBindings.Add("Text", findLinqInPlayer(GarrisonPosition.ShortStop), "Name");
            label_LeftFielder.DataBindings.Add("Text", findLinqInPlayer(GarrisonPosition.LeftFielder), "Name");
            label_CenterFielder.DataBindings.Add("Text", findLinqInPlayer(GarrisonPosition.CenterFielder), "Name");
            label_RightFielder.DataBindings.Add("Text", findLinqInPlayer(GarrisonPosition.RightFielder), "Name");
            label_Batter.DataBindings.Add("Text", BaseBallGame.playerOnBase[0], "PlayerOnBase");
            label_FirstBasePlayer.DataBindings.Add("Text", BaseBallGame.playerOnBase[1], "PlayerOnBase");
            label_SecondBasePlayer.DataBindings.Add("Text", BaseBallGame.playerOnBase[2], "PlayerOnBase");
            label_ThirdBasePlayer.DataBindings.Add("Text", BaseBallGame.playerOnBase[3], "PlayerOnBase");
        }
        private Player findLinqInPlayer(GarrisonPosition pos)
        {
            try
            {
                return (from player in BaseBallGame.fieldingTeam.Teamplayer
                        where player.GarrisonPos.Equals(pos)
                        select player).First();
            }
            catch (IndexOutOfRangeException)
            {
                throw new IndexOutOfRangeException();
            }
            catch (ArgumentOutOfRangeException)
            {
                throw new ArgumentOutOfRangeException();
            }
            catch (Exception)
            {
                throw new Exception("Don't find the equal position,please check the team player!");
            }
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
            BaseBallGame.BatterBallPos = (tableLayoutPanel_Batter.GetColumn(sender as Control), tableLayoutPanel_Batter.GetRow(sender as Control));
        }

        protected virtual void OnRaiseBattingStandbyEvent(BattingStandbyArgs e)
        {
            EventHandler<BattingStandbyArgs> raiseEvent = RaiseBattingStandbyEvent;
            if (raiseEvent != null)
            {
                raiseEvent(this, e);
            }
        }

        private void button_Bat_Click(object sender, EventArgs e)
        {
            OnRaiseBattingStandbyEvent(new BattingStandbyArgs(true));
        }

        private void button_SkipBat_Click(object sender, EventArgs e)
        {
            OnRaiseBattingStandbyEvent(new BattingStandbyArgs(false));
        }
    }
}
