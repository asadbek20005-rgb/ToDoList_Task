📌 Project Goal
This project implements a simple ToDo List application in the form of a Console Application.
The application is built following the principles of Clean Architecture and is divided into Domain, Application, Infrastructure and Presentation (Console UI) layers.

⚙️ Technologies

C# 12 / .NET 8

Clean Architecture

JSON file – for storing data (usually instead of DB)

xUnit – for testing

📂 Project structure
ToDoList/
│── ToDo.Domain/ # Main model and entities (Task, Category, Priority, Status...)
│── ToDo.Application/ # Business logic, Services, DTOs, Validators
│── ToDo.Infrastructure/ # Data storage (JSON File Repository)
│── ToDo.Presentation/ # Console Application (UI part)
│── ToDo.Tests/ # Unit tests (xUnit)
│── README.md # Project documentation


🔑 Main features

✅ Add task (name, description, due date, category, priority, status)

📖 View task list

✏️ Edit task

❌ Delete task

🔍 Filter tasks (category, status, due date, priority)

⏳ View overdue tasks

💾 Save data to JSON file



🚀 Launch
Download the project:
git clone https://github.com/username/todolist-clean-arch.git
cd todolist-clean-arch

Build:
dotnet build

Launch the Console app:
dotnet run --project ToDo.Presentation


📌 Usage

After starting, the following menu will appear on the console:
1. Add task
2. View tasks
3. Change task status
4. Delete task
5. Filter
6. Sort


🧪 Tests
Running tests:
dotnet test

🏗️ Architecture (Clean Architecture Diagram)

            +-------------------+
            |   Presentation    |
            | (Console App UI)  |
            +---------+---------+
                      |
                      v
            +-------------------+
            |   Application     |
            | (Services, DTOs,  |
            |  Validators)      |
            +---------+---------+
                      |
                      v
            +-------------------+
            |    Domain         |
            | (Entities, Enums) |
            +---------+---------+
                      |
                      v
            +-------------------+
            |  Infrastructure   |
            | (File Repository) |
            +-------------------+


👤 Author

Name: Asadbek Shermatov





Running tests:
