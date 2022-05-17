using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BaseBallGame
{
    public enum BatResult
    {
        GroundBall = 0,
        Single = 1,
        Double = 2,
        HomeRun = 4,
        Strike = 5,
        Ball = 6,
    }
    public enum OutCount
    {
        Zero = 0,
        OneOut = 1,
        TwoOut = 2,
        ThreeOut = 3,
    }
    public enum Balls
    {
        Zero = 0,
        OneOut = 1,
        TwoOut = 2,
        ThreeOut = 3,
        Walks = 4,
    }
    public enum Strikes
    {
        Zero = 0,
        OneOut = 1,
        TwoOut = 2,
        BaseOnBalls = 3,
    }
    public class BattingStandbyArgs : EventArgs
    {
        public BattingStandbyArgs(bool batterChooseBat)
        {
            BatterChooseBat = batterChooseBat;
        }
        public bool BatterChooseBat { get; set; }
    }
    public class FieldingStandbyArgs : EventArgs
    {
        public FieldingStandbyArgs((int, int) pitcherBallPos)
        {
            PitcherBallPos = pitcherBallPos;
        }
        public (int, int) PitcherBallPos { get; set; }
    }

    public class BallIsHitEventArgs : EventArgs
    {
        public BallIsHitEventArgs(BatResult batResult)
        {
            BatResult = batResult;
        }
        public BatResult BatResult { get; set; }
    }

    public class BaseBag : INotifyPropertyChanged
    {
        private string playerOnBagName;
        public string PlayerOnBagName
        {
            get { return playerOnBagName; }
            set {
                playerOnBagName = value;
                OnPropertyChanged("PlayerOnBagName");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    public class FieldPos : INotifyPropertyChanged
    {
        public FieldPos(GarrisonPosition pos) => this.Pos = pos;
        
        public GarrisonPosition Pos { get; }
        private string playerOnFieldPos;
        public string PlayerOnFieldPos
        {
            get { return playerOnFieldPos; }
            set
            {
                playerOnFieldPos = value;
                OnPropertyChanged("PlayerOnFieldPos");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
    public class BaseBallGame : INotifyPropertyChanged
    {
        private static string gameState = string.Empty;
        public string GameState
        {
            get { return gameState; }
            set
            {
                gameState = value;
                OnPropertyChanged("GameState");
            }
        }

        private Team s_team_1;
        private Team s_team_2;
        public Team battingTeam { get; private set; }
        public Team fieldingTeam { get; private set; }
        public (int, int) BatterBallPos { get; set; }
        public (int, int) PitcherBallPos { get; set; }
        private bool battingTeamStandby { get; set; } = false;
        private bool fieldingTeamStandby { get; set; } = false;
        private bool batterChooseBat { get; set; } = false;
        private OutCount outCount = OutCount.Zero;
        private Balls balls  = Balls.Zero;
        private Strikes strikes = Strikes.Zero;
        private bool changeSide { get; set; } = false;
        public (byte, byte) Point = (0, 0);
        public BaseBag[] BaseBagInBaseBall { get; set; } = new BaseBag[4] { 
            new BaseBag() {  PlayerOnBagName = string.Empty},
            new BaseBag() {  PlayerOnBagName = string.Empty},
            new BaseBag() {  PlayerOnBagName = string.Empty},
            new BaseBag() {  PlayerOnBagName = string.Empty}
        };
        public FieldPos[] FieldPosInBaseBall { get; set; } = new FieldPos[9] {
            new FieldPos(GarrisonPosition.Pitcher),
            new FieldPos(GarrisonPosition.Catcher),
            new FieldPos(GarrisonPosition.FirstBaseMan),
            new FieldPos(GarrisonPosition.SecondBaseMan),
            new FieldPos(GarrisonPosition.ThirdBaseMan),
            new FieldPos(GarrisonPosition.ShortStop),
            new FieldPos(GarrisonPosition.LeftFielder),
            new FieldPos(GarrisonPosition.CenterFielder),
            new FieldPos(GarrisonPosition.RightFielder),
        };

        private string findLinqInPlayer(GarrisonPosition pos)
        {
            try
            {
                return (from player in fieldingTeam.Teamplayer
                        where player.GarrisonPos.Equals(pos)
                        select player).First().Name;
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

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        public BaseBallGame()
        { }

        public BaseBallGame(Team team_1, Team team_2)
        {
            s_team_1 = team_1;
            s_team_2 = team_2;
            chooseBattingTeam();
        }
        private void chooseBattingTeam()
        {
            var coin = new Random().Next(2);
            battingTeam = (coin == 0) ? s_team_1 : s_team_2;
            fieldingTeam = (coin == 0) ? s_team_2 : s_team_1;
            setFieldPosAndBatPos();
        }
        private void setFieldPosAndBatPos()
        {
            foreach (var item in FieldPosInBaseBall)
            {
                item.PlayerOnFieldPos = (from player in fieldingTeam.Teamplayer
                                         where player.GarrisonPos.Equals(item.Pos)
                                         select player).First().Name;
            }
            foreach (var item in BaseBagInBaseBall)
            {
                item.PlayerOnBagName = string.Empty;
            }
        }
        public void HandleBattingStandbyEvent(object sender, BattingStandbyArgs e)
        {
            batterChooseBat = e.BatterChooseBat;
            battingTeamStandby = true;
        }
        public void HandleFieldingStandbyEvent(object sender, FieldingStandbyArgs e)
        {
            PitcherBallPos = e.PitcherBallPos;
            fieldingTeamStandby = true;
        }
        public async Task PlayBaseballGameAsync()
        {
            for (int i = 1; i <= 9; i++)
            {
                while (!changeSide)
                {
                    updateGameState();
                    BaseBagInBaseBall[0].PlayerOnBagName = battingTeam.Teamplayer[battingTeam.Batter_Index - 1 % 9].Name;
                    await fiedlPosSelected();
                    await batPosSelected();
                    BatResult batResult = battingTeam.Teamplayer[chooseBatter()].HitTheBall(PitcherBallPos, BatterBallPos, batterChooseBat);
                    System.Diagnostics.Debug.WriteLine(batResult);
                    ProgressBatResult(batResult);
                    battingTeamStandby = false;
                    fieldingTeamStandby = false;
                }
                setFieldPosAndBatPos();
                changeSide = false;
            }
        }
        private void ProgressBatResult(BatResult batResult)
        {
            switch(batResult)
            {
                case BatResult.Strike:
                    strikes += 1;
                    break;
                case BatResult.Ball:
                    balls += 1;
                    break;
                case BatResult.GroundBall:
                    strikes = 0;
                    balls = 0;
                    outCount += 1;
                    battingTeam.Batter_Index += 1;
                    break;
                case BatResult.Single:
                case BatResult.Double:
                    FieldResult fieldResult = fieldingTeam.FieldTheBall(batResult);
                    if (fieldResult == FieldResult.FieldError)
                        movePlayerByNormalBat(batResult);
                    else 
                        movePlayerByCatchOut();
                    break;
                case BatResult.HomeRun:
                    movePlayerByHomeRun();
                    break;
            }
            if(strikes == Strikes.BaseOnBalls)
            {
                strikes = Strikes.Zero;
                outCount += 1;
                battingTeam.Batter_Index += 1;
            }
            if(balls == Balls.Walks)
            {
                movePlayerByNormalBat(BatResult.Single);
            }
            if(outCount == OutCount.ThreeOut)
            {
                Team tmp = battingTeam;
                battingTeam = fieldingTeam;
                fieldingTeam = tmp;
                outCount = OutCount.Zero;
                strikes = Strikes.Zero;
                balls = Balls.Zero;
                changeSide = true;
            }
        }
        
        private int chooseBatter()
        {
            return battingTeam.Batter_Index-1;
        }
        private void movePlayerByCatchOut()
        {
            balls = Balls.Zero;
            strikes = Strikes.Zero;
            outCount += 1;
            battingTeam.Batter_Index += 1;
        }
        private void movePlayerByNormalBat(BatResult batResult)
        {
            try
            {
                for (int i= 3;  i>=0;i--)
                {
                    if(!string.IsNullOrEmpty(BaseBagInBaseBall[i].PlayerOnBagName))
                    {
                        int moveIndex = i + (int)batResult %4;
                        if(i + (int)batResult >= 4)
                        {
                            if (battingTeam == s_team_1) Point.Item1 += 1;
                            else Point.Item2 += 1;
                            BaseBagInBaseBall[i].PlayerOnBagName = string.Empty;
                        }
                        else
                        {
                            BaseBagInBaseBall[moveIndex].PlayerOnBagName = BaseBagInBaseBall[i].PlayerOnBagName;
                            BaseBagInBaseBall[i].PlayerOnBagName = string.Empty;
                        }
                    }
                }

                battingTeam.Batter_Index += 1;
                balls = 0;
                strikes = 0;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Write(ex.ToString());
                throw;
            }
        }
        private void movePlayerByHomeRun()
        {
            try
            {
                int point = (from baseBagInBaseBall in BaseBagInBaseBall
                             where !string.IsNullOrEmpty(baseBagInBaseBall.PlayerOnBagName)
                             select baseBagInBaseBall).Count();
                if (battingTeam == s_team_1) Point.Item1 += (byte)point;
                else Point.Item2 += (byte)point;
                foreach (var item in BaseBagInBaseBall)
                    item.PlayerOnBagName = string.Empty;

                battingTeam.Batter_Index += 1;
                balls = 0;
                strikes = 0;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Write(ex.ToString());
                throw;
            }
        }

        private void updateGameState()
        {
            GameState = $"{s_team_1} : {Point.Item1}\r" +
                        $"{s_team_2} : {Point.Item2}\r" +
                        $"Strikes : {strikes}, Balls : {balls}, Out : {outCount}";
        }
        private async Task fiedlPosSelected()
        {
            await Task.Run(() =>
            {
                while (!fieldingTeamStandby)
                    continue;
            });
        }
        private async Task batPosSelected()
        {
            await Task.Run(() =>
            {
                while (!battingTeamStandby)
                    continue;
            });
        }
    }
}
