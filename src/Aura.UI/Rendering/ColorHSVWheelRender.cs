﻿using Avalonia;
using Avalonia.Media;
using Avalonia.Platform;
using Avalonia.Rendering.SceneGraph;
using Avalonia.Skia;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aura.UI.Rendering
{
    public class ColorHSVWheelRender : AuraDrawOperationBase
    {
        public ColorHSVWheelRender(Rect bounds, IFormattedTextImpl noSKia, float strokeWidth, Color stroke) : base(bounds, noSKia)
        {
            Stroke = stroke;
            StrokeWidth = strokeWidth;
        }

        public float StrokeWidth { get; private set; }
        public Color Stroke { get; private set; }

        public override void Render(IDrawingContextImpl context)
        {
            var canvas = ((ISkiaDrawingContextImpl)context)?.SkCanvas;
            if (canvas == null)
            {
                context.Clear(Colors.White);
                context.DrawText(new SolidColorBrush(Colors.Black),new Point(), NoSkia);
            }
            else
            {
                int width = (int)Bounds.Width;
                int height = (int)Bounds.Height;

                var info = new SKImageInfo(width, height);

                using (SKPaint paint = new SKPaint())
                {
                    SKColor[] colors = new SKColor[8];

                    for (int i = 0; i < colors.Length; i++)
                    {
                        colors[i] = SKColor.FromHsl(i * 360f / 7, 100, 50); //sets the colors
                    }

                    SKPoint center = new SKPoint(info.Rect.MidX, info.Rect.MidY); // creates the center

                    // Create sweep gradient based on center of canvas
                    paint.Shader = SKShader.CreateSweepGradient(center, colors, null);

                    // Draw a circle with a wide line
                    int strokeWidth = 20; //sets the circle width
                                          //paint.Style = SKPaintStyle.Stroke;
                                          // paint.StrokeWidth = strokeWidth;

                    float radius = (Math.Min(info.Width, info.Height) - strokeWidth) / 2; //computes the radius
                    canvas.DrawCircle(center, radius, paint); // draw a circle with its respects parameters
                }

                // Creates the black gradient effect (transparent to black)
                using (SKPaint paint = new SKPaint())
                {
                    SKColor[] colors = { SKColors.White, SKColors.Transparent };

                    SKPoint center = new SKPoint(info.Rect.MidX, info.Rect.MidY); // creates the center

                    int strokeWidth = 20;
                    float radius = (Math.Min(info.Width, info.Height) - strokeWidth) / 2; //computes the radius

                    paint.Shader = SKShader.CreateRadialGradient(center, radius, colors, null, SKShaderTileMode.Repeat);

                    canvas.DrawCircle(center, radius, paint);
                }

                using (SKPaint paint = new SKPaint())
                {
                    var colorStroke = Stroke.ToSKColor();

                    SKPoint center = new SKPoint(info.Rect.MidX, info.Rect.MidY); // creates the center
                    float radius = (Math.Min(info.Width, info.Height) - StrokeWidth) / 2; //computes the radius

                    paint.Shader = SKShader.CreateColor(SKColors.Black);

                    paint.Style = SKPaintStyle.Stroke;
                    paint.StrokeWidth = StrokeWidth;

                    canvas.DrawCircle(center, radius, paint);
                }
            }
        }
    }
}
