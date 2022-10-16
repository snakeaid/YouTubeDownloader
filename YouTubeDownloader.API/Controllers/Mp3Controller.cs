using Microsoft.AspNetCore.Mvc;
using YoutubeExplode;
using YoutubeExplode.Videos.Streams;

namespace YouTubeDownloader.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class Mp3Controller : ControllerBase
{
    private readonly YoutubeClient _youtubeClient;
    private readonly ILogger _logger;

    public Mp3Controller(YoutubeClient youtubeClient, ILogger<Mp3Controller> logger)
    {
        _youtubeClient = youtubeClient;
        _logger = logger;
    }

    [HttpPost("download")]
    public async Task<IActionResult> Download(string link)
    {
        var video = await _youtubeClient.Videos.GetAsync(link);
        _logger.LogInformation(video.Title);
        
        var streamManifest = await _youtubeClient.Videos.Streams.GetManifestAsync(link);
        var streamInfo = streamManifest
            .GetAudioOnlyStreams()
            .Where(s => s.Container == Container.Mp3)
            .GetWithHighestBitrate();
        //await _youtubeClient.Videos.Streams.DownloadAsync(streamInfo, $"video.{streamInfo.Container}");

        return Ok();
    }
}