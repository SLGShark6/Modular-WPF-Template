using System;
using System.IO;
using System.Reflection;

namespace ModularWPFTemplate.Helpers
{
    /// <summary>
    /// Offers common directory operations of the application
    /// </summary>
    public static class PathHelper
    {
        /// <summary>
        /// The absolute disk path of the application
        /// </summary>
        /// <returns></returns>
        public static string AppBasePath
        {
            get
            {
                return AppContext.BaseDirectory;
            }
        }


        /// <summary>
        /// Gets the absolute directory disk path of the calling Assembly
        /// </summary>
        /// <returns></returns>
        public static string ExecutingDirectory
        {
            get
            {
                // Get the codebase defined by the calling assembly
                var callingAssemblyCodeBase = Assembly.GetCallingAssembly().CodeBase;
                // Get the Uri format of the codebase
                var uri = new UriBuilder(callingAssemblyCodeBase);

                // Get the full path string minus the URI elements (File://)
                var path = Uri.UnescapeDataString(uri.Path);

                // Return the directory without the filename at the end
                return Path.GetDirectoryName(path);
            }
        }


        /// <summary>
        /// Maps a virtual path (signified by tilde "~") to an absolute path
        /// using base path.
        /// 
        /// If no base path is provided the application base path is used.
        /// </summary>
        /// <param name="path">Path Relative to BaseDirectory. E.g. "~/bin"</param>
        /// <param name="basePath">Base path to map the virtual/ relative path to</param>
        /// <returns>The physical path. E.g. "c:\\project-dir\\.build\\bin"</returns>
        public static string MapPath(string path, string basePath = null)
        {
            // ToDo include resolving relative paths

            // If no base path 
            if (basePath == null)
            {
                // Use application base path
                basePath = AppBasePath;
            }

            // If the path starts with the wildcard
            if (path.StartsWith("~"))
            {
                // Replace the wildcard
                path = path.Replace("~/", "").TrimStart('/');
            }

            // Return the fully resolved path
            return Path.GetFullPath(path, basePath);
        }

        /// <summary>
        /// Check whether a path string is well-formed for the provided path
        /// type.
        /// 
        /// DOES NOT check if path exists.
        /// </summary>
        /// <param name="path">The path string to validate</param>
        /// <param name="pathType">The type of path to validate for</param>
        /// <param name="message">Outbound validation message</param>
        /// <returns></returns>
        public static bool IsPathWellFormed(string path, PathKind pathType, out string message)
        {
            // Default to valid
            var isValid = true;
            message = "Valid";

            // If provided path contains any invalid characters
            if (path == null || path.IndexOfAny(Path.GetInvalidPathChars()) >= 0)
            {
                // Mark invalid
                isValid = false;
                message = "Invalid path";
            }
            else
            {
                // Otherwise if it is necessary to check whether the path is valid for it's type
                if (pathType != PathKind.RelativeOrAbsolute)
                {
                    // Whether or not the path is fully qualified (has a root drive or is UNC)
                    var isFullyQualified = Path.IsPathFullyQualified(path);
                    // Get whether the path is a UNC path
                    var isUnc = Uri.TryCreate(path, UriKind.Absolute, out Uri uri) && uri.IsUnc;

                    // If the path should be relative BUT is fully qualified
                    if (pathType == PathKind.Relative && isFullyQualified)
                    {
                        // Mark invalid
                        isValid = false;
                        message = "Invalid relative path";
                    }
                    // Otherwise if the path should be fully qualified BUT is not
                    else if (pathType == PathKind.FullyQualified && !isFullyQualified)
                    {
                        // Mark invalid
                        isValid = false;
                        message = "Invalid fully qualified path";
                    }
                    // Otherwise If path should be UNC but is not UNC (rooted but not fully qualified, or relative)
                    else if (pathType == PathKind.UNC && !isUnc)
                    {
                        // Mark invalid
                        isValid = false;
                        message = "Invalid UNC path";
                    }
                    // Otherwise if path should be Local but is UNC
                    else if (pathType == PathKind.LocalAbsolute && (!isFullyQualified || isUnc))
                    {
                        // Mark invalid
                        isValid = false;
                        message = "Invalid local absolute path";
                    }
                }
            }

            return isValid;
        }
    }


    /// <summary>
    /// Enum used for specifying the type of path a string represents
    /// </summary>
    public enum PathKind
    {
        /// <summary>
        /// The path is a fully qualified local file system path
        /// </summary>
        LocalAbsolute,

        /// <summary>
        /// The path is a fully qualified UNC path
        /// </summary>
        UNC,

        /// <summary>
        /// The path is fully qualified
        /// </summary>
        FullyQualified,

        /// <summary>
        /// Path is relative to another or the current directory
        /// </summary>
        Relative,

        /// <summary>
        /// Path can either be relative or fully qualified
        /// </summary>
        RelativeOrAbsolute
    }
}
