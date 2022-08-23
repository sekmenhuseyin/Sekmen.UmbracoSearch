# Sekmen.UmbracoSearch

In this exercise, we will start by creating the foundation for our search functionality. Then, the search logic will be added in a custom service, and we will use a controller to handle and control incoming requests for content pages based on a specific Document Type.

The end goal of this approach is to enrich the view model passed to the template with additional properties before having everything available in the view. In this way, our view will not contain any custom code for our search functionality

## Requirements

- Visual Studio

You will need Visual Studio to participate. I recommend that you use VS 2022, which is the latest and fastest option. VS 2019 is also okay to go with.

- .NET Core

Make sure you install .NET Core (.NET 6 ) from https://dotnet.microsoft.com/en-us/download .

- Project setup

1. Follow the instructions here to create a new blank Umbraco project locally: https://our.umbraco.com/documentation/Fundamentals/Setup/Install/install-umbraco-with-templates
2. Run the project and do the initial setup 
3. Install UmbTrainingSite.Package from nuGet. The package contains the necessary files for most of our courses
4. Run the site - on the frontend, you should see a website with sample content.

- Exercise Files
Web: https://training.umbraco.com/resources
Password: TakeMeToSearch2556
PDF: /Examine___TakeMeToSearch2556.pdf
