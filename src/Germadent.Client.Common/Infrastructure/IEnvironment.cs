namespace Germadent.Client.Common.Infrastructure
{
    public interface IEnvironment
    {
        void Restart();

        void Shutdown();
    }
}
