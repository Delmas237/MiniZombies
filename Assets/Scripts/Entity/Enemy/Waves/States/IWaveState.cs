using System;
using System.Collections;

public interface IWaveState
{
    public IEnumerator Control(Func<IEnumerator> nextWaveOrRecheck);
    public void UpdateUIInfo();
}
