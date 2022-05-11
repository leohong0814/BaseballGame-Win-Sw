using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public (int,int) PitcherBallPos { get; set; }
        public (int,int) BatterBallPos { get; set; }
        public void PlayBaseballGame()
        {

        }
    }
}
