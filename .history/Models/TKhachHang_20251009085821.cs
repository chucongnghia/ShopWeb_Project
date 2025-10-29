using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ShopWeb_Project.Models;

public partial class TKhachHang
{
    public string MaKhanhHang { get; set; } = null!;

    public string? Username { get; set; }

    [Required(ErrorMessage = "Tên khách hàng không được để trống")]
    [StringLength(100, ErrorMessage = "Tên khách hàng không được vượt quá 100 ký tự")]
    public string? TenKhachHang { get; set; }

    public DateOnly? NgaySinh { get; set; }

    [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
    [StringLength(15, ErrorMessage = "Số điện thoại không được vượt quá 15 ký tự")]
    public string? SoDienThoai { get; set; }

    [StringLength(200, ErrorMessage = "Địa chỉ không được vượt quá 200 ký tự")]
    public string? DiaChi { get; set; }

    public byte? LoaiKhachHang { get; set; }

    public string? AnhDaiDien { get; set; }

    [StringLength(500, ErrorMessage = "Ghi chú không được vượt quá 500 ký tự")]
    public string? GhiChu { get; set; }

    public virtual ICollection<THoaDonBan> THoaDonBans { get; set; } = new List<THoaDonBan>();

    public virtual TUser? UsernameNavigation { get; set; }
}
