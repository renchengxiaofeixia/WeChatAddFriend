using AdvancedSharpAdbClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedSharpAdbClient.Receivers
{
    public class SerilogOutputReceiver : MultiLineReceiver
    {
        private List<string> _output = new List<string>();

        public List<string> Output
        {
            get
            {
                return _output;
            }
            set
            {
                _output = value;
            }
        }


        protected override void ProcessNewLines(IEnumerable<string> lines)
        {
            //foreach (var line in lines)
            //    Log.Information("[server] {@LogLine}", line);

            foreach (string item in lines)
            {
                this.Output.Add(item);
            }
        }
    }
}
