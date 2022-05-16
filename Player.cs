using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseBallGame
{ 
    public enum GarrisonPosition
    {
        Pitcher         = 1,
        Catcher         = 2,
        FirstBaseMan    = 3,
        SecondBaseMan   = 4,
        ThirdBaseMan    = 5,
        ShortStop       = 6,
        LeftFielder     = 7,
        CenterFielder   = 8,
        RightFielder    = 9,
    }

    public class Player
    {
        public Player(string name, GarrisonPosition pos)
        {
            Name = name;
            GarrisonPos = pos;
        }
        public string Name { get; private set; }
        public GarrisonPosition GarrisonPos { get; private set; }

        public BatResult HitTheBall((int, int) posFromPitcher, (int, int) posFromBatter, bool bat)
        {
            double xy_p,xy_b;
            xy_p = Math.Sqrt(Math.Pow(posFromPitcher.Item1, 2) + Math.Pow(posFromPitcher.Item2, 2));
            xy_b = Math.Sqrt(Math.Pow(posFromBatter.Item1+1, 2) + Math.Pow(posFromBatter.Item2+1, 2));
            double dis = Math.Round(Math.Abs(xy_p - xy_b), 1);
            if(bat == false)
            {
                if (posFromPitcher.Item1 < 2 || posFromPitcher.Item1 > 8 || posFromPitcher.Item2 < 2 || posFromPitcher.Item2 > 8)
                    return BatResult.Ball;
                else
                    return BatResult.Strike;
            }
            else
            {
                if (dis < 1) return BatResult.HomeRun;
                else if (dis >= 1 && dis <= 2) return BatResult.Double;
                else if (dis > 2 && dis <= 3) return BatResult.Single;
                else if (dis > 3 && dis <= 4.5) return BatResult.Single;
                else if (dis > 4.5F && dis <= 7.5F) return BatResult.GroundBall;
                else  return BatResult.Strike;
            }
        }
    }
}
