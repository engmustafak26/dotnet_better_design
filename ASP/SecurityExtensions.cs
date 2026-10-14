
public static class SecurityExtensions
{
   
    public static async Task<IApplicationBuilder> UseSecurityAsync(
       this IApplicationBuilder app, Func<Task> injectedCodeBetweenAuthenticationAndAuthorization = null)
    {
      
        app.UseAuthentication();

        if (injectedCodeBetweenAuthenticationAndAuthorization != null)
        {
            await injectedCodeBetweenAuthenticationAndAuthorization();
        }

        app.UseAuthorization();

        return app;
    }


    public static IApplicationBuilder UseSecurity(
       this IApplicationBuilder app, Action injectedCodeBetweenAuthenticationAndAuthorization = null)
    {
        app.UseAuthentication();

        if (injectedCodeBetweenAuthenticationAndAuthorization != null)
        {
            injectedCodeBetweenAuthenticationAndAuthorization();
        }

        app.UseAuthorization();

        return app;
    }
}