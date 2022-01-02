using SortingVisualizer.Algorithms;
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

        public List<IVisualizable> Algorithms => algorithmRepository.Algorithms;

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
                Point p0 = new(i, height - array[i]);
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

        IVisualizable algorithm = new NonrecursiveMergeSort();
        CancellationTokenSource cancellation;

        private async Task runVisualization(IVisualizable algorithm, CancellationToken token)
        {
            int[] array = new int[1000];
            Random rnd = new Random();
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = rnd.Next(1000);
            }
            // array = array.OrderBy(x => x).Select(x => x + rnd.Next(50) - 25).ToArray();
            render(await NewFrame(array));
            token.ThrowIfCancellationRequested();
            await foreach (int[] snapshot in algorithm.Run(array, this, token))
            {
                render(snapshot);
                token.ThrowIfCancellationRequested();
            }
        }

        public async Task<int[]> NewFrame(int[] array)
        {
            await Task.Delay(1);
            return array.ToArray();
        }

        private async void ComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            IVisualizable? algorithm = ((ComboBox)sender).SelectedItem as IVisualizable;
            if (algorithm == null) return;
            if (cancellation != null)
            {
                cancellation.Cancel();
            }
            cancellation = new CancellationTokenSource();
            try
            {
                await runVisualization(algorithm, cancellation.Token);
            }
            catch (OperationCanceledException) { }
        }
    }
}
