# 📱 Financial Manager

[![.NET MAUI](https://img.shields.io/badge/.NET%20MAUI-10.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/en-us/apps/maui)
[![Language](https://img.shields.io/badge/Language-C%23%20%2F%20XAML-blue?logo=csharp)](https://learn.microsoft.com/en-us/dotnet/csharp/)
[![Database](https://img.shields.io/badge/Database-SQLite-003B57?logo=sqlite)](https://www.sqlite.org/index.html)
[![MVVM](https://img.shields.io/badge/Architecture-MVVM%20Toolkit%208.4.2-orange)](https://learn.microsoft.com/en-us/dotnet/communitytoolkit/mvvm/)

**Financial Manager** is a modern, high-performance, cross-platform mobile application for personal expense tracking built on top of the cutting-edge **.NET 10.0 MAUI** framework. It helps you effortlessly monitor income, expenses, and savings, analyze financial behavior via interactive charts, and manage your budget across multiple currencies dynamically.

---

## 📸 Screenshots

<p align="center">
  <img src="https://github.com/user-attachments/assets/9107da13-976d-4062-8563-9ea7dff954e0" width="31%" alt="photo_2026-06-11_19-15-41"  />
  <img src="https://github.com/user-attachments/assets/b507cde7-ab6d-479f-a05c-d347476c8d7d" width="31%" alt="Screenshot_20260611_190127" />
  <img src="https://github.com/user-attachments/assets/6f863385-9727-4b6a-af35-3411f00bd009" width="31%" alt="Screenshot_20260611_190136" />
  <br />
  <img src="https://github.com/user-attachments/assets/410a71a8-5e53-4f2a-bc07-4dc18e113ed6" width="31%" alt="Screenshot_20260611_190146" />
  <img src="https://github.com/user-attachments/assets/80a0ee9a-92fa-4fd5-a89c-acf765386e2f" width="31%" alt="Screenshot_20260611_190153" />
  <img src="https://github.com/user-attachments/assets/0a713e01-f8eb-444e-bd94-ab125f54aa1c" width="31%" alt="Screenshot_20260611_190159" />
  <br />
  <img src="https://github.com/user-attachments/assets/b0cd60b6-da6f-438e-ae66-7347e480e0cc" width="31%" alt="Screenshot_20260611_190204" />
  <img src="https://github.com/user-attachments/assets/d3fd8357-e795-4229-a005-24ae4a87680d" width="31%" alt="Screenshot_20260611_190207" />
  <img src="https://github.com/user-attachments/assets/757bb9ce-bf7f-4fa5-8037-7b9395df82b8" width="31%" alt="Screenshot_20260611_190213" />
</p>

---

## ✨ Core Features

### 📊 1. Visual Analytics & Reporting
* **Interactive Donut Charts:** Powered by `Microcharts.Maui` and `SkiaSharp` to visually break down expense shares by category.
* **Automatic Color Schemes:** Dynamic styling per category icon (e.g., 🍔 Food — Red, 💰 Income — Green, 💊 Healthcare/Therapist — Pink).
* **Live Chart Legend:** Calculates totals and filters data in real-time.

### 💱 2. Smart Multi-Currency Support (`ICurrencyService`)
* **Native Currency Support:** Seamlessly switch between Ukrainian Hryvnia (₴), US Dollar ($), and Euro (€).
* **Historical Rate Locking:** Saves transactions in their original currency while binding the historical exchange rate at the exact moment of the operation.
* **On-The-Fly Conversion:** Instant conversion of your dashboard total, balances, and filter timelines into your selected display currency.

### 📥 3. Streamlined Cash Flow Segmentation
All transactions are cleanly mapped into distinct categories, each paired with a clear visual icon for rapid scanning:
* `📥` **Income** (Salary, freelance, asset sales, cashback).
* `📤` **Expenses** (Rent, taxes, groceries, hardware updates, utilities).
* `🐷` **Savings** (Piggy banks, investment pots, emergency funds).
* `🔄` **Others**.

### 🔍 4. Powerful Filtering & Sorting
* **Timeframe Constraints:** Select custom date ranges smoothly using reactive `StartDate` and `EndDate` lifecycle bindings.
* **Multi-Selection Filtering:** Toggle view state filters for multiple transaction types concurrently.
* **Smart Sorting:** Sort your history by transaction value in ascending or descending order, automatically normalizing values to UAH for absolute mathematical accuracy.

### 🌐 5. Dynamic Localization (`ILocalizationService`)
* Complete localization architecture layout supporting English and Ukrainian.
* Dynamically translates UI components, category names, and core workflow states based on user preferences.

### 💾 6. Secure Data Backup & Restore (`BackupPage`)
* **Data Portability:** Easily export your entire transaction history, categories, and settings into a portable backup file.
* **Seamless Recovery:** Import previous backups to instantly restore all financial records, ensuring your data remains completely safe during device migrations or clean app reinstalls.
* **Reliable Serialization:** Utilizes high-fidelity `Newtonsoft.Json` structures to guarantee clean, lightweight, and corruption-free local data formatting.

---

## 🛠 Tech Stack & Exact Dependencies

The project is built using modern .NET development ecosystems to guarantee rapid UI rendering times and efficient runtime execution.

### Target Frameworks & OS Compatibility
* **Android:** API 21.0+ (Lollipop) onwards (`net10.0-android`)
* **iOS / MacCatalyst:** iOS 15.0+ / macOS 15.0+ (`net10.0-ios`, `net10.0-maccatalyst`)
* **Windows:** Windows 10 version 10.0.17763.0+ (`net10.0-windows`)

### 📦 Key Dependencies & Packages Used

| Package | Version | Purpose |
| :--- | :--- | :--- |
| **Microsoft.Maui.Controls** | `10.0.60` | Standard core platform components & lifecycle layout engine. |
| **CommunityToolkit.Mvvm** | `8.4.2` | Robust MVVM design architecture with optimized Source Generators. |
| **Microcharts.Maui** | `1.0.1` | High-performance cross-platform charting components. |
| **sqlite-net-pcl** | `1.9.172` | Synchronous / Asynchronous lightweight local data storage layer. |
| **SQLiteNetExtensions / Async**| `2.1.0` | ORM relationship mapper (One-to-Many / Many-to-One data integrity). |
| **Newtonsoft.Json** | `13.0.4` | High-fidelity JSON serialization for data backups and API currency responses. |

### ⚡ Optimization Highlights
* **XAML Source Generation (`SourceGen`):** The project explicitly utilizes `<MauiXamlInflator>SourceGen</MauiXamlInflator>`. XAML design layouts are completely compiled into native C# files during compilation instead of inflating at runtime. This heavily reduces application startup overhead and memory allocations on physical mobile devices.

---

## 📂 Architecture Overview

The codebase strictly implements separation of concerns to maintain long-term code scalability and testing safety:
* **Repositories (`Data/Repositories`):** Isolated data access interfaces abstraction layer handling raw asynchronous SQLite procedures securely.
* **Services (`Services`):** Decoupled business engine workflows like `CurrencyService` (external rates fetching) and `LocalizationService` (handling global UI strings).
* **ViewModels (`ViewModels`):** UI-independent lifecycle state classes handling data pipelines, data-binding states, filtering, and cross-view notifications.

---

## 🚀 Getting Started

1.  **Clone the Repository:**
    ```bash
    git clone [https://github.com/your-username/FinancialManager.git](https://github.com/your-username/FinancialManager.git)
    ```
2.  Ensure you have **Visual Studio 2022** or **JetBrains Rider** installed with the **.NET MAUI (.NET 10.0)** development workload enabled.
3.  Open the solution file: `FinancialManager.sln`.
4.  Select your target environment/device (e.g., Android Emulator, Android Device, or iOS Simulator).
5.  Press **F5** to build, deploy, and launch!

---

## 📝 License

This project is open-source and available under the **MIT License**. Feel free to use it as a robust foundation for your own custom cross-platform applications!
