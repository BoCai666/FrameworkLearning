namespace QSFramework.Runtime.IOC
{
    public interface IInstanceProvider
    {
        object GetInstance(IObjectResolver resolver);
    }
}