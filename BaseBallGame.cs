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
        Tripie = 3,
        HomeRun = 4,
        StrikeAndFoulBall = 5,
        Ball = 6,
    }

    public enum FieldResult
    {
        Error = 0,
        TagOutAndCatchOut = 1,
    }
    public class BattingStandbyArgs : EventArgs
    {
        public BattingStandbyArgs((int, int) batterBallPos)
        {
            BatterBallPos = batterBallPos;
        }
        public (int, int) BatterBallPos { get; set; }
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

        public BaseBallGame()
        {

        }

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
        }
        public void HandleBattingStandbyEvent(object sender, BattingStandbyArgs e)
        {
            BatterBallPos = e.BatterBallPos;
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
                await fiedlPosSelected();
                await batPosSelected();
                BatResult batResult = battingTeam.Teamplayer[0].HitTheBall(PitcherBallPos, BatterBallPos, true);
                FieldResult fieldResult = fieldingTeam.Teamplayer[0].FieldTheBall(batResult);
                System.Diagnostics.Debug.WriteLine(fieldResult.ToString());
                battingTeamStandby = false;
                fieldingTeamStandby = false;
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
