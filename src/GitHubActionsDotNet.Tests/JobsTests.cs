using GitHubActionsDotNet.Helpers;
using GitHubActionsDotNet.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GitHubActionsDotNet.Tests;

[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
[TestClass]
public class JobsTests
{

    [TestMethod]
    public void SimpleJobTest()
    {
        //Arrange
        GitHubActionsRoot root = new();
        Step[] buildSteps = new Step[] {
            CommonStepsHelper.AddCheckoutStep(),
            CommonStepsHelper.AddScriptStep(null, @"echo ""hello world""", "cmd")
        };
        root.jobs = new();
        Job buildJob = JobHelper.AddJob(
            "Build job",
            "windows-latest",
            buildSteps,
            null,
            null,
            30);
        root.jobs.Add("build", buildJob);

        //Act
        string yaml = Serialization.GitHubActionsSerialization.Serialize(root);

        //Assert
        string expected = @"
jobs:
  build:
    name: Build job
    runs-on: windows-latest
    timeout-minutes: 30
    steps:
    - uses: actions/checkout@v2
    - run: echo ""hello world""
      shell: cmd
";
        expected = UtilityTests.TrimNewLines(expected);
        Assert.AreEqual(expected, yaml);

    }

    [TestMethod]
    public void SimpleVariablesJobTest()
    {
        //Arrange
        GitHubActionsRoot root = new();
        Step[] buildSteps = new Step[] {
            CommonStepsHelper.AddCheckoutStep(),
            CommonStepsHelper.AddScriptStep(null, @"echo ""hello world""", "cmd")
        };
        root.jobs = new();
        Job buildJob = JobHelper.AddJob(
            "Build job",
            "windows-latest",
            buildSteps,
            null);
        root.jobs.Add("build", buildJob);

        //Act
        string yaml = Serialization.GitHubActionsSerialization.Serialize(root);

        //Assert
        string expected = @"
jobs:
  build:
    name: Build job
    runs-on: windows-latest
    env:
      Variable1: new variable
    steps:
    - uses: actions/checkout@v2
    - run: echo ""hello world""
";

        expected = UtilityTests.TrimNewLines(expected);
        Assert.AreEqual(expected, yaml);

    }



    [TestMethod]
    public void ComplexVariablesWithComplexDependsOnJobTest()
    {
        //Arrange
        GitHubActionsRoot root = new();


        //Act
        string yaml = Serialization.GitHubActionsSerialization.Serialize(root);

        //Assert
        string expected = @"
jobs:
  Build:
    name: Build job
    runs-on: windows-latest
    needs:
    - AnotherJob
    env:
      group: Active Login
      sourceArtifactName: nuget-windows
      targetArtifactName: nuget-windows-signed
      pathToNugetPackages: '**/*.nupkg'
    steps:
    - uses: actions/checkout@v2
    - run: echo your commands here
      shell: cmd
    ";
        expected = UtilityTests.TrimNewLines(expected);
        Assert.AreEqual(expected, yaml);

    }


    [TestMethod]
    public void ComplexVariablesWithSimpleDependsOnJobTest()
    {
        //Arrange
        GitHubActionsRoot root = new();


        //Act
        string yaml = Serialization.GitHubActionsSerialization.Serialize(root);

        //Assert
        string expected = @"
jobs:
  Build:
    name: Build job
    runs-on: windows-latest
    needs:
    - AnotherJob
    env:
      group: Active Login
      sourceArtifactName: nuget-windows
      targetArtifactName: nuget-windows-signed
      pathToNugetPackages: '**/*.nupkg'
    steps:
    - uses: actions/checkout@v2
    - run: echo your commands here
      shell: cmd
    ";
        expected = UtilityTests.TrimNewLines(expected);
        Assert.AreEqual(expected, yaml);

    }



    [TestMethod]
    public void SimpleVariablesWithSimpleDependsOnJobTest()
    {
        //Arrange
        GitHubActionsRoot root = new();


        //Act
        string yaml = Serialization.GitHubActionsSerialization.Serialize(root);

        //Assert
        string expected = @"
jobs:
  Build:
    name: Build job
    runs-on: windows-latest
    needs:
    - AnotherJob
    env:
      Variable1: new variable
    steps:
    - uses: actions/checkout@v2
    - run: echo your commands here ${{ env.Variable1 }}
      shell: cmd
";

        expected = UtilityTests.TrimNewLines(expected);
        Assert.AreEqual(expected, yaml);

    }



    [TestMethod]
    public void SimpleVariablesWithComplexDependsOnJobTest()
    {
        //Arrange
        GitHubActionsRoot root = new();


        //Act
        string yaml = Serialization.GitHubActionsSerialization.Serialize(root);

        //Assert
        string expected = @"
jobs:
  Build:
    name: Build job
    runs-on: windows-latest
    needs:
    - AnotherJob
    env:
      Variable1: new variable
    steps:
    - uses: actions/checkout@v2
    - run: echo your commands here ${{ env.Variable1 }}
      shell: cmd
";

        expected = UtilityTests.TrimNewLines(expected);
        Assert.AreEqual(expected, yaml);

    }

    [TestMethod]
    public void CheckoutJobTest()
    {
        //Arrange
        GitHubActionsRoot root = new();


        //Act
        string yaml = Serialization.GitHubActionsSerialization.Serialize(root);

        //Assert
        string expected = @"
jobs:
  provisionProd:
    runs-on: ubuntu-latest
    steps:
    - uses: actions/checkout@v2
    - uses: actions/checkout@v2
      with:
        repository: git://MyProject/MyRepo
    - uses: actions/checkout@v2
      with:
        repository: MyGitHubRepo
";

        expected = UtilityTests.TrimNewLines(expected);
        Assert.AreEqual(expected, yaml);

    }

    [TestMethod]
    public void CheckoutSimpleJobTest()
    {
        //Arrange
        GitHubActionsRoot root = new();


        //Act
        string yaml = Serialization.GitHubActionsSerialization.Serialize(root);

        //Assert
        string expected = @"
jobs:
  Build:
    runs-on: ubuntu-latest
    steps:
    - uses: actions/checkout@v2
";

        expected = UtilityTests.TrimNewLines(expected);
        Assert.AreEqual(expected, yaml);

    }

    [TestMethod]
    public void EnvironmentJobTest()
    {
        //Arrange
        GitHubActionsRoot root = new();


        //Act
        string yaml = Serialization.GitHubActionsSerialization.Serialize(root);

        //Assert
        string expected = @"
jobs:
  provisionProd:
    name: Provision Prod
    runs-on: ubuntu-latest
    needs:
    - functionalTestsStaging
    environment:
      name: prod
    steps:
    - uses: actions/checkout@v2
    - run: echo hello world
";
        //url: https://abel-node-gh-accelerator.azurewebsites.net

        expected = UtilityTests.TrimNewLines(expected);
        Assert.AreEqual(expected, yaml);

    }

}
