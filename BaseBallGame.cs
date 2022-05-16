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

    internal class BaseBallGame
    {
        public static Team s_team_1;
        public static Team s_team_2;
        public static Team battingTeam { get; private set; }
        public static Team fieldingTeam { get; private set; }
        public static (int, int) BatterBallPos { get; set; }
        public static (int, int) PitcherBallPos { get; set; }
        private static bool battingTeamStandby { get; set; } = false;
        private static bool fieldingTeamStandby { get; set; } = false;
        private static bool batterChooseBat { get; set; } = false;
        private static OutCount outCount = OutCount.Zero;
        private static Balls balls  = Balls.Zero;
        private static Strikes strikes = Strikes.Zero;

        private static  (int, int) point = (0, 0);


        private bool changeSide { get; set; } = false;

        public BaseBallGame()
        {

        }

        public BaseBallGame(Team team_1, Team team_2)
        {
            s_team_1 = team_1;
            s_team_2 = team_2;
            chooseBattingTeam();
        }
        public void chooseBattingTeam()
        {
            var coin = new Random().Next(2);
            battingTeam = (coin == 0) ? s_team_1 : s_team_2;
            fieldingTeam = (coin == 0) ? s_team_2 : s_team_1;
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
                    await fiedlPosSelected();
                    await batPosSelected();
                    BatResult batResult = battingTeam.Teamplayer[chooseBatter()].HitTheBall(PitcherBallPos, BatterBallPos, batterChooseBat);
                    ProgressBatResult(batResult);
                    battingTeamStandby = false;
                    fieldingTeamStandby = false;
                }
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
        }
        private void movePlayerByNormalBat(BatResult batResult)
        {
            try
            {
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
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                throw;
            }
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
