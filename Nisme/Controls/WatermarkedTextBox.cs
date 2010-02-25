using System.Windows;
using System.Windows.Controls;
public class WatermarkedTextBox
{
    public static readonly DependencyProperty WatermarkProperty = DependencyProperty.RegisterAttached("Watermark", typeof(string), typeof(WatermarkedTextBox));

    public static void SetWatermark(TextBox element, string watermark)
    {
        element.SetValue(WatermarkProperty, watermark);
    }

    public static string GetWatermark(UIElement element)
    {
        return element.GetValue(WatermarkProperty) as string;
    }
}