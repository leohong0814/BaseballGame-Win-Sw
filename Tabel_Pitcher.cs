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
        private BaseBallGame _ballGame;
        public event EventHandler<FieldingStandbyArgs> RaiseFieldingStandbyEvent;

        public Tabel_Pitcher(BaseBallGame ballGame)
        {
            InitializeComponent();
            _ballGame = ballGame;
            initPitcherPanel();
            initLabelText();
            RaiseFieldingStandbyEvent += _ballGame.HandleFieldingStandbyEvent;
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
