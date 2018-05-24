using UnityEngine;

public class BulkDataTest : MonoBehaviour
{
    [SerializeField] int _bufferCount = 10;
    [SerializeField] int _bufferSize = 1920 * 1080;
    [SerializeField] bool _doGetData = true;
    [SerializeField] bool _doSetData = true;

    ComputeBuffer[] _buffers;
    int[] _data;

    void OnEnable()
    {
        _buffers = new ComputeBuffer[_bufferCount];

        for (var i = 0; i < _bufferCount; i++)
            _buffers[i] = new ComputeBuffer(_bufferSize, sizeof(int));

        _data = new int[_bufferSize];
    }

    void OnDisable()
    {
        foreach (var buffer in _buffers) buffer.Dispose();
    }

    void Update()
    {
        if (_doGetData) foreach (var buffer in _buffers) buffer.GetData(_data);
        if (_doSetData) foreach (var buffer in _buffers) buffer.SetData(_data);
    }
}
