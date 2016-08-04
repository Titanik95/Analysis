using Analysis.Models;
using Analysis.Others;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Analysis.Controllers
{
    class DataManager
    {
        string connectionString;
        DateTime dayBeginning;
        string datePostfix;

        public DataManager()
        {
            connectionString = Others.Common.GetConnectionString();
            dayBeginning = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 09, 59, 00);
            datePostfix = DateTime.Now.ToString("_yyMMdd");
        }

        public async void UpdateSecurities(Dictionary<string, Security> secs)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    con.Open();
                }
                catch
                {
                    MessageBox.Show("Unable to connect to database");
                }

                foreach (var sec in secs)
                {
                    if (sec.Value.DateFrom >= dayBeginning)
                        GetDayVolumes(sec.Value, con, false);
                    else if (sec.Value.DateTo < dayBeginning)
                        GetGeneralVolumes(sec.Value, con, false);
                    else
                    {
                        GetDayVolumes(sec.Value, con, true);
                        GetGeneralVolumes(sec.Value, con, true);
                    }
                }
            }
        }

        async void GetDayVolumes(Security sec, SqlConnection con, bool fromDayBeggining)
        {
            try
            {
                using (var command = con.CreateCommand())
                {
                    command.CommandText = Properties.Resources.ProcName_DayVolumeSum;
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Table", sec.SecurityName + datePostfix);
                    if (fromDayBeggining)
                        command.Parameters.AddWithValue("@DateFrom", dayBeginning.ToString("HHmm"));
                    else
                        command.Parameters.AddWithValue("@DateFrom", sec.TimeFrom.AddMinutes(-1).ToString("HHmm"));
                    command.Parameters.AddWithValue("@DateTo", sec.TimeTo.AddMinutes(-1).ToString("HHmm"));
                    using (var res = command.ExecuteReader())
                    {
                        try
                        {
                            res.Read();
                            sec.VolumeBuy = (int)res[0];
                            sec.VolumeSell = (int)res[1];
                        }
                        catch
                        {
                            sec.VolumeBuy = 0;
                            sec.VolumeSell = 0;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        async void GetGeneralVolumes(Security sec, SqlConnection con, bool addVolume)
        {
            using (var command = con.CreateCommand())
            {
                command.CommandText = Properties.Resources.ProcName_GeneralVolumeSum;
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Table", sec.SecurityName);
                command.Parameters.AddWithValue("@DateFrom", sec.DateFrom);
                command.Parameters.AddWithValue("@DateTo", sec.DateTo);

                using (var res = command.ExecuteReader())
                {
                    try
                    {
                        res.Read();
                        if (!addVolume)
                        {
                            sec.VolumeBuy = 0;
                            sec.VolumeSell = 0;
                        }
                        sec.VolumeBuy = (int)res[0];
                        sec.VolumeSell = (int)res[1];
                    }
                    catch
                    {
                        if (!addVolume)
                        {
                            sec.VolumeBuy = 0;
                            sec.VolumeSell = 0;
                        }
                    }
                }
            }
        }

    }
}
