using System;

namespace EntityLib.Friendly.Turret
{
    public interface ITurretInstallModule : IModule
    {
        event Action InstallStarted;
        bool IsInstalled { get; }

        void Install();
        void StopInstallImmediately();
    }
}
