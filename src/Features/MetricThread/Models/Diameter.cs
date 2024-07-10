using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ThreadApi.Features.MetricThread.Models
{
    public class DiameterModel
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        [Required]
        [DefaultValue(0)]
        public required int Id { get; set; } = 0;

        /// <summary>
        /// Gets or sets the diameter.
        /// </summary>
        [Required]
        [DefaultValue(0.0)]
        public required double Diameter { get; set; } = 0.0;


        public string? Test { get; set; }="20";
    }
}