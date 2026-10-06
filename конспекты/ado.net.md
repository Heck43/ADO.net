# Конспект: ADO.NET и работа с PostgreSQL (Npgsql) в C#

---

## 1. Архитектура доступа к данным: ADO.NET, Драйверы и ORM

### Схема взаимодействия

**Работа напрямую через ADO.NET и провайдер БД:**
```text
Backend (C#)  ───►  ADO.NET (абстракции)  ───►  Драйвер БД (Npgsql)  ───►  СУБД (PostgreSQL)
```

**Работа через ORM (Dapper / EF Core):**
```text
Backend (C#)  ───►  ORM (EF Core / Dapper)  ───►  ADO.NET + Npgsql   ───►  СУБД (PostgreSQL)
```

### Основные понятия

* **ADO.NET** — это набор базовых API и библиотек в платформе .NET для работы с источниками данных (реляционными БД, XML и др.).
  * ADO.NET **не является** отдельной базой данных и **не является** ORM.
  * Это **инфраструктура низкоуровневого доступа к данным**. Она предоставляет приложению стандартизированную модель работы (общие базовые классы и интерфейсы), а конкретный провайдер переводит вызовы этой модели в сетевой протокол и диалект конкретной СУБД.
* **Data Provider (Провайдер данных / Драйвер БД)** — библиотека, которая знает, как общаться с конкретной СУБД по её бинарному или текстовому протоколу.
  * Для **PostgreSQL** в .NET стандартом де-факто является библиотека **`Npgsql`**.
* **Dapper (Micro-ORM)** — легковесная микро-ORM поверх ADO.NET.
  * Он не пытается полностью скрыть SQL: разработчик по-прежнему пишет SQL-запросы вручную, но Dapper берет на себя рутинную работу — параметризацию, чтение строк из `DbDataReader` и автоматическое сопоставление (маппинг) колонок результата со свойствами C#-объектов.
* **EF Core (Entity Framework Core)** — полноценная ORM (Object-Relational Mapper).
  * Вы описываете модели сущностей (классы C#), используете `DbContext` и пишете запросы на **LINQ**, а EF Core сам **переводит запросы в SQL**, материализует результаты в объекты и умеет отслеживать изменения сущностей (*Change Tracking*).
  * **`DbContext`** — хранит состояние отслеживаемых объектов в памяти, а метод **`SaveChanges()` / `SaveChangesAsync()`** формирует `INSERT / UPDATE / DELETE` и применяет изменения к БД.

### Особенности чистого ADO.NET
В чистом **ADO.NET** вы самостоятельно управляете:
1. Текстом SQL-запроса.
2. Передачей параметров.
3. Способом чтения результатов.

Если запрос вернул `id`, `name` и `email`, именно ваш C#-код решает, в какие свойства объектов попадут эти значения (ручная материализация). ADO.NET **не отслеживает автоматически**, изменили ли вы свойства объекта в памяти после выполнения `SELECT`.

---

## 2. Базовые абстракции ADO.NET (`System.Data.Common`)

На уровне ADO.NET нас интересует единый принцип работы через абстрактные классы, от которых наследуются классы конкретного провайдера (например, `Npgsql`):

| Абстракция ADO.NET | Реализация в Npgsql | Назначение |
| :--- | :--- | :--- |
| **`DbConnection`** | `NpgsqlConnection` | Представляет физическое или логическое соединение с источником данных. Содержит строку подключения (`ConnectionString`), состояние (`State`) и операции открытия/закрытия (`Open`, `OpenAsync`, `Close`). |
| **`DbCommand`** | `NpgsqlCommand` | Абстрактное представление команды (SQL-запроса или хранимой процедуры) для источника данных. Содержит текст команды (`CommandText`), ссылку на соединение и коллекцию параметров (`Parameters`). |
| **`DbParameter`** | `NpgsqlParameter` | Представление значения, которое передается в команду **отдельно** от текста SQL. |
| **`DbDataReader`** | `NpgsqlDataReader` | Потоковый курсор для чтения набора результатов: читает данные последовательно, **вперед и построчно** (*forward-only, read-only*). Удобен для больших выборок, так как не требует загружать всю таблицу в оперативную память (`DataTable`). |
| **`DbTransaction`** | `NpgsqlTransaction` | Объединяет несколько операций в БД в одну **атомарную единицу работы** (принципы ACID: либо выполняются все команды через `Commit`, либо все откатываются через `Rollback`). |

### Зачем нужен `DbParameter`?
Параметры в SQL-командах необходимы по двум главным причинам:
1. **Безопасность:** отделяют данные от исполняемого кода SQL и защищают от **SQL-инъекций** (*SQL Injection*).
2. **Строгая типизация:** передают значение в его реальном типе данных (число, дата, `uuid`, `jsonb`), а не как кусок сырого текста, который нужно форматировать и экранировать вручную.

---

## 3. Подключение к PostgreSQL и управление ресурсами

### Строка подключения (`Connection String`)
Соединение (`DbConnection`) связано с сеансом БД и его состоянием. Настраивается оно через строку подключения, состоящую из пар `Ключ=Значение;`.

**Типовой набор параметров:**
1. `Host` (или `Server`) — адрес сервера БД (например, `localhost`).
2. `Port` — порт PostgreSQL (по умолчанию `5432`).
3. `Database` — имя базы данных.
4. `Username` — имя пользователя.
5. `Password` — пароль.

**Дополнительные настройки:**
* Тайм-ауты: `Timeout` (тайм-аут подключения), `Command Timeout` (тайм-аут выполнения запроса).
* Безопасность: `SSL Mode` (`Disable`, `Prefer`, `Require`, `VerifyFull`).
* Пул соединений: `Pooling=true`, `Minimum Pool Size`, `Maximum Pool Size`.

**Пример строки подключения:**
```text
Host=localhost;Port=5432;Database=mydb;Username=postgres;Password=secret;Pooling=true;Minimum Pool Size=1;Maximum Pool Size=20
```

### Различие `using` и `await using`
Соединения, команды и ридеры удерживают неуправляемые ресурсы (сетевые сокеты, дескрипторы курсоров), поэтому их необходимо гарантированно освобождать:
* **`using`** — вызывает синхронный метод `Dispose()` интерфейса `IDisposable`. Если закрытие ресурса требует сетевого обмена с БД, поток блокируется.
* **`await using`** — вызывает асинхронный метод `DisposeAsync()` интерфейса `IAsyncDisposable`. Позволяет освобождать ресурсы и закрывать сетевые сессии **асинхронно** без блокировки потока, если провайдер (как `Npgsql`) поддерживает асинхронное освобождение.

---

## 4. Пул соединений (Connection Pool) и `NpgsqlDataSource`

### Как работает Connection Pool
Открытие соединения в приложении (`connection.OpenAsync()`) **не означает**, что каждый раз с нуля создается новый TCP-сеанс и проходит тяжелая авторизация в PostgreSQL.
* Провайдер использует **пул соединений (Connection Pool)** — это встроенная оптимизация драйвера.
* При вызове `Close()` или `Dispose()` физическое соединение **не разрывается**, а сбрасывает состояние и возвращается в пул.
* Следующий вызов `OpenAsync()` мгновенно берет уже готовое открытое физическое соединение из пула.

### Современный подход: `NpgsqlDataSource` (Npgsql 7.0+)
В современном `Npgsql` рекомендуемой точкой входа является **`NpgsqlDataSource`**:
* Его рекомендуется создавать **один раз на всё приложение** (как Singleton в DI-контейнере).
* Он инкапсулирует конфигурацию подключения, настройку типов (например, JSON, enum, плагины) и сам **пул соединений**.
* При этом стандартное API ADO.NET никуда не исчезает: `NpgsqlDataSource` может как выдавать `NpgsqlConnection` (`await dataSource.OpenConnectionAsync()`), так и создавать команды напрямую (`dataSource.CreateCommand(...)`).

```csharp
using Npgsql;

var connectionString = "Host=localhost;Port=5432;Database=app_db;Username=postgres;Password=postgres";

// Создаем один раз на время жизни приложения
await using var dataSource = NpgsqlDataSource.Create(connectionString);

// Открываем соединение из пула
await using var connection = await dataSource.OpenConnectionAsync();
```

---

## 5. Потоковое чтение результатов: `DbDataReader`

`DbDataReader` (`NpgsqlDataReader`) — фундаментальный способ чтения результата `SELECT`-запроса в ADO.NET.
* Он работает **последовательно**: изначально курсор стоит **перед первой строкой**.
* Каждый вызов `await reader.ReadAsync()` переводит курсор на следующую строку и возвращает `true`, пока строки есть, либо `false`, когда выборка закончилась.

### Чтение по типу, по имени и по индексу

Методы строго типизированного чтения:
* `reader.GetInt32(ordinal)` — возвращает `int`
* `reader.GetString(ordinal)` — возвращает `string`
* `reader.GetDateTime(ordinal)` — возвращает `DateTime`
* `reader.GetBoolean(ordinal)`, `reader.GetDecimal(ordinal)` и др.
* `reader.GetFieldValue<T>(ordinal)` — универсальный метод чтения в конкретный тип .NET.

Это делает код явным и быстрым (без лишней упаковки `boxing`), но требует, чтобы тип столбца в БД совпадал с ожидаемым типом в C#.

**По имени vs По индексу:**
* Чтение по имени (`reader["email"]`) проще читать человеку, но на каждой строке тратится время на поиск индекса колонки по строке.
* Обращение по индексу (`reader.GetString(2)`) работает быстрее, но жесткие числа (`0, 1, 2`) ухудшают читаемость.
* **Практический компромисс:** один раз перед циклом `while` определить порядковые номера колонок через `reader.GetOrdinal("column_name")`, а внутри цикла читать значения по полученным индексам.

### `null` в C# и `DBNull` в ADO.NET
* В SQL `NULL` означает отсутствие значения.
* В .NET для отсутствия ссылки используется `null` (или `Nullable<T>`, например `int?`).
* Однако `DbDataReader` исторически представляет пустое SQL-значение через специальный объект-одиночку **`DBNull.Value`**.
* Поэтому вызов `reader.GetString(idx)` на колонке со значением `NULL` выбросит исключение `InvalidCastException`. Перед чтением nullable-колонки необходимо проверять её через **`reader.IsDBNull(idx)`**:

```csharp
int idOrd = reader.GetOrdinal("id");
int nameOrd = reader.GetOrdinal("name");
int bioOrd = reader.GetOrdinal("bio"); // колонка допускает NULL

while (await reader.ReadAsync())
{
    int id = reader.GetInt32(idOrd);
    string name = reader.GetString(nameOrd);
    string? bio = reader.IsDBNull(bioOrd) ? null : reader.GetString(bioOrd);
}
```

### Преимущества `DbDataReader`
Главная сила `DbDataReader` — **потоковая модель (forward-only, read-only)**. Вы читаете строки по мере их поступления из сети и сразу обрабатываете, не выделяя память под гигабайтные выборки целиком.

---

## 6. Связная (Connected) и Несвязная (Disconnected) модели ADO.NET

ADO.NET существует не только как связка `Connection + Command + DataReader` (*Connected-модель*, где соединение должно быть открыто на протяжении всего чтения).

Классическая архитектура ADO.NET также включает **дисконнектные (Disconnected) компоненты**:
* **`DataTable`** — таблица данных в оперативной памяти (строки `DataRow`, колонки `DataColumn`, ограничения).
* **`DataSet`** — коллекция из нескольких `DataTable` и связей между ними в памяти.
* **`DbDataAdapter` (`NpgsqlDataAdapter`)** — мост между БД и `DataSet` / `DataTable`.

**Разница подходов:**
* `DbDataReader` дает быстрый потоковый *read-only* доступ при активном соединении.
* `DbDataAdapter` открывает соединение, заполняет `DataTable` / `DataSet` методом `Fill()`, закрывает соединение, позволяет редактировать строки в памяти и при необходимости синхронизирует изменения обратно в БД методом `Update()`. *(В современной разработке на .NET вместо `DataSet` почти всегда используют коллекции строго типизированных классов DTO/Entity или ORM).*

---

## 7. Параметры SQL, защита от SQL-инъекций и типизация

### Почему нельзя склеивать SQL через конкатенацию строк?
Проблема возникает, когда пользовательский ввод (например, `name`) напрямую подставляется в строку запроса:
```csharp
// ❌ ОПАСНО: Уязвимость к SQL Injection!
string sql = $"SELECT * FROM users WHERE name = '{userInput}'";
```
Если злоумышленник передаст в `userInput` строку со спецсимволами или элементами SQL-кода (например, `' OR 1=1; DROP TABLE users; --`), он изменит синтаксическую структуру запроса. Это и называется **SQL-инъекцией (SQL Injection)**.

**Решение — параметризованные запросы:**
```csharp
// ✅ БЕЗОПАСНО: Параметры передаются отдельно от текста SQL
string sql = "SELECT id, name FROM users WHERE name = @name";
cmd.Parameters.AddWithValue("@name", userInput);
```

### Ограничения параметров: данные vs идентификаторы
Параметры SQL предназначены **только для значений данных** (`WHERE`, `VALUES`, `SET`), но **не для синтаксических частей запроса** (имен таблиц, колонок, `ORDER BY ASC/DESC`).
Нельзя сделать так:
```sql
-- ❌ Так параметр работать НЕ будет:
SELECT * FROM @tableName ORDER BY @columnName;
```
Если имя таблицы или колонки обязательно должно быть динамическим:
1. Используйте **белый список (whitelist)** допустимых идентификаторов в C#-коде (`switch` или `HashSet<string>`).
2. Используйте специальные механизмы безопасного цитирования идентификаторов, предоставляемые драйвером (например, `NpgsqlCommandBuilder.QuoteIdentifier(...)`).

### Типы параметров в Npgsql (`NpgsqlDbType`)
При вызове `Parameters.AddWithValue("@p", value)` `Npgsql` пытается автоматически вывести тип PostgreSQL из типа .NET (`int` $\to$ `integer`, `string` $\to$ `text`, `DateTime` $\to$ `timestamp`). Во многих случаях этого достаточно.

Однако PostgreSQL использует строгую типизацию, и для специфических типов (`jsonb`, массивов, `inet`, `cidr`, `daterange`, `uuid`) или при передаче `null` требуется **явное указание `NpgsqlDbType`**:
```csharp
using NpgsqlTypes;

var jsonParam = new NpgsqlParameter("@payload", NpgsqlDbType.Jsonb)
{
    Value = "{\"role\": \"admin\"}"
};
cmd.Parameters.Add(jsonParam);
```

---

## 8. Полный цикл CRUD (Create, Read, Update, Delete) и Транзакции в ADO.NET

**CRUD** — четыре базовые операции управления данными:
* **C**reate (`INSERT`) — создание записи
* **R**ead (`SELECT`) — чтение записей
* **U**pdate (`UPDATE`) — обновление записи
* **D**elete (`DELETE`) — удаление записи

Также команды в ADO.NET выполняются одним из трёх ключевых методов `DbCommand`:
1. **`ExecuteNonQueryAsync()`** — для команд, не возвращающих таблицу (`INSERT`, `UPDATE`, `DELETE`, `CREATE TABLE`). Возвращает количество затронутых строк (`int`).
2. **`ExecuteScalarAsync()`** — возвращает одно-единственное значение (первый столбец первой строки результата). Идеально для `COUNT(*)`, `SUM(...)` или `INSERT ... RETURNING id`.
3. **`ExecuteReaderAsync()`** — возвращает `DbDataReader` для чтения набора строк (`SELECT`).

### Практический пример полного CRUD + Транзакция на C# (`Npgsql`)

```csharp
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Npgsql;
using NpgsqlTypes;

public record User(int Id, string Name, string Email, DateTime CreatedAt);

public class UserRepository
{
    private readonly NpgsqlDataSource _dataSource;

    public UserRepository(NpgsqlDataSource dataSource)
    {
        _dataSource = dataSource;
    }

    // 1. CREATE (INSERT + RETURNING id через ExecuteScalarAsync)
    public async Task<int> CreateUserAsync(string name, string email)
    {
        const string sql = """
            INSERT INTO users (name, email, created_at)
            VALUES (@name, @email, @createdAt)
            RETURNING id;
            """;

        await using var cmd = _dataSource.CreateCommand(sql);
        cmd.Parameters.AddWithValue("@name", NpgsqlDbType.Text, name);
        cmd.Parameters.AddWithValue("@email", NpgsqlDbType.Text, email);
        cmd.Parameters.AddWithValue("@createdAt", NpgsqlDbType.TimestampTz, DateTime.UtcNow);

        object? result = await cmd.ExecuteScalarAsync();
        return Convert.ToInt32(result);
    }

    // 2. READ (SELECT по ID через ExecuteReaderAsync)
    public async Task<User?> GetByIdAsync(int id)
    {
        const string sql = """
            SELECT id, name, email, created_at
            FROM users
            WHERE id = @id;
            """;

        await using var cmd = _dataSource.CreateCommand(sql);
        cmd.Parameters.AddWithValue("@id", NpgsqlDbType.Integer, id);

        await using var reader = await cmd.ExecuteReaderAsync();
        if (!await reader.ReadAsync())
            return null;

        return new User(
            Id: reader.GetInt32(reader.GetOrdinal("id")),
            Name: reader.GetString(reader.GetOrdinal("name")),
            Email: reader.GetString(reader.GetOrdinal("email")),
            CreatedAt: reader.GetDateTime(reader.GetOrdinal("created_at"))
        );
    }

    // 3. READ ALL (Потоковое чтение списка через DbDataReader)
    public async Task<List<User>> GetAllAsync()
    {
        const string sql = "SELECT id, name, email, created_at FROM users ORDER BY id;";

        await using var cmd = _dataSource.CreateCommand(sql);
        await using var reader = await cmd.ExecuteReaderAsync();

        // Определяем индексы колонок один раз перед циклом
        int idOrd = reader.GetOrdinal("id");
        int nameOrd = reader.GetOrdinal("name");
        int emailOrd = reader.GetOrdinal("email");
        int createdOrd = reader.GetOrdinal("created_at");

        var users = new List<User>();
        while (await reader.ReadAsync())
        {
            users.Add(new User(
                Id: reader.GetInt32(idOrd),
                Name: reader.GetString(nameOrd),
                Email: reader.GetString(emailOrd),
                CreatedAt: reader.GetDateTime(createdOrd)
            ));
        }

        return users;
    }

    // 4. UPDATE (Обновление данных через ExecuteNonQueryAsync)
    public async Task<bool> UpdateEmailAsync(int id, string newEmail)
    {
        const string sql = """
            UPDATE users
            SET email = @email
            WHERE id = @id;
            """;

        await using var cmd = _dataSource.CreateCommand(sql);
        cmd.Parameters.AddWithValue("@id", NpgsqlDbType.Integer, id);
        cmd.Parameters.AddWithValue("@email", NpgsqlDbType.Text, newEmail);

        int affectedRows = await cmd.ExecuteNonQueryAsync();
        return affectedRows > 0;
    }

    // 5. DELETE (Удаление записи через ExecuteNonQueryAsync)
    public async Task<bool> DeleteAsync(int id)
    {
        const string sql = "DELETE FROM users WHERE id = @id;";

        await using var cmd = _dataSource.CreateCommand(sql);
        cmd.Parameters.AddWithValue("@id", NpgsqlDbType.Integer, id);

        int affectedRows = await cmd.ExecuteNonQueryAsync();
        return affectedRows > 0;
    }

    // 6. ТРАНЗАКЦИЯ (DbTransaction — атомарное выполнение нескольких операций)
    public async Task TransferBalanceAsync(int fromUserId, int toUserId, decimal amount)
    {
        await using var connection = await _dataSource.OpenConnectionAsync();
        await using var transaction = await connection.BeginTransactionAsync();

        try
        {
            const string debitSql = "UPDATE accounts SET balance = balance - @amount WHERE user_id = @userId;";
            await using (var debitCmd = new NpgsqlCommand(debitSql, connection, transaction))
            {
                debitCmd.Parameters.AddWithValue("@amount", NpgsqlDbType.Numeric, amount);
                debitCmd.Parameters.AddWithValue("@userId", NpgsqlDbType.Integer, fromUserId);
                await debitCmd.ExecuteNonQueryAsync();
            }

            const string creditSql = "UPDATE accounts SET balance = balance + @amount WHERE user_id = @userId;";
            await using (var creditCmd = new NpgsqlCommand(creditSql, connection, transaction))
            {
                creditCmd.Parameters.AddWithValue("@amount", NpgsqlDbType.Numeric, amount);
                creditCmd.Parameters.AddWithValue("@userId", NpgsqlDbType.Integer, toUserId);
                await creditCmd.ExecuteNonQueryAsync();
            }

            // Фиксируем изменения, если обе операции прошли успешно
            await transaction.CommitAsync();
        }
        catch
        {
            // Откатываем все изменения при любой ошибке
            await transaction.RollbackAsync();
            throw;
        }
    }
}
```
