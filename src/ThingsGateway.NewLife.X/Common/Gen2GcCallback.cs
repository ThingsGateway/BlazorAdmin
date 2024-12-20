﻿using System.ComponentModel;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;

namespace ThingsGateway.NewLife;

/// <summary>Gen2垃圾回收回调</summary>
[EditorBrowsable(EditorBrowsableState.Never)]
public class Gen2GcCallback : CriticalFinalizerObject
{
    private readonly Func<Boolean>? _callback0;

    private readonly Func<Object, Boolean>? _callback1;

    private GCHandle _weakTargetObj;

    private Gen2GcCallback(Func<Boolean> callback) => _callback0 = callback;

    private Gen2GcCallback(Func<Object, Boolean> callback, Object targetObj)
    {
        _callback1 = callback;
        _weakTargetObj = GCHandle.Alloc(targetObj, GCHandleType.Weak);
    }

    /// <summary>
    /// Registers a callback to be invoked during Gen2 garbage collection.
    /// </summary>
    /// <param name="callback">The callback function to be invoked.</param>
    public static void Register(Func<Boolean> callback) => _ = new Gen2GcCallback(callback);

    /// <summary>
    /// Registers a callback to be invoked during Gen2 garbage collection with a target object.
    /// </summary>
    /// <param name="callback">The callback function to be invoked.</param>
    /// <param name="targetObj">The target object associated with the callback.</param>
    public static void Register(Func<Object, Boolean> callback, Object targetObj) => _ = new Gen2GcCallback(callback, targetObj);

    /// <summary>析构</summary>
    ~Gen2GcCallback()
    {
        if (_weakTargetObj.IsAllocated)
        {
            var target = _weakTargetObj.Target;
            if (target == null)
            {
                _weakTargetObj.Free();
                return;
            }
            try
            {
                if (_callback1 != null && !_callback1(target))
                {
                    _weakTargetObj.Free();
                    return;
                }
            }
            catch { }
        }
        else
        {
            try
            {
                if (_callback0 != null && !_callback0())
                {
                    return;
                }
            }
            catch { }
        }
        GC.ReRegisterForFinalize(this);
    }
}
