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
        private BaseBallGame _ballGame;
        public event EventHandler<BattingStandbyArgs> RaiseBattingStandbyEvent;

        public Tabel_Batter(BaseBallGame ballGame)
        {
            InitializeComponent();
            _ballGame = ballGame;
            initBatterPanel();
            initLabelText();
            RaiseBattingStandbyEvent += _ballGame.HandleBattingStandbyEvent;
        }

        private void initLabelText()
        {
            label_Pitcher.DataBindings.Add("Text", findLinqInPlayer(GarrisonPosition.Pitcher), "PlayerOnFieldPos");
            label_Catcher.DataBindings.Add("Text", findLinqInPlayer(GarrisonPosition.Catcher), "PlayerOnFieldPos");
            label_FirstBaseMan.DataBindings.Add("Text", findLinqInPlayer(GarrisonPosition.FirstBaseMan), "PlayerOnFieldPos");
            label_SecondBaseMan.DataBindings.Add("Text", findLinqInPlayer(GarrisonPosition.SecondBaseMan), "PlayerOnFieldPos");
            label_ThirdBaseMan.DataBindings.Add("Text", findLinqInPlayer(GarrisonPosition.ThirdBaseMan), "PlayerOnFieldPos");
            label_ShortStop.DataBindings.Add("Text", findLinqInPlayer(GarrisonPosition.ShortStop), "PlayerOnFieldPos");
            label_LeftFielder.DataBindings.Add("Text", findLinqInPlayer(GarrisonPosition.LeftFielder), "PlayerOnFieldPos");
            label_CenterFielder.DataBindings.Add("Text", findLinqInPlayer(GarrisonPosition.CenterFielder), "PlayerOnFieldPos");
            label_RightFielder.DataBindings.Add("Text", findLinqInPlayer(GarrisonPosition.RightFielder), "PlayerOnFieldPos");
            label_Batter.DataBindings.Add("Text", _ballGame.BaseBagInBaseBall[0], "PlayerOnBagName");
            label_FirstBasePlayer.DataBindings.Add("Text", _ballGame.BaseBagInBaseBall[1], "PlayerOnBagName");
            label_SecondBasePlayer.DataBindings.Add("Text", _ballGame.BaseBagInBaseBall[2], "PlayerOnBagName");
            label_ThirdBasePlayer.DataBindings.Add("Text", _ballGame.BaseBagInBaseBall[3], "PlayerOnBagName");
            label_GameState.DataBindings.Add("Text", _ballGame, "GameState");
        }
        private FieldPos findLinqInPlayer(GarrisonPosition pos)
        {
            try
            {
                return (from playerOnPos in _ballGame.FieldPosInBaseBall
                        where playerOnPos.Pos.Equals(pos)
                        select playerOnPos).First();
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
            _ballGame.BatterBallPos = (tableLayoutPanel_Batter.GetColumn(sender as Control), tableLayoutPanel_Batter.GetRow(sender as Control));
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
