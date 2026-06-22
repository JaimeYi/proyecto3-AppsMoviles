using Microsoft.Maui.Controls;
using proyecto3.ViewModels;

namespace proyecto3.Views
{
    public partial class NuevaTransaccionPage : ContentPage
    {
        public NuevaTransaccionPage(NuevaTransaccionViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}
