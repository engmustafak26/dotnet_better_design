 // Misordering mistakes, easily broken design **Wrong**
  app.UseAuthorization();
  app.UseAuthentication();
 

// By using app.UseSecurity(), we encapsulate the correct ordering (Temporal Cohesion).
// This prevents the common mistake of misordering the authentication and authorization middlewares. 

app.UseSecurity();
 // Or
 app.UseSecurity(() => app.UseRequestLocalization()); // inject whatever between authentication and authorization

 /* rest of flow
  ...
*/

 app.MapControllers();
