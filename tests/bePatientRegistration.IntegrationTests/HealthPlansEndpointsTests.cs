using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using bePatientRegistration.Application.Patients.Dtos;
using Xunit;

namespace bePatientRegistration.IntegrationTests
{
    public class HealthPlansEndpointsTests : IClassFixture<ApiFactory>
    {
        private readonly HttpClient _client;

        public HealthPlansEndpointsTests(ApiFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetAll_ShouldReturnOk_AndAtLeastOneItem()
        {
            // Act
            var response = await _client.GetAsync("/api/HealthPlans");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var items = await response.Content.ReadFromJsonAsync<List<HealthPlanDto>>();

            Assert.NotNull(items);
            Assert.NotEmpty(items!);
        }

        [Fact]
        public async Task GetById_UnknownId_ShouldReturnNotFound()
        {
            // Act
            var response = await _client.GetAsync($"/api/HealthPlans/{Guid.NewGuid()}");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Create_ShouldReturnCreated_AndAllowGetById()
        {
            // Arrange
            var request = new CreateHealthPlanRequest
            {
                Name = "Plano Integração Teste"
            };

            // Act
            var createResponse = await _client.PostAsJsonAsync("/api/HealthPlans", request);

            // Assert
            Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);

            var created = await createResponse.Content.ReadFromJsonAsync<HealthPlanDto>();
            Assert.NotNull(created);
            Assert.NotEqual(Guid.Empty, created!.Id);
            Assert.Equal(request.Name, created.Name);

            // GET by id
            var getResponse = await _client.GetAsync($"/api/HealthPlans/{created.Id}");
            Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

            var byId = await getResponse.Content.ReadFromJsonAsync<HealthPlanDto>();
            Assert.NotNull(byId);
            Assert.Equal(created.Id, byId!.Id);
        }

        [Fact]
        public async Task Create_WithInvalidModel_ShouldReturnBadRequest()
        {
            // Name vazio viola [Required] / [MaxLength]
            var request = new CreateHealthPlanRequest
            {
                Name = string.Empty
            };

            var response = await _client.PostAsJsonAsync("/api/HealthPlans", request);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
