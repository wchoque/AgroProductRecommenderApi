using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppCentroIdiomas
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
