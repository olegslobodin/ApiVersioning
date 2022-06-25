using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace ApiVersioning.Foundation
{
    public static class ApiDescriptionConflictResolver
    {
        /// <summary>
        /// Builds a conflicts resolver which checks <see cref="MapToApiVersionAttribute"/> for each conflicting method.<br/>
        /// Methods with default API version are preferred if <paramref name="defaultApiVersion"/> is specified.<br/>
        /// If <paramref name="defaultApiVersion"/> is not specified, method with the latest API version is chosen.
        /// </summary>
        /// <param name="defaultApiVersion"></param>
        /// <returns>API Description resolver func</returns>
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
