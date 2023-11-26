using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HKQTravellingAuthenication.Models.Tour
{
    public class StartLocations
    {
        [Key]
        [Column("START_LOCATION_ID")]
        public long StartLocationId { get; set; }

        [Required]
        [Column("START_LOCATION_NAME")]
        [MaxLength(100)]
        public string StartLocationName { get; set; }
    }
}
