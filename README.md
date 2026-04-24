# Project Expense Tracker - COMP1786

## Overview
This repository contains the source code for the Project Expense Tracking system developed for the COMP1786 Mobile Application Design and Development module at the University of Greenwich. 

The system consists of two interconnected applications:
1. **Native Android Application (Admin):** Built with Java, utilizing a local SQLite database for offline administrative data entry and management.
2. **Hybrid Application (User):** Built with .NET MAUI (C#), designed for users to view projects and submit expenses. 

Both applications synchronize their data through a **Firebase Realtime Database**, ensuring seamless cross-platform data consistency.

---

## System Requirements & Prerequisites

To compile and run these applications, ensure your development environment meets the following requirements:

### For the Native Android App (Java)
* **IDE:** Android Studio (Latest version recommended)
* **SDK:** Minimum Android SDK Level 24 (Nougat) / Target SDK Level 34
* **Java:** JDK 17 or higher
* **Device:** Android Emulator or physical Android device enabled for USB Debugging

### For the Hybrid App (.NET MAUI)
* **IDE:** Visual Studio 2022 (v17.3 or higher)
* **Workloads:** The ".NET Multi-platform App UI development" workload must be installed
* **Framework:** .NET 8.0 (or .NET 7.0 depending on local configuration)
* **Device:** Android Emulator configured within Visual Studio


### Running the Hybrid .NET MAUI Application (User)
Open Visual Studio 2022.

Select Open a project or solution and navigate to the ExpenseTrackerHybrid folder.

Open the ExpenseTrackerHybrid.slnx (or .csproj) file.

Right-click the Solution in the Solution Explorer and select Restore NuGet Packages to ensure the Firebase Client library (FirebaseDatabase.net) is loaded.

In the top toolbar run dropdown, select Android Emulators and choose your configured device (e.g., Pixel 5 - API 33).

Click the Play / Run button (F5) to build, deploy, and launch the hybrid application.
---

## Compilation, Installation, and Running Instructions

### 1. Cloning the Repository
First, clone this repository to your local machine:
```bash
git clone [https://github.com/Voanh4869/COMP1786-Mobile-Application-Design-and-Development-Hybrid-Android-App.git](https://github.com/Voanh4869/COMP1786-Mobile-Application-Design-and-Development-Hybrid-Android-App.git)


