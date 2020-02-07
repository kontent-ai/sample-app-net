using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DancingGoat
{
    /// <summary>
    /// Configuration section representing App's default configuration
    /// </summary>
    public class AppConfiguration
    {
        /// <summary>
        /// Gets to use image transformation (https://docs.kontent.ai/reference/image-transformation).
        /// </summary>
        public bool ResponsiveImagesEnabled => ResponsiveWidthsArray.Any();

        /// <summary>
        /// Widths used to generate srcset of responsive images.
        /// </summary>
        public string ResponsiveWidths { get; set; }

        public string[] ResponsiveWidthsArray => ResponsiveWidths?.Split(',') ?? new string[] { };

        /// <summary>
        /// Project ID of a reference Kentico Kontent sample project.
        /// </summary>
        public Guid DefaultProjectId { get; set; }
    }
}
