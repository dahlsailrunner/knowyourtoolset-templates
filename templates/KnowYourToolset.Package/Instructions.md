# Instructions

Using this template is pretty straight-forward - especially after you've done it once!  :)

Basic steps (described in detail below) are:

- Update Package Properties
- Push the code into a GitHub repository
- Create a NuGet API Key and a GitHub Repository Secret for it
- Run the included Action to publish your package as a "prerelease" version to Nuget

After the above, new versions are published automatically:

- Pre-releases on commits to `main` (push or pull request)
- Formal version on creation of a "published release" via GitHub "tag" functionality

## Update the Package Properties

Make sure to also include package details in the csproj file (Package tab of Project->Properties)

- [ ] `Authors`: names of author(s)
- [ ] `Description`: "what's in it for me" description - tell developers why they should use this package
- [ ] `Project URL`: Link to your repo - should show readme
- [ ] `Repository URL`: Project repo root URL
- [ ] `PackageLicenseExpression`: Set the license if you are publishing this publicly
- [ ] `Icon`: update the icon file to be your own (128x128 png or jpg)
- [ ] `Tags`: add any you think might be worthwhile

## Publishing the package

### Create and push to GitHub repo

Log in to GitHub and create new repository (leave everything empty except
the repo name).

- In the top-level folder for this new solution, type `git init` from a terminal.  
- Using whatever git tool you want, commit all of the files with an initial commit message (you may want to add `.gitignore` to omit certain files).
- Follow the provided instructions on your newly-created GitHub repo to add it as a remote -- should be something like (the URL will be for YOUR repo, not the one below):

    ```bash
    git remote add origin https://github.com/dahlsailrunner/knowyourtoolset-samplepackage.git
    ```

- Get your files pushed to GitHub:  `git push -u origin main` (or whatever way you prefer).

### Set up the Action to Publish to NuGet

This template contains an
action (`.github/workflows/pack-and-publish-package.yml`) that will
publish the project(s) in this solution to [NuGet.org](https://www.nuget.org).

In order to actually succeed in publishing, the action requires a **Repository Secret**
called `NUGET_API_KEY` to be defined in the GitHub repository where you put this code and want to run the action.

To create an API key for NuGet, follow the [documentation here](https://docs.microsoft.com/en-us/nuget/nuget-org/publish-a-package#create-api-keys).

Once you have copied the API key, go to the `Settings->Secrets` area within your repo and create a new `Repository Secret`.

Give it a name of `NUGET_API_KEY` and paste the key you copied above into the Value field.  Then save the key.  

If you wanted to use a different name for the key, just make sure to update the name in the workflow yml file as well.

Then run the action manually or it should trigger on any update to the `main` branch.  

**N O T E:** It will likely take a couple of minutes for your package to actually be "visible" within NuGet -- there is a `Validating` step performed by NuGet on
just-published packages that needs to complete before they show up.

### Versioning - Automagically!

Versioning works via a NuGet package included in this template called [MinVer](https://github.com/adamralph/minver). It's a terrific package and I encourage you
giving your support to the project.

Whenever the action is triggered (completed pull requests or a push to `main`), a new "prerelease" version number is created that increments the most recently-tagged
version.  So if your most recent tag on the repo was `1.0.4`, it would create a new version like `1.0.5-alpha.0.1`.

This new pre-release version gets published to NuGet and you need to specify "Include Prerelease" in order to include this version if you are using the
"Manage NuGet Packages" feature of Visual Studio.

To create a "real" release that will be available without using the "Include Prerelease", go to the "tags" area and Create a new Release with a semantic version
number (like `1.0.5`).  This will trigger the action AGAIN, but the NuGet package will be published with the version that you specified for your tag.

Sweet!
