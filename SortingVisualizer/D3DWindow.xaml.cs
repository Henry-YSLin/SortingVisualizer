using SortingVisualizer.Algorithms;
using SortingVisualizer.ArrayGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

using D2D = SharpDX.Direct2D1;
using SharpDX.Direct3D;
using SharpDX.Mathematics.Interop;
using DXGI = SharpDX.DXGI;
using D3D11 = SharpDX.Direct3D11;
using D3D9 = SharpDX.Direct3D9;
using System.Windows.Interop;
using System.Windows.Media;
using System.Runtime.InteropServices;
using System.Windows.Threading;

namespace SortingVisualizer
{
    /// <summary>
    /// Interaction logic for D3DWindow.xaml
    /// </summary>
    public partial class D3DWindow : Window, IVisualizer
    {
        private readonly AlgorithmRepository algorithmRepository = new();
        private readonly ArrayGeneratorRepository arrayGeneratorRepository = new();

        public List<IVisualizable> Algorithms => algorithmRepository.Algorithms;
        public List<ArrayGenerator> Generators => arrayGeneratorRepository.Generators;
        public bool IsPaused { get; set; }
        public IVisualizable Algorithm { get; set; }
        public ArrayGenerator Generator { get; set; }
        public int ArrayLength { get; set; }

        private D3D9.Texture renderTarget;
        private D2D.RenderTarget d2DRenderTarget;

        public D3DWindow()
        {
            InitializeComponent();
        }

        private void CreateAndBindTargets()
        {
            var width = Math.Max((int)ActualWidth, 100);
            var height = Math.Max((int)ActualHeight, 100);

            var renderDesc = new D3D11.Texture2DDescription
            {
                BindFlags = D3D11.BindFlags.RenderTarget | D3D11.BindFlags.ShaderResource,
                Format = DXGI.Format.B8G8R8A8_UNorm,
                Width = width,
                Height = height,
                MipLevels = 1,
                SampleDescription = new DXGI.SampleDescription(1, 0),
                Usage = D3D11.ResourceUsage.Default,
                OptionFlags = D3D11.ResourceOptionFlags.Shared,
                CpuAccessFlags = D3D11.CpuAccessFlags.None,
                ArraySize = 1
            };

            var device = new D3D11.Device(DriverType.Hardware, D3D11.DeviceCreationFlags.BgraSupport);

            var renderTarget = new D3D11.Texture2D(device, renderDesc);

            var surface = renderTarget.QueryInterface<DXGI.Surface>();

            var d2DFactory = new D2D.Factory();

            var renderTargetProperties =
                new D2D.RenderTargetProperties(new D2D.PixelFormat(DXGI.Format.Unknown, D2D.AlphaMode.Premultiplied));

            d2DRenderTarget = new D2D.RenderTarget(d2DFactory, surface, renderTargetProperties);

            SetRenderTarget(renderTarget);

            device.ImmediateContext.Rasterizer.SetViewport(0, 0, (int)ActualWidth, (int)ActualHeight);
        }

        private void SetRenderTarget(D3D11.Texture2D target)
        {
            var format = TranslateFormat(target);
            var handle = GetSharedHandle(target);

            var presentParams = GetPresentParameters();
            var createFlags = D3D9.CreateFlags.HardwareVertexProcessing | D3D9.CreateFlags.Multithreaded |
                              D3D9.CreateFlags.FpuPreserve;

            var d3DContext = new D3D9.Direct3DEx();
            var d3DDevice = new D3D9.DeviceEx(d3DContext, 0, D3D9.DeviceType.Hardware, IntPtr.Zero, createFlags,
                presentParams);

            renderTarget = new D3D9.Texture(d3DDevice, target.Description.Width, target.Description.Height, 1,
                D3D9.Usage.RenderTarget, format, D3D9.Pool.Default, ref handle);

            using (var surface = renderTarget.GetSurfaceLevel(0))
            {
                d3dimg.Lock();
                d3dimg.SetBackBuffer(D3DResourceType.IDirect3DSurface9, surface.NativePointer);
                d3dimg.Unlock();
            }
        }

        private static D3D9.PresentParameters GetPresentParameters()
        {
            var presentParams = new D3D9.PresentParameters();

            presentParams.Windowed = true;
            presentParams.SwapEffect = D3D9.SwapEffect.Discard;
            presentParams.DeviceWindowHandle = NativeMethods.GetDesktopWindow();
            presentParams.PresentationInterval = D3D9.PresentInterval.Default;

            return presentParams;
        }

        private IntPtr GetSharedHandle(D3D11.Texture2D texture)
        {
            using (var resource = texture.QueryInterface<DXGI.Resource>())
            {
                return resource.SharedHandle;
            }
        }

        private static D3D9.Format TranslateFormat(D3D11.Texture2D texture)
        {
            switch (texture.Description.Format)
            {
                case SharpDX.DXGI.Format.R10G10B10A2_UNorm:
                    return D3D9.Format.A2B10G10R10;
                case SharpDX.DXGI.Format.R16G16B16A16_Float:
                    return D3D9.Format.A16B16G16R16F;
                case SharpDX.DXGI.Format.B8G8R8A8_UNorm:
                    return D3D9.Format.A8R8G8B8;
                default:
                    return D3D9.Format.Unknown;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CreateAndBindTargets();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
        }

        private void render(int[] array)
        {
            d2DRenderTarget.BeginDraw();

            var width = d2DRenderTarget.PixelSize.Width;
            var height = d2DRenderTarget.PixelSize.Height;

            var dataBrush = new D2D.SolidColorBrush(d2DRenderTarget, new RawColor4(1, 0, 0, 1));

            d2DRenderTarget.Clear(null);

            if (array == null || array.Length <= 0) return;

            var length = array.Length;

            for (int i = 0; i < length; i++)
            {
                var val = array[i];
                d2DRenderTarget.FillRectangle(new RawRectangleF(i * width / length, (length - val) * height / length, (i + 1) * width / length, height), dataBrush);
            }

            d2DRenderTarget.Flush();

            d2DRenderTarget.EndDraw();

            d3dimg.Lock();

            d3dimg.AddDirtyRect(new Int32Rect(0, 0, d3dimg.PixelWidth, d3dimg.PixelHeight));

            d3dimg.Unlock();
        }

        CancellationTokenSource cancellation;

        private async Task runVisualization(CancellationToken token)
        {
            int[] array = Generator.Generate(ArrayLength);
            render(array);
            token.ThrowIfCancellationRequested();
            await foreach (int[] snapshot in Algorithm.Run(array, this, token))
            {
                render(snapshot);
                token.ThrowIfCancellationRequested();
            }
        }

        public async Task<int[]> NewFrame(int[] array)
        {
            await Task.Delay(1);
            while (IsPaused)
                await Task.Delay(25);
            return array.ToArray();
        }

        private async void ButtonVisualize_Click(object sender, RoutedEventArgs e)
        {
            if (Algorithm == null) return;
            if (cancellation != null)
            {
                cancellation.Cancel();
            }
            cancellation = new CancellationTokenSource();
            try
            {
                await runVisualization(cancellation.Token);
            }
            catch (OperationCanceledException) { }
        }
    }

    public static class NativeMethods
    {
        [DllImport("user32.dll", SetLastError = false)]
        public static extern IntPtr GetDesktopWindow();
    }
}
