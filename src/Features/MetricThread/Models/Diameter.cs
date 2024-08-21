using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ThreadApi.Features.MetricThread.Models
{
    public class DiameterModel
    {

        [Required]
        public required int id { get; set; }

        [Required]
        public required string diam { get; set; }


    }
}