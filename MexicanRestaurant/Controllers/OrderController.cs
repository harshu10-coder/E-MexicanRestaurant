﻿using MexicanRestaurant.Data;
using MexicanRestaurant.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace MexicanRestaurant.Controllers
{
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;
        private Repository<Product> _products;
        private Repository<Order> _orders;
        public readonly UserManager <ApplicationUser> _userManager;

        public OrderController(ApplicationDbContext context, UserManager<ApplicationUser>userManager )
        {
            _context = context;
            _userManager = userManager;
            _products = new Repository<Product>(context);
            _orders = new Repository<Order>(context);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = HttpContext.Session.Get<OrderViewModel>("OrderViewModel") ?? new OrderViewModel
            {
                OrderItems = new List<OrderItemViewModel>(),
                Products=await _products.GetAllAsync()
            };
            return View(model);
        }

        [HttpPost]
        [Authorize]

        public async Task<IActionResult>AddItem(int prodId,int prodQty)
        {
            var product = await _context.Products.FindAsync(prodId);
            if (product == null)
            {
                return NotFound();
            }
            var model = HttpContext.Session.Get<OrderViewModel>("OrderViewModel") ?? new OrderViewModel
            {
                OrderItems = new List<OrderItemViewModel>(),
                Products = await _products.GetAllAsync()
            };
            var existingItem=model.OrderItems.FirstOrDefault(oi=>oi.ProductId == prodId);
            if(existingItem!= null)
            {
                existingItem.Quantity = prodQty;
            }
            else
            {
                model.OrderItems.Add(new OrderItemViewModel
                {
                    ProductId=product.ProductId,
                    Price=product.Price,
                    Quantity=prodQty,
                    ProductName=product.Name
                });
            }
            model.TotalAmount=model.OrderItems.Sum(oi=>oi.Price*oi.Quantity);

            HttpContext.Session.Set("OrderViewModel", model);
            return RedirectToAction("Create",model);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Cart()
        {
            var model = HttpContext.Session.Get<OrderViewModel>("OrderViewModel");
            if (model == null || model.OrderItems.Count == 0)
            {
                return RedirectToAction("create");
            }
            return View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> PlaceOrder()
        {
            var model = HttpContext.Session.Get<OrderViewModel>("OrderViewModel");
            if (model==null||model.OrderItems.Count==0)
            {
                return RedirectToAction("create");
            }
            // create a new Order entity
            Order order = new Order
            {
                OrderDate = DateTime.Now,
                TotalAmount = model.TotalAmount,
                UserId = _userManager.GetUserId(User)
            };
            //Add order item to other entity
            foreach (var item in model.OrderItems)
            {
                order.OrderItems.Add(new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Price,
                });
            }

            await _orders.AddAsync(order);
            HttpContext.Session.Remove("OrderViewModel"); ;
            return RedirectToAction("ViewOrders");
        }

        [HttpGet]
        [Authorize]

        public async Task<IActionResult> ViewOrders()
        {
            var userId= _userManager.GetUserId(User);
            var userOrders=await _orders.GetAllByIdAsync(userId,"UserId",new QueryOptions<Order>
            {
                Includes="OrderITems.Product"
            });
            return View(userOrders);
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
