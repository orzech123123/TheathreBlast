using System;
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
        public int B { get; set; }
        [JsonProperty("t")]
        public dynamic T { get; set; }

        [JsonIgnore]
        public bool IsBlocked => Convert.ToInt16(B) == 1;
        [JsonIgnore]
        public bool IsIdGreatherThanMinusOne => Convert.ToInt64(Id) > -1;
        [JsonIgnore]
        public bool IsReserved => T != null && ((string)T.ToString()).Trim().StartsWith("[");
    }
}
