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
            initLabelText();
            RaiseFieldingStandbyEvent += ballGame.HandleFieldingStandbyEvent;
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
