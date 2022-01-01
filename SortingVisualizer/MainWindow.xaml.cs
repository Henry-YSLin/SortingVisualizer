using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SortingVisualizer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void render(int[] array)
        {
            var gg = new GeometryGroup();

            for (int i = 0; i < array.Length; i++)
            {
                Point p0 = new Point(i, 1000 - array[i]);
                Point p1 = new Point(i, 1000);
                var lineGeometry = new LineGeometry(p0, p1);

                gg.Children.Add(lineGeometry);
            }

            var stroke = new SolidColorBrush(Colors.Red);

            gg.Freeze();
            stroke.Freeze();

            Content = new Path()
            {
                Data = gg,
                Stroke = stroke,
                SnapsToDevicePixels = true
            };
        }

        IVisualizable algorithm = new PartitionedSort();

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            int[] array = new int[1000];
            Random rnd = new Random();
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = rnd.Next(1000);
            }
            await foreach (int[] snapshot in algorithm.Run(array, async array =>
            {
                await Task.Delay(1);
                return (int[])array.Clone();
            }))
            {
                render(snapshot);
                await Task.Delay(1);
            }
        }
    }
}
