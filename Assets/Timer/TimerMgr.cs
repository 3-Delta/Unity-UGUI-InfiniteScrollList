/*
 * Unity Timer
 *
 * Version: 1.0
 * By: Alexander Biggs + Adam Robinson-Yu
 */

using System.Collections.Generic;

/// <summary>
/// Manages updating all the <see cref="Timer"/>s that are running in the application.
/// This will be instantiated the first time you create a timer -- you do not need to add it into the
/// scene manually.
/// </summary>
public static class TimerMgr {
    private static List<MonoTimer> timers = new List<MonoTimer>();
    private static List<MonoTimer> timersToAdd = new List<MonoTimer>();

    public static void Register(MonoTimer timer) {
        timersToAdd.Add(timer);
    }

    public static void UnRegister(MonoTimer timer) {
        int index = timers.IndexOf(timer);
        int lastIndex = 0;
        if (index != -1) {
            if (timers.Count > 1) {
                lastIndex = timers.Count - 1;
                if (index != lastIndex) {
                    timers[index] = timers[lastIndex];
                }
            }
            else {
                timers.RemoveAt(lastIndex);
            }
        }

        index = timersToAdd.IndexOf(timer);
        if (index == -1) {
            lastIndex = timersToAdd.Count - 1;
            if (index != lastIndex) {
                timersToAdd[index] = timersToAdd[lastIndex];
            }

            timersToAdd.RemoveAt(lastIndex);
        }
    }

    public static void CancelAll() {
        timers.ForEach(timer => { timer.Cancel(); });
        timers.Clear();
        timersToAdd.Clear();
    }

    public static void PauseAll() {
        timers.ForEach(timer => { timer.Pause(); });
    }

    public static void ResumeAll() {
        timers.ForEach(timer => { timer.Resume(); });
    }

    public static void OnUpdate() {
        UpdateAll();
    }

    private static void UpdateAll() {
        if (timersToAdd.Count > 0) {
            timers.AddRange(timersToAdd);
            timersToAdd.Clear();
        }

        timers.ForEach(timer => { timer.Update(); });
        timers.RemoveAll(t => t.isDone);
    }
}