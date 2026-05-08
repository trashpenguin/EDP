# EDP – Event Driven Programming Final Project

A comprehensive final project demonstrating core concepts of event-driven programming, developed as part of a university or advanced coursework curriculum.

---

## 📌 Table of Contents
- [About the Project](#about-the-project)
- [Key Features](#key-features)
- [Built With](#built-with)
- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Installation & Running](#installation--running)
- [Project Structure](#project-structure)
- [Usage Example](#usage-example)
- [Acknowledgments](#acknowledgments)

---

## About the Project

This repository contains the final project for an **Event Driven Programming** course. It showcases the implementation of reactive, event-based systems where the flow of the program is determined by events such as user actions, sensor outputs, or message passing. The project emphasizes loose coupling, asynchronous handling, and responsive design.

---

## Key Features

- **Event‑Driven Architecture** – Central event loop with custom event handlers.
- **Modular Design** – Event producers, consumers, and dispatchers are separated.
- **Asynchronous Processing** – Non‑blocking execution for long‑running tasks.
- **GUI Integration** – (If applicable) User interface events drive application logic.
- **Extensible** – Easy to add new event types and listeners.

---

## Built With

| Technology | Purpose |
|------------|---------|
| **Visual Basic .NET** (100%) | Primary language; uses .NET’s event model and WinForms/WPF event handlers |
| .NET Framework / .NET Core | Provides the runtime and event infrastructure |

> *Note: The repository consists entirely of Visual Basic .NET code, making it ideal for learning event handling in the .NET ecosystem.*

---

## Getting Started

Follow these instructions to get a copy of the project up and running on your local machine for development and testing.

### Prerequisites

- **Windows OS** (recommended) – The project targets .NET Framework / WinForms.
- **Visual Studio** (2019 or later) with **.NET desktop development** workload.
- Basic knowledge of events, delegates, and UI programming.

### Installation & Running

1. **Clone the repository**
   ```bash
   git clone https://github.com/trashpenguin/EDP.git
   cd EDP
Open the solution

Launch Visual Studio.

Open the .sln or .vbproj file located in the EDP folder.

Build & Run

Press F5 or click Start to compile and execute the project.

Interact with the GUI (if present) or observe the console‑based event flow.

Project Structure
text
EDP/
├── EDP/                     # Main project folder
│   ├── Form1.vb             # Main form / event handlers
│   ├── Program.vb           # Application entry point
│   ├── ...                  # Additional modules / classes
├── .gitignore
└── README.md
The actual file names may vary – refer to the latest commit for precise structure.

Usage Example
Once the application runs:

Click buttons, type into text boxes, or trigger custom events.

The system will log or display event information (e.g., “Button Click Event raised”).

You can see how different parts of the code respond to the same event (multicast delegates) or how events are passed between forms.

Acknowledgments
Course instructor and teaching assistants for guidance on event-driven paradigms.

Microsoft .NET documentation for event handling patterns.

Open source community for inspiration and debugging help.

License
This project is for educational purposes as part of a final assignment. Please consult your instructor before reusing any code.

Made with ❤️ as a Final Project in Event Driven Programming

text

### How to add it to your repository:
1. Go to your repository: `https://github.com/trashpenguin/EDP`
2. Click **Add file** → **Create new file**
3. Name the file exactly: `README.md`
4. Copy the entire markdown content above and paste it into the editor
5. Scroll down and click **Commit new file**

The README will now appear beautifully formatted on your repository's main page. It explains the project's purpose, how to run it, and highlights that it is written in Visual Basic .NET for event-driven concepts.

Would you like me to adjust any section (e.g., add specific features of your project or change the language focus)?