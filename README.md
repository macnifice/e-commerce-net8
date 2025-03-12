# Proyecto E-commerce

Este repositorio contiene una solución completa de e-commerce con una API RESTful en .NET 8 y una interfaz de usuario en Angular.

## Estructura del Proyecto

El repositorio está organizado como un monorepo con dos componentes principales:

- **api**: API RESTful desarrollada con ASP.NET Core 8
- **ui**: Aplicación frontend desarrollada con Angular

## Requisitos Previos

Para ejecutar el proyecto localmente necesitarás:

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Node.js](https://nodejs.org/) (v18 o superior)
- [Angular CLI](https://angular.io/cli) (`npm install -g @angular/cli`)
- [SQL Server](https://www.microsoft.com/es-es/sql-server/sql-server-downloads) (o SQL Server Express)
- [Visual Studio 2022](https://visualstudio.microsoft.com/es/downloads/) (recomendado) o [Visual Studio Code](https://code.visualstudio.com/)

## Configuración y Ejecución Local

### 1. Configuración de la API (.NET)

```bash
# Navegar al directorio de la API
cd api

# Abrir la solución en Visual Studio
start api.sln
```

Una vez abierto en Visual Studio:

1. Modificar la cadena de conexión a la base de datos:
   - Abre el archivo `api/appsettings.json`
   - Actualiza la cadena de conexión en `ConnectionStrings:DefaultConnection` con tu configuración de SQL Server

2. Ejecutar las migraciones para crear la base de datos:
   - Abre la Consola del Administrador de Paquetes (Tools > NuGet Package Manager > Package Manager Console)
   - Asegúrate de que el proyecto predeterminado es "api"
   - Ejecuta el comando: `Update-Database`

3. Iniciar la API:
   - Presiona F5 o usa el botón de inicio para ejecutar la API

La API estará disponible en:
- **API**: https://localhost:7211
- **Swagger (Documentación API)**: https://localhost:7211/swagger

### 2. Configuración de la UI (Angular)

```bash
# Navegar al directorio de la UI
cd ui

# Instalar dependencias
npm install

# Verificar la configuración de URL de API
# Edita ui/src/environments/environment.ts para asegurarte de que apiUrl apunte a https://localhost:7211

# Iniciar el servidor de desarrollo
npm start
```

La interfaz de usuario estará disponible en:
- **Frontend (UI)**: http://localhost:4200

## Acceso a la Aplicación

Una vez que ambos servicios estén en ejecución, puedes acceder a la aplicación a través de:
- **Frontend**: http://localhost:4200
- **API**: https://localhost:7211
- **Swagger**: https://localhost:7211/swagger

## Solución de problemas comunes

### Error de conexión a la base de datos

Si tienes problemas para conectar a la base de datos:

1. Verifica que SQL Server esté funcionando ejecutando SQL Server Management Studio
2. Asegúrate de que la cadena de conexión sea correcta en `appsettings.json`

Ejemplo de cadena de conexión:
```
"ConnectionStrings": {
  "DefaultConnection": "Data Source=localhost\\SQLEXPRESS;Initial Catalog=EcommerceApi;Integrated Security=True;TrustServerCertificate=True;"
}
```

3. Si necesitas crear la base de datos manualmente:
   - Abre SQL Server Management Studio
   - Crea una nueva base de datos llamada `EcommerceApi`
   - Ejecuta las migraciones desde Visual Studio con el comando `Update-Database`

4. Verifica que el usuario tiene permisos para acceder a la base de datos

### Error en CORS

Si encuentras errores de CORS al conectar el frontend con el backend:

1. Verifica la configuración de CORS en `Program.cs`
2. Asegúrate de que las URLs que utilizas para acceder a la API coinciden con las configuradas en la política CORS
3. En desarrollo local, verifica que estás usando los puertos correctos (API: 7211, UI: 4200)

### Problemas con Angular

Si tienes problemas con la interfaz de usuario:

1. Verifica que la URL de la API está configurada correctamente en `environment.ts`
2. Limpia la caché del navegador o usa el modo incógnito para pruebas
3. Si hay problemas de compilación, prueba con `npm clean-install`

## Estructura de la API

La API sigue una arquitectura en capas:

- **Controllers**: Controladores REST que manejan las solicitudes HTTP
- **Business**: Lógica de negocio y servicios
- **Data**: Acceso a datos con Entity Framework Core
- **Models**: Modelos de datos y DTOs
- **Filters**: Filtros de ASP.NET Core para manejo transversal

### Características principales

- Autenticación JWT
- Documentación Swagger
- Entity Framework Core con SQL Server
- Filtrado y paginación de resultados
- CORS configurado para desarrollo

## Estructura de la UI

La UI está desarrollada con Angular utilizando:

- Angular Material para componentes de UI
- Arquitectura modular con lazy loading
- Servicios para comunicación con la API
- Guards para protección de rutas
- Interceptores para manejo de tokens JWT

## Entornos

El proyecto está configurado para funcionar en los siguientes entornos:

- **Desarrollo**: Configuración local para desarrollo
- **Producción**: Configuración optimizada para producción

## Tecnologías Utilizadas

### Backend
- ASP.NET Core 8
- Entity Framework Core
- JWT para autenticación
- SQL Server
- Swagger para documentación

### Frontend
- Angular 18
- Angular Material
- RxJS
- NgRx (si se utiliza)

## Licencia

[MIT](LICENSE) 