﻿using Core.DTOs;
using Core.Entities.Application;
using Core.IServices;
using Core.IUoW;
using Microsoft.AspNetCore.Mvc;
using Services;


namespace Web.Controllers
{
    public class ItemController : Controller
    {
        #region Dependency Injection
        private readonly IItemService _itemService;
        public ItemController(IItemService itemService)
        {
            _itemService = itemService;
        }
        #endregion

        #region Get All
        public async Task<IActionResult> Index()
        {
            var pagination = new PaginationDto();
            ViewBag.Page = pagination.Page;
            ViewBag.TotalPages = await _itemService.GetTotalPagesAsync(pagination.Size);
            return View(await _itemService.GetPageAsync(pagination));
        }

        [HttpPost]
        public async Task<IActionResult> Index(PaginationDto pagination)
        {
            ViewBag.Page = pagination.Page;
            ViewBag.TotalPages = await _itemService.GetTotalPagesAsync(pagination.Size);
            return View(await _itemService.GetPageAsync(pagination));
        }
        #endregion


        // Auth
        #region Add Item To Receipt
        public async Task<IActionResult> AddItem(int itemId)
        {
            await _itemService.UserAddAsync(User, itemId);

            return RedirectToAction("Index");
        }
        #endregion

        #region Delete Item from Receipt
        public async Task<IActionResult> DeleteItem(int itemId)
        {
            await _itemService.UserDeleteAsync(User, itemId);
            return RedirectToAction("NewReceipt","Receipt");
        }
        #endregion

    }
}
