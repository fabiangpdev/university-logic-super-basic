## WinForms Auth App (C#) + PostgreSQL en Docker

### Requisitos
- Windows 10/11
- Docker Desktop
- .NET SDK 8

### 1) Levantar base de datos
```powershell
cd test
docker compose up -d
```

DB disponible en `localhost:5432`
- Usuario: `appuser`
- Password: `apppassword`
- Base: `authdb`

### 2) Ejecutar la app de escritorio
```powershell
cd test/
./dist/WinFormsAuthApp.exe
```

### Funcionalidad
- Registro: nombre, correo, contraseña
- Hash de contraseña con SHA256
- Login validando contra hash guardado

### Estructura del proyecto
```
test/
  docker-compose.yml
  db/
    init.sql
  src/
    WinFormsAuthApp/
      WinFormsAuthApp.csproj
      Program.cs
      appsettings.json
      Config/
        Config.cs
      Security/
        PasswordHasher.cs
      Data/
        UserRepository.cs
      MainForm.cs
      RegisterForm.cs
      LoginForm.cs
```

### Notas
- La UI se ejecuta en Windows (host). Docker se usa para la base de datos.
- Si `docker compose up -d` falla, asegúrate de que Docker Desktop esté iniciado.


