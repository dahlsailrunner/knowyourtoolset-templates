# KnowYourToolset.Templates

_[![NuGet version](https://img.shields.io/nuget/v/KnowYourToolset.Templates.svg?style=flat&label=nuget%3A%20KnowYourToolset.Templates)](https://www.nuget.org/packages/KnowYourToolset.Templates)_

[![GitHub Action](https://github.com/dahlsailrunner/knowyourtoolset-templates/actions/workflows/pack-and-publish-templates.yml/badge.svg)](https://github.com/dahlsailrunner/knowyourtoolset-templates/actions/workflows/pack-and-publish-templates.yml)

This is a "template pack" that uses the .NET templating engine.  It has example templates that can be used as starting points that may
be helpful to you or could be extended to be your own templates.

To install (see below for more information): `dotnet new -i KnowYourToolset.Templates`  

This package and repo contains sample .NET templates that can help jump-start .NET development efforts.

Included templates:

* `kyt-package`: Creates a .NET Standard class library with [MinVer](https://github.com/adamralph/minver) and [Serilog](https://github.com/serilog/serilog) added and includes a GitHub action
that publishes the package to NuGet (assuming you create a secret with your NuGet API Key).
* `kyt-backend`: Creates a multi-project solution for an API.  Includes a separate project for "business logic" and also includes a unit test project.  _Runnable out
of the box from [Visual Studio](https://visualstudio.microsoft.com/), [Rider](https://www.jetbrains.com/rider/), and [VS Code](https://code.visualstudio.com/)!!_

## Use cases

These templates can be modified to include YOUR best practices and cross-cutting concerns.  Just fork the repo
and make them your own!  :)  See [this blog post](https://knowyourtoolset.com/2021/08/creating-useful-net-templates/) for more details on why and how you can do this.

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
to use them.  That's included with Visual Studio 2019/2022 (Mac or Windows version) or you
can get it standalone from here (you want the SDKs for your platform):

[https://dotnet.microsoft.com/download](https://dotnet.microsoft.com/download)

Then to install the templates:

`dotnet new -i KnowYourToolset.Templates`

**N O T E:** If you're using [VS Code](https://code.visualstudio.com/), you'll want at least the **C# extension** installed, and if you're using the Docker version of
the templates you'll also need the **Docker extension**.

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

`dotnet new kyt-package -o KnowYourToolset.ApiComponents`

The repo is here (and it has a pre-release version and a formal `1.0.1` version published for you to see): [https://github.com/dahlsailrunner/knowyourtoolset-apicomponents](https://github.com/dahlsailrunner/knowyourtoolset-apicomponents)

### **dotnet new kyt-backend**

This template creates a multi-project .NET solution for an API that includes business logic and unit tests.

`dotnet new kyt-backend -o <projname>`  (no Docker support included)

`dotnet new kyt-backend -D -o <projname>` (includes Docker support - .dockerignore and Dockerfiles, and Kubernetes manifests are _planned_).

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

* Clone the repo
* Branch it
* Make your changes and test them (see below)
* Update the semantic version of the `.nuspec` file, and update [ReleaseNotes.md](./ReleaseNotes.md)
* Commit and publish your new branch
* Submit a pull request

Upon completion of the pull request, a new version of the package will be published.

### Key Concepts and Files

Here are some links for reference:

* [Custom Templates Overview](https://learn.microsoft.com/en-us/dotnet/core/tools/custom-templates)
* [Templating wiki reference](https://github.com/dotnet/templating/wiki)

The thing turns the different folders into actual templates
is the `.template.config/template.json` file in each of the
top-level subfolders.  Each of the different `template.json`
files defines the parameters for that template and its
various options.

There is also _**some**_ conditional processing for the templates,
and that is either defined directly in the `template.json` files -
for example when an entire file or directory should be excluded
based on options - or by `#if` conditionals in the code files
themselves based on conditions in the template.

An example of a `#if` evaluation is the `POSTGRESQL` conditional
in the API project.

### Testing Changes to Templates

First off, you should be able to run any of the templates if
you open the appropriate folder / solution inside your favorite IDE.

For the new API templates, they support different databases, and the way to "pick" the database is
to define one of the following constants in the `.csproj` files for the AppHost and the API:

* `SQLITE`
* `POSTGRESQL`
* `MSSQL`

To try the templates as they would be experienced by users of the templates, use the following commands
in sequence:

```bash
dotnet new uninstall KnowYourToolset.Templates
RemoveBinObjDirs ## see note below
nuget pack -NoDefaultExcludes
dotnet new install ./KnowYourToolset.Templates.<version>.nupkg
```

> **N O T E:** When packaging the templates, ALL files in the directory where the current template
> exists will be included. It's best to NOT include `bin`, `obj`, and `node_modules` folders, so
> the following handy PowerShell funciton will remove all of those folders from any subdirectories if you
> run it from the root folder of the templates:
>
> ```posh
> function RemoveBinObjDirs 
> {
>    gci -include bin,obj,node_modules -recurse | remove-item -force -recurse
> }
> ```
>

The version number should be part of the output of the
`nuget pack` command - and the `.nupkg` file will be in the
root directory of the templates.
