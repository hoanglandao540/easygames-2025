<<<<<<< HEAD
<<<<<<< HEAD
﻿using System.ComponentModel.DataAnnotations.Schema;
=======
﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
>>>>>>> feature/akshata/data-shops
=======
﻿using System.ComponentModel.DataAnnotations.Schema;
>>>>>>> origin/feature/hoang/pos-tier-email

namespace EasyGames.Web.Models
{
    public class Order
    {
<<<<<<< HEAD
<<<<<<< HEAD
        public int Id { get; set; }

        // where the sale happened
        public int ShopId { get; set; }
        public ShopLocation? Shop { get; set; }  // ← CHANGED
=======
        public int Id { get; set; }                 // PK

        // where the sale happened
        public int ShopId { get; set; }
        public Shop? Shop { get; set; }
>>>>>>> feature/akshata/data-shops
=======
        public int Id { get; set; }

        // where the sale happened
        public int ShopId { get; set; }
        public ShopLocation? Shop { get; set; }  // ← CHANGED
>>>>>>> origin/feature/hoang/pos-tier-email

        // who bought (nullable for guest sale)
        public int? CustomerId { get; set; }
        public Customer? Customer { get; set; }

        // captured at POS (works for guest too)
        public string? CustomerPhone { get; set; }

        // canonical financials & time
<<<<<<< HEAD
<<<<<<< HEAD
        public decimal Total { get; set; }
=======
        public decimal Total { get; set; }         
>>>>>>> feature/akshata/data-shops
=======
        public decimal Total { get; set; }
>>>>>>> origin/feature/hoang/pos-tier-email
        public DateTime CreatedUtc { get; set; }

        public List<OrderLine> Lines { get; set; } = new();

<<<<<<< HEAD
<<<<<<< HEAD
        // These keep older code compiling (don't touch DB schema).
=======
        // These keep older code compiling (don’t touch DB schema).
>>>>>>> feature/akshata/data-shops
=======
        // These keep older code compiling (don't touch DB schema).
>>>>>>> origin/feature/hoang/pos-tier-email
        [NotMapped] public string? CustomerName { get; set; }
        [NotMapped] public string? CustomerEmail { get; set; }

        [NotMapped]
<<<<<<< HEAD
<<<<<<< HEAD
        public DateTime CreatedAt
=======
        public DateTime CreatedAt                
>>>>>>> feature/akshata/data-shops
=======
        public DateTime CreatedAt
>>>>>>> origin/feature/hoang/pos-tier-email
        {
            get => CreatedUtc;
            set => CreatedUtc = value;
        }

        [NotMapped]
<<<<<<< HEAD
<<<<<<< HEAD
        public decimal GrandTotal
=======
        public decimal GrandTotal               
>>>>>>> feature/akshata/data-shops
=======
        public decimal GrandTotal
>>>>>>> origin/feature/hoang/pos-tier-email
        {
            get => Total;
            set => Total = value;
        }
    }
<<<<<<< HEAD
<<<<<<< HEAD
}
=======
}


>>>>>>> feature/akshata/data-shops
=======
}
>>>>>>> origin/feature/hoang/pos-tier-email
