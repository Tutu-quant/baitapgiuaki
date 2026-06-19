using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lesson3_CNLTWeb.Models
{
    public class RoomBCS240032
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Không được để trống")]
        [Display(Name = "Tên phòng")]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Loại phòng là bắt buộc")]
        [Range(1, int.MaxValue, ErrorMessage = "Vui lòng chọn loại phòng")]
        [Display(Name = "Loại phòng")]
        public int RoomTypeId { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Giá phải lớn hơn 0")]
        [Display(Name = "Giá thuê (VNĐ)")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Diện tích phải lớn hơn 0")]
        [Display(Name = "Diện tích (m²)")]
        public double Area { get; set; }

        [Display(Name = "Còn phòng")]
        public bool IsAvailable { get; set; } = true;

        [Display(Name = "Mô tả")]
        [StringLength(500)]
        public string? Description { get; set; }

        [NotMapped]
        [Display(Name = "Giá/m²")]
        public decimal PricePerSquareMeter => Area > 0 ? Price / (decimal)Area : 0;

        [ForeignKey("RoomTypeId")]
        public virtual RoomTypeBCS240032? RoomType { get; set; }

        public virtual ICollection<RoomImageBCS240032> RoomImages { get; set; } = new List<RoomImageBCS240032>();
    }
}
