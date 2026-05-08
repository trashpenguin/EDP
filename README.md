# EDP – Event Driven Programming Final Project

*A hands-on demonstration of reactive, event-based systems using Visual Basic .NET*

---

## 📚 Table of Contents

- [About the Project](#about-the-project)
- [✨ Features](#-features)
- [🛠️ Tech Stack](#️-tech-stack)
- [🚀 Getting Started](#-getting-started)
- [📁 Project Structure](#-project-structure)
- [💡 Usage](#-usage)
- [🤝 Contributing](#-contributing)
- [📄 License](#-license)

---

## About the Project

This repository contains the final project for an **Event Driven Programming** course. It showcases the implementation of reactive, event-based systems where the program flow is determined by events such as user actions, sensor outputs, or message passing.

The project emphasizes:

- **Loose coupling** between components
- **Asynchronous** event handling
- **Responsive** UI design patterns

---

## ✨ Features

- 🎯 **Event-Driven Architecture** – Central event loop with custom event handlers
- 🔧 **Modular Design** – Separated event producers, consumers, and dispatchers
- ⚡ **Asynchronous Processing** – Non-blocking execution for long-running tasks
- 🖥️ **GUI Integration** – User interface events drive application logic
- 🔌 **Extensible** – Easy to add new event types and listeners
- 📡 **Multicast Delegates** – Multiple handlers respond to the same event

---

## 🛠️ Tech Stack

[![Visual Basic .NET](https://img.shields.io/badge/Visual%20Basic%20.NET-512BD4?logo=dotnet&logoColor=white)](https://learn.microsoft.com/en-us/dotnet/visual-basic/)
[![.NET Framework](https://img.shields.io/badge/.NET%20Framework-512BD4?logo=dotnet&logoColor=white)]
[![Windows](https://img.shields.io/badge/Windows-0078D6?logo=windows&logoColor=white)]

| Technology | Purpose |
|------------|---------|
| **Visual Basic .NET** | Primary language with .NET event model and WinForms/WPF handlers |
| .NET Framework / .NET Core | Runtime and event infrastructure |

---

## 🚀 Getting Started

Follow these instructions to get a copy of the project up and running locally.

### Prerequisites

- **Windows OS** (recommended) – The project targets .NET Framework / WinForms
- **Visual Studio** (2019 or later) with **.NET desktop development** workload
- Basic knowledge of events, delegates, and UI programming

### Installation & Running

```bash
# Clone the repository
git clone https://github.com/trashpenguin/EDP.git
cd EDP
```

1. **Open the solution** in Visual Studio
2. **Build the project** (`Ctrl + Shift + B`)
3. **Run the application** (`F5`)

---

## 📁 Project Structure

```
EDP/
├── EDP/
│   ├── Form1.vb           # Main form / event handlers
│   ├── Program.vb         # Application entry point
│   ├── ...                # Additional modules / classes
├── .gitignore
└── README.md
```

---

## 💡 Usage

Once the application runs:

1. Click buttons, type into text boxes, or trigger custom events
2. The system logs or displays event information (e.g., "Button Click Event raised")
3. Observe how different code parts respond to the same event via multicast delegates

---

## 🤝 Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

---

## 📄 License

This project is for **educational purposes** as part of a final assignment. Please consult your instructor before reusing any code.

---
*Made with ❤️ as a Final Project in Event Driven Programming*
