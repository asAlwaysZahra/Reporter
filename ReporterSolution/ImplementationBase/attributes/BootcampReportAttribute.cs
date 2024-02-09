namespace ImplementationBase.attributes;

[AttributeUsage(AttributeTargets.Method, Inherited = false)]
public class BootcampReportAttribute : Attribute
{
    public string MethodName { get; set; }

    public BootcampReportAttribute(string methodName)
    {
        MethodName = methodName;
    }
}
