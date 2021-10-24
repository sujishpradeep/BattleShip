using System.Collections.Generic;
using BattleShipApi.Constants;
using BattleShipApi.Models;

namespace BattleShipApi.Strategies
{
    public class HorizontalAllignmentStrategy : IBattleShipAllignmentStrategy
    {
        public List<Cell> AddBattleShipToCells(BattleShip battleShip, Cell startingCell)
        {
            var battleShipLength = BattleShipTypeDefaults.GetBattleShipTypeProperty(battleShip.BattleShipType).Length;

            var Row = startingCell.RowID;
            var Column = startingCell.ColumnID;
            for (int i = 0; i < battleShipLength; i++)
            {
                var Cell = new Cell(Row, Column);
                battleShip.CellsOccupied.Add(Cell);
                Column++;
            }
            return battleShip.CellsOccupied;
        }

        public bool CheckIfBoardWillOverFlowWhenShipIsAdded(Board board, BattleShipType battleShipType, Cell startingCell)
        {
            var battleShipLength = BattleShipTypeDefaults.GetBattleShipTypeProperty(battleShipType).Length;

            var batleShipLastColumn = startingCell.ColumnID + battleShipLength;

            if (batleShipLastColumn > board.MaxRows)
            {
                return true;
            }
            return false;

        }

        public BattleShipAllignment GetBattleShipAllignment()
        {
            return BattleShipAllignment.Horizontal;
        }
    }
}