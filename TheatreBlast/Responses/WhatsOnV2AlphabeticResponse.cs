using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using TheatreBlast.Converters;

namespace TheatreBlast.Responses
{
    public class WhatsOnV2AlphabeticResponse
    {
        public List<WhatsOnAlphabeticFilmsEntry> WhatsOnAlphabeticFilms { get; set; }
    }

    public class WhatsOnAlphabeticFilmsEntry
    {
        public List<WhatsOnAlphabeticCinemasEntry> WhatsOnAlphabeticCinemas { get; set; }
        public string Title { get; set; }
        public int FilmId { get; set; }
    }

    public class WhatsOnAlphabeticCinemasEntry
    {
        public List<WhatsOnAlphabeticCinemasEntry> WhatsOnAlphabeticCinemas { get; set; }
        public List<WhatsOnAlphabeticShedulesEntry> WhatsOnAlphabeticShedules { get; set; }
    }

    public class WhatsOnAlphabeticShedulesEntry
    {
        public string BookingLink { get; set; }
        public string VersionTitle { get; set; }
        [JsonConverter(typeof(DateFormatConverter), "yyyy-MM-dd HH:mm:ss")]
        public DateTime? Time { get; set; }
    }
}
