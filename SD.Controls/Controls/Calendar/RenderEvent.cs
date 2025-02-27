using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SD.Controls.Controls
{
    public class RenderEvent
    {
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int GridRow { get; set; }
        public int GridColumn { get; set; }
        public int ColumnSpan { get; set; }
        public string ColorKey { get; set; }
    }

}
