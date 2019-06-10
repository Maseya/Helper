namespace Maseya.Controls
{
    using System;
    using System.ComponentModel;
    using System.Drawing;

    public interface IGraphicsRenderer : IComponent
    {
        event EventHandler Redraw;

        void Draw(Graphics graphics);
    }
}
