using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HKQTravellingAuthenication.Models.Tour
{
    public class EndLocations
    {
        [Key]
        [Column("END_LOCATION_ID")]
        public long EndLocationId { get; set; }

        [Required]
        [Column("END_LOCATION_NAME")]
        [MaxLength(100)]
        public string EndLocationName { get; set; }
    }
}
