﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace czynsze.DataAccess
{
    [Table("lokale", Schema = "public")]
    public class Place
    {
        [Key, Column("nr_system"), DatabaseGenerated(databaseGeneratedOption: DatabaseGeneratedOption.None)]
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
        public int kod_kuch { get; set; }

        [Column("nr_kontr")]
        public int nr_kontr { get; set; }

        public string[] ImportantFields()
        {
            string kod_typ = String.Empty;
            TypeOfPlace typeOfPlace;
            string kod_kuch = String.Empty;
            TypeOfKitchen typeOfKitchen;

            using (Czynsze_Entities db = new Czynsze_Entities())
            {
                typeOfPlace = db.typesOfPlace.Where(t => t.kod_typ == this.kod_typ).FirstOrDefault();
                typeOfKitchen = db.typesOfKitchen.Where(t => t.kod_kuch == this.kod_kuch).FirstOrDefault();
            }

            if (typeOfPlace != null)
                kod_typ = typeOfPlace.typ_lok;

            if (typeOfKitchen != null)
                kod_kuch = typeOfKitchen.typ_kuch;

            return new string[] { nr_system.ToString(), kod_lok.ToString(), nr_lok.ToString(), kod_typ, pow_uzyt.ToString("F2"), nazwisko, imie };
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

            return new string[] { nr_system.ToString(), kod_lok.ToString(), nr_lok.ToString(), kod_typ.ToString(), adres.Trim(), adres_2.Trim(), pow_uzyt.ToString("F2"), pow_miesz.ToString("F2"), udzial.ToString("F2"), dat_od, dat_do, p_1.ToString("F2"), p_2.ToString("F2"), p_3.ToString("F2"), p_4.ToString("F2"), p_5.ToString("F2"), p_6.ToString("F2"), kod_kuch.ToString() };
        }

        public void Set(string[] record)
        {
            nr_system = Convert.ToInt16(record[0]);
            kod_lok = Convert.ToInt16(record[1]);
            nr_lok = Convert.ToInt16(record[2]);
            kod_typ = Convert.ToInt16(record[3]);
            adres = record[4];
            adres_2 = record[5];
            pow_uzyt = Convert.ToSingle(record[6]);
            pow_miesz = Convert.ToSingle(record[7]);
            udzial = Convert.ToSingle(record[8]);
            dat_od = record[9];
            dat_do = record[10];
            p_1 = Convert.ToSingle(record[11]);
            p_2 = Convert.ToSingle(record[12]);
            p_3 = Convert.ToSingle(record[13]);
            p_4 = Convert.ToSingle(record[14]);
            p_5 = Convert.ToSingle(record[15]);
            p_6 = Convert.ToSingle(record[16]);
            kod_kuch = Convert.ToInt16(record[17]);
        }

        public static string Validate(EnumP.Action action, string[] record)
        {
            string result = "";
            int kod_lok, nr_lok;

            if (action == EnumP.Action.Dodaj)
            {
                if (record[2].Length > 0)
                {
                    try
                    {
                        kod_lok = Convert.ToInt16(record[1]);
                        nr_lok = Convert.ToInt16(record[2]);

                        using (Czynsze_Entities db = new Czynsze_Entities())
                            if (db.places.Where(p => p.kod_lok == kod_lok && p.nr_lok == nr_lok).Count() != 0)
                                result += "W wybranym budynku istnieje już lokal o danym numerze! <br />";
                    }
                    catch { result += "Nr lokalu musi być liczbą całkowitą! <br />"; }
                }
                else
                    result += "Należy podać numer lokalu! <br />";
            }

            if (action != EnumP.Action.Usuń)
            {
                result += Czynsze_Entities.ValidateFloat("Powierzchnia użytkowa", ref record[6]);
                result += Czynsze_Entities.ValidateFloat("Powierzchnia mieszkalna", ref record[7]);
                result += Czynsze_Entities.ValidateFloat("Udział", ref record[8]);
                result += Czynsze_Entities.ValidateDate("Początek zakresu dat", ref record[9]);
                result += Czynsze_Entities.ValidateDate("Koniec zakresu dat", ref record[10]);
                result += Czynsze_Entities.ValidateFloat("Powierzchnia I pokoju", ref record[11]);
                result += Czynsze_Entities.ValidateFloat("Powierzchnia II pokoju", ref record[12]);
                result += Czynsze_Entities.ValidateFloat("Powierzchnia III pokoju", ref record[13]);
                result += Czynsze_Entities.ValidateFloat("Powierzchnia IV pokoju", ref record[14]);
                result += Czynsze_Entities.ValidateFloat("Powierzchnia V pokoju", ref record[15]);
                result += Czynsze_Entities.ValidateFloat("Powierzchnia VI pokoju", ref record[16]);
            }

            return result;
        }
    }
}