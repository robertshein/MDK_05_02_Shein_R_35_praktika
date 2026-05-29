using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace PermDynamics_Shein.Classes
{
    public static class ChartRepository
    {
        public static void SaveCharts(List<PointInfo> points1, List<PointInfo> points2)
        {
            MySqlConnection connection = DBconnection.OpenConnection();
            try
            {
                MySqlCommand sessionCmd = new MySqlCommand(
                    "INSERT INTO sessions (created_at) VALUES (NOW())", connection);
                sessionCmd.ExecuteNonQuery();

                MySqlCommand idCmd = new MySqlCommand("SELECT LAST_INSERT_ID()", connection);
                long sessionId = Convert.ToInt64(idCmd.ExecuteScalar());

                MySqlCommand insertCmd = new MySqlCommand(
                    "INSERT INTO chart_values (session_id, chart_number, position, value) " +
                    "VALUES (@sessionId, @chartNumber, @position, @value)", connection);

                insertCmd.Parameters.Add("@sessionId", MySqlDbType.Int64);
                insertCmd.Parameters.Add("@chartNumber", MySqlDbType.Byte);
                insertCmd.Parameters.Add("@position", MySqlDbType.Int32);
                insertCmd.Parameters.Add("@value", MySqlDbType.Double);

                insertCmd.Parameters["@sessionId"].Value = sessionId;

                SavePoints(insertCmd, points1, chartNumber: 1);
                SavePoints(insertCmd, points2, chartNumber: 2);
            }
            finally
            {
                DBconnection.CloseConnection(connection);
            }
        }

        private static void SavePoints(MySqlCommand cmd, List<PointInfo> points, byte chartNumber)
        {
            cmd.Parameters["@chartNumber"].Value = chartNumber;
            for (int i = 0; i < points.Count; i++)
            {
                cmd.Parameters["@position"].Value = i;
                cmd.Parameters["@value"].Value = points[i].value;
                cmd.ExecuteNonQuery();
            }
        }
    }
}
