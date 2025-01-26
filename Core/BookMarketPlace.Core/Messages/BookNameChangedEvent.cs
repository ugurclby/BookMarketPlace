using System;
using System.Collections.Generic;
using System.Text;

namespace BookMarketPlace.Core.Messages
{
    public class BookNameChangedEvent
    {
        public string BookId { get; set; }
        public string NewName { get; set; } 
    }
}
