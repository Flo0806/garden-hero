using Avalonia.Controls.Presenters;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.VisualTree;
using System.Collections.ObjectModel;
using Avalonia.Controls.Metadata;
using Avalonia.Threading;
using System.Collections.Specialized;

namespace SD.Controls.Controls
{
    [TemplatePart("PART_ItemsControl", typeof(ItemsControl))]
    [TemplatePart("PART_Border", typeof(Border))]
    public partial class MonthSelection: TemplatedControl
    {
        protected ItemsControl? ItemsControlPart { get; private set; }

        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);
            ActiveMonthsProperty.Changed.AddClassHandler<MonthSelection>(ActiveMonthsChanged);

            ActiveMonths = new ObservableCollection<MonthItem>();
            ActiveMonths.Add(new MonthItem() { Name = "JAN", State = MonthState.Empty });
            ActiveMonths.Add(new MonthItem() { Name = "FEB", State = MonthState.Empty });

            ItemsControlPart = e.NameScope.Get<ItemsControl>("PART_ItemsControl");
            if (ItemsControlPart != null)
            {
                ItemsControlPart.Loaded += ItemsControlPart_Loaded;
                ItemsControlPart.Items.CollectionChanged += Items_CollectionChanged;
            } else throw new ArgumentNullException("Cannot find ItemsControl");
        }

        #region Events
        private void Items_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (var item in e.OldItems)
                {
                    Dispatcher.UIThread.InvokeAsync(() =>
                    {
                        var itemContainer = ItemsControlPart!.ContainerFromItem(item);
                        if (itemContainer != null)
                        {
                            var borderControl = FindControl<Border>(itemContainer as Control, "PART_Border");
                            if (borderControl != null)
                                borderControl.PointerPressed -= BorderControl_PointerPressed;

                        }
                    });
                }
            }

            if (e.NewItems != null)
            {
                foreach (var item in e.NewItems)
                {
                    Dispatcher.UIThread.InvokeAsync(() =>
                    {
                        var itemContainer = ItemsControlPart!.ContainerFromItem(item);
                        if (itemContainer != null)
                        {
                            var borderControl = FindControl<Border>(itemContainer as Control, "PART_Border");
                            if (borderControl != null)
                            {
                                borderControl.PointerPressed += BorderControl_PointerPressed;
                                borderControl.Classes.Clear();
                                borderControl.Classes.Add("Empty");
                            }
                        }
                    });
                }

                UpdateArray();
            }
        }

        private static void ActiveMonthsChanged(MonthSelection sender, AvaloniaPropertyChangedEventArgs e)
        {
            sender.UpdateArray();
        }

        private void ItemsControlPart_Loaded(object? sender, global::Avalonia.Interactivity.RoutedEventArgs e)
        {
            for (int i = 0; i < ItemsControlPart!.Items.Count; i++)
            {
                var borderControl = FindControl<Border>(ItemsControlPart!.ContainerFromIndex(i)!, "PART_Border");
                if (borderControl != null)
                {
                    borderControl.PointerPressed += BorderControl_PointerPressed;
                }
            }
        }

        private void BorderControl_PointerPressed(object? sender, PointerPressedEventArgs e)
        {
            var currentItem = FindParentOfType<MonthItem>(sender as Control);
            if (currentItem != null)
            {
                SetMonthItemsState(currentItem);
            }
        }
        #endregion

        #region Dependency Properties
        public static readonly StyledProperty<int[]> ArrayDataProperty =
        AvaloniaProperty.Register<MonthSelection, int[]>(nameof(ArrayData), defaultBindingMode: Avalonia.Data.BindingMode.OneWayToSource);

        public int[] ArrayData
        {
            get => GetValue(ArrayDataProperty);
            set => SetValue(ArrayDataProperty, value);
        }

        public static readonly StyledProperty<ObservableCollection<MonthItem>> ActiveMonthsProperty =
            AvaloniaProperty.Register<MonthSelection, ObservableCollection<MonthItem>>(nameof(ActiveMonths), new ObservableCollection<MonthItem>());

        public ObservableCollection<MonthItem> ActiveMonths
        {
            get => GetValue(ActiveMonthsProperty);
            set => SetValue(ActiveMonthsProperty, value);
        }
        #endregion

        #region Helper
        private void UpdateArray()
        {
            var oldArray = ArrayData;
            if (ActiveMonths == null)
            {
                ArrayData = [];
                return;
            }
            var newArray = new int[ActiveMonths.Count];

            for (int i = 0; i < ActiveMonths.Count; i++)
            {
                newArray[i] = (int)ActiveMonths[i].State;
            }
            ArrayData = newArray;
        }

        private void UpdateColorByState(MonthItem monthItem)
        {
            var borderControl = FindControl<Border>(ItemsControlPart!.ContainerFromItem(monthItem)!, "PART_Border");
            if (borderControl != null)
            {
                string currentClass = monthItem.State == MonthState.Empty ? "Empty" : monthItem.State == MonthState.HalfMonth ? "Half" : "Full";
                borderControl.Classes.Clear();
                borderControl.Classes.Add(currentClass);
            }
        }

        private void SetMonthItemsState(MonthItem monthItem)
        {
            switch(monthItem.State)
            {
                case MonthState.Empty:
                    monthItem.State = MonthState.HalfMonth;
                    UpdateColorByState(monthItem);
                    break;
                case MonthState.HalfMonth:
                    monthItem.State = MonthState.FullMonth;
                    UpdateColorByState(monthItem);
                    break;
                case MonthState.FullMonth:
                    monthItem.State = MonthState.Empty;
                    UpdateColorByState(monthItem);
                    break;
            }

            UpdateArray();
        }

        private static T? FindControl<T>(Visual visual, string name) where T : Control
        {
            if (visual is T control && control.Name == name)
            {
                var test = visual.GetType();
                return control;
            }

            var children = visual.GetVisualChildren();
            foreach (var child in children)
            {
                var result = FindControl<T>(child, name);
                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }

        private static T? FindParentOfType<T>(Visual visual) where T : class
        {
            Visual? parent = visual.GetVisualParent();
            while (parent != null) 
            {
                if (parent is ContentPresenter cp && cp.Content is T correctType)
                {
                    return correctType;
                }
                parent = parent.GetVisualParent();
            }
            return null;
        }
        #endregion
    }
}
