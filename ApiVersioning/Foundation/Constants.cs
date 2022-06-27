namespace ApiVersioning.Foundation
{
    public static class Constants
    {
        public const int DefaultApiVersion = 2;

        public static string SwaggerDocumentVersion => $"v{DefaultApiVersion}";

        public static class Forecasts
        {
            public static class Params
            {
                public const int MinDaysCount = 1;
            }
        }

        public static class Policies
        {
            public static class AllowOrigin
            {
                public const string Any = "AllowAnyOrigin";
                public const string Google = "AllowGoogleOrigin";
            }
        }
    }
}
