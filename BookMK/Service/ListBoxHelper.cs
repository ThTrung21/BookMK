using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace BookMK.Service
{
    public static class ListBoxHelper
    {
        public static readonly DependencyProperty SelectedItemsProperty =
            DependencyProperty.RegisterAttached(
                "SelectedItems",
                typeof(IList),
                typeof(ListBoxHelper),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, SelectedItemsChanged));

        public static IList GetSelectedItems(DependencyObject obj)
        {
            return (IList)obj.GetValue(SelectedItemsProperty);
        }

        public static void SetSelectedItems(DependencyObject obj, IList value)
        {
            obj.SetValue(SelectedItemsProperty, value);
        }

        private static void SelectedItemsChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is ListBox listBox)
            {
                listBox.SelectionChanged += (sender, args) =>
                {
                    SetSelectedItems(listBox, listBox.SelectedItems);
                };
            }
        }
    }
}
