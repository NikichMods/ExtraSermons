// SPDX-License-Identifier: MPL-2.0
namespace ExtraSermons.Research
{
    // Memory only. The host's weekly flag is never mirrored or restored here.
    internal sealed class ProbeState
    {
        internal bool Permit { get; private set; }
        internal bool Pending { get; private set; }
        internal bool Running { get; private set; }
        internal int Sequence { get; private set; }

        internal bool Grant()
        {
            if (Permit || Pending || Running) return false;
            Permit = true;
            return true;
        }

        internal bool Reserve(bool nativeFree)
        {
            if (nativeFree || !Permit || Pending || Running) return false;
            Pending = true;
            return true;
        }

        internal bool Started()
        {
            if (!Pending || !Permit || Running) return false;
            Permit = false;
            Pending = false;
            Running = true;
            Sequence++;
            return true;
        }

        internal void LaunchFailed() { Pending = false; }
        internal void Ended() { Running = false; Pending = false; }
        internal void Clear() { Permit = false; Pending = false; Running = false; }
    }
}
