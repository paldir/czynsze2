﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace czynsze.DataAccess
{
    [Table("lokale_a", Schema = "public")]
    public class InactivePlace// : Place
    {
        [Key, Column("nr_system")]
        public int nr_system { get; set; }

        [Column("kod_lok")]
        public int kod_lok { get; set; }

        [Column("nr_lok")]
        public int nr_lok { get; set; }

        [Column("kod_typ")]
        public int kod_typ { get; set; }

        [Column("pow_uzyt")]
        public float pow_uzyt { get; set; }

        [Column("nazwisko")]
        public string nazwisko { get; set; }

        [Column("imie")]
        public string imie { get; set; }

        [Column("adres")]
        public string adres { get; set; }

        [Column("adres_2")]
        public string adres_2 { get; set; }

        [Column("pow_miesz")]
        public float pow_miesz { get; set; }

        [Column("udzial")]
        public float udzial { get; set; }

        [Column("dat_od")]
        public string dat_od { get; set; }

        [Column("dat_do")]
        public string dat_do { get; set; }

        [Column("p_1")]
        public float p_1 { get; set; }

        [Column("p_2")]
        public float p_2 { get; set; }

        [Column("p_3")]
        public float p_3 { get; set; }

        [Column("p_4")]
        public float p_4 { get; set; }

        [Column("p_5")]
        public float p_5 { get; set; }

        [Column("p_6")]
        public float p_6 { get; set; }

        [Column("kod_kuch")]
        public Nullable<int> kod_kuch { get; set; }

        [Column("nr_kontr")]
        public Nullable<int> nr_kontr { get; set; }

        [Column("il_osob")]
        public Nullable<int> il_osob { get; set; }

        [Column("kod_praw")]
        public Nullable<int> kod_praw { get; set; }

        [Column("uwagi_1")]
        public string uwagi_1 { get; set; }

        [Column("uwagi_2")]
        public string uwagi_2 { get; set; }

        [Column("uwagi_3")]
        public string uwagi_3 { get; set; }

        [Column("uwagi_4")]
        public string uwagi_4 { get; set; }

        public string[] ImportantFields()
        {
            string kod_typ = String.Empty;
            TypeOfPlace typeOfPlace;

            using (Czynsze_Entities db = new Czynsze_Entities())
                typeOfPlace = db.typesOfPlace.Where(t => t.kod_typ == this.kod_typ).FirstOrDefault();

            if (typeOfPlace != null)
                kod_typ = typeOfPlace.typ_lok;

            return new string[] 
            { 
                nr_system.ToString(), 
                kod_lok.ToString(), 
                nr_lok.ToString(), 
                kod_typ, 
                pow_uzyt.ToString("F2"), 
                nazwisko, 
                imie 
            };
        }

        public string[] AllFields()
        {
            string dat_od, dat_do;

            if (this.dat_od == null)
                dat_od = String.Empty;
            else
                dat_od = this.dat_od.ToString();

            if (this.dat_do == null)
                dat_do = String.Empty;
            else
                dat_do = this.dat_do.ToString();

            return new string[] 
            { 
                nr_system.ToString(), 
                kod_lok.ToString(), 
                nr_lok.ToString(), 
                kod_typ.ToString(), 
                adres.Trim(), 
                adres_2.Trim(), 
                pow_uzyt.ToString("F2"),
                pow_miesz.ToString("F2"), 
                udzial.ToString("F2"), 
                dat_od, 
                dat_do,
                p_1.ToString("F2"),
                p_2.ToString("F2"),
                p_3.ToString("F2"), 
                p_4.ToString("F2"), 
                p_5.ToString("F2"), 
                p_6.ToString("F2"),
                kod_kuch.ToString(), 
                nr_kontr.ToString(), 
                il_osob.ToString(), 
                kod_praw.ToString(), 
                String.Concat(uwagi_1.Trim(), uwagi_2.Trim(), uwagi_3.Trim(), uwagi_4.Trim()) 
            };
        }
    }
}