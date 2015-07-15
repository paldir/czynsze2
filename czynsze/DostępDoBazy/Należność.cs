﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace czynsze.DostępDoBazy
{
    public abstract class Należność : PozycjaDoAnalizy
    {
        public const string Rok = "15";

        [Key, Column("__record")]
        public int __record { get; set; }

        [Column("kwota_nal")]
        public decimal kwota_nal { get; set; }

        [Column("data_nal")]
        public DateTime data_nal { get; set; }

        [Column("opis")]
        public string opis { get; set; }

        [Column("kod_lok")]
        public int kod_lok { get; set; }

        [Column("nr_lok")]
        public int nr_lok { get; set; }

        [Column("nr_kontr")]
        public int nr_kontr { get; set; }

        [Column("nr_skl")]
        public int nr_skl { get; set; }

        [Column("stawka")]
        public decimal stawka { get; set; }

        [Column("ilosc")]
        public decimal ilosc { get; set; }

        public override DateTime Data
        {
            get { return data_nal; }
        }

        public override decimal Kwota
        {
            get
            {
                switch (Informacje.RodzajEwidencji)
                {
                    case 2:
                    case 3:
                        return -kwota_nal;

                    default:
                        return kwota_nal;
                }
            }
        }

        public override decimal Ilość
        {
            get { return ilosc; }
        }

        public override decimal Stawka
        {
            get { return stawka; }
        }

        public override int IdInformacji
        {
            get { return nr_skl; }
        }

        public override int KodBudynku
        {
            get { return kod_lok; }
        }

        public override int NrLokalu
        {
            get { return nr_lok; }
        }
            
        public string[] WażnePola()
        {
            return new string[]
            {
                __record.ToString(),
                kwota_nal.ToString("F2"),
                String.Format(DostępDoBazy.CzynszeKontekst.FormatDaty, data_nal),
                opis,
                kod_lok.ToString(),
                nr_lok.ToString()
            };
        }

        public string[] WażnePolaDoNależnościIObrotówNajemcy()
        {
            return new string[]
            {
                (-1*__record).ToString(),
                kwota_nal.ToString("F2"),
                String.Empty,
                String.Format(DostępDoBazy.CzynszeKontekst.FormatDaty, data_nal),
                opis
            };
        }

        public void Ustaw(decimal kwota_nal, DateTime data_nal, string opis, int nr_kontr, int nr_skl, int kod_lok, int nr_lok, decimal stawka, decimal ilosc)
        {
            this.kwota_nal = kwota_nal;
            this.data_nal = data_nal;
            this.opis = opis;
            this.nr_kontr = nr_kontr;
            this.nr_skl = nr_skl;
            this.kod_lok = kod_lok;
            this.nr_lok = nr_lok;
            this.stawka = stawka;
            this.ilosc = ilosc;
        }
    }
}