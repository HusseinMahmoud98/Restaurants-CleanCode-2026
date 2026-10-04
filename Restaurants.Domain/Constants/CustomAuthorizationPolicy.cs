namespace Restaurants.Domain.Constants
{
    public static class CustomAuthorizationPolicy
    {
        public const string HasNationality = "HasNationality";
        public const string HasDateOfBirth = "HasDateOfBirth";
        public const string AtLeastTwentyYears = "AtLeast20Years";
        public const string CreatedAtLeastTwoRestaurants = "CreatedAtLeastTwoRestaurants";
    }
}
