using BattleShipApi.Constants;
using BattleShipApi.DataProcessing;
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
        public Board AddBoard(int gameID, int playerID, Color colorPreference)
        {
            var checkIfBoardExists = _boardDataProcessing.GetByGameIDAndPlayerID(gameID, playerID);

            if (checkIfBoardExists != null)
            {
                // TODO: Return error in response Model 'Board already created'
                return null;
            }

            var OpponentBoard = GetOpponentBoard(gameID, playerID);

            if (OpponentBoard.Color == colorPreference)
            {
                // TODO: Return error in response Model 'Color is already picked by opponent'
                return null;
            }

            Board board = new Board(gameID, playerID, DefaultBoardConfig.BoardSize, colorPreference);

            var boardCreated = _boardDataProcessing.Create(board);

            return boardCreated;
        }
        public BattleShip AddBattleShipToBoard(int boardID, BattleShipAllignment BattleShipAllignment, Cell StartingCell)
        {
            throw new System.NotImplementedException();
        }

        private Board GetOpponentBoard(int gameID, int PlayerID)
        {

            return new Board();
        }


        public AttackResponse Attack(int boardID, Cell cell)
        {
            throw new System.NotImplementedException();
        }


    }
}