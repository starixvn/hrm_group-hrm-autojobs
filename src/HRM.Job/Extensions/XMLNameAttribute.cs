namespace HRM.Job.Extensions;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
public sealed class XMLNameAttribute(string name) : Attribute
{
    public string Name => name;
}