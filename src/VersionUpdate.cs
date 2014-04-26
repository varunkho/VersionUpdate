using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Build.Utilities;
using System.IO;
using Microsoft.Build.Framework;
using System.Text.RegularExpressions;

namespace VersionUpdate
{
    public class VersionUpdate : Task
    {

        const string AIV_PATTERN = @"AssemblyInformationalVersion\(""(\d+\.\d+)\.(\d+)-(.+?)""\)";
        const string AV_PATTERN = @"AssemblyVersion\(""(\d+\.\d+\.\d+)\.(\d+)""\)";
        const string AFV_PATTERN = @"AssemblyFileVersion\(""(\d+\.\d+\.\d+)\.(\d+)""\)";

        public override bool Execute()
        {
            Log.LogMessage("Version file: {0}", AssemblyInfoFilePath);

            string content = File.ReadAllText(AssemblyInfoFilePath, Encoding.UTF8);

            if (!this.SkipAssemblyInformationalVersion)
            {
                content = Regex.Replace(content,
                AIV_PATTERN,
                (Match m) =>
                {
                    Log.LogMessage("Current AssemblyInformationalVersion: {0}.{1}-{2}", m.Groups[1].Value, m.Groups[2].Value, m.Groups[3].Value);

                    int patch = int.Parse(m.Groups[2].Value);

                    return string.Format("AssemblyInformationalVersion(\"{0}.{1}-{2}\")",
                        m.Groups[1].Value, // major and miner versions
                        ++patch, // incremented patch version
                        m.Groups[3].Value // version text
                        );
                }, RegexOptions.IgnoreCase);
            }

            if (!this.SkipAssemblyVersion)
            {
                content = Regex.Replace(content,
            AV_PATTERN,
            (Match m) =>
            {
                Log.LogMessage("Current AssemblyVersion: {0}.{1}", m.Groups[1].Value, m.Groups[2].Value);

                int revision = int.Parse(m.Groups[2].Value);

                return string.Format("AssemblyVersion(\"{0}.{1}\")",
                    m.Groups[1].Value, // major, miner and build number
                    ++revision // incremented revision
                    );
            }, RegexOptions.IgnoreCase);
            }

            if (!this.SkipAssemblyFileVersion)
            {
                content = Regex.Replace(content,
    AFV_PATTERN,
    (Match m) =>
    {
        Log.LogMessage("Current AssemblyFileVersion: {0}.{1}", m.Groups[1].Value, m.Groups[2].Value);

        int revision = int.Parse(m.Groups[2].Value);

        return string.Format("AssemblyFileVersion(\"{0}.{1}\")",
            m.Groups[1].Value, // major, miner and build number
            ++revision // incremented revision
            );
    }, RegexOptions.IgnoreCase);
            }

            File.WriteAllText(AssemblyInfoFilePath, content, Encoding.UTF8);

            return true;
        }

        /// <summary>
        /// Gets or sets the path to AssemblyInfo.cs file.
        /// </summary>
        /// <value>
        /// The path to AssemblyInfo.cs file.
        /// </value>
        [Required]
        public string AssemblyInfoFilePath { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to skip update of AssemblyVersion attribute. The default is false.
        /// </summary>
        /// <value>
        /// <c>true</c> if AssemblyVersion attribute shouldn't be updated; otherwise, <c>false</c>.
        /// </value>
        public bool SkipAssemblyVersion { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to skip update of AssemblyInformationalVersion attribute. The default is false.
        /// </summary>
        /// <value>
        /// <c>true</c> if AssemblyInformationalVersion attribute shouldn't be updated; otherwise, <c>false</c>.
        /// </value>
        public bool SkipAssemblyInformationalVersion { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to skip update of AssemblyFileVersion attribute. The default is false.
        /// </summary>
        /// <value>
        /// <c>true</c> if AssemblyFileVersion attribute shouldn't be updated; otherwise, <c>false</c>.
        /// </value>
        public bool SkipAssemblyFileVersion { get; set; }
    }
}
