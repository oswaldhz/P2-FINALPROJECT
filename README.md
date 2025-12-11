# P2 Final Project

Este repositorio contiene una solución básica con backend en ASP.NET Core y frontend en React/Chakra UI para administrar reservas de equipos y software.

## Backend (.NET)

Estructura de proyectos:

- `src/Domain`: Entidades de dominio (`Equipo`, `Usuario`, `Reserva`, `Software`).
- `src/Infrastructure`: `ApplicationDbContext`, configuraciones de EF Core, migración inicial y siembra de datos.
- `src/Api`: API Web con controladores para autenticación, reservas y equipos. Incluye JWT para proteger los endpoints.

### Ejecutar la API

1. Instala .NET 8 SDK y las herramientas de EF Core.
2. Restaura paquetes y aplica la migración inicial:
   ```bash
   dotnet restore
   dotnet ef database update --project src/Infrastructure --startup-project src/Api
   ```
3. Levanta la API:
   ```bash
   dotnet run --project src/Api
   ```
4. Swagger estará disponible en `/swagger` en entorno de desarrollo.

### Configuración del usuario administrador inicial

El sembrado crea un usuario con rol `Admin` utilizando los valores de configuración `AdminUser:Email`, `AdminUser:Name` y `AdminUser:Password`.

- Para ambientes productivos, define estas claves mediante variables de entorno (ej. `AdminUser__Email`, `AdminUser__Name`, `AdminUser__Password`) o en un archivo de configuración seguro antes de ejecutar las migraciones o el servicio.
- Si las claves no están presentes y el entorno es **Desarrollo**, se usarán valores por defecto (`admin@example.com` / `Administrador` / `Admin123$`).
- Si las claves faltan en ambientes no desarrollos, el sembrado fallará para evitar crear credenciales inseguras.

## Frontend (React + Vite + Chakra UI)

Ubicación: `web/`

### Ejecutar el frontend

```bash
cd web
npm install
npm run dev
```

El frontend asume que la API corre en `http://localhost:5178` y exige iniciar sesión para ver la página principal y gestionar reservas.

## Notas de seguridad

- Cambia la clave `Jwt:Key` en `src/Api/appsettings.json` antes de usar en producción.
- Ajusta la cadena de conexión según el proveedor de base de datos que utilices.
