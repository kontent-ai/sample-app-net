# Kentico Cloud sample .NET MVC web application

This is a sample website written in ASP.NET MVC 5 that uses the [Kentico Cloud Delivery .NET SDK](https://github.com/Kentico/delivery-sdk-net) to manage and retrieve content from Kentico Cloud, and the [Kentico Cloud Personalization .NET SDK](https://github.com/Kentico/personalization-sdk-net) to track site visits.

You can register your account for free at <https://app.kenticocloud.com>.

## Application setup

We recommend running the sample application in Visual Studio 2013 or later. By default, the application uses the default Kentico Cloud Sample project to serve content.

To run the aplication:

1. Clone the sample application repository.
2. Open the solution in Visual Studio (using the _DancingGoat.sln_ file).
3. Run the application.

The sample application opens in your browser.

## Connect your project

If you already have a Kentico Cloud account, you can connect this sample application to a project of your own to access its unpublished content items, and track visitors on the site. For example, you can connect the application to your version of the Sample project. To do this, you will need to modify the application's `web.config` file.

### Previewing unpublished content

To preview content in the sample application, follow these steps:

1. In Kentico Cloud, select your project.
2. Navigate to the Development section.
3. Copy your Project ID and Preview API key.
4. Open the sample application's `Web.config` file.
5. Insert the copied Project ID to the value of the `ProjectId` application setting.
6. Create a new application setting named `PreviewToken` in the `<appSettings>` section.
7. Use the copied Preview API key as the setting's value.
8. Save the changes.

When you now run the application, you will see all project content including the unpublished version of content items. If you want to know more about using the `DeliveryClient`, see the [Delivery .NET SDK documentation](https://github.com/Kentico/delivery-sdk-net#using-the-deliveryclient).

### Enabling personalization

To enable personalization in the sample application, follow these steps:

1. In Kentico Cloud, select your project.
2. Navigate to the Development section.
3. Copy your Project ID and Personalization API key.
4. Open the sample application's `Web.config` file.
5. Insert the copied Project ID to the value of the ProjectId application setting.
6. Create a new application setting named `PersonalizationToken` in the `<appSettings>` section.
7. Use the copied Personalization API key as the setting's value.
8. Save the changes.

When you now run the application, Kentico Cloud will track the site visits and create new contacts when visitors submit a form. Fore more information about using the `PersonalizationClient`, see the [Personalization .NET SDK documentation](https://github.com/Kentico/personalization-sdk-net#basic-scenarios).

## Content administration

1. Navigate to <https://app.kenticocloud.com> in your browser.
2. Sign in with your credentials.
3. Manage content in the content administration interface of your sample project.

You can learn more about content editing with Kentico Cloud in our [Help Center](http://help.kenticocloud.com/).

## Content delivery

You can retrieve content either through the Kentico Cloud Delivery SDK or the Kentico Cloud Delivery API:

* For published content, use `https://deliver.kenticocloud.com/PROJECT_ID/items`.
* For unpublished content, use `https://preview-deliver.kenticocloud.com/PROJECT_ID/items`.

For more details about Kentico Cloud APIs, see our [API reference](https://developer.kenticocloud.com/reference).