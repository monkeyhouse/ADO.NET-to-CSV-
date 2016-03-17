# ADO.NET to CSV

Exceute an ADO.NET stored procedure and return a CSV file.

Inlcudes simple interface for mapping betwen database column names and English column names and formatting. It is a light wrapper ( syntactic sugar ) on top of [CSV Helper](https://github.com/JoshClose/CsvHelper) project. Mainly I wanted an adapter to abstract away repetitive non-business logic code.

Sample use is like
``` c#
var dt = DbMethods.FetchDataTable(
    "procedure_name",
    DbMethods.GetParameter("02/01/16", SqlDbType.DateTime, "@DateFrom", 10),
    DbMethods.GetParameter("03/16/16", SqlDbType.DateTime, "@DateTo", 10)                
);

var renderOptions = new MyAdapterOptions
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

var csvBytes = dt.ExportToCsv(renderOptions);

File.WriteAllBytes("C:\path\to\the.csv", csvBytes);
```

Why upload? Its a nice bit of code I enjoyed writing in a few hours. :)
