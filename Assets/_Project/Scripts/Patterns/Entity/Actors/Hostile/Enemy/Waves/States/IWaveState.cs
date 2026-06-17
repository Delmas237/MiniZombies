using System;
using System.Collections;

namespace WavesLib
{
    public interface IWaveState
    {
        IEnumerator Control(Func<IEnumerator> nextWaveOrRecheck);
        void UpdateUIInfo();
    }
}
