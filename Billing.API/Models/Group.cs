﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Billing.API.Models
{
    public class Group
    {
        public List<Product> Products { get; set; }
        public int Total { get; set; }
    }
}
