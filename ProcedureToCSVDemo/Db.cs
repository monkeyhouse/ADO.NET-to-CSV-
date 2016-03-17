using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcedureToCSVDemo
{
    public class DbBase
    {
        protected string _connectionString { get; set; }
        protected int _timeout { get; set; }

        public DbBase()
        {
            _timeout = int.Parse(ConfigurationSettings.AppSettings["sqlCommandTimeout"]);
            _connectionString = ConfigurationSettings.AppSettings["sqlConn.ConnectionString"];
        }

        protected SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }

    public class DbMethods : DbBase
    {
        public static DataTable FetchDataTable(string procedure, params SqlParameter[] p)
        {
            var db = new DbMethods();
            return db.GetTable(procedure, p);
        }


        public DataTable GetTable(string storedProc, SqlParameter[] parameters = null)
        {
            using (var conn = GetConnection())
            {
                return GetTable(conn, storedProc, parameters);
            }
        }

        public DataTable GetTable(SqlConnection conn, string storedProc, SqlParameter[] parameters = null)
        {
            using (var cmd = new SqlCommand())
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = storedProc;
                cmd.Connection = conn;
                cmd.CommandTimeout = _timeout;


                if (parameters != null && parameters.Length > 0)
                    cmd.Parameters.AddRange(parameters);

                SqlDataAdapter CheckAdapter = new SqlDataAdapter(cmd);
                DataSet data = new DataSet();
                CheckAdapter.Fill(data);

                return data.Tables[0];
            }
        }


        public static SqlParameter GetParameter<T>(T value, SqlDbType paramType, string name, int length)
        {
            if ( !value.Equals( null ))
            {
                SqlParameter retVal = new SqlParameter(name, paramType, length);
                retVal.Direction = ParameterDirection.Input;
                if (paramType.Equals(SqlDbType.Int))
                {
                    retVal.Value = Convert.ToInt64(value);
                }
                else if (paramType.Equals(SqlDbType.Float))
                {
                    retVal.Value = Convert.ToSingle(value);
                }
                else
                {
                    retVal.Value = value.ToString();
                }
                return retVal;
            }
            return new SqlParameter(name, DBNull.Value);
        }

    }
}
