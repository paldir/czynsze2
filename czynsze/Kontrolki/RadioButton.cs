﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace czynsze.Kontrolki
{
    public class RadioButton : System.Web.UI.WebControls.RadioButton
    {
        public RadioButton(string klasaCss, string id, string nazwaGrupy)
        {
            CssClass = klasaCss;
            ID = id;
            GroupName = nazwaGrupy;
        }
    }
}