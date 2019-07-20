using System.Collections.Generic;

namespace TheatreBlast.Responses
{
    public class WhatsOnV2GetCinemasResponse
    {
        public List<WhatsOnV2GetCinemasResponseEntry> WhatsOnCinemas { get; set; }
    }

    public class WhatsOnV2GetCinemasResponseEntry
    {
        public int CinemaId { get; set; }
        public string CinemaName { get; set; }
    }
}
