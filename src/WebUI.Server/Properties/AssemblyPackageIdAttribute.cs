namespace System;

[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
public sealed class AssemblyPackageIdAttribute(string packageId) : Attribute
{
    public string PackageId { get; } = packageId;
}
