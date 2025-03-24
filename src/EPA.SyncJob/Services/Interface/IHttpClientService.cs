using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPA.SyncJob.Services.Interface;

public interface IHttpClientService
{
    T ExecuteGet<T>(string baseUrl, string token = null, string data = null) where T : new();

    T ExecutePost<T>(string baseUrl, string token, string data) where T : new();

    Task<T> ExecuteGetAsync<T>(string baseUrl, string token = null, string data = null) where T : new();

    Task<T> ExecutePostAsync<T>(string baseUrl, string token, string data) where T : new();
}