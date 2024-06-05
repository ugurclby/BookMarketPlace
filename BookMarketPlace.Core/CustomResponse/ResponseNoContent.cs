using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BookMarketPlace.Core.CustomResponse
{
    public class ResponseNoContent
    {
        [JsonIgnore]
        public int StatusCode { get; private set; }
        [JsonIgnore]
        public bool IsSuccess { get; private set; }

        public List<string> Errors { get; set; }

        public static ResponseNoContent Success(int statusCode)
        {
            return new ResponseNoContent { IsSuccess = true, StatusCode = statusCode };
        } 
        public static ResponseNoContent Error(List<string> errors, int statusCode)
        {
            return new ResponseNoContent { IsSuccess = false, Errors = errors,StatusCode=statusCode };
        } 
    }
}
