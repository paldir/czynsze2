﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace czynsze.ControlsP
{
    public class HtmlGenericControlP : System.Web.UI.HtmlControls.HtmlGenericControl
    {
        public HtmlGenericControlP(string tagName, string className)
        {
            this.TagName = tagName;

            this.Attributes.Add("class", className);
        }
    }
}