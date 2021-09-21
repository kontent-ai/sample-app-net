# Kontent sample ASP.NET Core MVC web application
[![Build & Test](https://github.com/Kentico/kontent-sample-app-net/actions/workflows/integrate.yml/badge.svg)](https://github.com/Kentico/kontent-sample-app-net/actions/workflows/integrate.yml)
[![codecov](https://codecov.io/gh/Kentico/kontent-sample-app-net/branch/master/graph/badge.svg?token=hj8JmDzLjJ)](https://codecov.io/gh/Kentico/kontent-sample-app-net)
[![Stack Overflow](https://img.shields.io/badge/Stack%20Overflow-ASK%20NOW-FE7A16.svg?logo=stackoverflow&logoColor=white)](https://stackoverflow.com/tags/kentico-kontent)
[![Discord](https://img.shields.io/discord/821885171984891914?label=Discord&logo=Discord&logoColor=white)](https://discord.gg/SKCxwPtevJ)
[![Deploy to Azure](https://aka.ms/deploytoazurebutton)](https://azuredeploy.net/)

This is a sample ASP.NET Core MVC app that uses the [Kontent Delivery .NET SDK](https://github.com/Kentico/kontent-delivery-sdk-net) to retrieve content from [Kontent by Kentico](https://kontent.ai).


## Getting started

To run the app:
1. Clone the app repository with your favorite GIT client
1. Run `npm install && npm run build` in the `DancingGoat` directory ([node.js](https://nodejs.org/) must be installed before running this command)
1. Open the `DancingGoat.sln` solution file in VS or VS Code
1. Run the app
1. Follow the setup wizard or
1. Alternatively, adjust the `\DancingGoat\appsettings.json` file:

    ```json
	"DeliveryOptions": {
		"ProjectId": "YOUR_PROJECT_ID",
	},
    ```

## Tutorial
Follow the [step-by-step tutorial](https://docs.kontent.ai/tutorials/develop-apps/get-started/run-sample-app?tech=dotnet) for even more details.

## Features

### Edit mode & preview

Content contributors sometimes need to fix errors or typos right when they see them on the website. The sample app allows users to navigate from a piece of content on the site straight to the corresponding content item or element in Kontent. 

To see Edit mode in action:

1. Enable Delivery Preview API by adding the following keys to the `\DancingGoat\appsettings.json` file:

    ```json
	"DeliveryOptions": {
		"UsePreviewApi": true,
		"PreviewApiKey": "YOUR_DELIVERY_PREVIEW_API_KEY"
	},
    ```
	* **Delivery Preview API**: Create a new key named `PreviewApiKey` in the `DeliveryOptions` section, and use the Delivery Preview API key as its value. To enable calls over the Delivery Preview API, you also need to add a key named `UsePreviewApi` and set it to `true`.
2. Run the app.
3. Navigate to the **About us** section.
4. Click the **Edit mode** switch in the bottom-left corner.

Edit buttons will appear next to each piece of content on the page.

To explore how the functionality is implemented, navigate to the [`TagHelpers`](https://github.com/Kentico/kontent-sample-app-net/tree/master/DancingGoat/TagHelpers) folder.

### Responsive images

The sample app contains a sample implementation of the `img-asset` tag helper from the [Kentico.Kontent.AspNetCore](https://www.nuget.org/packages/Kentico.Kontent.AspNetCore) NuGet package. Using the `img-asset` tag helper, you can easily create an `img` tag with `srcset` and `sizes` attributes. Read more about image transformation API in the [docs](https://docs.kontent.ai/reference/image-transformation).
You can adjust the behavir in the `appsettings.json` file.

```json
"ImageTransformationOptions": {
  "ResponsiveWidths": [ 200, 400, 600, 800, 1000, 1200, 1400, 1600, 2000, 4000 ]
},
```

### Localized routing
The app demonstrates the usage of the [`Kentico.AspNetCore.LocalizedRouting`](https://www.nuget.org/packages/Kentico.AspNetCore.LocalizedRouting) NuGet package for localizing URLs for SEO purposes.

## Get involved

Check out the [contributing](CONTRIBUTING.md) page to see the best places to file issues, start discussions, and begin contributing.

