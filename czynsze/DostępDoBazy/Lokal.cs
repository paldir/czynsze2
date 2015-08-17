﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace czynsze.DostępDoBazy
{
    public class Lokal : IRekord
    {
        [Key, DatabaseGenerated(databaseGeneratedOption: DatabaseGeneratedOption.None)]
        public int nr_system { get; set; }

        [PrzyjaznaNazwaPola("budynek")]
        public int kod_lok { get; set; }

        [PrzyjaznaNazwaPola("nr lokalu")]
        public int nr_lok { get; set; }

        [PrzyjaznaNazwaPola("typ")]
        public int kod_typ { get; set; }

        [PrzyjaznaNazwaPola("powierzchnia użytkowa")]
        public float pow_uzyt { get; set; }

        [PrzyjaznaNazwaPola("nazwisko")]
        public string nazwisko { get; set; }

        [PrzyjaznaNazwaPola("imię")]
        public string imie { get; set; }

        [PrzyjaznaNazwaPola("adres")]
        public string adres { get; set; }

        [PrzyjaznaNazwaPola("adres cd.")]
        public string adres_2 { get; set; }

        [PrzyjaznaNazwaPola("powierzchnia mieszkalna")]
        public float pow_miesz { get; set; }

        [PrzyjaznaNazwaPola("udział")]
        public float udzial { get; set; }

        [PrzyjaznaNazwaPola("początek zakresu dat")]
        public Nullable<DateTime> dat_od { get; set; }

        [PrzyjaznaNazwaPola("koniec zakresu dat")]
        public Nullable<DateTime> dat_do { get; set; }

        [PrzyjaznaNazwaPola("powierzchnia I pokoju")]
        public float p_1 { get; set; }

        [PrzyjaznaNazwaPola("powierzchnia II pokoju")]
        public float p_2 { get; set; }

        [PrzyjaznaNazwaPola("powierzchnia III pokoju")]
        public float p_3 { get; set; }

        [PrzyjaznaNazwaPola("powierzchnia IV pokoju")]
        public float p_4 { get; set; }

        [PrzyjaznaNazwaPola("powierzchnia V pokoju")]
        public float p_5 { get; set; }

        [PrzyjaznaNazwaPola("powierzchnia VI pokoju")]
        public float p_6 { get; set; }

        [PrzyjaznaNazwaPola("typ kuchni")]
        public Nullable<int> kod_kuch { get; set; }

        [PrzyjaznaNazwaPola("najemca")]
        public Nullable<int> nr_kontr { get; set; }

        [PrzyjaznaNazwaPola("ilość osób")]
        public Nullable<int> il_osob { get; set; }

        [PrzyjaznaNazwaPola("tytuł prawny do lokalu")]
        public Nullable<int> kod_praw { get; set; }

        string uwagi_1 { get; set; }

        string uwagi_2 { get; set; }

        string uwagi_3 { get; set; }

        string uwagi_4 { get; set; }

        [PrzyjaznaNazwaPola("uwagi")]
        [NotMapped]
        public string uwagi
        {
            get { return String.Concat(uwagi_1, uwagi_2, uwagi_3, uwagi_4).Trim(); }

            set
            {
                string uwagi = value.PadRight(240);
                uwagi_1 = uwagi.Substring(0, 60).Trim();
                uwagi_2 = uwagi.Substring(60, 60).Trim();
                uwagi_3 = uwagi.Substring(120, 60).Trim();
                uwagi_4 = uwagi.Substring(180, 60).Trim();
            }
        }

        [NotMapped]
        public int id
        {
            get { return nr_system; }
            set { nr_system = value; }
        }

        public static List<TypLokalu> TypyLokali { get; set; }

        public string[] PolaDoTabeli()
        {


            return new string[] 
            { 
                nr_system.ToString(), 
                kod_lok.ToString(), 
                nr_lok.ToString(), 
                Rozpoznaj_kod_typ(),
                pow_uzyt.ToString("F2"), 
                nazwisko, 
                imie 
            };
        }

        public string Rozpoznaj_kod_typ()
        {
            TypLokalu typLokalu = TypyLokali.FirstOrDefault(t => t.kod_typ == this.kod_typ);

            if (typLokalu == null)
                return String.Empty;
            else
                return typLokalu.typ_lok;
        }

        public string[] WszystkiePola()
        {
            string dat_od, dat_do;

            if (this.dat_od == null)
                dat_od = null;
            else
                dat_od = String.Format(DostępDoBazy.CzynszeKontekst.FormatDaty, this.dat_od);

            if (this.dat_do == null)
                dat_do = null;
            else
                dat_do = String.Format(DostępDoBazy.CzynszeKontekst.FormatDaty, this.dat_do);

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
                uwagi
            };
        }

        public void Ustaw(string[] rekord)
        {
            nr_system = Int32.Parse(rekord[0]);
            kod_lok = Int32.Parse(rekord[1]);
            nr_lok = Int32.Parse(rekord[2]);
            kod_typ = Int32.Parse(rekord[3]);
            adres = rekord[4];
            adres_2 = rekord[5];
            pow_uzyt = Single.Parse(rekord[6]);
            pow_miesz = Single.Parse(rekord[7]);
            udzial = Single.Parse(rekord[8]);

            if (rekord[9] != null)
                dat_od = Convert.ToDateTime(rekord[9]);

            if (rekord[10] != null)
                dat_do = Convert.ToDateTime(rekord[10]);

            p_1 = Single.Parse(rekord[11]);
            p_2 = Single.Parse(rekord[12]);
            p_3 = Single.Parse(rekord[13]);
            p_4 = Single.Parse(rekord[14]);
            p_5 = Single.Parse(rekord[15]);
            p_6 = Single.Parse(rekord[16]);
            kod_kuch = null;
            nr_kontr = Int32.Parse(rekord[18]);

            using (CzynszeKontekst db = new CzynszeKontekst())
            {
                AktywnyNajemca najemca = db.AktywniNajemcy.Where(t => t.nr_kontr == nr_kontr).FirstOrDefault();

                if (najemca == null)
                    nazwisko = imie = String.Empty;
                else
                {
                    nazwisko = najemca.nazwisko;
                    imie = najemca.imie;
                }
            }

            il_osob = Int32.Parse(rekord[19]);
            kod_praw = Int32.Parse(rekord[20]);
            uwagi = rekord[21];
        }

        public string Waliduj(Enumeratory.Akcja akcja, string[] rekord)
        {
            string wynik = "";
            int kod_lok, nr_lok;

            if (akcja == Enumeratory.Akcja.Dodaj)
            {
                if (rekord[2].Length > 0)
                {
                    try
                    {
                        kod_lok = Int32.Parse(rekord[1]);
                        nr_lok = Int32.Parse(rekord[2]);

                        using (CzynszeKontekst db = new CzynszeKontekst())
                            if (db.AktywneLokale.Where(p => p.kod_lok == kod_lok && p.nr_lok == nr_lok).Any())
                                wynik += "W wybranym budynku istnieje już lokal o danym numerze! <br />";
                    }
                    catch { wynik += "Nr lokalu musi być liczbą całkowitą! <br />"; }
                }
                else
                    wynik += "Należy podać numer lokalu! <br />";
            }

            if (akcja != Enumeratory.Akcja.Przenieś)
            {
                wynik += CzynszeKontekst.WalidujFloat("Powierzchnia użytkowa", ref rekord[6]);
                wynik += CzynszeKontekst.WalidujFloat("Powierzchnia mieszkalna", ref rekord[7]);
                wynik += CzynszeKontekst.WalidujFloat("Udział", ref rekord[8]);
                wynik += CzynszeKontekst.WalidujDatę("Początek zakresu dat", ref rekord[9]);
                wynik += CzynszeKontekst.WalidujDatę("Koniec zakresu dat", ref rekord[10]);
                wynik += CzynszeKontekst.WalidujFloat("Powierzchnia I pokoju", ref rekord[11]);
                wynik += CzynszeKontekst.WalidujFloat("Powierzchnia II pokoju", ref rekord[12]);
                wynik += CzynszeKontekst.WalidujFloat("Powierzchnia III pokoju", ref rekord[13]);
                wynik += CzynszeKontekst.WalidujFloat("Powierzchnia IV pokoju", ref rekord[14]);
                wynik += CzynszeKontekst.WalidujFloat("Powierzchnia V pokoju", ref rekord[15]);
                wynik += CzynszeKontekst.WalidujFloat("Powierzchnia VI pokoju", ref rekord[16]);
                wynik += CzynszeKontekst.WalidujInt("Ilość osób", ref rekord[19]);
            }

            return wynik;
        }
    }
}