using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lesson3_CNLTWeb.Models
{
    public class RoomImageBCS240032
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "URL ảnh là bắt buộc")]
        [Display(Name = "Đường dẫn ảnh")]
        [StringLength(500)]
        public string ImageUrl { get; set; } = string.Empty;

        [Display(Name = "Ảnh đại diện")]
        public bool IsThumbnail { get; set; }

        public int RoomId { get; set; }

        [ForeignKey("RoomId")]
        public virtual RoomBCS240032? Room { get; set; }
    }
}
