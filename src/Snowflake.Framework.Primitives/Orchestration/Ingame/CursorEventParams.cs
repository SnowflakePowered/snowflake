using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Orchestration.Ingame
{

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct CursorEventParams
    {
        public Cursor Cursor;
    }

    /// <summary>
    /// Cursor types. These are the same as CefSharp cursors
    /// </summary>
    public enum Cursor : byte
    {
        Pointer = 0,
        Cross,
        Hand,
        IBeam,
        Wait,
        Help,
        EastResize,
        NorthResize,
        NortheastResize,
        NorthwestResize,
        SouthResize,
        SoutheastResize,
        SouthwestResize,
        WestResize,
        NorthSouthResize,
        EastWestResize,
        NortheastSouthwestResize,
        NorthwestSoutheastResize,
        ColumnResize,
        RowResize,
        MiddlePanning,
        EastPanning,
        NorthPanning,
        NortheastPanning,
        NorthwestPanning,
        SouthPanning,
        SoutheastPanning,
        SouthwestPanning,
        WestPanning,
        Move,
        VerticalText,
        Cell,
        ContextMenu,
        Alias,
        Progress,
        NoDrop,
        Copy,
        None,
        NotAllowed,
        ZoomIn,
        ZoomOut,
        Grab,
        Grabbing,
        MiddlePanningVertical,
        MiddlePanningHorizontal,
        Custom,
        DndNone,
        DndMove,
        DndCopy,
        DndLink
    }
}
