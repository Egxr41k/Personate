using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBStyler.Win32.Types;

public struct MonitorInfo
{
    public long cbSize;
    public Rect rcMonitor;
    public Rect rcWork;
    public long dwFlags;
}
