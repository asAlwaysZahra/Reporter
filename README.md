# Reporter

A lightweight, **plugin-based reporting framework** built with .NET 8.  
Designed for extensibility, dynamic report loading, and SQL-based logging.

[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet&logoColor=white)]()
[![C#](https://img.shields.io/badge/C%23-12-blue?logo=csharp)]()
[![License](https://img.shields.io/badge/license-MIT-informational)]()

---

## 📌 Overview
**Reporter** is a modular framework for building and executing reports.  
The core idea: **every report and data source is a plugin (DLL)** that can be dynamically loaded by the main console application (`TestReporter`).  

Reports are executed through **LINQ queries**, and every execution is logged into a **SQL Server database (`ReporterLogsDB`)**.

---

## 🔎 How It Works

- Each **Data Provider (plugin)** is a DLL that defines:
  - What data to fetch  
  - Which reports (tasks) exist  
  - How to execute those reports (LINQ-based logic)  

- The **Reporter core library**:
  - Provides the standard runtime for executing reports  
  - Handles logging of report runs into **SQL Server**  

- The **TestReporter console app**:
  - Loads all available plugins from the `plugins/` folder  
  - Lists reports and allows you to run them interactively  
  - Supports adding or removing extensions at runtime  

---

## ✨ Key Capabilities
- **Plugin-based architecture**: drop a DLL in `plugins/`, and it becomes available instantly  
- **Extensible providers**: create your own by implementing `ImplementationBase` interfaces  
- **Dynamic runtime loading**: enable/disable extensions without rebuilding the app  
- **SQL Server logging**: execution logs stored in `ReporterLogsDB`  
- **Console runner**: simple CLI for managing and running reports  

---

## 🏗️ Project Architecture
- **Reporter**: Core logic: report execution with LINQ  
- **ImplementationBase**: Contracts & interfaces for providers  
- **DataProviders**: Example plugins (`DataProviderOne`, `DataProviderTwo`, plus test clones)  
- **TestReporter**: Console app for running & managing reports  

---

## ⚙️ Prerequisites
- [.NET SDK 8.0](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)  
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or newer  
- SQL Server instance (local or remote)  
- Database named **`ReporterLogsDB`**

---

## 🚀 Getting Started

### 1. Clone the repository
```bash
git clone https://github.com/asAlwaysZahra/Reporter.git
````

### 2. Open solution

Open `ReporterSolution.sln` in Visual Studio.

### 3. Database setup

```sql
CREATE DATABASE ReporterLogsDB;
```

### 4. Build & run

* Set **`TestReporter`** as the Startup Project.
* Build the solution.

---

## 🔌 Plugin Configuration

1. Copy the `plugins` folder and `tasks.json` into:

   ```
   TestReporter/bin/Debug/net8.0
   ```

   > ⚠️ `tasks.json` is required for **DataProviderTwo**

2. To add new providers, place their DLLs into the same folder.

3. At runtime, use:

   ```
   Manage Extensions → Add New Extension
   ```

   to activate plugins dynamically.

---

## 🛠️ Creating a New Data Provider

1. Create a **Class Library** project (`net8.0`).
2. Add a reference to **`ImplementationBase`**.
3. Implement the required interfaces (see `DataProviderOne` as an example).
4. Build → copy the DLL into the `plugins/` folder.
5. Enable it via the **Manage Extensions** menu in `TestReporter`.

---

## ▶️ Running Reports

* Launch **TestReporter**.
* Available reports will be listed (from all active plugins).
* Select and execute a report.
* Results will show in the console and logs will be stored in **`ReporterLogsDB`**.

---

## 📂 Folder Structure

```
Reporter/
├─ ReporterSolution/
│  ├─ Reporter/              # Core runtime
│  ├─ ImplementationBase/    # Contracts & interfaces
│  ├─ DataProviders/         # Example providers
│  └─ TestReporter/          # Console runner
├─ plugins/                  # Ready-to-use DLLs
├─ tasks.json                # Required for DataProviderTwo
└─ README.md
```

---

## 🤝 Contributing

Pull requests are welcome! For major changes, open an **Issue** first to discuss.

Guidelines:

* Clean, documented code
* Stick to `.NET 8`
* Add/update tests if possible

