using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HKQTravellingAuthenication.Models.Tour
{
    public class Discounts
    {
        [Key]
        [Column("DIS_ID")]
        public long DiscountId { get; set; }

        [Column("DIS_PER")]
        public double? DiscountPercentage { get; set; }

        [Column("DIS_NAME")]
        [MaxLength(1000)]
        public string DiscountName { get; set; }

        [Column("DIS_DATE_START")]
        public DateTime? DiscountDateStart { get; set; }

        [Column("DIS_DATE_END")]
        public DateTime? DiscountDateEnd { get; set; }

        [Column("STATUS")]
        public int? Status { get; set; }
    }
}
