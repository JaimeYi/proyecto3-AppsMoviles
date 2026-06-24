using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using proyecto3.Models;
using proyecto3.Services;
using proyecto3.Views;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace proyecto3.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly DatabaseService _databaseService;

        public ObservableCollection<Transaccion> Transacciones { get; } = new();

        [ObservableProperty]
        public partial decimal BalanceTotal { get; set; }

        [ObservableProperty]
        public partial decimal TotalIngresos { get; set; }

        [ObservableProperty]
        public partial decimal TotalGastos { get; set; }

        [ObservableProperty]
        public partial bool IsListEmpty { get; set; } = true;

        [ObservableProperty]
        public partial bool IsListNotEmpty { get; set; } = false;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(UsuarioNombreCompleto))]
        public partial string UsuarioNombre { get; set; } = string.Empty;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(UsuarioNombreCompleto))]
        public partial string UsuarioApellido { get; set; } = string.Empty;

        public string UsuarioNombreCompleto => string.IsNullOrWhiteSpace(UsuarioApellido)
            ? (string.IsNullOrWhiteSpace(UsuarioNombre) ? "Usuario" : UsuarioNombre)
            : $"{UsuarioNombre} {UsuarioApellido}".Trim();

        [ObservableProperty]
        public partial bool IsEditingName { get; set; } = false;

        [ObservableProperty]
        public partial string EditNombre { get; set; } = string.Empty;

        [ObservableProperty]
        public partial string EditApellido { get; set; } = string.Empty;

        public MainViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;
            UsuarioNombre = Microsoft.Maui.Storage.Preferences.Default.Get("UsuarioNombre", "Juanito");
            UsuarioApellido = Microsoft.Maui.Storage.Preferences.Default.Get("UsuarioApellido", "Perez");
        }

        [RelayCommand]
        public async Task CargarTransaccionesAsync()
        {
            var lista = await _databaseService.GetTransaccionesAsync();
            
            Transacciones.Clear();
            foreach (var transaccion in lista)
            {
                Transacciones.Add(transaccion);
            }

            CalcularIngresos();
            CalcularGastos();
            CalcularBalance();

            IsListEmpty = !Transacciones.Any();
            IsListNotEmpty = Transacciones.Any();
        }

        [RelayCommand]
        public async Task EliminarTransaccionAsync(Transaccion transaccion)
        {
            if (transaccion == null) return;
            
            bool confirm = await Shell.Current.DisplayAlertAsync("Confirmar", "¿Desea eliminar esta transacción?", "Sí", "No");
            if (confirm)
            {
                await _databaseService.DeleteTransaccionAsync(transaccion);
                await CargarTransaccionesAsync();
            }
        }

        [RelayCommand]
        public async Task NavegarNuevaTransaccionAsync()
        {
            await Shell.Current.GoToAsync(nameof(NuevaTransaccionPage));
        }

        [RelayCommand]
        public async Task LimpiarHistorialAsync()
        {
            if (!Transacciones.Any())
            {
                await Shell.Current.DisplayAlertAsync("Aviso", "No hay transacciones en el historial.", "OK");
                return;
            }

            bool confirm = await Shell.Current.DisplayAlertAsync(
                "Confirmar", 
                "¿Desea limpiar todo el historial de transacciones? Esta acción no se puede deshacer.", 
                "Sí, limpiar", 
                "Cancelar");

            if (confirm)
            {
                await _databaseService.DeleteAllTransaccionesAsync();
                await CargarTransaccionesAsync();
            }
        }

        [RelayCommand]
        public void IniciarEdicionNombre()
        {
            EditNombre = UsuarioNombre;
            EditApellido = UsuarioApellido;
            IsEditingName = true;
        }

        [RelayCommand]
        public async Task GuardarNombreAsync()
        {
            if (string.IsNullOrWhiteSpace(EditNombre))
            {
                await Shell.Current.DisplayAlertAsync("Error", "El nombre es obligatorio.", "OK");
                return;
            }

            UsuarioNombre = EditNombre.Trim();
            UsuarioApellido = (EditApellido ?? "").Trim();

            Microsoft.Maui.Storage.Preferences.Default.Set("UsuarioNombre", UsuarioNombre);
            Microsoft.Maui.Storage.Preferences.Default.Set("UsuarioApellido", UsuarioApellido);

            IsEditingName = false;
        }

        [RelayCommand]
        public void CancelarEdicionNombre()
        {
            IsEditingName = false;
        }

        public void CalcularBalance()
        {
            BalanceTotal = TotalIngresos - TotalGastos;
        }

        public void CalcularIngresos()
        {
            decimal total = 0;
            foreach (var t in Transacciones)
            {
                if (t.EsIngreso)
                {
                    total += t.Cantidad;
                }
            }
            TotalIngresos = total;
        }

        public void CalcularGastos()
        {
            decimal total = 0;
            foreach (var t in Transacciones)
            {
                if (!t.EsIngreso)
                {
                    total += t.Cantidad;
                }
            }
            TotalGastos = total;
        }
    }
}
