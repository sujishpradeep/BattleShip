using System.Collections.Generic;
using BattleShipApi.Constants;
using BattleShipApi.Models;

namespace BattleShipApi.Strategies
{
    public class VerticalAllignmentStrategy : IBattleShipAllignmentStrategy
    {
        public List<Cell> AddBattleShipToCells(BattleShip battleShip, Cell startingCell)
        {
            var battleShipLength = BattleShipTypeDefaults.GetBattleShipTypeProperty(battleShip.BattleShipType).Length;

            var Column = startingCell.ColumnID;
            var Row = startingCell.RowID;
            for (int i = 0; i < battleShipLength; i++)
            {
                var Cell = new Cell(Row, Column);
                battleShip.CellsOccupied.Add(Cell);
                Row++;
            }
            return battleShip.CellsOccupied;
        }
        public bool CheckIfBoardWillOverFlowWhenShipIsAdded(Board board, BattleShipType battleShipType, Cell startingCell)
        {
            var battleShipLength = BattleShipTypeDefaults.GetBattleShipTypeProperty(battleShipType).Length;

            var batleShipLastRow = startingCell.RowID + battleShipLength;

            if (batleShipLastRow > board.MaxRows)
            {
                return true;
            }
            return false;

        }
        public BattleShipAllignment GetBattleShipAllignment()
        {
            return BattleShipAllignment.Vertical;
        }
    }
}