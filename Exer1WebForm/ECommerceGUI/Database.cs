using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

public class Database
{
    string strConn = ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString;

    public DataTable GetDataTable(string sql)
    {
        using (SqlConnection conn = new SqlConnection(strConn))
        {
            SqlDataAdapter da = new SqlDataAdapter(sql, conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }
    }
    public void ExecuteNonQuery(string sql)
    {
        using (SqlConnection conn = new SqlConnection(strConn))
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.ExecuteNonQuery();
        }
    }
}