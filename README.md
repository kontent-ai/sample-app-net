# Kentico Cloud sample .NET MVC web application

This is a sample website written in ASP.NET MVC 5 utilizing the [Kentico Cloud Delivery .NET SDK](https://github.com/Kentico/delivery-sdk-net) to manage and retrieve content from Kentico Cloud. You can register your account for free at <https://app.kenticocloud.com>.

## Application setup

We recommend running the sample application in Visual Studio 2013 or later. By default, the application uses the default Kentico Cloud sample project for its content.

To try the aplication:

1. Clone the sample application repository.
2. Open the solution in Visual Studio (using the _DancingGoat.sln_ file).
3. Run the application.
4. The sample application opens in your browser.

## Preview content from your project

If you already have a Kentico Cloud account and you want to connect the sample application to a project of your own, you need to provide your Project ID and Preview API key to authorize requests to the Delivery Preview API. For example, you can connect the application to your modified version of the sample project.

To preview content in the sample application, follow these steps:

1. In Kentico Cloud, select your project.
2. Navigate to the Development section.
3. Copy your Project ID and Preview API key.
4. Open the sample application's `Web.config` file.
5. Insert the copied Project ID to the value of the `ProjectId` application setting.
6. Create a new application setting named `PreviewToken` in the `<appSettings>` section.
7. Use the copied Preview API key as the setting's value.
8. Save the changes.
9. Run the application.

When you now run the application, you will see all project content including the unpublished version of content items.

## Content administration

1. Navigate to <https://app.kenticocloud.com> in your browser.
2. Sign in with your credentials.
3. Manage content in the content administration interface of your sample project.

You can learn more about content editing with Kentico Cloud in the [user documentation](http://help.kenticocloud.com/).

## Content delivery

You can retrieve content either through the Kentico Cloud Delivery SDKs or the Kentico Cloud Delivery API:

* For published content, use `https://deliver.kenticocloud.com/PROJECT_ID/items`.
* For unpublished content, use `https://preview-deliver.kenticocloud.com/PROJECT_ID/items`.

For more info about the API, see the [API reference](https://developer.kenticocloud.com/reference#delivery-api).

You can find more information about the Kentico Cloud Delivery .NET SDK at <https://github.com/Kentico/delivery-sdk-net>. To use the SDK in your projects, include it as a [NuGet package] (https://www.nuget.org/packages/KenticoCloud.Delivery).