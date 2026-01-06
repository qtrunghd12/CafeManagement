using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;

public class TransparentTextBox : TextBox
{
    public TransparentTextBox()
    {
        SetStyle(ControlStyles.SupportsTransparentBackColor, true);
        BackColor = System.Drawing.Color.Transparent;
        BorderStyle = BorderStyle.None;
    }

    protected override CreateParams CreateParams
    {
        get
        {
            CreateParams cp = base.CreateParams;
            cp.ExStyle |= 0x20; // WS_EX_TRANSPARENT
            return cp;
        }
    }
}
