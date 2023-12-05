using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HKQTravellingAuthenication.Areas.Tour.Models
{
    public class DiscountAdministratorModelView
    {
        [Display(Name = "Mã giảm giá")]
        public long DiscountId { get; set; }

        [Display(Name = "Giá trị giảm giá")]
        public double? DiscountPercentage { get; set; }

        [Display(Name = "Tên giảm giá")]
        public string DiscountName { get; set; }

        [Display(Name = "Ngày bắt đầu")]
        public DateTime? DiscountDateStart { get; set; }

        [Display(Name = "Ngày kết thúc")]
        public DateTime? DiscountDateEnd { get; set; }

        [Display(Name = "Trạng thái")]
        public int? Status { get; set; }
    }
}
