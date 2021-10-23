using BattleShipApi.Common;
using BattleShipApi.Models;
using BattleShipApi.Persistence;

namespace BattleShipApi.DataProcessing
{
    public interface IBoardDataProcessing
    {
        public Board Create(Board Board);
        public Board GetByID(string boardID);
        public Board GetByGameIDAndPlayerID(int GameID, int PlayerID);
        public Board GetOpponentBoard(int GameID, int PlayerID);
    }
    public class BoardDataProcessing : IBoardDataProcessing
    {
        private readonly BoardStateCache _boardStateCache;
        public BoardDataProcessing(BoardStateCache boardStateCache)
        {
            _boardStateCache = boardStateCache;
        }
        public Board Create(Board Board)
        {
            Board.BoardID = UniqueID.Generate();

            var BoardState = new BoardState();
            BoardState.Board = Board;

            _boardStateCache.SetCache(Board.BoardID, BoardState);
            return Board;

        }
        public Board GetByID(string boardID)
        {
            var boardState = _boardStateCache.GetCache(boardID);

            return boardState.Board;
        }
        public Board GetByGameIDAndPlayerID(int GameID, int PlayerID)
        {

            return null;

        }

        public Board GetOpponentBoard(int GameID, int PlayerID)
        {
            return null;
        }

    }
}