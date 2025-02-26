using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GardenHero.Models
{
    // Schlechte Nachbarn
    public class BadNeighbor
    {
        public int Plant1Id { get; set; }
        public Plant Plant1 { get; set; } = null!;
        public int Plant2Id { get; set; }
        public Plant Plant2 { get; set; } = null!;
    }
}
