using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace RoleBasedAccess.Controllers;

public class CultureController : Controller
{
    public IActionResult SetLanguage(
        string culture,
        string returnUrl)
    {
        Response.Cookies.Append(
            CookieRequestCultureProvider.DefaultCookieName,
            CookieRequestCultureProvider.MakeCookieValue(
                new RequestCulture(culture)));

        return LocalRedirect(returnUrl);
    }
}