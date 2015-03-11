﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace czynsze.DataAccess
{
    [Table("nal_" + Receivable.ReceivableYear + "__", Schema = "public")]
    public class ReceivableFrom1stSet : Receivable
    {
        [Key, Column("__record")]
        public override int __record { get; set; }

        [Column("kwota_nal")]
        public override float kwota_nal { get; set; }

        [Column("data_nal")]
        public override DateTime data_nal { get; set; }

        [Column("opis")]
        public override string opis { get; set; }

        [Column("kod_lok")]
        public override int kod_lok { get; set; }

        [Column("nr_lok")]
        public override int nr_lok { get; set; }

        [Column("nr_kontr")]
        public override int nr_kontr { get; set; }

        [Column("nr_skl")]
        public override int nr_skl { get; set; }

        [Column("stawka")]
        public override float stawka { get; set; }

        [Column("ilosc")]
        public override float ilosc { get; set; }
    }
}