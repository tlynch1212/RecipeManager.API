using System.Collections.Generic;
using System.Reflection.Emit;

namespace RecipeManager.Core.Analytics
{
    public class TableData
    {
        public string Title { get; set; }
        public List<TableDataSet> DataSets { get; set; }
    }

    public class TableDataSet
    {
        public List<string> Labels { get; set; }
        public List<int> Data { get; set; }
    }
}