using Core.Interfaces;
using Microsoft.Extensions.Localization;



namespace Core.Services
{
    public class LocalizationService : ILocalizationService
    {
         private readonly IStringLocalizer _localizer;
        public LocalizationService(IStringLocalizer<LocalizationService> localizer)
    {
        _localizer = localizer;
    }
        public string GetLocalizedString(string key)
        {
            throw new NotImplementedException();
        }
    }
}