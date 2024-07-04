using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BookMarketPlace.Core.CustomResponse
{
    public class ResponseNoContent<T>:ICustomResponse<T>
    {
        [JsonIgnore]
        public int StatusCode { get; set; }
        [JsonIgnore]
        public bool IsSuccess { get; set; }

        public List<string> Errors { get; set; }

        public static ResponseNoContent<T> Success(int statusCode)
        {
            return new ResponseNoContent<T> { IsSuccess = true, StatusCode = statusCode };
        } 
        public static ResponseNoContent<T> Error(List<string> errors, int statusCode)
        {
            return new ResponseNoContent<T> { IsSuccess = false, Errors = errors,StatusCode=statusCode };
        } 
    }
}
