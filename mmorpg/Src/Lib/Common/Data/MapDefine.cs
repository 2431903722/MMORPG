using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Data
{
    public enum MapType
    {
        Normal = 0,
        Arena = 1,
    }

    public class MapDefine
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Resource { get; set; }
        public string Music { get; set; }
        public string Minimap { get; set; }
        public MapType Type { get; set; }
    }
}
