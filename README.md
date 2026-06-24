# proyecto3 - Control de Transacciones Financieras

Este proyecto es una aplicación móvil y de escritorio multiplataforma construida con **.NET MAUI (Multi-platform App UI)** utilizando **.NET 10.0**. La aplicación implementa el patrón de arquitectura **MVVM (Model-View-ViewModel)** y utiliza una base de datos local **SQLite** para persistir transacciones financieras (ingresos y egresos).

---

## Cómo levantar el proyecto al clonar

Sigue estos pasos para configurar y ejecutar la aplicación en tu entorno local:

### 1. Requisitos Previos

Asegúrate de tener instalado lo siguiente en tu máquina:

- **SDK de .NET 10.0** (o superior).
- **Carga de trabajo de .NET MAUI**. Puedes instalarla desde la terminal ejecutando:
  ```bash
  dotnet workload install maui
  ```
- **Emuladores/Dispositivos**:
  - Para Android: Android SDK y un emulador Android configurado o dispositivo físico con depuración USB.
  - Para iOS/macOS: Requiere una máquina macOS con Xcode instalado.

### 2. Clonar el Repositorio

Clona este repositorio en tu máquina local:
```bash
git clone <URL_DEL_REPOSITORIO>
cd proyecto3
```

### 3. Restaurar Dependencias

Restaura los paquetes NuGet necesarios para el proyecto:
```bash
dotnet restore
```

### 4. Compilar y Ejecutar

Puedes ejecutar la aplicación desde tu IDE preferido seleccionando el dispositivo objetivo (Windows, Android, iOS o macOS) y presionando `F5`, o utilizando la interfaz de comandos (CLI) de .NET:

* **Windows Machine:**
  ```bash
  dotnet build -t:Run -f net10.0-windows10.0.19041.0
  ```
* **Android (Emulador por defecto):**
  ```bash
  dotnet build -t:Run -f net10.0-android
  ```
* **iOS (Solo macOS):**
  ```bash
  dotnet build -t:Run -f net10.0-ios
  ```
* **macOS (Mac Catalyst - Solo macOS):**
  ```bash
  dotnet build -t:Run -f net10.0-maccatalyst
  ```

---

## Estructura General del Proyecto

La estructura del proyecto sigue una arquitectura **MVVM** limpia y modular, separando las responsabilidades de datos, lógica de negocio y presentación:

```
proyecto3/
├── Models/              # Modelos de datos del negocio
│   └── Transaccion.cs   # Representación de la transacción (Glosa, Cantidad, Fecha, etc.)
├── Views/               # Vistas de la interfaz de usuario en XAML
│   ├── MainPage.xaml            # Vista principal (Historial de transacciones y balance)
│   └── NuevaTransaccionPage.xaml # Vista para registrar un nuevo ingreso o egreso
├── ViewModels/          # Lógica de presentación vinculada a las Vistas
│   ├── MainViewModel.cs          # Vinculado a MainPage
│   └── NuevaTransaccionViewModel.cs # Vinculado a NuevaTransaccionPage
├── Services/            # Servicios auxiliares de la aplicación
│   └── DatabaseService.cs # Servicio de acceso a datos para SQLite (CRUD)
├── Resources/           # Recursos estáticos de la aplicación
│   ├── AppIcon/         # Iconos de la aplicación (SVG)
│   ├── Fonts/           # Fuentes de texto (OpenSans)
│   ├── Images/          # Imágenes de la app
│   └── Splash/          # Pantalla de carga (Splash screen)
├── Platforms/           # Configuraciones y código nativo por plataforma
│   ├── Android/
│   ├── iOS/
│   ├── MacCatalyst/
│   └── Windows/
├── MauiProgram.cs       # Configuración inicial, inyección de dependencias y fuentes
├── App.xaml / App.xaml.cs # Estructura global de la app
├── AppShell.xaml        # Enrutamiento de navegación y jerarquía de páginas
└── proyecto3.csproj     # Archivo de configuración del proyecto de .NET y dependencias NuGet
```

### Componentes Clave:
1. **`DatabaseService.cs`**: Maneja la conexión asíncrona a la base de datos SQLite (`transacciones.db3`), la cual se crea automáticamente en el almacenamiento local del dispositivo (`FileSystem.AppDataDirectory`).
2. **`Transaccion.cs`**: Clase modelo decorada con atributos de `SQLite-net` para definir la tabla, llave primaria autoincremental, propiedades y helpers de presentación formateados para la UI.
3. **`MauiProgram.cs`**: Registra el contenedor de dependencias (DI):
   - Servicios como singleton (`DatabaseService`).
   - ViewModels y Vistas como transient (`MainPage`, `MainViewModel`, `NuevaTransaccionPage`, `NuevaTransaccionViewModel`).
4. **`AppShell.xaml.cs`**: Registra las rutas de navegación dinámicas para poder navegar de forma sencilla con `Shell.Current.GoToAsync()`.

---

## Tecnologías y Paquetes NuGet Utilizados

- **.NET MAUI** - Framework para la creación de apps nativas multiplataforma.
- **CommunityToolkit.Mvvm** - Biblioteca oficial para simplificar MVVM usando Source Generators (atributos como `[ObservableProperty]` y `[RelayCommand]`).
- **sqlite-net-pcl** - ORM ligero de SQLite para almacenar las transacciones de manera local.
- **Microsoft.Extensions.Logging.Debug** - Herramienta para depuración y logging en desarrollo.
