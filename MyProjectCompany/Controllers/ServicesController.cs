using Microsoft.AspNetCore.Mvc;
using MyProjectCompany.Domain;
using MyProjectCompany.Domain.Entities;
using MyProjectCompany.Infrastructure;
using MyProjectCompany.Models;

namespace MyProjectCompany.Controllers
{
    public class ServicesController : Controller
    {
        private readonly DataManager _dataManager;

        public ServicesController(DataManager dataManager)
        {
            _dataManager = dataManager;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Service> list = await _dataManager.Services.GetServicesAsync();

            //доменную сущность оборачиваем в DTO, для ее использования на клиенте
            IEnumerable<ServiceDTO> listDTO = HelperDTO.TransformService(list);

            return View(listDTO);
        }

        public async Task<IActionResult> Show(int id)
        {
            Service? entity = await _dataManager.Services.GetServiceByIdAsync(id);
            if (entity is null)
                return NotFound();

            ServiceDTO entityDTO = HelperDTO.TransformService(entity);
            return View(entityDTO);
        }
    }
}
