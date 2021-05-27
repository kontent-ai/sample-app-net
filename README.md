# Kentico Kontent sample ASP.NET Core MVC web application
[![Build & Test](https://github.com/Kentico/kontent-sample-app-net/actions/workflows/integrate.yml/badge.svg)](https://github.com/Kentico/kontent-sample-app-net/actions/workflows/integrate.yml)
[![codecov](https://codecov.io/gh/Kentico/kontent-sample-app-net/branch/master/graph/badge.svg?token=hj8JmDzLjJ)](https://codecov.io/gh/Kentico/kontent-sample-app-net)
[![Stack Overflow](https://img.shields.io/badge/Stack%20Overflow-ASK%20NOW-FE7A16.svg?logo=stackoverflow&logoColor=white)](https://stackoverflow.com/tags/kentico-kontent)
[![Discord](https://img.shields.io/discord/821885171984891914?label=Discord&logo=Discord&logoColor=white)](https://discord.gg/SKCxwPtevJ)
[![Deploy to Azure](https://aka.ms/deploytoazurebutton)](https://azuredeploy.net/)

This is a sample website written in ASP.NET Core that uses the [Kentico Kontent Delivery .NET SDK](https://github.com/Kentico/kontent-delivery-sdk-net) to manage and retrieve content from Kentico Kontent. For a brief walkthrough, read [Running the .NET sample app](https://docs.kontent.ai/tutorials/develop-apps/get-started/running-a-sample-application?tech=dotnet) on our Developer Hub.

## Quick start

This version is modified to provide a single default instance for Sample project available from Quickstart screen.

It receives the Project ID from a subdomain and renders respective sample content. By default, the shared sample project ID is used for development convenience.

https://975bf280fd91488c994c2f04416e5ee3.dancinggoat-sample.com/

Besides that, it provides a footer with preview mode onboarding where you can easily enable and later toggle preview mode by just entering preview API key.

### Quick start local testing and development
Follow the [Getting started first](#getting-started).

To run and debug the **quick start version in IIS** (with the domain wildcard):

* Make sure you have .NET Core SDK installed
* Make sure you have IIS enabled as a Windows feature
* Create an IIS site with .NET Core application pool (No managed code) and domain binding for *.kontent-sample-app-net.com
* Add the desired project-based domains to the hosts file, e.g. `127.0.0.1 975bf280fd91488c994c2f04416e5ee3.kontent-sample-app-net.com`
* Run the app in Debug mode with *IIS - Project ID from subdomain* profile selected

If you are missing anything, VS should tell you what's wrong (Rider just won't run the configuration).

To run and debug the **quick start version in IIS Express**:
* Make sure you have .NET Core runtime installed
* Make sure you have .NET Core hosting bundle installed
* Make sure you have IIS Express installed (usually bundled with one of Visual Studio's web development packages)
* Add desired project ID into *appsettings.json* (e.g. `975bf280-fd91-488c-994c-2f04416e5ee3`)
* Run the app in Debug mode with *IIS express* profile selected

If your Visual Studio or Rider have trouble running the .NET Core app in IIS Express, checkout their respective *applicationhost.config*. More details can be found in [a great article on the topic](https://blog.maartenballiauw.be/post/2019/02/26/asp-net-core-iis-express-empty-error-starting-application.html#fixing-the-applicationhostconfig-template)  

### Before you commit
* rebase on the top of the current `master` branch
* consider whether amending the commits or adding new one is the best way for your case

### Notes
* Quickstart is used in each copy of Sample Project in Kontent - links to this app (deployment of this branch)
  * can be found at Home > Quickstart and Settings > Preview URLs
  * are used in item editor where there is a special popup for the Preview button
* Before the latest revamp of this branch, it lived in [a529811](https://github.com/Kentico/kontent-sample-app-net/commit/a5298114ce54ae363fa0c46321f9b7b82713a5ba) commit tree    


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

Content contributors sometimes need to fix errors or typos right when they see them on the website. The sample app allows users to navigate from a piece of content on the site straight to the corresponding content item or element in Kentico Kontent. 

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

