﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace czynsze.Forms
{
    public partial class CommunityBuildings : Page
    {
        List<DostępDoBazy.BudynekWspólnoty> communityBuildings
        {
            get { return (List<DostępDoBazy.BudynekWspólnoty>)Session["communityBuildings"]; }
            set { Session["communityBuildings"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            int kod = GetParamValue<int>("kod");
            List<string[]> rows = new List<string[]>();
            string window = GetParamValue<string>("ShowWindow");
            Enums.Akcja parentAction = GetParamValue<Enums.Akcja>("parentAction");
            Enums.Akcja childAction = GetParamValue<Enums.Akcja>("ChildAction");
            int id = GetParamValue<int>("id");
            string postBackUrl = "CommunityBuildings.aspx";
            DostępDoBazy.BudynekWspólnoty currentCommunityBuilding = null;

            if (id != 0)
                currentCommunityBuilding = communityBuildings.ElementAt(id - 1);

            if ((int)childAction != 0)
            {
                string[] record = new string[]
                {
                    kod.ToString(),
                    GetParamValue<string>("kod_1"),
                    GetParamValue<string>("uwagi")
                };

                switch (childAction)
                {
                    case Enums.Akcja.Dodaj:
                        DostępDoBazy.BudynekWspólnoty communityBuilding = new DostępDoBazy.BudynekWspólnoty();

                        communityBuilding.Ustaw(record);
                        communityBuildings.Add(communityBuilding);

                        break;

                    case Enums.Akcja.Edytuj:
                        currentCommunityBuilding.Ustaw(record);

                        break;

                    case Enums.Akcja.Usuń:
                        communityBuildings.Remove(currentCommunityBuilding);

                        break;
                }
            }

            for (int i = 0; i < communityBuildings.Count; i++)
            {
                string index = (i + 1).ToString();

                rows.Add(new string[] { index, index }.Concat(communityBuildings.ElementAt(i).WażnePola()).ToArray());
            }

            ViewState["id"] = id;

            form.Controls.Add(new MyControls.HtmlInputHidden("parentAction", parentAction.ToString()));
            form.Controls.Add(new MyControls.HtmlInputHidden("kod", kod.ToString()));

            if (window == null)
                switch (parentAction)
                {
                    case Enums.Akcja.Dodaj:
                    case Enums.Akcja.Edytuj:
                        placeOfButtons.Controls.Add(new MyControls.Button("button", "addShowWindow", "Dodaj", postBackUrl));
                        placeOfButtons.Controls.Add(new MyControls.Button("button", "removeChildAction", "Usuń", postBackUrl));
                        placeOfButtons.Controls.Add(new MyControls.Button("button", "editShowWindow", "Edytuj", postBackUrl));

                        break;
                }
            else
            {
                string firstLabel;
                Control firstControl;
                string comments;
                string textOfSaveButton;

                if (window == "Dodaj")
                {
                    firstLabel = "Nowy budynek: ";
                    textOfSaveButton = "Dodaj";
                    comments = String.Empty;

                    using (DostępDoBazy.CzynszeKontekst db = new DostępDoBazy.CzynszeKontekst())
                        firstControl = new MyControls.DropDownList("field", "kod_1", db.Budynki.OrderBy(b => b.kod_1).ToList().Select(b => b.WażnePola()).ToList(), String.Empty, true, false);
                }
                else
                {
                    firstLabel = "Budynek: ";
                    textOfSaveButton = "Edytuj";
                    comments = currentCommunityBuilding.uwagi.Trim();

                    using (DostępDoBazy.CzynszeKontekst db = new DostępDoBazy.CzynszeKontekst())
                        firstControl = new MyControls.TextBox("field", "budynek", db.Budynki.FirstOrDefault(b => b.kod_1 == currentCommunityBuilding.kod_1).kod_1.ToString(), MyControls.TextBox.TextBoxMode.SingleLine, 30, 1, false);

                    form.Controls.Add(new MyControls.HtmlInputHidden("kod_1", currentCommunityBuilding.kod_1.ToString()));
                    form.Controls.Add(new MyControls.HtmlInputHidden("id", id.ToString()));
                }

                placeOfNewBuilding.Controls.Add(new MyControls.Label("label", "kod_1", firstLabel, String.Empty));

                //using (DostępDoBazy.Czynsze_Entities db = new DostępDoBazy.Czynsze_Entities())
                placeOfNewBuilding.Controls.Add(firstControl);
                placeOfComments.Controls.Add(new MyControls.Label("field", "uwagi", "Uwagi: ", String.Empty));
                placeOfComments.Controls.Add(new MyControls.TextBox("field", "uwagi", comments, MyControls.TextBox.TextBoxMode.SingleLine, 30, 1, true));
                placeOfButtonsOfWindow.Controls.Add(new MyControls.Button("button", "saveChildAction", textOfSaveButton, postBackUrl));
                placeOfButtonsOfWindow.Controls.Add(new MyControls.Button("button", String.Empty, "Anuluj", postBackUrl));
            }

            placeOfTable.Controls.Add(new MyControls.Table("mainTable tabTable", rows, new string[] { "Lp.", "Nr budynku", "Adres", "Uwagi" }, false, String.Empty, new List<int>() { 1, 2 }, new List<int>()));
        }
    }
}