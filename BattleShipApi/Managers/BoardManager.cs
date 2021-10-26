using System;
using System.Collections.Generic;
using System.Linq;
using BattleShipApi.Constants;
using BattleShipApi.DataProcessing;
using BattleShipApi.DTOs;
using BattleShipApi.Models;
using BattleShipApi.Validators;

namespace BattleShipApi.Managers
{
    public class BoardManager : IBoardManager
    {
        private readonly IBoardDataProcessing _boardDataProcessing;
        private readonly IBattleShipManager _battleShipManager;
        public BoardManager(IBoardDataProcessing boardDataProcessing,
                            IBattleShipManager battleShipManager)
        {
            _boardDataProcessing = boardDataProcessing;
            _battleShipManager = battleShipManager;
        }
        public ResultDTO<Board> Add(AddBoardDTO addBoardDTO)
        {
            var BoardResult = new ResultDTO<Board>();
            var gameID = addBoardDTO.gameID.Value;
            var playerID = addBoardDTO.playerID.Value;
            var colorPreference = addBoardDTO.colorPreference.Value;

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
        public ResultDTO<BoardState> PlaceBattleShip(string boardID, BattleShipDTO battleShipDTO)
        {

            var PlaceBattleShipResult = new ResultDTO<BoardState>();

            var boardState = _boardDataProcessing.GetState(boardID);

            if (boardState == null)
            {
                return PlaceBattleShipResult.FromError("Invalid Board ID");
            }

            var CellValid = CellValidator.IsValid(battleShipDTO.StartingCell, boardState);

            if (!CellValid)
            {
                return PlaceBattleShipResult.FromError("Invalid Target Cell. RowID and Column ID should be greater than 0 and should not exceed maximum number of rows in board");
            }

            var battleShipType = (BattleShipType)battleShipDTO.BattleShipType;
            var battleShipAllignment = (BattleShipAllignment)battleShipDTO.BattleShipAllignment;

            var BoardWillOverFlow = _battleShipManager.WillBoardOverFlowIfShipPlaced(boardState.Board, battleShipType, battleShipAllignment, battleShipDTO.StartingCell);
            if (BoardWillOverFlow)
            {
                return PlaceBattleShipResult.FromError("Board cells will overflow if the BattleShip is placed");
            }

            if (boardState.NumberOfShipsPlaced >= boardState.Board.MaxNumberOfShips)
            {
                return PlaceBattleShipResult.FromError("Maximum number of ships placed");
            }

            var battleShip = _battleShipManager.Setup(boardID, battleShipType, battleShipAllignment, battleShipDTO.StartingCell);

            if (!boardState.Board.canOverLap)
            {
                if (_battleShipManager.WillShipsOverlap(boardState, battleShip))
                {
                    return PlaceBattleShipResult.FromError("BattleShips Will Overlap");
                }
            }


            //TODO: check if battleship already used based on config if battleShips can be used multiple times.

            var newBattleShip = _boardDataProcessing.CreateBattleShip(battleShip);

            boardState = _boardDataProcessing.GetState(boardID);


            return PlaceBattleShipResult.FromSuccess(boardState);

        }

        public ResultDTO<AttackResponseDTO> Attack(string boardID, Cell targetCell)
        {
            var AttackResponseResult = new ResultDTO<AttackResponseDTO>();

            var boardState = _boardDataProcessing.GetState(boardID);

            if (boardState == null)
            {
                return AttackResponseResult.FromError("Invalid Board ID");
            }

            var CellValid = CellValidator.IsValid(targetCell, boardState);

            if (!CellValid)
            {
                return AttackResponseResult.FromError("Invalid Target Cell");
            }

            var CellAlreadyAttacked = checkIfCellsAlreadyAttacked(targetCell, boardState);

            if (CellAlreadyAttacked)
            {
                return AttackResponseResult.FromError("Target cell already hit");
            }


            var attackResponse = FindHitOrMiss(boardState, targetCell);

            _boardDataProcessing.UpdateCommand(boardID, BattleCommand.Attack, targetCell, attackResponse);

            var attackResponseDTO = new AttackResponseDTO();
            attackResponseDTO.AttackResponse = attackResponse;

            attackResponseDTO.BoardState = _boardDataProcessing.GetState(boardID);


            return AttackResponseResult.FromSuccess(attackResponseDTO);
        }
        private bool checkIfCellsAlreadyAttacked(Cell targetCell, BoardState boardState)
        {
            var HitCells = boardState.HitCells;
            var MissedCells = boardState.MissedCells;

            var AllCellsHitOrMissed = HitCells.Concat(MissedCells).ToHashSet();

            if (AllCellsHitOrMissed.Contains(targetCell))
            {
                return true;
            }
            return false;
        }

        private AttackResponse FindHitOrMiss(BoardState boardState, Cell targetCell)
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

            if (CellsInBoardHashSet.Contains(targetCell))
            {
                return AttackResponse.Hit;
            }
            return AttackResponse.Miss;
        }


    }
}