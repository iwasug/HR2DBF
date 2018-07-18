using System;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.ComponentModel;
using System.Text;
using System.Collections.Generic;
using System.Diagnostics;

//IMPORTANT:
//Please leave these comments in place as they help protect intellectual rights and allow
//developers to determine the version of the theme they are using. The preffered method
//of distributing this theme is through the Nimoru Software home page at nimoru.com.

//Name: Net Seal Theme
//Created: 6/21/2013
//Version: 1.0.0.2 beta
//Site: http://nimoru.com/

//This work is licensed under a Creative Commons Attribution 3.0 Unported License.
//To view a copy of this license, please visit http://creativecommons.org/licenses/by/3.0/

//Copyright © 2013 Nimoru Software

static class ThemeModule
{

    internal static Graphics G;

    private static Bitmap TextBitmap;

    private static Graphics TextGraphics;

    private static GraphicsPath CreateRoundPath;

    private static Rectangle CreateRoundRectangle;

    static ThemeModule()
    {
        ThemeModule.TextBitmap = new Bitmap(1, 1);
        ThemeModule.TextGraphics = Graphics.FromImage(ThemeModule.TextBitmap);
    }

    internal static SizeF MeasureString(string text, Font font)
    {
        return ThemeModule.TextGraphics.MeasureString(text, font);
    }

    internal static SizeF MeasureString(string text, Font font, int width)
    {
        return ThemeModule.TextGraphics.MeasureString(text, font, width, StringFormat.GenericTypographic);
    }

    internal static GraphicsPath CreateRound(int x, int y, int width, int height, int slope)
    {
        ThemeModule.CreateRoundRectangle = new Rectangle(x, y, width, height);
        return ThemeModule.CreateRound(ThemeModule.CreateRoundRectangle, slope);
    }

    internal static GraphicsPath CreateRound(Rectangle r, int slope)
    {
        ThemeModule.CreateRoundPath = new GraphicsPath(FillMode.Winding);
        ThemeModule.CreateRoundPath.AddArc(r.X, r.Y, slope, slope, 180f, 90f);
        checked
        {
            ThemeModule.CreateRoundPath.AddArc(r.Right - slope, r.Y, slope, slope, 270f, 90f);
            ThemeModule.CreateRoundPath.AddArc(r.Right - slope, r.Bottom - slope, slope, slope, 0f, 90f);
            ThemeModule.CreateRoundPath.AddArc(r.X, r.Bottom - slope, slope, slope, 90f, 90f);
            ThemeModule.CreateRoundPath.CloseFigure();
            return ThemeModule.CreateRoundPath;
        }
    }

}

class NSTheme_Alt : ThemeContainer154
{
    private int _AccentOffset;

    //private Rectangle R1;

    private Pen P1;

    private Pen P2;

    private SolidBrush B1;

    //private int Pad;

    public int AccentOffset
    {
        get
        {
            return this._AccentOffset;
        }
        set
        {
            this._AccentOffset = value;
            base.Invalidate();
        }
    }

    public NSTheme_Alt()
    {
        this._AccentOffset = 42;
        base.Header = 30;
        this.BackColor = Color.FromArgb(20, 20, 20);
        this.P1 = new Pen(Color.FromArgb(5, 5, 5));
        this.P2 = new Pen(Color.FromArgb(30, 30, 30));
        this.B1 = new SolidBrush(Color.FromArgb(20, 20, 20));
    }

    protected override void ColorHook()
    {
    }

    protected override void PaintHook()
    {
        this.G.Clear(this.BackColor);
        base.DrawBorders(this.P2, 1);
        base.DrawText(Brushes.Black, HorizontalAlignment.Center, 8, 1);
        base.DrawText(Brushes.Silver, HorizontalAlignment.Center, 7, 0);
        this.G.FillRectangle(this.B1, 0, 27, base.Width, 2);
        base.DrawBorders(Pens.Black);
    }
}

internal class NSListBox : Control
{
    public class NSListBoxItem
    {
        //private string _Text;

        public string Text
        {
            get;
            set;
        }

        public override string ToString()
        {
            return this.Text;
        }
    }

    public enum Title
    {
        Left,
        Center
    }

    private List<NSListBox.NSListBoxItem> _Items;

    private List<NSListBox.NSListBoxItem> _SelectedItems;

    private bool _MultiSelect;

    private int ItemHeight;

    private NSVScrollBar VS;

    //[DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
    //private NSListBox.Title _TitleAlign;

    private Pen P1;

    private Pen P2;

    private Pen P3;

    private SolidBrush B1;

    private SolidBrush B2;

    private SolidBrush B3;

    private SolidBrush B4;

    private LinearGradientBrush GB1;

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public NSListBox.NSListBoxItem[] Items
    {
        get
        {
            return this._Items.ToArray();
        }
        set
        {
            this._Items = new List<NSListBox.NSListBoxItem>(value);
            this.InvalidateScroll();
        }
    }

    public int ItemsCount
    {
        get
        {
            return this._Items.Count;
        }
    }

    public string SelectedItemText
    {
        get
        {
            return this._SelectedItems[0].Text;
        }
    }

    public int SelectedItemIndex
    {
        get
        {
            checked
            {
                int num = this._Items.Count - 1;
                int result;
                for (int i = 0; i <= num; i++)
                {
                    bool flag = String.Compare(this._Items[i].Text, this._SelectedItems[0].Text, false) == 0;
                    if (flag)
                    {
                        result = i;
                        return result;
                    }
                }
                result = 0;
                return result;
            }
        }
    }

    public NSListBox.NSListBoxItem[] SelectedItems
    {
        get
        {
            return this._SelectedItems.ToArray();
        }
    }

    public bool MultiSelect
    {
        get
        {
            return this._MultiSelect;
        }
        set
        {
            this._MultiSelect = value;
            bool flag = this._SelectedItems.Count > 1;
            if (flag)
            {
                this._SelectedItems.RemoveRange(1, checked(this._SelectedItems.Count - 1));
            }
            base.Invalidate();
        }
    }

    public override Font Font
    {
        get
        {
            return base.Font;
        }
        set
        {
            this.ItemHeight = checked((int)Math.Round((double)Graphics.FromHwnd(base.Handle).MeasureString("@", this.Font).Height) + 6);
            bool flag = this.VS != null;
            if (flag)
            {
                this.VS.SmallChange = this.ItemHeight;
                this.VS.LargeChange = this.ItemHeight;
            }
            base.Font = value;
            this.InvalidateLayout();
        }
    }

    public NSListBox.Title TitleAlign
    {
        get;
        set;
    }

    public void AddItem(string Items)
    {
        NSListBox.NSListBoxItem nSListBoxItem = new NSListBox.NSListBoxItem();
        nSListBoxItem.Text = Items;
        this._Items.Add(nSListBoxItem);
        this.InvalidateScroll();
    }

    public void AddItems(string[] Items)
    {
        checked
        {
            for (int i = 0; i < Items.Length; i++)
            {
                string text = Items[i];
                NSListBox.NSListBoxItem nSListBoxItem = new NSListBox.NSListBoxItem();
                nSListBoxItem.Text = text;
                this._Items.Add(nSListBoxItem);
            }
            this.InvalidateScroll();
        }
    }

    public void AddItemAt(int Index, string Items)
    {
        NSListBox.NSListBoxItem nSListBoxItem = new NSListBox.NSListBoxItem();
        nSListBoxItem.Text = Items;
        this._Items.Insert(Index, nSListBoxItem);
        this.InvalidateScroll();
    }

    public void SortItems()
    {
        this._Items.Sort();
    }

    public void RemoveItem(NSListBox.NSListBoxItem Item)
    {
        this._Items.Remove(Item);
        this.InvalidateScroll();
    }

    public void RemoveItems(NSListBox.NSListBoxItem[] Items)
    {
        checked
        {
            for (int i = 0; i < Items.Length; i++)
            {
                NSListBox.NSListBoxItem item = Items[i];
                this._Items.Remove(item);
            }
            this.InvalidateScroll();
        }
    }

    public void RemoveItemAt(int Index)
    {
        this._Items.RemoveAt(Index);
        this.InvalidateScroll();
    }

    public void ClearItems()
    {
        NSListBox.NSListBoxItem[] items = this.Items;
        checked
        {
            for (int i = 0; i < items.Length; i++)
            {
                NSListBox.NSListBoxItem item = items[i];
                this._Items.Remove(item);
            }
            this.InvalidateScroll();
        }
    }

    public NSListBox()
    {
        this._Items = new List<NSListBox.NSListBoxItem>();
        this._SelectedItems = new List<NSListBox.NSListBoxItem>();
        this._MultiSelect = true;
        this.ItemHeight = 24;
        this.TitleAlign = NSListBox.Title.Center;
        base.SetStyle(ControlStyles.UserPaint | ControlStyles.Opaque | ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
        base.SetStyle(ControlStyles.Selectable, true);
        this.BackColor = Color.FromArgb(20, 20, 20);
        this.P1 = new Pen(Color.FromArgb(25, 25, 25));
        this.P2 = new Pen(Color.FromArgb(5, 5, 5));
        this.P3 = new Pen(Color.FromArgb(25, 25, 25));
        this.B1 = new SolidBrush(Color.FromArgb(32, 32, 32));
        this.B2 = new SolidBrush(Color.FromArgb(35, 35, 35));
        this.B3 = new SolidBrush(Color.FromArgb(17, 17, 17));
        this.B4 = new SolidBrush(Color.FromArgb(20, 20, 20));
        this.VS = new NSVScrollBar();
        this.VS.SmallChange = this.ItemHeight;
        this.VS.LargeChange = this.ItemHeight;
        this.VS.Scroll += new NSVScrollBar.ScrollEventHandler(this.HandleScroll);
        this.VS.MouseDown += new MouseEventHandler(this.VS_MouseDown);
        base.Controls.Add(this.VS);
        this.Font = new Font("Verdana", 8f, FontStyle.Regular);
        base.Size = new Size(180, 150);
        this.InvalidateLayout();
    }

    protected override void OnSizeChanged(EventArgs e)
    {
        this.InvalidateLayout();
        base.OnSizeChanged(e);
    }

    private void HandleScroll(object sender)
    {
        base.Invalidate();
    }

    private void InvalidateScroll()
    {
        this.VS.Maximum = checked((int)Math.Round(unchecked((double)(checked(this._Items.Count * this.ItemHeight)) - (double)this.ItemHeight / 2.0)));
        base.Invalidate();
    }

    private void InvalidateLayout()
    {
        checked
        {
            this.VS.Location = new Point(base.Width - this.VS.Width - 1, 1);
            this.VS.Size = new Size(18, base.Height - 2);
            base.Invalidate();
        }
    }

    private void VS_MouseDown(object sender, MouseEventArgs e)
    {
        base.Focus();
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        base.Focus();
        bool flag = e.Button == MouseButtons.Left;
        checked
        {
            if (flag)
            {
                int num = (int)Math.Round(unchecked(this.VS.Percent * (double)(checked(this.VS.Maximum - (base.Height - this.ItemHeight * 2)))));
                int num2 = (e.Y + num - this.ItemHeight) / this.ItemHeight;
                bool flag2 = num2 > this._Items.Count - 1;
                if (flag2)
                {
                    num2 = -1;
                }
                bool flag3 = num2 != -1;
                if (flag3)
                {
                    bool flag4 = Control.ModifierKeys == Keys.Control && this._MultiSelect;
                    if (flag4)
                    {
                        bool flag5 = this._SelectedItems.Contains(this._Items[num2]);
                        if (flag5)
                        {
                            this._SelectedItems.Remove(this._Items[num2]);
                        }
                        else
                        {
                            this._SelectedItems.Add(this._Items[num2]);
                        }
                    }
                    else
                    {
                        this._SelectedItems.Clear();
                        this._SelectedItems.Add(this._Items[num2]);
                    }
                }
                base.Invalidate();
            }
            base.OnMouseDown(e);
        }
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        Graphics graphics = e.Graphics;
        graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
        graphics.Clear(this.BackColor);
        bool flag5;
        checked
        {
            graphics.DrawRectangle(this.P1, 1, 1, base.Width - 3, base.Height - 3);
            int num = (int)Math.Round(unchecked(this.VS.Percent * (double)(checked(this.VS.Maximum - (base.Height - this.ItemHeight * 2)))));
            bool flag = num == 0;
            int num2;
            if (flag)
            {
                num2 = 0;
            }
            else
            {
                num2 = num / this.ItemHeight;
            }
            int num3 = Math.Min(num2 + base.Height / this.ItemHeight, this._Items.Count - 1);
            int num4 = num2;
            int num5 = num3;
            Rectangle rect;
            for (int i = num4; i <= num5; i++)
            {
                NSListBox.NSListBoxItem nSListBoxItem = this.Items[i];
                rect = new Rectangle(0, this.ItemHeight + i * this.ItemHeight + 1 - num, base.Width, this.ItemHeight - 1);
                float height = graphics.MeasureString(nSListBoxItem.Text, this.Font).Height;
                int num6 = rect.Y + (int)Math.Round(unchecked((double)this.ItemHeight / 2.0 - (double)(height / 2f)));
                bool flag2 = this._SelectedItems.Contains(nSListBoxItem);
                if (flag2)
                {
                    bool flag3 = i % 2 == 0;
                    if (flag3)
                    {
                        graphics.FillRectangle(this.B1, rect);
                    }
                    else
                    {
                        graphics.FillRectangle(this.B2, rect);
                    }
                }
                else
                {
                    bool flag4 = i % 2 == 0;
                    if (flag4)
                    {
                        graphics.FillRectangle(this.B3, rect);
                    }
                    else
                    {
                        graphics.FillRectangle(this.B4, rect);
                    }
                }
                graphics.DrawLine(this.P2, 0, rect.Bottom, base.Width, rect.Bottom);
                graphics.DrawString(nSListBoxItem.Text, this.Font, Brushes.Black, 10f, (float)(num6 + 1));
                graphics.DrawString(nSListBoxItem.Text, this.Font, Brushes.White, 9f, (float)num6);
                graphics.ResetClip();
            }
            rect = new Rectangle(0, 0, base.Width, this.ItemHeight);
            this.GB1 = new LinearGradientBrush(rect, Color.FromArgb(20, 20, 20), Color.FromArgb(18, 18, 18), 90f);
            graphics.FillRectangle(this.GB1, rect);
            graphics.DrawRectangle(this.P3, 1, 1, base.Width - 22, this.ItemHeight - 2);
            int num7 = Math.Min(this.VS.Maximum + this.ItemHeight - num, base.Height);
            flag5 = (this.TitleAlign == NSListBox.Title.Left);
        }
        if (flag5)
        {
            graphics.DrawString(this.Text, new Font("verdana", 8f, FontStyle.Regular), Brushes.Black, 5f, 5f);
            graphics.DrawString(this.Text, new Font("verdana", 8f, FontStyle.Regular), Brushes.White, 4f, 4f);
        }
        else
        {
            graphics.DrawString(this.Text, new Font("verdana", 8f, FontStyle.Regular), Brushes.Black, (float)((double)(checked(base.Width - this.VS.Width)) / 2.0 - (double)(checked(this.Text.Length * 2))), 5f);
            graphics.DrawString(this.Text, new Font("verdana", 8f, FontStyle.Regular), Brushes.White, (float)((double)(checked(base.Width - this.VS.Width)) / 2.0 - (double)(checked(this.Text.Length * 2))), 4f);
        }
        checked
        {
            graphics.DrawRectangle(this.P2, 0, 0, base.Width - 1, base.Height - 1);
            graphics.DrawLine(this.P2, 0, this.ItemHeight, base.Width, this.ItemHeight);
            graphics.DrawLine(this.P2, this.VS.Location.X - 1, 0, this.VS.Location.X - 1, base.Height);
        }
    }

    protected override void OnMouseWheel(MouseEventArgs e)
    {
        checked
        {
            int num = 0 - e.Delta * SystemInformation.MouseWheelScrollLines / 120 * (this.ItemHeight / 2);
            int value = Math.Max(Math.Min(this.VS.Value + num, this.VS.Maximum), this.VS.Minimum);
            this.VS.Value = value;
            base.OnMouseWheel(e);
        }
    }
}


class NSSeparator_alt : ThemeControl154
{
    private Color Accent;

    private Color Border;

    public NSSeparator_alt()
    {
        base.SetColor("Border", Color.Black);
        base.SetColor("Accent", 1, 135, 199);
        base.LockHeight = 5;
    }

    protected override void ColorHook()
    {
        this.Border = base.GetColor("Border");
        this.Accent = base.GetColor("Accent");
    }

    protected override void PaintHook()
    {
        this.G.Clear(this.BackColor);
        Point point = new Point(0, 0);
        Point point2 = new Point(base.Width, 0);
        this.G.DrawLine(new Pen(Color.FromArgb(10, Color.White)), point, point2);
        point2 = new Point(0, 1);
        point = new Point(base.Width, 1);
        this.G.DrawLine(new Pen(this.Border), point2, point);
        ColorBlend colorBlend = new ColorBlend(3);
        colorBlend.Colors = new Color[]
        {
                Color.Black,
                this.Accent,
                Color.Black
        };
        colorBlend.Positions = new float[]
        {
                0f,
                0.5f,
                1f
        };
        Rectangle r = new Rectangle(1, 2, checked(base.Width - 2), 2);
        base.DrawGradient(colorBlend, r, 0f);
        point2 = new Point(0, 4);
        point = new Point(base.Width, 4);
        this.G.DrawLine(new Pen(Color.FromArgb(10, Color.White)), point2, point);
    }
}



class NSSeparatorHorizontal : ThemeControl154
{
    public NSSeparatorHorizontal()
    {
        base.LockHeight = 10;
    }

    protected override void ColorHook()
    {
    }

    protected override void PaintHook()
    {
        this.G.Clear(Color.FromArgb(20, 20, 20));
        checked
        {
            this.G.DrawLine(new Pen(Color.FromArgb(6, 6, 6)), 0, 4, base.Width - 1, 4);
            this.G.DrawLine(new Pen(Color.FromArgb(32, 32, 32)), 0, 5, base.Width - 1, 5);
        }
    }
}



class NSSeparatorVertical : ThemeControl154
{
    public NSSeparatorVertical()
    {
        base.LockWidth = 10;
    }

    protected override void ColorHook()
    {
    }

    protected override void PaintHook()
    {
        this.G.Clear(Color.FromArgb(20, 20, 20));
        checked
        {
            this.G.FillRectangle(new SolidBrush(Color.FromArgb(6, 6, 6)), new Rectangle(4, 0, 1, base.Height - 1));
            this.G.FillRectangle(new SolidBrush(Color.FromArgb(32, 32, 32)), new Rectangle(5, 0, 1, base.Height - 1));
        }
    }
}

class NSTheme : ThemeContainer154
{

    private int _AccentOffset = 0;
    private bool _ShowIcon;

    private Color Accent;

    private Color Border;

    private Color TextColor;

    private Color TitleBottom;

    private Color TitleTop;
    public int AccentOffset
    {
        get { return _AccentOffset; }
        set
        {
            _AccentOffset = value;
            Invalidate();
        }
    }

    public bool ShowIcon
    {
        get
        {
            return this._ShowIcon;
        }
        set
        {
            this._ShowIcon = value;
            base.Invalidate();
        }
    }

    public NSTheme()
    {
        Header = 30;
        SetColor("Titlebar Gradient Top", 20, 20, 20);
        SetColor("Titlebar Gradient Bottom", 22, 22, 22);
        SetColor("Text", 170, 170, 170);
        SetColor("Accent", 1, 135, 199);
        SetColor("Border", Color.Black);
        TransparencyKey = Color.Fuchsia;
        Padding = new Padding(10, 45, 10, 10);
        BackColor = Color.FromArgb(20, 20, 20);
    }


    protected override void ColorHook()
    {
        this.TitleTop = base.GetColor("Titlebar Gradient Top");
        this.TitleBottom = base.GetColor("Titlebar Gradient Bottom");
        this.TextColor = base.GetColor("Text");
        this.Accent = base.GetColor("Accent");
        this.Border = base.GetColor("Border");
    }

    /*
    private Rectangle R1;
    private Pen P1;
    private Pen P2;

    private SolidBrush B1;

    private int Pad;
    */
    protected override void PaintHook()
    {
        this.G.Clear(this.Border);
        checked
        {
            Rectangle rectangle = new Rectangle(1, 1, base.Width - 2, 35);
            LinearGradientBrush brush = new LinearGradientBrush(rectangle, this.TitleTop, this.TitleBottom, 90f);
            this.G.FillPath(brush, base.CreateRound(1, 1, base.Width - 2, 35, 7));
            this.G.DrawPath(new Pen(Color.FromArgb(15, Color.White), 1f), base.CreateRound(1, 1, base.Width - 3, 35, 7));
            this.G.FillPath(new SolidBrush(this.BackColor), base.CreateRound(1, 32, base.Width - 2, base.Height - 33, 7));
            this.G.DrawPath(new Pen(Color.FromArgb(15, Color.White), 1f), base.CreateRound(1, 32, base.Width - 3, base.Height - 34, 7));
            rectangle = new Rectangle(1, 32, base.Width - 2, 3);
            this.G.FillRectangle(new SolidBrush(this.Border), rectangle);
            Point point = new Point(1, 31);
            Point point2 = new Point(base.Width - 2, 31);
            this.G.DrawLine(new Pen(Color.FromArgb(15, Color.White)), point, point2);
            ColorBlend colorBlend = new ColorBlend(3);
            colorBlend.Colors = new Color[]
            {
                    Color.Black,
                    this.Accent,
                    Color.Black
            };
            colorBlend.Positions = new float[]
            {
                    0f,
                    0.5f,
                    1f
            };
            rectangle = new Rectangle(1, 33, base.Width - 2, 2);
            base.DrawGradient(colorBlend, rectangle, 0f);
            point2 = new Point(1, 35);
            point = new Point(base.Width - 2, 35);
            this.G.DrawLine(new Pen(this.BackColor), point2, point);
            point2 = new Point(1, 35);
            point = new Point(base.Width - 2, 35);
            this.G.DrawLine(new Pen(Color.FromArgb(15, Color.White)), point2, point);
            bool showIcon = this._ShowIcon;
            if (showIcon)
            {
                rectangle = new Rectangle(11, 8, 16, 16);
                this.G.DrawIcon(base.FindForm().Icon, rectangle);
                point2 = new Point(32, 8);
                this.G.DrawString(base.FindForm().Text, this.Font, new SolidBrush(this.TextColor), point2);
            }
            else
            {
                point2 = new Point(13, 8);
                this.G.DrawString(base.FindForm().Text, this.Font, new SolidBrush(this.TextColor), point2);
            }
            base.DrawPixel(Color.Fuchsia, 0, 0);
            base.DrawPixel(Color.Fuchsia, 1, 0);
            base.DrawPixel(Color.Fuchsia, 2, 0);
            base.DrawPixel(Color.Fuchsia, 3, 0);
            base.DrawPixel(Color.Fuchsia, 0, 1);
            base.DrawPixel(Color.Fuchsia, 0, 2);
            base.DrawPixel(Color.Fuchsia, 0, 3);
            base.DrawPixel(Color.Fuchsia, 1, 1);
            base.DrawPixel(Color.Fuchsia, base.Width - 1, 0);
            base.DrawPixel(Color.Fuchsia, base.Width - 2, 0);
            base.DrawPixel(Color.Fuchsia, base.Width - 3, 0);
            base.DrawPixel(Color.Fuchsia, base.Width - 4, 0);
            base.DrawPixel(Color.Fuchsia, base.Width - 1, 1);
            base.DrawPixel(Color.Fuchsia, base.Width - 1, 2);
            base.DrawPixel(Color.Fuchsia, base.Width - 1, 3);
            base.DrawPixel(Color.Fuchsia, base.Width - 2, 1);
            base.DrawPixel(Color.Fuchsia, 0, base.Height);
            base.DrawPixel(Color.Fuchsia, 1, base.Height);
            base.DrawPixel(Color.Fuchsia, 2, base.Height);
            base.DrawPixel(Color.Fuchsia, 3, base.Height);
            base.DrawPixel(Color.Fuchsia, 0, base.Height - 1);
            base.DrawPixel(Color.Fuchsia, 0, base.Height - 2);
            base.DrawPixel(Color.Fuchsia, 0, base.Height - 3);
            base.DrawPixel(Color.Fuchsia, 1, base.Height - 1);
            base.DrawPixel(Color.Fuchsia, base.Width - 1, base.Height);
            base.DrawPixel(Color.Fuchsia, base.Width - 2, base.Height);
            base.DrawPixel(Color.Fuchsia, base.Width - 3, base.Height);
            base.DrawPixel(Color.Fuchsia, base.Width - 4, base.Height);
            base.DrawPixel(Color.Fuchsia, base.Width - 1, base.Height - 1);
            base.DrawPixel(Color.Fuchsia, base.Width - 1, base.Height - 2);
            base.DrawPixel(Color.Fuchsia, base.Width - 1, base.Height - 3);
            base.DrawPixel(Color.Fuchsia, base.Width - 2, base.Height - 1);
        }
    }

}


class NSTabcontrol_VC : TabControl
{
    private Pen Border;

    public NSTabcontrol_VC()
    {
        this.Border = Pens.Black;
        base.SetStyle(ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer, true);
        this.DoubleBuffered = true;
        base.SizeMode = TabSizeMode.Fixed;
        Size itemSize = new Size(44, 136);
        base.ItemSize = itemSize;
    }

    protected override void CreateHandle()
    {
        base.CreateHandle();
        base.Alignment = TabAlignment.Left;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        Bitmap bitmap = new Bitmap(base.Width, base.Height);
        Graphics graphics = Graphics.FromImage(bitmap);
        try
        {
            base.SelectedTab.BackColor = Color.FromArgb(20, 20, 20);
        }
        catch (Exception)
        {
            //ProjectData.SetProjectError(arg_37_0);
            //ProjectData.ClearProjectError();
        }
        graphics.Clear(Color.FromArgb(20, 20, 20));
        checked
        {
            Point location = new Point(base.ItemSize.Height + 3, 0);
            Point location2 = new Point(base.ItemSize.Height + 3, 999);
            graphics.DrawLine(this.Border, location, location2);
            Size itemSize = base.ItemSize;
            location2 = new Point(itemSize.Height + 2, 0);
            location = new Point(base.ItemSize.Height + 2, 999);
            graphics.DrawLine(new Pen(Color.FromArgb(15, Color.White)), location2, location);
            Rectangle rect = new Rectangle(0, 0, base.Width - 1, base.Height - 1);
            graphics.DrawRectangle(this.Border, rect);
            rect = new Rectangle(1, 1, base.Width - 3, base.Height - 3);
            graphics.DrawRectangle(new Pen(Color.FromArgb(15, Color.White)), rect);
            int num = base.TabCount - 1;
            int num2 = 0;
            while (true)
            {
                bool flag = num2 > num;
                if (flag)
                {
                    break;
                }
                bool flag2 = num2 == base.SelectedIndex;
                if (flag2)
                {
                    bool flag3 = num2 == -1;
                    Point location3;
                    Rectangle rectangle;
                    Rectangle tabRect;
                    if (flag3)
                    {
                        location = base.GetTabRect(num2).Location;
                        location3 = new Point(base.GetTabRect(num2).Location.X - 2, location.Y - 2);
                        itemSize = new Size(base.GetTabRect(num2).Width + 3, base.GetTabRect(num2).Height + 1);
                        rectangle = new Rectangle(location3, itemSize);
                    }
                    else
                    {
                        tabRect = base.GetTabRect(num2);
                        location = new Point(tabRect.Location.X - 2, base.GetTabRect(num2).Location.Y - 2);
                        itemSize = new Size(base.GetTabRect(num2).Width + 3, base.GetTabRect(num2).Height);
                        rectangle = new Rectangle(location, itemSize);
                    }
                    ColorBlend colorBlend = new ColorBlend();
                    colorBlend.Colors = new Color[]
                    {
                            Color.FromArgb(24, 24, 24),
                            Color.FromArgb(20, 20, 20),
                            Color.FromArgb(14, 14, 14)
                    };
                    colorBlend.Positions = new float[]
                    {
                            0f,
                            0.5f,
                            1f
                    };
                    LinearGradientBrush brush = new LinearGradientBrush(rectangle, Color.Black, Color.Black, 90f)
                    {
                        InterpolationColors = colorBlend
                    };
                    graphics.FillRectangle(brush, rectangle);
                    graphics.DrawRectangle(this.Border, rectangle);
                    tabRect = new Rectangle(rectangle.Location.X + 1, rectangle.Location.Y + 1, rectangle.Width - 2, rectangle.Height - 2);
                    graphics.DrawRectangle(new Pen(Color.FromArgb(15, Color.White)), tabRect);
                    location2 = base.GetTabRect(num2).Location;
                    location = new Point(base.GetTabRect(num2).Location.X - 2, location2.Y - 2);
                    itemSize = new Size(base.GetTabRect(num2).Width + 3, base.GetTabRect(num2).Height + 1);
                    rectangle = new Rectangle(location, itemSize);
                    graphics.SmoothingMode = SmoothingMode.HighQuality;
                    Point[] array = new Point[3];
                    itemSize = base.ItemSize;
                    tabRect = base.GetTabRect(num2);
                    location3 = tabRect.Location;
                    location2 = new Point(itemSize.Height - 3, location3.Y + 20);
                    array[0] = location2;
                    location = base.GetTabRect(num2).Location;
                    Point point = new Point(base.ItemSize.Height + 4, location.Y + 14);
                    array[1] = point;
                    Size itemSize2 = base.ItemSize;
                    Point location4 = new Point(itemSize2.Height + 4, base.GetTabRect(num2).Location.Y + 27);
                    array[2] = location4;
                    Point[] points = array;
                    graphics.DrawPolygon(new Pen(Color.FromArgb(15, Color.White), 3f), points);
                    graphics.FillPolygon(new SolidBrush(Color.FromArgb(1, 135, 199)), points);
                    graphics.DrawPolygon(this.Border, points);
                    bool flag4 = base.ImageList != null;
                    StringFormat format;
                    if (flag4)
                    {
                        try
                        {
                            bool flag5 = base.ImageList.Images[base.TabPages[num2].ImageIndex] != null;
                            if (flag5)
                            {
                                location4 = rectangle.Location;
                                point = new Point(location4.X + 8, rectangle.Location.Y + 6);
                                graphics.DrawImage(base.ImageList.Images[base.TabPages[num2].ImageIndex], point);
                                format = new StringFormat
                                {
                                    LineAlignment = StringAlignment.Center,
                                    Alignment = StringAlignment.Center
                                };
                                graphics.DrawString("      " + base.TabPages[num2].Text, this.Font, Brushes.DimGray, rectangle, format);
                            }
                            else
                            {
                                format = new StringFormat
                                {
                                    LineAlignment = StringAlignment.Center,
                                    Alignment = StringAlignment.Center
                                };
                                graphics.DrawString(base.TabPages[num2].Text, this.Font, Brushes.White, rectangle, format);
                            }
                            goto IL_A1F;
                        }
                        catch (Exception)
                        {
                            //ProjectData.SetProjectError(arg_66C_0);
                            format = new StringFormat
                            {
                                LineAlignment = StringAlignment.Center,
                                Alignment = StringAlignment.Center
                            };
                            graphics.DrawString(base.TabPages[num2].Text, this.Font, Brushes.White, rectangle, format);
                            //ProjectData.ClearProjectError();
                            goto IL_A1F;
                        }
                    }
                    format = new StringFormat
                    {
                        LineAlignment = StringAlignment.Center,
                        Alignment = StringAlignment.Center
                    };
                    graphics.DrawString(base.TabPages[num2].Text, this.Font, Brushes.White, rectangle, format);
                }
                else
                {
                    Rectangle tabRect = base.GetTabRect(num2);
                    Point location4 = tabRect.Location;
                    Point location5 = base.GetTabRect(num2).Location;
                    Point point = new Point(location4.X - 1, location5.Y - 1);
                    Size itemSize2 = new Size(base.GetTabRect(num2).Width + 2, base.GetTabRect(num2).Height);
                    Rectangle r = new Rectangle(point, itemSize2);
                    location4 = new Point(r.Right, r.Top);
                    location5 = new Point(r.Right, r.Bottom);
                    graphics.DrawLine(this.Border, location4, location5);
                    location4 = new Point(r.Right - 1, r.Top);
                    location5 = new Point(r.Right - 1, r.Bottom);
                    graphics.DrawLine(new Pen(Color.FromArgb(43, 43, 43)), location4, location5);
                    bool flag6 = base.ImageList != null;
                    StringFormat format;
                    if (flag6)
                    {
                        try
                        {
                            bool flag7 = base.ImageList.Images[base.TabPages[num2].ImageIndex] != null;
                            if (flag7)
                            {
                                location4 = r.Location;
                                point = new Point(location4.X + 8, r.Location.Y + 6);
                                graphics.DrawImage(base.ImageList.Images[base.TabPages[num2].ImageIndex], point);
                                format = new StringFormat
                                {
                                    LineAlignment = StringAlignment.Center,
                                    Alignment = StringAlignment.Center
                                };
                                graphics.DrawString("      " + base.TabPages[num2].Text, this.Font, new SolidBrush(Color.FromArgb(170, 170, 170)), r, format);
                            }
                            else
                            {
                                format = new StringFormat
                                {
                                    LineAlignment = StringAlignment.Center,
                                    Alignment = StringAlignment.Center
                                };
                                graphics.DrawString(base.TabPages[num2].Text, this.Font, new SolidBrush(Color.FromArgb(170, 170, 170)), r, format);
                            }
                            goto IL_A1F;
                        }
                        catch (Exception)
                        {
                            //ProjectData.SetProjectError(arg_95E_0);
                            format = new StringFormat
                            {
                                LineAlignment = StringAlignment.Center,
                                Alignment = StringAlignment.Center
                            };
                            graphics.DrawString(base.TabPages[num2].Text, this.Font, new SolidBrush(Color.FromArgb(170, 170, 170)), r, format);
                            //ProjectData.ClearProjectError();
                            goto IL_A1F;
                        }
                    }
                    format = new StringFormat
                    {
                        LineAlignment = StringAlignment.Center,
                        Alignment = StringAlignment.Center
                    };
                    graphics.DrawString(base.TabPages[num2].Text, this.Font, new SolidBrush(Color.FromArgb(170, 170, 170)), r, format);
                }
            IL_A1F:
                num2++;
            }
            e.Graphics.DrawImage((Image)bitmap.Clone(), 0, 0);
            graphics.Dispose();
            bitmap.Dispose();
        }
    }

    public Brush ToBrush(Color color)
    {
        return new SolidBrush(color);
    }

    public Pen ToPen(Color color)
    {
        return new Pen(color);
    }
}

class NSGroupBox_alt : ThemeContainer154
{
    private Color Bz;

    private Color G1;

    private Color G2;

    private Color TC;

    public NSGroupBox_alt()
    {
        base.ControlMode = true;
        base.SetColor("Gradient Top", 28, 28, 28);
        base.SetColor("Gradient Bottom", 20, 20, 20);
        base.SetColor("Text", 170, 170, 170);
        base.SetColor("Border", Color.Black);
    }

    protected override void ColorHook()
    {
        this.G1 = base.GetColor("Gradient Top");
        this.G2 = base.GetColor("Gradient Bottom");
        this.TC = base.GetColor("Text");
        this.Bz = base.GetColor("Border");
    }

    protected override void PaintHook()
    {
        this.G.Clear(this.BackColor);
        this.G.SmoothingMode = SmoothingMode.HighQuality;
        checked
        {
            this.G.DrawPath(new Pen(this.Bz), base.CreateRound(0, 0, base.Width - 1, base.Height - 1, 7));
            Rectangle rect = new Rectangle(0, 0, base.Width - 1, 27);
            LinearGradientBrush brush = new LinearGradientBrush(rect, this.G1, this.G2, 90f);
            this.G.FillPath(brush, base.CreateRound(0, 0, base.Width - 1, 27, 7));
            this.G.DrawPath(new Pen(this.Bz), base.CreateRound(0, 0, base.Width - 1, 27, 7));
            this.G.SmoothingMode = SmoothingMode.None;
            rect = new Rectangle(1, 24, base.Width - 2, 10);
            this.G.FillRectangle(new SolidBrush(this.BackColor), rect);
            Point point = new Point(0, 24);
            Point point2 = new Point(base.Width, 24);
            this.G.DrawLine(new Pen(this.Bz), point, point2);
            point2 = new Point(2, 23);
            point = new Point(base.Width - 3, 23);
            this.G.DrawLine(new Pen(Color.FromArgb(15, Color.White)), point2, point);
            point2 = new Point(7, 5);
            this.G.DrawString(this.Text, this.Font, new SolidBrush(this.TC), point2);
            this.G.SmoothingMode = SmoothingMode.HighQuality;
            this.G.DrawPath(new Pen(Color.FromArgb(15, Color.White)), base.CreateRound(1, 1, base.Width - 3, base.Height - 3, 7));
        }
    }
}

class NSBoxPanel : ContainerControl
{
    private GraphicsPath GP1;

    private GraphicsPath GP2;

    private Pen P1;

    private Pen P2;

    //private SolidBrush B1;

    public NSBoxPanel()
    {
        base.SetStyle(ControlStyles.UserPaint | ControlStyles.Opaque | ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
        base.SetStyle(ControlStyles.Selectable, false);
        this.P1 = new Pen(Color.FromArgb(5, 5, 5));
        this.P2 = new Pen(Color.FromArgb(25, 25, 25));
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        ThemeModule.G = e.Graphics;
        ThemeModule.G.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
        ThemeModule.G.Clear(this.BackColor);
        ThemeModule.G.SmoothingMode = SmoothingMode.AntiAlias;
        checked
        {
            this.GP1 = ThemeModule.CreateRound(0, 0, base.Width - 1, base.Height - 1, 7);
            this.GP2 = ThemeModule.CreateRound(1, 1, base.Width - 3, base.Height - 3, 7);
            ThemeModule.G.DrawPath(this.P1, this.GP1);
            ThemeModule.G.DrawPath(this.P2, this.GP2);
        }
    }
}

class NSButton : Control
{

    public NSButton()
    {
        base.SetStyle(ControlStyles.UserPaint | ControlStyles.Opaque | ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
        base.SetStyle(ControlStyles.Selectable, false);
        this.P1 = new Pen(Color.FromArgb(5, 5, 5));
        this.P2 = new Pen(Color.FromArgb(35, 35, 35));
    }


    private bool IsMouseDown;
    private GraphicsPath GP1;

    private GraphicsPath GP2;
    private SizeF SZ1;

    private PointF PT1;
    private Pen P1;

    private Pen P2;
    private PathGradientBrush PB1;

    private LinearGradientBrush GB1;

    //private Graphics G;
    protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
	{
        ThemeModule.G = e.Graphics;
        ThemeModule.G.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
        ThemeModule.G.Clear(this.BackColor);
        ThemeModule.G.SmoothingMode = SmoothingMode.AntiAlias;
        checked
        {
            this.GP1 = ThemeModule.CreateRound(0, 0, base.Width - 1, base.Height - 1, 7);
            this.GP2 = ThemeModule.CreateRound(1, 1, base.Width - 3, base.Height - 3, 7);
            bool isMouseDown = this.IsMouseDown;
            if (isMouseDown)
            {
                this.PB1 = new PathGradientBrush(this.GP1);
                this.PB1.CenterColor = Color.FromArgb(30, 30, 30);
                this.PB1.SurroundColors = new Color[]
                {
                        Color.FromArgb(25, 25, 25)
                };
                this.PB1.FocusScales = new PointF(0.8f, 0.5f);
                ThemeModule.G.FillPath(this.PB1, this.GP1);
            }
            else
            {
                this.GB1 = new LinearGradientBrush(base.ClientRectangle, Color.FromArgb(30, 30, 30), Color.FromArgb(25, 25, 25), 90f);
                ThemeModule.G.FillPath(this.GB1, this.GP1);
            }
            ThemeModule.G.DrawPath(this.P1, this.GP1);
            ThemeModule.G.DrawPath(this.P2, this.GP2);
            this.SZ1 = ThemeModule.G.MeasureString(this.Text, this.Font);
        }
        this.PT1 = new PointF(5f, (float)(base.Height / 2) - this.SZ1.Height / 2f);
        bool isMouseDown2 = this.IsMouseDown;
        if (isMouseDown2)
        {
            this.PT1.X = this.PT1.X + 1f;
            this.PT1.Y = this.PT1.Y + 1f;
        }
        ThemeModule.G.DrawString(this.Text, this.Font, Brushes.Black, this.PT1.X + 1f, this.PT1.Y + 1f);
        ThemeModule.G.DrawString(this.Text, this.Font, Brushes.White, this.PT1);
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        IsMouseDown = true;
        Invalidate();
    }

    protected override void OnMouseUp(MouseEventArgs e)
    {
        IsMouseDown = false;
        Invalidate();
    }

}
internal class NSButton_alt : ThemeControl154
{
    private Color G1;

    private Color G2;

    private Color TC;

    public NSButton_alt()
    {
        base.SetColor("Gradient Top", 40, 40, 40);
        base.SetColor("Gradient Bottom", 20, 20, 20);
        base.SetColor("Text", 170, 170, 170);
    }

    protected override void ColorHook()
    {
        this.G1 = base.GetColor("Gradient Top");
        this.G2 = base.GetColor("Gradient Bottom");
        this.TC = base.GetColor("Text");
    }

    protected override void PaintHook()
    {
        this.G.Clear(this.BackColor);
        this.G.SmoothingMode = SmoothingMode.HighQuality;
        checked
        {
            Rectangle rectangle;
            switch (this.State)
            {
                case MouseState.None:
                    {
                        rectangle = new Rectangle(0, 0, base.Width - 1, base.Height - 1);
                        LinearGradientBrush brush = new LinearGradientBrush(rectangle, this.G1, this.G2, 90f);
                        this.G.FillPath(brush, base.CreateRound(0, 0, base.Width - 1, base.Height - 1, 5));
                        this.G.DrawPath(Pens.Black, base.CreateRound(0, 0, base.Width - 1, base.Height - 1, 5));
                        this.G.DrawPath(new Pen(Color.FromArgb(15, Color.White)), base.CreateRound(1, 1, base.Width - 3, base.Height - 3, 5));
                        break;
                    }
                case MouseState.Over:
                    {
                        rectangle = new Rectangle(0, 0, base.Width - 1, base.Height - 1);
                        LinearGradientBrush brush2 = new LinearGradientBrush(rectangle, this.G1, this.G2, 90f);
                        this.G.FillPath(brush2, base.CreateRound(0, 0, base.Width - 1, base.Height - 1, 5));
                        this.G.FillPath(new SolidBrush(Color.FromArgb(7, Color.White)), base.CreateRound(0, 0, base.Width - 1, base.Height - 1, 5));
                        this.G.DrawPath(Pens.Black, base.CreateRound(0, 0, base.Width - 1, base.Height - 1, 5));
                        this.G.DrawPath(new Pen(Color.FromArgb(15, Color.White)), base.CreateRound(1, 1, base.Width - 3, base.Height - 3, 5));
                        break;
                    }
                case MouseState.Down:
                    {
                        rectangle = new Rectangle(0, 0, base.Width - 1, base.Height - 1);
                        LinearGradientBrush brush3 = new LinearGradientBrush(rectangle, this.G1, this.G2, 90f);
                        this.G.FillPath(brush3, base.CreateRound(0, 0, base.Width - 1, base.Height - 1, 5));
                        this.G.FillPath(new SolidBrush(Color.FromArgb(20, Color.Black)), base.CreateRound(0, 0, base.Width - 1, base.Height - 1, 5));
                        this.G.DrawPath(Pens.Black, base.CreateRound(0, 0, base.Width - 1, base.Height - 1, 5));
                        this.G.DrawPath(new Pen(Color.FromArgb(15, Color.White)), base.CreateRound(1, 1, base.Width - 3, base.Height - 3, 5));
                        break;
                    }
            }
            rectangle = new Rectangle(0, 0, base.Width - 1, base.Height);
            StringFormat format = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };
            this.G.DrawString(this.Text, this.Font, new SolidBrush(this.TC), rectangle, format);
        }
    }
}
internal class NSProgressBar : Control
{
    private int _Minimum;

    private int _Maximum;

    private int _Value;

    private GraphicsPath GP1;

    private GraphicsPath GP2;

    private GraphicsPath GP3;

    private Rectangle R1;

    private Rectangle R2;

    private Pen P1;

    private Pen P2;

    private SolidBrush B1;

    private LinearGradientBrush GB1;

    private LinearGradientBrush GB2;

    private int I1;

    public int Minimum
    {
        get
        {
            return this._Minimum;
        }
        set
        {
            bool flag = value < 0;
            if (flag)
            {
                throw new Exception("Property value is not valid.");
            }
            this._Minimum = value;
            bool flag2 = value > this._Value;
            if (flag2)
            {
                this._Value = value;
            }
            bool flag3 = value > this._Maximum;
            if (flag3)
            {
                this._Maximum = value;
            }
            base.Invalidate();
        }
    }

    public int Maximum
    {
        get
        {
            return this._Maximum;
        }
        set
        {
            bool flag = value < 0;
            if (flag)
            {
                throw new Exception("Property value is not valid.");
            }
            this._Maximum = value;
            bool flag2 = value < this._Value;
            if (flag2)
            {
                this._Value = value;
            }
            bool flag3 = value < this._Minimum;
            if (flag3)
            {
                this._Minimum = value;
            }
            base.Invalidate();
        }
    }

    public int Value
    {
        get
        {
            return this._Value;
        }
        set
        {
            bool flag = value > this._Maximum || value < this._Minimum;
            if (flag)
            {
                throw new Exception("Property value is not valid.");
            }
            this._Value = value;
            base.Invalidate();
        }
    }

    private void Increment(int amount)
    {
        checked
        {
            this.Value += amount;
        }
    }

    public NSProgressBar()
    {
        this._Maximum = 100;
        base.SetStyle(ControlStyles.UserPaint | ControlStyles.Opaque | ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
        base.SetStyle(ControlStyles.Selectable, false);
        this.P1 = new Pen(Color.FromArgb(5, 5, 5));
        this.P2 = new Pen(Color.FromArgb(25, 25, 25));
        this.B1 = new SolidBrush(Color.FromArgb(1, 135, 199));
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        ThemeModule.G = e.Graphics;
        ThemeModule.G.Clear(this.BackColor);
        ThemeModule.G.SmoothingMode = SmoothingMode.AntiAlias;
        checked
        {
            this.GP1 = ThemeModule.CreateRound(0, 0, base.Width - 1, base.Height - 1, 7);
            this.GP2 = ThemeModule.CreateRound(1, 1, base.Width - 3, base.Height - 3, 7);
            this.R1 = new Rectangle(0, 2, base.Width - 1, base.Height - 1);
            this.GB1 = new LinearGradientBrush(this.R1, Color.FromArgb(15, 15, 15), Color.FromArgb(20, 20, 20), 90f);
            ThemeModule.G.SetClip(this.GP1);
            ThemeModule.G.FillRectangle(this.GB1, this.R1);
            this.I1 = (int)Math.Round(unchecked(checked((double)(this._Value - this._Minimum) / (double)(this._Maximum - this._Minimum)) * (double)(checked(base.Width - 3))));
            bool flag = this.I1 > 1;
            if (flag)
            {
                this.GP3 = ThemeModule.CreateRound(1, 1, this.I1, base.Height - 3, 7);
                this.R2 = new Rectangle(1, 1, this.I1, base.Height - 3);
                this.GB2 = new LinearGradientBrush(this.R2, Color.FromArgb(13, 152, 255), Color.FromArgb(1, 73, 139), 90f);
                ThemeModule.G.FillPath(this.GB2, this.GP3);
                ThemeModule.G.DrawPath(this.P1, this.GP3);
                ThemeModule.G.SetClip(this.GP3);
                ThemeModule.G.SmoothingMode = SmoothingMode.None;
                ThemeModule.G.FillRectangle(this.B1, this.R2.X, this.R2.Y + 1, this.R2.Width, this.R2.Height / 2);
                ThemeModule.G.SmoothingMode = SmoothingMode.AntiAlias;
                ThemeModule.G.ResetClip();
            }
            ThemeModule.G.DrawPath(this.P2, this.GP1);
            ThemeModule.G.DrawPath(this.P1, this.GP2);
        }
    }
}

class NSLabel : Control
{

    public NSLabel()
    {

        base.SetStyle(ControlStyles.UserPaint | ControlStyles.Opaque | ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
        base.SetStyle(ControlStyles.Selectable, false);
        Font = new Font("Verdana", 10.25f, FontStyle.Bold);
        B1 = new SolidBrush(Color.FromArgb(13, 152, 255));
    }

    private string _Value1 = "NET";
    public string Value1
    {
        get { return _Value1; }
        set
        {
            _Value1 = value;
            Invalidate();
        }
    }

    private string _Value2 = "SEAL";
    public string Value2
    {
        get { return _Value2; }
        set
        {
            _Value2 = value;
            Invalidate();
        }
    }


    private SolidBrush B1;
    private PointF PT1;
    private PointF PT2;
    private SizeF SZ1;

    private SizeF SZ2;
    private Graphics G;

    protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
    {
        G = e.Graphics;
        G.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

        G.Clear(BackColor);

        SZ1 = G.MeasureString(Value1, Font, Width, StringFormat.GenericTypographic);
        SZ2 = G.MeasureString(Value2, Font, Width, StringFormat.GenericTypographic);

        PT1 = new PointF(0, Height / 2 - SZ1.Height / 2);
        PT2 = new PointF(SZ1.Width + 1, Height / 2 - SZ1.Height / 2);

        G.DrawString(Value1, Font, Brushes.Black, PT1.X + 1, PT1.Y + 1);
        G.DrawString(Value1, Font, Brushes.White, PT1);

        G.DrawString(Value2, Font, Brushes.Black, PT2.X + 1, PT2.Y + 1);
        G.DrawString(Value2, Font, B1, PT2);
    }

}

[DefaultEvent("TextChanged")]
class NSTextBox : Control
{

    private HorizontalAlignment _TextAlign = HorizontalAlignment.Left;
    public HorizontalAlignment TextAlign
    {
        get { return _TextAlign; }
        set
        {
            _TextAlign = value;
            if (Base != null)
            {
                Base.TextAlign = value;
            }
        }
    }

    private int _MaxLength = 32767;
    public int MaxLength
    {
        get { return _MaxLength; }
        set
        {
            _MaxLength = value;
            if (Base != null)
            {
                Base.MaxLength = value;
            }
        }
    }

    private bool _ReadOnly;
    public bool ReadOnly
    {
        get { return _ReadOnly; }
        set
        {
            _ReadOnly = value;
            if (Base != null)
            {
                Base.ReadOnly = value;
            }
        }
    }

    private bool _UseSystemPasswordChar;
    public bool UseSystemPasswordChar
    {
        get { return _UseSystemPasswordChar; }
        set
        {
            _UseSystemPasswordChar = value;
            if (Base != null)
            {
                Base.UseSystemPasswordChar = value;
            }
        }
    }

    private bool _Multiline;
    public bool Multiline
    {
        get { return _Multiline; }
        set
        {
            _Multiline = value;
            if (Base != null)
            {
                Base.Multiline = value;

                if (value)
                {
                    Base.Height = Height - 11;
                }
                else
                {
                    Height = Base.Height + 11;
                }
            }
        }
    }

    public override string Text
    {
        get { return base.Text; }
        set
        {
            base.Text = value;
            if (Base != null)
            {
                Base.Text = value;
            }
        }
    }

    public override Font Font
    {
        get { return base.Font; }
        set
        {
            base.Font = value;
            if (Base != null)
            {
                Base.Font = value;
                Base.Location = new Point(5, 5);
                Base.Width = Width - 8;

                if (!_Multiline)
                {
                    Height = Base.Height + 11;
                }
            }
        }
    }

    protected override void OnHandleCreated(EventArgs e)
    {
        if (!Controls.Contains(Base))
        {
            Controls.Add(Base);
        }

        base.OnHandleCreated(e);
    }

    private TextBox Base;
    public NSTextBox()
    {
        this._TextAlign = HorizontalAlignment.Left;
        this._MaxLength = 32767;
        base.SetStyle(ControlStyles.UserPaint | ControlStyles.Opaque | ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
        base.SetStyle(ControlStyles.Selectable, true);
        this.Cursor = Cursors.IBeam;
        this.Base = new TextBox();
        this.Base.Font = this.Font;
        this.Base.Text = this.Text;
        this.Base.MaxLength = this._MaxLength;
        this.Base.Multiline = this._Multiline;
        this.Base.ReadOnly = this._ReadOnly;
        this.Base.UseSystemPasswordChar = this._UseSystemPasswordChar;
        this.Base.ForeColor = Color.White;
        this.Base.BackColor = Color.FromArgb(20, 20, 20);
        this.Base.BorderStyle = BorderStyle.None;
        this.Base.Location = new Point(5, 5);
        checked
        {
            this.Base.Width = base.Width - 14;
            bool multiline = this._Multiline;
            if (multiline)
            {
                this.Base.Height = base.Height - 11;
            }
            else
            {
                base.Height = this.Base.Height + 11;
            }
            this.Base.TextChanged += new EventHandler(this.OnBaseTextChanged);
            this.Base.KeyDown += new KeyEventHandler(this.OnBaseKeyDown);
            this.P1 = new Pen(Color.FromArgb(5, 5, 5));
            this.P2 = new Pen(Color.FromArgb(25, 25, 25));
        }
    }

    private GraphicsPath GP1;

    private GraphicsPath GP2;
    private Pen P1;
    private Pen P2;

    private PathGradientBrush PB1;
    //private Graphics G;

    protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
	{
        ThemeModule.G = e.Graphics;
        ThemeModule.G.Clear(this.BackColor);
        ThemeModule.G.SmoothingMode = SmoothingMode.AntiAlias;
        checked
        {
            this.GP1 = ThemeModule.CreateRound(0, 0, base.Width - 1, base.Height - 1, 7);
            this.GP2 = ThemeModule.CreateRound(1, 1, base.Width - 3, base.Height - 3, 7);
            this.PB1 = new PathGradientBrush(this.GP1);
            this.PB1.CenterColor = Color.FromArgb(20, 20, 20);
            this.PB1.SurroundColors = new Color[]
            {
                    Color.FromArgb(15, 15, 15)
            };
            this.PB1.FocusScales = new PointF(0.9f, 0.5f);
            ThemeModule.G.FillPath(this.PB1, this.GP1);
            ThemeModule.G.DrawPath(this.P2, this.GP1);
            ThemeModule.G.DrawPath(this.P1, this.GP2);
        }
    }

    private void OnBaseTextChanged(object s, EventArgs e)
    {
        Text = Base.Text;
    }

    private void OnBaseKeyUp(object s, KeyEventArgs e)
    {
        base.OnKeyUp(e);
    }

    private void OnBaseKeyDown(object s, KeyEventArgs e)
    {
        if (e.Control && e.KeyCode == Keys.A)
        {
            Base.SelectAll();
            e.SuppressKeyPress = true;
        }
        else
        {
            base.OnKeyDown(e);
        }
    }

    protected override void OnResize(EventArgs e)
    {
        Base.Location = new Point(5, 5);

        Base.Width = Width - 10;
        Base.Height = Height - 11;

        base.OnResize(e);
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        Base.Focus();
        base.OnMouseDown(e);
    }

    protected override void OnEnter(EventArgs e)
    {
        Base.Focus();
        Invalidate();
        base.OnEnter(e);
    }

    protected override void OnLeave(EventArgs e)
    {
        Invalidate();
        base.OnLeave(e);
    }


}

[DefaultEvent("CheckedChanged")]
class NSCheckBox : Control
{

    public event CheckedChangedEventHandler CheckedChanged;
    public delegate void CheckedChangedEventHandler(object sender);

    public NSCheckBox()
    {
        base.SetStyle(ControlStyles.UserPaint | ControlStyles.Opaque | ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
        base.SetStyle(ControlStyles.Selectable, false);
        this.P11 = new Pen(Color.FromArgb(25, 25, 25));
        this.P22 = new Pen(Color.FromArgb(5, 5, 5));
        this.P3 = new Pen(Color.Black, 2f);
        this.P4 = new Pen(Color.FromArgb(1, 135, 199), 2f);
    }

    private bool _Checked;
    public bool Checked
    {
        get { return _Checked; }
        set
        {
            _Checked = value;
            if (CheckedChanged != null)
            {
                CheckedChanged(this);
            }

            Invalidate();
        }
    }

    private GraphicsPath GP1;

    private GraphicsPath GP2;
    private SizeF SZ1;

    private PointF PT1;
    private Pen P11;
    private Pen P22;
    private Pen P3;

    private Pen P4;

    private PathGradientBrush PB1;
    //private Graphics G;
    protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
	{
        ThemeModule.G = e.Graphics;
        ThemeModule.G.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
        ThemeModule.G.Clear(this.BackColor);
        ThemeModule.G.SmoothingMode = SmoothingMode.AntiAlias;
        checked
        {
            this.GP1 = ThemeModule.CreateRound(0, 2, base.Height - 5, base.Height - 5, 5);
            this.GP2 = ThemeModule.CreateRound(1, 3, base.Height - 7, base.Height - 7, 5);
            this.PB1 = new PathGradientBrush(this.GP1);
            this.PB1.CenterColor = Color.FromArgb(20, 20, 20);
            this.PB1.SurroundColors = new Color[]
            {
                    Color.FromArgb(15, 15, 15)
            };
            this.PB1.FocusScales = new PointF(0.3f, 0.3f);
            ThemeModule.G.FillPath(this.PB1, this.GP1);
            ThemeModule.G.DrawPath(this.P11, this.GP1);
            ThemeModule.G.DrawPath(this.P22, this.GP2);
            bool @checked = this._Checked;
            if (@checked)
            {
                ThemeModule.G.DrawLine(this.P3, 5, base.Height - 9, 8, base.Height - 7);
                ThemeModule.G.DrawLine(this.P3, 7, base.Height - 7, base.Height - 8, 7);
                ThemeModule.G.DrawLine(this.P4, 4, base.Height - 10, 7, base.Height - 8);
                ThemeModule.G.DrawLine(this.P4, 6, base.Height - 8, base.Height - 9, 6);
            }
            this.SZ1 = ThemeModule.G.MeasureString(this.Text, this.Font);
            this.PT1 = new PointF((float)(base.Height - 3), unchecked((float)(base.Height / 2) - this.SZ1.Height / 2f));
        }
        ThemeModule.G.DrawString(this.Text, this.Font, Brushes.Black, this.PT1.X + 1f, this.PT1.Y + 1f);
        ThemeModule.G.DrawString(this.Text, this.Font, Brushes.White, this.PT1);
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        Checked = !Checked;
    }

}

[DefaultEvent("CheckedChanged")]
class NSRadioButton : Control
{

    public event CheckedChangedEventHandler CheckedChanged;
    public delegate void CheckedChangedEventHandler(object sender);

    public NSRadioButton()
    {
        SetStyle((ControlStyles)139286, true);
        SetStyle(ControlStyles.Selectable, false);

        P1 = new Pen(Color.FromArgb(55, 55, 55));
        P2 = new Pen(Color.FromArgb(35, 35, 35));
    }

    private bool _Checked;
    public bool Checked
    {
        get { return _Checked; }
        set
        {
            _Checked = value;

            if (_Checked)
            {
                InvalidateParent();
            }

            if (CheckedChanged != null)
            {
                CheckedChanged(this);
            }
            Invalidate();
        }
    }

    private void InvalidateParent()
    {
        if (Parent == null)
            return;

        foreach (Control C in Parent.Controls)
        {
            if ((!object.ReferenceEquals(C, this)) && (C is NSRadioButton))
            {
                ((NSRadioButton)C).Checked = false;
            }
        }
    }


    private GraphicsPath GP1;
    private SizeF SZ1;

    private PointF PT1;
    private Pen P1;

    private Pen P2;

    private PathGradientBrush PB1;
    //private Graphics G;
    protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
	{
        ThemeModule.G = e.Graphics;
        ThemeModule.G.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
        ThemeModule.G.Clear(this.BackColor);
        ThemeModule.G.SmoothingMode = SmoothingMode.AntiAlias;
        this.GP1 = new GraphicsPath();
        checked
        {
            this.GP1.AddEllipse(0, 2, base.Height - 5, base.Height - 5);
            this.PB1 = new PathGradientBrush(this.GP1);
            this.PB1.CenterColor = Color.FromArgb(20, 20, 20);
            this.PB1.SurroundColors = new Color[]
            {
                    Color.FromArgb(15, 15, 15)
            };
            this.PB1.FocusScales = new PointF(0.3f, 0.3f);
            ThemeModule.G.FillPath(this.PB1, this.GP1);
            ThemeModule.G.DrawEllipse(this.P1, 0, 2, base.Height - 5, base.Height - 5);
            ThemeModule.G.DrawEllipse(this.P2, 1, 3, base.Height - 7, base.Height - 7);
            bool @checked = this._Checked;
            if (@checked)
            {
                ThemeModule.G.FillEllipse(Brushes.Black, 6, 8, base.Height - 15, base.Height - 15);
                ThemeModule.G.FillEllipse(Brushes.DodgerBlue, 5, 7, base.Height - 15, base.Height - 15);
            }
            this.SZ1 = ThemeModule.G.MeasureString(this.Text, this.Font);
            this.PT1 = new PointF((float)(base.Height - 3), unchecked((float)(base.Height / 2) - this.SZ1.Height / 2f));
        }
        ThemeModule.G.DrawString(this.Text, this.Font, Brushes.Black, this.PT1.X + 1f, this.PT1.Y + 1f);
        ThemeModule.G.DrawString(this.Text, this.Font, Brushes.White, this.PT1);
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        Checked = true;
        base.OnMouseDown(e);
    }

}

class NSComboBox : ComboBox
{

    public NSComboBox()
    {
        base.SetStyle(ControlStyles.UserPaint | ControlStyles.Opaque | ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
        base.SetStyle(ControlStyles.Selectable, false);
        base.DrawMode = DrawMode.OwnerDrawFixed;
        base.DropDownStyle = ComboBoxStyle.DropDownList;
        this.BackColor = Color.FromArgb(20, 20, 20);
        this.ForeColor = Color.White;
        this.P1 = new Pen(Color.FromArgb(5, 5, 5));
        this.P2 = new Pen(Color.White, 2f);
        this.P3 = new Pen(Brushes.Black, 2f);
        this.P4 = new Pen(Color.FromArgb(35, 35, 35));
        this.B1 = new SolidBrush(Color.FromArgb(35, 35, 35));
        this.B2 = new SolidBrush(Color.FromArgb(25, 25, 25));
    }

    private GraphicsPath GP1;

    private GraphicsPath GP2;
    private SizeF SZ1;

    private PointF PT1;
    private Pen P1;
    private Pen P2;
    private Pen P3;
    private Pen P4;
    private SolidBrush B1;

    private SolidBrush B2;

    private LinearGradientBrush GB1;
    private Graphics G;
    protected override void OnPaint(PaintEventArgs e)
    {
        G = e.Graphics;
        G.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

        G.Clear(BackColor);
        G.SmoothingMode = SmoothingMode.AntiAlias;

        GP1 = ThemeModule.CreateRound(0, 0, Width - 1, Height - 1, 7);
        GP2 = ThemeModule.CreateRound(1, 1, Width - 3, Height - 3, 7);

        GB1 = new LinearGradientBrush(ClientRectangle, Color.FromArgb(60, 60, 60), Color.FromArgb(55, 55, 55), 90f);
        G.SetClip(GP1);
        G.FillRectangle(GB1, ClientRectangle);
        G.ResetClip();

        G.DrawPath(P1, GP1);
        G.DrawPath(P4, GP2);

        SZ1 = G.MeasureString(Text, Font);
        PT1 = new PointF(5, Height / 2 - SZ1.Height / 2);

        G.DrawString(Text, Font, Brushes.Black, PT1.X + 1, PT1.Y + 1);
        G.DrawString(Text, Font, Brushes.White, PT1);

        G.DrawLine(P3, Width - 15, 10, Width - 11, 13);
        G.DrawLine(P3, Width - 7, 10, Width - 11, 13);
        G.DrawLine(Pens.Black, Width - 11, 13, Width - 11, 14);

        G.DrawLine(P2, Width - 16, 9, Width - 12, 12);
        G.DrawLine(P2, Width - 8, 9, Width - 12, 12);
        G.DrawLine(Pens.White, Width - 12, 12, Width - 12, 13);

        G.DrawLine(P1, Width - 22, 0, Width - 22, Height);
        G.DrawLine(P4, Width - 23, 1, Width - 23, Height - 2);
        G.DrawLine(P4, Width - 21, 1, Width - 21, Height - 2);
    }

    protected override void OnDrawItem(DrawItemEventArgs e)
    {
        e.Graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

        if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
        {
            e.Graphics.FillRectangle(B1, e.Bounds);
        }
        else
        {
            e.Graphics.FillRectangle(B2, e.Bounds);
        }

        if (!(e.Index == -1))
        {
            e.Graphics.DrawString(GetItemText(Items[e.Index]), e.Font, Brushes.White, e.Bounds);
        }
    }

}

class NSTabControl : TabControl
{

    public NSTabControl()
    {
        base.SetStyle(ControlStyles.UserPaint | ControlStyles.Opaque | ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
        base.SetStyle(ControlStyles.Selectable, false);
        base.SizeMode = TabSizeMode.Fixed;
        base.Alignment = TabAlignment.Left;
        base.ItemSize = new Size(28, 130);
        base.DrawMode = TabDrawMode.OwnerDrawFixed;
        this.P1 = new Pen(Color.FromArgb(25, 25, 25));
        this.P2 = new Pen(Color.FromArgb(5, 5, 5));
        this.P3 = new Pen(Color.FromArgb(15, 15, 15), 2f);
        this.B1 = new SolidBrush(Color.FromArgb(20, 20, 20));
        this.B2 = new SolidBrush(Color.FromArgb(5, 5, 5));
        this.B3 = new SolidBrush(Color.FromArgb(13, 152, 255));
        this.B4 = new SolidBrush(Color.FromArgb(35, 35, 35));
        this.SF1 = new StringFormat();
        this.SF1.LineAlignment = StringAlignment.Center;
    }

    protected override void OnControlAdded(ControlEventArgs e)
    {
        if (e.Control is TabPage)
        {
            e.Control.BackColor = Color.FromArgb(20, 20, 20);
        }

        base.OnControlAdded(e);
    }

    private GraphicsPath GP1;
    private GraphicsPath GP2;
    private GraphicsPath GP3;

    private GraphicsPath GP4;
    private Rectangle R1;

    private Rectangle R2;
    private Pen P1;
    private Pen P2;
    private Pen P3;
    private SolidBrush B1;
    private SolidBrush B2;
    private SolidBrush B3;

    private SolidBrush B4;

    private PathGradientBrush PB1;
    private TabPage TP1;

    private StringFormat SF1;
    private int Offset;

    private int ItemHeight;
    //private Graphics G;

    protected override void OnPaint(PaintEventArgs e)
	{
        ThemeModule.G = e.Graphics;
        ThemeModule.G.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
        ThemeModule.G.Clear(Color.FromArgb(20, 20, 20));
        ThemeModule.G.SmoothingMode = SmoothingMode.AntiAlias;
        checked
        {
            this.ItemHeight = base.ItemSize.Height + 2;
            this.GP1 = ThemeModule.CreateRound(0, 0, this.ItemHeight + 3, base.Height - 1, 7);
            this.GP2 = ThemeModule.CreateRound(1, 1, this.ItemHeight + 3, base.Height - 3, 7);
            this.PB1 = new PathGradientBrush(this.GP1);
            this.PB1.CenterColor = Color.FromArgb(20, 20, 20);
            this.PB1.SurroundColors = new Color[]
            {
                    Color.FromArgb(15, 15, 15)
            };
            this.PB1.FocusScales = new PointF(0.8f, 0.95f);
            ThemeModule.G.FillPath(this.PB1, this.GP1);
            ThemeModule.G.DrawPath(this.P1, this.GP1);
            ThemeModule.G.DrawPath(this.P2, this.GP2);
            int num = base.TabCount - 1;
            for (int i = 0; i <= num; i++)
            {
                this.R1 = base.GetTabRect(i);
                this.R1.Y = this.R1.Y + 2;
                this.R1.Height = this.R1.Height - 3;
                this.R1.Width = this.R1.Width + 1;
                this.R1.X = this.R1.X - 1;
                this.TP1 = base.TabPages[i];
                this.Offset = 0;
                bool flag = base.SelectedIndex == i;
                if (flag)
                {
                    ThemeModule.G.FillRectangle(this.B1, this.R1);
                    int num2 = 0;
                    do
                    {
                        ThemeModule.G.FillRectangle(this.B2, this.R1.X + 5 + num2 * 5, this.R1.Y + 6, 2, this.R1.Height - 9);
                        ThemeModule.G.SmoothingMode = SmoothingMode.None;
                        ThemeModule.G.FillRectangle(this.B3, this.R1.X + 5 + num2 * 5, this.R1.Y + 5, 2, this.R1.Height - 9);
                        ThemeModule.G.SmoothingMode = SmoothingMode.AntiAlias;
                        this.Offset += 5;
                        num2++;
                    }
                    while (num2 <= 1);
                    ThemeModule.G.DrawRectangle(this.P3, this.R1.X + 1, this.R1.Y - 1, this.R1.Width, this.R1.Height + 2);
                    ThemeModule.G.DrawRectangle(this.P1, this.R1.X + 1, this.R1.Y + 1, this.R1.Width - 2, this.R1.Height - 2);
                    ThemeModule.G.DrawRectangle(this.P2, this.R1);
                }
                else
                {
                    int num3 = 0;
                    do
                    {
                        ThemeModule.G.FillRectangle(this.B2, this.R1.X + 5 + num3 * 5, this.R1.Y + 6, 2, this.R1.Height - 9);
                        ThemeModule.G.SmoothingMode = SmoothingMode.None;
                        ThemeModule.G.FillRectangle(this.B4, this.R1.X + 5 + num3 * 5, this.R1.Y + 5, 2, this.R1.Height - 9);
                        ThemeModule.G.SmoothingMode = SmoothingMode.AntiAlias;
                        this.Offset += 5;
                        num3++;
                    }
                    while (num3 <= 1);
                }
                this.R1.X = this.R1.X + (5 + this.Offset);
                this.R2 = this.R1;
                this.R2.Y = this.R2.Y + 1;
                this.R2.X = this.R2.X + 1;
                ThemeModule.G.DrawString(this.TP1.Text, this.Font, Brushes.Black, this.R2, this.SF1);
                ThemeModule.G.DrawString(this.TP1.Text, this.Font, Brushes.White, this.R1, this.SF1);
            }
            this.GP3 = ThemeModule.CreateRound(this.ItemHeight, 0, base.Width - this.ItemHeight - 1, base.Height - 1, 7);
            this.GP4 = ThemeModule.CreateRound(this.ItemHeight + 1, 1, base.Width - this.ItemHeight - 3, base.Height - 3, 7);
            ThemeModule.G.DrawPath(this.P2, this.GP3);
            ThemeModule.G.DrawPath(this.P1, this.GP4);
        }
    }

}

[DefaultEvent("CheckedChanged")]
class NSOnOffBox : Control
{

    public event CheckedChangedEventHandler CheckedChanged;
    public delegate void CheckedChangedEventHandler(object sender);

    public NSOnOffBox()
    {
        base.SetStyle(ControlStyles.UserPaint | ControlStyles.Opaque | ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
        base.SetStyle(ControlStyles.Selectable, false);
        this.P1 = new Pen(Color.FromArgb(25, 25, 25));
        this.P2 = new Pen(Color.FromArgb(5, 5, 5));
        this.P3 = new Pen(Color.FromArgb(35, 35, 35));
        this.B1 = new SolidBrush(Color.FromArgb(5, 5, 5));
        this.B2 = new SolidBrush(Color.FromArgb(55, 55, 55));
        this.B3 = new SolidBrush(Color.FromArgb(35, 35, 35));
        this.B4 = new SolidBrush(Color.FromArgb(13, 152, 255));
        this.B5 = new SolidBrush(Color.FromArgb(10, 10, 10));
        this.SF1 = new StringFormat();
        this.SF1.LineAlignment = StringAlignment.Center;
        this.SF1.Alignment = StringAlignment.Near;
        this.SF2 = new StringFormat();
        this.SF2.LineAlignment = StringAlignment.Center;
        this.SF2.Alignment = StringAlignment.Far;
        base.Size = new Size(56, 24);
        this.MinimumSize = base.Size;
        this.MaximumSize = base.Size;
    }

    private bool _Checked;
    public bool Checked
    {
        get { return _Checked; }
        set
        {
            _Checked = value;
            if (CheckedChanged != null)
            {
                CheckedChanged(this);
            }

            Invalidate();
        }
    }

    private GraphicsPath GP1;
    private GraphicsPath GP2;
    private GraphicsPath GP3;

    private GraphicsPath GP4;
    private Pen P1;
    private Pen P2;
    private Pen P3;
    private SolidBrush B1;
    private SolidBrush B2;
    private SolidBrush B3;
    private SolidBrush B4;

    private SolidBrush B5;
    private PathGradientBrush PB1;

    private LinearGradientBrush GB1;
    private Rectangle R1;
    private Rectangle R2;
    private Rectangle R3;
    private StringFormat SF1;

    private StringFormat SF2;

    private int Offset;
    //private Graphics G;

    protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
	{
        ThemeModule.G = e.Graphics;
        ThemeModule.G.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
        ThemeModule.G.Clear(this.BackColor);
        ThemeModule.G.SmoothingMode = SmoothingMode.AntiAlias;
        checked
        {
            this.GP1 = ThemeModule.CreateRound(0, 0, base.Width - 1, base.Height - 1, 7);
            this.GP2 = ThemeModule.CreateRound(1, 1, base.Width - 3, base.Height - 3, 7);
            this.PB1 = new PathGradientBrush(this.GP1);
            this.PB1.CenterColor = Color.FromArgb(20, 20, 20);
            this.PB1.SurroundColors = new Color[]
            {
                    Color.FromArgb(15, 15, 15)
            };
            this.PB1.FocusScales = new PointF(0.3f, 0.3f);
            ThemeModule.G.FillPath(this.PB1, this.GP1);
            ThemeModule.G.DrawPath(this.P1, this.GP1);
            ThemeModule.G.DrawPath(this.P2, this.GP2);
            this.R1 = new Rectangle(5, 0, base.Width - 10, base.Height + 2);
            this.R2 = new Rectangle(6, 1, base.Width - 10, base.Height + 2);
            this.R3 = new Rectangle(1, 1, base.Width / 2 - 1, base.Height - 3);
            bool @checked = this._Checked;
            if (@checked)
            {
                ThemeModule.G.DrawString("On", this.Font, Brushes.Black, this.R2, this.SF1);
                ThemeModule.G.DrawString("On", this.Font, Brushes.White, this.R1, this.SF1);
                this.R3.X = this.R3.X + (base.Width / 2 - 1);
            }
            else
            {
                ThemeModule.G.DrawString("Off", this.Font, this.B1, this.R2, this.SF2);
                ThemeModule.G.DrawString("Off", this.Font, this.B2, this.R1, this.SF2);
            }
            this.GP3 = ThemeModule.CreateRound(this.R3, 7);
            this.GP4 = ThemeModule.CreateRound(this.R3.X + 1, this.R3.Y + 1, this.R3.Width - 2, this.R3.Height - 2, 7);
            this.GB1 = new LinearGradientBrush(base.ClientRectangle, Color.FromArgb(30, 30, 30), Color.FromArgb(25, 25, 25), 90f);
            ThemeModule.G.FillPath(this.GB1, this.GP3);
            ThemeModule.G.DrawPath(this.P2, this.GP3);
            ThemeModule.G.DrawPath(this.P3, this.GP4);
            this.Offset = this.R3.X + this.R3.Width / 2 - 3;
            int num = 0;
            do
            {
                bool checked2 = this._Checked;
                if (checked2)
                {
                    ThemeModule.G.FillRectangle(this.B1, this.Offset + num * 5, 7, 2, base.Height - 14);
                }
                else
                {
                    ThemeModule.G.FillRectangle(this.B3, this.Offset + num * 5, 7, 2, base.Height - 14);
                }
                ThemeModule.G.SmoothingMode = SmoothingMode.None;
                bool checked3 = this._Checked;
                if (checked3)
                {
                    ThemeModule.G.FillRectangle(this.B4, this.Offset + num * 5, 7, 2, base.Height - 14);
                }
                else
                {
                    ThemeModule.G.FillRectangle(this.B5, this.Offset + num * 5, 7, 2, base.Height - 14);
                }
                ThemeModule.G.SmoothingMode = SmoothingMode.AntiAlias;
                num++;
            }
            while (num <= 1);
        }
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        Checked = !Checked;
        base.OnMouseDown(e);
    }

}

class NSControlButton : Control
{

    public enum Button : byte
    {
        None = 0,
        Minimize = 1,
        MaximizeRestore = 2,
        Close = 3
    }

    private Button _ControlButton = Button.Close;
    public Button ControlButton
    {
        get { return _ControlButton; }
        set
        {
            _ControlButton = value;
            Invalidate();
        }
    }

    public NSControlButton()
    {
        SetStyle((ControlStyles)139286, true);
        SetStyle(ControlStyles.Selectable, false);

        Anchor = AnchorStyles.Top | AnchorStyles.Right;

        Width = 18;
        Height = 20;

        MinimumSize = Size;
        MaximumSize = Size;

        Margin = new Padding(0);
    }

    private Graphics G;
    protected override void OnPaint(PaintEventArgs e)
    {
        G = e.Graphics;
        G.Clear(BackColor);

        switch (_ControlButton)
        {
            case Button.Minimize:
                DrawMinimize(3, 10);
                break;
            case Button.MaximizeRestore:
                if (FindForm().WindowState == FormWindowState.Normal)
                {
                    DrawMaximize(3, 5);
                }
                else
                {
                    DrawRestore(3, 4);
                }
                break;
            case Button.Close:
                DrawClose(4, 5);
                break;
        }
    }

    private void DrawMinimize(int x, int y)
    {
        G.FillRectangle(Brushes.White, x, y, 12, 5);
        G.DrawRectangle(Pens.Black, x, y, 11, 4);
    }

    private void DrawMaximize(int x, int y)
    {
        G.DrawRectangle(new Pen(Color.White, 2), x + 2, y + 2, 8, 6);
        G.DrawRectangle(Pens.Black, x, y, 11, 9);
        G.DrawRectangle(Pens.Black, x + 3, y + 3, 5, 3);
    }

    private void DrawRestore(int x, int y)
    {
        G.FillRectangle(Brushes.White, x + 3, y + 1, 8, 4);
        G.FillRectangle(Brushes.White, x + 7, y + 5, 4, 4);
        G.DrawRectangle(Pens.Black, x + 2, y + 0, 9, 9);

        G.FillRectangle(Brushes.White, x + 1, y + 3, 2, 6);
        G.FillRectangle(Brushes.White, x + 1, y + 9, 8, 2);
        G.DrawRectangle(Pens.Black, x, y + 2, 9, 9);
        G.DrawRectangle(Pens.Black, x + 3, y + 5, 3, 3);
    }

    private GraphicsPath ClosePath;
    private void DrawClose(int x, int y)
    {
        if (ClosePath == null)
        {
            ClosePath = new GraphicsPath();
            ClosePath.AddLine(x + 1, y, x + 3, y);
            ClosePath.AddLine(x + 5, y + 2, x + 7, y);
            ClosePath.AddLine(x + 9, y, x + 10, y + 1);
            ClosePath.AddLine(x + 7, y + 4, x + 7, y + 5);
            ClosePath.AddLine(x + 10, y + 8, x + 9, y + 9);
            ClosePath.AddLine(x + 7, y + 9, x + 5, y + 7);
            ClosePath.AddLine(x + 3, y + 9, x + 1, y + 9);
            ClosePath.AddLine(x + 0, y + 8, x + 3, y + 5);
            ClosePath.AddLine(x + 3, y + 4, x + 0, y + 1);
        }

        G.FillPath(Brushes.White, ClosePath);
        G.DrawPath(Pens.Black, ClosePath);
    }

    protected override void OnMouseClick(MouseEventArgs e)
    {

        if (e.Button == System.Windows.Forms.MouseButtons.Left)
        {
            Form F = FindForm();

            switch (_ControlButton)
            {
                case Button.Minimize:
                    F.WindowState = FormWindowState.Minimized;
                    break;
                case Button.MaximizeRestore:
                    if (F.WindowState == FormWindowState.Normal)
                    {
                        F.WindowState = FormWindowState.Maximized;
                    }
                    else
                    {
                        F.WindowState = FormWindowState.Normal;
                    }
                    break;
                case Button.Close:
                    F.Close();
                    break;
            }

        }

        Invalidate();
        base.OnMouseClick(e);
    }

}

class NSGroupBox : ContainerControl
{

    private bool _DrawSeperator;
    public bool DrawSeperator
    {
        get { return _DrawSeperator; }
        set
        {
            _DrawSeperator = value;
            Invalidate();
        }
    }

    private string _Title = "GroupBox";
    public string Title
    {
        get { return _Title; }
        set
        {
            _Title = value;
            Invalidate();
        }
    }

    private string _SubTitle = "Details";
    public string SubTitle
    {
        get { return _SubTitle; }
        set
        {
            _SubTitle = value;
            Invalidate();
        }
    }

    private Font _TitleFont;

    private Font _SubTitleFont;
    public NSGroupBox()
    {
        this._Title = "GroupBox";
        this._SubTitle = "Details";
        base.SetStyle(ControlStyles.UserPaint | ControlStyles.Opaque | ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
        base.SetStyle(ControlStyles.Selectable, false);
        this._TitleFont = new Font("Verdana", 10f);
        this._SubTitleFont = new Font("Verdana", 6.5f);
        this.P1 = new Pen(Color.FromArgb(5, 5, 5));
        this.P2 = new Pen(Color.FromArgb(25, 25, 25));
        this.B1 = new SolidBrush(Color.FromArgb(13, 152, 255));
    }

    private GraphicsPath GP1;

    private GraphicsPath GP2;
    private PointF PT1;
    private SizeF SZ1;

    private SizeF SZ2;
    private Pen P1;
    private Pen P2;

    private SolidBrush B1;
    private Graphics G;

    protected override void OnPaint(PaintEventArgs e)
    {
        G = e.Graphics;
        G.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

        G.Clear(BackColor);
        G.SmoothingMode = SmoothingMode.AntiAlias;

        GP1 = ThemeModule.CreateRound(0, 0, Width - 1, Height - 1, 7);
        GP2 = ThemeModule.CreateRound(1, 1, Width - 3, Height - 3, 7);

        G.DrawPath(P1, GP1);
        G.DrawPath(P2, GP2);

        SZ1 = G.MeasureString(_Title, _TitleFont, Width, StringFormat.GenericTypographic);
        SZ2 = G.MeasureString(_SubTitle, _SubTitleFont, Width, StringFormat.GenericTypographic);

        G.DrawString(_Title, _TitleFont, Brushes.Black, 6, 6);
        G.DrawString(_Title, _TitleFont, B1, 5, 5);

        PT1 = new PointF(6f, SZ1.Height + 4f);

        G.DrawString(_SubTitle, _SubTitleFont, Brushes.Black, PT1.X + 1, PT1.Y + 1);
        G.DrawString(_SubTitle, _SubTitleFont, Brushes.White, PT1.X, PT1.Y);

        if (_DrawSeperator)
        {
            int Y = Convert.ToInt32(PT1.Y + SZ2.Height) + 8;

            G.DrawLine(P1, 4, Y, Width - 5, Y);
            G.DrawLine(P2, 4, Y + 1, Width - 5, Y + 1);
        }
    }

}

class NSSeperator : Control
{

    public NSSeperator()
    {
        SetStyle((ControlStyles)139286, true);
        SetStyle(ControlStyles.Selectable, false);

        Height = 10;

        P1 = new Pen(Color.FromArgb(35, 35, 35));
        P2 = new Pen(Color.FromArgb(55, 55, 55));
    }

    private Pen P1;

    private Pen P2;
    private Graphics G;

    protected override void OnPaint(PaintEventArgs e)
    {
        G = e.Graphics;
        G.Clear(BackColor);

        G.DrawLine(P1, 0, 5, Width, 5);
        G.DrawLine(P2, 0, 6, Width, 6);
    }

}

[DefaultEvent("Scroll")]
class NSTrackBar : Control
{

    public event ScrollEventHandler Scroll;
    public delegate void ScrollEventHandler(object sender);

    private int _Minimum;
    public int Minimum
    {
        get { return _Minimum; }
        set
        {
            if (value < 0)
            {
                throw new Exception("Property value is not valid.");
            }

            _Minimum = value;
            if (value > _Value)
                _Value = value;
            if (value > _Maximum)
                _Maximum = value;
            Invalidate();
        }
    }

    private int _Maximum = 10;
    public int Maximum
    {
        get { return _Maximum; }
        set
        {
            if (value < 0)
            {
                throw new Exception("Property value is not valid.");
            }

            _Maximum = value;
            if (value < _Value)
                _Value = value;
            if (value < _Minimum)
                _Minimum = value;
            Invalidate();
        }
    }

    private int _Value;
    public int Value
    {
        get { return _Value; }
        set
        {
            if (value == _Value)
                return;

            if (value > _Maximum || value < _Minimum)
            {
                throw new Exception("Property value is not valid.");
            }

            _Value = value;
            Invalidate();

            if (Scroll != null)
            {
                Scroll(this);
            }
        }
    }

    public NSTrackBar()
    {
        this._Maximum = 10;
        base.SetStyle(ControlStyles.UserPaint | ControlStyles.Opaque | ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
        base.SetStyle(ControlStyles.Selectable, false);
        base.Height = 17;
        this.P1 = new Pen(Color.FromArgb(1, 73, 139), 2f);
        this.P2 = new Pen(Color.FromArgb(25, 25, 25));
        this.P3 = new Pen(Color.FromArgb(5, 5, 5));
        this.P4 = new Pen(Color.FromArgb(35, 35, 35));
    }

    private GraphicsPath GP1;
    private GraphicsPath GP2;
    private GraphicsPath GP3;

    private GraphicsPath GP4;
    private Rectangle R1;
    private Rectangle R2;
    private Rectangle R3;

    private int I1;
    private Pen P1;
    private Pen P2;
    private Pen P3;

    private Pen P4;
    private LinearGradientBrush GB1;
    private LinearGradientBrush GB2;

    private LinearGradientBrush GB3;
    //private Graphics G;

    protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
    {
        ThemeModule.G = e.Graphics;
        ThemeModule.G.Clear(this.BackColor);
        ThemeModule.G.SmoothingMode = SmoothingMode.AntiAlias;
        checked
        {
            this.GP1 = ThemeModule.CreateRound(0, 5, base.Width - 1, 10, 5);
            this.GP2 = ThemeModule.CreateRound(1, 6, base.Width - 3, 8, 5);
            this.R1 = new Rectangle(0, 7, base.Width - 1, 5);
            this.GB1 = new LinearGradientBrush(this.R1, Color.FromArgb(15, 15, 15), Color.FromArgb(20, 20, 20), 90f);
            this.I1 = (int)Math.Round(unchecked(checked((double)(this._Value - this._Minimum) / (double)(this._Maximum - this._Minimum)) * (double)(checked(base.Width - 11))));
            this.R2 = new Rectangle(this.I1, 0, 10, 20);
            ThemeModule.G.SetClip(this.GP2);
            ThemeModule.G.FillRectangle(this.GB1, this.R1);
            this.R3 = new Rectangle(1, 7, this.R2.X + this.R2.Width - 2, 8);
            this.GB2 = new LinearGradientBrush(this.R3, Color.FromArgb(13, 152, 255), Color.FromArgb(1, 73, 139), 90f);
            ThemeModule.G.SmoothingMode = SmoothingMode.None;
            ThemeModule.G.FillRectangle(this.GB2, this.R3);
            ThemeModule.G.SmoothingMode = SmoothingMode.AntiAlias;
            int num = this.R3.Width - 15;
            for (int i = 0; i <= num; i += 5)
            {
                ThemeModule.G.DrawLine(this.P1, i, 0, i + 15, base.Height);
            }
            ThemeModule.G.ResetClip();
            ThemeModule.G.DrawPath(this.P2, this.GP1);
            ThemeModule.G.DrawPath(this.P3, this.GP2);
            this.GP3 = ThemeModule.CreateRound(this.R2, 5);
            this.GP4 = ThemeModule.CreateRound(this.R2.X + 1, this.R2.Y + 1, this.R2.Width - 2, this.R2.Height - 2, 5);
            this.GB3 = new LinearGradientBrush(base.ClientRectangle, Color.FromArgb(30, 30, 30), Color.FromArgb(25, 25, 25), 90f);
            ThemeModule.G.FillPath(this.GB3, this.GP3);
            ThemeModule.G.DrawPath(this.P3, this.GP3);
            ThemeModule.G.DrawPath(this.P4, this.GP4);
        }
    }

    private bool TrackDown;
    protected override void OnMouseDown(MouseEventArgs e)
    {
        if (e.Button == System.Windows.Forms.MouseButtons.Left)
        {
            I1 = Convert.ToInt32((double)(_Value - _Minimum) / (double)(_Maximum - _Minimum) * (Width - 11));
            R2 = new Rectangle(I1, 0, 10, 20);

            TrackDown = R2.Contains(e.Location);
        }

        base.OnMouseDown(e);
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        if (TrackDown && e.X > -1 && e.X < (Width + 1))
        {
            Value = _Minimum + Convert.ToInt32((_Maximum - _Minimum) * ((double)e.X / (double)Width));
        }

        base.OnMouseMove(e);
    }

    protected override void OnMouseUp(MouseEventArgs e)
    {
        TrackDown = false;
        base.OnMouseUp(e);
    }

}

[DefaultEvent("ValueChanged")]
class NSRandomPool : Control
{

    public event ValueChangedEventHandler ValueChanged;
    public delegate void ValueChangedEventHandler(object sender);

    private StringBuilder _Value = new StringBuilder();
    public string Value
    {
        get { return _Value.ToString(); }
    }

    public string FullValue
    {
        get { return BitConverter.ToString(Table).Replace("-", ""); }
    }


    private Random RNG = new Random();
    private int ItemSize = 9;

    private int DrawSize = 8;

    private Rectangle WA;
    private int RowSize;

    private int ColumnSize;
    public NSRandomPool()
    {
        this._Value = new StringBuilder();
        this.RNG = new Random();
        this.ItemSize = 9;
        this.DrawSize = 8;
        this.Index1 = -1;
        base.SetStyle(ControlStyles.UserPaint | ControlStyles.Opaque | ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
        base.SetStyle(ControlStyles.Selectable, false);
        this.P1 = new Pen(Color.FromArgb(25, 25, 25));
        this.P2 = new Pen(Color.FromArgb(5, 5, 5));
        this.B1 = new SolidBrush(Color.FromArgb(30, 30, 30));
    }

    protected override void OnHandleCreated(EventArgs e)
    {
        UpdateTable();
        base.OnHandleCreated(e);
    }

    private byte[] Table;
    private void UpdateTable()
    {
        WA = new Rectangle(5, 5, Width - 10, Height - 10);

        RowSize = WA.Width / ItemSize;
        ColumnSize = WA.Height / ItemSize;

        WA.Width = RowSize * ItemSize;
        WA.Height = ColumnSize * ItemSize;

        WA.X = (Width / 2) - (WA.Width / 2);
        WA.Y = (Height / 2) - (WA.Height / 2);

        Table = new byte[(RowSize * ColumnSize)];

        for (int I = 0; I <= Table.Length - 1; I++)
        {
            Table[I] = Convert.ToByte(RNG.Next(100));
        }

        Invalidate();
    }

    protected override void OnSizeChanged(EventArgs e)
    {
        UpdateTable();
    }

    private int Index1 = -1;

    private int Index2;

    private bool InvertColors;
    protected override void OnMouseMove(MouseEventArgs e)
    {
        HandleDraw(e);
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        HandleDraw(e);
        base.OnMouseDown(e);
    }

    private void HandleDraw(MouseEventArgs e)
    {
        if (e.Button == System.Windows.Forms.MouseButtons.Left || e.Button == System.Windows.Forms.MouseButtons.Right)
        {
            if (!WA.Contains(e.Location))
                return;

            InvertColors = (e.Button == System.Windows.Forms.MouseButtons.Right);

            Index1 = GetIndex(e.X, e.Y);
            if (Index1 == Index2)
                return;

            bool L = !(Index1 % RowSize == 0);
            bool R = !(Index1 % RowSize == (RowSize - 1));

            Randomize(Index1 - RowSize);
            if (L)
                Randomize(Index1 - 1);
            Randomize(Index1);
            if (R)
                Randomize(Index1 + 1);
            Randomize(Index1 + RowSize);

            _Value.Append(Table[Index1].ToString("X"));
            if (_Value.Length > 32)
                _Value.Remove(0, 2);

            if (ValueChanged != null)
            {
                ValueChanged(this);
            }

            Index2 = Index1;
            Invalidate();
        }
    }

    private GraphicsPath GP1;

    private GraphicsPath GP2;
    private Pen P1;
    private Pen P2;
    private SolidBrush B1;

    private SolidBrush B2;

    private PathGradientBrush PB1;
    private Graphics G;

    protected override void OnPaint(PaintEventArgs e)
	{
		G = e.Graphics;

		G.Clear(BackColor);
		G.SmoothingMode = SmoothingMode.AntiAlias;

        GP1 = ThemeModule.CreateRound(0, 0, Width - 1, Height - 1, 7);
        GP2 = ThemeModule.CreateRound(1, 1, Width - 3, Height - 3, 7);

		PB1 = new PathGradientBrush(GP1);
		PB1.CenterColor = Color.FromArgb(50, 50, 50);
		PB1.SurroundColors = new Color[]{ Color.FromArgb(45, 45, 45) };
		PB1.FocusScales = new PointF(0.9f, 0.5f);

		G.FillPath(PB1, GP1);

		G.DrawPath(P1, GP1);
		G.DrawPath(P2, GP2);

		G.SmoothingMode = SmoothingMode.None;

		for (int I = 0; I <= Table.Length - 1; I++) {
			int C = Math.Max(Table[I], (byte)75);

			int X = ((I % RowSize) * ItemSize) + WA.X;
			int Y = ((I / RowSize) * ItemSize) + WA.Y;

			B2 = new SolidBrush(Color.FromArgb(C, C, C));

			G.FillRectangle(B1, X + 1, Y + 1, DrawSize, DrawSize);
			G.FillRectangle(B2, X, Y, DrawSize, DrawSize);

			B2.Dispose();
		}

	}

    private int GetIndex(int x, int y)
    {
        return (((y - WA.Y) / ItemSize) * RowSize) + ((x - WA.X) / ItemSize);
    }

    private void Randomize(int index)
    {
        if (index > -1 && index < Table.Length)
        {
            if (InvertColors)
            {
                Table[index] = Convert.ToByte(RNG.Next(100));
            }
            else
            {
                Table[index] = Convert.ToByte(RNG.Next(100, 256));
            }
        }
    }

}

class NSKeyboard : Control
{

    private Bitmap TextBitmap;

    private Graphics TextGraphics;
    const string LowerKeys = "1234567890-=qwertyuiop[]asdfghjkl\\;'zxcvbnm,./`";

    const string UpperKeys = "!@#$%^&*()_+QWERTYUIOP{}ASDFGHJKL|:\"ZXCVBNM<>?~";
    public NSKeyboard()
    {
        SetStyle((ControlStyles)139286, true);
        SetStyle(ControlStyles.Selectable, false);

        Font = new Font("Verdana", 8.25f);

        TextBitmap = new Bitmap(1, 1);
        TextGraphics = Graphics.FromImage(TextBitmap);

        MinimumSize = new Size(386, 162);
        MaximumSize = new Size(386, 162);

        Lower = LowerKeys.ToCharArray();
        Upper = UpperKeys.ToCharArray();

        PrepareCache();

        this.P1 = new Pen(Color.FromArgb(15, 15, 15));
        this.P2 = new Pen(Color.FromArgb(35, 35, 35));
        this.P3 = new Pen(Color.FromArgb(5, 5, 5));
        this.B1 = new SolidBrush(Color.FromArgb(70, 70, 70));
    }

    private Control _Target;
    public Control Target
    {
        get { return _Target; }
        set { _Target = value; }
    }


    private bool Shift;
    private int Pressed = -1;

    private Rectangle[] Buttons;
    private char[] Lower;
    private char[] Upper;
    private string[] Other = {
		"Shift",
		"Space",
		"Back"

	};
    private PointF[] UpperCache;

    private PointF[] LowerCache;
    private void PrepareCache()
    {
        Buttons = new Rectangle[51];
        UpperCache = new PointF[Upper.Length];
        LowerCache = new PointF[Lower.Length];

        int I = 0;

        SizeF S = default(SizeF);
        Rectangle R = default(Rectangle);

        for (int Y = 0; Y <= 3; Y++)
        {
            for (int X = 0; X <= 11; X++)
            {
                I = (Y * 12) + X;
                R = new Rectangle(X * 32, Y * 32, 32, 32);

                Buttons[I] = R;

                if (!(I == 47) && !char.IsLetter(Upper[I]))
                {
                    S = TextGraphics.MeasureString(Upper[I].ToString(), Font);
                    UpperCache[I] = new PointF(R.X + (R.Width / 2 - S.Width / 2), R.Y + R.Height - S.Height - 2);

                    S = TextGraphics.MeasureString(Lower[I].ToString(), Font);
                    LowerCache[I] = new PointF(R.X + (R.Width / 2 - S.Width / 2), R.Y + R.Height - S.Height - 2);
                }
            }
        }

        Buttons[48] = new Rectangle(0, 4 * 32, 2 * 32, 32);
        Buttons[49] = new Rectangle(Buttons[48].Right, 4 * 32, 8 * 32, 32);
        Buttons[50] = new Rectangle(Buttons[49].Right, 4 * 32, 2 * 32, 32);
    }


    private GraphicsPath GP1;
    private SizeF SZ1;

    private PointF PT1;
    private Pen P1;
    private Pen P2;
    private Pen P3;

    private SolidBrush B1;
    private PathGradientBrush PB1;

    private LinearGradientBrush GB1;
    private Graphics G;
    protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
	{
		G = e.Graphics;
		G.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

		G.Clear(BackColor);

		Rectangle R = default(Rectangle);

		int Offset = 0;
		G.DrawRectangle(P1, 0, 0, (12 * 32) + 1, (5 * 32) + 1);

		for (int I = 0; I <= Buttons.Length - 1; I++) {
			R = Buttons[I];

			Offset = 0;
			if (I == Pressed) {
				Offset = 1;

				GP1 = new GraphicsPath();
				GP1.AddRectangle(R);

				PB1 = new PathGradientBrush(GP1);
				PB1.CenterColor = Color.FromArgb(60, 60, 60);
				PB1.SurroundColors = new Color[]{ Color.FromArgb(55, 55, 55) };
				PB1.FocusScales = new PointF(0.8f, 0.5f);

				G.FillPath(PB1, GP1);
			} else {
				GB1 = new LinearGradientBrush(R, Color.FromArgb(60, 60, 60), Color.FromArgb(55, 55, 55), 90f);
				G.FillRectangle(GB1, R);
			}

			switch (I) {
				case 48:
				case 49:
				case 50:
					SZ1 = G.MeasureString(Other[I - 48], Font);
					G.DrawString(Other[I - 48], Font, Brushes.Black, R.X + (R.Width / 2 - SZ1.Width / 2) + Offset + 1, R.Y + (R.Height / 2 - SZ1.Height / 2) + Offset + 1);
					G.DrawString(Other[I - 48], Font, Brushes.White, R.X + (R.Width / 2 - SZ1.Width / 2) + Offset, R.Y + (R.Height / 2 - SZ1.Height / 2) + Offset);
					break;
				case 47:
					DrawArrow(Color.Black, R.X + Offset + 1, R.Y + Offset + 1);
					DrawArrow(Color.White, R.X + Offset, R.Y + Offset);
					break;
				default:
					if (Shift) {
                        G.DrawString(Upper[I].ToString(), Font, Brushes.Black, R.X + 3 + Offset + 1, R.Y + 2 + Offset + 1);
                        G.DrawString(Upper[I].ToString(), Font, Brushes.White, R.X + 3 + Offset, R.Y + 2 + Offset);

						if (!char.IsLetter(Lower[I])) {
							PT1 = LowerCache[I];
                            G.DrawString(Lower[I].ToString(), Font, B1, PT1.X + Offset, PT1.Y + Offset);
						}
					} else {
                        G.DrawString(Lower[I].ToString(), Font, Brushes.Black, R.X + 3 + Offset + 1, R.Y + 2 + Offset + 1);
                        G.DrawString(Lower[I].ToString(), Font, Brushes.White, R.X + 3 + Offset, R.Y + 2 + Offset);

						if (!char.IsLetter(Upper[I])) {
							PT1 = UpperCache[I];
                            G.DrawString(Upper[I].ToString(), Font, B1, PT1.X + Offset, PT1.Y + Offset);
						}
					}
					break;
			}

			G.DrawRectangle(P2, R.X + 1 + Offset, R.Y + 1 + Offset, R.Width - 2, R.Height - 2);
			G.DrawRectangle(P3, R.X + Offset, R.Y + Offset, R.Width, R.Height);

			if (I == Pressed) {
				G.DrawLine(P1, R.X, R.Y, R.Right, R.Y);
				G.DrawLine(P1, R.X, R.Y, R.X, R.Bottom);
			}
		}
	}

    private void DrawArrow(Color color, int rx, int ry)
    {
        Rectangle R = new Rectangle(rx + 8, ry + 8, 16, 16);
        G.SmoothingMode = SmoothingMode.AntiAlias;

        Pen P = new Pen(color, 1);
        AdjustableArrowCap C = new AdjustableArrowCap(3, 2);
        P.CustomEndCap = C;

        G.DrawArc(P, R, 0f, 290f);

        P.Dispose();
        C.Dispose();
        G.SmoothingMode = SmoothingMode.None;
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        int Index = ((e.Y / 32) * 12) + (e.X / 32);

        if (Index > 47)
        {
            for (int I = 48; I <= Buttons.Length - 1; I++)
            {
                if (Buttons[I].Contains(e.X, e.Y))
                {
                    Pressed = I;
                    break; // TODO: might not be correct. Was : Exit For
                }
            }
        }
        else
        {
            Pressed = Index;
        }

        HandleKey();
        Invalidate();
    }

    protected override void OnMouseUp(MouseEventArgs e)
    {
        Pressed = -1;
        Invalidate();
    }

    private void HandleKey()
    {
        if (_Target == null)
            return;
        if (Pressed == -1)
            return;

        switch (Pressed)
        {
            case 47:
                _Target.Text = string.Empty;
                break;
            case 48:
                Shift = !Shift;
                break;
            case 49:
                _Target.Text += " ";
                break;
            case 50:
                if (!(_Target.Text.Length == 0))
                {
                    _Target.Text = _Target.Text.Remove(_Target.Text.Length - 1);
                }
                break;
            default:
                if (Shift)
                {
                    _Target.Text += Upper[Pressed];
                }
                else
                {
                    _Target.Text += Lower[Pressed];
                }
                break;
        }
    }

}

[DefaultEvent("SelectedIndexChanged")]
class NSPaginator : Control
{

    public event SelectedIndexChangedEventHandler SelectedIndexChanged;
    public delegate void SelectedIndexChangedEventHandler(object sender, EventArgs e);

    private Bitmap TextBitmap;

    private Graphics TextGraphics;
    public NSPaginator()
    {
        SetStyle((ControlStyles)139286, true);
        SetStyle(ControlStyles.Selectable, false);

        Size = new Size(202, 26);

        TextBitmap = new Bitmap(1, 1);
        TextGraphics = Graphics.FromImage(TextBitmap);

        InvalidateItems();

        B1 = new SolidBrush(Color.FromArgb(50, 50, 50));
        B2 = new SolidBrush(Color.FromArgb(55, 55, 55));

        P1 = new Pen(Color.FromArgb(35, 35, 35));
        P2 = new Pen(Color.FromArgb(55, 55, 55));
        P3 = new Pen(Color.FromArgb(65, 65, 65));
    }

    private int _SelectedIndex;
    public int SelectedIndex
    {
        get { return _SelectedIndex; }
        set
        {
            _SelectedIndex = Math.Max(Math.Min(value, MaximumIndex), 0);
            Invalidate();
        }
    }

    private int _NumberOfPages;
    public int NumberOfPages
    {
        get { return _NumberOfPages; }
        set
        {
            _NumberOfPages = value;
            _SelectedIndex = Math.Max(Math.Min(_SelectedIndex, MaximumIndex), 0);
            Invalidate();
        }
    }

    public int MaximumIndex
    {
        get { return NumberOfPages - 1; }
    }


    private int ItemWidth;
    public override Font Font
    {
        get { return base.Font; }
        set
        {
            base.Font = value;

            InvalidateItems();
            Invalidate();
        }
    }

    private void InvalidateItems()
    {
        Size S = TextGraphics.MeasureString("000 ..", Font).ToSize();
        ItemWidth = S.Width + 10;
    }

    private GraphicsPath GP1;

    private GraphicsPath GP2;

    private Rectangle R1;
    private Size SZ1;

    private Point PT1;
    private Pen P1;
    private Pen P2;
    private Pen P3;
    private SolidBrush B1;

    private SolidBrush B2;
    private Graphics G;
    protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
    {
        G = e.Graphics;
        G.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

        G.Clear(BackColor);
        G.SmoothingMode = SmoothingMode.AntiAlias;

        bool LeftEllipse = false;
        bool RightEllipse = false;

        if (_SelectedIndex < 4)
        {
            for (int I = 0; I <= Math.Min(MaximumIndex, 4); I++)
            {
                RightEllipse = (I == 4) && (MaximumIndex > 4);
                DrawBox(I * ItemWidth, I, false, RightEllipse);
            }
        }
        else if (_SelectedIndex > 3 && _SelectedIndex < (MaximumIndex - 3))
        {
            for (int I = 0; I <= 4; I++)
            {
                LeftEllipse = (I == 0);
                RightEllipse = (I == 4);
                DrawBox(I * ItemWidth, _SelectedIndex + I - 2, LeftEllipse, RightEllipse);
            }
        }
        else
        {
            for (int I = 0; I <= 4; I++)
            {
                LeftEllipse = (I == 0) && (MaximumIndex > 4);
                DrawBox(I * ItemWidth, MaximumIndex - (4 - I), LeftEllipse, false);
            }
        }
    }

    private void DrawBox(int x, int index, bool leftEllipse, bool rightEllipse)
    {
        R1 = new Rectangle(x, 0, ItemWidth - 4, Height - 1);

        GP1 = ThemeModule.CreateRound(R1, 7);
        GP2 = ThemeModule.CreateRound(R1.X + 1, R1.Y + 1, R1.Width - 2, R1.Height - 2, 7);

        string T = Convert.ToString(index + 1);

        if (leftEllipse)
            T = ".. " + T;
        if (rightEllipse)
            T = T + " ..";

        SZ1 = G.MeasureString(T, Font).ToSize();
        PT1 = new Point(R1.X + (R1.Width / 2 - SZ1.Width / 2), R1.Y + (R1.Height / 2 - SZ1.Height / 2));

        if (index == _SelectedIndex)
        {
            G.FillPath(B1, GP1);

            Font F = new Font(Font, FontStyle.Underline);
            G.DrawString(T, F, Brushes.Black, PT1.X + 1, PT1.Y + 1);
            G.DrawString(T, F, Brushes.White, PT1);
            F.Dispose();

            G.DrawPath(P1, GP2);
            G.DrawPath(P2, GP1);
        }
        else
        {
            G.FillPath(B2, GP1);

            G.DrawString(T, Font, Brushes.Black, PT1.X + 1, PT1.Y + 1);
            G.DrawString(T, Font, Brushes.White, PT1);

            G.DrawPath(P3, GP2);
            G.DrawPath(P1, GP1);
        }
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        if (e.Button == System.Windows.Forms.MouseButtons.Left)
        {
            int NewIndex = 0;
            int OldIndex = _SelectedIndex;

            if (_SelectedIndex < 4)
            {
                NewIndex = (e.X / ItemWidth);
            }
            else if (_SelectedIndex > 3 && _SelectedIndex < (MaximumIndex - 3))
            {
                NewIndex = (e.X / ItemWidth);

                if (NewIndex == 2)
                {
                    NewIndex = OldIndex;
                }
                else if (NewIndex < 2)
                {
                    NewIndex = OldIndex - (2 - NewIndex);
                }
                else if (NewIndex > 2)
                {
                    NewIndex = OldIndex + (NewIndex - 2);
                }
            }
            else
            {
                NewIndex = MaximumIndex - (4 - (e.X / ItemWidth));
            }

            if ((NewIndex < _NumberOfPages) && (!(NewIndex == OldIndex)))
            {
                SelectedIndex = NewIndex;
                if (SelectedIndexChanged != null)
                {
                    SelectedIndexChanged(this, null);
                }
            }
        }

        base.OnMouseDown(e);
    }

}

[DefaultEvent("Scroll")]
class NSVScrollBar : Control
{

    public event ScrollEventHandler Scroll;
    public delegate void ScrollEventHandler(object sender);

    private int _Minimum;
    public int Minimum
    {
        get { return _Minimum; }
        set
        {
            if (value < 0)
            {
                throw new Exception("Property value is not valid.");
            }

            _Minimum = value;
            if (value > _Value)
                _Value = value;
            if (value > _Maximum)
                _Maximum = value;

            InvalidateLayout();
        }
    }

    private int _Maximum = 100;
    public int Maximum
    {
        get { return _Maximum; }
        set
        {
            if (value < 1)
                value = 1;

            _Maximum = value;
            if (value < _Value)
                _Value = value;
            if (value < _Minimum)
                _Minimum = value;

            InvalidateLayout();
        }
    }

    private int _Value;
    public int Value
    {
        get
        {
            if (!ShowThumb)
                return _Minimum;
            return _Value;
        }
        set
        {
            if (value == _Value)
                return;

            if (value > _Maximum || value < _Minimum)
            {
                throw new Exception("Property value is not valid.");
            }

            _Value = value;
            InvalidatePosition();

            if (Scroll != null)
            {
                Scroll(this);
            }
        }
    }

    public double _Percent { get; set; }
    public double Percent
    {
        get
        {
            if (!ShowThumb)
                return 0;
            return GetProgress();
        }
    }

    private int _SmallChange = 1;
    public int SmallChange
    {
        get { return _SmallChange; }
        set
        {
            if (value < 1)
            {
                throw new Exception("Property value is not valid.");
            }

            _SmallChange = value;
        }
    }

    private int _LargeChange = 10;
    public int LargeChange
    {
        get { return _LargeChange; }
        set
        {
            if (value < 1)
            {
                throw new Exception("Property value is not valid.");
            }

            _LargeChange = value;
        }
    }

    private int ButtonSize = 16;
    // 14 minimum
    private int ThumbSize = 24;

    private Rectangle TSA;
    private Rectangle BSA;
    private Rectangle Shaft;

    private Rectangle Thumb;
    private bool ShowThumb;

    private bool ThumbDown;
    public NSVScrollBar()
    {
        SetStyle((ControlStyles)139286, true);
        SetStyle(ControlStyles.Selectable, false);

        Width = 18;

        B1 = new SolidBrush(Color.FromArgb(55, 55, 55));
        B2 = new SolidBrush(Color.FromArgb(35, 35, 35));

        P1 = new Pen(Color.FromArgb(35, 35, 35));
        P2 = new Pen(Color.FromArgb(65, 65, 65));
        P3 = new Pen(Color.FromArgb(55, 55, 55));
        P4 = new Pen(Color.FromArgb(40, 40, 40));
    }

    private GraphicsPath GP1;
    private GraphicsPath GP2;
    private GraphicsPath GP3;

    private GraphicsPath GP4;
    private Pen P1;
    private Pen P2;
    private Pen P3;
    private Pen P4;
    private SolidBrush B1;

    private SolidBrush B2;

    int I1;
    private Graphics G;

    protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
    {
        G = e.Graphics;
        G.Clear(BackColor);

        GP1 = DrawArrow(4, 6, false);
        GP2 = DrawArrow(5, 7, false);

        G.FillPath(B1, GP2);
        G.FillPath(B2, GP1);

        GP3 = DrawArrow(4, Height - 11, true);
        GP4 = DrawArrow(5, Height - 10, true);

        G.FillPath(B1, GP4);
        G.FillPath(B2, GP3);

        if (ShowThumb)
        {
            G.FillRectangle(B1, Thumb);
            G.DrawRectangle(P1, Thumb);
            G.DrawRectangle(P2, Thumb.X + 1, Thumb.Y + 1, Thumb.Width - 2, Thumb.Height - 2);

            int Y = 0;
            int LY = Thumb.Y + (Thumb.Height / 2) - 3;

            for (int I = 0; I <= 2; I++)
            {
                Y = LY + (I * 3);

                G.DrawLine(P1, Thumb.X + 5, Y, Thumb.Right - 5, Y);
                G.DrawLine(P2, Thumb.X + 5, Y + 1, Thumb.Right - 5, Y + 1);
            }
        }

        G.DrawRectangle(P3, 0, 0, Width - 1, Height - 1);
        G.DrawRectangle(P4, 1, 1, Width - 3, Height - 3);
    }

    private GraphicsPath DrawArrow(int x, int y, bool flip)
    {
        GraphicsPath GP = new GraphicsPath();

        int W = 9;
        int H = 5;

        if (flip)
        {
            GP.AddLine(x + 1, y, x + W + 1, y);
            GP.AddLine(x + W, y, x + H, y + H - 1);
        }
        else
        {
            GP.AddLine(x, y + H, x + W, y + H);
            GP.AddLine(x + W, y + H, x + H, y);
        }

        GP.CloseFigure();
        return GP;
    }

    protected override void OnSizeChanged(EventArgs e)
    {
        InvalidateLayout();
    }

    private void InvalidateLayout()
    {
        TSA = new Rectangle(0, 0, Width, ButtonSize);
        BSA = new Rectangle(0, Height - ButtonSize, Width, ButtonSize);
        Shaft = new Rectangle(0, TSA.Bottom + 1, Width, Height - (ButtonSize * 2) - 1);

        ShowThumb = ((_Maximum - _Minimum) > Shaft.Height);

        if (ShowThumb)
        {
            //ThumbSize = Math.Max(0, 14) 'TODO: Implement this.
            Thumb = new Rectangle(1, 0, Width - 3, ThumbSize);
        }

        if (Scroll != null)
        {
            Scroll(this);
        }
        InvalidatePosition();
    }

    private void InvalidatePosition()
    {
        Thumb.Y = Convert.ToInt32(GetProgress() * (Shaft.Height - ThumbSize)) + TSA.Height;
        Invalidate();
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        if (e.Button == System.Windows.Forms.MouseButtons.Left && ShowThumb)
        {
            if (TSA.Contains(e.Location))
            {
                I1 = _Value - _SmallChange;
            }
            else if (BSA.Contains(e.Location))
            {
                I1 = _Value + _SmallChange;
            }
            else
            {
                if (Thumb.Contains(e.Location))
                {
                    ThumbDown = true;
                    base.OnMouseDown(e);
                    return;
                }
                else
                {
                    if (e.Y < Thumb.Y)
                    {
                        I1 = _Value - _LargeChange;
                    }
                    else
                    {
                        I1 = _Value + _LargeChange;
                    }
                }
            }

            Value = Math.Min(Math.Max(I1, _Minimum), _Maximum);
            InvalidatePosition();
        }

        base.OnMouseDown(e);
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        if (ThumbDown && ShowThumb)
        {
            int ThumbPosition = e.Y - TSA.Height - (ThumbSize / 2);
            int ThumbBounds = Shaft.Height - ThumbSize;

            I1 = Convert.ToInt32(((double)ThumbPosition / (double)ThumbBounds) * (_Maximum - _Minimum)) + _Minimum;

            Value = Math.Min(Math.Max(I1, _Minimum), _Maximum);
            InvalidatePosition();
        }

        base.OnMouseMove(e);
    }

    protected override void OnMouseUp(MouseEventArgs e)
    {
        ThumbDown = false;
        base.OnMouseUp(e);
    }

    private double GetProgress()
    {
        return (double)(_Value - _Minimum) / (double)(_Maximum - _Minimum);
    }

}

[DefaultEvent("Scroll")]
class NSHScrollBar : Control
{

    public event ScrollEventHandler Scroll;
    public delegate void ScrollEventHandler(object sender);

    private int _Minimum;
    public int Minimum
    {
        get { return _Minimum; }
        set
        {
            if (value < 0)
            {
                throw new Exception("Property value is not valid.");
            }

            _Minimum = value;
            if (value > _Value)
                _Value = value;
            if (value > _Maximum)
                _Maximum = value;

            InvalidateLayout();
        }
    }

    private int _Maximum = 100;
    public int Maximum
    {
        get { return _Maximum; }
        set
        {
            if (value < 0)
            {
                throw new Exception("Property value is not valid.");
            }

            _Maximum = value;
            if (value < _Value)
                _Value = value;
            if (value < _Minimum)
                _Minimum = value;

            InvalidateLayout();
        }
    }

    private int _Value;
    public int Value
    {
        get
        {
            if (!ShowThumb)
                return _Minimum;
            return _Value;
        }
        set
        {
            if (value == _Value)
                return;

            if (value > _Maximum || value < _Minimum)
            {
                throw new Exception("Property value is not valid.");
            }

            _Value = value;
            InvalidatePosition();

            if (Scroll != null)
            {
                Scroll(this);
            }
        }
    }

    private int _SmallChange = 1;
    public int SmallChange
    {
        get { return _SmallChange; }
        set
        {
            if (value < 1)
            {
                throw new Exception("Property value is not valid.");
            }

            _SmallChange = value;
        }
    }

    private int _LargeChange = 10;
    public int LargeChange
    {
        get { return _LargeChange; }
        set
        {
            if (value < 1)
            {
                throw new Exception("Property value is not valid.");
            }

            _LargeChange = value;
        }
    }

    private int ButtonSize = 16;
    // 14 minimum
    private int ThumbSize = 24;

    private Rectangle LSA;
    private Rectangle RSA;
    private Rectangle Shaft;

    private Rectangle Thumb;
    private bool ShowThumb;

    private bool ThumbDown;
    public NSHScrollBar()
    {
        this._Maximum = 100;
        this._SmallChange = 1;
        this._LargeChange = 10;
        this.ButtonSize = 16;
        this.ThumbSize = 24;
        base.SetStyle(ControlStyles.UserPaint | ControlStyles.Opaque | ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
        base.SetStyle(ControlStyles.Selectable, false);
        base.Height = 18;
        this.B1 = new SolidBrush(Color.FromArgb(25, 25, 25));
        this.B2 = new SolidBrush(Color.FromArgb(5, 5, 5));
        this.P1 = new Pen(Color.FromArgb(5, 5, 5));
        this.P2 = new Pen(Color.FromArgb(35, 35, 35));
        this.P3 = new Pen(Color.FromArgb(25, 25, 25));
        this.P4 = new Pen(Color.FromArgb(10, 10, 10));
    }

    private GraphicsPath GP1;
    private GraphicsPath GP2;
    private GraphicsPath GP3;

    private GraphicsPath GP4;
    private Pen P1;
    private Pen P2;
    private Pen P3;
    private Pen P4;
    private SolidBrush B1;

    private SolidBrush B2;

    int I1;
    private Graphics G;
    protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
    {
        G = e.Graphics;
        G.Clear(BackColor);

        GP1 = DrawArrow(6, 4, false);
        GP2 = DrawArrow(7, 5, false);

        G.FillPath(B1, GP2);
        G.FillPath(B2, GP1);

        GP3 = DrawArrow(Width - 11, 4, true);
        GP4 = DrawArrow(Width - 10, 5, true);

        G.FillPath(B1, GP4);
        G.FillPath(B2, GP3);

        if (ShowThumb)
        {
            G.FillRectangle(B1, Thumb);
            G.DrawRectangle(P1, Thumb);
            G.DrawRectangle(P2, Thumb.X + 1, Thumb.Y + 1, Thumb.Width - 2, Thumb.Height - 2);

            int X = 0;
            int LX = Thumb.X + (Thumb.Width / 2) - 3;

            for (int I = 0; I <= 2; I++)
            {
                X = LX + (I * 3);

                G.DrawLine(P1, X, Thumb.Y + 5, X, Thumb.Bottom - 5);
                G.DrawLine(P2, X + 1, Thumb.Y + 5, X + 1, Thumb.Bottom - 5);
            }
        }

        G.DrawRectangle(P3, 0, 0, Width - 1, Height - 1);
        G.DrawRectangle(P4, 1, 1, Width - 3, Height - 3);
    }

    private GraphicsPath DrawArrow(int x, int y, bool flip)
    {
        GraphicsPath GP = new GraphicsPath();

        int W = 5;
        int H = 9;

        if (flip)
        {
            GP.AddLine(x, y + 1, x, y + H + 1);
            GP.AddLine(x, y + H, x + W - 1, y + W);
        }
        else
        {
            GP.AddLine(x + W, y, x + W, y + H);
            GP.AddLine(x + W, y + H, x + 1, y + W);
        }

        GP.CloseFigure();
        return GP;
    }

    protected override void OnSizeChanged(EventArgs e)
    {
        InvalidateLayout();
    }

    private void InvalidateLayout()
    {
        LSA = new Rectangle(0, 0, ButtonSize, Height);
        RSA = new Rectangle(Width - ButtonSize, 0, ButtonSize, Height);
        Shaft = new Rectangle(LSA.Right + 1, 0, Width - (ButtonSize * 2) - 1, Height);

        ShowThumb = ((_Maximum - _Minimum) > Shaft.Width);

        if (ShowThumb)
        {
            //ThumbSize = Math.Max(0, 14) 'TODO: Implement this.
            Thumb = new Rectangle(0, 1, ThumbSize, Height - 3);
        }

        if (Scroll != null)
        {
            Scroll(this);
        }
        InvalidatePosition();
    }

    private void InvalidatePosition()
    {
        Thumb.X = Convert.ToInt32(GetProgress() * (Shaft.Width - ThumbSize)) + LSA.Width;
        Invalidate();
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        if (e.Button == System.Windows.Forms.MouseButtons.Left && ShowThumb)
        {
            if (LSA.Contains(e.Location))
            {
                I1 = _Value - _SmallChange;
            }
            else if (RSA.Contains(e.Location))
            {
                I1 = _Value + _SmallChange;
            }
            else
            {
                if (Thumb.Contains(e.Location))
                {
                    ThumbDown = true;
                    base.OnMouseDown(e);
                    return;
                }
                else
                {
                    if (e.X < Thumb.X)
                    {
                        I1 = _Value - _LargeChange;
                    }
                    else
                    {
                        I1 = _Value + _LargeChange;
                    }
                }
            }

            Value = Math.Min(Math.Max(I1, _Minimum), _Maximum);
            InvalidatePosition();
        }

        base.OnMouseDown(e);
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        if (ThumbDown && ShowThumb)
        {
            int ThumbPosition = e.X - LSA.Width - (ThumbSize / 2);
            int ThumbBounds = Shaft.Width - ThumbSize;

            I1 = Convert.ToInt32(((double)ThumbPosition / (double)ThumbBounds) * (_Maximum - _Minimum)) + _Minimum;

            Value = Math.Min(Math.Max(I1, _Minimum), _Maximum);
            InvalidatePosition();
        }

        base.OnMouseMove(e);
    }

    protected override void OnMouseUp(MouseEventArgs e)
    {
        ThumbDown = false;
        base.OnMouseUp(e);
    }

    private double GetProgress()
    {
        return (double)(_Value - _Minimum) / (double)(_Maximum - _Minimum);
    }

}

class NSContextMenu : ContextMenuStrip
{

    public NSContextMenu()
    {
        Renderer = new ToolStripProfessionalRenderer(new NSColorTable());
        ForeColor = Color.White;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        e.Graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
        base.OnPaint(e);
    }

}

class NSColorTable : ProfessionalColorTable
{


    private Color BackColor = Color.FromArgb(55, 55, 55);
    public override Color ButtonSelectedBorder
    {
        get { return BackColor; }
    }

    public override Color CheckBackground
    {
        get { return BackColor; }
    }

    public override Color CheckPressedBackground
    {
        get { return BackColor; }
    }

    public override Color CheckSelectedBackground
    {
        get { return BackColor; }
    }

    public override Color ImageMarginGradientBegin
    {
        get { return BackColor; }
    }

    public override Color ImageMarginGradientEnd
    {
        get { return BackColor; }
    }

    public override Color ImageMarginGradientMiddle
    {
        get { return BackColor; }
    }

    public override Color MenuBorder
    {
        get { return Color.FromArgb(25, 25, 25); }
    }

    public override Color MenuItemBorder
    {
        get { return BackColor; }
    }

    public override Color MenuItemSelected
    {
        get { return Color.FromArgb(65, 65, 65); }
    }

    public override Color SeparatorDark
    {
        get { return Color.FromArgb(35, 35, 35); }
    }

    public override Color ToolStripDropDownBackground
    {
        get { return BackColor; }
    }

}

//If you have made it this far it's not too late to turn back, you must not continue on! If you are trying to fullfill some 
//sick act of masochism by studying the source of the ListView then, may god have mercy on your soul.
class NSListView : Control
{

    public class NSListViewItem
    {
        public string Text { get; set; }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public List<NSListViewSubItem> SubItems { get; set; }


        protected Guid UniqueId;
        public NSListViewItem()
        {
            UniqueId = Guid.NewGuid();
        }

        public override string ToString()
        {
            return Text;
        }

        public override bool Equals(object obj)
        {
            if (obj is NSListViewItem)
            {
                return (((NSListViewItem)obj).UniqueId == UniqueId);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

    }

    public class NSListViewSubItem
    {
        public string Text { get; set; }

        public override string ToString()
        {
            return Text;
        }
    }

    public class NSListViewColumnHeader
    {
        public string Text { get; set; }
        public int Width { get; set; }

        public override string ToString()
        {
            return Text;
        }
    }

    private List<NSListViewItem> _Items = new List<NSListViewItem>();
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public NSListViewItem[] Items
    {
        get { return _Items.ToArray(); }
        set
        {
            _Items = new List<NSListViewItem>(value);
            InvalidateScroll();
        }
    }

    private List<NSListViewItem> _SelectedItems = new List<NSListViewItem>();
    public NSListViewItem[] SelectedItems
    {
        get { return _SelectedItems.ToArray(); }
    }

    private List<NSListViewColumnHeader> _Columns = new List<NSListViewColumnHeader>();
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public NSListViewColumnHeader[] Columns
    {
        get { return _Columns.ToArray(); }
        set
        {
            _Columns = new List<NSListViewColumnHeader>(value);
            InvalidateColumns();
        }
    }

    private bool _MultiSelect = true;
    public bool MultiSelect
    {
        get { return _MultiSelect; }
        set
        {
            _MultiSelect = value;

            if (_SelectedItems.Count > 1)
            {
                _SelectedItems.RemoveRange(1, _SelectedItems.Count - 1);
            }

            Invalidate();
        }
    }

    private int ItemHeight = 24;
    public override Font Font
    {
        get { return base.Font; }
        set
        {
            ItemHeight = Convert.ToInt32(Graphics.FromHwnd(Handle).MeasureString("@", Font).Height) + 6;

            if (VS != null)
            {
                VS.SmallChange = ItemHeight;
                VS.LargeChange = ItemHeight;
            }

            base.Font = value;
            InvalidateLayout();
        }
    }

    #region " Item Helper Methods "

    //Ok, you've seen everything of importance at this point; I am begging you to spare yourself. You must not read any further!

    public void AddItem(string text, params string[] subItems)
    {
        List<NSListViewSubItem> Items = new List<NSListViewSubItem>();
        foreach (string I in subItems)
        {
            NSListViewSubItem SubItem = new NSListViewSubItem();
            SubItem.Text = I;
            Items.Add(SubItem);
        }

        NSListViewItem Item = new NSListViewItem();
        Item.Text = text;
        Item.SubItems = Items;

        _Items.Add(Item);
        InvalidateScroll();
    }

    public void RemoveItemAt(int index)
    {
        _Items.RemoveAt(index);
        InvalidateScroll();
    }

    public void RemoveItem(NSListViewItem item)
    {
        _Items.Remove(item);
        InvalidateScroll();
    }

    public void RemoveItems(NSListViewItem[] items)
    {
        foreach (NSListViewItem I in items)
        {
            _Items.Remove(I);
        }

        InvalidateScroll();
    }

    #endregion


    private NSVScrollBar VS;
    public NSListView()
    {
        this._Items = new List<NSListView.NSListViewItem>();
        this._SelectedItems = new List<NSListView.NSListViewItem>();
        this._Columns = new List<NSListView.NSListViewColumnHeader>();
        this._MultiSelect = true;
        this.ItemHeight = 24;
        base.SetStyle(ControlStyles.UserPaint | ControlStyles.Opaque | ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
        base.SetStyle(ControlStyles.Selectable, true);
        this.P1 = new Pen(Color.FromArgb(25, 25, 25));
        this.P2 = new Pen(Color.FromArgb(5, 5, 5));
        this.P3 = new Pen(Color.FromArgb(35, 35, 35));
        this.B1 = new SolidBrush(Color.FromArgb(32, 32, 32));
        this.B2 = new SolidBrush(Color.FromArgb(35, 35, 35));
        this.B3 = new SolidBrush(Color.FromArgb(17, 17, 17));
        this.B4 = new SolidBrush(Color.FromArgb(20, 20, 20));
        this.VS = new NSVScrollBar();
        this.VS.SmallChange = this.ItemHeight;
        this.VS.LargeChange = this.ItemHeight;
        this.VS.Scroll += new NSVScrollBar.ScrollEventHandler(this.HandleScroll);
        this.VS.MouseDown += new MouseEventHandler(this.VS_MouseDown);
        base.Controls.Add(this.VS);
        this.InvalidateLayout();
    }

    protected override void OnSizeChanged(EventArgs e)
    {
        InvalidateLayout();
        base.OnSizeChanged(e);
    }

    private void HandleScroll(object sender)
    {
        Invalidate();
    }

    private void InvalidateScroll()
    {
        VS.Maximum = (_Items.Count * ItemHeight);
        Invalidate();
    }

    private void InvalidateLayout()
    {
        VS.Location = new Point(Width - VS.Width - 1, 1);
        VS.Size = new Size(18, Height - 2);

        Invalidate();
    }

    private int[] ColumnOffsets;
    private void InvalidateColumns()
    {
        int Width = 3;
        ColumnOffsets = new int[_Columns.Count];

        for (int I = 0; I <= _Columns.Count - 1; I++)
        {
            ColumnOffsets[I] = Width;
            Width += Columns[I].Width;
        }

        Invalidate();
    }

    private void VS_MouseDown(object sender, MouseEventArgs e)
    {
        Focus();
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        Focus();

        if (e.Button == System.Windows.Forms.MouseButtons.Left)
        {
            int Offset = Convert.ToInt32(VS.Percent * (VS.Maximum - (Height - (ItemHeight * 2))));
            int Index = ((e.Y + Offset - ItemHeight) / ItemHeight);

            if (Index > _Items.Count - 1)
                Index = -1;

            if (!(Index == -1))
            {
                //TODO: Handle Shift key

                if (ModifierKeys == Keys.Control && _MultiSelect)
                {
                    if (_SelectedItems.Contains(_Items[Index]))
                    {
                        _SelectedItems.Remove(_Items[Index]);
                    }
                    else
                    {
                        _SelectedItems.Add(_Items[Index]);
                    }
                }
                else
                {
                    _SelectedItems.Clear();
                    _SelectedItems.Add(_Items[Index]);
                }
            }

            Invalidate();
        }

        base.OnMouseDown(e);
    }

    private Pen P1;
    private Pen P2;
    private Pen P3;
    private SolidBrush B1;
    private SolidBrush B2;
    private SolidBrush B3;
    private SolidBrush B4;

    private LinearGradientBrush GB1;
    //I am so sorry you have to witness this. I tried warning you. ;.;

    //private Graphics G;
    protected override void OnPaint(PaintEventArgs e)
    {
        ThemeModule.G = e.Graphics;
        ThemeModule.G.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
        ThemeModule.G.Clear(this.BackColor);
        checked
        {
            ThemeModule.G.DrawRectangle(this.P1, 1, 1, base.Width - 3, base.Height - 3);
            int num = (int)Math.Round(unchecked(this.VS.Percent * (double)(checked(this.VS.Maximum - (base.Height - this.ItemHeight * 2)))));
            bool flag = num == 0;
            int num2;
            if (flag)
            {
                num2 = 0;
            }
            else
            {
                num2 = num / this.ItemHeight;
            }
            int num3 = Math.Min(num2 + base.Height / this.ItemHeight, this._Items.Count - 1);
            int num4 = num2;
            int num5 = num3;
            Rectangle rectangle;
            for (int i = num4; i <= num5; i++)
            {
                NSListView.NSListViewItem nSListViewItem = this.Items[i];
                rectangle = new Rectangle(0, this.ItemHeight + i * this.ItemHeight + 1 - num, base.Width, this.ItemHeight - 1);
                float height = ThemeModule.G.MeasureString(nSListViewItem.Text, this.Font).Height;
                int num6 = rectangle.Y + (int)Math.Round(unchecked((double)this.ItemHeight / 2.0 - (double)(height / 2f)));
                bool flag2 = this._SelectedItems.Contains(nSListViewItem);
                if (flag2)
                {
                    bool flag3 = i % 2 == 0;
                    if (flag3)
                    {
                        ThemeModule.G.FillRectangle(this.B1, rectangle);
                    }
                    else
                    {
                        ThemeModule.G.FillRectangle(this.B2, rectangle);
                    }
                }
                else
                {
                    bool flag4 = i % 2 == 0;
                    if (flag4)
                    {
                        ThemeModule.G.FillRectangle(this.B3, rectangle);
                    }
                    else
                    {
                        ThemeModule.G.FillRectangle(this.B4, rectangle);
                    }
                }
                ThemeModule.G.DrawLine(this.P2, 0, rectangle.Bottom, base.Width, rectangle.Bottom);
                bool flag5 = this.Columns.Length > 0;
                if (flag5)
                {
                    rectangle.Width = this.Columns[0].Width;
                    ThemeModule.G.SetClip(rectangle);
                }
                ThemeModule.G.DrawString(nSListViewItem.Text, this.Font, Brushes.Black, 10f, (float)(num6 + 1));
                ThemeModule.G.DrawString(nSListViewItem.Text, this.Font, Brushes.White, 9f, (float)num6);
                bool flag6 = nSListViewItem.SubItems != null;
                if (flag6)
                {
                    int num7 = Math.Min(nSListViewItem.SubItems.Count, this._Columns.Count) - 1;
                    for (int j = 0; j <= num7; j++)
                    {
                        int num8 = this.ColumnOffsets[j + 1] + 4;
                        rectangle.X = num8;
                        rectangle.Width = this.Columns[j].Width;
                        ThemeModule.G.SetClip(rectangle);
                        ThemeModule.G.DrawString(nSListViewItem.SubItems[j].Text, this.Font, Brushes.Black, (float)(num8 + 1), (float)(num6 + 1));
                        ThemeModule.G.DrawString(nSListViewItem.SubItems[j].Text, this.Font, Brushes.White, (float)num8, (float)num6);
                    }
                }
                ThemeModule.G.ResetClip();
            }
            rectangle = new Rectangle(0, 0, base.Width, this.ItemHeight);
            this.GB1 = new LinearGradientBrush(rectangle, Color.FromArgb(30, 30, 30), Color.FromArgb(25, 25, 25), 90f);
            ThemeModule.G.FillRectangle(this.GB1, rectangle);
            ThemeModule.G.DrawRectangle(this.P3, 1, 1, base.Width - 22, this.ItemHeight - 2);
            int y = Math.Min(this.VS.Maximum + this.ItemHeight - num, base.Height);
            int num9 = this._Columns.Count - 1;
            for (int k = 0; k <= num9; k++)
            {
                NSListView.NSListViewColumnHeader nSListViewColumnHeader = this.Columns[k];
                float height = ThemeModule.G.MeasureString(nSListViewColumnHeader.Text, this.Font).Height;
                int num6 = (int)Math.Round(unchecked((double)this.ItemHeight / 2.0 - (double)(height / 2f)));
                int num8 = this.ColumnOffsets[k];
                ThemeModule.G.DrawString(nSListViewColumnHeader.Text, this.Font, Brushes.Black, (float)(num8 + 1), (float)(num6 + 1));
                ThemeModule.G.DrawString(nSListViewColumnHeader.Text, this.Font, Brushes.White, (float)num8, (float)num6);
                ThemeModule.G.DrawLine(this.P2, num8 - 3, 0, num8 - 3, y);
                ThemeModule.G.DrawLine(this.P3, num8 - 2, 0, num8 - 2, this.ItemHeight);
            }
            ThemeModule.G.DrawRectangle(this.P2, 0, 0, base.Width - 1, base.Height - 1);
            ThemeModule.G.DrawLine(this.P2, 0, this.ItemHeight, base.Width, this.ItemHeight);
            ThemeModule.G.DrawLine(this.P2, this.VS.Location.X - 1, 0, this.VS.Location.X - 1, base.Height);
        }
    }

    protected override void OnMouseWheel(MouseEventArgs e)
    {
        int Move = -((e.Delta * SystemInformation.MouseWheelScrollLines / 120) * (ItemHeight / 2));

        int Value = Math.Max(Math.Min(VS.Value + Move, VS.Maximum), VS.Minimum);
        VS.Value = Value;

        base.OnMouseWheel(e);
    }


}