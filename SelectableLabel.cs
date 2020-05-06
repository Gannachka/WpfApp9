using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfApp7_8
{
    class SelectableLabel : Label
    {
        
        public SelectableLabel() : base()
        {
            
            this.Background = new SolidColorBrush(Colors.Red);
        }
        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsSelected.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register("IsSelected", typeof(bool), typeof(SelectableLabel), new UIPropertyMetadata(false, IsSelectedChanged));

        private static void IsSelectedChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            SelectableLabel label = sender as SelectableLabel;
            if (label != null)
            {
                if (label.IsSelected)
                {
                    label.Background = new SolidColorBrush(Colors.Green);
                }
                else
                {
                    label.Background = new SolidColorBrush(Colors.Red);
                }

            }
        }
    }
} 