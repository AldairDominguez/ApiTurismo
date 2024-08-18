# ApiTurismo

ApiTurismo es un proyecto diseñado para proporcionar servicios relacionados con la gestión de turismo a través de una arquitectura basada en microservicios. Este proyecto está construido utilizando ASP.NET Core y sigue una arquitectura de capas, lo que facilita la separación de preocupaciones y la escalabilidad del sistema.

## 🏛️ Arquitectura del Proyecto
![Diagrama de Arquitectura](https://github.com/AldairDominguez/ApiTurismo/blob/main/TurismoApp.Api/Properties/image.png)
El proyecto se organiza en varias capas y bibliotecas de clases que se comunican entre sí para gestionar las operaciones de la API de turismo. A continuación se describen los componentes clave:

## ⚙️ Requisitos Previos

Antes de ejecutar el proyecto, asegúrate de tener instalados los siguientes requisitos:

- **.NET 8.0 SDK o superior**
- **SQL Server** o cualquier otro servidor compatible para la base de datos
- **Servidor SMTP** para el servicio de correo electrónico

## 🛠️ Instalación

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

3. **Configurar la cadena de conexión de la base de datos y las credenciales del servidor SMTP:**

   Abre el archivo `appsettings.json` en la carpeta `TurismoApp.API` y actualiza la cadena de conexión de la base de datos según tu entorno. Además, configura las credenciales del servidor SMTP en el archivo       `EmailService.cs`, ubicado en la carpeta `TurismoApp.Services`, para habilitar el envío de correos electrónicos.

4. **Aplicar migraciones y actualizar la base de datos:**

    Ejecuta el siguiente comando para aplicar las migraciones a la base de datos:

    ```bash
    dotnet ef database update --project TurismoApp.Infrastructure
    ```
    Alternativamente, si prefieres ejecutar la migración manualmente en tu PC, puedes utilizar los siguientes comandos en la consola del Package Manager (PMC) o en la terminal de comandos:
   
    Crear la migración inicial
   
    ```bash
    Add-Migration InitialCreate -Project TurismoApp.Infrastructure
    ```
   
    Aplicar la migración a la base de datos
   
    ```bash
    Update-Database -Project TurismoApp.Infrastructure
    ```

## 🚀 Ejecución

1. **Compilar el proyecto:**

    ```bash
    dotnet build
    ```

2. **Ejecutar la API:**

    ```bash
    dotnet run --project TurismoApp.API
    ```

    La API estará disponible en `https://localhost:5001` o `http://localhost:5000`.

## ⚡ Probar la API

Puedes probar la API utilizando la interfaz de Swagger proporcionada en el despliegue en la web. Swagger permite interactuar fácilmente con los endpoints y ver las respuestas de la API.

### Acceso a la Documentación de la API con Swagger

1. **URL de Swagger:**

    Puedes acceder a la documentación y probar los endpoints directamente desde [http://apiturismo.somee.com/swagger/index.html](http://apiturismo.somee.com/swagger/index.html).

## 🤝 Contribuciones

Si deseas contribuir a este proyecto, puedes hacer un fork del repositorio y enviar un pull request con tus mejoras o correcciones. Todas las contribuciones son bienvenidas.

## 📄 Licencia

Este proyecto está bajo la [MIT License](https://opensource.org/licenses/MIT).
