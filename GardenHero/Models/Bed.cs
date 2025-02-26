using System.Collections.Generic;

namespace GardenHero.Models
{
    // Beet-Tabelle
    public class Bed
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public double Width { get; set; }
        public double Length { get; set; }
        public ICollection<Planting> Plantings { get; set; } = new List<Planting>();
    }
}
