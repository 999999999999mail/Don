using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Don.Common.Filter
{
    public class CustomAuthorizeFilter : IAsyncAuthorizationFilter
    {
        public AuthorizationPolicy Policy { get; }

        public CustomAuthorizeFilter()
            :this(new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build())
        {
        }

        public CustomAuthorizeFilter(AuthorizationPolicy policy)
        {
            Policy = policy ?? throw new ArgumentNullException(nameof(policy));
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.Filters.Any(p => p is IAllowAnonymousFilter))
            {
                return;
            }

            var policyEvaluator = context.HttpContext.RequestServices.GetRequiredService<IPolicyEvaluator>();
            var authenticateResult = await policyEvaluator.AuthenticateAsync(Policy, context.HttpContext);
            var authorizeResult = await policyEvaluator.AuthorizeAsync(Policy, authenticateResult, context.HttpContext, context);
            if (authorizeResult.Challenged)
            {
                context.Result = new ChallengeResult();
            }
            else if (authorizeResult.Forbidden)
            {
                context.Result = new ForbidResult();
            }
        }
    }
}
