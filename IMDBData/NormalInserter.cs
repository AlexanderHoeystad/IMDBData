using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMDBData
{
    public class NormalInserter
    {
        public NormalInserter()
        {
            // Empty constructor
        }

        public void InsertData(List<Title> titles, SqlConnection sqlConnect, SqlTransaction transaction)
        {
            foreach (Title title in titles) 
            {
                string SQL = "INSERT INTO  [dbo].[Titles]" +
                    "([Tconst]," +
                    "[TitleTypes]," +
                    "[PrimaryTitle]," +
                    "[OriginalTitle]," +
                    "[IsAdult]," +
                    "[StartYear]," +
                    "[EndYear]," +
                    "[RuntimeMinutes])"
                    + "VALUES ('" + title.Tconst + "', '" + title.TitleType + "', '" + title.PrimaryTitle.Replace("'","''") + "', '" + title.OriginalTitle.Replace("'", "''") + "', '"+ title.IsAdult +"' , "+ CheckIntForNull(title.StartYear) +" , "+ CheckIntForNull(title.EndYear) +" , "+ CheckIntForNull(title.RuntimeMinutes) +")";

                //throw new Exception(SQL);

                SqlCommand command = new SqlCommand(SQL, sqlConnect, transaction);
                command.ExecuteNonQuery();
            }
        }
        public string CheckIntForNull(int? value)
        {
            if (value == null)
            {
                return "NULL";
            }
            else
            {
                return "" + value;
            }
        }

    }
}
