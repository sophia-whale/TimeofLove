using System;
using SkiaSharp.Views.Forms;
using TabbedTemplate.Models;
using Xamarin.Forms;

namespace TabbedTemplate.Views
{
    public class SKRenderView : SKCanvasView
    {
        public static readonly BindableProperty RenderProperty = BindableProperty.Create(
            nameof(Render),
            typeof(Renders.IRender),
            typeof(SKRenderView),
            null,
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: (bindable, oldValue, newValue) =>
            {
                ((SKRenderView)bindable).RendererChanged((Renders.IRender)oldValue, (Renders.IRender)newValue);
            });

        // callback appelée lors de la modificiation de la propriété "Renderer"
        // 修改“Renderer”属性时调用的回调
        void RendererChanged(Renders.IRender currentRenderer, Renders.IRender newRenderer)
        {
            if (currentRenderer != newRenderer)
            {
                // détacher l'événement de l'ancien renderer
                // 从旧渲染器中分离事件
                if (currentRenderer != null)
                    currentRenderer.RefreshRequested -= Renderer_RefreshRequested;

                // attacher l'événement du nouveau renderer
                // 附加新渲染器的事件
                if (newRenderer != null)
                    newRenderer.RefreshRequested += Renderer_RefreshRequested;

                // rafraichir le contrôle
                // 刷新控制
                InvalidateSurface();
            }
        }

        // Provoque le raffraichissement lors d'un événement déclenché par l'interface ISKRenderer
        // 在 ISKRenderer 接口触发的事件期间导致刷新
        void Renderer_RefreshRequested(object sender, EventArgs e)
        {
            InvalidateSurface();
        }

        protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
        {
            Render.PaintSurface(e.Surface, e.Info);
        }

        public Renders.IRender Render
        {
            get { return (Renders.IRender)GetValue(RenderProperty); }
            set { SetValue(RenderProperty, value); }
        }
    }
}