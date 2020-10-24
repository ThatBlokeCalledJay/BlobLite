namespace ThatBlokeCalledJay.BlobLite
{
    /// <summary></summary>
    public class BlobOperationResult
    {
        /// <summary>Blob ETag</summary>
        public string ETag { get; }

        /// <summary>
        /// Instantiate a new instance of <see cref="BlobOperationResult"/> with the provided <paramref name="eTag"/>
        /// </summary>
        /// <param name="eTag"></param>
        public BlobOperationResult(string eTag)
        {
            ETag = eTag;
        }
    }

    /// <summary></summary>
    public class BlobOperationResult<T> : BlobOperationResult
    {
        /// <summary>Blob content</summary>
        public T Result { get; }

        /// <summary>
        /// Instantiate a new instance of <see cref="BlobOperationResult"/> with the provided <paramref name="result"/> and <paramref name="eTag"/>
        /// </summary>
        /// <param name="result"></param>
        /// <param name="eTag"></param>
        public BlobOperationResult(T result, string eTag) : base(eTag)
        {
            Result = result;
        }
    }
}