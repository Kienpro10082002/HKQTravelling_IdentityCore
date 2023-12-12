using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HKQTravellingAuthenication.Models.Tour
{
    public class Tours
    {
        [Key]
        [Column("TOUR_ID")]
        public long TourId { get; set; }

        [Column("TOUR_NAME")]
        [MaxLength(200)]
        public string TourName { get; set; }

        [Column("DESCRIPTION")]
        public string? Description {  get; set; }

        [Column("PRICE")]
        public int? Price { get; set; }

        [Column("START_DATE")]
        public DateTime? StartDate { get; set; }

        [Column("END_DATE")]
        public DateTime? EndDate { get; set; }

        [Column("STATUS")]
        public int? Status { get; set; }

        [Column("CREATION_DATE")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? CreationDate { get; set; }

        [Column("UPDATE_DATE")]
        public DateTime? UpdateDate { get; set; }

        //Rules

        [Column("REMAINING")]
        public int? Remaining { get; set; }

        [Column("PRICE_INCLUDE")]
        [MaxLength(1000)]
        public string? PriceInclude { get; set; }

        [Column("PRICE_NOT_INCLUDE")]
        [MaxLength(1000)]
        public string? PriceNotInclude { get; set; }

        [Column("SURCHARGE")]
        [MaxLength(1000)]
        public string? Surcharge { get; set; }

        [Column("CANCLE_CHANGE")]
        [MaxLength(1000)]
        public string? CancelChange { get; set; }

        [Column("NOTE")]
        [MaxLength(1000)]
        public string? Note { get; set; }

        //Khóa ngoại
        [Column("DIS_ID")]
        public long? DiscountId { get; set; }

        [ForeignKey("DiscountId")]
        public Discounts discounts { get; set; }

        [Column("START_LOCATION_ID")]
        public long? StartLocationId { get; set; }

        [ForeignKey("StartLocationId")]
        public StartLocations startLocations { get; set; }

        [Column("END_LOCATION_ID")]
        public long? EndLocationId { get; set; }

        [ForeignKey("EndLocationId")]
        public EndLocations endLocations { get; set; }

        [Column("TOUR_TYPE_ID")]
        public long? TourTypeId { get; set; }

        [ForeignKey("TourTypeId")]
        public TourTypes tourTypes{ get; set; }
    }
}
