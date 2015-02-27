﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace czynsze.DataAccess
{
    public abstract class Turnover : IRecord
    {
        public const string TurnoverYear = "14";

        public abstract int __record { get; set; }

        public abstract float suma { get; set; }

        public abstract string data_obr { get; set; }

        public abstract string opis { get; set; }

        public abstract int nr_kontr { get; set; }

        public abstract int kod_wplat { get; set; }

        public abstract string nr_dowodu { get; set; }

        public abstract int pozycja_d { get; set; }

        public abstract string uwagi { get; set; }

        //public static Dictionary<Enums.SettlementTable, List<Turnover>> SettlementTableToListOfTurnovers { get; private set; }

        enum Account { Wn, Ma };

        /*static Turnover()
        {
            SettlementTableToListOfTurnovers = new Dictionary<Enums.SettlementTable, List<Turnover>>();

            using (Czynsze_Entities db = new Czynsze_Entities())
            {
                SettlementTableToListOfTurnovers.Add(Enums.SettlementTable.Czynsze, new Czynsze_Entities().turnoversFor14.ToList().Cast<DataAccess.Turnover>().ToList());
                SettlementTableToListOfTurnovers.Add(Enums.SettlementTable.SecondSet, new Czynsze_Entities().turnoversFor14From2ndSet.ToList().Cast<DataAccess.Turnover>().ToList());
                SettlementTableToListOfTurnovers.Add(Enums.SettlementTable.ThirdSet, new Czynsze_Entities().turnoversFor14From3rdSet.ToList().Cast<DataAccess.Turnover>().ToList());
            }
        }*/

        public string[] ImportantFields()
        {
            return new string[] 
            {
                __record.ToString(),
                String.Format("{0:N2}", suma),
                data_obr,
                DateTime.Today.ToShortDateString(),
                opis,
                nr_dowodu,
                pozycja_d.ToString(),
                uwagi
            };
        }

        public string[] ImportantFieldsForReceivablesAndTurnoversOfTenant()
        {
            TypeOfPayment type;
            Account account = Account.Wn;
            int factor = 1;
            string wn = String.Empty;
            string ma = String.Empty;

            using (Czynsze_Entities db = new Czynsze_Entities())
                type = db.typesOfPayment.FirstOrDefault(t => t.kod_wplat == kod_wplat);

            switch (type.s_rozli)
            {
                case 1:
                    switch (type.tn_odset)
                    {
                        case 0:
                            account = Account.Wn;
                            factor = -1;

                            break;

                        case 1:
                            account = Account.Ma;

                            break;
                    }

                    break;

                case 2:
                    account = Account.Wn;

                    break;

                case 3:
                    account = Account.Ma;
                    factor = -1;

                    break;
            }

            string suma = (this.suma * factor).ToString("F2");

            switch (account)
            {
                case Account.Wn:
                    wn = suma;

                    break;

                case Account.Ma:
                    ma = suma;

                    break;
            }

            return new string[]
            {
                __record.ToString(),
                wn,
                ma,
                data_obr,
                opis
            };
        }

        public string[] AllFields()
        {
            return new string[]
            {
                __record.ToString(),
                suma.ToString(),
                data_obr.Trim(),
                DateTime.Today.ToShortDateString(),
                kod_wplat.ToString(),
                nr_dowodu.Trim(),
                pozycja_d.ToString(),
                uwagi.Trim(),
                nr_kontr.ToString()
            };
        }

        public void Set(string[] record)
        {
            __record = Convert.ToInt16(record[0]);
            suma = Convert.ToSingle(record[1]);
            data_obr = record[2];
            //data_NO = record[3];
            kod_wplat = Convert.ToInt16(record[4]);
            nr_dowodu = record[5];
            pozycja_d = Convert.ToInt16(record[6]);
            uwagi = record[7];
            nr_kontr = Convert.ToInt16(record[8]);

            using (Czynsze_Entities db = new Czynsze_Entities())
                opis = db.typesOfPayment.FirstOrDefault(t => t.kod_wplat == kod_wplat).typ_wplat;
        }

        public abstract IRecord Find(Czynsze_Entities dataBase, int id);

        public string Validate(Enums.Action action, string[] record)
        {
            string validationResult = String.Empty;

            if (action != Enums.Action.Usuń)
            {
                validationResult += Czynsze_Entities.ValidateFloat("Kwota", ref record[1]);
                validationResult += Czynsze_Entities.ValidateDate("Data", ref record[2]);
                validationResult += Czynsze_Entities.ValidateDate("Data NO", ref record[3]);
                validationResult += Czynsze_Entities.ValidateInt("Pozycja", ref record[6]);
            }

            return validationResult;
        }
    }
}