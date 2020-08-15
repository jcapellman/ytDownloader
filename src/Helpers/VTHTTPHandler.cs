﻿using System.Net.Http;
using System.Threading.Tasks;

using VTDownloader.Enums;
using VTDownloader.Objects;

namespace VTDownloader.Helpers
{
    public class VTHTTPHandler
    {
        public static async Task<DownloadResponseItem> DownloadAsync(string vtKey, string hash)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var file = await httpClient.GetByteArrayAsync(
                        $"https://www.virustotal.com/vtapi/v2/file/download?apikey={vtKey}&hash={hash}");

                    if (file == null)
                    {
                        return new DownloadResponseItem(DownloadResponseStatus.CANNOT_CONNECT_TO_VT);
                    }

                    return new DownloadResponseItem(file);
                }
            } catch (HttpRequestException requestException)
            {
                switch (requestException.StatusCode)
                {
                    case System.Net.HttpStatusCode.Forbidden:
                        return new DownloadResponseItem(DownloadResponseStatus.INVALID_VT_KEY);
                    default:
                        return new DownloadResponseItem(DownloadResponseStatus.UNEXPECTED_HTTP_ERROR, requestException);
                }
            }
        }
    }
}