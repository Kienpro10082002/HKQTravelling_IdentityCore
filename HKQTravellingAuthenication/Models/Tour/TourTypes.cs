using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HKQTravellingAuthenication.Models.Tour
{
    public class TourTypes
    {
        [Key]
        [Column("TYPE_ID")]
        public long typeID { get; set; }

        [Column("TYPE_NAME")]
        public string typeName { get; set; }

        [Column("TOUR_ID")]
        public long? TourId { get; set; }

        [ForeignKey("TourId")]
        public Tours tours { get; set; }
    }
}
