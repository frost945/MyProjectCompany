using Microsoft.AspNetCore.Mvc;
using MyProjectCompany.Domain;
using MyProjectCompany.Domain.Entities;
using MyProjectCompany.Infrastructure;
using System.Collections.Generic;

namespace MyProjectCompany.Models.Components
{
    public class MenuViewComponent: ViewComponent
    {
        private readonly DataManager _dataManager;
        
        public MenuViewComponent(DataManager dataManager)
        {
            _dataManager = dataManager;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            IEnumerable<Service> list= await _dataManager.Services.GetServicesAsync();

            //доменную сущность оборачиваем в DTO, для ее использования на клиенте
            IEnumerable<ServiceDTO> listDTO = HelperDTO.TransformService(list);

            return await Task.FromResult((IViewComponentResult)View("Default", listDTO));
        }
    }
}
