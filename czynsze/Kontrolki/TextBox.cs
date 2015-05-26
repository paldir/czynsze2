﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace czynsze.Kontrolki
{
    public class TextBox : System.Web.UI.WebControls.TextBox
    {
        public enum TextBoxMode { PojedynczaLinia, KilkaLinii, Data, LiczbaCałkowita, LiczbaNiecałkowita, Hasło };
        
        public TextBox(string klasaCss, string id, string tekst, TextBoxMode tryb, int długośćMaksymalna, int liczbaWierszy, bool włączony)
        {
            CssClass = klasaCss;
            ID = id;
            Text = tekst;

            switch (tryb)
            {
                case TextBoxMode.KilkaLinii:
                    TextMode = System.Web.UI.WebControls.TextBoxMode.MultiLine;

                    Attributes.Add("maxlength", długośćMaksymalna.ToString());

                    break;

                case TextBoxMode.LiczbaCałkowita:
                    Attributes.Add("onkeypress", "return isInteger(event)");

                    break;

                case TextBoxMode.LiczbaNiecałkowita:
                    Attributes.Add("onkeypress", "return isFloat(event)");

                    break;

                case TextBoxMode.Data:
                    Attributes.Add("onkeypress", "return isDate(event)");

                    break;

                case TextBoxMode.Hasło:
                    TextMode = System.Web.UI.WebControls.TextBoxMode.Password;

                    break;
            }
            
            MaxLength = długośćMaksymalna; Columns = długośćMaksymalna / liczbaWierszy;
            Rows = liczbaWierszy;
            Enabled = włączony;
        }
    }
}