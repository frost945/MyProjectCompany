﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyProjectCompany.Domain;
using System.Text.Json;

namespace MyProjectCompany.Controllers.Admin
{
    [Authorize(Roles ="admin")]
    public partial class AdminController:Controller
    {
        private readonly DataManager _dataManager;
        private readonly IWebHostEnvironment _hostingtEnvironment;
        private readonly ILogger<AdminController> _logger;

        public AdminController(DataManager dataManager, IWebHostEnvironment hostingEnvironment, ILogger<AdminController> logger)
        {
            _dataManager = dataManager;
            _hostingtEnvironment = hostingEnvironment;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.ServiceCategories=await _dataManager.ServiceCategories.GetServiceCategoriesAsync();
            ViewBag.Services=await _dataManager.Services.GetServicesAsync();
            return View();
        }

        public async Task<string> SaveImg(IFormFile img)
        {
            string path = Path.Combine(_hostingtEnvironment.WebRootPath, "img/", img.FileName);
            await using FileStream stream = new FileStream(path, FileMode.Create);
            await img.CopyToAsync(stream);

            return path;
        }

        public async Task<string> SaveEditorImg()
        {
            IFormFile img = Request.Form.Files[0];
            await SaveImg(img);

            return JsonSerializer.Serialize(new { location = Path.Combine("/img/", img.FileName) });
        }
    }
}
