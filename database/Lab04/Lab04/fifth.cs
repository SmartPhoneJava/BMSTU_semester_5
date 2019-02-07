using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.SqlServer.Server;

public partial class Triggers
{
    // Enter existing table or view for the target and uncomment the attribute line
    public static void fifth()
    {
        // Replace with your own code
        SqlCommand command;
        SqlTriggerContext triggContext = SqlContext.TriggerContext;
        SqlPipe pipe = SqlContext.Pipe;
        SqlDataReader reader;

        using (SqlConnection connection
            = new SqlConnection("context connection=true"))
        {
            connection.Open();
            command = new SqlCommand("SELECT * FROM INSERTED;",
                connection);
            reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                pipe.Send("You inserted the following rows:");
                while (reader.Read())
                {
                    pipe.Send("'" + reader.GetString(2)
                    + "', '" + reader.GetInt32(3) + "', '" + reader.GetString(4) + "'");
                }

                reader.Close();

                //alternately, to just send a tabular resultset back:  
                //pipe.ExecuteAndSend(command);  
            }
            else
            {
                pipe.Send("No rows affected.");
            }
        }

    }

}
