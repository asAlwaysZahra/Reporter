namespace ImplementationBase.attributes;

[AttributeUsage(AttributeTargets.Class)]
public class BootcampExtensionAttribute(string name) : Attribute
{
    public string Name { get; set; } = name;
}
