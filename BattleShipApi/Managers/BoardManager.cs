using System;
using BattleShipApi.Constants;
using BattleShipApi.DataProcessing;
using BattleShipApi.DTOs;
using BattleShipApi.Models;

namespace BattleShipApi.Managers
{
    public class BoardManager : IBoardManager
    {
        private readonly IBoardDataProcessing _boardDataProcessing;
        public BoardManager(IBoardDataProcessing boardDataProcessing)
        {
            _boardDataProcessing = boardDataProcessing;
        }
        public ResultDTO<Board> Add(int gameID, int playerID, Color colorPreference)
        {
            var BoardResult = new ResultDTO<Board>();

            var checkIfBoardExists = _boardDataProcessing.GetByGameIDAndPlayerID(gameID, playerID);

            if (checkIfBoardExists != null)
            {
                return BoardResult.FromError("Board is already created for the player");
            }

            var OpponentBoard = _boardDataProcessing.GetOpponentBoard(gameID, playerID);

            if (OpponentBoard?.Color == colorPreference)
            {
                return BoardResult.FromError("Opponent has selected the same color");
            }

            Board board = new Board(gameID, playerID, DefaultBoardConfig.MaxRows, DefaultBoardConfig.MaxNumberOfShips, DefaultBoardConfig.CanOverlap, colorPreference);

            var newBoard = _boardDataProcessing.Create(board);
            return BoardResult.FromSuccess(newBoard);
        }
        public ResultDTO<BoardState> PlaceBattleShip(string boardID, BattleShipType battleShipType, BattleShipAllignment battleShipAllignment, Cell startingCell)
        {
            var BoardStateResult = new ResultDTO<BoardState>();

            var boardState = _boardDataProcessing.GetState(boardID);

            var IsOverFlowing = WillOverFlow(boardState.Board, battleShipType, battleShipAllignment, startingCell);
            if (IsOverFlowing)
            {
                return BoardStateResult.FromError("Board cells will overflow if the BattleShip is placed");
            }

            if (boardState.NumberOfShipsPlaced > boardState.Board.MaxNumberOfShips)
            {
                return BoardStateResult.FromError("Maximum number of ships placed");
            }

            //TODO: Check overlap
            if (!boardState.Board.canOverLap)
            {

            }


            //TODO: check if battleship already used


            return new ResultDTO<BoardState>();

        }

        private bool WillOverFlow(Board board, BattleShipType battleShipType, BattleShipAllignment battleShipAllignment, Cell startingCell)
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

        public AttackResponse Attack(string boardID, Cell cell)
        {
            throw new System.NotImplementedException();
        }


    }
}