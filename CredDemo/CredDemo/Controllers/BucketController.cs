using Amazon;
using Amazon.S3;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;
namespace CredDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BucketController : ControllerBase
    {
        private readonly IAmazonS3 _amazonS3;
        public BucketController(IAmazonS3 amazonS3)
        {
            _amazonS3 = amazonS3;
        }
        [HttpGet("list")]
        public async Task<IActionResult> ListAsync()
        {
            // var awsAccessKeyId = _configuration["AWS:AccessKey"];
            // var awsSecretAccessKey = _configuration["AWS:SecretKey"];
            // var s3Client = new AmazonS3Client(awsAccessKeyId,awsSecretAccessKey,RegionEndpoint.USEast1);
            // var data = await s3Client.ListBucketsAsync();
            var data = await _amazonS3.ListBucketsAsync();
            var buckets = data.Buckets.Select(b => { return b.BucketName;});
            return Ok(buckets);
        } 
    }
}
