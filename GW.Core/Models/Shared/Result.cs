namespace GW.Core.Models.Shared
{
    public class Result
    {
        public bool Success { get; set; }
        public string ErrorCode { get; set; }
        public string Message { get; set; }

        public static Result Ok() => new Result { Success = true };

        public static Result Fail(string errorCode, string message)
            => new Result { Success = false, ErrorCode = errorCode, Message = message };
    }

    public class Result<T> : Result
    {
        public T Data { get; set; }

        public static Result<T> Ok(T data) => new Result<T> { Success = true, Data = data };

        public static new Result<T> Fail(string errorCode, string message)
            => new Result<T> { Success = false, ErrorCode = errorCode, Message = message };
    }
}
