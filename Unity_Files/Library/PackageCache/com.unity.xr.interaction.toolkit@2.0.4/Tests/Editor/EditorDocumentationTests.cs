using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using NUnit.Framework;
using UnityEditor.PackageManager;

namespace UnityEditor.XR.Interaction.Toolkit.Editor.Tests
{
    [TestFixture]
    class EditorDocumentationTests
    {
        /// <summary>
        /// <see cref="PackageManager.PackageInfo"/> for com.unity.xr.interaction.toolkit.
        /// </summary>
        PackageManager.PackageInfo m_PackageInfo;

        [OneTimeSetUp]
        public void SetUp()
        {
            var assembly = Assembly.Load("Unity.XR.Interaction.Toolkit");
            Assert.That(assembly, Is.Not.Null);

            m_PackageInfo = PackageManager.PackageInfo.FindForAssembly(assembly);
            Assert.That(m_PackageInfo, Is.Not.Null);
            Assert.That(m_PackageInfo.version, Is.Not.Null);
            Assert.That(m_PackageInfo.version, Is.Not.Empty);
        }

        [Test]
        public void InstallationVersionMatchesPackageVersion()
        {
            // Verify that the 2021.3 installation instructions to install the package by name
            // has the correct version of this package.

            var documentationDirectory = new DirectoryInfo("Packages/com.unity.xr.interaction.toolkit/Documentation~");
            Assert.That(documentationDirectory, Is.Not.Null);
            Assert.That(documentationDirectory.Exists, Is.True);

            var installationFile = new FileInfo(Path.Combine(documentationDirectory.FullName, "installation.md"));
            Assert.That(installationFile, Is.Not.Null);
            Assert.That(installationFile.Exists, Is.True);

            var lines = File.ReadAllLines(installationFile.FullName);
            Assert.That(lines, Is.Not.Null);
            Assert.That(lines, Is.Not.Empty);

            // Find the lines in the file with the Version (optional) row in the table
            var versionLines = lines.Where(line => line.StartsWith("|**Version (optional)**|")).ToArray();
            Assert.That(versionLines, Is.Not.Empty, "Could not find version row of table. Has installation.md been updated to remove instructions for installing by name?");

            // Parse version
            foreach (var line in versionLines)
            {
                var version = ExtractCode(line);
                Assert.That(version, Is.Not.Empty, $"Could not parse the version field from the table line: {line}");
                Assert.That(version, Is.EqualTo(m_PackageInfo.version), "Version string in installation.md should match current version of package.");
            }
        }

        [Test]
        public void PackageDependencyLinksToCurrentVersionDependency()
        {
            // Verify that within the documentation, any package references to a specific version
            // matches the dependency versions of this package.

            var documentationDirectory = new DirectoryInfo("Packages/com.unity.xr.interaction.toolkit/Documentation~");
            Assert.That(documentationDirectory, Is.Not.Null);
            Assert.That(documentationDirectory.Exists, Is.True);

            // Create a list of all package dependencies in the form name@major.minor
            var nameAndVersions = new List<string>(m_PackageInfo.dependencies.Length);
            nameAndVersions.AddRange(m_PackageInfo.dependencies.Select(dependency => $"{dependency.name}@{GetMajorMinor(dependency)}"));

            // Package name must contain only lowercase letters, digits, hyphens(-), underscores (_), and periods (.)
            // See https://docs.unity3d.com/Manual/cus-naming.html
            // Regex in pattern name@major.minor
            var regex = new Regex(@"com\.unity\.[a-z0-9-_.]+@\d+\.\d+");
            foreach (var nameAndVersion in nameAndVersions)
            {
                Assert.That(regex.IsMatch(nameAndVersion), Is.True);
            }

            foreach (var fileInfo in documentationDirectory.EnumerateFiles("*.md"))
            {
                var lines = File.ReadAllLines(fileInfo.FullName);
                var lineNumber = 0;
                foreach (var line in lines)
                {
                    ++lineNumber;
                    foreach (Match match in regex.Matches(line))
                    {
                        var reference = match.ToString();
                        Assert.That(nameAndVersions, Has.Member(reference), $"{reference} in {fileInfo.Name}:{lineNumber} does not match dependency version.");
                    }
                }
            }
        }

        static string ExtractCode(string value)
        {
            // Extract the code from the string.
            // For example, "`1.2.3`" would return "1.2.3"
            var firstIndex = value.IndexOf('`');
            var lastIndex = value.LastIndexOf('`');
            if (firstIndex == -1 || lastIndex == -1 || firstIndex == lastIndex)
                return string.Empty;

            return value.Substring(firstIndex + 1, lastIndex - firstIndex - 1);
        }

        static string GetMajorMinor(DependencyInfo dependency) => GetMajorMinor(dependency.version);

        static string GetMajorMinor(string version)
        {
            // Return major.minor from the string.
            // For example, "1.2.3" would return "1.2"
            Assert.That(version, Is.Not.Null);
            Assert.That(version, Is.Not.Empty);
            var secondDotIndex = version.IndexOf('.', version.IndexOf('.') + 1);
            Assert.That(secondDotIndex, Is.GreaterThan(0));
            return version.Substring(0, secondDotIndex);
        }
    }
}