using System.Collections.Generic;
using Newtonsoft.Json;

namespace TheatreBlast.Responses
{
    public class GetSeatsResponse
    {
        public bool IsReservable { get; set; }

        [JsonProperty("room")]
        public List<SeatsRow> Rows { get; set; }
    }

    public class SeatsRow
    {
        [JsonProperty("rS")]
        public List<Seat> Seats { get; set; }
    }

    public class Seat
    {
        public string Id { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public bool Lock { get; set; }
        [JsonProperty("b")]
        public bool Bought { get; set; }
    }
}
