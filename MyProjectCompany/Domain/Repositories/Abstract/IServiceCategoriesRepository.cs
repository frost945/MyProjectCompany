using MyProjectCompany.Domain.Entities;

namespace MyProjectCompany.Domain.Repositories.Abstract
{
    public interface IServiceCategoriesRepository
    {
        Task<IEnumerable<ServiceCategory>> GetServiceCategoriesAsync();
        Task<ServiceCategory?> GetServiceCategoryByIdAsync(int id);
        Task SaveServiceCategoryAsync(ServiceCategory entity);
        Task DeleteServiceCategoryAsync(int id);
    }
}
