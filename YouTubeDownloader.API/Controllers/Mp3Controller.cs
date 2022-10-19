using System.Net;
using Microsoft.AspNetCore.Mvc;
using YoutubeExplode;
using YoutubeExplode.Videos.Streams;
using YoutubeExplode.Converter;

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
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<IActionResult> Download(string link)
    {
        var video = await _youtubeClient.Videos.GetAsync(link);
        _logger.LogInformation(video.Title);

        var streamManifest = await _youtubeClient.Videos.Streams.GetManifestAsync(link);
        var streamInfo = streamManifest.GetAudioOnlyStreams().GetWithHighestBitrate();
        var stream = await _youtubeClient.Videos.Streams.GetAsync(streamInfo);

        return File(stream, "application/mp3", $"{video.Title}.mp3");
    }
}