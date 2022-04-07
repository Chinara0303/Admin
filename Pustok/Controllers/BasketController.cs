using Microsoft.AspNetCore.Mvc;
using Pustok.DAL;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Pustok.ViewModels.Basket;
using Newtonsoft.Json;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Pustok.Controllers
{
    public class BasketController : Controller
    {
        private readonly AppDbContext _context;
        public BasketController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            string basket = HttpContext.Request.Cookies["basket"];

            List<BasketVM> basketVMs = null;

            if (basket != null)
            {
                basketVMs = JsonConvert.DeserializeObject<List<BasketVM>>(basket);
            }
            else
            {
                basketVMs = new List<BasketVM>();
            }

            foreach (BasketVM basketVM in basketVMs)
            {
                basketVM.Title = _context.Products.FirstOrDefault(p => p.Id == basketVM.Id).Title;
                basketVM.MainImage = _context.Products.FirstOrDefault(p => p.Id == basketVM.Id).MainImage;
                basketVM.Price = _context.Products.FirstOrDefault(p => p.Id == basketVM.Id).Price;
            }


            return View(basketVMs);
        }
    }
}
