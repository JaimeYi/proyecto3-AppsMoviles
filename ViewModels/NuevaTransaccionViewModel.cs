using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using proyecto3.Models;
using proyecto3.Services;

namespace proyecto3.ViewModels
{
    public partial class NuevaTransaccionViewModel : ObservableObject
    {
        private readonly DatabaseService _databaseService;

        [ObservableProperty]
        public partial string Glosa { get; set; } = string.Empty;

        [ObservableProperty]
        public partial decimal Cantidad { get; set; }

        [ObservableProperty]
        public partial DateTime Fecha { get; set; } = DateTime.Today;

        [ObservableProperty]
        public partial bool EsIngreso { get; set; }

        public NuevaTransaccionViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        [RelayCommand]
        private async Task GuardarAsync()
        {
            if (string.IsNullOrWhiteSpace(Glosa))
            {
                await Shell.Current.DisplayAlertAsync("Validación", "Por favor ingresa una glosa.", "OK");
                return;
            }

            if (Cantidad <= 0)
            {
                await Shell.Current.DisplayAlertAsync("Validación", "Por favor ingresa una cantidad válida mayor a 0.", "OK");
                return;
            }

            var nuevaTransaccion = new Transaccion
            {
                Glosa = Glosa.Trim(),
                Cantidad = Cantidad,
                Fecha = Fecha,
                EsIngreso = EsIngreso
            };

            await _databaseService.SaveTransaccionAsync(nuevaTransaccion);
            await Shell.Current.GoToAsync("..");
        }

        [RelayCommand]
        private async Task CancelarAsync()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
