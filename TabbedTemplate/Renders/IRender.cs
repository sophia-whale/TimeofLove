using System;
using GalaSoft.MvvmLight;
using SkiaSharp;

namespace TabbedTemplate.Renders
{
    public interface IRender
    {
        void PaintSurface(SKSurface surface, SKImageInfo info);
        event EventHandler RefreshRequested;
    }
}