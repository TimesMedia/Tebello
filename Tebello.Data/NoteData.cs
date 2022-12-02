using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Subs.Data
{
    public class NoteData
    {
        public static string GetBulletin()
        {
            SqlConnection lConnection = new SqlConnection();
            try
            {
                SqlCommand Command = new SqlCommand();
                SqlDataAdapter Adaptor = new SqlDataAdapter();
                lConnection.ConnectionString = Settings.ConnectionString;
                lConnection.Open();
                Command.Connection = lConnection;
                Command.CommandType = CommandType.Text;
                Command.CommandText = "select isnull(item, '') from Note where Id = 1";
                return (string)Command.ExecuteScalar();
            }
            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, "NoteData", "GetBulletin", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                throw ex;
            }
            finally
            {
                lConnection.Close();
            }
        }


        public static bool SetBulletin(string pItem)
        {
            SqlConnection lConnection = new SqlConnection();

            try
            {
                SqlCommand Command = new SqlCommand();
                SqlDataAdapter Adaptor = new SqlDataAdapter();
                lConnection.ConnectionString = Settings.ConnectionString;
                lConnection.Open();
                Command.Connection = lConnection;
                Command.CommandType = CommandType.Text;
                Command.CommandText = "Update Note set item= '" + pItem + 
                                                       "' ,ModifiedBy = SYSTEM_USER" + 
                                                       " ,ModifiedOn = GetDate()" + 
                                                  " where Id = 1";
                Command.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, "NoteData", "SetBulletin", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                throw ex;
            }
            finally
            {
                lConnection.Close();
            }
        }




    }
}
