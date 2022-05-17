using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseBallGame
{
    public enum TeamState
    {
        Batting = 0,
        Fielding = 1,
    }

    public enum FieldResult
    {
        FieldError = 0,
        TagOutAndCatchOut = 1,
    }
    public abstract class Team
    {
        public string Name { get; protected set; }
        public List<Player> Teamplayer { get; protected set; }
        public TeamState State { get; set; }
        public int Batter_Index { get; set; } = 1;

        public FieldResult FieldTheBall(BatResult batResult)
        {
            FieldResult fieldResult = FieldResult.FieldError;
            int errorRate = new Random().Next(100);
            switch (batResult)
            {
                case BatResult.GroundBall:
                    {
                        if (errorRate > 10) fieldResult = FieldResult.TagOutAndCatchOut;
                        else fieldResult = FieldResult.FieldError;
                    }
                    break;
                case BatResult.Single:
                    {
                        if (errorRate > 80) fieldResult = FieldResult.TagOutAndCatchOut;
                        else fieldResult = FieldResult.FieldError;
                    }
                    break;
                case BatResult.Double:
                    {
                        if (errorRate > 90) fieldResult = FieldResult.TagOutAndCatchOut;
                        else fieldResult = FieldResult.FieldError;
                    }
                    break;
                case BatResult.HomeRun:
                case BatResult.Strike:
                case BatResult.Ball:
                    fieldResult = FieldResult.FieldError;
                    break;
            }
            System.Diagnostics.Debug.WriteLine(fieldResult);
            return fieldResult;
        }
    }
    public class NewYorkTeam : Team
    {
        public NewYorkTeam()
        {
            Name = "NewYork";
            Teamplayer = new List<Player>()
            {
                new Player("NY_Player_1", GarrisonPosition.Pitcher),
                new Player("NY_Player_2", GarrisonPosition.Catcher),
                new Player("NY_Player_3", GarrisonPosition.FirstBaseMan),
                new Player("NY_Player_4", GarrisonPosition.SecondBaseMan),
                new Player("NY_Player_5", GarrisonPosition.ThirdBaseMan),
                new Player("NY_Player_6", GarrisonPosition.ShortStop),
                new Player("NY_Player_7", GarrisonPosition.LeftFielder),
                new Player("NY_Player_8", GarrisonPosition.CenterFielder),
                new Player("NY_Player_9", GarrisonPosition.RightFielder),
            };
        }
    }
    public class BostonTeam : Team
    {
        public BostonTeam()
        {
            Name = "NewYork";
            Teamplayer = new List<Player>()
            {
                new Player("Bos_Player_1", GarrisonPosition.Pitcher),
                new Player("Bos_Player_2", GarrisonPosition.Catcher),
                new Player("Bos_Player_3", GarrisonPosition.FirstBaseMan),
                new Player("Bos_Player_4", GarrisonPosition.SecondBaseMan),
                new Player("Bos_Player_5", GarrisonPosition.ThirdBaseMan),
                new Player("Bos_Player_6", GarrisonPosition.ShortStop),
                new Player("Bos_Player_7", GarrisonPosition.LeftFielder),
                new Player("Bos_Player_8", GarrisonPosition.CenterFielder),
                new Player("Bos_Player_9", GarrisonPosition.RightFielder),
            };
        }
    }
}
