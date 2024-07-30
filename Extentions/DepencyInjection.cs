using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using System.Globalization;
using System.Reflection;

namespace WebApplicationCulture.Extentions
{
    public static class DepencyInjection
    {
        public static IServiceCollection AddMVCServices(this IServiceCollection services)
        {
            //Reference Type olan verilerin null olabilme özelliğini true yapar.
            services
                .AddControllersWithViews(opt => opt.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true)
                .AddMvcLocalization(LanguageViewLocationExpanderFormat.Suffix, opt => opt.DataAnnotationLocalizerProvider = (type, factory) =>
                {
                    var assemblyName = new AssemblyName(typeof(SharedResource).GetTypeInfo().Assembly.FullName!); //SharedResource'u köprü olarak kullanıyoruz.
                    return factory.Create(nameof(SharedResource), assemblyName.Name!); //ünlem işareti asla null dönmeyeceğine söz veriyor
                });

            services.AddLocalization(opt => opt.ResourcesPath = "Resources");

            services.Configure<RequestLocalizationOptions>(opt =>
            {
                var supportedCulture = new List<CultureInfo>
                {
                    new CultureInfo("en"),
                    new CultureInfo("tr")
                };
                opt.DefaultRequestCulture = new RequestCulture("tr"); //cookie gerekiyor

                opt.SupportedUICultures = supportedCulture;
                opt.SupportedCultures = supportedCulture;

                //opt.RequestCultureProviders.Insert(0, new CookieRequestCultureProvider());

            });

            return services;
        }
    }
}
