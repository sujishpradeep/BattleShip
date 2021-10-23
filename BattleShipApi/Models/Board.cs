using BattleShipApi.Constants;

namespace BattleShipApi.Models
{
    public class Board
    {
        public string BoardID { get; set; }
        public int GameID { get; set; }
        public int PlayerID { get; set; }
        public int MaxRows { get; set; }
        public int MaxNumberOfShips { get; set; }
        public bool canOverLap { get; set; }
        public Color Color { get; set; }

        public Board(int gameID, int playerID, int maxRows, int MaxNumberOfShips, bool canOverLap, Color color)
        {
            this.GameID = gameID;
            this.PlayerID = playerID;
            this.Color = color;
            this.MaxRows = maxRows;
            this.MaxNumberOfShips = MaxNumberOfShips;
            this.canOverLap = canOverLap;
        }

        public Board()
        {
        }
    }
}