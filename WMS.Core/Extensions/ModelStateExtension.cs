using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace WMS.Core.Extensions;

public static class ModelStateErrorExtension
{
    public static List<string> GetErrorMessages(this ModelStateDictionary modelState)
    {
        return modelState
               .Where(x => x.Value.Errors.Count > 0)
               .SelectMany(x => x.Value.Errors)
               .Select(x => x.ErrorMessage)
               .ToList();
    }
}