using Amazon.S3;
using Amazon.S3.Model;
using BeatGrid.Data.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace BeatGrid.Data.Repositories
{
    public interface IPublicBeatRepository
    {
        Task<IEnumerable<BeatEntity>> GetBeats();
        void SaveBeatsAsync(IEnumerable<BeatEntity> beats);
    }

    public class PublicBeatRepository : IPublicBeatRepository
    {
        private const string BUCKET_NAME = "beatgrid-beats-public";
        private const string ALL_BEATS_KEY = "AllBeatsJson";

        private readonly IAmazonS3 _s3Client;

        public PublicBeatRepository(IAmazonS3 s3Client) => _s3Client = s3Client;

        public async Task<IEnumerable<BeatEntity>> GetBeats()
        {
            try
            {
                // Read beats from S3 (single json blob)
                // https://docs.aws.amazon.com/AmazonS3/latest/dev/RetrievingObjectUsingNetSDK.html
                var request = new GetObjectRequest
                {
                    BucketName = BUCKET_NAME,
                    Key = ALL_BEATS_KEY
                };

                using (var response = await _s3Client.GetObjectAsync(request))
                using (var responseStream = response.ResponseStream)
                using (var reader = new StreamReader(responseStream))
                {
                    var beatsJson = await reader.ReadToEndAsync();
                    var beats = JsonConvert.DeserializeObject<List<BeatEntity>>(beatsJson);
                    return beats;
                }
            }
            catch (Exception)
            {
                return new List<BeatEntity>();
            }
        }

        public async void SaveBeatsAsync(IEnumerable<BeatEntity> beats)
        {
            var request = new PutObjectRequest
            {
                BucketName = BUCKET_NAME,
                Key = "AllBeatsJson",
                ContentType = "application/json",
                ContentBody = JsonConvert.SerializeObject(beats)
            };
            await _s3Client.PutObjectAsync(request);
            
        }
    }
}
