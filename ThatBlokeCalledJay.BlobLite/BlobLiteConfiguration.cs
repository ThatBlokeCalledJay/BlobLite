namespace ThatBlokeCalledJay.BlobLite
{
    /// <summary>BlobLiteClient configuration properties.</summary>
    public interface IBlobLiteConfiguration
    {
        /// <summary>Blob connection string.</summary>
        string ConnectionString { get; set; }
    }

    /// <inheritdoc />
    public class BlobLiteConfiguration : IBlobLiteConfiguration
    {
        /// <inheritdoc />
        public string ConnectionString { get; set; }

        /// <summary>
        /// Instantiate a new instance of <see cref="BlobLiteConfiguration"/>.
        /// </summary>
        public BlobLiteConfiguration() : this(string.Empty)
        { }

        /// <summary>
        /// Instantiate a new instance of <see cref="BlobLiteConfiguration"/> with the provided <paramref name="connectionString"/>.
        /// </summary>
        /// <param name="connectionString"></param>
        public BlobLiteConfiguration(string connectionString)
        {
            ConnectionString = connectionString;
        }
    }
}