# Kentico Cloud sample .NET MVC web application
[![Build status](https://ci.appveyor.com/api/projects/status/3b9v2fl52v4aiptk/branch/master?svg=true)](https://ci.appveyor.com/project/kentico/cloud-sample-app-net/branch/master)
[![Stack Overflow](https://img.shields.io/badge/Stack%20Overflow-ASK%20NOW-FE7A16.svg?logo=stackoverflow&logoColor=white)](https://stackoverflow.com/tags/kentico-kontent)

This is a sample website written in ASP.NET MVC 5 that uses the [Kentico Cloud Delivery .NET SDK](https://github.com/Kentico/delivery-sdk-net) to manage and retrieve content from Kentico Cloud. For a brief walkthrough, read [Running the .NET sample app](https://docs.kontent.ai/tutorials/develop-apps/get-started/running-a-sample-application?tech=dotnet) on our Developer Hub.

You can register your account for free at <https://app.kontent.ai>.

## Application setup

You can run the app in two following ways:

* By cloning the code and running it via Visual Studio 2013 or later
* By deploying the app to a new Azure App Service instance in your Azure subscription.

### Running via Visual Studio

To run the app:
1. Clone the app repository with your favorite GIT client
   1. For instance, you can use [Visual Studio](https://www.visualstudio.com/vs/), [Visual Studio Code](https://code.visualstudio.com/), [GitHub Desktop](https://desktop.github.com/), etc.
   1. Alternatively, you can download the repo as a ZIP file, however, this will not adapt line endings in downloaded files to your platform (Windows, Unix).
1. Open the solution in Visual Studio (using the _DancingGoat.sln_ file).
1. Run the app.

### Running in Azure

To run the app, just click the below "Deploy to Azure" button.

[![Deploy to Azure](http://azuredeploy.net/deploybutton.png)](https://azuredeploy.net/)

### Connecting to your sample project

At the first run of the app, you'll be presented with a configuration page. It will allow you to connect the app to your Kentico Cloud project or create a new one. You'll also be able to start a trial and convert to a free plan when the trial expires.

Alternatively, you can connect your project manually as per the chapter below.

#### Connecting to your project manually

If you already have a Kentico Cloud account, you can connect this sample application to a project of your own to access its unpublished content items, and track visitors on the site. For example, you can connect the application to your version of the Sample project.

1. In Kentico Cloud, choose Project settings from the app menu.
1. Under Development, choose API keys.

    * You will be copying the Project ID and API key for the Delivery Preview API.

1. Open the `\DancingGoat\Web.config` file.
1. Use the values from your Kentico Cloud project in the `Web.config` file:

    * **Project ID**: Insert your project ID into the `ProjectId` application setting.
    * **Delivery Preview API**: Create a new application setting named `PreviewApiKey` in the `<appSettings>` section, and use the Delivery Preview API key as its value. To enable calls over the Delivery Preview API, you also need to add a setting named `UsePreviewApi` and set it to `true`.

    ```xml
    <appSettings>
        ...
        <add key="ProjectId" value="YOUR_PROJECT_ID" />
        <add key="UsePreviewApi" value="true"/>
        <add key="PreviewApiKey" value="YOUR_DELIVERY_PREVIEW_API_KEY" />
        ...
    </appSettings>
    ```

1. Save the changes.
1. Run the application.

After you run the application, Kentico Cloud will track site visits and create new contacts when visitors submit a form. You will also be able to see all project content including the unpublished version of content items.

For more information about the integrations with the Delivery API, see the following:

* [Delivery .NET SDK documentation](https://github.com/Kentico/delivery-sdk-net#using-the-deliveryclient) on using the `DeliveryClient`

## Content administration

1. Navigate to <https://app.kontent.ai> in your browser.
1. Sign in with your credentials.
1. Manage content in the content administration interface of your sample project.

You can learn more about content editing with Kentico Cloud in our [Help Center](https://docs.kontent.ai/).

## Content delivery

You can retrieve content either through the Kentico Cloud Delivery SDK or the Kentico Cloud Delivery API:

* For published content, use `https://deliver.kontent.ai/PROJECT_ID/items`.
* For unpublished content, use `https://preview-deliver.kontent.ai/PROJECT_ID/items`.

For more details about Kentico Cloud APIs, see our [API reference](https://docs.kontent.ai/reference/kentico-kontent-apis-overview).
For details on how the preview functionality works in this app, see the [wiki](https://github.com/Kentico/kontent-sample-app-net/wiki/Preview-URLs-explained).

## Edit mode

Content contributors sometimes need to fix errors or typos right when they see them on the website. The sample app allows users to navigate from a piece of content on the site straight to the corresponding content item or element in Kentico Cloud. 

To see Edit mode in action:

1. Enable Delivery Preview API by adding the following keys to the `\DancingGoat\Web.config` file:
```xml
        <add key="ProjectId" value="YOUR_PROJECT_ID" />
        <add key="UsePreviewApi" value="true"/>
        <add key="PreviewApiKey" value="YOUR_DELIVERY_PREVIEW_API_KEY" />
```
2. Run the app.
3. Navigate to the **About us** section.
4. Click the **Edit mode** switch in the bottom-left corner.

Edit buttons will appear next to each piece of content on the page.

## Adjustable images

The sample website uses adjustable images via the [image transformation](https://docs.kontent.ai/reference/image-transformation) feature available in the Delivery API. The `srcset` attribute is automatically added to the `img` tag. The value of the attribute is then driven by the **ResponsiveWidths** web.config setting. You can always disable this behavior by deleting the setting from the `\DancingGoat\Web.config` file.

## Troubleshooting

Kentico Cloud evolves over time. If you connect your sample app to an older Kentico Cloud sample project, the app may not run correctly. You can always generate the latest version of the sample Dancing Goat content project at https://app.kontent.ai/sample-project-generator . Once generated, you can either paste the new project ID to web.config, or, you can navigate to your app's relative URL "/Admin/SelfConfig" and pick the new project.

## Feedback & Contributing

Check out the [contributing](https://github.com/Kentico/delivery-sdk-net/blob/master/CONTRIBUTING.md) page to see the best places to file issues, start discussions, and begin contributing.

### Wall of Fame
We would like to express our thanks to the following people who contributed and made the project possible:

- [Steve Fenton](https://github.com/Steve-Fenton)

Would you like to become a hero too? Pick an [issue](https://github.com/Kentico/kontent-sample-app-net/issues) and send us a pull request!

![Analytics](https://kentico-ga-beacon.azurewebsites.net/api/UA-69014260-4/Kentico/kontent-sample-app-net?pixel)
