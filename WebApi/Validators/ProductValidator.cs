using FluentValidation;
using Models;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Validators
{
    public class ProductValidator : AbstractValidator<Product>
    {
        public ProductValidator(ILogger<ProductValidator> logger, ICrudService<Product> products)
        {
            logger.LogInformation("logowanie walidatora");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Brakuje tu czegoś....").Length(5, 15)
                .Must((product, element) => !element.Contains(product.Price.ToString())).WithMessage("Nazwa produktu zawiera cenę!")
                .Must(x => !products.ReadAsync().Result.Any(xx => xx.Name == x)).WithMessage("Nazwa produktu musi być unikalna!")
                .WithName("Nazwa");

        }
    }
}
