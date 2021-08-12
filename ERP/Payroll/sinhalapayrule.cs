using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP.ERPService;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace ERP.Payroll
{
    class sinhalapayrule
    {
        SqlConnection Connection;
        public sinhalapayrule()
        {
            Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ERPConnectionString"].ConnectionString.ToString());
        }
        public DataTable GetRuleName()
        {
            Connection.Open();
            SqlCommand Command = Connection.CreateCommand();
            Command.CommandType = CommandType.Text;
            Command.CommandText = "select * from mas_CompanyRule where isActive = 'True' and isdelete = 'False' ";
            SqlDataAdapter Adapter = new SqlDataAdapter(Command);
            DataTable Dt1 = new DataTable();
            Adapter.Fill(Dt1);
            Connection.Close();
            return Dt1;
        }//get rule name for combobox

        public int SaveSinalaName(string rule_id, string sinhala_name)
        {
            int result;
            Connection.Open();
            SqlCommand Command = Connection.CreateCommand();
            Command.CommandType = CommandType.StoredProcedure;
            Command.CommandText = "SP_SaveRule";
            Command.Parameters.AddWithValue("@rule_id", new Guid(rule_id));
            Command.Parameters.AddWithValue("@sinhala_name", sinhala_name);
            result = Command.ExecuteNonQuery();
            Connection.Close();
            return result;
        }//save rule name and id
    }

}
