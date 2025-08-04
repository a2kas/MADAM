namespace Tamro.Madam.Common.Configuration;
public class MinioSettings
{
    public string Endpoint { get; set; }
    public string AccessKey { get; set; }
    public string SecretKey { get; set; }
    public string SupplierContractsBucketName { get; set; }
    public string PeppolInvoicesBucketName { get; set; }
    public string MasterdataBucketName { get; set; }
    public string PublicApiUrl { get; set; }
    public string ReferenceBaseUrl { get; set; }
}
