﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace czynsze.DataAccess
{
    [Table("vat", Schema="public")]
    public class VatRate
    {
        [Key, Column("__record")]
        public int __record { get; set; }
        
        [Column("nazwa")]
        public string nazwa { get; set; }

        [Column("symb_fisk")]
        public string symb_fisk { get; set; }

        public string[] ImportantFields()
        {
            return new string[]
            {
                __record.ToString(),
                nazwa,
                symb_fisk
            };
        }

        public string[] ImportantFieldsForDropDown()
        {
            return new string[]
            {
                nazwa,
                nazwa
            };
        }

        public string[] AllFields()
        {
            return new string[]
            {
                __record.ToString(),
                nazwa.Trim(),
                symb_fisk.Trim()
            };
        }

        public void Set(string[] record)
        {
            nazwa = record[1];
            symb_fisk = record[2];
        }

        public static string Validate(EnumP.Action action, string[] record)
        {
            string result = String.Empty;
            string nazwa = record[1];

            if (action == EnumP.Action.Usuń)
                using (Czynsze_Entities db = new Czynsze_Entities())
                    if (db.typesOfPayment.Count(t => t.vat == nazwa) > 0)
                        result += "Nie można usunąć stawki VAT, ponieważ jest ona wykorzystywana w innych tabelach! <br />";

            return result;
        }
    }
}