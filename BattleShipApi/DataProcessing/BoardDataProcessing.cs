using BattleShipApi.Common;
using BattleShipApi.Models;

namespace BattleShipApi.DataProcessing
{
    public interface IBoardDataProcessing
    {
        public Board Create(Board Board);
        public Board GetByGameIDAndPlayerID(int GameID, int PlayerID);
        public Board GetOpponentBoard(int GameID, int PlayerID);
    }
    public class BoardDataProcesing
    {
        public Board Create(Board Board)
        {
            Board.BoardID = UniqueID.Generate();

            return Board;

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