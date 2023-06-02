using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenVapour.OpenVapourAPI {
    internal partial class FixedFlowLayoutPanel : FlowLayoutPanel {
        internal FixedFlowLayoutPanel() : base() {
            SetStyle(ControlStyles.ResizeRedraw | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.SupportsTransparentBackColor, true); }
            protected override void OnScroll(ScrollEventArgs se) {
                Invalidate();
                base.OnScroll(se); }
            protected override CreateParams CreateParams  {
                get {
                    CreateParams cp = base.CreateParams;
                    cp.ExStyle |= 0x02000000;
                    return cp; }}}}
