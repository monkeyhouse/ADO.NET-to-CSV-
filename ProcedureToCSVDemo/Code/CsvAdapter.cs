using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.TypeConversion;

namespace ProcedureToCSVDemo.Code
{
    public static class CsvAdapter
    {
        public static byte[] ExportToCsv(this DataTable dt, CsvAdapterOptions opts)
        {

            var nameMapMerged = opts.ConventionNameMap.ToDictionary(entry => entry.Key, entry => entry.Value); 
          
            foreach (var nm in opts.NameMap)
                nameMapMerged[nm.Key] = nm.Value;


            var formatMapMerged = opts.ConventionFormatMap.ToDictionary(entry => entry.Key, entry => entry.Value);

            foreach (var fmt in opts.FormatMap)
                formatMapMerged[fmt.Key] = fmt.Value;

            return dt.ExportToCsv(nameMapMerged, formatMapMerged);
        }

        public static byte[] ExportToCsv(this DataTable dt, Dictionary<string, string> nameMap, Dictionary<string, ITypeConverter> formatMap)
        {
            byte[] res;
            using (var ms = new MemoryStream())
            {
                using (var sw = new StreamWriter(ms))
                {

                    var csv = new CsvWriter(sw);

                    var columnFormats = new ITypeConverter[dt.Columns.Count];

                    for (var i = 0; i < dt.Columns.Count; i++)
                    {
                        var columnName = dt.Columns[i].ColumnName;

                        if ( nameMap.ContainsKey(columnName))
                            csv.WriteField(nameMap[columnName]);
                        else
                            csv.WriteField(columnName);

                        //build format map
                        if (formatMap.ContainsKey(columnName))
                            columnFormats[i] = formatMap[columnName];
                    }

                    csv.NextRecord();

                    foreach (DataRow row in dt.Rows)
                    {
                        for (var i = 0; i < dt.Columns.Count; i++)
                        {
                          
                            if ( columnFormats[i] != null)
                                csv.WriteField(row[i], columnFormats[i]);
                            else
                                csv.WriteField(row[i]);                                
                        }
                        csv.NextRecord();
                    }
                }

                res = ms.ToArray();
            }

            return res;
        }


        public static byte[] ExportToCsv(this DataTable dt )
        {
            byte[] res;
            using (var ms = new MemoryStream())
            {
                using (var sw = new StreamWriter(ms))
                {

                    var csv = new CsvWriter(sw);

                    foreach (DataColumn column in dt.Columns)
                    {
                        csv.WriteField(column.ColumnName);
                    }
                    csv.NextRecord();

                    foreach (DataRow row in dt.Rows)
                    {
                        for (var i = 0; i < dt.Columns.Count; i++)
                        {
                            csv.WriteField(row[i]);
                        }
                        csv.NextRecord();
                    }
                }

                res = ms.ToArray();
            }

            return res;
        }

    }
}