using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web;
using System.Configuration;
using System.Data;

namespace BolumTer.App_Class
{
    public class dbOp
    {
           private string myConn=ConfigurationManager.ConnectionStrings["tConn"].ConnectionString;

           public int runThisSP(string sp)
           {
               int ret = 0;
               using (SqlConnection conn = new SqlConnection(myConn))
               using (SqlCommand cmd = conn.CreateCommand())
               {
                   cmd.CommandText = sp;
                   cmd.CommandType = CommandType.StoredProcedure;
                   var returnParameter = cmd.Parameters.Add("@newId", SqlDbType.Int);
                   returnParameter.Direction = ParameterDirection.ReturnValue;

                   conn.Open();
                   cmd.ExecuteNonQuery();
                   ret = Convert.ToInt32 (returnParameter.Value);
               }
            return ret;
           
           
           }

           public int runThisSPWithPrmtr(int tId, string tList,string sp)
           {
               int ret = 0;
               using (SqlConnection conn = new SqlConnection(myConn))
               using (SqlCommand cmd = conn.CreateCommand())
               {
                   cmd.CommandText = sp;
                   cmd.CommandType = CommandType.StoredProcedure;
                   cmd.Parameters.Add("@tId", SqlDbType.Int).Value = tId;
                   cmd.Parameters.Add("@tList", SqlDbType.VarChar).Value = tList;
                   conn.Open();
                   ret=cmd.ExecuteNonQuery();
               }
               return ret;
           }



           public void errLog(string sayfa, string fonk,string hataKod, string hata) {

               try
               {
                   using (SqlConnection conn = new SqlConnection(myConn))
                   using (SqlCommand cmd = conn.CreateCommand())
                   {
                       cmd.CommandText = "zWriteLog";
                       cmd.CommandType = CommandType.StoredProcedure;
                       cmd.Parameters.Add(sayfa.Substring(1, 10), SqlDbType.VarChar, 10);
                       cmd.Parameters.Add(fonk.Substring(1, 20), SqlDbType.VarChar, 20);
                       cmd.Parameters.Add(hataKod.Substring(1, 20), SqlDbType.VarChar, 20);
                       cmd.Parameters.Add(hata.Substring(1, 400), SqlDbType.VarChar, 400);
                       conn.Open();
                       cmd.ExecuteNonQuery();
                   }
               }
               catch 
               {
                
               }

           }



           public void writeIletisim(int LocalId, string eMail, string comment)
           {

               try
               {
                   using (SqlConnection conn = new SqlConnection(myConn))
                   using (SqlCommand cmd = conn.CreateCommand())
                   {
                       cmd.CommandText = "zWriteComment";
                       cmd.CommandType = CommandType.StoredProcedure;
                       cmd.Parameters.AddWithValue("@LocalId", LocalId);

                       cmd.Parameters.AddWithValue("@eMail", eMail);
                       cmd.Parameters.AddWithValue("@comment", comment);

                       conn.Open();
                       cmd.ExecuteNonQuery();
                   }
               }
               catch
               {

               }

           }
    }
}