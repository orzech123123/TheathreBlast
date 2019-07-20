using Newtonsoft.Json.Converters;

namespace TheatreBlast.Converters
{
    public class DateFormatConverter : IsoDateTimeConverter
    {
        public DateFormatConverter(string format)
        {
            DateTimeFormat = format;
        }
    }
}