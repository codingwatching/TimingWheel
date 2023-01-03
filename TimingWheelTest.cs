#region 注 释

/***
 *
 *  Title:
 *  
 *  Description:
 *  
 *  Date:
 *  Version:
 *  Writer: 半只龙虾人
 *  Github: https://github.com/HalfLobsterMan
 *  Blog: https://www.crosshair.top/
 *
 */

#endregion

using System;
using Sirenix.OdinInspector;
using UnityEngine;
using CZToolKit.TimingWheel;

public class TimingWheelTest : MonoBehaviour
{
    private TimingWheel tm, mtm, htm, dtm, montm, ytm;

    void Start()
    {
        // 秒轮(基础轮)
        tm = new TimingWheel(200000, 50, DateTime.Now.ToFileTime());
        // 分轮
        mtm = tm.BuildParent(60);
        // 时轮
        htm = mtm.BuildParent(60);
        // 天轮
        dtm = htm.BuildParent(24);
        // 月轮(大概天数),这个31只要是大于每月最大天数就行，保证循环
        montm = dtm.BuildParent(31);
        // 年轮
        ytm = montm.BuildParent(12);
    }

    void FixedUpdate()
    {
        tm.Step(DateTime.Now.ToFileTime());
    }

    [Button]
    public void B()
    {
        // 秒 分 时 天 比例固定
        // 月 年 比例动态变化

        // 添加秒级事件(每秒执行)
        tm.AddTask(new TimingWheel.TimeTask(tm.CurrentTime + tm.WheelSpan, StaticTask, tm.WheelSpan, -1));
        // 添加分钟级事件(每分钟某个时间执行)
        tm.AddTask(new TimingWheel.TimeTask(tm.CurrentTime + mtm.WheelSpan, StaticTask, mtm.WheelSpan, -1));
        // 添加小时级事件(每小时某个时间执行)
        tm.AddTask(new TimingWheel.TimeTask(tm.CurrentTime + htm.WheelSpan, StaticTask, htm.WheelSpan, -1));
        // 添加天级事件(每天某个时间执行)
        tm.AddTask(new TimingWheel.TimeTask(tm.CurrentTime + dtm.WheelSpan, StaticTask, dtm.WheelSpan, -1));
        // 添加月级事件(每月某个时间执行)
        tm.AddTask(new TimingWheel.TimeTask(tm.CurrentTime + tm.WheelSpan, MonthTask));
        // 添加年级事件(每年某个时间执行)
        tm.AddTask(new TimingWheel.TimeTask(tm.CurrentTime + tm.WheelSpan, YearTask));
    }

    private void StaticTask()
    {
        Debug.Log(DateTime.FromFileTime(tm.CurrentTime));
    }

    private void MonthTask()
    {
        Debug.Log(DateTime.FromFileTime(tm.CurrentTime));
        tm.AddTask(new TimingWheel.TimeTask(DateTime.Now.AddMonths(1).ToFileTime(), MonthTask));
    }

    private void YearTask()
    {
        Debug.Log(DateTime.FromFileTime(tm.CurrentTime));
        tm.AddTask(new TimingWheel.TimeTask(DateTime.Now.AddYears(1).ToFileTime(), YearTask));
    }
}