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

            Board board = new Board(gameID, playerID, DefaultBoardConfig.MaxRows, colorPreference);

            var newBoard = _boardDataProcessing.Create(board);
            return BoardResult.FromSuccess(newBoard);
        }
        public ResultDTO<BoardState> PlaceBattleShip(string boardID, BattleShipType battleShipType, BattleShipAllignment BattleShipAllignment, Cell StartingCell)
        {
            var board = _boardDataProcessing.GetByID(boardID);

            //TODO: Check over flow

            //TODO: Check Max BattleShip reached

            //TODO: Check overlap

            //TODO: check if battleship already used


            return new ResultDTO<BoardState>();

        }

        public AttackResponse Attack(string boardID, Cell cell)
        {
            throw new System.NotImplementedException();
        }


    }
}