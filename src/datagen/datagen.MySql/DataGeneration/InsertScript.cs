using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace datagen.MySql
{
    internal class InsertScripts
    {
        public List<InsertScript> Scripts { get; set; } = new List<InsertScript>();
        public InsertScript AddLast()
        {
            var script = new InsertScript();
            Scripts.Add(script);
            return script;
        }
        public void InsertRange(IEnumerable<InsertScript> scripts) =>
            Scripts.InsertRange(0, scripts);                    

    }

    internal class InsertScript
    {
        public string Script { get; set; } = String.Empty;
        public Dictionary<string, object> Parameters { get; set; } = new Dictionary<string, object>();
    }
}
