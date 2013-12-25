using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using MathUtils.Collections;

namespace WpfUtils.DataGridCollection
{
    /// <summary>
    /// Interaction logic for DataGridCollectionControl.xaml
    /// </summary>
    public partial class DataGridCollectionControl
    {
        public DataGridCollectionControl()
        {
            InitializeComponent();
        }

        #region Matrix

        /// <summary>
        /// Matrix Dependency Property
        /// </summary>
        public static readonly DependencyProperty MatrixProperty =
            DependencyProperty.Register("Matrix", typeof(IMatrix), typeof(DataGridCollectionControl),
                new FrameworkPropertyMetadata(OnMatrixChanged));

        /// <summary>
        /// Gets or sets the Matrix property. This dependency property 
        /// indicates the content of the PivotItem.
        /// </summary>
        public IMatrix Matrix
        {
            get { return (IMatrix)GetValue(MatrixProperty); }
            set { SetValue(MatrixProperty, value); }
        }

        /// <summary>
        /// Handles changes to the Matrix property.
        /// </summary>
        /// <param name="d">PivotItem</param>
        /// <param name="e">DependencyProperty changed event arguments</param>
        private static void OnMatrixChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var mextrix = e.NewValue as IMatrix;
            var dataGridCollectionControl = d as DataGridCollectionControl;

            if ((mextrix == null) || (dataGridCollectionControl == null))
            {
                return;
            }

            for (var i = 0; i < mextrix.ColumnCount; i++)
            {
                var propertyPathName = "Item[" + i + "]";

                var bind = new Binding {Path = new PropertyPath(propertyPathName), Mode = BindingMode.OneWay};

                var cbc = new CustomBoundColumn
                    {
                        Header = "Sw_" + i,
                        Binding = bind,
                        TemplateName = "CustomTemplate",
                        SortMemberPath = propertyPathName
                    };

                dataGridCollectionControl.MyDataGrid.Columns.Add(cbc);


                //var col = new DataGridTemplateColumn();
                //col.Header = "Sw_" + i;
                //var propertyPathName = "Item[" + i + "]";
                //col.SortMemberPath = propertyPathName;
                ////col.Binding = new Binding(path:"Item["+i+"]");
                //dataGridCollectionControl.MyDataGrid.Columns.Add(col);

                //// First: create and add the data template to the parent control
                //var dt = new DataTemplate(typeof(TextBox));
                //col.CellTemplate = dt;

                //// Second: create and add the text box to the data template
                //var txtElement = new FrameworkElementFactory(typeof(TextBox));
                //dt.VisualTree = txtElement;

                //// Create binding
                //var bind = new Binding();
                //bind.Path = new PropertyPath(propertyPathName);
                //bind.Mode = BindingMode.TwoWay;

                //// Third: set the binding in the text box
                //txtElement.SetBinding(TextBox.TextProperty, bind);
                //txtElement.SetValue(FontSizeProperty, 18.0);
            }
        }

        private static void OnMatrixChangedo(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var mextrix = e.NewValue as IMatrix;
            var dataGridCollectionControl = d as DataGridCollectionControl;

            if ((mextrix == null) || (dataGridCollectionControl == null))
            {
                return;
            }

            for (var i = 0; i < mextrix.ColumnCount; i++)
            {
                var col = new DataGridTemplateColumn();
                col.Header = "Sw_" + i;
                var propertyPathName = "Item[" + i + "]";
                col.SortMemberPath = propertyPathName;
                //col.Binding = new Binding(path:"Item["+i+"]");
                dataGridCollectionControl.MyDataGrid.Columns.Add(col);

                // First: create and add the data template to the parent control
                var dt = new DataTemplate(typeof(TextBox));
                col.CellTemplate = dt;

                // Second: create and add the text box to the data template
                var txtElement = new FrameworkElementFactory(typeof(TextBox));
                dt.VisualTree = txtElement;

                // Create binding
                var bind = new Binding();
                bind.Path = new PropertyPath(propertyPathName);
                bind.Mode = BindingMode.TwoWay;

                // Third: set the binding in the text box
                txtElement.SetBinding(TextBox.TextProperty, bind);
                txtElement.SetValue(FontSizeProperty, 18.0);
            }
        }

        #endregion

        class CustomBoundColumn : DataGridBoundColumn
        {
            public string TemplateName { get; set; }

            protected override FrameworkElement GenerateElement(DataGridCell cell, object dataItem)
            {
                var binding = new Binding(((Binding)Binding).Path.Path)
                    {
                        Source = dataItem,
                        Mode = BindingMode.OneWay
                    };

                var content = new ContentControl();
                content.ContentTemplate = (DataTemplate)cell.FindResource(TemplateName);
                content.SetBinding(ContentProperty, binding);
                return content;
            }

            protected override FrameworkElement GenerateEditingElement(DataGridCell cell, object dataItem)
            {
                return GenerateElement(cell, dataItem);
            }
        }
    }
}
