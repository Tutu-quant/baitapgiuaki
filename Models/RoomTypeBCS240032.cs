using System.ComponentModel.DataAnnotations;

namespace Lesson3_CNLTWeb.Models
{
    public class RoomTypeBCS240032
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Không được để trống")]
        [Display(Name = "Tên loại phòng")]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Mô tả")]
        [StringLength(500)]
        public string? Description { get; set; }

        public virtual ICollection<RoomBCS240032> Rooms { get; set; } = new List<RoomBCS240032>();
    }
}
