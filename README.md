# Flight Information Display System (FIDS)

## Overview
The **Flight Information Display System (FIDS)** is a console application designed for Changi Airport's Terminal 5. This system displays real-time flight information, manages boarding gates, and handles advanced operations such as processing unassigned flights and calculating airline fees. The application utilizes object-oriented programming principles and features robust error handling.

## Features
### Basic Features
1. **Load Airlines and Boarding Gates**: Automatically load airlines and boarding gates from CSV files.
2. **Load Flights**: Populate flights with their details, including flight number, origin, destination, and special requests.
3. **List Flights and Boarding Gates**: Display flights and gates in a structured table format.
4. **Assign Boarding Gates**: Manually assign a boarding gate to a flight.
5. **Create Flights**: Add new flights with details like origin, destination, and special requests.
6. **Modify Flights**: Update or delete flight information.
7. **Display Flight Schedule**: List all flights in chronological order.

### Advanced Features
1. **Process Unassigned Flights**: Automatically assign boarding gates to flights without a gate based on compatibility.
2. **Calculate Airline Fees**: Generate a fee report for airlines based on flights, boarding gates, and discounts.

## Installation
1. Clone the repository:
   ```bash
   git clone https://github.com/<your-repo-name>
   ```
2. Open the project in your preferred C# IDE (e.g., Visual Studio).
3. Ensure the following files are present in the project directory:
   - `airlines.csv`
   - `boardinggates.csv`
   - `flights.csv`
4. Build and run the application.

## Usage
### Main Menu
- The program starts with a main menu displaying the following options:
  ```
  1. List All Flights
  2. List Boarding Gates
  3. Assign a Boarding Gate to a Flight
  4. Create Flight
  5. Display Airline Flights
  6. Modify Flight Details
  7. Display Flight Schedule
  8. Process Unassigned Flights
  9. Display the Total Fee per Airline for the Day
  0. Exit
  ```

### Example Workflow
1. **Load Data**: The program automatically loads data from the CSV files upon startup.
2. **Assign Gates**: Select a flight and assign it a boarding gate.
3. **Create Flights**: Add new flights as needed.
4. **Process Flights**: Use the advanced feature to assign gates to multiple flights in bulk.
5. **Generate Fee Reports**: Calculate and display the total fees for airlines.

## Class Diagram
The application is structured around the following key classes:
- `Airline`
- `BoardingGate`
- `Flight` (Base class)
  - `CFFTFFlight`
  - `DDJBFlight`
  - `LWTTFlight`
  - `NORMFlight`
- `Terminal`

## File Structure
```
.
├── Airlines.cs       # Handles airline-related logic
├── BoardingGate.cs   # Manages boarding gate properties and assignments
├── CFFTFFlight.cs    # Handles flights with CFFT special requests
├── DDJBFlight.cs     # Handles flights with DDJB special requests
├── Flight.cs         # Base class for flights
├── LWTTFlight.cs     # Handles flights with LWTT special requests
├── NORMFlight.cs     # Handles flights without special requests
├── Program.cs        # Main program logic
├── Terminal.cs       # Coordinates airlines, gates, and flights
├── airlines.csv      # Airline data
├── boardinggates.csv # Boarding gate data
└── flights.csv       # Flight data
```

## Contributing
1. Fork the repository.
2. Create a new branch for your feature:
   ```bash
   git checkout -b feature-name
   ```
3. Commit your changes:
   ```bash
   git commit -m "Add feature-name"
   ```
4. Push to your branch and open a pull request:
   ```bash
   git push origin feature-name
   ```

## Authors
- **Aiden Tan Yihan** (S10267626)
- **Ang Shun Xiang** (S10270399)

## License
This project is licensed under the MIT License.

