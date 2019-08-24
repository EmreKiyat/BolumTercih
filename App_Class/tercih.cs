using System;
using System.Collections.Generic;
using System.Web;
using BolumTer.Models;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Linq;
using System.Web.Script.Serialization;

namespace BolumTer.App_Class
{
    public class tercih
    {
        private string myConn = ConfigurationManager.ConnectionStrings["tConn"].ConnectionString;


        public Guid GetNewId(string myListJSON)
        {
            //dbOp dbOp = new dbOp();
            //return dbOp.runThisSP("getNewId");
            Guid ret;
            using (SqlConnection conn = new SqlConnection(myConn))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = "GetNewId";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@TercihJSON", SqlDbType.VarChar);
                cmd.Parameters["@TercihJSON"].Value = myListJSON;

                SqlParameter returnParameter = new SqlParameter("@NewId", SqlDbType.UniqueIdentifier);
                returnParameter.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(returnParameter);

                conn.Open();
                cmd.ExecuteNonQuery();
                ret = (Guid)returnParameter.Value;
            }
            return ret;
        }

        public int UpdateTercihList(Guid tId, string tList) {

            int ret = 0;
            using (SqlConnection conn = new SqlConnection(myConn))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = "UpdateTercihList";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@tId", SqlDbType.UniqueIdentifier).Value = tId;
                cmd.Parameters.Add("@tList", SqlDbType.VarChar).Value = tList;
                conn.Open();
                ret = cmd.ExecuteNonQuery();
            }
            return ret;
        
        }

        public List<TercihListModel> GetBolumList(FilterObjModel FilterObj) {   //Tercih listesiyle aynı data model kullanıldı!!!
                DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(myConn))
            {

                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "dbo.GetBolumListByFilter";
                    cmd.CommandType = CommandType.StoredProcedure;
                    var tmpTX="";
                    if (string.IsNullOrEmpty(FilterObj.TX) == true)  tmpTX=""; else tmpTX=FilterObj.TX;



                    SqlParameter parameter1 = cmd.Parameters.AddWithValue("@DVFilter", CreateDataTable(FilterObj, "DV"));
                    SqlParameter parameter2 = cmd.Parameters.AddWithValue("@PTFilter", CreateDataTable(FilterObj, "PT"));
                    SqlParameter parameter3 = cmd.Parameters.AddWithValue("@SHFilter", CreateDataTable(FilterObj, "SH"));
                    SqlParameter parameter4 = cmd.Parameters.AddWithValue("@TxtFilter", tmpTX);
                    SqlParameter parameter5 = cmd.Parameters.AddWithValue("@PageNumber", FilterObj.PG-1);

                    parameter1.SqlDbType = SqlDbType.Structured;
                    parameter1.TypeName = "dbo.IDType";
                    parameter2.SqlDbType = SqlDbType.Structured;
                    parameter2.TypeName = "dbo.IDType";
                    parameter3.SqlDbType = SqlDbType.Structured;
                    parameter3.TypeName = "dbo.IDType";
                    parameter4.SqlDbType = SqlDbType.VarChar;
                    parameter5.SqlDbType = SqlDbType.Int;

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                   
                }
            }
            return ConvertToTercihListType(dt);

        }

        private DataTable CreateDataTable(FilterObjModel FilterObj, string Tip) {

                          DataTable dt=new DataTable();
                          dt.Columns.Add("Id", typeof(System.Int32));
                          switch (Tip)
                            {
                                case "DV":

                                  foreach (var obj in FilterObj.DV)
                                    {
                                        DataRow dr = dt.NewRow();
                                        dr["Id"] = obj.Id;
                                        dt.Rows.Add(dr);
                                    }
                                  break;
                                case "PT":

                                  foreach (var obj in FilterObj.PT)
                                  {
                                      DataRow dr = dt.NewRow();
                                      dr["Id"] = obj.Id;
                                      dt.Rows.Add(dr);
                                  }
                                  break;
                                case "SH":

                                  foreach (var obj in FilterObj.SH)
                                  {
                                      DataRow dr = dt.NewRow();
                                      dr["Id"] = obj.Id;
                                      dt.Rows.Add(dr);
                                  }
                                  break;

                          }
            return dt;
            }

        internal List<TercihListModel> ConvertToTercihListType(DataTable dt)
        {
            var convertedList = (from rw in dt.AsEnumerable()
                                 select new TercihListModel()
                                 {
                                     tId = Convert.ToInt32(rw["BolumID"]),
                                     Ad = Convert.ToString(rw["Bolum"]),
                                     Uni = Convert.ToString(rw["Universite"]),
                                     PT = Convert.ToString(rw["PuanTuru"]),
                                     SR = rw["BasariSirasi"] == DBNull.Value ? 0 : Convert.ToInt32(rw["BasariSirasi"]),
                                     EKP = rw["EnKck"] == DBNull.Value ? 0 : Convert.ToDecimal(rw["EnKck"]),
                                     DV = Convert.ToString(rw["Aciklama"]),
                                     Sira = Convert.ToInt32(rw["Sira"]),
                                     Fakulte = Convert.ToString(rw["Fakulte"]),
                                     BolumKodu = Convert.ToString(rw["BolumKodu"]),
                                     Kosullar= Convert.ToString(rw["Kosullar"]),
                                     Sure = Convert.ToInt32(rw["Sure"]),
                                     Knt = Convert.ToInt32(rw["Knt"]),

                                 }).ToList <TercihListModel>();

            return convertedList;
        }

        private string GetTercihList(Guid? tId)
        {
            string ret;
            using (SqlConnection conn = new SqlConnection(myConn))
            {
                
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "dbo.GetTercihList";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@tId", SqlDbType.UniqueIdentifier).Value = tId;
                    ret=(string)cmd.ExecuteScalar();



                }
            }
            return ret;

        }

        public List<TercihListModel> GetTercihListByIdList(ListIdModel ListId)
        {
            //tercih list id'lerini al ve table value parametreye çevir
            DataTable dt = new DataTable();
            string st = GetTercihList(ListId.Id);
            st = st == null ? "" : st;
            
            JavaScriptSerializer js = new JavaScriptSerializer();
            List<ListIdModelInt> tl = js.ConvertToType <List<ListIdModelInt>>(js.DeserializeObject(st));
            if (tl != null)
            {
                using (SqlConnection conn = new SqlConnection(myConn))
                {

                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "dbo.GetTercihListByIdList";
                        cmd.CommandType = CommandType.StoredProcedure;


                        SqlParameter parameter1 = cmd.Parameters.AddWithValue("@IdList", CreateDataTableForId(tl));

                        parameter1.SqlDbType = SqlDbType.Structured;
                        parameter1.TypeName = "dbo.IDType3";

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(dt);
                        }

                    }
                }
                return ConvertToTercihListType(dt);
            }
            else return ConvertToTercihListType(dt);

        }

        private DataTable CreateDataTableForId(List<ListIdModelInt> tl)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Id", typeof(System.Int32));
            dt.Columns.Add("Sira", typeof(System.Int16));
            int k = 1;
            foreach (var obj in tl)
             {
                 DataRow dr = dt.NewRow();
                 dr["Id"] = obj.Id;
                 dr["Sira"] = k;
                 dt.Rows.Add(dr);
                 k++;
             }
            return dt;
        }

        public List<BolumListModel> GetBolumListBySearchText(SearchTextModel SearchObj)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(myConn))
            {

                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "dbo.GetBolumListBySearchText";
                    cmd.CommandType = CommandType.StoredProcedure;
                    var tmpTX = "";
                    if (string.IsNullOrEmpty(SearchObj.TX) == true) tmpTX = ""; else tmpTX = SearchObj.TX;

                    SqlParameter parameter1 = cmd.Parameters.AddWithValue("@TxtFilter", tmpTX);
                    SqlParameter parameter2 = cmd.Parameters.AddWithValue("@PageNumber", SearchObj.PG-1);

                    parameter1.SqlDbType = SqlDbType.VarChar;
                    parameter2.SqlDbType = SqlDbType.Int;

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }
            return ConvertToBolumListType(dt);
        }

        internal List<BolumListModel> ConvertToBolumListType(DataTable dt)
        {
            var convertedList = (from rw in dt.AsEnumerable()
                                 select new BolumListModel()
                                 {
                                     tId = Convert.ToInt32(rw["BolumID"]),
                                     Ad = Convert.ToString(rw["Bolum"]),
                                     Uni = Convert.ToString(rw["Universite"]),
                                     PT = Convert.ToString(rw["PuanTuru"]),
                                     SR = rw["BasariSirasi"] == DBNull.Value ? 0 : Convert.ToInt32(rw["BasariSirasi"]),
                                     EKP = rw["EnKck"] == DBNull.Value ? 0 : Convert.ToDecimal(rw["EnKck"]),
                                     DV = Convert.ToString(rw["Aciklama"]),
                                     Sira = Convert.ToInt32(rw["Sira"]),
                                     Kontenjan = Convert.ToInt32(rw["Kontenjan"]),
                                     Kosullar = Convert.ToString(rw["Kosullar"]),
                                     SU = Convert.ToInt32(rw["SU"]),
                                     BK = Convert.ToString(rw["BK"]),
                                 }).ToList<BolumListModel>();
            return convertedList;
        }

        public List<KosulModel> GetBolumKosul(int? BolumId) {

            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(myConn))
            {

                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "dbo.GetBolumKosul";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@BolumId", SqlDbType.Int).Value = BolumId;

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }

                }
            }
            return ConvertToKosulListType(dt);

        }

        internal List<KosulModel> ConvertToKosulListType(DataTable dt)
        {
            var convertedList = (from rw in dt.AsEnumerable()
                                 select new KosulModel()
                                 {
                                     Kosul = Convert.ToString(rw["Kosul"]),
                                     Detay = Convert.ToString(rw["Detay"])
                                 }).ToList<KosulModel>();
            return convertedList;
        }

        public UniModel GetUniDetay(int? uId)
        {
            DataTable dt = new DataTable();
            DataTable dtB = new DataTable();

            using (SqlConnection conn = new SqlConnection(myConn))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "dbo.GetUniDetay";
                    cmd.CommandType = CommandType.StoredProcedure;


                    cmd.Parameters.AddWithValue("@UniversiteId", uId);

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }

                }
//Uni Bölümleri almak için
                using (SqlCommand cmd2 = conn.CreateCommand())
                {
                    cmd2.CommandText = "dbo.GetUniDetayBolumList";
                    cmd2.CommandType = CommandType.StoredProcedure;

                    cmd2.Parameters.AddWithValue("@UniversiteId", uId);

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd2))
                    {
                        da.Fill(dtB);
                    }
                }
//***************
            }

            return ConvertToUniModel(dt,dtB);

        }

        internal UniModel ConvertToUniModel(DataTable dt, DataTable dtB) {
            UniModel CUni = new UniModel();
            DataRow dr = dt.Rows[0];
            CUni.UniId = Convert.ToInt32(dr["UniId"]);
            CUni.Adres = Convert.ToString(dr["adres"]);
            CUni.AnabilimDali = Convert.ToInt32(dr["AnabilimDali"]);
            CUni.ArsGorevlisi = Convert.ToInt32(dr["ArsGorevlisi"]);
            CUni.ArsUygMer = Convert.ToInt32(dr["ArsUygMer"]);
            CUni.aWeb = Convert.ToString(dr["aWeb"]);
            CUni.BirOgr = Convert.ToInt32(dr["BirOgr"]);
            CUni.Bolge = Convert.ToString(dr["Bolge"]);
            CUni.Bolum = Convert.ToInt32(dr["Bolum"]);
            CUni.Docent = Convert.ToInt32(dr["Docent"]);
            CUni.Doktora = Convert.ToInt32(dr["Doktora"]);
            CUni.Enstitu = Convert.ToInt32(dr["Enstitu"]);
            CUni.EPosta = Convert.ToString(dr["EPosta"]);
            CUni.Fakulte = Convert.ToInt32(dr["Fakulte"]);
            CUni.Fax = Convert.ToString(dr["Fax"]);
            CUni.IkiOgr = Convert.ToInt32(dr["IkiOgr"]);
            CUni.isDevlet = Convert.ToInt32(dr["isDevlet"]);
            CUni.Kurulus = Convert.ToString(dr["Kurulus"]);
            CUni.MYO = Convert.ToInt32(dr["MYO"]);
            CUni.OgrGorevlisi = Convert.ToInt32(dr["OgrGorevlisi"]);
            CUni.Okutman = Convert.ToInt32(dr["Okutman"]);
            CUni.Prof = Convert.ToInt32(dr["Prof"]);
            CUni.Program = Convert.ToInt32(dr["Program"]);
            CUni.Sehir = Convert.ToString(dr["Sehir"]);
            CUni.Tel = Convert.ToString(dr["Tel"]);
            CUni.Uni = Convert.ToString(dr["Uni"]);
            CUni.Uzman = Convert.ToInt32(dr["Uzman"]);
            CUni.web = Convert.ToString(dr["web"]);
            CUni.Ydocent = Convert.ToInt32(dr["Ydocent"]);
            CUni.YuksekLisans = Convert.ToInt32(dr["YuksekLisans"]);
            CUni.Yuksekokul = Convert.ToInt32(dr["Yuksekokul"]);
            CUni.Bolumler = (from rw in dtB.AsEnumerable()
                             select new UniBolum()
                                 {
                                     bId = Convert.ToInt32(rw["BolumId"]),
                                     Ad= Convert.ToString(rw["Bolum"]),
                                     Fak = Convert.ToString(rw["Fakulte"]),
                                     PT=Convert.ToString(rw["PuanTuru"]),
                                     DV = Convert.ToString(rw["Aciklama"]),
                                     SR = Convert.ToInt32(rw["BasariSirasi"]),
                                     EKP=Convert.ToDecimal(rw["EnKck"]),
                                     Kontenjan= Convert.ToInt32(rw["Kontenjan"]),
                                     Kosullar= Convert.ToString(rw["Kosullar"]),
                                     SU = Convert.ToInt32(rw["SU"]),
                                     BK= Convert.ToString(rw["BK"])
                                 }).ToList<UniBolum>();
            return CUni;
        }


    }
}