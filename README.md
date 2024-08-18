# ApiTurismo

ApiTurismo es un proyecto diseñado para proporcionar servicios relacionados con la gestión de turismo a través de una arquitectura basada en microservicios. Este proyecto está construido utilizando ASP.NET Core y sigue una arquitectura de capas, lo que facilita la separación de preocupaciones y la escalabilidad del sistema.

## Arquitectura del Proyecto

El proyecto se organiza en varias capas y bibliotecas de clases que se comunican entre sí para gestionar las operaciones de la API de turismo. A continuación se describen los componentes clave:

### 1. TurismoApp.Common (Biblioteca de Clases)
- **Propósito:** Contiene clases comunes que son reutilizadas por otras capas del proyecto.
- **Componentes:**
  - `Dto` (Data Transfer Objects)
  - `Helpers`
  - `Data Annotations Custom` (Anotaciones personalizadas para validaciones)

### 2. TurismoApp.Application (Biblioteca de Clases)
- **Propósito:** Implementa la lógica de negocio y las interfaces necesarias para las operaciones de la aplicación.
- **Componentes:**
  - `Lógica de negocio` (Implementación e interfaces)

### 3. TurismoApp.Infrastructure (Biblioteca de Clases)
- **Propósito:** Gestiona la comunicación con la base de datos y otras infraestructuras necesarias.
- **Componentes:**
  - `DbContext` (Contexto de la base de datos)
  - `Repositories` (Implementación e interfaces para acceso a datos)
  - `Entities`
  - `Mapping` (Mapeo de entidades a la base de datos)

### 4. TurismoApp.API (API Web)
- **Propósito:** Expone los servicios RESTful a través de controladores que manejan las solicitudes HTTP.
- **Componentes:**
  - `Controllers`
  - `Settings`

### 5. TurismoApp.Services (Biblioteca de Clases)
- **Propósito:** Gestiona servicios externos, como el envío de correos electrónicos.
- **Componentes:**
  - `EmailService` (Implementación e interfaz para envío de correos a través de un servidor SMTP)

### 6. Base de Datos y Servidor SMTP
- **Base de Datos:** Gestiona el almacenamiento persistente de la información.
- **Servidor SMTP:** Encargado del envío de correos electrónicos desde el servicio de la aplicación.

## Requisitos Previos

Antes de ejecutar el proyecto, asegúrate de tener instalados los siguientes requisitos:

- **.NET 6.0 SDK o superior**
- **SQL Server** o cualquier otro servidor compatible para la base de datos
- **Servidor SMTP** para el servicio de correo electrónico

## Instalación

1. **Clonar el repositorio:**

    ```bash
    git clone https://github.com/AldairDominguez/ApiTurismo.git
    cd ApiTurismo
    ```

2. **Restaurar dependencias:**

    Navega a la carpeta raíz del proyecto y ejecuta:

    ```bash
    dotnet restore
    ```

3. **Configurar la cadena de conexión de la base de datos:**

    Abre el archivo `appsettings.json` en la carpeta `TurismoApp.API` y actualiza la cadena de conexión de la base de datos según tu entorno. Además, configura las credenciales del servidor SMTP en el archivo `EmailService.cs`, ubicado en la carpeta `TurismoApp.Services`, para habilitar el envío de correos electrónicos.

4. **Aplicar migraciones y actualizar la base de datos:**

    Ejecuta el siguiente comando para aplicar las migraciones a la base de datos:

    ```bash
    dotnet ef database update --project TurismoApp.Infrastructure
    ```

## Ejecución

1. **Compilar el proyecto:**

    ```bash
    dotnet build
    ```

2. **Ejecutar la API:**

    ```bash
    dotnet run --project TurismoApp.API
    ```

    La API estará disponible en `https://localhost:5001` o `http://localhost:5000`.

## Probar la API

Puedes probar la API utilizando la interfaz de Swagger proporcionada en el despliegue en la web. Swagger permite interactuar fácilmente con los endpoints y ver las respuestas de la API.

### Acceso a la Documentación de la API con Swagger

1. **URL de Swagger:**

    Puedes acceder a la documentación y probar los endpoints directamente desde [http://apiturismo.somee.com/swagger/index.html](http://apiturismo.somee.com/swagger/index.html).

## Contribuciones

Si deseas contribuir a este proyecto, puedes hacer un fork del repositorio y enviar un pull request con tus mejoras o correcciones. Todas las contribuciones son bienvenidas.

## Licencia

Este proyecto está bajo la [MIT License](https://opensource.org/licenses/MIT).
