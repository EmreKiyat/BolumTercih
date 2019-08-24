using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BolumTer.Models
{
    public class TercihListJsonModel
    {
        public List<BolumJsonModel> tJson { get; set; }
        public Guid tId { get; set; }
    }


    public class FilterObjModel
    {
        public int PG { get; set; }
        public string TX { get; set; }
        public List<GModel> DV { get; set; }
        public List<GModel> PT { get; set; }
        public List<GModel> SH { get; set; }
    }

    public class GModel
    { 
        public int Id { get; set; }
        public string T { get; set; }

    }

    //public class BolumReturnModel
    //{
    //    public string Bolum { get; set; }       
    //    public int BolumID { get; set; }
    //    public string Universite { get; set; }
    //    public string PT { get; set; }
    //    public string SH { get; set; }
    //    public int? SR { get; set; }
    //    public decimal? EKP { get; set; }
    //    public string DV { get; set; }

    //}

    public class BolumJsonModel
    {
        public int? Id { get; set; }
    }
    public class ListIdModel
    {
        public Guid? Id { get; set; }
    }
    public class ListIdModelInt
    {
        public int? Id { get; set; }
    }
    public class TercihListModel
    { 
         public int tId { get; set; }
         public string Ad { get; set; }
         public string Uni { get; set; }
         public string PT { get; set; }
         public string DV { get; set; }
         public int? SR { get; set; }
         public decimal? EKP { get; set; }
         public int Sira { get; set; }
         public string BolumKodu { get; set; }
         public string Fakulte { get; set; }
         public string Kosullar { get; set; }
         public int? Sure { get; set; }
         public int? Knt { get; set; }

    }
    public class SearchTextModel
    {
        public string TX { get; set; }
        public int PG { get; set; }
    }

    public class BolumListModel
    {
        public int tId { get; set; }
        public string Ad { get; set; }
        public string Uni { get; set; }
        public string PT { get; set; }
        public string DV { get; set; }
        public int? SR { get; set; }
        public decimal? EKP { get; set; }
        public int Sira { get; set; }
        public int Kontenjan { get; set; }
        public string Kosullar { get; set; }
        public int? SU { get; set; }
        public string BK { get; set; }
    }

    public class KosulModel 
    {
        public string Kosul { get; set; }
        public string Detay { get; set; }
    }

    public class UniModel
    {
        public int UniId { get; set; }
        public string Uni { get; set; }
        public string Sehir { get; set; }
        public int isDevlet { get; set; }
        public string Kurulus { get; set; }
        public string Bolge { get; set; }
        public string web { get; set; }
        public string aWeb { get; set; }
        public string EPosta { get; set; }
        public string Tel { get; set; }
        public string Fax { get; set; }
        public string Adres { get; set; }
        public int Prof { get; set; }
        public int Docent { get; set; }
        public int Ydocent { get; set; }
        public int OgrGorevlisi { get; set; }
        public int Okutman { get; set; }
        public int Uzman { get; set; }
        public int ArsGorevlisi { get; set; }
        public int BirOgr { get; set; }
        public int IkiOgr { get; set; }
        public int Fakulte { get; set; }
        public int Enstitu { get; set; }
        public int Yuksekokul { get; set; }
        public int MYO { get; set; }
        public int ArsUygMer { get; set; }
        public int Bolum { get; set; }
        public int AnabilimDali { get; set; }
        public int Program { get; set; }
        public int YuksekLisans { get; set; }
        public int Doktora { get; set; }
        public List<UniBolum> Bolumler { get; set; }
    }

    public class UniBolum {
        public int bId { get; set; }
        public string Ad { get; set; }
        public string Fak { get; set; }
        public string PT { get; set; }
        public int? SR { get; set; }
        public string DV { get; set; }
        public decimal? EKP { get; set; }
        public int Kontenjan { get; set; }
        public string Kosullar { get; set; }
        public int? SU { get; set; }
        public string BK { get; set; }
    }
    public class Feedback {
        public int localId { get; set; }
        public string EMail { get; set; }
        public string comment { get; set; }
    }
}