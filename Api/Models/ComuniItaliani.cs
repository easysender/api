﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Api.Models
{
    public class ComuniItaliani
    {
        public string nome { get; set; }
        public string codice { get; set; }
        public object zona { get; set; }
        public object regione { get; set; }
        public object cm { get; set; }
        public object provincia { get; set; }
        public string sigla { get; set; }
        public string codiceCatastale { get; set; }
        public object cap { get; set; }
        public string popolazione { get; set; }
    }
}