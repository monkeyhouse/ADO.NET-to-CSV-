using System;
using CsvHelper.TypeConversion;

namespace ProcedureToCSVDemo
{
    public class MyDateFormatter : DefaultTypeConverter
    {
        public override  bool CanConvertFrom(Type type)
        {
            return type == typeof(DateTime);
        }

        public override String ConvertToString(TypeConverterOptions options, Object value)
        {
            if (value is DBNull)
                return String.Empty;

            return ((DateTime)value).ToString("d");
        }
       
    }

    public class MyCurrencyFormatter : DefaultTypeConverter
    {
        public override bool CanConvertFrom(Type type)
        {
            return type == typeof(Decimal);
        }

        public override String ConvertToString(TypeConverterOptions options, Object value)
        {
            if (value is DBNull)
                return String.Empty;

            return ((Decimal) value).ToString("F2");
        }

    }
}