using BattleShipApi.Common;
using BattleShipApi.Constants;
using BattleShipApi.Models;
using BattleShipApi.Persistence;

namespace BattleShipApi.DataProcessing
{
    public interface IBoardDataProcessing
    {
        public Board Create(Board Board);
        public BoardState GetState(string boardID);
        public Board GetByGameIDAndPlayerID(int GameID, int PlayerID);
        public Board GetOpponentBoard(int GameID, int PlayerID);
        public BattleShip CreateBattleShip(BattleShip battleShip);
        void UpdateCommand(string boardID, BattleCommand command, Cell targetCell, AttackResponse attackResponse);
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
            BoardState.BoardStatus = BoardStatus.InProgress;

            _boardStateCache.SetCache(Board.BoardID, BoardState);
            return Board;

        }
        public BoardState GetState(string boardID)
        {
            var boardState = _boardStateCache.GetCache(boardID);

            return boardState;
        }
        public Board GetByGameIDAndPlayerID(int GameID, int PlayerID)
        {

            return null;

        }

        public Board GetOpponentBoard(int GameID, int PlayerID)
        {
            return null;
        }

        public BattleShip CreateBattleShip(BattleShip battleShip)
        {
            battleShip.BattleShipID = UniqueID.Generate();

            var boardState = _boardStateCache.GetCache(battleShip.BoardID);
            boardState.BattleShips.Add(battleShip);

            return battleShip;
        }

        public void UpdateCommand(string boardID, BattleCommand command, Cell targetCell, AttackResponse attackResponse)
        {
            var boardState = _boardStateCache.GetCache(boardID);

            if (attackResponse == AttackResponse.Hit)
            {
                boardState.HitCells.Add(targetCell);
            }
            else
            if (attackResponse == AttackResponse.Miss)
            {
                boardState.MissedCells.Add(targetCell);
            }

        }
    }
}