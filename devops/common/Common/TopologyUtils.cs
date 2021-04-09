using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Solid.Core;

namespace Common
{
    public static class TopologyUtils
    {
        public static IEnumerable<IPackageGroup> InitTopology()
        {
            var contents = File.ReadAllText("topology.json");
            var data = JsonConvert.DeserializeObject<Topology>(contents).Data;
            data = data.SortTopologically().ToList();
            return data;
        }
    }
}
