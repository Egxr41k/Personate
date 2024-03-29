﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBStyler.Win32.Types;

public enum WindowStyles
{
    WS_BORDER = 0x800000,
    WS_CAPTION = 0xC00000,
    WS_CHILD = 0x40000000,
    WS_CLIPCHILDREN = 0x2000000,
    WS_CLIPSIBLINGS = 0x4000000,
    WS_DISABLED = 0x8000000,
    WS_DLGFRAME = 0x400000,
    WS_GROUP = 0x20000,
    WS_HSCROLL = 0x100000,
    WS_MAXIMIZE = 0x1000000,
    WS_MAXIMIZEBOX = 0x10000,
    WS_MINIMIZE = 0x20000000,
    WS_MINIMIZEBOX = 0x20000,
    WS_OVERLAPPED = 0x0,
    WS_OVERLAPPEDWINDOW =
        WS_OVERLAPPED |
        WS_CAPTION |
        WS_SYSMENU |
        WS_SIZEFRAME |
        WS_MINIMIZEBOX |
        WS_MAXIMIZEBOX,
    WS_SIZEFRAME = 0x40000,
    WS_SYSMENU = 0x80000,
    WS_TABSTOP = 0x10000,
    WS_VISIBLE = 0x10000000,
    WS_VSCROLL = 0x200000
}
