using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace SugmanOEE.Resources
{
    public partial class DataGridResources : ResourceDictionary
    {
        private void DataGridCell_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DataGridCell cell = sender as DataGridCell;
            var grid = VisualTreeHelper.GetParent(cell);

            while (grid.GetType() != typeof(System.Windows.Controls.DataGrid))
            {
                grid = VisualTreeHelper.GetParent(grid);
            }

            if (cell == null || cell.IsEditing || cell.IsReadOnly || !cell.IsSelected)
                return;

            //if (cell.)

            if (!cell.IsFocused)
            {
                cell.Focus();
            }


            ((DataGrid)grid).BeginEdit(e);

            cell.Dispatcher.Invoke(
                   DispatcherPriority.Background,
                   new Action(delegate { })
            );
        }
    }
}
