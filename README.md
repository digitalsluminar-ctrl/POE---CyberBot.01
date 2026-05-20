# POE - CyberBot.01Overview
CyberBot is a Windows Presentation Foundation (WPF) desktop application built with C# and .NET. It functions as an interactive virtual assistant designed to educate users on cybersecurity best practices, test their knowledge through interactive quizzes, and manage personal tasks with built-in reminder tracking.

This project demonstrates strong capabilities in desktop UI design using XAML, state management, event-driven programming, the integration of native Windows APIs, and modern CI/CD practices.

💻 Tech Stack
Language: C#

Framework: WPF (Windows Presentation Foundation) / .NET 8

UI Markup: XAML

Libraries: System.Speech.Synthesis (Microsoft Speech API)

DevOps: GitHub Actions (Automated CI Pipeline)

IDE: Visual Studio 2022

🚀 Key Features
Interactive Chat Interface: A continuous, conversational UI where the bot responds to specific user keywords (simulated NLP) to trigger different workflows, such as providing cybersecurity tips on topics like Phishing, Firewalls, and Hacking.

Text-to-Speech Integration: Utilizes the SpeechSynthesizer class to provide real-time audio greetings and accessibility features (configured to the Microsoft Desktop voice).

Cybersecurity Quiz Engine: A built-in state machine that tracks active quizzes, evaluates multiple-choice inputs (A, B, C, D), provides instant educational feedback on incorrect answers, and calculates a final score.

Task & Reminder Management: A dedicated secondary window (TaskWindow.xaml) featuring full CRUD capabilities for local tasks. Users can add tasks via natural chat commands (e.g., "add task"), set specific date reminders, and mark them as complete.

Dynamic Activity Logging: The system silently logs recent user actions and bot state changes, allowing users to request a printed activity history on demand.

⚙️ Continuous Integration (CI/CD)
This repository is configured with a GitHub Actions workflow (dotnet.yml). Every push to the main or master branch triggers an automated build environment on a windows-latest runner to restore dependencies and verify that the application compiles successfully (dotnet build --configuration Release).

🛠️ Architecture & Logic Highlights
State Management: The application effectively manages multiple internal states (e.g., isQuizActive, waitingForTask, waitingForReminderText) to dictate how user input is parsed and routed without breaking the conversational flow.

Event-Driven UI: Clean separation of concerns between the XAML frontend and the C# code-behind, utilizing click handlers, focus events (GotFocus/LostFocus for placeholder text), and UI thread updating.

Custom Data Structures: Implements generic List<T> and Tuple structures to store question banks and activity logs in memory efficiently.

🏃‍♂️ How to Run Locally
Clone this repository to your Windows machine.

Open the solution file (.sln) in Visual Studio 2022.

Ensure your system audio is enabled to hear the text-to-speech greetings.

Press F5 or click Start to build and launch the application.

Tip: Type "hello" to start, or type "add task" to test the automated reminder workflow.
