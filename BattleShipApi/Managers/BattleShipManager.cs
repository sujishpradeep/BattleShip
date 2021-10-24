using System;
using System.Collections.Generic;
using System.Linq;
using BattleShipApi.Constants;
using BattleShipApi.DataProcessing;
using BattleShipApi.DTOs;
using BattleShipApi.Models;

namespace BattleShipApi.Managers
{
    public class BattleShipManager : IBattleShipManager
    {


        public bool CheckIfBoardWillOverFlowWhenShipIsAdded(Board board, BattleShipType battleShipType, BattleShipAllignment battleShipAllignment, Cell startingCell)
        {
            var battleShipLength = BattleShipTypeDefaults.GetBattleShipTypeProperty(battleShipType).Length;
            var maxRows = board.MaxRows;

            if (battleShipAllignment == BattleShipAllignment.Horizontal)
            {
                var batleShipLastColumn = startingCell.ColumnID + battleShipLength;

                if (batleShipLastColumn > maxRows)
                {
                    return true;
                }
            }
            if (battleShipAllignment == BattleShipAllignment.Vertical)
            {
                var batleShipLastRow = startingCell.RowID + battleShipLength;

                if (batleShipLastRow > maxRows)
                {
                    return true;
                }
            }

            return false;
        }

        public BattleShip Setup(string boardID, BattleShipType battleShipType, BattleShipAllignment battleShipAllignment, Cell startingCell)
        {
            var battleShip = new BattleShip();

            battleShip.BattleShipType = battleShipType;
            battleShip.BoardID = boardID;

            var battleShipLength = BattleShipTypeDefaults.GetBattleShipTypeProperty(battleShipType).Length;


            if (battleShipAllignment == BattleShipAllignment.Horizontal)
            {
                var Row = startingCell.RowID;
                var Column = startingCell.ColumnID;
                for (int i = 0; i < battleShipLength; i++)
                {
                    var Cell = new Cell(Row, Column);
                    battleShip.CellsOccupied.Add(Cell);
                    Column++;
                }
            }
            if (battleShipAllignment == BattleShipAllignment.Vertical)
            {
                var Column = startingCell.ColumnID;
                var Row = startingCell.RowID;
                for (int i = 0; i < battleShipLength; i++)
                {
                    var Cell = new Cell(Row, Column);
                    battleShip.CellsOccupied.Add(Cell);
                    Row++;
                }
            }
            return battleShip;
        }

        public bool WillShipsOverlap(BoardState boardState, BattleShip battleShip)
        {
            var CellsInBoard = new List<Cell>();

            boardState.BattleShips.ForEach(b =>
            {
                b.CellsOccupied.ForEach(c =>
                {
                    CellsInBoard.Add(c);
                });
            });

            var CellsInBoardHashSet = CellsInBoard.ToHashSet();
            var IsOverLapping = false;

            battleShip.CellsOccupied.ForEach(cell =>
            {
                if (CellsInBoardHashSet.Contains(cell))
                {
                    IsOverLapping = true;

                }
            });

            return IsOverLapping;

        }



    }
}