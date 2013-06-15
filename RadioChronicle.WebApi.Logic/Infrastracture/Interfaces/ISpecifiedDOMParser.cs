namespace RadioChronicle.WebApi.Logic.Infrastracture.Interfaces
{
    public interface ISpecifiedDOMParser<out TReturned, in TType>
       // where TType : class
    {
        TReturned Parse(TType input);
    }
}