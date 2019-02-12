using System;
using Microsoft.AspNetCore.Http;

namespace hookup.API.Helpers
{
  public static class Extensions
  {
    // general purpose extensions class

    public static void AddApplicationError(this HttpResponse response, string message)
    {
      response.Headers.Add("Application-Error", message);
      response.Headers.Add("Access-Control-Expose-Headers", "Application-Error");
      response.Headers.Add("Access-Control-Allow-Origin", "*");
    }

    /**
     * Extension to date time class to calculate an age
     * based on a supplied year.completionlist It then
     * calculates between the year and now and subtracts
     * if a birthday has not yet been reached.
     */
    public static int CalculateAge(this DateTime theDateTime)
    {
      var age = DateTime.Today.Year - theDateTime.Year;
      if (theDateTime.AddYears(age) > DateTime.Today)
        age--;
      return age;
    }
  }
}