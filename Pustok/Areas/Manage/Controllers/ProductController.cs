﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pustok.DAL;
using Pustok.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pustok.Areas.Manage.Controllers
{
    [Area("manage")]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        public ProductController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<Product> products = await _context.Products.Where(p=>!p.IsDeleted).OrderByDescending(p=>p.Id).Take(5).ToListAsync();

            return View(products);
        }
        public async Task<IActionResult> Create()
        {
            ViewBag.Authors = await _context.Authors.ToListAsync();
            ViewBag.Genres = await _context.Genres.ToListAsync();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            ViewBag.Authors = await _context.Authors.ToListAsync();
            ViewBag.Genres = await _context.Genres.ToListAsync();


            if (!ModelState.IsValid)
            {
                return View();
            }

            if (product.AuthorId != null && !await _context.Authors.AnyAsync(a => a.Id == product.AuthorId))
            {
                ModelState.AddModelError("AuthorId", "Duzgun daxil edin");
                return View();
            }
            if (product.GenreId != null && !await _context.Genres.AnyAsync(a => a.Id == product.GenreId))
            {
                ModelState.AddModelError("GenreId", "Duzgun daxil edin");
                return View();
            }

            if (product.MainImgFile == null)
            {
                ModelState.AddModelError("MainImgFile", "Sekil mutleq secilmelidir");
                return View();
            }

            if (product.HoverImgFile == null)
            {
                ModelState.AddModelError("HoverImgFile", "Sekil mutleq secilmelidir");
                return View();
            }
            if (product.MainImgFile.ContentType != "image/jpeg")
            {
                ModelState.AddModelError("MainImgFile", "Sekil novu yalniz jpeg ve ya hpg olmalidir");
                return View();
            }
            if (product.HoverImgFile.ContentType != "image/jpeg")
            {
                ModelState.AddModelError("HoverImgFile", "Sekil novu yalniz jpeg ve ya hpg olmalidir");
                return View();
            }


            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return BadRequest();
            Product product = await _context.Products
                .Include(p => p.Author)
                .Include(p => p.Genre)
                .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);
            if (product == null) return NotFound();
            return View(product);
        }

        public async Task<IActionResult> Update(int? id)
        {
            
            if (id == null) return BadRequest();

            Product product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);

            if (product == null) return NotFound();

            ViewBag.Authors = await _context.Authors.ToListAsync();
            ViewBag.Genres = await _context.Genres.ToListAsync();

            
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, Product product)
        {
            if (ModelState.IsValid)
            {
                return View();
            }

            if (id == null) return BadRequest();
            if (id != product.Id) return BadRequest();

            Product dbproduct = await _context.Products.FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);
            if (dbproduct == null) return BadRequest();


            ViewBag.Authors = await _context.Authors.ToListAsync();
            ViewBag.Genres = await _context.Genres.ToListAsync();

            if (product.AuthorId != null && !await _context.Authors.AnyAsync(a => a.Id == product.AuthorId))
            {
                ModelState.AddModelError("AuthorId", "Duzgun daxil edin");
                return View(product);
            }
            if (product.GenreId != null && !await _context.Genres.AnyAsync(a => a.Id == product.GenreId))
            {
                ModelState.AddModelError("GenreId", "Duzgun daxil edin");
                return View(product);
            }


            dbproduct.Title = product.Title;
            dbproduct.Author = product.Author;
            dbproduct.Genre = product.Genre;
            dbproduct.Price = product.Price;
            dbproduct.DiscountPrice = product.DiscountPrice;
            dbproduct.AuthorId = product.AuthorId;
            dbproduct.GenreId = product.GenreId;
            dbproduct.MainImage = product.MainImage;
            dbproduct.HoverImage = product.HoverImage;
            dbproduct.IsArrival = product.IsArrival;
            dbproduct.IsFeature = product.IsFeature;
            dbproduct.IsMostView = product.IsMostView;


            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();
            Product product = await _context.Products
                .Include(p => p.Author)
                .Include(p => p.Genre)
                .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);
            if (product == null) return NotFound();
            return View(product);
        }

        public async Task<IActionResult> DeleteProduct(int? id)
        {
            if (id == null) return BadRequest();
            Product product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);
            if (product == null) return NotFound();

            //_context.Products.Remove(product);Databaseden de silmek ucun istifade edirik!

            product.IsDeleted = true;

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }



    }
}
