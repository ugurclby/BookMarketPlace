using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BookMarketPlace.Core.CustomResponse
{
    public class Response<T>
    {
        public T Data { get; private set; }
        [JsonIgnore]
        public int StatusCode { get; private set; }
        [JsonIgnore]
        public bool IsSuccess { get; private set; }

        public List<string> Errors { get; set; }

        public static Response<T> Success(T data, int statusCode)
        {
            return new Response<T> { Data = data, IsSuccess = true, StatusCode = statusCode };
        } 
        public static Response<T> Error(List<string> errors, int statusCode)
        {
            return new Response<T> { IsSuccess = false, Errors = errors,StatusCode=statusCode };
        } 
    }
}
