using BattleShipApi.Common;
using BattleShipApi.Models;

namespace BattleShipApi.DataProcessing
{
    public interface IBoardDataProcessing
    {
        public Board Create(Board Board);
        Board GetByGameIDAndPlayerID(int GameID, int PlayerID);
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
    }
}