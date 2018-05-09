using System;
using System.Collections.Specialized;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Interactivity;

namespace LogoFX.Tools.TemplateGenerator.Shared.Behaviors
{
    public class WrappingCollectionSelectorControlBehavior : Behavior<Selector>
    {
        public Binding SelectedItemBinding { get; set; }

        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.Loaded += AssociatedObject_Loaded;
            AssociatedObject.ItemContainerGenerator.ItemsChanged += ItemContainerGenerator_ItemsChanged;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.ItemContainerGenerator.ItemsChanged -= ItemContainerGenerator_ItemsChanged;
            
            base.OnDetaching();
        }

        private void AssociatedObject_Loaded(object sender, RoutedEventArgs e)
        {
            AssociatedObject.Unloaded += AssociatedObject_Unloaded;
            AssociatedObject.Loaded -= AssociatedObject_Loaded;

            if (AssociatedObject is ComboBox)
            {
                var comboBox = (ComboBox) AssociatedObject;
                comboBox.IsDropDownOpen = true;
                comboBox.IsDropDownOpen = false;
            }
        }

        private void AssociatedObject_Unloaded(object sender, RoutedEventArgs e)
        {
            AssociatedObject.Unloaded -= AssociatedObject_Unloaded;
            Detach();
        }

        private async void SetBindingAsync(ItemContainerGenerator generator, object item, Binding binding)
        {
            while (true)
            {
                if (generator.Status != GeneratorStatus.ContainersGenerated &&
                    generator.Status != GeneratorStatus.NotStarted)
                {
                    return;
                }

                var container = generator.ContainerFromItem(item);
                if (container != null)
                {
                    BindingOperations.SetBinding(container, Selector.IsSelectedProperty, binding);
                    return;
                }

                await Task.Delay(100);
            }
        }

        private void ItemContainerGenerator_ItemsChanged(object sender, ItemsChangedEventArgs e)
        {
            if (SelectedItemBinding == null)
            {
                return;
            }

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    var generator = ((ItemContainerGenerator) sender);
                    for (int i = 0; i < e.ItemCount; ++i)
                    {
                        var index = e.Position.Index + e.Position.Offset + i;
                        var item = generator.Items[index];
                        SetBindingAsync(generator, item, SelectedItemBinding);
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    break;
                case NotifyCollectionChangedAction.Replace:
                    break;
                case NotifyCollectionChangedAction.Move:
                    break;
                case NotifyCollectionChangedAction.Reset:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}