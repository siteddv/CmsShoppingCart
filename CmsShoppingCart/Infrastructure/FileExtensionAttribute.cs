using CmsShoppingCart.Infrastructure;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;

namespace CmsShoppingCart.Model
{
    public class FileExtensionAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var context = (CmsShoppingCartContext)validationContext.GetService(typeof(CmsShoppingCartContext));
            var errorMessage = "Allowed extensions are jpg and png.";

            var file = value as IFormFile;

            if(file == null)
                return new ValidationResult(errorMessage);

            var extension = Path.GetExtension(file.FileName);
            string[] extensions = { "jpg", "png" };

            var result = extensions.Any(x => extension.EndsWith(x));

            if (!result)
            {
                    
                return new ValidationResult(errorMessage);
            }

            return ValidationResult.Success;    
        }
    }
}