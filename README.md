# SolviaDynamics365AddressManager

## Overview

**SolviaDynamics365AddressManager** is a Blazor Server web application that integrates with Microsoft Dynamics 365 Sales to perform basic CRUD (Create, Read, Update, Delete) operations on **Accounts** and **Contacts**. This project demonstrates how to interact with the Dynamics 365 Web API using HTTP requests in C# and how to build a modern user interface with Blazor.

The application uses the Microsoft Dynamics 365 Web API to:
- List all **Accounts** and **Contacts**
- Create, Read, Update, and Delete **Accounts** and **Contacts**

## Key Features

- **Blazor Server Application**: The app is built using Blazor Server, providing a modern, component-based UI framework with real-time interactivity.
- **Microsoft Dynamics 365 Integration**: The app connects to Microsoft Dynamics 365 Sales using OAuth authentication and interacts with the Dynamics Web API.
- **CRUD Operations**: The application allows users to view, create, update, and delete accounts and contacts stored in the Dynamics 365 Sales database.
- **Strongly Typed Models**: JSON responses from the Web API are deserialized into C# models for better type safety and easier data manipulation.

## Technologies Used

- **Blazor Server**: Frontend framework for building the user interface.
- **C#**: The primary programming language for both the Blazor components and the service layer interacting with Dynamics 365.
- **Microsoft Dynamics 365 Web API**: Used to interact with the Dynamics 365 Sales data.
- **MSAL (Microsoft Authentication Library)**: For authentication and acquiring access tokens from Azure Active Directory.
- **Newtonsoft.Json**: For serializing and deserializing JSON data from the Dynamics 365 API.

## Setup and Configuration

### Prerequisites

- .NET 6 SDK (or later)
- Dynamics 365 Sales instance with API access
- Azure Active Directory Application (App registration) for OAuth authentication
- Dynamics 365 API Permissions configured in Azure AD
- Client ID and Secret from the Azure AD App Registration

### Configuration

Before running the application, update the following configurations in the `appsettings.json` or directly in the code:

- **ApiBaseUrl**: The URL of your Dynamics 365 instance, e.g., `https://your_org.crm.dynamics.com`
- **ApiVersion**: The version of the Dynamics 365 Web API, e.g., `v9.2`
- **ClientId**: The Azure AD Application (App Registration) client ID
- **ClientSecret**: The secret generated from the Azure AD App Registration
- **TenantId**: The tenant ID of your Azure AD instance
- **RedirectUri**: The redirect URI configured for the Azure AD app (use `http://localhost` during development)

Example configuration in `appsettings.json`:
```json
{
  "DynamicsApi": {
    "ApiBaseUrl": "https://your_org.crm.dynamics.com",
    "ApiVersion": "v9.2",
    "ClientId": "your_client_id",
    "ClientSecret": "your_client_secret",
    "TenantId": "your_tenant_id",
    "RedirectUri": "http://localhost"
  }
}
```

### Authentication

The application uses OAuth to authenticate with Dynamics 365 via Azure Active Directory. Follow these steps to set up the authentication:

1. **Azure App Registration**: Go to Azure Active Directory -> App Registrations -> New Registration.
2. **API Permissions**: Grant API permissions to Dynamics CRM, specifically the `user_impersonation` scope.
3. **Create Client Secret**: Generate a client secret for the registered application.
4. **Set Redirect URI**: Add a Redirect URI for your app, e.g., `http://localhost`.

### Running the Application

1. Clone the repository:
   ```bash
   git clone https://github.com/yourusername/SolviaDynamics365AddressManager.git
   cd SolviaDynamics365AddressManager
   ```

2. Restore NuGet packages:
   ```bash
   dotnet restore
   ```

3. Update the necessary configuration fields (as mentioned above).

4. Run the application:
   ```bash
   dotnet run
   ```

5. Navigate to `http://localhost:5000` in your browser to interact with the app.

## Application Structure

- **Services/DynamicsService.cs**: Contains all the logic to interact with the Dynamics 365 Web API, including authentication and CRUD operations for Accounts and Contacts.
- **Pages/Accounts.razor**: Displays a list of all Accounts and Contacts from Dynamics 365.
- **Pages/CreateAccount.razor**: A form to create new accounts in Dynamics 365.
- **Shared/MainLayout.razor**: Contains the overall structure of the web app, including navigation.

## CRUD Operations

### 1. Listing Accounts and Contacts

- The `Accounts.razor` page makes a call to the `DynamicsService` to fetch all accounts and contacts from Dynamics 365. The results are displayed in a list.

```csharp
var accounts = await dynamicsService.GetAccountsAsync();
var contacts = await dynamicsService.GetContactsAsync();
```

### 2. Creating a New Account

- The `CreateAccount.razor` page allows users to enter a new account name and send it to Dynamics 365 using the `CreateAccountAsync` method.

```csharp
await dynamicsService.CreateAccountAsync(accountName);
```

### 3. Updating an Account

- The `UpdateAccountAsync` method is used to update the name of an existing account.

```csharp
await dynamicsService.UpdateAccountAsync(accountId, "Updated Account Name");
```

### 4. Deleting an Account

- The `DeleteAccountAsync` method removes an account from Dynamics 365.

```csharp
await dynamicsService.DeleteAccountAsync(accountId);
```

## Future Improvements

- **Pagination**: Add pagination to handle large datasets efficiently.
- **Error Handling**: Enhance error handling with user-friendly error messages.
- **Authentication Improvements**: Securely handle access tokens and refresh them when they expire.
- **UI/UX Improvements**: Enhance the UI with more dynamic feedback, better styling, and accessibility improvements.
- **Test Coverage**: Implement unit and integration tests for critical parts of the app.

## Contribution

Feel free to fork this repository and submit pull requests. Contributions are always welcome!

---

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

---

## Contact

If you have any questions or issues, feel free to open an issue on the repository

