using BattleShipApi.Constants;

namespace BattleShipApi.DTOs
{
    public class ResultDTO<T>
    {
        private ResultStatus Status { get; set; }
        public bool IsError => Status == ResultStatus.Error;
        public bool IsSuccess => Status == ResultStatus.Success;
        public string ErrorMessage { get; private set; }
        public T Result { get; private set; }

        public ResultDTO<T> FromError(string errorMessage)
        {
            var ResultDTO = new ResultDTO<T>();
            ResultDTO.Status = ResultStatus.Error;
            ResultDTO.ErrorMessage = errorMessage;
            return ResultDTO;
        }

        public ResultDTO<T> FromSuccess(T Result)
        {
            var ResultDTO = new ResultDTO<T>();
            ResultDTO.Status = ResultStatus.Success;
            ResultDTO.Result = Result;
            return ResultDTO;
        }
    }
}