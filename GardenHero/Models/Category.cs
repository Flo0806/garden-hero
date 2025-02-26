using System.Collections.Generic;

namespace GardenHero.Models
{
    // Kategorie-Tabelle mit bevorzugtem Boden
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string SoilType { get; set; } = string.Empty;
        public ICollection<Plant> Plants { get; set; } = new List<Plant>();
    }
}
