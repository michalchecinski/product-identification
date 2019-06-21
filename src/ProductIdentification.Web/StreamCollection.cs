using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductIdentification.Web
{
    public class StreamCollection : Collection<Stream>, IDisposable
    {
        public void Dispose()
        {
            foreach (var stream in Items)
            {
                stream.Dispose();
            }
        }
    }
}
