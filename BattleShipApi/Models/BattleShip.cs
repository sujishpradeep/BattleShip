using System.Collections.Generic;
using BattleShipApi.Constants;

namespace BattleShipApi.Models
{
    public class BattleShip
    {
        public string BattleShipID { get; set; }
        public string BoardID { get; set; }
        public BattleShipType BattleShipType { get; set; }
        public List<Cell> CellsOccupied { get; set; }

        public BattleShip(List<Cell> Cells)
        {
            this.CellsOccupied = Cells;
        }

        public BattleShip()
        {
            this.CellsOccupied = new List<Cell>();
        }
    }
}