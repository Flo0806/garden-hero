
namespace SD.Controls.Controls
{
    public enum MonthState
    {
        Empty = 0,
        HalfMonth = 1,
        FullMonth = 2,
    }

    public class MonthItem
    {
        public string Name { get; set; }
        public MonthState State { get; set; }
    }

}
