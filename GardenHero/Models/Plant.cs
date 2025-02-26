using System.Text.Json;

namespace GardenHero.Models
{
    // Pflanze-Tabelle mit Keimverhalten
    public class Plant
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;
        public string LightRequirement { get; set; } = string.Empty;
        public string SowingTimeJson { get; set; } = "[]";
        public string HarvestTimeJson { get; set; } = "[]";
        public double Depth { get; set; }
        public double Spacing { get; set; }
        public string GerminationType { get; set; } = string.Empty;
        public string ImagePath { get; set; } = string.Empty;

        // **Getter/Setter für int[] mit JSON-Serialisierung**
        public int[] SowingTime
        {
            get => JsonSerializer.Deserialize<int[]>(SowingTimeJson) ?? new int[0];
            set => SowingTimeJson = JsonSerializer.Serialize(value);
        }

        public int[] HarvestTime
        {
            get => JsonSerializer.Deserialize<int[]>(HarvestTimeJson) ?? new int[0];
            set => HarvestTimeJson = JsonSerializer.Serialize(value);
        }
    }
}
