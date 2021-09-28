using System.Reflection;
using System.Threading.Tasks;
using System.IO;

namespace ModularWPFTemplate.Helpers
{
    public class ResourceHelper
    {
        public ResourceHelper()
        {

        }


        /// <summary>
        /// Full qualifies a resource using the assembly it is contained in
        /// </summary>
        /// <param name="resourceName">Relative path of the resource in the assembly folder structure e.g. "Modules/description-schema.json"</param>
        /// <param name="assembly">(Optional) If no assembly is given the current calling assembly is used</param>
        /// <returns></returns>
        public string FormatResourceName(string resourceName, Assembly assembly = null)
        {
            // If no assembly provided
            if (assembly == null)
                assembly = getDefaultAssembly(); // Get Default assembly


            // Replace any invalid file system characters with appropriate namespace characters
            resourceName = resourceName.Replace(" ", "_")
                                       .Replace("\\", ".")
                                       .Replace("/", ".");

            // "Fully Qualified Assembly Name"."Resource Name"
            return $"{assembly.GetName().Name}.{resourceName}";
        }


        /// <summary>
        /// Gets the stream for an embedded resource in a specific assembly
        /// </summary>
        /// <param name="resourceName">Fully Qualified name of the resource</param>
        /// <param name="assembly">(Optional) if not provided the current executing assembly is used</param>
        /// <returns>All the text contained in the resource</returns>
        public string GetEmbeddedResource(string resourceName, Assembly assembly = null)
        {
            var resourceText = string.Empty;
            
            // If no assembly provided
            if (assembly == null)
                assembly = getDefaultAssembly(); // Get default assembly
            

            var resourceStream = assembly.GetManifestResourceStream(resourceName);

            if (resourceStream == null)
                throw new FileNotFoundException($"No {resourceName} resource in assembly {assembly.FullName}.");

            // read from the resource stream
            using (var reader = new StreamReader(resourceStream))
            {
                resourceText = reader.ReadToEnd();
            }

            return resourceText;
        }

        /// <summary>
        /// Gets the stream for an embedded resource in a specific assembly
        /// </summary>
        /// <param name="resourceName">Fully Qualified name of the resource</param>
        /// <param name="assembly">(Optional) if not provided the current executing assembly is used</param>
        /// <returns>All the text contained in the resource</returns>
        public Task<string> GetEmbeddedResourceAsync(string resourceName, Assembly assembly = null)
        {
            return Task.FromResult(GetEmbeddedResource(resourceName, assembly));
        }


        /// <summary>
        /// Gets the default assembly to use for Embedded Resource operations
        /// </summary>
        /// <returns>Default Assembly</returns>
        private Assembly getDefaultAssembly()
        {
            return Assembly.GetCallingAssembly();
        }
    }
}
