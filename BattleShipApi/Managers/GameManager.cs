using BattleShipApi.Constants;
using BattleShipApi.DataProcessing;
using BattleShipApi.DTOs;
using BattleShipApi.Models;

namespace BattleShipApi.Managers
{
    public class GameManager : IGameManager
    {
        private readonly IBoardDataProcessing _boardDataProcessing;
        public GameManager(IBoardDataProcessing boardDataProcessing)
        {
            _boardDataProcessing = boardDataProcessing;
        }
        public ResultDTO<Board> AddBoard(int gameID, int playerID, Color colorPreference)
        {
            var BoardResult = new ResultDTO<Board>();

            var checkIfBoardExists = _boardDataProcessing.GetByGameIDAndPlayerID(gameID, playerID);

            if (checkIfBoardExists != null)
            {
                return BoardResult.FromError("Board is already created for the player");
            }

            var OpponentBoard = _boardDataProcessing.GetOpponentBoard(gameID, playerID);

            if (OpponentBoard.Color == colorPreference)
            {
                return BoardResult.FromError("Opponent has selected the same color");
            }

            Board board = new Board(gameID, playerID, DefaultBoardConfig.BoardSize, colorPreference);

            var newBoard = _boardDataProcessing.Create(board);
            return BoardResult.FromSuccess(newBoard);
        }
        public BattleShip AddBattleShipToBoard(int boardID, BattleShipAllignment BattleShipAllignment, Cell StartingCell)
        {
            throw new System.NotImplementedException();
        }

        public AttackResponse Attack(int boardID, Cell cell)
        {
            throw new System.NotImplementedException();
        }


    }
}