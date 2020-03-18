﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Api.Dtos
{
    public class NamesListsDto
    {
        public int id { get; set; }
        public string businessName { get; set; }
        public string name { get; set; }
        public string surname { get; set; }

        [Required]
        public string dug { get; set; }

        [Required]
        public string address { get; set; }

        public string houseNumber { get; set; }

        [Required]
        public string cap { get; set; }

        [Required]
        public string city { get; set; }

        [Required]
        public string province { get; set; }

        [Required]
        public string state { get; set; }

        [Required]
        public int listId { get; set; }

        public string complementNames { get; set; }
        public string complementAddress { get; set; }

        public string fileName { get; set; }
        public string fiscalCode { get; set; }
        public string mobile { get; set; }

        public NamesListsDto()
        {
            id = 0;
            businessName = "";
            name = "";
            surname = "";
            dug = "";
            address = "";
            houseNumber = "";
            cap = "";
            city = "";
            province = "";
            state = "";
            listId = 0;
            complementNames = "";
            complementAddress = "";
            fileName = "";
            fiscalCode = "";
            mobile = "";
        }
    }
}