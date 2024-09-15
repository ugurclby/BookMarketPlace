using System;
using System.Collections.Generic;
using System.Text;

namespace BookMarketPlace.Core.Services
{
    public interface ISharedIdentityService
    {
        public string getUserId { get; }
    }
}
