﻿using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace BulkyBookWeb.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> productList = _unitOfWork.Product.GetAll(includeProperties: "Category,CoverType");

            return View(productList);
        }
        
        public IActionResult Details(int productId)
        {
            var product = _unitOfWork.Product.GetFirstOrDefault(p => p.Id == productId, includeProperties: "Category,CoverType");

            var shoppingCart = new ShoppingCart()
            {
                Product = product,
                ProductId = productId,
                Count = 1
            };

            return View(shoppingCart);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Details(ShoppingCart shoppingCart)
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;

            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            shoppingCart.ApplicationUserId = claim.Value;

            var cartFromDb = _unitOfWork.ShoppingCart.GetFirstOrDefault(s => s.ApplicationUserId == claim.Value && s.ProductId == shoppingCart.ProductId);

            if(cartFromDb is null)
            {
                _unitOfWork.ShoppingCart.Add(shoppingCart);
            }
            else
            {
                _unitOfWork.ShoppingCart.IncrementCount(cartFromDb, shoppingCart.Count);
            }
            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}