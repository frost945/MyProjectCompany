using MyProjectCompany.Domain.Repositories.Abstract;

namespace MyProjectCompany.Domain
{
    public class DataManager
    {
        public IServiceCategoriesRepository ServiceCategories { get; set; }
        public IServicesRepository Services { get; set; }

        public DataManager(IServiceCategoriesRepository serviceCategoriesRepository, IServicesRepository servicesRepository)
        {
            ServiceCategories = serviceCategoriesRepository;
            Services = servicesRepository;
        }
    }
}
