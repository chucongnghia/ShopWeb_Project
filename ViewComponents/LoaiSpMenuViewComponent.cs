using ShopWeb_Project.Models;
using Microsoft.AspNetCore.Mvc;
using ShopWeb_Project.Repository;
namespace ShopWeb_Project.ViewComponents
{
    public class LoaiSpMenuViewComponent: ViewComponent
    {
        private readonly IloaiSpRepository _loaiSpRepository;
        public LoaiSpMenuViewComponent(IloaiSpRepository loaiSpRepository)
        {
            _loaiSpRepository=loaiSpRepository;
        }
        public IViewComponentResult Invoke()
        {
            var loaisp= _loaiSpRepository.GetAllLoaiSp().OrderBy(x=>x.Loai);
            return View(loaisp);
        }
    }
}
