﻿using GitHubActionsDotNet.Common;
using GitHubActionsDotNet.Models.Dependabot;
using System.Collections.Generic;
using System.IO;

namespace GitHubActionsDotNet.Serialization
{
    public static class DependabotSerialization
    {
        public static string Serialize(string startingDirectory,
            List<string> files,
            string interval = null,
            string time = null,
            string timezone = null,
            List<string> assignees = null,
            int openPRLimit = 0,
            bool includeActions = true)
        {
            if (startingDirectory == null)
            {
                return "";
            }

            DependabotRoot root = new DependabotRoot();
            List<Package> packages = new List<Package>();
            foreach (string file in files)
            {
                FileInfo fileInfo = new FileInfo(file);
                string cleanedFilePath = file.Replace(startingDirectory + "/", "");
                cleanedFilePath = cleanedFilePath.Replace(startingDirectory + "\\", "");
                cleanedFilePath = cleanedFilePath.Replace(fileInfo.Name, "");
                cleanedFilePath = "/" + cleanedFilePath.Replace("\\", "/");
                string packageEcoSystem = DependabotCommon.GetPackageEcoSystemFromFileName(fileInfo.Name);
                Package package = CreatePackage(cleanedFilePath, packageEcoSystem, interval, time, timezone, assignees, openPRLimit);
                packages.Add(package);
            }
            //Add actions
            if (includeActions == true)
            {
                Package actionsPackage = CreatePackage("/", "github-actions", interval, time, timezone, assignees, openPRLimit);
                packages.Add(actionsPackage);
            }
            root.updates = packages;

            //Serialize the object into YAML
            string yaml = YamlSerialization.SerializeYaml(root);

            //I can't use - in variable names, so replace _ with -
            yaml = yaml.Replace("package_ecosystem", "package-ecosystem");
            yaml = yaml.Replace("replaces_base", "replaces-base");
            yaml = yaml.Replace("dependency_name", "dependency-name");
            yaml = yaml.Replace("dependency_type", "dependency-type");
            yaml = yaml.Replace("prefix_development", "prefix-development");
            yaml = yaml.Replace("commit_message", "commit-message");
            yaml = yaml.Replace("update_types", "update-types");
            yaml = yaml.Replace("insecure_external_code_execution", "insecure-external-code-execution");
            yaml = yaml.Replace("pull_request_branch_name", "pull-request-branch-name");
            yaml = yaml.Replace("rebase_strategy", "rebase-strategy");

            return yaml;
        }

        public static DependabotRoot Deserialize(string yaml)
        {
            yaml = yaml.Replace("package-ecosystem", "package_ecosystem");
            yaml = yaml.Replace("open-pull-requests-limit", "open_pull_requests_limit");
            yaml = yaml.Replace("replaces-base", "replaces_base");
            yaml = yaml.Replace("dependency-name", "dependency_name");
            yaml = yaml.Replace("dependency-type", "dependency_type");
            yaml = yaml.Replace("prefix-development", "prefix_development");
            yaml = yaml.Replace("commit-message", "commit_message");
            yaml = yaml.Replace("update-types", "update_types");
            yaml = yaml.Replace("insecure-external-code-execution", "insecure_external_code_execution");
            yaml = yaml.Replace("pull-request-branch-name", "pull_request_branch_name");
            yaml = yaml.Replace("rebase-strategy", "rebase_strategy");

            DependabotRoot root = YamlSerialization.DeserializeYaml<DependabotRoot>(yaml);
            return root;
        }

        private static Package CreatePackage(string filePath,
            string packageEcoSystem,
            string interval = null,
            string time = null,
            string timezone = null,
            List<string> assignees = null,
            int openPRLimit = 0)
        {
            Package package = new Package()
            {
                package_ecosystem = packageEcoSystem,
                directory = filePath,
                assignees = assignees
            };
            if (interval != null ||
                time != null ||
                timezone != null)
            {
                package.schedule = new Schedule();
                if (interval != null)
                {
                    package.schedule.interval = interval;
                }
                if (time != null)
                {
                    package.schedule.time = time;
                }
                if (timezone != null)
                {
                    package.schedule.timezone = timezone;
                }
            }
            if (openPRLimit > 0)
            {
                package.open_pull_requests_limit = openPRLimit.ToString();
            }
            return package;
        }
    }
}
