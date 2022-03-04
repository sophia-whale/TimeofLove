using System;
using NUnit.Framework;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using TabbedTemplate.Converters;
using TabbedTemplate.Models;
using TabbedTemplate.Utils;
using Xamarin.Forms;

namespace TabbedTemplate.UnitTest.Converters {
    public class SKPaintSurfaceEventArgsToItemConverterTest {
        [Test]
        public void TestConvertBack()
        {
            var skPaintSurfaceEventArgsToItemConverter =
                new SKPaintSurfaceEventArgsToItemConverter();
            Assert.Catch<DoNotCallThisException>(() =>
                skPaintSurfaceEventArgsToItemConverter.ConvertBack(null,
                    null, null, null));
        }
        
        public void TestConvertFailed()
        {
            var skPaintSurfaceEventArgsToItemConverter =
                new SKPaintSurfaceEventArgsToItemConverter();
            Assert.IsNull(
                skPaintSurfaceEventArgsToItemConverter.Convert(new object(),
                    null, null, null));

            var skPaintSurfaceEventArgsToConvert =
                new SKPaintSurfaceEventArgs(null, new SKImageInfo());
            Assert.IsNull(skPaintSurfaceEventArgsToItemConverter.Convert(
                skPaintSurfaceEventArgsToConvert, null, null, null));
        }
        
        public void TestConvertSucceeded()
        {
            var skPaintSurfaceEventArgsToItemConverter =
                new SKPaintSurfaceEventArgsToItemConverter();
            var sKEventItemToReturn = new SKEventItem();
            SKImageInfo skImageInfo = new SKImageInfo(100, 100);
            SKSurface skSurface = SKSurface.Create(new SKPixmap(skImageInfo, IntPtr.Zero));
            var skPaintSurfaceEventArgsToConvert =
                new SKPaintSurfaceEventArgs(skSurface, skImageInfo);
            Assert.AreSame(sKEventItemToReturn,
                skPaintSurfaceEventArgsToItemConverter.Convert(
                    skPaintSurfaceEventArgsToConvert, null, null, null));
        }
    }
}