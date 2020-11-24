using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using Company.WorkflowSystem.Infrastructure.EmsIntegration.Endpoint;
using Company.WorkflowSystem.Domain.Interfaces.EmsTradepoint;
using System.Linq;
using System.Globalization;
using Company.WorkflowSystem.Domain.Util;

namespace Company.WorkflowSystem.Infrastructure.EmsIntegration
{
    public class EmsTradepointService : IEmsTradepointService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;
        public EmsTradepointService(IHttpClientFactory clientFactory, IConfiguration configuration)
        {
            _clientFactory = clientFactory;
            _configuration = configuration;
        }
        public async Task<EmsTradepointFetchTradesResponse> FetchTrades(DateTimeOffset createdFrom, DateTimeOffset createdTo)
        {
            var url = _configuration.GetValue<string>("EmsTradepoint:Url");
            string token = await Authenticate();

            try
            {
                var httpClient = _clientFactory.CreateClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                var client = new EmsTradepointClient(httpClient) { BaseUrl = url };

                var trades = await client.GetV1TradesAsync(
                    owner: Owner.Company_trades,
                    created_from: JsonConvert.SerializeObject(createdFrom),
                    created_to: JsonConvert.SerializeObject(createdTo),
                    voided: Voided.No,
                    deal_category: null,
                    delivery_end: null,
                    delivery_start: null,
                    seller_company_id: null,
                    buyer_company_id: null,
                    broker: null,
                    per_page: null,
                    page: null,
                    market_category: null
                    );

                var payload = JsonConvert.SerializeObject(trades);

                return new EmsTradepointFetchTradesResponse
                {
                    Trades = trades.Select(t =>
                    {
                        var buy = t.Buyer != null;
                        var trader = (buy ? t.Buyer : t.Seller);
                        var position = (buy ? EmsTradePositionEnum.Buy : EmsTradePositionEnum.Sell);

                        return new DealItemFromEmsTrade
                        {
                            TradeId = t.Id,
                            StartDate = t.Deal.Delivery_start.LocalDateStringToLocalDate("yyyy-MM-dd"),
                            EndDate = t.Deal.Delivery_end.LocalDateStringToLocalDate("yyyy-MM-dd"),
                            Quantity = t.Quantity,
                            Price = t.Price,
                            Position = position,
                            CreationDate = DateTimeOffset.Parse(t.Created_at).ToLocalTimeZone(),
                            TraderId = trader.Id,
                            TraderName = $"{trader.First_name} {trader.Last_name}",
                        };
                    }).ToList(),
                    Payload = payload,
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching from EmsTradepoint: {ex.ToString()} - URL: {url}", ex);
            }
        }

        private async Task<string> Authenticate()
        {
            var authUrl = _configuration.GetValue<string>("EmsTradepoint:OAuthUrl");
            try
            {
                var authRequest = new HttpRequestMessage(HttpMethod.Post, authUrl);

                var nvc = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("client_id", _configuration.GetValue<string>("EmsTradepoint:ClientId")),
                    new KeyValuePair<string, string>("secret_id", _configuration.GetValue<string>("EmsTradepoint:SecretId")),
                    new KeyValuePair<string, string>("grant_type", "password"),
                    new KeyValuePair<string, string>("username", _configuration.GetValue<string>("EmsTradepoint:Username")),
                    new KeyValuePair<string, string>("password", _configuration.GetValue<string>("EmsTradepoint:Password"))
                };

                authRequest.Content = new FormUrlEncodedContent(nvc);

                var client = _clientFactory.CreateClient();

                var response = await client.SendAsync(authRequest);
                var stringContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    var errorResponse = JsonConvert.DeserializeObject<EmsTradepointAuthErrorResponse>(stringContent);
                    throw new Exception($"Error: {errorResponse.error}; Error Description: {errorResponse.error_description}");
                }

                var successResponse = JsonConvert.DeserializeObject<EmsTradepointAuthResponse>(stringContent);

                return successResponse.access_token;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error authenticating to EmsTradepoint: {ex.ToString()} - URL: {authUrl}", ex);
            }
        }
    }

}