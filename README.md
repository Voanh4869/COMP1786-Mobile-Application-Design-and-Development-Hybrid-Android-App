# Project Expense Tracker - Hybrid Application (COMP1786)

## Overview
This repository contains the **Hybrid (.NET MAUI)** source code for the Project Expense Tracking system, developed for the COMP1786 Mobile Application Design and Development module at the University of Greenwich.

This application serves as the **User portal**, allowing team members to view available projects, search by specific criteria, and submit expense claims. It is designed to work alongside a native Android (Admin) application by utilizing a shared **Firebase Realtime Database** for seamless, cross-platform data synchronization.

---

## System Requirements & Prerequisites

To compile and run this hybrid application, ensure your development environment is configured with the following:

* **IDE:** Visual Studio 2022 (v17.3 or higher)
* **Workloads:** The **".NET Multi-platform App UI development"** workload must be installed via the Visual Studio Installer.
* **Framework:** .NET 8.0 (or .NET 7.0 depending on your local SDK configuration)
* **Packages:** `FirebaseDatabase.net` and `Newtonsoft.Json` (handled via NuGet).
* **Device:** Android Emulator configured within Visual Studio or a connected physical device.

---

## Compilation, Installation, and Running Instructions

Running the Application in Visual Studio
Open Visual Studio 2022.

Select Open a project or solution and navigate to the cloned directory.

Open the ExpenseTrackerHybrid.slnx (or the main .csproj file).

Restore Packages: Right-click the Solution in the Solution Explorer and select Restore NuGet Packages to ensure the Firebase client libraries and dependencies are properly loaded.

Select Target Device: In the top toolbar run dropdown, select Android Emulators and choose your configured device (e.g., Pixel 5 - API 33).

Compile and Run: Click the Play / Run button (or press F5) to build, deploy, and launch the hybrid application on the emulator.

### 1. Cloning the Repository
Clone this repository to your local machine using Git:
```bash
git clone [https://github.com/Voanh4869/COMP1786-Mobile-Application-Design-and-Development-Hybrid-Android-App.git](https://github.com/Voanh4869/COMP1786-Mobile-Application-Design-and-Development-Hybrid-Android-App.git)
