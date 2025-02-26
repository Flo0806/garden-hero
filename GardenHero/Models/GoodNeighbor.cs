namespace GardenHero.Models
{
    // Gute Nachbarn
    public class GoodNeighbor
    {
        public int Plant1Id { get; set; }
        public Plant Plant1 { get; set; } = null!;
        public int Plant2Id { get; set; }
        public Plant Plant2 { get; set; } = null!;
    }
}
