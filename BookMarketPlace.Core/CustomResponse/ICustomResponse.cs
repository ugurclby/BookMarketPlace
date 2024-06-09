using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BookMarketPlace.Core.CustomResponse
{
    public  interface ICustomResponse<T>
    {
       
        [JsonIgnore]
        public int StatusCode { get; set; }
        [JsonIgnore]
        public bool IsSuccess { get; set; }

        public List<string> Errors { get; set; }
    }
}
