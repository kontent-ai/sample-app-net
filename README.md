# Kontent.ai sample ASP.NET Core MVC web application

[![Build & Test](https://github.com/kontent-ai/sample-app-net/actions/workflows/integrate.yml/badge.svg)](https://github.com/kontent-ai/sample-app-net/actions/workflows/integrate.yml)
[![codecov](https://codecov.io/gh/kontent-ai/sample-app-net/branch/master/graph/badge.svg?token=X90Anf22sl)](https://codecov.io/gh/kontent-ai/sample-app-net)
[![Stack Overflow](https://img.shields.io/badge/Stack%20Overflow-ASK%20NOW-FE7A16.svg?logo=stackoverflow&logoColor=white)](https://stackoverflow.com/tags/kontent)
[![Discord](https://img.shields.io/discord/821885171984891914?label=Discord&logo=Discord&logoColor=white)](https://discord.gg/SKCxwPtevJ)

This is a sample ASP.NET Core MVC app that uses the [Kontent.ai Delivery .NET SDK](https://github.com/kontent-ai/delivery-sdk-net) to retrieve content from [Kontent.ai](https://kontent.ai).

## Getting started

To run the app:

1. Clone the app repository
2. Run `npm install && npm run build` in the `DancingGoat` directory to build CSS files for the project ([node.js](https://nodejs.org/) must be installed before running this command)
3. Open the `DancingGoat.sln` solution file and run the app
4. Follow the setup wizard to setup your project or, alternatively, adjust the `\DancingGoat\appsettings.json` file:

   ```jsonc
   {
     // ...
     "DeliveryOptions": {
       "ProjectId": "YOUR_PROJECT_ID"
     }
     // ...
   }
   ```
> Follow the [step-by-step tutorial](https://kontent.ai/learn/tutorials/develop-apps/get-started/run-sample-app?tech=dotnet) for even more details.

## Features

### Edit mode & preview

Content contributors sometimes need to fix errors or typos right when they see them on the website. The sample app allows users to navigate from a piece of content on the site straight to the corresponding content item or element in Kontent.ai.

To see Edit mode in action:

1. In your Kontent.ai project navigate to Project Settings -> API keys to get the Preview API key. 
2. Enable Delivery Preview API by adding the key to the `\DancingGoat\appsettings.json` file:

   ```jsonc
   {
     // ...
     "DeliveryOptions": {
       "UsePreviewApi": true,
       "PreviewApiKey": "YOUR_DELIVERY_PREVIEW_API_KEY"
     }
     // ...
   }
   ```

   - **Delivery Preview API**: change the key named `PreviewApiKey` in the `DeliveryOptions` section, and use the Delivery Preview API key as its value. To enable calls over the Delivery Preview API, you also need to change the value to `true` of the key named `UsePreviewApi`.

3. Run the app.
4. Navigate to the **About us** section.
5. Click the **Edit mode** switch in the bottom-left corner.

Edit buttons will appear next to each piece of content on the page.
To explore how the functionality is implemented, navigate to the [`TagHelpers`](https://github.com/kontent-ai/sample-app-net/tree/master/DancingGoat/TagHelpers) folder.

### Responsive images

The sample app contains a sample implementation of the `img-asset` tag helper from the [Kontent.Ai.AspNetCore](https://www.nuget.org/packages/Kontent.Ai.AspNetCore) NuGet package. Using the `img-asset` tag helper, you can easily create an `img` tag with `srcset` and `sizes` attributes. Read more about [image transformation API](https://kontent.ai/learn/reference/image-transformation).
You can adjust the behaviour in the `appsettings.json` file.

```json
"ImageTransformationOptions": {
  "ResponsiveWidths": [ 200, 400, 600, 800, 1000, 1200, 1400, 1600, 2000, 4000 ]
},
```

### Localized routing

The app demonstrates the usage of language prefixes (e.g. `/en-US/articles`) for localizing URLs for SEO purposes. Each language is identified by its codename, in case of this project it is `en-US` and `es-ES`.
The used language is obtained from the URL in the `/DancingGoat/Infrastructure/RouteRequestCultureProvider` and set as a culture of the app. 
## Get involved

Check out the [contributing](CONTRIBUTING.md) page to see the best places to file issues, start discussions, and begin contributing.
