// SPDX-License-Identifier: MPL-2.0
using System;
namespace ExtraSermons.Research
{
    internal static class ProbeStateTests
    {
        private static int checks;
        private static void Check(bool b) { checks++; if (!b) throw new Exception("Check " + checks); }
        private static void Main()
        {
            var s = new ProbeState();
            Check(!s.Reserve(false)); Check(s.Grant()); Check(!s.Grant());
            Check(!s.Reserve(true)); Check(s.Permit); // free service wins
            // Selection, cancel and reopen have no state transition.
            Check(s.Permit && !s.Pending && !s.Running);
            Check(s.Reserve(false)); Check(!s.Reserve(false)); Check(!s.Grant());
            s.LaunchFailed(); Check(s.Permit && !s.Pending);
            Check(s.Reserve(false)); Check(s.Started()); Check(!s.Started());
            Check(!s.Permit && s.Running && s.Sequence == 1); Check(!s.Grant());
            s.Ended(); Check(s.Grant()); Check(s.Reserve(false)); Check(s.Started());
            Check(s.Sequence == 2); s.Clear(); Check(!s.Permit && !s.Pending && !s.Running);
            Console.WriteLine("Probe policy: " + checks + " assertions passed (not host/runtime acceptance).");
        }
    }
}
