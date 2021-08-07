using Serilog;

namespace KnowYourToolset.Package
{
    public class CustomClass
    {
        //TODO: Update Package settings in Project Properties ->Package tab (or csproj)
        //  Authors: names of authors
        //  Description: "what's in it for me" description - tell developers why they should use package
        //  Project URL: link to your repo (should show the readme)
        //  Repository URL: same as project URL
        //  Tags: add any you think might be worthwhile
        public static void SayHello(string greeting)
        {
            Log.Information("Passed greeting was {greeting}", greeting);
        }
    }
}
