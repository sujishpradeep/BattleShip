using System.Collections.Generic;
using BattleShipApi.Constants;

namespace BattleShipApi.Models
{
    public class BoardState : Board
    {
        public BoardStatus BoardStatus { get; set; }
        public List<Cell> HitCells { get; set; }
        public List<Cell> MissedCells { get; set; }
        public List<BattleShip> BattleShips { get; set; }
    }
}