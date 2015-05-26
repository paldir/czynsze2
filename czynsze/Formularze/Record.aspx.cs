﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace czynsze.Formularze
{
    public partial class Record : Strona
    {
        int id;
        Enumeratory.Akcja action;
        Enumeratory.Tabela table;

        List<DostępDoBazy.AtrybutObiektu> attributesOfObject
        {
            get { return (List<DostępDoBazy.AtrybutObiektu>)Session["attributesOfObject"]; }
            set { Session["attributesOfObject"] = value; }
        }

        List<DostępDoBazy.SkładnikCzynszuLokalu> rentComponentsOfPlace
        {
            get { return (List<DostępDoBazy.SkładnikCzynszuLokalu>)Session["rentComponentsOfPlace"]; }
            set { Session["rentComponentsOfPlace"] = value; }
        }

        List<DostępDoBazy.BudynekWspólnoty> communityBuildings
        {
            get { return (List<DostępDoBazy.BudynekWspólnoty>)Session["communityBuildings"]; }
            set { Session["communityBuildings"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            bool globalEnabled = false;
            bool idEnabled = false;
            string[] values = (string[])Session["values"];
            int numberOfFields = 0;
            string[] labels = null;
            string heading = null;
            List<Kontrolki.Button> buttons = new List<Kontrolki.Button>();
            List<Control> controls = new List<Control>();
            List<int> columnSwitching = null;
            List<Kontrolki.HtmlIframe> tabs = null;
            List<Kontrolki.HtmlInputRadioButton> tabButtons = null;
            List<Kontrolki.Label> labelsOfTabButtons = null;
            List<Control> preview = null;
            //id = Int32.Parse(Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("id"))]);
            id = PobierzWartośćParametru<int>("id");
            //action = (EnumP.Action)Enum.Parse(typeof(EnumP.Action), Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("action"))]);
            action = PobierzWartośćParametru<Enumeratory.Akcja>("action");
            //table = (EnumP.Table)Enum.Parse(typeof(EnumP.Table), Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("table"))]);
            table = PobierzWartośćParametru<Enumeratory.Tabela>("table");
            string backUrl = "javascript: Load('" + Request.UrlReferrer + "')";
            Dictionary<bool, string> fromIdEnabledToIdSuffix = new Dictionary<bool, string>()
            {
                {true, String.Empty},
                {false, "_disabled"}
            };

            switch (action)
            {
                case Enumeratory.Akcja.Dodaj:
                    globalEnabled = idEnabled = true;
                    heading = "Dodawanie ";

                    buttons.Add(new Kontrolki.Button("buttons", "Save", "Zapisz", "RecordValidation.aspx"));
                    buttons.Add(new Kontrolki.Button("buttons", "Cancel", "Anuluj", backUrl));

                    break;

                case Enumeratory.Akcja.Edytuj:
                    globalEnabled = true;
                    idEnabled = false;
                    heading = "Edycja ";

                    buttons.Add(new Kontrolki.Button("buttons", "Save", "Zapisz", "RecordValidation.aspx"));
                    buttons.Add(new Kontrolki.Button("buttons", "Cancel", "Anuluj", backUrl));

                    break;

                case Enumeratory.Akcja.Usuń:
                    globalEnabled = idEnabled = false;
                    heading = "Usuwanie ";

                    buttons.Add(new Kontrolki.Button("buttons", "Delete", "Usuń", "RecordValidation.aspx"));
                    buttons.Add(new Kontrolki.Button("buttons", "Cancel", "Anuluj", backUrl));

                    break;

                case Enumeratory.Akcja.Przeglądaj:
                    globalEnabled = idEnabled = false;
                    heading = "Przeglądanie ";

                    buttons.Add(new Kontrolki.Button("buttons", "Back", "Powrót", backUrl));

                    break;

                case Enumeratory.Akcja.Przenieś:
                    globalEnabled = idEnabled = false;
                    heading = "Przenoszenie ";

                    buttons.Add(new Kontrolki.Button("buttons", "Move", "Przenieś", "RecordValidation.aspx"));
                    buttons.Add(new Kontrolki.Button("buttons", "Cancel", "Anuluj", backUrl));

                    break;
            }

            using (DostępDoBazy.CzynszeKontekst db = new DostępDoBazy.CzynszeKontekst())
                switch (table)
                {
                    case Enumeratory.Tabela.Budynki:
                        Title = "Budynek";
                        numberOfFields = 7;
                        heading += "budynku";
                        columnSwitching = new List<int>() { 0, 6 };
                        labels = new string[] 
                        { 
                            "Kod budynku: ", 
                            "Ilość lokali: ", 
                            "Sposób rozliczania: ", 
                            "Adres: ", 
                            "Adres cd.: ",
                            "Udział w koszt.: ",
                            "Uwagi: " 
                        };

                        tabButtons = new List<Kontrolki.HtmlInputRadioButton>()
                        {
                            new Kontrolki.HtmlInputRadioButton("tabRadio", "dane", "tabRadios", "dane", true),
                            new Kontrolki.HtmlInputRadioButton("tabRadio", "cechy", "tabRadios", "cechy", false),
                        };

                        labelsOfTabButtons = new List<Kontrolki.Label>()
                        {
                            new Kontrolki.Label("tabLabel", tabButtons.ElementAt(0).ID, "Dane", String.Empty),
                            new Kontrolki.Label("tabLabel", tabButtons.ElementAt(1).ID, "Cechy", String.Empty),
                        };

                        tabs = new List<Kontrolki.HtmlIframe>()
                        {
                            new Kontrolki.HtmlIframe("tab", "cechy_tab", "AttributeOfObject.aspx?attributeOf="+Enumeratory.Atrybut.Budynku+"&parentId="+id.ToString()+"&action="+action.ToString()+"&childAction=Przeglądaj", "hidden")
                        };

                        if (values == null)
                        {
                            if (action != Enumeratory.Akcja.Dodaj)
                                values = db.Budynki.FirstOrDefault(b => b.kod_1 == id).WszystkiePola();
                            else
                                values = new string[numberOfFields];

                            attributesOfObject = new List<DostępDoBazy.AtrybutObiektu>();

                            foreach (DostępDoBazy.AtrybutBudynku attributeOfBuilding in db.AtrybutyBudynków.ToList().Where(a => Int32.Parse(a.kod_powiaz) == id))
                                attributesOfObject.Add(attributeOfBuilding);
                        }

                        preview = new List<Control>()
                        {
                            new LiteralControl("Kod budynku: "),
                            new Kontrolki.Label("previewLabel", String.Empty, values[0], "id_preview"),
                            new LiteralControl("Adres: "),
                            new Kontrolki.Label("previewLabel", String.Empty, values[3], "adres_preview"),
                            new LiteralControl("Adres cd.: "),
                            new Kontrolki.Label("previewLabel", String.Empty, values[4], "adres_2_preview")
                        };
                        
                        if (!idEnabled)
                            form.Controls.Add(new Kontrolki.HtmlInputHidden("id", id.ToString()));

                        controls.Add(new Kontrolki.TextBox("field", "id" + fromIdEnabledToIdSuffix[idEnabled], values[0], Kontrolki.TextBox.TextBoxMode.LiczbaCałkowita, 5, 1, idEnabled));
                        controls.Add(new Kontrolki.TextBox("field", "il_miesz", values[1], Kontrolki.TextBox.TextBoxMode.LiczbaCałkowita, 3, 1, globalEnabled));
                        controls.Add(new Kontrolki.RadioButtonList("field", "sp_rozl", new List<string>() { "budynek", "lokale" }, new List<string>() { "0", "1" }, values[2], globalEnabled, false));
                        controls.Add(new Kontrolki.TextBox("field", "adres", values[3], Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 30, 1, globalEnabled));
                        controls.Add(new Kontrolki.TextBox("field", "adres_2", values[4], Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 30, 1, globalEnabled));
                        controls.Add(new Kontrolki.TextBox("field", "udzial_w_k", values[5], Kontrolki.TextBox.TextBoxMode.LiczbaNiecałkowita, 6, 1, globalEnabled));
                        controls.Add(new Kontrolki.TextBox("field", "uwagi", values[6], Kontrolki.TextBox.TextBoxMode.KilkaLinii, 420, 6, globalEnabled));

                        break;

                    case Enumeratory.Tabela.AktywneLokale:
                    case Enumeratory.Tabela.NieaktywneLokale:

                        Title = "Lokal";
                        numberOfFields = 22;
                        heading += "lokalu";

                        if (table == Enumeratory.Tabela.NieaktywneLokale)
                        {
                            Title = "Lokal (nieaktywny)";
                            heading += "(nieaktywnego)";
                        }

                        columnSwitching = new List<int> { 0, 5, 10, 17 };
                        labels = new string[] 
                        { 
                            //"Nr system: ", 
                            "Budynek: ", 
                            "Nr lokalu: ",
                            "Typ: ", 
                            "Adres: ",
                            "Adres cd.: ",
                            "Powierzchnia użytkowa: ", 
                            "Powierzchnia mieszkalna: ", 
                            "Udział: ", 
                            "Początek zakresu dat: ",
                            "Koniec zakresu dat: ", 
                            "Powierzchnia I pokoju: ",
                            "Powierzchnia II pokoju: ", 
                            "Powierzchnia III pokoju: ", 
                            "Powierzchnia IV pokoju: ", 
                            "Powierzchnia V pokoju: ",
                            "Powierzchnia VI pokoju: ", 
                            "Typ kuchni: ", 
                            "Najemca: ", 
                            "Ilość osób: ", 
                            "Tytuł prawny do lokalu: ", 
                            "Uwagi: " 
                        };

                        if (values == null)
                        {
                            if (action != Enumeratory.Akcja.Dodaj)
                            {
                                if (table == Enumeratory.Tabela.AktywneLokale)
                                    values = db.AktywneLokale.FirstOrDefault(b => b.nr_system == id).WszystkiePola();
                                else
                                    values = db.NieaktywneLokale.FirstOrDefault(b => b.nr_system == id).WszystkiePola();
                            }
                            else
                            {
                                values = new string[numberOfFields];

                                IEnumerable<DostępDoBazy.Lokal> places = db.AktywneLokale.ToList().Cast<DostępDoBazy.Lokal>().Concat(db.NieaktywneLokale.ToList().Cast<DostępDoBazy.Lokal>());

                                if (places.Any())
                                    values[0] = (places.Max(p => p.nr_system) + 1).ToString();
                                else
                                    values[0] = "0";

                                values[1] = values[2] = "0";
                            }

                            attributesOfObject = new List<DostępDoBazy.AtrybutObiektu>();
                            rentComponentsOfPlace = new List<DostępDoBazy.SkładnikCzynszuLokalu>();

                            attributesOfObject.AddRange(db.AtrybutyLokali.ToList().Where(a => Int32.Parse(a.kod_powiaz) == id));
                            rentComponentsOfPlace.AddRange(db.SkładnikiCzynszuLokalu.ToList().Where(c => c.kod_lok == Int32.Parse(values[1]) && c.nr_lok == Int32.Parse(values[2])));
                        }

                        tabButtons = new List<Kontrolki.HtmlInputRadioButton>()
                        {
                            new Kontrolki.HtmlInputRadioButton("tabRadio", "dane", "tabRadios", "dane", true),
                            new Kontrolki.HtmlInputRadioButton("tabRadio", "skladnikiCzynszu", "tabRadios", "skladnikiCzynszu", false),
                            new Kontrolki.HtmlInputRadioButton("tabRadio", "cechy", "tabRadios", "cechy", false),
                            new Kontrolki.HtmlInputRadioButton("tabRadio", "dokumenty", "tabRadios", "dokumenty", false)
                        };

                        labelsOfTabButtons = new List<Kontrolki.Label>()
                        {
                            new Kontrolki.Label("tabLabel", tabButtons.ElementAt(0).ID, "Dane", String.Empty),
                            new Kontrolki.Label("tabLabel", tabButtons.ElementAt(1).ID, "Składniki czynszu", String.Empty),
                            new Kontrolki.Label("tabLabel", tabButtons.ElementAt(2).ID, "Cechy", String.Empty),
                            new Kontrolki.Label("tabLabel", tabButtons.ElementAt(3).ID, "Dokumenty", String.Empty)
                        };

                        preview = new List<Control>()
                        {
                            new LiteralControl("Nr budynku: "),
                            new Kontrolki.Label("previewLabel", String.Empty, values[1], "kod_lok_preview"),
                            new LiteralControl("Nr lokalu: "),
                            new Kontrolki.Label("previewLabel", String.Empty, values[2], "nr_lok_preview"),
                            new LiteralControl("Adres: "),
                            new Kontrolki.Label("previewLabel", String.Empty, values[4], "adres_preview"),
                            new LiteralControl("Adres cd.: "),
                            new Kontrolki.Label("previewLabel", String.Empty, values[5], "adres_2_preview")
                        };

                        //
                        //CXP PART
                        //
                        string parentAction;

                        switch (action)
                        {
                            case Enumeratory.Akcja.Dodaj:
                                parentAction = "add";

                                break;

                            case Enumeratory.Akcja.Edytuj:
                                parentAction = "edit";

                                break;

                            case Enumeratory.Akcja.Usuń:
                                parentAction = "delete";

                                break;

                            default:
                                parentAction = "browse";

                                break;
                        }
                        //
                        //TO DUMP BEHIND THE WALL
                        //

                        tabs = new List<Kontrolki.HtmlIframe>()
                        {
                            //new Kontrolki.HtmlIframe("tab", "skladnikiCzynszu_tab", "/czynsze1/SkladnikiCzynszuLokalu.cxp?parentAction="+parentAction+"&kod_lok="+values[1]+"&nr_lok="+values[2], "hidden"),
                            new Kontrolki.HtmlIframe("tab", "skladnikiCzynszu_tab", "RentComponentsOfPlace.aspx?parentAction="+action.ToString()+"&kod_lok="+values[1]+"&nr_lok="+values[2], "hidden"),
                            new Kontrolki.HtmlIframe("tab", "cechy_tab", "AttributeOfObject.aspx?attributeOf="+Enumeratory.Atrybut.Lokalu+"&parentId="+id.ToString()+"&action="+action.ToString()+"&childAction=Przeglądaj", "hidden"),
                            new Kontrolki.HtmlIframe("tab", "dokumenty_tab", "/czynsze1/PlikiNajemcy.cxp?parentAction="+parentAction+"&nr_system="+values[0], "hidden")
                        };

                        //controls.Add(new Kontrolki.TextBoxP("field", "Nr_system_disabled", values[0], Kontrolki.TextBoxP.TextBoxMode.Number, 14, 1, false));
                        form.Controls.Add(new Kontrolki.HtmlInputHidden("id", values[0]));

                        if (!idEnabled)
                        {
                            form.Controls.Add(new Kontrolki.HtmlInputHidden("kod_lok", values[1]));
                            form.Controls.Add(new Kontrolki.HtmlInputHidden("nr_lok", values[2]));
                        }

                        controls.Add(new Kontrolki.DropDownList("field", "kod_lok"+fromIdEnabledToIdSuffix[idEnabled], db.Budynki.ToList().OrderBy(b => b.kod_1).Select(b => b.WażnePola()).ToList(), values[1], idEnabled, false));
                        controls.Add(new Kontrolki.TextBox("field", "nr_lok" + fromIdEnabledToIdSuffix[idEnabled], values[2], Kontrolki.TextBox.TextBoxMode.LiczbaCałkowita, 3, 1, idEnabled));
                        controls.Add(new Kontrolki.DropDownList("field", "kod_typ", db.TypyLokali.ToList().Select(t => t.WażnePolaDoRozwijanejListy()).ToList(), values[3], globalEnabled, false));
                        controls.Add(new Kontrolki.TextBox("field", "adres", values[4], Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 30, 1, globalEnabled));
                        controls.Add(new Kontrolki.TextBox("field", "adres_2", values[5], Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 30, 1, globalEnabled));
                        controls.Add(new Kontrolki.TextBox("field", "pow_uzyt", values[6], Kontrolki.TextBox.TextBoxMode.LiczbaNiecałkowita, 8, 1, globalEnabled));
                        controls.Add(new Kontrolki.TextBox("field", "pow_miesz", values[7], Kontrolki.TextBox.TextBoxMode.LiczbaNiecałkowita, 8, 1, globalEnabled));
                        controls.Add(new Kontrolki.TextBox("field", "udzial", values[8], Kontrolki.TextBox.TextBoxMode.LiczbaNiecałkowita, 5, 1, globalEnabled));
                        controls.Add(new Kontrolki.TextBox("field", "dat_od", values[9], Kontrolki.TextBox.TextBoxMode.Data, 10, 1, globalEnabled));
                        controls.Add(new Kontrolki.TextBox("field", "dat_do", values[10], Kontrolki.TextBox.TextBoxMode.Data, 10, 1, globalEnabled));
                        controls.Add(new Kontrolki.TextBox("field", "p_1", values[11], Kontrolki.TextBox.TextBoxMode.LiczbaNiecałkowita, 5, 1, globalEnabled));
                        controls.Add(new Kontrolki.TextBox("field", "p_2", values[12], Kontrolki.TextBox.TextBoxMode.LiczbaNiecałkowita, 5, 1, globalEnabled));
                        controls.Add(new Kontrolki.TextBox("field", "p_3", values[13], Kontrolki.TextBox.TextBoxMode.LiczbaNiecałkowita, 5, 1, globalEnabled));
                        controls.Add(new Kontrolki.TextBox("field", "p_4", values[14], Kontrolki.TextBox.TextBoxMode.LiczbaNiecałkowita, 5, 1, globalEnabled));
                        controls.Add(new Kontrolki.TextBox("field", "p_5", values[15], Kontrolki.TextBox.TextBoxMode.LiczbaNiecałkowita, 5, 1, globalEnabled));
                        controls.Add(new Kontrolki.TextBox("field", "p_6", values[16], Kontrolki.TextBox.TextBoxMode.LiczbaNiecałkowita, 5, 1, globalEnabled));
                        controls.Add(new Kontrolki.DropDownList("field", "kod_kuch", db.TypyKuchni.ToList().Select(t => t.WażnePolaDoRozwijanejListy()).ToList(), values[17], globalEnabled, false));
                        controls.Add(new Kontrolki.DropDownList("field", "nr_kontr", db.AktywniNajemcy.OrderBy(t => t.nazwisko).ToList().Select(t => t.WażnePola().ToList().GetRange(1, 4).ToArray()).ToList(), values[18], globalEnabled, true));
                        controls.Add(new Kontrolki.TextBox("field", "il_osob", values[19], Kontrolki.TextBox.TextBoxMode.LiczbaCałkowita, 3, 1, globalEnabled));
                        controls.Add(new Kontrolki.DropDownList("field", "kod_praw", db.TytułyPrawne.ToList().Select(t => t.WażnePolaDoRozwijanejListy()).ToList(), values[20], globalEnabled, false));
                        controls.Add(new Kontrolki.TextBox("field", "uwagi", values[21], Kontrolki.TextBox.TextBoxMode.KilkaLinii, 240, 4, globalEnabled));

                        //
                        //CXP PART
                        //
                        try
                        {
                            switch (action)
                            {
                                case Enumeratory.Akcja.Dodaj:
                                    //db.Database.ExecuteSqlCommand("CREATE TABLE skl_cz_tmp AS SELECT * FROM skl_cz WHERE 1=2");
                                    db.Database.ExecuteSqlCommand("CREATE TABLE pliki_tmp AS SELECT * FROM pliki WHERE 1=2");

                                    break;

                                default:
                                    //db.Database.ExecuteSqlCommand("CREATE TABLE skl_cz_tmp AS SELECT * FROM skl_cz WHERE kod_lok=" + values[1] + " AND nr_lok=" + values[2]);
                                    db.Database.ExecuteSqlCommand("CREATE TABLE pliki_tmp AS SELECT * FROM pliki WHERE nr_system=" + values[0]);

                                    break;
                            }
                        }
                        catch { }
                        //
                        //TO DUMP BEHIND THE WALL
                        //

                        break;

                    case Enumeratory.Tabela.AktywniNajemcy:
                    case Enumeratory.Tabela.NieaktywniNajemcy:
                        Title = "Najemca";
                        numberOfFields = 12;
                        heading += "najemcy";
                        columnSwitching = new List<int> { 0, 6 };
                        labels = new string[] 
                        { 
                            "Nr kontrolny: ", 
                            "Najemca: ", 
                            "Nazwisko: ",
                            "Imię: ", 
                            "Adres: ", 
                            "Adres cd.: ", 
                            "Numer dowodu osobistego: ", 
                            "Pesel: ", 
                            "Zakład pracy: ",
                            "Login/e-mail: ", 
                            "Hasło: ", 
                            "Uwagi: " 
                        };

                        if (table == Enumeratory.Tabela.NieaktywniNajemcy)
                        {
                            Title = "Najemca (nieaktywny)";
                            heading += "(nieaktywnego)";
                        }

                        if (values == null)
                        {
                            if (action != Enumeratory.Akcja.Dodaj)
                                switch (table)
                                {
                                    case Enumeratory.Tabela.AktywniNajemcy:
                                        values = db.AktywniNajemcy.FirstOrDefault(t => t.nr_kontr == id).WszystkiePola();

                                        break;

                                    case Enumeratory.Tabela.NieaktywniNajemcy:
                                        values = db.NieaktywniNajemcy.FirstOrDefault(t => t.nr_kontr == id).WszystkiePola();

                                        break;
                                }
                            else
                            {
                                values = new string[numberOfFields];
                                IEnumerable<DostępDoBazy.Najemca> tenants = db.AktywniNajemcy.ToList().Cast<DostępDoBazy.Najemca>().Concat(db.NieaktywniNajemcy.Cast<DostępDoBazy.Najemca>());

                                if (tenants.Any())
                                    values[0] = (tenants.Max(t => t.nr_kontr) + 1).ToString();
                                else
                                    values[0] = "1";
                            }

                            attributesOfObject = new List<DostępDoBazy.AtrybutObiektu>();

                            foreach (DostępDoBazy.AtrybutNajemcy attributeOfTenant in db.AtrybutyNajemców.ToList().Where(a => Int32.Parse(a.kod_powiaz) == id))
                                attributesOfObject.Add(attributeOfTenant);
                        }

                        preview = new List<Control>()
                        {
                            new LiteralControl("Numer kontrolny: "),
                            new Kontrolki.Label("previewLabel", String.Empty, values[0], "id_preview"),
                            new LiteralControl("Nazwisko: "),
                            new Kontrolki.Label("previewLabel", String.Empty, values[2], "nazwisko_preview"),
                            new LiteralControl("Imię: "),
                            new Kontrolki.Label("previewLabel", String.Empty, values[3], "imie_preview"),
                            new LiteralControl("Adres: "),
                            new Kontrolki.Label("previewLabel", String.Empty, values[4], "adres_1_preview"),
                            new LiteralControl("Adres cd.: "),
                            new Kontrolki.Label("previewLabel", String.Empty, values[5], "adres_2_preview")
                        };

                        tabButtons = new List<Kontrolki.HtmlInputRadioButton>()
                        {
                            new Kontrolki.HtmlInputRadioButton("tabRadio", "dane", "tabRadios", "dane", true),
                            new Kontrolki.HtmlInputRadioButton("tabRadio", "cechy", "tabRadios", "cechy", false),
                        };

                        labelsOfTabButtons = new List<Kontrolki.Label>()
                        {
                            new Kontrolki.Label("tabLabel", tabButtons.ElementAt(0).ID, "Dane", String.Empty),
                            new Kontrolki.Label("tabLabel", tabButtons.ElementAt(1).ID, "Cechy", String.Empty),
                        };

                        tabs = new List<Kontrolki.HtmlIframe>()
                        {
                            new Kontrolki.HtmlIframe("tab", "cechy_tab", "AttributeOfObject.aspx?attributeOf="+Enumeratory.Atrybut.Najemcy+"&parentId="+id.ToString()+"&action="+action.ToString()+"&childAction=Przeglądaj", "hidden")
                        };

                        controls.Add(new Kontrolki.TextBox("field", "nr_kontr_disabled", values[0], Kontrolki.TextBox.TextBoxMode.LiczbaCałkowita, 6, 1, false));
                        placeOfButtons.Controls.Add(new Kontrolki.HtmlInputHidden("id", values[0]));
                        controls.Add(new Kontrolki.DropDownList("field", "kod_najem", db.TypyNajemców.ToList().Select(t => t.WażnePolaDoRozwijanejListy()).ToList(), values[1], globalEnabled, false));
                        controls.Add(new Kontrolki.TextBox("field", "nazwisko", values[2], Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 25, 1, globalEnabled));
                        controls.Add(new Kontrolki.TextBox("field", "imie", values[3], Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 25, 1, globalEnabled));
                        controls.Add(new Kontrolki.TextBox("field", "adres_1", values[4], Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 30, 1, globalEnabled));
                        controls.Add(new Kontrolki.TextBox("field", "adres_2", values[5], Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 30, 1, globalEnabled));
                        controls.Add(new Kontrolki.TextBox("field", "nr_dow", values[6], Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 9, 1, globalEnabled));
                        controls.Add(new Kontrolki.TextBox("field", "pesel", values[7], Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 11, 1, globalEnabled));
                        controls.Add(new Kontrolki.TextBox("field", "nazwa_z", values[8], Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 40, 1, globalEnabled));
                        controls.Add(new Kontrolki.TextBox("field", "e_mail", values[9], Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 40, 1, globalEnabled));
                        controls.Add(new Kontrolki.TextBox("field", "l__has", values[10], Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 15, 1, globalEnabled));
                        controls.Add(new Kontrolki.TextBox("field", "uwagi", values[11], Kontrolki.TextBox.TextBoxMode.KilkaLinii, 120, 2, globalEnabled));

                        break;

                    case Enumeratory.Tabela.SkladnikiCzynszu:
                        Title = "Składnik opłat";
                        numberOfFields = 19;
                        heading += "składnika opłat";
                        columnSwitching = new List<int> { 0, 6, 9 };
                        labels = new string[]
                        {
                            "Nr składnika: ",
                            "Nazwa: ",
                            "Rodzaj ewidencji: ",
                            "Sposób naliczania: ",
                            "Stawka: ",
                            "Stawka do korespondencji: ",
                            "Typ składnika: ",
                            "Początek okresu naliczania: ",
                            "Koniec okresu naliczania: ",
                            "Przedziały za osobę (dotyczy sposoby naliczania &quot;za osobę - przedziały&quot): "
                        };

                        if (values == null)
                        {
                            if (action != Enumeratory.Akcja.Dodaj)
                                values = db.SkładnikiCzynszu.FirstOrDefault(c => c.nr_skl == id).WszystkiePola();
                            else
                                values = new string[numberOfFields];
                        }

                        if (!idEnabled)
                            form.Controls.Add(new Kontrolki.HtmlInputHidden("id", values[0]));

                        controls.Add(new Kontrolki.TextBox("field", "id" + fromIdEnabledToIdSuffix[idEnabled], values[0], Kontrolki.TextBox.TextBoxMode.LiczbaCałkowita, 3, 1, idEnabled));
                        controls.Add(new Kontrolki.TextBox("field", "nazwa", values[1], Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 30, 1, globalEnabled));
                        controls.Add(new Kontrolki.DropDownList("field", "rodz_e", new List<string[]> { new string[] { "1", "dziennik komornego" }, new string[] { "2", "wpłaty" }, new string[] { "3", "zmniejszenia" }, new string[] { "4", "zwiększenia" } }, values[2], globalEnabled, false));
                        controls.Add(new Kontrolki.DropDownList("field", "s_zaplat", new List<string[]> { new string[] { "1", "za m2 pow. użytkowej" }, new string[] { "2", "za określoną ilość" }, new string[] { "3", "za osobę" }, new string[] { "4", "za lokal" }, new string[] { "5", "za ilość dni w miesiącu" }, new string[] { "6", "za osobę - przedziały" } }, values[3], globalEnabled, false));
                        controls.Add(new Kontrolki.TextBox("field", "stawka", values[4], Kontrolki.TextBox.TextBoxMode.LiczbaNiecałkowita, 10, 1, globalEnabled));
                        controls.Add(new Kontrolki.TextBox("field", "stawka_inf", values[5], Kontrolki.TextBox.TextBoxMode.LiczbaNiecałkowita, 10, 1, globalEnabled));
                        controls.Add(new Kontrolki.DropDownList("field", "typ_skl", new List<string[]> { new string[] { "0", "stały" }, new string[] { "1", "zmienny" } }, values[6], globalEnabled, false));
                        controls.Add(new Kontrolki.TextBox("field", "data_1", values[7], Kontrolki.TextBox.TextBoxMode.Data, 10, 1, globalEnabled));
                        controls.Add(new Kontrolki.TextBox("field", "data_2", values[8], Kontrolki.TextBox.TextBoxMode.Data, 10, 1, globalEnabled));

                        Table interval = new Table();
                        TableHeaderRow headerRow = new TableHeaderRow();
                        TableHeaderCell headerCell = new TableHeaderCell();

                        headerCell.Controls.Add(new LiteralControl("Os."));
                        headerRow.Cells.Add(headerCell);

                        headerCell = new TableHeaderCell();

                        headerCell.Controls.Add(new LiteralControl("Cena"));
                        headerRow.Cells.Add(headerCell);
                        interval.Rows.Add(headerRow);

                        for (int i = 0; i < 10; i++)
                        {
                            TableRow tableRow = new TableRow();
                            TableCell tableCell = new TableCell();

                            tableCell.Controls.Add(new LiteralControl(i.ToString()));
                            tableRow.Controls.Add(tableCell);

                            tableCell = new TableCell();

                            tableCell.Controls.Add(new Kontrolki.TextBox("field", "stawka_0" + i.ToString(), values[i + 9], Kontrolki.TextBox.TextBoxMode.LiczbaNiecałkowita, 10, 1, globalEnabled));
                            tableRow.Cells.Add(tableCell);
                            interval.Rows.Add(tableRow);
                        }

                        controls.Add(interval);

                        break;

                    case Enumeratory.Tabela.Wspolnoty:
                        Title = "Wspólnota";
                        numberOfFields = 12;
                        heading += "wspólnoty";
                        columnSwitching = new List<int>() { 0, 7 };
                        labels = new string[]
                        {
                            "Kod wspólnoty: ",
                            "Ilość budynków: ",
                            "Ilość lokali: ",
                            "Nazwa pełna wspólnoty: ",
                            "Nazwa skrócona: ",
                            "Adres wspólnoty: ",
                            "Adres cd.: ",
                            "Nr konta 1: ",
                            "Nr konta 2: ",
                            "Nr konta 3: ",
                            "Ścieżka do F-K: ",
                            "Uwagi: "
                        };

                        if (values == null)
                        {
                            if (action != Enumeratory.Akcja.Dodaj)
                                values = db.Wspólnoty.FirstOrDefault(c => c.kod == id).WszystkiePola();
                            else
                                values = new string[numberOfFields];

                            attributesOfObject = new List<DostępDoBazy.AtrybutObiektu>();
                            communityBuildings = new List<DostępDoBazy.BudynekWspólnoty>();

                            attributesOfObject.AddRange(db.AtrybutyWspólnot.ToList().Where(a => Int32.Parse(a.kod_powiaz) == id));
                            communityBuildings.AddRange(db.BudynkiWspólnot.Where(c => c.kod == id).OrderBy(b => b.kod_1));
                        }

                        preview = new List<Control>()
                        {
                            new LiteralControl("Kod: "),
                            new Kontrolki.Label("previewLabel", String.Empty, values[0], "id_preview"),
                            new LiteralControl("Nazwa: "),
                            new Kontrolki.Label("previewLabel", String.Empty, values[4], "nazwa_skr_preview"),
                            new LiteralControl("Ilość budynków: "),
                            new Kontrolki.Label("previewLabel", String.Empty, values[1], "il_bud_preview"),
                            new LiteralControl("Ilość lokali: "),
                            new Kontrolki.Label("previewLabel", String.Empty, values[2], "il_miesz_preview")
                        };

                        tabButtons = new List<Kontrolki.HtmlInputRadioButton>()
                        {
                            new Kontrolki.HtmlInputRadioButton("tabRadio", "dane", "tabRadios", "dane", true),
                            new Kontrolki.HtmlInputRadioButton("tabRadio", "budynki", "tabRadios", "budynki", false),
                            new Kontrolki.HtmlInputRadioButton("tabRadio", "cechy", "tabRadios", "cechy", false),
                        };

                        labelsOfTabButtons = new List<Kontrolki.Label>()
                        {
                            new Kontrolki.Label("tabLabel", tabButtons.ElementAt(0).ID, "Dane", String.Empty),
                            new Kontrolki.Label("tabLabel", tabButtons.ElementAt(1).ID, "Budynki", String.Empty),
                            new Kontrolki.Label("tabLabel", tabButtons.ElementAt(2).ID, "Cechy", String.Empty),
                        };

                        tabs = new List<Kontrolki.HtmlIframe>()
                        {
                            new Kontrolki.HtmlIframe("tab", "budynki_tab", "CommunityBuildings.aspx?kod="+id.ToString()+"&parentAction="+action.ToString(), "hidden"),
                            new Kontrolki.HtmlIframe("tab", "cechy_tab", "AttributeOfObject.aspx?attributeOf="+Enumeratory.Atrybut.Wspólnoty+"&parentId="+id.ToString()+"&action="+action.ToString()+"&childAction=Przeglądaj", "hidden")
                        };

                        if (!idEnabled)
                            form.Controls.Add(new Kontrolki.HtmlInputHidden("id", values[0]));

                        controls.Add(new Kontrolki.TextBox("field", "id" + fromIdEnabledToIdSuffix[idEnabled], values[0], Kontrolki.TextBox.TextBoxMode.LiczbaCałkowita, 5, 1, idEnabled));
                        controls.Add(new Kontrolki.TextBox("field", "il_bud", values[1], Kontrolki.TextBox.TextBoxMode.LiczbaCałkowita, 3, 1, globalEnabled));
                        controls.Add(new Kontrolki.TextBox("field", "il_miesz", values[2], Kontrolki.TextBox.TextBoxMode.LiczbaCałkowita, 4, 1, globalEnabled));
                        controls.Add(new Kontrolki.TextBox("field", "nazwa_pel", values[3], Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 50, 1, globalEnabled));
                        controls.Add(new Kontrolki.TextBox("field", "nazwa_skr", values[4], Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 30, 1, globalEnabled));
                        controls.Add(new Kontrolki.TextBox("field", "adres", values[5], Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 30, 1, globalEnabled));
                        controls.Add(new Kontrolki.TextBox("field", "adres_2", values[6], Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 30, 1, globalEnabled));
                        controls.Add(new Kontrolki.TextBox("field", "nr1_konta", values[7], Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 32, 1, globalEnabled));
                        controls.Add(new Kontrolki.TextBox("field", "nr2_konta", values[8], Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 32, 1, globalEnabled));
                        controls.Add(new Kontrolki.TextBox("field", "nr3_konta", values[9], Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 32, 1, globalEnabled));
                        controls.Add(new Kontrolki.TextBox("field", "sciezka_fk", values[10], Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 30, 1, globalEnabled));
                        controls.Add(new Kontrolki.TextBox("field", "uwagi", values[11], Kontrolki.TextBox.TextBoxMode.KilkaLinii, 420, 6, globalEnabled));

                        break;

                    case Enumeratory.Tabela.TypyLokali:
                        Title = "Typ lokali";
                        numberOfFields = 2;
                        heading += "typu lokalu";
                        columnSwitching = new List<int>() { 0 };
                        labels = new string[]
                        {
                            "Kod: ",
                            "Typ lokalu: "
                        };

                        if (values == null)
                        {
                            if (action != Enumeratory.Akcja.Dodaj)
                                values = db.TypyLokali.FirstOrDefault(t => t.kod_typ == id).WszystkiePola();
                            else
                                values = new string[numberOfFields];
                        }

                        if (!idEnabled)
                            form.Controls.Add(new Kontrolki.HtmlInputHidden("id", values[0]));

                        controls.Add(new Kontrolki.TextBox("field", "id" + fromIdEnabledToIdSuffix[idEnabled], values[0], Kontrolki.TextBox.TextBoxMode.LiczbaCałkowita, 3, 1, idEnabled));
                        controls.Add(new Kontrolki.TextBox("field", "typ_lok", values[1], Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 6, 1, globalEnabled));

                        break;

                    case Enumeratory.Tabela.TypyKuchni:
                        Title = "Rodzaj kuchni";
                        numberOfFields = 2;
                        heading += "rodzaju kuchni";
                        columnSwitching = new List<int>() { 0 };
                        labels = new string[]
                        {
                            "Kod: ",
                            "Rodzaj kuchni: "
                        };

                        if (values == null)
                        {
                            if (action != Enumeratory.Akcja.Dodaj)
                                values = db.TypyKuchni.FirstOrDefault(t => t.kod_kuch == id).WszystkiePola();
                            else
                                values = new string[numberOfFields];
                        }

                        if (!idEnabled)
                            form.Controls.Add(new Kontrolki.HtmlInputHidden("id", values[0]));

                        controls.Add(new Kontrolki.TextBox("field", "id" + fromIdEnabledToIdSuffix[idEnabled], values[0], Kontrolki.TextBox.TextBoxMode.LiczbaCałkowita, 3, 1, idEnabled));
                        controls.Add(new Kontrolki.TextBox("field", "typ_kuch", values[1], Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 15, 1, globalEnabled));

                        break;

                    case Enumeratory.Tabela.RodzajeNajemcy:
                        Title = "Rodzaj najemców";
                        numberOfFields = 2;
                        heading += "rodzaju najemców";
                        columnSwitching = new List<int>() { 0 };
                        labels = new string[]
                        {
                            "Kod: ",
                            "Rodzaj najemcy: "
                        };

                        if (values == null)
                        {
                            if (action != Enumeratory.Akcja.Dodaj)
                                values = db.TypyNajemców.FirstOrDefault(t => t.kod_najem == id).WszystkiePola();
                            else
                                values = new string[numberOfFields];
                        }

                        if (!idEnabled)
                            form.Controls.Add(new Kontrolki.HtmlInputHidden("id", values[0]));

                        controls.Add(new Kontrolki.TextBox("field", "id" + fromIdEnabledToIdSuffix[idEnabled], values[0], Kontrolki.TextBox.TextBoxMode.LiczbaCałkowita, 3, 1, idEnabled));
                        controls.Add(new Kontrolki.TextBox("field", "r_najemcy", values[1], Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 15, 1, globalEnabled));

                        break;

                    case Enumeratory.Tabela.TytulyPrawne:
                        Title = "Tytuł prawny do lokali";
                        numberOfFields = 2;
                        heading += "tytułu prawnego do lokali";
                        columnSwitching = new List<int>() { 0 };
                        labels = new string[]
                        {
                            "Kod: ",
                            "Tytuł prawny: "
                        };

                        if (values == null)
                        {
                            if (action != Enumeratory.Akcja.Dodaj)
                                values = db.TytułyPrawne.FirstOrDefault(t => t.kod_praw == id).WszystkiePola();
                            else
                                values = new string[numberOfFields];
                        }

                        if (!idEnabled)
                            form.Controls.Add(new Kontrolki.HtmlInputHidden("id", values[0]));

                        controls.Add(new Kontrolki.TextBox("field", "id" + fromIdEnabledToIdSuffix[idEnabled], values[0], Kontrolki.TextBox.TextBoxMode.LiczbaCałkowita, 3, 1, idEnabled));
                        controls.Add(new Kontrolki.TextBox("field", "tyt_prawny", values[1], Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 15, 1, globalEnabled));

                        break;

                    case Enumeratory.Tabela.TypyWplat:
                        Title = "Rodzaj wpłaty lub wypłaty";
                        numberOfFields = 8;
                        heading += "rodzaju wpłaty lub wypłaty";
                        columnSwitching = new List<int>() { 0, 4 };
                        labels = new string[]
                        {
                            "Kod: ",
                            "Rodzaj wpłaty lub wypłaty: ",
                            "Rodzaj ewidencji: ",
                            "Sposób rozliczenia: ",
                            "Czy naliczać odsetki? ",
                            "Czy liczyć odsetki na nocie? ",
                            "VAT: ",
                            "SWW: "
                        };

                        if (values == null)
                        {
                            if (action != Enumeratory.Akcja.Dodaj)
                                values = db.RodzajePłatności.FirstOrDefault(t => t.kod_wplat == id).WszystkiePola();
                            else
                                values = new string[numberOfFields];
                        }

                        if (!idEnabled)
                            form.Controls.Add(new Kontrolki.HtmlInputHidden("id", values[0]));

                        controls.Add(new Kontrolki.TextBox("field", "id" + fromIdEnabledToIdSuffix[idEnabled], values[0], Kontrolki.TextBox.TextBoxMode.LiczbaCałkowita, 3, 1, idEnabled));
                        controls.Add(new Kontrolki.TextBox("field", "typ_wplat", values[1], Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 15, 1, globalEnabled));

                        controls.Add(new Kontrolki.DropDownList("field", "rodz_e", new List<string[]>()
                        {
                            new string[] {"0", String.Empty},
                            new string[] {"1", "dziennik komornego"},
                            new string[] {"2", "wpłaty"},
                            new string[] {"3", "zmniejszenia"},
                            new string[] {"4", "zwiększenia"}
                        }, values[2], globalEnabled, false));

                        controls.Add(new Kontrolki.DropDownList("field", "s_rozli", new List<string[]>()
                        {
                            new string[] {"1", "Zmniejszenie"},
                            new string[] {"2", "Zwiększenie"},
                            new string[] {"3", "Zwrot"}
                        }, values[3], globalEnabled, false));

                        controls.Add(new Kontrolki.RadioButtonList("field", "tn_odset", new List<string>() { "Nie", "Tak" }, new List<string>() { "0", "1" }, values[4], globalEnabled, false));
                        controls.Add(new Kontrolki.RadioButtonList("field", "nota_odset", new List<string>() { "Nie", "Tak" }, new List<string>() { "0", "1" }, values[5], globalEnabled, false));
                        controls.Add(new Kontrolki.DropDownList("field", "vat", db.StawkiVat.ToList().Select(r => r.WażnePolaDoRozwijanejListy()).ToList(), values[6], globalEnabled, false));
                        controls.Add(new Kontrolki.TextBox("field", "sww", values[7], Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 10, 1, globalEnabled));

                        break;

                    case Enumeratory.Tabela.GrupySkładnikowCzynszu:
                        Title = "Grupa składników czynszu";
                        numberOfFields = 2;
                        heading += "grupy składników czynszu";
                        columnSwitching = new List<int>() { 0 };
                        labels = new string[]
                        {
                            "Kod: ",
                            "Nazwa grupy składników czynszu: "
                        };

                        if (values == null)
                        {
                            if (action != Enumeratory.Akcja.Dodaj)
                                values = db.GrupySkładnikówCzynszu.FirstOrDefault(g => g.kod == id).WszystkiePola();
                            else
                                values = new string[numberOfFields];
                        }

                        if (!idEnabled)
                            form.Controls.Add(new Kontrolki.HtmlInputHidden("id", values[0]));

                        controls.Add(new Kontrolki.TextBox("field", "id" + fromIdEnabledToIdSuffix[idEnabled], values[0], Kontrolki.TextBox.TextBoxMode.LiczbaCałkowita, 3, 1, idEnabled));
                        controls.Add(new Kontrolki.TextBox("field", "nazwa", values[1], Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 15, 1, globalEnabled));

                        break;

                    case Enumeratory.Tabela.GrupyFinansowe:
                        Title = "Grupa finansowa";
                        numberOfFields = 3;
                        heading += "grupy finansowej";
                        columnSwitching = new List<int>() { 0 };
                        labels = new string[]
                        {
                            "Kod: ",
                            "Konto FK: ",
                            "Nazwa grupy finansowej: "
                        };

                        if (values == null)
                        {
                            if (action != Enumeratory.Akcja.Dodaj)
                                values = db.GrupyFinansowe.FirstOrDefault(g => g.kod == id).WszystkiePola();
                            else
                                values = new string[numberOfFields];
                        }

                        if (!idEnabled)
                            form.Controls.Add(new Kontrolki.HtmlInputHidden("id", values[0]));

                        controls.Add(new Kontrolki.TextBox("field", "id" + fromIdEnabledToIdSuffix[idEnabled], values[0], Kontrolki.TextBox.TextBoxMode.LiczbaCałkowita, 3, 1, idEnabled));
                        controls.Add(new Kontrolki.TextBox("field", "k_syn", values[1], Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 3, 1, globalEnabled));
                        controls.Add(new Kontrolki.TextBox("field", "nazwa", values[2], Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 30, 1, globalEnabled));

                        break;

                    case Enumeratory.Tabela.StawkiVat:
                        Title = "Stawka VAT";
                        numberOfFields = 3;
                        heading += "stawki VAT";
                        columnSwitching = new List<int>() { 0 };
                        labels = new string[]
                        {
                            "Oznaczenie stawki: ",
                            "Symbol fiskalny: "
                        };

                        if (values == null)
                        {
                            if (action != Enumeratory.Akcja.Dodaj)
                                values = db.StawkiVat.FirstOrDefault(r => r.__record == id).WszystkiePola();
                            else
                                values = new string[numberOfFields];
                        }

                        form.Controls.Add(new Kontrolki.HtmlInputHidden("id", values[0]));

                        if (!idEnabled)
                            form.Controls.Add(new Kontrolki.HtmlInputHidden("nazwa", values[1]));

                        controls.Add(new Kontrolki.TextBox("field", "nazwa" + fromIdEnabledToIdSuffix[idEnabled], values[1], Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 2, 1, idEnabled));
                        controls.Add(new Kontrolki.TextBox("field", "symb_fisk", values[2], Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 2, 1, globalEnabled));

                        break;

                    case Enumeratory.Tabela.Atrybuty:
                        Title = "Cecha obiektów";
                        numberOfFields = 10;
                        heading += "cechy obiektów";
                        columnSwitching = new List<int>() { 0 };
                        labels = new string[]
                        {
                            "Kod: ",
                            "Nazwa: ",
                            "Numeryczna/charakter: ",
                            "Jednostka miary: ",
                            "Wartość domyślna: ",
                            "Uwagi: ",
                            "Dotyczy: "
                        };

                        if (values == null)
                        {
                            if (action != Enumeratory.Akcja.Dodaj)
                                values = db.Atrybuty.FirstOrDefault(a => a.kod == id).WszystkiePola();
                            else
                            {
                                values = new string[numberOfFields];
                                values[2] = "N";
                                values[6] = values[7] = values[8] = values[9] = "X";
                            }
                        }

                        if (!idEnabled)
                            form.Controls.Add(new Kontrolki.HtmlInputHidden("id", values[0]));

                        controls.Add(new Kontrolki.TextBox("field", "id" + fromIdEnabledToIdSuffix[idEnabled], values[0], Kontrolki.TextBox.TextBoxMode.LiczbaCałkowita, 3, 1, idEnabled));
                        controls.Add(new Kontrolki.TextBox("field", "nazwa", values[1], Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 20, 1, globalEnabled));
                        controls.Add(new Kontrolki.RadioButtonList("field", "nr_str", new List<string>() { "numeryczna", "charakter" }, new List<string>() { "N", "C" }, values[2], globalEnabled, false));
                        controls.Add(new Kontrolki.TextBox("field", "jedn", values[3], Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 6, 1, globalEnabled));
                        controls.Add(new Kontrolki.TextBox("field", "wartosc", values[4], Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 25, 1, globalEnabled));
                        controls.Add(new Kontrolki.TextBox("field", "uwagi", values[5], Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 30, 1, globalEnabled));

                        List<string> selectedValues = new List<string>();

                        if (values[6] == "X")
                            selectedValues.Add("l");

                        if (values[7] == "X")
                            selectedValues.Add("n");

                        if (values[8] == "X")
                            selectedValues.Add("b");

                        if (values[9] == "X")
                            selectedValues.Add("s");

                        controls.Add(new Kontrolki.CheckBoxList("field", "zb", new List<string>() { "lokale", "najemcy", "budynki", "wspólnoty" }, new List<string>() { "l", "n", "b", "s" }, selectedValues, globalEnabled));

                        break;

                    case Enumeratory.Tabela.Uzytkownicy:
                        Title = "Użytkownik";
                        numberOfFields = 6;
                        heading += "użytkownika";
                        columnSwitching = new List<int>() { 0 };
                        labels = new string[]
                        {
                            "Symbol: ",
                            "Nazwisko: ",
                            "Imię: ",
                            "Użytkownik: ",
                            "Hasło: ",
                            "Potwierdź hasło: "
                        };

                        if (values == null)
                        {
                            if (action != Enumeratory.Akcja.Dodaj)
                            {
                                values = db.Użytkownicy.FirstOrDefault(u => u.__record == id).WszystkiePola();

                                values[5] = String.Empty;
                            }
                            else
                                values = new string[numberOfFields];
                        }

                        form.Controls.Add(new Kontrolki.HtmlInputHidden("id", values[0]));
                        controls.Add(new Kontrolki.TextBox("field", "symbol", values[1], Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 2, 1, globalEnabled));

                        if (!idEnabled)
                        {
                            form.Controls.Add(new Kontrolki.HtmlInputHidden("nazwisko", values[2]));
                            form.Controls.Add(new Kontrolki.HtmlInputHidden("imie", values[3]));
                        }

                        controls.Add(new Kontrolki.TextBox("field", "nazwisko" + fromIdEnabledToIdSuffix[idEnabled], values[2], Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 25, 1, idEnabled));
                        controls.Add(new Kontrolki.TextBox("field", "imie" + fromIdEnabledToIdSuffix[idEnabled], values[3], Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 15, 1, idEnabled));
                        controls.Add(new Kontrolki.TextBox("field", "uzytkownik", values[4], Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 40, 1, false));
                        controls.Add(new Kontrolki.TextBox("field", "haslo", values[5], Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 8, 1, globalEnabled));
                        controls.Add(new Kontrolki.TextBox("field", "haslo2", String.Empty, Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 8, 1, globalEnabled));

                        break;

                    case Enumeratory.Tabela.ObrotyNajemcy:
                        Title = "Obrót najemcy";
                        numberOfFields = 9;
                        heading += "obrotu najemcy";
                        columnSwitching = new List<int>() { 0, 3, 4 };
                        labels = new string[]
                        {
                            "Kwota: ",
                            "Data: ",
                            "Data NO: ",
                            "Rodzaj obrotu: ",
                            "Nr dowodu: ",
                            "Pozycja",
                            "Uwagi"
                        };

                        if (values == null)
                        {
                            IEnumerable<DostępDoBazy.Obrót> turnOvers = null;

                            switch (Start.AktywnyZbiór)
                            {
                                case Enumeratory.Zbiór.Czynsze:
                                    turnOvers = db.ObrotyZPierwszegoZbioru.ToList().Cast<DostępDoBazy.Obrót>();

                                    break;

                                case Enumeratory.Zbiór.Drugi:
                                    turnOvers = db.ObrotyZDrugiegoZbioru.ToList().Cast<DostępDoBazy.Obrót>();

                                    break;

                                case Enumeratory.Zbiór.Trzeci:
                                    turnOvers = db.ObrotyZTrzeciegoZbioru.ToList().Cast<DostępDoBazy.Obrót>();

                                    break;
                            }

                            if (action != Enumeratory.Akcja.Dodaj)
                                values = turnOvers.FirstOrDefault(t => t.__record == id).WszystkiePola();
                            else
                            {
                                values = new string[numberOfFields];
                                values[8] = PobierzWartośćParametru<string>("additionalId");

                                if (turnOvers.Any())
                                    values[0] = (turnOvers.Max(t => t.__record) + 1).ToString();
                                else
                                    values[0] = "1";

                            }
                        }

                        form.Controls.Add(new Kontrolki.HtmlInputHidden("id", values[0]));
                        controls.Add(new Kontrolki.TextBox("field", "suma", values[1], Kontrolki.TextBox.TextBoxMode.LiczbaNiecałkowita, 14, 1, globalEnabled));
                        controls.Add(new Kontrolki.TextBox("field", "data_obr", values[2], Kontrolki.TextBox.TextBoxMode.Data, 10, 1, globalEnabled));
                        controls.Add(new Kontrolki.TextBox("field", "?", values[3], Kontrolki.TextBox.TextBoxMode.Data, 10, 1, globalEnabled));

                        List<DostępDoBazy.RodzajPłatności> typesOfPayment = db.RodzajePłatności.ToList();

                        //controls.Add(new Kontrolki.RadioButtonList("field", "kod_wplat", typesOfPayment.Select(t => t.typ_wplat).ToList(), typesOfPayment.Select(t => t.kod_wplat.ToString()).ToList(), values[4], globalEnabled, false));
                        controls.Add(new Kontrolki.DropDownList("field", "kod_wplat", typesOfPayment.Select(t => t.ImportantFieldsForDropdown()).ToList(), values[4], globalEnabled, false));
                        controls.Add(new Kontrolki.TextBox("field", "nr_dowodu", values[5], Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 11, 1, globalEnabled));
                        controls.Add(new Kontrolki.TextBox("field", "pozycja_d", values[6], Kontrolki.TextBox.TextBoxMode.LiczbaCałkowita, 2, 1, globalEnabled));
                        controls.Add(new Kontrolki.TextBox("field", "uwagi", values[7], Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 40, 1, globalEnabled));
                        form.Controls.Add(new Kontrolki.HtmlInputHidden("nr_kontr", values[8]));

                        break;
                }

            placeOfHeading.Controls.Add(new LiteralControl("<h2>" + heading + "</h2>"));
            form.Controls.Add(new Kontrolki.HtmlInputHidden("action", action.ToString()));
            form.Controls.Add(new Kontrolki.HtmlInputHidden("table", table.ToString()));

            Control cell = null;
            int columnIndex = -1;

            for (int i = 0; i < controls.Count; i++)
            {
                if (columnSwitching.Contains(i))
                {
                    columnIndex++;
                    cell = formRow.FindControl("column" + columnIndex.ToString());
                }

                cell.Controls.Add(new LiteralControl("<div class='fieldWithLabel'>"));
                cell.Controls.Add(new Kontrolki.Label("fieldLabel", controls[i].ID, labels[i], String.Empty));
                DodajNowąLinię(cell);
                cell.Controls.Add(controls[i]);
                cell.Controls.Add(new LiteralControl("</div>"));
            }

            if (preview != null)
            {
                placeOfPreview.Controls.Add(new LiteralControl("<h3>"));

                for (int i = 0; i < preview.Count; i += 2)
                {
                    placeOfPreview.Controls.Add(preview[i]);
                    placeOfPreview.Controls.Add(preview[i + 1]);
                    DodajNowąLinię(placeOfPreview);
                }

                placeOfPreview.Controls.Add(new LiteralControl("</h3>"));
            }

            if (tabButtons != null)
            {
                for (int i = 0; i < tabButtons.Count; i++)
                {
                    placeOfTabButtons.Controls.Add(tabButtons[i]);
                    placeOfTabButtons.Controls.Add(labelsOfTabButtons[i]);
                }

                foreach (Kontrolki.HtmlIframe tab in tabs)
                    placeOfTabs.Controls.Add(tab);
            }

            foreach (Kontrolki.Button button in buttons)
                placeOfButtons.Controls.Add(button);

            if (Start.ŚcieżkaStrony.Count > 0)
                if (!Start.ŚcieżkaStrony.Contains(heading))
                {
                    Start.ŚcieżkaStrony[Start.ŚcieżkaStrony.Count - 1] = String.Concat("<a href=\"" + backUrl + "\">", Start.ŚcieżkaStrony[Start.ŚcieżkaStrony.Count - 1]) + "</a>";

                    Start.ŚcieżkaStrony.Add(heading);
                }
        }
    }
}