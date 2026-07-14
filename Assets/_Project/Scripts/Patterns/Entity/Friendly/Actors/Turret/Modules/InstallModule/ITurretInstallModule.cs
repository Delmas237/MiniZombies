using System;

namespace Entity.Friendly.Turret
{
    public interface ITurretInstallModule : IModule
    {
        event Action InstallStarted;
        bool IsInstalled { get; }

        void Install();
        void StopInstallImmediately();
    }
}
