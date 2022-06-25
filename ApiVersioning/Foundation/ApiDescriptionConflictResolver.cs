using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace ApiVersioning.Foundation
{
    public static class ApiDescriptionConflictResolver
    {
        public static Func<IEnumerable<ApiDescription>, ApiDescription> PreferDefaultOrLatestApiVersion(int? defaultApiVersion = null) =>
            (IEnumerable<ApiDescription> descriptions) => PreferDefaultOrLatestApiVersion(descriptions, defaultApiVersion);

        private static ApiDescription PreferDefaultOrLatestApiVersion(IEnumerable<ApiDescription> descriptions, int? defaultApiVersion = null)
        {
            IReadOnlyCollection<int> getApiVersions(ApiDescription description)
                => description.ActionDescriptor.EndpointMetadata
                    .OfType<MapToApiVersionAttribute>()
                    .SelectMany(m => m.Versions.Where(v => v.MajorVersion.HasValue).Select(v => v.MajorVersion!.Value))
                    .ToList();

            bool hasDefaultApiVersion(ApiDescription description)
                => defaultApiVersion.HasValue && getApiVersions(description).Any(v => v == defaultApiVersion.Value);

            int getMaxApiVersion(ApiDescription description)
                => getApiVersions(description).DefaultIfEmpty(0).Max();

            return descriptions.OrderByDescending(hasDefaultApiVersion).ThenByDescending(getMaxApiVersion).First();
        }
    }
}
