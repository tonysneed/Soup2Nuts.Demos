namespace PocoDemo.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Product
    {
        public int ProductId { get; set; }

        [Required]
        [StringLength(40)]
        public string ProductName { get; set; }

        public int? CategoryId { get; set; }

        public decimal? UnitPrice { get; set; }

        public bool Discontinued { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        public Category Category { get; set; }
    }
}
