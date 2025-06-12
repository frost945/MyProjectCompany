using Microsoft.AspNetCore.Mvc;
using MyProjectCompany.Domain.Entities;

namespace MyProjectCompany.Controllers.Admin
{
    public partial class AdminController
    {
        public async Task<IActionResult> ServicesEdit(int id)
        {
            //в зависимости от id либо добавляем, либо изменяем запись
            Service? entity = id == default ? new Service() : await _dataManager.Services.GetServiceByIdAsync(id);
            ViewBag.ServiceCategories = await _dataManager.ServiceCategories.GetServiceCategoriesAsync();

            return View(entity);
        }

        [HttpPost]
        public async Task<IActionResult> ServicesEdit(Service entity, IFormFile? titleImageFile)
        {
            //если в модели есть ошибки, возвращаем на доработку
            if (!ModelState.IsValid)
            {
                ViewBag.ServiceCategories = await _dataManager.ServiceCategories.GetServiceCategoriesAsync();
                return View(entity);
            }

            if(titleImageFile!= null)
            {
                entity.Photo = titleImageFile.FileName;
                await SaveImg(titleImageFile);
            }
            await _dataManager.Services.SaveServiceAsync(entity);
            _logger.LogInformation($"Добавлена/Обновлена услуга с ID: {entity.Id}");

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> ServicesDelete(int id)
        {
            await _dataManager.Services.DeleteServiceAsync(id);
            _logger.LogInformation($"Удалена услуга с ID: {id}");

            return RedirectToAction("Index");
        }

    }
}
