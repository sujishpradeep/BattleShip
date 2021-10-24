using System;
using System.Collections.Generic;
using System.Linq;
using BattleShipApi.Constants;
using BattleShipApi.DataProcessing;
using BattleShipApi.DTOs;
using BattleShipApi.Models;
using BattleShipApi.Strategies;

namespace BattleShipApi.Managers
{
    public class BattleShipManager : IBattleShipManager
    {
        private readonly IEnumerable<IBattleShipAllignmentStrategy> _battleShipAllignmentStrategies;

        public BattleShipManager(IEnumerable<IBattleShipAllignmentStrategy> battleShipAllignmentStrategies)
        {
            this._battleShipAllignmentStrategies = battleShipAllignmentStrategies;
        }

        public bool CheckIfBoardWillOverFlowWhenShipIsAdded(Board board, BattleShipType battleShipType, BattleShipAllignment battleShipAllignment, Cell startingCell)
        {

            var battleShipAllignemtStrategy = _battleShipAllignmentStrategies.Where(b => b.GetBattleShipAllignment() == battleShipAllignment).FirstOrDefault();

            if (battleShipAllignemtStrategy != null)
            {
                return battleShipAllignemtStrategy.CheckIfBoardWillOverFlowWhenShipIsAdded(board, battleShipType, startingCell);
            }

            return true;
        }

        public BattleShip Setup(string boardID, BattleShipType battleShipType, BattleShipAllignment battleShipAllignment, Cell startingCell)
        {
            var battleShip = new BattleShip();

            battleShip.BattleShipType = battleShipType;
            battleShip.BoardID = boardID;

            var battleShipAllignemtStrategy = _battleShipAllignmentStrategies.Where(b => b.GetBattleShipAllignment() == battleShipAllignment).FirstOrDefault();

            if (battleShipAllignemtStrategy != null)
            {
                battleShip.CellsOccupied = battleShipAllignemtStrategy.AddBattleShipToCells(battleShip, startingCell);
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