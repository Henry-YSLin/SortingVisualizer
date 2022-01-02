using SortingVisualizer.Algorithms;
using SortingVisualizer.ArrayGenerators;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SortingVisualizer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IVisualizer
    {
        private readonly List<LineGeometry> lines = new();
        private readonly AlgorithmRepository algorithmRepository = new();
        private readonly ArrayGeneratorRepository arrayGeneratorRepository = new();

        public List<IVisualizable> Algorithms => algorithmRepository.Algorithms;
        public List<ArrayGenerator> Generators => arrayGeneratorRepository.Generators;
        public bool IsPaused { get; set; }
        public IVisualizable Algorithm { get; set; }
        public ArrayGenerator Generator { get; set; }
        public int ArrayLength { get; set; }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void render(int[] array)
        {
            var group = new GeometryGroup();
            lines.Clear();
            var height = GridVisualizer.ActualHeight;

            for (int i = 0; i < array.Length; i++)
            {
                Point p0 = new(i, height - array[i] * height / array.Length);
                Point p1 = new(i, height);
                var lineGeometry = new LineGeometry(p0, p1);

                group.Children.Add(lineGeometry);
                lines.Add(lineGeometry);
            }

            SolidColorBrush stroke = new(Colors.Red);

            GridVisualizer.Children.Clear();
            GridVisualizer.Children.Add(new Path()
            {
                Data = group,
                Stroke = stroke,
                SnapsToDevicePixels = true
            });
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
}
