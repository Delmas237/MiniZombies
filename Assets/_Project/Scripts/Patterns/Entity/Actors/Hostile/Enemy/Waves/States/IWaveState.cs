using System;
using System.Collections;

namespace Waves
{
    public interface IWaveState
    {
        IEnumerator Control(Func<IEnumerator> nextWaveOrRecheck);
        void UpdateUIInfo();
    }
}
