using BattleShipApi.Constants;

namespace BattleShipApi.Models
{
    public class Board
    {
        public string BoardID { get; set; }
        public int GameID { get; set; }
        public int PlayerID { get; set; }
        public int Size { get; set; }
        public Color Color { get; set; }

        public Board(int gameID, int playerID, int Size, Color color)
        {
            this.GameID = gameID;
            this.PlayerID = playerID;
            this.Color = color;
        }

        public Board()
        {
        }
    }
}