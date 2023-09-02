using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Gs.LOL
{
    public class LeagueClient
    {
        private HttpClient _http;
        private readonly ILogger<LeagueClient> _logger;

        public LeagueClient(HttpClient client, ILogger<LeagueClient> logger)
        {
            _http = client;
            _logger = logger;
        }

        public async Task<Dictionary<string, object>> GameflowSessionGetAsync()
        {
            var rt = await _http.GetFromJsonAsync<Dictionary<string, object>>($"lol-gameflow/v1/session");
            return rt;
        }
        public async Task<Dictionary<string, object>> PerksRecommendedGetAsync(object championId)
        {
            var rt = await _http.GetFromJsonAsync<Dictionary<string, object>>($"https://www.wegame.com.cn/lol/resources/js/champion/recommend/{championId}.js");
            return rt;
        }
        public async Task<Dictionary<string, object>> ChampSelectSessionGetAsync()
        {
            var rt = await _http.GetFromJsonAsync<Dictionary<string, object>>("/lol-champ-select/v1/session");
            return rt;
        }

        public async Task<bool> HeroLockAsync(HeroLockRequest heroLockRequest)
        {
            var body = new
            {
                completed = true,
                type = heroLockRequest.Type,
                championId = heroLockRequest.ChampionId
            };
            var content = new StringContent(JsonSerializer.Serialize(body));
            var rsp = await _http.PatchAsync($"/lol-champ-select/v1/session/actions/{heroLockRequest.ActionId}", content);
            return rsp.IsSuccessStatusCode;
        }

        public async Task<IEnumerable<Dictionary<string, object>>> ChatConversationsGetAsync()
        {
            var rt = await _http.GetFromJsonAsync<IEnumerable<Dictionary<string, object>>>("lol-chat/v1/conversations");
            return rt;
        }
        public async Task<bool> ChatConversationMessageSendAsync(object accountId, string message)
        {
            var body = new
            {
                body = message,
                type = "chat"
            };
            var rsp = await _http.PostAsJsonAsync($"/lol-chat/v1/conversations/{accountId}/messages", body);
            return rsp.IsSuccessStatusCode;
        }
        public async Task<Dictionary<string, object>> SummonerQueryAsync(string summonerName)
        {
            var rt = await _http.GetFromJsonAsync<Dictionary<string, object>>($"lol-summoner/v1/summoners?name={summonerName}");
            return rt;
        }

        [Obsolete]
        public async Task<Dictionary<string, object>> RankStatsGetAsync(object puuid)
        {
            var rt = await _http.GetFromJsonAsync<Dictionary<string, object>>($"/lol-ranked/v1/ranked-stats/{puuid}");
            return rt;
        }
        public async Task<Dictionary<string, object>> RankStatsGetAsync()
        {
            var rt = await _http.GetFromJsonAsync<Dictionary<string, object>>("/lol-ranked/v1/current-ranked-stats");
            return rt;
        }
        public async Task<bool> GameAcceptAsync()
        {
            var rsp = await _http.PostAsync("/lol-matchmaking/v1/ready-check", null);
            return rsp.IsSuccessStatusCode;
        }

        public async Task<bool> PerkDeleteAsync(object id)
        {
            var rsp = await _http.DeleteAsync($"/lol-perks/v1/pages/{id}");
            return rsp.IsSuccessStatusCode;
        }
        public async Task<IEnumerable<Dictionary<string, object>>> PerkDeletableGetAsync()
        {
            var perks = await PerkGetAsync();
            perks = perks.Where(i => (i["isDeletable"].ToString().Equals("True")));
            return perks;
        }
        public async Task<IEnumerable<Dictionary<string, object>>> PerkGetAsync()
        {
            var rt = await _http.GetFromJsonAsync<IEnumerable<Dictionary<string, object>>>("lol-perks/v1/pages");
            return rt;
        }
        public async Task<bool> PerkAddAsync(PerkAddRequest perkAddRequest)
        {
            var rsp = await _http.PostAsJsonAsync("/lol-perks/v1/pages", perkAddRequest);
            return rsp.IsSuccessStatusCode;
        }
        public async Task<Dictionary<string, object>> CurrentPerkGetAsync()
        {
            var rt = await _http.GetFromJsonAsync<Dictionary<string, object>>("/lol-perks/v1/currentpage");
            return rt;
        }

        [Obsolete]
        public async Task<bool> UxLaunchAsync()
        {
            var rsp = await _http.PostAsync("riotclient/launch-ux", null);
            return rsp.IsSuccessStatusCode;
        }
        [Obsolete]
        public async Task<bool> UxKillAsync()
        {
            var rsp = await _http.PostAsync("riotclient/kill-ux", null);
            return rsp.IsSuccessStatusCode;
        }

        public async Task<bool> UxMinimizeAsync()
        {
            var rsp = await _http.PostAsync("riotclient/ux-minimize", null);
            return rsp.IsSuccessStatusCode;
        }
        public async Task<bool> UxShowAsync()
        {
            var rsp = await _http.PostAsync("riotclient/ux-show", null);
            return rsp.IsSuccessStatusCode;
        }

        public async Task<LoginSession> LoginSessionGetAsync()
        {
            var rt = await _http.GetFromJsonAsync<LoginSession>("/lol-login/v1/session");
            return rt;
        }

        public async Task<Summoner> SummonerGetAsync()
        {
            var rt = await _http.GetFromJsonAsync<Summoner>("/lol-summoner/v1/current-summoner");
            return rt;
        }

    }

    public class HeroLockRequest
    {
        public string Type { get; set; }
        public string ChampionId { get; set; }
        public string ActionId { get; set; }
    }

    public class PerkAddRequest
    {
        public string name { get; set; }
        public bool current { get; set; }
        public int primaryStyleId { get; set; }
        public int subStyleId { get; set; }
        public int[] selectedPerkIds { get; set; }
    }

    public class LoginSession
    {
        public long accountId { get; set; }
        public bool connected { get; set; }
        public object error { get; set; }
        public string idToken { get; set; }
        public bool isInLoginQueue { get; set; }
        public bool isNewPlayer { get; set; }
        public string puuid { get; set; }
        public string state { get; set; }
        public long summonerId { get; set; }
        public string userAuthToken { get; set; }
        public string username { get; set; }
    }

    public class Summoner
    {
        public long accountId { get; set; }
        public string displayName { get; set; }
        public string internalName { get; set; }
        public bool nameChangeFlag { get; set; }
        public int percentCompleteForNextLevel { get; set; }
        public string privacy { get; set; }
        public int profileIconId { get; set; }
        public string puuid { get; set; }
        public Rerollpoints rerollPoints { get; set; }
        public long summonerId { get; set; }
        public int summonerLevel { get; set; }
        public bool unnamed { get; set; }
        public int xpSinceLastLevel { get; set; }
        public int xpUntilNextLevel { get; set; }
    }

    public class Rerollpoints
    {
        public int currentPoints { get; set; }
        public int maxRolls { get; set; }
        public int numberOfRolls { get; set; }
        public int pointsCostToRoll { get; set; }
        public int pointsToReroll { get; set; }
    }
}

