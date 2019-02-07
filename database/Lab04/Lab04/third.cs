using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;

using Microsoft.SqlServer.Server;

public partial class third
{
    private class TitleInfo
    {
        public SqlString name_local;
        public SqlString description_title;
        public SqlInt32 time;

        public TitleInfo(SqlString name,
            SqlString desc, SqlInt32 tim)
        {
            name_local = name;
            description_title = desc;
            time = tim;
        }
    }

    [SqlFunction(
        DataAccess = DataAccessKind.Read,
        FillRowMethodName = "fill",
        TableDefinition = "name_local nvarchar(1000), description_title nvarchar(1300), time int")]

    public static IEnumerable getInfo(SqlInt32 type)
    {
        ArrayList arraylist = new ArrayList();

        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            using (SqlCommand sqlTitle = new SqlCommand(
                "SELECT " +
                "name_local, description_title, episodes, duration " +
                "FROM title " +
                "WHERE id_type = @type",
                connection))
            {
                SqlParameter modifiedSinceParam = sqlTitle.Parameters.Add(
                    "@type",
                    SqlDbType.Int);
                modifiedSinceParam.Value = type;

                using (SqlDataReader Reader = sqlTitle.ExecuteReader())
                {
                    while (Reader.Read())
                    {
                        arraylist.Add(new TitleInfo(Reader.GetSqlString(0),
                            Reader.GetSqlString(1), Reader.GetSqlInt32(2) *
                            Reader.GetSqlInt32(3)));
                    }
                }
            }
        }

        return arraylist;
    }

    public static void fill(
    object obj,
    out SqlString name,
    out SqlString desc,
    out SqlInt32 time)
    {
        TitleInfo tinfo = (TitleInfo)obj;

        name = tinfo.name_local;
        desc = tinfo.description_title;
        time = tinfo.time;
    }

};
