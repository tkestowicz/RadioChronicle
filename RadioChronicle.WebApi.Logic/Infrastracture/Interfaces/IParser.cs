namespace RadioChronicle.WebApi.Logic.Infrastracture.Interfaces
{
    public interface IParser<in TInput, out TResult>
        where TInput : class
        where TResult : class
    {
        TResult Parse(TInput node);
    }
}