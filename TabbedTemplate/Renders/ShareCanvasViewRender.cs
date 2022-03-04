using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using GalaSoft.MvvmLight;
using SkiaSharp;
using Syncfusion.DocIO.DLS;
using TabbedTemplate.Extensions;
using TabbedTemplate.Models;
using TabbedTemplate.Utils;
using TabbedTemplate.ViewModels;

namespace TabbedTemplate.Renders
{
    public class ShareCanvasViewRender : ViewModelBase, IRender
    {
        private SKBitmap _bitmap = null;
        private Diary _diary;
        private SKImage _image;
        private SKPaint _titleTextPaint;
        private SKPaint _titleShadowPaint;
        private SKPaint _contentTextPaint;
        private SKPaint _contentShadowPaint;

        public Diary Diary
        {
            get => _diary;
            set
            {
                if (_diary != value)
                {
                    _diary = value;
                    RefreshRequested?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public SKBitmap Bitmap
        {
            get => _bitmap;
            set
            {
                if (_bitmap != value)
                {
                    _bitmap = value;
                    RefreshRequested?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public SKImage Image
        {
            get => _image;
            set => Set(nameof(Image), ref _image, value);
        }

        public SKPaint TitleTextPaint
        {
            get => _titleTextPaint;
            set => Set(nameof(TitleTextPaint), ref _titleTextPaint, value);
        }

        public SKPaint TitleShadowPaint
        {
            get => _titleShadowPaint;
            set => Set(nameof(TitleShadowPaint), ref _titleShadowPaint, value);
        }

        public SKPaint ContentTextPaint
        {
            get => _contentTextPaint;
            set => Set(nameof(ContentTextPaint), ref _contentTextPaint, value);
        }

        public SKPaint ContentShadowPaint
        {
            get => _contentShadowPaint;
            set => Set(nameof(ContentShadowPaint), ref _contentShadowPaint, value);
        }

        public void PaintSurface(SKSurface surface, SKImageInfo info)
        {
            SKCanvas canvas = surface.Canvas;
            if (_titleShadowPaint == null)
            {
                _titleShadowPaint = new SKPaint
                {
                    Style = SKPaintStyle.Fill,
                    TextSize = 120,
                    FakeBoldText = true,
                    TextEncoding = SKTextEncoding.Utf32,
                    Color = SKColors.Black
                };

                _titleTextPaint = new SKPaint
                {
                    Style = SKPaintStyle.Fill,
                    TextSize = 120,
                    FakeBoldText = true,
                    TextEncoding = SKTextEncoding.Utf32,
                    Color = SKColors.Pink
                };

                _contentShadowPaint = new SKPaint
                {
                    Style = SKPaintStyle.Fill,
                    TextSize = 50,
                    FakeBoldText = true,
                    TextEncoding = SKTextEncoding.Utf32,
                    Color = SKColors.Black
                };

                _contentTextPaint = new SKPaint
                {
                    Style = SKPaintStyle.Fill,
                    TextSize = 50,
                    FakeBoldText = true,
                    TextEncoding = SKTextEncoding.Utf32,
                    Color = SKColors.White
                };
                _bitmap = BitmapExtensions.LoadBitmapResource(
                    typeof(ShareCanvasViewRender),
                    "TabbedTemplate.Resources.hearts.png");
            }
            SKPaint bitmapPaint = new() { Style = SKPaintStyle.Stroke };
            canvas.DrawBitmap(_bitmap, info.Rect, bitmapPaint);
            float x = 50;
            float y = _titleTextPaint.TextSize;
            var fontManager = SKFontManager.Default;
            var emojiTypeface = fontManager.MatchCharacter('时');
            _titleTextPaint.Typeface = emojiTypeface;
            _titleShadowPaint.Typeface = emojiTypeface;
            _contentTextPaint.Typeface = emojiTypeface;
            _contentShadowPaint.Typeface = emojiTypeface;

            //draw title
            canvas.Translate(10, 10);
            canvas.DrawText(Diary.Title, x, y, _titleShadowPaint);
            canvas.Translate(-10, -10);
            canvas.DrawText(Diary.Title, x, y, _titleTextPaint);

            y = 3 * y;
            string outputString = IgnoreVoidElementsInHTML(Diary.Content);
            SKRect area = SKRect.Create(x, y);

            //draw content
            canvas.Save();
            canvas.Translate(5, 5);

            DrawText(canvas, outputString, area, _contentShadowPaint);
            DrawText(canvas, outputString, area, _contentTextPaint);
            //canvas.DrawText(outputString, x, y, _contentShadowPaint);
            canvas.Restore();
            //canvas.DrawText(outputString, x, y, _contentTextPaint);
            canvas.Save();
            _image = surface.Snapshot();
        }

        public event EventHandler RefreshRequested;

        /// <summary>
        /// 去除html标签。
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        public string IgnoreVoidElementsInHTML(string inputString)
        {
            string pattern = "<(.+?)>";
            string replacement = "";
            inputString = Regex.Replace(inputString, pattern, replacement);
            return inputString;
        }

        private void DrawText(SKCanvas canvas, string text, SKRect area, SKPaint paint)
        {
            float lineHeight = paint.TextSize * 1.2f;
            var spaceWidth = paint.MeasureText(" ");
            string pattern1 = "<(.+?)>";
            string replacement = "";
            var pureText = Regex.Replace(text, pattern1, replacement);
            pureText = pureText.Replace("&nbsp;", " ");
            var length = 360 / spaceWidth;
            var lines = SplitLines(pureText, length, spaceWidth);
            var height = lines.Count() * lineHeight;

            var y = 2 * _titleTextPaint.TextSize;

            foreach (var line in lines)
            {
                y += lineHeight;
                var x = area.MidX - line.Width / 2;
                canvas.DrawText(line.Value, 50, y, paint);
            }
        }

        public static Line[] SplitLines(string text, float length, float spaceWidth)
        {
            StringBuilder result = new StringBuilder();

            int len = 0;
            int star = 0;
            int templen = 0;
            int end = 0;
            int i = 0;
            while (i < text.Length)
            {
                byte[] byte_len = Encoding.Default.GetBytes(text.Substring(i, 1));
                if (byte_len.Length > 1)
                {
                    len += 2;
                }
                else
                {
                    len += 1;
                }
                if (len > length)
                {
                    templen = i - star;
                    result.Append(text.Substring(star, templen) + "\n");
                    star = i;
                    len = 0;
                    end = i;
                }
                else
                {
                    i++;
                }
            }

            if (end <= text.Length - 1)
            {
                result.Append(text.Substring(end));
            }
            var lines = result.ToString().Split('\n');
            return lines.SelectMany((line) =>
            {
                var result = new List<Line>();
                result.Add(new Line()
                {
                    Value = line,
                    Width = line.Length * spaceWidth,
                });

                return result.ToArray();
            }).ToArray();
        }

        public class Line
        {
            public string Value { get; set; }
            public float Width { get; set; }
        }
    }
}