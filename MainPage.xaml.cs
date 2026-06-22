using Microsoft.Maui.Controls;
using proyecto3.ViewModels;
using System;

namespace proyecto3
{
    public partial class MainPage : ContentPage
    {
        private readonly MainViewModel _viewModel;

        public MainPage(MainViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            BindingContext = _viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _viewModel.CargarTransaccionesAsync();
        }
    }
}
