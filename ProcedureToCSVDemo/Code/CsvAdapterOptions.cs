using System;
using System.Collections.Generic;
using CsvHelper.TypeConversion;

namespace ProcedureToCSVDemo.Code
{
    public class MyAdapterOptions : CsvAdapterOptions
    {
        public MyAdapterOptions() 
        {
           
            ConventionNameMap = new Dictionary<String, String>()
            {
                {"claim_nbr", "Claim"},
                {"insured_name", "Member"},
                {"loss_d", "Loss Date"},
                {"close_d", "Close Date"},
                {"last_paid_d", "Last Paid Date"},
                {"thrp_ser", "Insured Party"},
                {"thrp_name", "Insured Party Name"},
                {"cov_code", "Coverage Code"},
                {"cov_desc", "Coverage Description"}
            };
            
            ConventionFormatMap = new Dictionary<String, ITypeConverter>
            {
                {"loss_d", DateConverter},
                {"close_d", DateConverter},
                {"last_paid_d", DateConverter},
            };
        }
    }

    /// <summary>
    ///  CSV Adapter Options Base class
    /// </summary>
    public abstract class CsvAdapterOptions
    {
        protected DefaultTypeConverter DateConverter = new MyDateFormatter();
        protected DefaultTypeConverter CurrencyFormatter = new MyCurrencyFormatter();

        public Dictionary<string, string> NameMap = new Dictionary<String, String>();
        public Dictionary<string, ITypeConverter> FormatMap = new Dictionary<String, ITypeConverter>();

        public Dictionary<string, string> ConventionNameMap = new Dictionary<String, String>();
        public Dictionary<string, ITypeConverter> ConventionFormatMap = new Dictionary<String, ITypeConverter>();
      
    }


}