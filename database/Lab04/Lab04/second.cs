using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;

[Serializable]
[Microsoft.SqlServer.Server.SqlUserDefinedAggregate(Format.Native)]
public struct getPopularStatus
{
    public void Init()
    {
        anounced = 0;
        ongoing = 0;
        shown = 0;
    }

    public void Accumulate(double user_rating, SqlString status)
    {
        if (user_rating > 4)
        {
            if (status == "anounce")
            {
                anounced++;
            }
            if (status == "ongoing")
            {
                ongoing++;
            }
            if (status == "shown")
            {
                shown++;
            }
        }
    }

    public void Merge (getPopularStatus other)
    {
        ongoing += other.ongoing;
        shown += other.shown;
        anounced += other.anounced;
    }

    public SqlString Terminate ()
    {
        if (ongoing > shown)
        {
            if (ongoing > anounced)
                return new SqlString("Онгоинг");
        }
        else
        {
            if (shown > anounced)
                return new SqlString("Выпущенные");
        }
        return new SqlString("Анонсирован");
    }

    // This is a place-holder member field
    private int anounced;
    private int shown;
    private int ongoing;
}
