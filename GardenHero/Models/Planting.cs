using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GardenHero.Models
{
    public class Planting
    {
        public int Id { get; set; }
        public int PlantId { get; set; }
        public Plant Plant { get; set; } = null!;
        public int BedId { get; set; }
        public Bed Bed { get; set; } = null!;
        public int Quantity { get; set; }
        public string SowingDate { get; set; } = string.Empty;
        public string ExpectedHarvestDate { get; set; } = string.Empty;
    }
}
