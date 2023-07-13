using CoreBeliefsSurvey.Shared.Models;
using Microsoft.JSInterop;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace CoreBeliefsSurvey.Client.Services
{
    public class BeliefResponseService
    {
        private IJSRuntime _jsRuntime;

        public BeliefResponseService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task SaveBeliefResponses(List<CoreBeliefResponse> beliefResponses)
        {
            await _jsRuntime.InvokeVoidAsync("sessionStorage.setItem", "beliefResponses", JsonSerializer.Serialize(beliefResponses));
        }

        public async Task<List<CoreBeliefResponse>> LoadBeliefResponses()
        {
            var beliefResponses = new List<CoreBeliefResponse>();
        var beliefResponsesJson = await _jsRuntime.InvokeAsync<string>("sessionStorage.getItem", "beliefResponses");
            if (!string.IsNullOrEmpty(beliefResponsesJson))
            {
                beliefResponses = JsonSerializer.Deserialize<List<CoreBeliefResponse>>(beliefResponsesJson);
            }
            else
            {
                beliefResponses = new List<CoreBeliefResponse>();
            }
            return beliefResponses;
        }

        // Retrieve filteredBeliefs from session storage
        public async Task<List<CoreBelief>> LoadFilteredBeliefs()
        {
            List<CoreBelief> filteredBeliefs = new List<CoreBelief>();

            var filteredBeliefsJson = await _jsRuntime.InvokeAsync<string>("sessionStorage.getItem", new string[] { "filteredBeliefs" });
            if (!string.IsNullOrEmpty(filteredBeliefsJson))
            {
                filteredBeliefs = JsonSerializer.Deserialize<List<CoreBelief>>(filteredBeliefsJson);
            }
            if (filteredBeliefs == null)
            {
                filteredBeliefs = new List<CoreBelief> { };
            }
            return filteredBeliefs;
        }
        public async Task RemoveBeliefResponses()
        {
            await _jsRuntime.InvokeVoidAsync("sessionStorage.removeItem", "beliefResponses");
        }

        public async Task RemoveFilteredBeliefs()
        {
            await _jsRuntime.InvokeVoidAsync("sessionStorage.removeItem", "filteredBeliefs");
        }

        public async Task SaveFilteredBeliefs(List<CoreBelief> filteredBeliefs)
        {
            await _jsRuntime.InvokeVoidAsync("sessionStorage.setItem", "filteredBeliefs", JsonSerializer.Serialize(filteredBeliefs));
        }
        public async Task UpdateBeliefResponses(List<CoreBeliefResponse> beliefResponses)
        {
            await _jsRuntime.InvokeVoidAsync("sessionStorage.setItem", "beliefResponses", JsonSerializer.Serialize(beliefResponses));
        }

    }

}
