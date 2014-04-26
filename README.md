Introduction
-----------

An MS Build task that you can integrate in your build flow to update patch/revision numbers of assemblies.

Features
-----------

The task can update patch/revision component of some or all of the following attributes:

1.  [AssemblyInformationalVersion](http://msdn.microsoft.com/en-us/library/system.reflection.assemblyinformationalversionattribute.aspx)
2.  [AssemblyFileVersion](http://msdn.microsoft.com/en-us/library/system.reflection.assemblyfileversionattribute.aspx)
3.  [AssemblyVersion](http://msdn.microsoft.com/en-us/library/system.reflection.assemblyversionattribute.aspx)

Usage
-----------

1.  Include the task in your .csproj file (usually just before the first property group definition).

    <code>&lt;UsingTask TaskName="VersionUpdate.VersionUpdate" AssemblyFile="..\References\VersionUpdate.dll"/&gt;</code>

2.  Refer the task inside before build target (this means that we want task to update patch/revision component of version strings on every build of this project):

<code>
    &lt;Target Name="BeforeBuild"&gt;
        &lt;VersionUpdate AssemblyInfoFilePath="$(ProjectDir)\properties\AssemblyInfo.cs" /&gt;
    &lt;/Target&gt;
</code>

3.  By default, all of the version attributes (mentioned above) are updated. However, you can tell VersionUpdate to ignore one or more attributes by using following properties:
    1. IgnoreAssemblyVersion
    2. IgnoreAssemblyInformationalVersion
    3. IgnoreAssemblyFileVersion

Notes
-----------

Task assumes following:

-  Both AssemblyVersion and AssemblyFileVersion are defined in the format n.n.n.n (where n is one or more digits)
-  AssemblyInformationalVersion is defined in the format n.n.n-s (where n is one or more digits and s is a string value for a [semantic version](http://semver.org/))
-  If attributes do not follow the format above, update will not happen for such attributes. 
