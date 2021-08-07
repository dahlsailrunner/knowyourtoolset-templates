# KnowYourToolset.Templates

_[![NuGet version](https://img.shields.io/nuget/v/KnowYourToolset.Templates.svg?style=flat&label=nuget%3A%20KnowYourToolset.Templates)](https://www.nuget.org/packages/KnowYourToolset.Templates)_

[![GitHub Action](https://github.com/dahlsailrunner/knowyourtoolset-templates/actions/workflows/pack-and-publish-templates.yml/badge.svg)](https://github.com/dahlsailrunner/knowyourtoolset-templates/actions/workflows/pack-and-publish-templates.yml)

This is a "template pack" that uses the .NET templating engine.  It has example templates that can be used as starting points that may be helpful to you 
or could be extended to be your own templates.

To install (see below for more information): `dotnet new -i KnowYourToolset.Templates`  

This package and repo contains sample .NET templates that can help jump-start .NET development efforts.

Included templates:
* `kyt-package`: Creates a .NET Standard class library with [MinVer](https://github.com/adamralph/minver) and [Serilog](https://github.com/serilog/serilog) added and includes a GitHub action 
that publishes the package to NuGet (assuming you create a secret with your NuGet API Key).
* `kyt-backend`: Creates a multi-project solution for an API.  Includes a separate project for "business logic" and also includes a unit test project.

## Use cases
These templates can be modified to include YOUR best practices and cross-cutting concerns.  Just fork the repo 
and make them your own!  :)  See [this blog post]() for more details on why and how you can do this.

Primary use-cases for these templates:

* Creating brand new applications: new APIs, new NuGet packages, etc.
* Evaluating code as a reference for existing applications you've created
* Trying out new ideas using simple projects   

This second use-case can be a valuable one -- these starter templates have been kept simple but include 
some key cross-cutting concerns like testability, versioning, authentication / authorization, and more.  If 
you have an existing .NET Core application and want to make sure that it follows good 
practices for these concerns, compare your existing code with what is in these templates.  

And instantiating and debugging a template is a good way to gain a better understanding of what's there.

## Installation 
Installation of the templates is simple, but you need the .NET Core SDK installed 
to use them.  That's included with Visual Studio 2019 (Mac or Windows version) or you 
can get it standalone from here (you want the SDKs for your platform):

https://dotnet.microsoft.com/download 

Then to install the templates:

`dotnet new -i KnowYourToolset.Templates`

### Uninstalling / Resetting
If you'd like to uninstall these templates, it's pretty simple:

`dotnet new -u KnowYourToolset.Templates`

If you need to set back your `dotnet new` list to "factory defaults", use this command:

`dotnet new --debug:reinit`

## Templates
### **dotnet new kyt-package**
This template creates a .NET Standard class library that will be published as a NuGet package to the RealPage
Artifactory Nuget feed.

`dotnet new kyt-package -o <projname>`

Check out the Readme and Instructions documents in that folder (for your generated project) for more details about this template.

I created a sample instance with the following command:

`dotnet new kyt-package -o KnowYourToolset.SamplePackage`

The repo is here (and it has a pre-release version and a formal `1.0.0` version published for you to see): https://github.com/dahlsailrunner/knowyourtoolset-samplepackage

### **dotnet new kyt-backend**
This template creates a multi-project .NET solution for an API that includes business logic and unit tests.

`dotnet new kyt-backend -o <projname>`  (no Docker support included)
`dotnet new kyt-backend -D <projname>` (includes Docker support - .dockerignore and Dockerfiles, and Kubernetes manifests are *planned*).

Check out the Readme and Instructions documents in that folder (for your generated project) for more details about this template.

A sample you might create:

`dotnet new kyt-backend -o BookClub`

## Things to try 
Check the `Instructions.md` file for some simple things to try and more detailed guidance on introducing 
your own logic into the projects / applications. 

## Todos in the projects
When you create a project there are a few steps you need to follow.  They are documented with the following syntax:

`//TODO: take this action`

So if you open a "Task View" in Visual Studio you should be able to see the short list of things you probably need to follow-up on to really get your project set up correctly.

## Contributing
If you'd like to add more templates or update the existing ones:

- fork the repo
- branch it
- make your changes, update the semantic version of the `.nuspec` file, and update the readme and other applicable docs
- commit and publish your new branch (to your fork)
- submit a pull request

Upon completion of the pull request, a new version of the package will be published.