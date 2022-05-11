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
            xy_b = Math.Sqrt(Math.Pow(posFromBatter.Item1, 2) + Math.Pow(posFromBatter.Item2, 2));
            double dis = Math.Round(Math.Abs(xy_p - xy_b), 1);
            if(bat == false)
            {
                if (posFromPitcher.Item1 < 2 || posFromPitcher.Item1 > 8 || posFromPitcher.Item2 < 2 || posFromPitcher.Item2 > 8)
                    return BatResult.Ball;
                else
                    return BatResult.StrikeAndFoulBall;
            }
            else
            {
                if (dis < 1) return BatResult.HomeRun;
                else if (dis >= 1 && dis <= 2) return BatResult.Tripie;
                else if (dis > 2 && dis <= 3) return BatResult.Double;
                else if (dis > 3 && dis <= 4.5) return BatResult.Single;
                else if (dis > 4.5F && dis <= 6) return BatResult.GroundBall;
                else  return BatResult.StrikeAndFoulBall;
            }
        }
        public FieldResult FieldTheBall(BatResult batResult)
        {
            FieldResult fieldResult = FieldResult.Error;
            int errorRate = new Random().Next(100);
            switch (batResult)
            {
                case BatResult.GroundBall:
                    {
                        if(errorRate>10) fieldResult = FieldResult.TagOutAndCatchOut;
                        else fieldResult = FieldResult.Error;
                    }
                    break;
                case BatResult.Single:
                    {
                        if (errorRate > 80) fieldResult = FieldResult.TagOutAndCatchOut;
                        else fieldResult = FieldResult.Error;
                    }
                    break;
                case BatResult.Double:
                    {
                        if (errorRate > 90) fieldResult = FieldResult.TagOutAndCatchOut;
                        else fieldResult = FieldResult.Error;
                    }
                    break;
                case BatResult.Tripie:
                    {
                        if (errorRate > 98) fieldResult = FieldResult.TagOutAndCatchOut;
                        else fieldResult = FieldResult.Error;
                    }
                    break;
                case BatResult.HomeRun:
                case BatResult.StrikeAndFoulBall:
                case BatResult.Ball:
                    fieldResult = FieldResult.Error;
                    break;
            }
            return fieldResult;
        }
    }
}
