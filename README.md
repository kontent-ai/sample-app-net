# Kentico Cloud sample .NET MVC web application

Dancing Goat is a sample website utilizing the Kentico Cloud Delivery SDK to manage and retrieve content.

## Prerequisites

1. Environment that is capable of running the .NET MVC 5 application such as Visual Studio 2013+.
2. A Kentico Cloud account. You can register at https://app.kenticocloud.com.


## Content administration

1. Navigate to https://app.kenticocloud.com in your browser.
2. Sign in with your credentials.
3. Manage content in the content administration interface of your sample project.

You can learn more about content editing with Kentico Cloud in the [documentation](http://help.kenticocloud.com/).

## Application setup

1. In Kentico Cloud, navigate to the Development section and copy the ID of your project.
2. Open the application's `Web.config` file.
3. Insert the Project ID to the value of the `ProjectId` application setting.
4. Save the changes.
5. Run the application.

## Preview content
For previewing content that is not yet published, you need to provide the Preview API key to the application.

1. In Kentico Cloud, navigate to the Development section and copy the Preview API key for the Delivery API.
2. Open the application's `Web.config` file.
3. Create a new application setting named `PreviewToken` in the `<appSettings>` section.
4. Use the Preview API key as the setting's value.
5. Save the changes.
6. Run the application.

## Content delivery

You can retrieve content either through the Kentico Cloud Delivery SDKs or the Kentico Cloud Delivery API:
* For published content, use `https://deliver.kenticocloud.com/PROJECT_ID/items`.
* For unpublished content, use `https://preview-deliver.kenticocloud.com/PROJECT_ID/items`.

For more info about the API, see the [API reference](http://docs.kenticodeliver.apiary.io).

You can find more information about the Kentico Cloud Delivery .NET SDK at https://github.com/Kentico/delivery-sdk-net. To use the SDK in your projects, include it as a NuGet package (https://www.nuget.org/packages/KenticoCloud.Delivery).