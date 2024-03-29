﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MyAspNetProject.Web.Filters;
using MyAspNetProject.Web.Models;
using MyAspNetProject.Web.ViewModels;
using System.Diagnostics;

namespace MyAspNetProject.Web.Controllers
{
    [LogFilter]
    [Route("[controller]/[action]")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public HomeController(ILogger<HomeController> logger, AppDbContext context, IMapper mapper)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
        }
        [Route("/")] // slash koymazsan sınıfın attribute'u bunları ezer ve bunlar çalışmaz.
        [Route("/Home")]
        [Route("/Home/Index")]
        public IActionResult Index()
        {
            var products = _context.Products.OrderBy(x => x.Id).Select(x => new ProductPartialViewModel()
            {
                Id=x.Id,
                Name=x.Name,
                Price=x.Price,
                Stock=x.Stock
            }).ToList();

            ViewBag.productListPartialViewModel = new ProductListPartialViewModel()
            {
                Products = products
            };
            return View();
        }
        [CustomExceptionFilter]
        public IActionResult Privacy()
        {
            //throw new Exception("Veritabanı bağlantı hatası.");
            var products = _context.Products.OrderBy(x => x.Id).Select(x => new ProductPartialViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                Price = x.Price,
                Stock = x.Stock
            }).ToList();

            ViewBag.productListPartialViewModel = new ProductListPartialViewModel()
            {
                Products = products
            };
            return View();
        }

        public IActionResult Visitor()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SaveVisitorComment(VisitorViewModel visitorViewModel)
        {
            try
            {

                var visitor = _mapper.Map<Visitor>(visitorViewModel);
                _context.Visitors.Add(visitor);
                _context.SaveChanges();
                TempData["result"] = "Yorum Kaydedilmiştir.";
                return RedirectToAction(nameof(HomeController.Visitor));
            }
            catch (Exception)
            {
                TempData["result"] = "Yorum Kaydedilirken Bir Hata Meydana Geldi.";
                return RedirectToAction(nameof(HomeController.Visitor));
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(ErrorViewModel errorViewModel)
        {
            errorViewModel.RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;//RequestId'ye unique bir değer atadı.
            return View(errorViewModel);
        }
    }
}