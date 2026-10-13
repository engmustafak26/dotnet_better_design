/// <summary>
/// Provides extension methods for <see cref="IApplicationBuilder"/> to enforce 
/// the correct ordering of Authentication and Authorization middlewares.
/// </summary>
public static class SecurityExtensions
{
    /// <summary>
    /// An asynchronous version of the security pipeline setup.
    /// It registers Authentication, optionally runs an injected middleware/delegate, 
    /// and then registers Authorization.
    /// </summary>
    /// <param name="app">The application builder instance.</param>
    /// <param name="injectedCodeBetweenAuthenticationAndAuthorization">
    /// An optional asynchronous delegate to execute custom logic/middleware 
    /// between the Authentication and Authorization steps.
    /// </param>
    /// <returns>The <see cref="IApplicationBuilder"/> so calls can be chained.</returns>
    public static async Task<IApplicationBuilder> UseSecurityAsync(
       this IApplicationBuilder app, Func<Task> injectedCodeBetweenAuthenticationAndAuthorization = null)
    {
        // 1. Must run first: Populates HttpContext.User based on incoming credentials (e.g., JWT, Cookies).
        app.UseAuthentication();

        // 2. Optional middle step: Allows injecting custom logic/middleware that requires 
        //    the user to be authenticated, but not yet authorized.
        if (injectedCodeBetweenAuthenticationAndAuthorization != null)
        {
            await injectedCodeBetweenAuthenticationAndAuthorization();
        }

        // 3. Must run last: Evaluates policies against the authenticated user to allow/deny access.
        app.UseAuthorization();

        return app;
    }

    /// <summary>
    /// A synchronous version of the security pipeline setup.
    /// It registers Authentication, optionally runs an injected middleware/delegate, 
    /// and then registers Authorization.
    /// </summary>
    /// <param name="app">The application builder instance.</param>
    /// <param name="injectedCodeBetweenAuthenticationAndAuthorization">
    /// An optional synchronous delegate to execute custom logic/middleware 
    /// between the Authentication and Authorization steps.
    /// </param>
    /// <returns>The <see cref="IApplicationBuilder"/> so calls can be chained.</returns>
    public static IApplicationBuilder UseSecurity(
       this IApplicationBuilder app, Action injectedCodeBetweenAuthenticationAndAuthorization = null)
    {
        // 1. Authenticate: Identifies who the user is.
        app.UseAuthentication();

        // 2. Optional Injection: Run custom code (e.g., app.UseRequestLocalization()) 
        //    that depends on the authenticated user but not on authorization policies.
        if (injectedCodeBetweenAuthenticationAndAuthorization != null)
        {
            injectedCodeBetweenAuthenticationAndAuthorization();
        }

        // 3. Authorize: Determines if the identified user has access to the requested resource.
        app.UseAuthorization();

        return app;
    }
}