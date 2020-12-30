/*
 * Unity Timer
 *
 * Version: 1.0
 * By: Alexander Biggs + Adam Robinson-Yu
 */

using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;
using Object = UnityEngine.Object;

// Path: Assets/ThirdParty/UnityTimer/Source/TimerManager.cs.
// SvnVersion: -1.
// Author: kaclok created 2018/12/16 15:15:26 Sunday on pc: KACLOK.
// Copyright@nullgame`s testgame. All rights reserved.

/// <summary>
/// Manages updating all the <see cref="Timer"/>s that are running in the application.
/// This will be instantiated the first time you create a timer -- you do not need to add it into the
/// scene manually.
/// </summary>
public static class TimerMgr
{
    private static List<MonoTimer> timers = new List<MonoTimer>();
    // ����Ϊ��ʹ���������Ļ���ȥ��¼������timer��ԭ���ǣ���foreach�У�ÿ��timerִ�еĹ����У����ֱ������һ������times���ͻᵼ������������ʧЧ��
    // ����һ�㶼���ȵ�����timersȫ��foreach��ϣ�Ȼ���ȥ�����µ�timer
    private static List<MonoTimer> timersToAdd = new List<MonoTimer>();

    public static void Register(MonoTimer timer) { timersToAdd.Add(timer); }
    public static void CancelAll()
    {
        timers.ForEach(timer => { timer.Cancel(); });
        timers.Clear();
        timersToAdd.Clear();
    }
    public static void PauseAll() { timers.ForEach(timer => { timer.Pause(); }); }
    public static void ResumeAll() { timers.ForEach(timer => { timer.Resume(); }); }
    public static void OnUpdate() { UpdateAll(); }
    private static void UpdateAll()
    {
        if (timersToAdd.Count > 0)
        {
            timers.AddRange(timersToAdd);
            timersToAdd.Clear();
        }
        timers.ForEach(timer => { timer.Update(); });
        timers.RemoveAll(t => t.isDone);
    }
}