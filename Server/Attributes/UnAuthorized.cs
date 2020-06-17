using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Server.Attributes {
    public class UnAuthorizedAttribute : TypeFilterAttribute {
        public UnAuthorizedAttribute() : base(typeof(UnauthorizedFilter)) {
            //Empty constructor
        }

        public class UnauthorizedFilter : IAuthorizationFilter {
            public void OnAuthorization(AuthorizationFilterContext context) {
                bool IsAuthenticated = context.HttpContext.User.Identity.IsAuthenticated;
                if (!IsAuthenticated) {
                    context.Result = new RedirectResult("~/Login");
                }
            }
        }
    }
}
