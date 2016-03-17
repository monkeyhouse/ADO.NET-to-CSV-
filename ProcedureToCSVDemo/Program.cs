using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using CsvHelper.TypeConversion;
using ProcedureToCSVDemo.Code;

namespace ProcedureToCSVDemo
{
    public class Program
    {
        static void Main(string[] args)
        {

            var dt = DbMethods.FetchResultSet(
                "rpt_SSCS_Res_paid_variance",
                DbMethods.GetParameter("02/01/16", SqlDbType.DateTime, "@DateFrom", 10),
                DbMethods.GetParameter("03/16/16", SqlDbType.DateTime, "@DateTo", 10)                
            );

            var opts = new MyAdapterOptions
            {
                NameMap = new Dictionary<string, string>()
                {
                    {"cov_adj", "Adjuster?"},
                    {"total_res", "Total ? "},
                    {"total_paid", "Total Paid "},
                    {"total_var", "Total Variance "}
                },
                FormatMap = new Dictionary<string, ITypeConverter>()
                {
                    {"total_res", new MyCurrencyFormatter()},
                    {"total_paid", new MyCurrencyFormatter()},
                    {"total_var", new MyCurrencyFormatter()},
                }

            };

            var csvBytes = dt.ExportToCsv(opts);

            var dir = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var path = Path.Combine(dir, "junk\\temp.csv");
            File.WriteAllBytes(path, csvBytes);
        }
    }
}
