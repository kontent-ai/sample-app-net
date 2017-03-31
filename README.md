# Kentico Cloud sample .NET MVC web application

This is a sample website written in ASP.NET MVC 5 that uses the [Kentico Cloud Delivery .NET SDK](https://github.com/Kentico/delivery-sdk-net) to manage and retrieve content from Kentico Cloud, and the [Kentico Cloud Personalization .NET SDK](https://github.com/Kentico/personalization-sdk-net) to track site visits.

You can register your account for free at <https://app.kenticocloud.com>.

## Application setup

We recommend running the sample application in Visual Studio 2013 or later. By default, the application uses the default Kentico Cloud Sample project to serve content.

To run the aplication:

1. Clone the sample application repository.
1. Open the solution in Visual Studio (using the _DancingGoat.sln_ file).
1. Run the application.

The sample application will open in your browser.

## Connect your project

If you already have a Kentico Cloud account, you can connect this sample application to a project of your own to access its unpublished content items, and track visitors on the site. For example, you can connect the application to your version of the Sample project.

1. Select your project in Kentico Cloud.
1. Navigate to the **API keys** section.

    * You will be copying the Project ID and API keys for the Delivery Preview API and Personalization API.

1. Open the `\DancingGoat\Web.config` file.
1. Use the values from your Kentico Cloud project in the `Web.config` file:

    * **Project ID**: Insert your project ID into the `ProjectId` application setting.
    * **Delivery Preview API**: Create a new application setting named `PreviewToken` in the `<appSettings>` section, and use the Delivery Preview API key as its value.
    * **Personalization API**: Create a new application setting named `PersonalizationToken` in the `<appSettings>` section, and use the Personalization API key as its value.

    ```xml
    <appSettings>
        ...
        <add key="ProjectId" value="YOUR_PROJECT_ID" />
        <add key="PreviewToken" value="YOUR_DELIVERY_PREVIEW_API_KEY" />
        <add key="PersonalizationToken" value="YOUR_PERSONALIZATION_API_KEY" />
        ...
    </appSettings>
    ```

1. Save the changes.
1. Run the application.

After you run the application, Kentico Cloud will track site visits and create new contacts when visitors submit a form. You will also be able to see all project content including the unpublished version of content items.

For more information about the integrations with the Delivery API and Personalization API, see the following:

* [Delivery .NET SDK documentation](https://github.com/Kentico/delivery-sdk-net#using-the-deliveryclient) on using the `DeliveryClient`
* [Personalization .NET SDK documentation](https://github.com/Kentico/personalization-sdk-net#basic-scenarios) on using the `PersonalizationClient`

## Content administration

1. Navigate to <https://app.kenticocloud.com> in your browser.
1. Sign in with your credentials.
1. Manage content in the content administration interface of your sample project.

You can learn more about content editing with Kentico Cloud in our [Help Center](http://help.kenticocloud.com/).

## Content delivery

You can retrieve content either through the Kentico Cloud Delivery SDK or the Kentico Cloud Delivery API:

* For published content, use `https://deliver.kenticocloud.com/PROJECT_ID/items`.
* For unpublished content, use `https://preview-deliver.kenticocloud.com/PROJECT_ID/items`.

For more details about Kentico Cloud APIs, see our [API reference](https://developer.kenticocloud.com/reference).