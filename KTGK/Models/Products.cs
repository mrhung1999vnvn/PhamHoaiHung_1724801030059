namespace KTGK.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Products
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [StringLength(50)]
        public string Code { get; set; }

        [StringLength(200)]
        public string Name { get; set; }

        [StringLength(200)]
        public string ShortName { get; set; }

        [StringLength(50)]
        public string Note { get; set; }

        public int? CategoryId { get; set; }

        public virtual Categorys Categorys { get; set; }
    }
}
