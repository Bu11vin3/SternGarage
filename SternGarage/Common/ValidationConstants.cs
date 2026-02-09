namespace MercedesBlog.Common
{
    public static class ValidationConstants
    {
        // Car
        public const int CarModelMinLength = 2;
        public const int CarModelMaxLength = 100;
        public const int CarYearMin = 1900;
        public const int CarYearMax = 2026;
        public const int CarHorsepowerMin = 50;
        public const int CarHorsepowerMax = 2000;
        public const string CarPriceMin = "5000";
        public const string CarPriceMax = "5000000";
        public const int CarEngineTypeMaxLength = 50;
        public const int CarDescriptionMaxLength = 2000;

        // Review
        public const int ReviewAuthorMinLength = 2;
        public const int ReviewAuthorMaxLength = 100;
        public const int ReviewTitleMinLength = 5;
        public const int ReviewTitleMaxLength = 200;
        public const int ReviewContentMinLength = 10;
        public const int ReviewContentMaxLength = 5000;
        public const int ReviewRatingMin = 1;
        public const int ReviewRatingMax = 5;

        // CarClass
        public const int ClassNameMinLength = 1;
        public const int ClassNameMaxLength = 50;
        public const int ClassDescriptionMaxLength = 500;
    }
}
