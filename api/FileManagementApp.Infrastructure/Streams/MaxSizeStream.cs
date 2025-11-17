using FileManagementApp.Application.Common.Exceptions;

namespace FileManagementApp.Infrastructure.Streams;

/// <summary>
/// A stream wrapper that enforces a maximum size limit on the data read from the underlying stream.
/// </summary>
public sealed class MaxSizeStream : Stream
{
    private readonly Stream _inner;
    private readonly long _maxSize;
    private long _totalRead;

    public MaxSizeStream(Stream inner, long maxSize)
    {
        _inner = inner ?? throw new ArgumentNullException(nameof(inner));
        _maxSize = maxSize;
    }

    public override bool CanRead => true;
    public override bool CanSeek => false;
    public override bool CanWrite => false;

    public override long Length => throw new NotSupportedException();

    public override long Position
    {
        get => _totalRead;
        set => throw new NotSupportedException();
    }

    private void CheckLimit()
    {
        if (_totalRead > _maxSize)
        {
            throw new FileServiceException(FileServiceErrorCode.FileTooLarge);
        }
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
        int read = _inner.Read(buffer, offset, count);
        _totalRead += read;
        CheckLimit();
        return read;
    }

    public override async ValueTask<int> ReadAsync(
        Memory<byte> buffer,
        CancellationToken cancellationToken = default)
    {
        int read = await _inner.ReadAsync(buffer, cancellationToken);
        _totalRead += read;
        CheckLimit();
        return read;
    }

    public override async Task<int> ReadAsync(
        byte[] buffer, int offset, int count,
        CancellationToken cancellationToken)
    {
        int read = await _inner.ReadAsync(buffer, offset, count, cancellationToken);
        _totalRead += read;
        CheckLimit();
        return read;
    }

    public override void Flush() => throw new NotSupportedException();

    public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();

    public override void SetLength(long value) => throw new NotSupportedException();

    public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException();
}
