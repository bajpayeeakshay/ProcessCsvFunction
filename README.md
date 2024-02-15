# ProcessCsvFunction

ProcessCsvFunction is an Azure Functions project designed to process CSV files, enrich the data within the CSV with additional information from the GLEIF (Global Legal Entity Identifier Foundation) API, and store the enriched data in Azure Table Storage.

## Getting Started

### Prerequisites
Ensure the following prerequisites are met before setting up the project:

- .NET Core SDK
- Azure Storage account for Blob and Table Storage
- GLEIF API key (if not using a public endpoint)

### Setting up Local Environment

#### Install NPM
1. Install NPM from the official website.

#### Install and Run Azurite
1. Install Azurite for local Azure Storage emulation.

#### Install Storage Explorer
1. Install Azure Storage Explorer.

#### Get Azure Storage Connection String
1. Run Storage Explorer.
2. Expand the Storage Accounts.
3. Two options will be visible: 
    a. (Attached Containers)
    b. (Emulator - Default Ports) (Key)
4. Click on (Emulator - Default Ports) (Key) and open the Properties Tab.
5. Click on the View Icon for the Primary Connection String and copy it.

#### Paste Connection String in local.settings.json
1. Open `local.settings.json` in the root of the project.
2. Replace the placeholder for `"AZURE_STORAGE_CONNECTION_STRING"` with the copied Primary Connection String.

#### Create Blob Container
1. In Storage Explorer:
   - Expand Storage Accounts.
   - Expand (Emulator - Default Ports) (Key).
   - Right-click on Blob Containers and create a new Blob Container named "process-csv-function-blob".

### Running the Blob Trigger Application

1. Run the application.
2. Open Azure Storage Explorer.
3. Expand Storage Accounts > (Emulator - Default Ports) (Key) > Blob Containers > process-csv-function-blob.
4. Click the UPLOAD button on the right side and upload a CSV file into the folder.
5. The CSV file will be processed and saved in the Azure Table Storage with the table name "EnrichedDataSet".

### Running the HTTP Trigger

1. Run the application.
2. Locate the URL on which the HTTP Trigger is being run.
3. Copy the URL.
4. Open Postman.
5. Paste the URL in Postman.
6. Set the HTTP method to GET.
7. Add a query parameter "fileName" and provide the name of the uploaded CSV file.
8. Click Send.
   - This will trigger the HTTP Trigger function, and you'll receive the enriched data as a response.

## Project Overview
The project follows a modular structure, separating concerns into different folders. Key components include:

### 1. **Startup.cs**
This file is part of the Azure Functions startup process. It configures services required for dependency injection, sets up logging using Serilog, and configures the HTTP client for interacting with the GLEIF API.

### 2. **ProcessCsvBlobTrigger.cs**
This class defines a Blob Trigger function that is triggered whenever a new CSV file is uploaded to the specified Azure Blob Storage container. It uses the `IProcessEnrichDataService` service to enrich the CSV data.

### 3. **EnrichedDataController.cs**
This class defines an HTTP-triggered function that allows users to retrieve enriched data by providing the file name as a query parameter.

### 4. **local.settings.json**
Configuration file for local development. It contains settings such as Azure Storage connection strings, blob storage name, table storage name, and GLEIF API URL.

### 5. **Services**
The `Services` folder contains classes responsible for interacting with external services, mainly the GLEIF API.

### 6. **Data**
The `Data` folder contains interfaces and classes related to data storage and retrieval, including Azure Table Storage and data models.

### 7. **Tests**
The `Services.Tests` folder contains NUnit tests for the `ProcessEnrichDataService` class.

## Project Structure
The modular structure of the project ensures a clear separation of concerns, making it scalable and maintainable. Key folders include:

- **Services:** Contains service classes responsible for processing and enriching CSV data.
- **Data:** Contains interfaces and classes for data storage and retrieval.

## Functionality
1. **Blob Trigger Function (`ProcessCsvBlobTrigger`):**
   - Triggered when a new CSV file is uploaded to the specified Azure Blob Storage container.
   - Enriches the CSV data using the `IProcessEnrichDataService` service.

2. **HTTP Trigger Function (`EnrichedDataController`):**
   - Allows users to retrieve enriched data by providing the file name as a query parameter.

3. **Enrichment Service (`ProcessEnrichDataService`):**
   - Enriches CSV data with GLEIF values, including legal names, BIC codes, and transaction costs.
   - Validates CSV headers and data integrity.
