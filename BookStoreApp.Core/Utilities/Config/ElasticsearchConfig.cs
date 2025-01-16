using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreApp.Core.Utilities.Config
{
    public class ElasticsearchConfig
    {
        public string Url { get; set; }
        public string IndexName { get; set; }
    }
}
