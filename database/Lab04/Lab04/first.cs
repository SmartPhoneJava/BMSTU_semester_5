using Microsoft.SqlServer.Server;
using System.Data.SqlClient;

public class first
{
    [SqlFunction(DataAccess = DataAccessKind.Read)]
    public static double max_rating_OVER_ALL_TIMES()
    {
        using (SqlConnection conn
            = new SqlConnection("context connection=true"))
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand(
                "SELECT MAX(user_rating)" +
                "FROM airtime " +
                "WHERE begin_date BETWEEN '1995-01-01' AND '2000-01-31'  AND finish_date BETWEEN '2000-01-01' AND '2018-01-31'", conn);
            return (double)cmd.ExecuteScalar();
        }
    }
}
