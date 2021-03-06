using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace PortManager
{
    public class Helper
    {
        public static IActionResult Protect(ISession session)
        {
            if (CurrentUser(session) == null)
                return new RedirectResult("/Login");
            return null;
        }

        public static Models.User CurrentUser(ISession session)
        {
            if (session.GetInt32("user_id") != null)
                return Models.User.GetOne((int)session.GetInt32("user_id"));
            return null;
        }
    }
}
