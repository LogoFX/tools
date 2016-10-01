using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.Xaml.Interactivity;

namespace $safeprojectname$.Interactivity
{
    public class UpdateSourceOnChangeBehavior : Behavior<DependencyObject>
    {
        protected override void OnAttached()
        {
            base.OnAttached();

            var txt = AssociatedObject as TextBox;
            if (txt != null)
            {
                txt.TextChanged += OnTextChanged;
            }
        }

        protected override void OnDetaching()
        {
            var txt = AssociatedObject as TextBox;
            if (txt != null)
            {
                txt.TextChanged -= OnTextChanged;
                return;
            }
            base.OnDetaching();
        }

        static void OnTextChanged(object sender,
          TextChangedEventArgs e)
        {
            var txt = sender as TextBox;
            if (txt == null)
                return;
            var be = txt.GetBindingExpression(TextBox.TextProperty);
            if (be != null)
            {
                be.UpdateSource();
            }
        }

    }
}
