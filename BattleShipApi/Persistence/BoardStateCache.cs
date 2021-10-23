using BattleShipApi.Models;

namespace BattleShipApi.Persistence
{
    public class BoardStateCache
    {
        private readonly ICacheProvider _cacheProvider;
        public BoardStateCache(ICacheProvider cacheProvider)
        {
            _cacheProvider = cacheProvider;
        }

        public BoardState GetCache(string boardID)
        {
            return _cacheProvider.GetFromCache<BoardState>(boardID);
        }
        public void SetCache(string boardID, BoardState boardState)
        {
            _cacheProvider.SetValue(boardID, boardState);
        }
    }
}