using System;
using Microsoft.Data.SqlClient;

namespace AgroProductRecommenderApi
{
    public static class ReaderExtensions
    {
        public static DateTimeOffset? GetNullableDateTimeOffset(this SqlDataReader reader, string name)
        {
            var col = reader.GetOrdinal(name);
            return reader.IsDBNull(col) ?
                        (DateTimeOffset?)null :
                        (DateTimeOffset?)reader.GetDateTimeOffset(col);
        }
    }
}
