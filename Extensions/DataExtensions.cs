using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace HyosungManagement.Extensions
{
    public static class DataExtensions
    {
        public static readonly string UrlEncodedDateFormat = "yyyyMMdd";
        public static readonly string SimpleDateFormat = "yyyy-MM-dd";

        public static DateTime? ParseUrlEncodedDate(this string dateString)
        {
            DateTime parsedDate;

            if (
                DateTime.TryParseExact(
                    dateString,
                    UrlEncodedDateFormat,
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out parsedDate
                )
            )
            {
                return parsedDate.Date;
            }

            return null;
        }

        public static string ToSimpleDateString(this DateTime date)
        {
            return date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        }

        public static TEnum? ParseUrlEncodedEnum<TEnum>(this string enumString)
            where TEnum : struct
        {
            TEnum @enum;

            if (Enum.TryParse(enumString, out @enum))
            {
                return @enum;
            }

            return null;
        }

        public static string GetName<TEnum>(this TEnum @enum)
            where TEnum : struct
        {
            return Enum.GetName(typeof(TEnum), @enum);
        }

        public static bool Between(this DateTime date, DateTime start, DateTime end)
        {
            return date >= start && date < end;
        }
    }
}
