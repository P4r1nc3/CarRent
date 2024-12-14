### CarRent

---

# CarRent

CarRent to aplikacja WPF do zarz�dzania wypo�yczalni� samochod�w, zbudowana w oparciu o .NET oraz Entity Framework Core. Aplikacja obs�uguje baz� danych MySQL.

---

## Spis tre�ci

- [Wymagania](#wymagania)
- [Instalacja](#instalacja)
- [Konfiguracja](#konfiguracja)
- [Tworzenie bazy danych](#tworzenie-bazy-danych)
- [Zale�no�ci](#zale�no�ci)
- [Plany na przysz�o��](#plany-na-przysz�o��)

---

## Wymagania

Do uruchomienia projektu wymagane s�:

- [SDK .NET 6.0+](https://dotnet.microsoft.com/download)
- MySQL Server lub kompatybilna baza danych


---

## Instalacja

1. Sklonuj repozytorium i przejd� do katalogu projektu:

   ```bash
   git clone https://github.com/P4r1nc3/CarRent
   cd CarRent
   ```

2. Przywr�� pakiety NuGet:

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

0. Zainstaluj narz�dzie Entity Framework Core:

	```bash
	dotnet tool install --global dotnet-ef
	```

1. Utw�rz pierwsz� migracj�:

   ```bash
   dotnet ef migrations add InitialCreate
   ```

2. Zaktualizuj baz� danych:

   ```bash
   dotnet ef database update
   ```

---

## Zale�no�ci

Projekt korzysta z nast�puj�cych pakiet�w:

- **Microsoft.EntityFrameworkCore** (v8.0.2)  
- **Microsoft.EntityFrameworkCore.Design** (v8.0.2)  
- **Microsoft.EntityFrameworkCore.SqlServer** (v8.0.2)  
- **Microsoft.EntityFrameworkCore.Tools** (v8.0.2)  
- **MySql.EntityFrameworkCore** (v8.0.0)  
- **Pomelo.EntityFrameworkCore.MySql** (v8.0.0)  
