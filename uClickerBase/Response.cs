using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uClickerBase
{
    class Response
    {
        public string strResponse { get; set; }
        public int voterCount { get; set; }
        public Response(string input)
        {
            strResponse = input;
        }
    }
}
