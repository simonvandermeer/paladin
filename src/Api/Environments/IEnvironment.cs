﻿using Paladin.Api.Dto.Environments;

namespace Paladin.Api.Environments
{
    public interface IEnvironment
    {
        string Id { get; }
        bool Simulated { get; }
        EnvironmentState State { get; }

        void RunAction(EnvironmentAction action);
        void Close();
        void Reset();
    }
}
