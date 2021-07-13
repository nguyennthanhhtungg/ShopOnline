﻿using System;
using System.Collections.Generic;

#nullable disable

namespace ShopOnlineAPI.Models
{
    public partial class Category
    {
        public Category()
        {
            Products = new HashSet<Product>();
        }

        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string ImageUrl { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
