using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WpfUtils.Behaviors
{
    public static class WatermarkBehavior
    {
        #region Watermark

        public static string GetWatermark(UIElement target)
        {
            return (string)target.GetValue(WatermarkProperty);
        }

        public static void SetWatermark(UIElement target, string value)
        {
            target.SetValue(WatermarkProperty, value);
        }

        public static readonly DependencyProperty WatermarkProperty =
            DependencyProperty.RegisterAttached("Watermark", typeof(string), typeof(WatermarkBehavior),
            new UIPropertyMetadata(string.Empty, WatermarkChanged));


        static void WatermarkChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var textBox = d as TextBox;
            if (textBox != null)
            {
                textBox.Text = GetWatermark(textBox);
                textBox.GotFocus += (s, ee) => { textBox.Text = string.Empty; };
                textBox.LostFocus += (s, ee) =>
                                         {
                                             textBox.Text = GetWatermark(textBox);
                                         };
            }
        }

        #endregion

        #region Command

        public static ICommand GetCommand(UIElement target)
        {
            return (ICommand)target.GetValue(CommandProperty);
        }

        public static void SetCommand(UIElement target, ICommand value)
        {
            target.SetValue(CommandProperty, value);
        }

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command", typeof(ICommand), typeof(WatermarkBehavior),
            new UIPropertyMetadata(null, CommandChanged));

        static void CommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var textBox = d as TextBox;
            if (textBox != null)
            {
                textBox.TextChanged += (s, ee) =>
                    {
                        if (textBox.Text != GetWatermark(textBox))
                        {
                            GetCommand(textBox).Execute(textBox.Text);
                        }
                    };
            }
        }

        #endregion
    }
}
