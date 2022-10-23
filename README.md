# QA Task Tracker
This program was written for a specific work environment. All references to that company have been removed from the UI and from the code.

*Notes: please note that I wrote this program several years ago (2017-2018) and so the code is not as elegant as some more recent coding projects. In addition, this program was meant to be used within the QA department of a specific company. As such, the UI is quite minimal and simple, since it was not meant to be used by external users that would be unfamiliar with its purpose or process.*

Purposes of this program:
1. To give an at-a-glance status of status of all current tasks using colour-coding.
2. To keep track of each steps required for each version that a specific task/bug/issue would be released in.

## General Information
This is a desktop program written in C# and WPF (XAML) using a SQLite local database. It incorporates Material Design UI libraries.

Each task has the following fields:
- Project
- TFS Number
- Task
- Status
- Priority
- Iteration
- Ticket
- Customer
- Description
- User Story
- Dev Task
- Developer
- Notes

In addition, each task can have one or more versions that must be tested. Each version has the following fields:
- Version
- Status
- Notes
- Tested
- Patched
- Email Sent
- Test Case
- Automated UI Test
- Release Tasks
- Release Notes
- User Guide
- Parameters List
- Sample Report
- Internal Docs
- Install Instructions
- TFS Updated
- Ticket Updated

## Database
The database used for this project is SQLite. This format was chosen because it was a local database that did not require much setup.

The database contains 3 tables:
1. TaskHeader: this table contains the main task information.
2. TaskVersionDetail: this table contains multiple rows per task. Each row stores the status and steps for one version.
3. AppParameters: this table contains the application parameters.

A sample database is provided in the [Resources/Samples](/QATaskTracker/Resources/Samples/) directory.

## Main Screen
The main screen of the application is separated in two main parts: a top grid that displays selected information from all the tasks, and the bottom section displays the version information for the selected task in the grid.

![Main Screen Screenshot](/QATaskTracker/Resources/Images/Main%20Screen.png)

### Top Grid
The user can select which columns display in the top grid in the [Parameters](#parameters-screen) screen. All columns can be sorted by clicking on their header. Double-clicking on a task will open the [Edit Task](#edit-task) screen.

### Bottom Section
The user can update the status for each version in the bottom section and the background colour of the version number updates automatically based on the status selected. They can also check/uncheck the boxes for the required steps to indicate whether it has been completed.

## Add New Task
The '+' button in the bottom-right corner of the main screen allows the user to create a new task.

![Add New Task Screenshot](/QATaskTracker/Resources/Images/Add%20New%20Task.png)

The top section allows the user to enter the task information.

The bottom section of this screen will display each version defined (in the parameters) for the project selected. Checking the box next to a version indicates that this task must be tested in that version. 

Within each version, the list of all available steps is displayed. Checking a step indicates that this step is required for that version. The program automatically selects default tasks per version based on the 'Released', 'Beta', 'Dev' versions set in the parameters for that project, but they can be modified.

## Edit Task
Double-clicking a task in the grid on the main screen opens the edit task screen where the user can modify all fields related to that task except the project and TFS number.

![Edit Task Screenshot](/QATaskTracker/Resources/Images/Edit%20Task.png)

## Parameters Screen
The parameters screen displays various parameters for the user to set.

![Edit Task Screenshot](/QATaskTracker/Resources/Images/Parameters%20Screen.png)

### General Parameters
This is where the default project can be set. This determines which project will be selected by default when creating a new task.

### Project Green & Magenta Parameters
This is where the available versions for Project Green and Project Magenta can be set. They should be set in order from lowest (top of the list) to highest (bottom). 

The 'Released', 'Beta' and 'Dev' parameters determine which steps will be selected by default for each version when creating a new task.

The 'Iteration' parameter determines the default value of the iteration field when creating a new task.

### UI Colours
This is where the user can define the colours displayed in the main grid for each task and for each version based on their current status. Clicking on the colour square opens the Windows Color Picker.

### Grid Parameters
This is where the user can select which columns will be displayed in the grid on the main screen.
