using System;
using System.Collections.Generic;
using System.Linq;
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
        public ResultDTO<BoardState> PlaceBattleShip(string boardID, BattleShipDTO battleShipDTO)
        {

            var PlaceBattleShipResult = new ResultDTO<BoardState>();

            var boardState = _boardDataProcessing.GetState(boardID);

            if (boardState == null)
            {
                PlaceBattleShipResult.FromError("Invalid Board ID");
            }

            var battleShipType = (BattleShipType)battleShipDTO.BattleShipType;
            var battleShipAllignment = (BattleShipAllignment)battleShipDTO.BattleShipAllignment;

            var BoardWillOverFlow = CheckIfBoardWillOverFlow(boardState.Board, battleShipType, battleShipAllignment, battleShipDTO.StartingCell);
            if (BoardWillOverFlow)
            {
                return PlaceBattleShipResult.FromError("Board cells will overflow if the BattleShip is placed");
            }

            if (boardState.NumberOfShipsPlaced > boardState.Board.MaxNumberOfShips)
            {
                return PlaceBattleShipResult.FromError("Maximum number of ships placed");
            }

            var battleShip = SetUpShip(boardID, battleShipType, battleShipAllignment, battleShipDTO.StartingCell);

            if (!boardState.Board.canOverLap)
            {
                if (BattleShipWillOverlap(boardState, battleShip))
                {
                    return PlaceBattleShipResult.FromError("BattleShips Will Overlap");
                }
            }


            //TODO: check if battleship already used

            var newBattleShip = _boardDataProcessing.CreateBattleShip(battleShip);

            boardState = _boardDataProcessing.GetState(boardID);


            return PlaceBattleShipResult.FromSuccess(boardState);

        }

        private bool BattleShipWillOverlap(BoardState boardState, BattleShip battleShip)
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

        private BattleShip SetUpShip(string boardID, BattleShipType battleShipType, BattleShipAllignment battleShipAllignment, Cell startingCell)
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

        private bool CheckIfBoardWillOverFlow(Board board, BattleShipType battleShipType, BattleShipAllignment battleShipAllignment, Cell startingCell)
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

        public ResultDTO<AttackResponseDTO> Attack(string boardID, Cell targetCell)
        {
            var AttackResponseResult = new ResultDTO<AttackResponseDTO>();

            var boardState = _boardDataProcessing.GetState(boardID);

            if (boardState == null)
            {
                AttackResponseResult.FromError("Invalid Board ID");
            }

            var CellValid = checkIfCellValid(targetCell, boardState);

            if (CellValid)
            {
                AttackResponseResult.FromError("Invalid Target Cell");
            }

            var CellAlreadyAttacked = checkIfCellsAlreadyAttacked(targetCell, boardState);

            if (CellAlreadyAttacked)
            {
                AttackResponseResult.FromError("Target cell already hit");
            }


            var attackResponse = FindHitOrMiss(boardState, targetCell);

            _boardDataProcessing.UpdateCommand(boardID, BattleCommand.Attack, targetCell, attackResponse);

            var attackResponseDTO = new AttackResponseDTO();
            attackResponseDTO.AttackResponse = attackResponse;

            attackResponseDTO.BoardState = _boardDataProcessing.GetState(boardID);


            return AttackResponseResult.FromSuccess(attackResponseDTO);
        }
        public bool checkIfCellValid(Cell targetCell, BoardState boardState)
        {
            if ((targetCell.RowID <= 0) || (targetCell.ColumnID <= 0))
            {
                return false;
            }

            if ((targetCell.RowID > boardState.Board.MaxRows) || (targetCell.ColumnID > boardState.Board.MaxRows))
            {
                return false;
            }

            return true;
        }
        public bool checkIfCellsAlreadyAttacked(Cell targetCell, BoardState boardState)
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