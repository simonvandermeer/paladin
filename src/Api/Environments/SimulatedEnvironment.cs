using Paladin.Api.Dto.Environments;

namespace Paladin.Api.Environments
{
    public class SimulatedEnvironment : IEnvironment
    {
        public SimulatedEnvironment()
        {
        }

        public string Id => throw new NotImplementedException();

        public bool Simulated => throw new NotImplementedException();

        public EnvironmentState State => throw new NotImplementedException();

        public void RunAction(EnvironmentAction action)
        {
            throw new NotImplementedException();
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
            throw new NotImplementedException();
        }
    }
}
