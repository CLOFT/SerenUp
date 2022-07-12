using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CLOFT.SerenUp.WebApp.Pages
{
    public class ReportModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IAmazonS3 _amazonS3;
        private readonly IConfiguration _configuration;

        public ReportModel(ILogger<IndexModel> logger, IAmazonS3 amazonS3, IConfiguration configuration)
        {
            _logger = logger;
            _amazonS3 = amazonS3;
            _configuration = configuration;
        }

        public List<string> S3FilesList { get; set; }

        public async Task OnGet()
        {
            ListObjectsRequest request = new ListObjectsRequest
            {
                BucketName = _configuration.GetConnectionString("bucketName"),
            };

            S3FilesList = new List<string>();
            ListObjectsResponse response = await _amazonS3.ListObjectsAsync(request);
            foreach (S3Object obj in response.S3Objects)
            {
                S3FilesList.Add(obj.Key);
            }
        }

        [BindProperty]
        public string objKey { get; set; }

        public async Task<IActionResult> OnPost(string objKey)
        {
            await ReadObjectDataAsync(objKey);

            return RedirectToPage("Report");
        }


        async Task ReadObjectDataAsync(string objKey)
        {
            TransferUtility fileTransferUtility = new TransferUtility(_amazonS3);

            string localfolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var array = localfolder.Split('\\');
            var username = array[2];
            string downloads = @"C:\Users\" + username + @"\Downloads\" + objKey.Substring(8);
            //var res = fileTransferUtility.OpenStream(_configuration.GetConnectionString("bucketName"), objKey);
            fileTransferUtility.Download(downloads, _configuration.GetConnectionString("bucketName"), objKey);
            fileTransferUtility.Dispose();

        }
    }
}
