﻿namespace MyProjectCompany.Domain.Entities
{
    public class ServiceCategory:EntityBase
    {
        public ICollection<Service>? Services {  get; set; }
    }
}
