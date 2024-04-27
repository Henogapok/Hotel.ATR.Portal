using System;
using System.Collections.Generic;

namespace Hotel.ATR.Api.Models
{
    public partial class Client
    {
        public int Id { get; set; }
        public string PathToImage { get; set; } = null!;
        public string PathToHoveredImage { get; set; } = null!;
    }
}
