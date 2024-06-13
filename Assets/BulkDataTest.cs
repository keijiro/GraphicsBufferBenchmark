using UnityEngine;
using System.Linq;
using Array = System.Array;

public class BulkDataTest : MonoBehaviour
{
    public enum AccessorType { SetData, LockBuffer }

    [field:SerializeField] public int BufferCount = 32;
    [field:SerializeField] public int BufferSize = 1920 * 1080;
    [field:SerializeField] public AccessorType Accessor;

    GraphicsBuffer[] _buffers;
    uint[] _data;

    GraphicsBuffer NewBuffer()
      => new GraphicsBuffer(GraphicsBuffer.Target.Structured,
                            GraphicsBuffer.UsageFlags.LockBufferForWrite,
                            BufferSize, sizeof(uint));

    void RunSetData(GraphicsBuffer buffer)
      => buffer.SetData(_data);

    void RunLockBuffer(GraphicsBuffer buffer)
    {
        var array = buffer.LockBufferForWrite<uint>(0, BufferSize);
        array.CopyFrom(_data);
        buffer.UnlockBufferAfterWrite<uint>(BufferSize);
    }

    void Start()
    {
        _buffers = Enumerable.Range(0, BufferCount).
                   Select(i => NewBuffer()).ToArray();
        _data = new uint[BufferSize];
    }

    void OnDestroy()
      => Array.ForEach(_buffers, x => x.Dispose());

    void Update()
    {
        var sw = new System.Diagnostics.Stopwatch();
        sw.Start();

        if (Accessor == AccessorType.SetData)
            Array.ForEach(_buffers, x => RunSetData(x));
        else
            Array.ForEach(_buffers, x => RunLockBuffer(x));

        sw.Stop();
        Debug.Log($"{sw.Elapsed.TotalMilliseconds} ms");
    }
}
