### CarRent

---

# CarRent

CarRent to aplikacja WPF do zarz¹dzania wypo¿yczalni¹ samochodów, zbudowana w oparciu o .NET oraz Entity Framework Core. Aplikacja obs³uguje bazê danych MySQL.

---

## Spis treœci

- [Wymagania](#wymagania)
- [Instalacja](#instalacja)
- [Konfiguracja](#konfiguracja)
- [Tworzenie bazy danych](#tworzenie-bazy-danych)
- [Zale¿noœci](#zale¿noœci)
- [Plany na przysz³oœæ](#plany-na-przysz³oœæ)

---

## Wymagania

Do uruchomienia projektu wymagane s¹:

- [SDK .NET 6.0+](https://dotnet.microsoft.com/download)
- MySQL Server lub kompatybilna baza danych


---

## Instalacja

1. Sklonuj repozytorium i przejdŸ do katalogu projektu:

   ```bash
   git clone https://github.com/P4r1nc3/CarRent
   cd CarRent
   ```

2. Przywróæ pakiety NuGet:

   ```bash
   dotnet restore
   ```

---

## Konfiguracja

Edytuj plik `AppDbContext.cs` i dostosuj connection string:

```csharp
"server=localhost;database=car-rent;user=root;password=admin12345"
```

---

## Tworzenie bazy danych

0. Zainstaluj narzêdzie Entity Framework Core:

	```bash
	dotnet tool install --global dotnet-ef
	```

1. Utwórz pierwsz¹ migracjê:

   ```bash
   dotnet ef migrations add InitialCreate
   ```

2. Zaktualizuj bazê danych:

   ```bash
   dotnet ef database update
   ```

---

## Zale¿noœci

Projekt korzysta z nastêpuj¹cych pakietów:

- **Microsoft.EntityFrameworkCore** (v8.0.2)  
- **Microsoft.EntityFrameworkCore.Design** (v8.0.2)  
- **Microsoft.EntityFrameworkCore.SqlServer** (v8.0.2)  
- **Microsoft.EntityFrameworkCore.Tools** (v8.0.2)  
- **MySql.EntityFrameworkCore** (v8.0.0)  
- **Pomelo.EntityFrameworkCore.MySql** (v8.0.0)  
