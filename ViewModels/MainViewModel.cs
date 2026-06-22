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

        public MainViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;
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

        // Lógica de negocio requerida por la rúbrica de evaluación:
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
