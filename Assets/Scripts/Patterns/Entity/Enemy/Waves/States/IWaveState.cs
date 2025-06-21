using System;
using System.Collections;

namespace WavesLib
{
    public interface IWaveState
    {
        public IEnumerator Control(Func<IEnumerator> nextWaveOrRecheck);
        public void UpdateUIInfo();
    }
}
