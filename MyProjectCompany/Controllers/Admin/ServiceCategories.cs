using Microsoft.AspNetCore.Mvc;
using MyProjectCompany.Domain.Entities;
using System.Threading.Tasks;

namespace MyProjectCompany.Controllers.Admin
{
    public partial class AdminController
    {
        public async Task<IActionResult> ServiceCategoriesEdit(int id)
        {
            //в зависимости от id либо добавляем, либо изменяем запись
            ServiceCategory? entity= id ==default ? new ServiceCategory() : await _dataManager.ServiceCategories.GetServiceCategoryByIdAsync(id);

            return View(entity);
        }

        [HttpPost]
        public async Task<IActionResult> ServiceCategoriesEdit(ServiceCategory entity)
        {
            //если в модели есть ошибки, возвращаем на доработку
            if(!ModelState.IsValid)
                return View(entity);

            await _dataManager.ServiceCategories.SaveServiceCategoryAsync(entity);
            _logger.LogInformation($"Добавлена/Обновлена категория услуги с ID: {entity.Id}");

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> ServiceCategoriesDelete(int id)
        {
            //т.к. в целях безопасности отключено каскадное удаление, то прежде чем удалить категорию,
            //что на нее нет ссылки ни у одной из услуг
            await _dataManager.ServiceCategories.DeleteServiceCategoryAsync(id);
            _logger.LogInformation($"Удалена категория услуги с ID: {id}");

            return RedirectToAction("Index");
        }
    }
}
