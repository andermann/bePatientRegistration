//using System;
//using System.Collections.Generic;
//using System.Net;
//using System.Net.Http;
//using System.Net.Http.Json;
//using System.Threading.Tasks;
//using bePatientRegistration.Application.Patients.Dtos;
//using bePatientRegistration.Domain.Entities;
//using Xunit;

//namespace bePatientRegistration.IntegrationTests
//{
//    public class PatientsEndpointsTests : IClassFixture<ApiFactory>
//    {
//        private readonly HttpClient _client;

//        public PatientsEndpointsTests(ApiFactory factory)
//        {
//            _client = factory.CreateClient();
//        }

//        private async Task<Guid> GetAnyHealthPlanIdAsync()
//        {
//            var response = await _client.GetAsync("/api/HealthPlans");
//            response.EnsureSuccessStatusCode();

//            var plans = await response.Content.ReadFromJsonAsync<List<HealthPlanDto>>();

//            if (plans == null || plans.Count == 0)
//            {
//                throw new InvalidOperationException("Nenhum convênio disponível para criar pacientes de teste.");
//            }

//            return plans[0].Id;
//        }

//        [Fact]
//        public async Task GetAll_ShouldReturnOk()
//        {
//            var response = await _client.GetAsync("/api/Patients");

//            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

//            var items = await response.Content.ReadFromJsonAsync<List<PatientDto>>();
//            Assert.NotNull(items);
//        }

//        [Fact]
//        public async Task GetById_UnknownId_ShouldReturnNotFound()
//        {
//            var response = await _client.GetAsync($"/api/Patients/{Guid.NewGuid()}");

//            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
//        }

//        [Fact]
//        public async Task Create_ShouldReturnCreated_AndAllowGetById()
//        {
//            var healthPlanId = await GetAnyHealthPlanIdAsync();

//            var request = new CreatePatientRequest
//            {
//                FirstName = "Maria",
//                LastName = "Oliveira",
//                DateOfBirth = new DateTime(1985, 5, 20),
//                Gender = Gender.Female,
//                Cpf = "98765432100",
//                Rg = "RJ7654321",
//                UfRg = Uf.RJ,
//                Email = "maria.oliveira@example.com",
//                MobilePhone = "21988887777",
//                LandlinePhone = null,
//                HealthPlanId = healthPlanId,
//                HealthPlanCardNumber = "CARD-9999",
//                HealthPlanCardExpirationMonth = 12,
//                HealthPlanCardExpirationYear = 2030,
//                IsActive = true
//            };

//            var createResponse = await _client.PostAsJsonAsync("/api/Patients", request);

//            Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);

//            var created = await createResponse.Content.ReadFromJsonAsync<PatientDto>();
//            Assert.NotNull(created);
//            Assert.NotEqual(Guid.Empty, created!.Id);
//            Assert.Equal(request.FirstName, created.FirstName);
//            Assert.Equal(request.LastName, created.LastName);

//            var getResponse = await _client.GetAsync($"/api/Patients/{created.Id}");
//            Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

//            var byId = await getResponse.Content.ReadFromJsonAsync<PatientDto>();
//            Assert.NotNull(byId);
//            Assert.Equal(created.Id, byId!.Id);
//        }

//        [Fact]
//        public async Task Create_WithInvalidModel_ShouldReturnBadRequest()
//        {
//            var healthPlanId = await GetAnyHealthPlanIdAsync();

//            var request = new CreatePatientRequest
//            {
//                FirstName = "", // inválido
//                LastName = "Teste",
//                DateOfBirth = new DateTime(1995, 1, 1),
//                Gender = Gender.Male,
//                Cpf = "12345678901",
//                Rg = "RJ000000",
//                UfRg = Uf.RJ,
//                Email = "teste@example.com",
//                MobilePhone = "21999998888",
//                LandlinePhone = null,
//                HealthPlanId = healthPlanId,
//                HealthPlanCardNumber = "CARD-TESTE",
//                HealthPlanCardExpirationMonth = 12,
//                HealthPlanCardExpirationYear = 2030,
//                IsActive = true
//            };

//            var response = await _client.PostAsJsonAsync("/api/Patients", request);

//            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
//        }

//        [Fact]
//        public async Task Delete_ShouldReturnNoContent_AndAfterThat_NotFound()
//        {
//            var healthPlanId = await GetAnyHealthPlanIdAsync();

//            var request = new CreatePatientRequest
//            {
//                FirstName = "Carlos",
//                LastName = "Souza",
//                DateOfBirth = new DateTime(1992, 3, 10),
//                Gender = Gender.Male,
//                Cpf = "19575446003",
//                Rg = "RJ555555",
//                UfRg = Uf.RJ,
//                Email = "carlos.souza@example.com",
//                MobilePhone = "21977776666",
//                LandlinePhone = null,
//                HealthPlanId = healthPlanId,
//                HealthPlanCardNumber = "CARD-5555",
//                HealthPlanCardExpirationMonth = 12,
//                HealthPlanCardExpirationYear = 2030,
//                IsActive = true
//            };

//            var createResponse = await _client.PostAsJsonAsync("/api/Patients", request);
//            createResponse.EnsureSuccessStatusCode();

//            var created = await createResponse.Content.ReadFromJsonAsync<PatientDto>();
//            Assert.NotNull(created);

//            var deleteResponse = await _client.DeleteAsync($"/api/Patients/{created!.Id}");
//            Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

//            var getAfterDelete = await _client.GetAsync($"/api/Patients/{created.Id}");
//            Assert.Equal(HttpStatusCode.NotFound, getAfterDelete.StatusCode);
//        }
//    }
//}
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using bePatientRegistration.Application.Patients.Dtos;
using bePatientRegistration.Domain.Entities;
using Xunit;

namespace bePatientRegistration.IntegrationTests
{
    public class PatientsEndpointsTests : IClassFixture<ApiFactory>
    {
        private readonly HttpClient _client;

        public PatientsEndpointsTests(ApiFactory factory)
        {
            _client = factory.CreateClient();
        }

        private async Task<Guid> GetAnyHealthPlanIdAsync()
        {
            var response = await _client.GetAsync("/api/HealthPlans");
            response.EnsureSuccessStatusCode();

            var plans = await response.Content.ReadFromJsonAsync<List<HealthPlanDto>>();

            if (plans == null || plans.Count == 0)
            {
                throw new InvalidOperationException("Nenhum convênio disponível para criar pacientes de teste.");
            }

            return plans[0].Id;
        }

        [Fact]
        public async Task GetAll_ShouldReturnOk()
        {
            var response = await _client.GetAsync("/api/Patients");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var items = await response.Content.ReadFromJsonAsync<List<PatientDto>>();
            Assert.NotNull(items);
        }

        [Fact]
        public async Task GetById_UnknownId_ShouldReturnNotFound()
        {
            var response = await _client.GetAsync($"/api/Patients/{Guid.NewGuid()}");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Create_ShouldReturnCreated_AndAllowGetById()
        {
            var healthPlanId = await GetAnyHealthPlanIdAsync();

            var request = new CreatePatientRequest
            {
                FirstName = "Maria",
                LastName = "Oliveira",
                DateOfBirth = new DateTime(1985, 5, 20),
                Gender = Gender.Female,
                // CPF VÁLIDO
                Cpf = "11144477735",
                Rg = "RJ7654321",
                UfRg = Uf.RJ,
                Email = "maria.oliveira@example.com",
                MobilePhone = "21988887777",
                LandlinePhone = null,
                HealthPlanId = healthPlanId,
                HealthPlanCardNumber = "CARD-9999",
                HealthPlanCardExpirationMonth = 12,
                HealthPlanCardExpirationYear = 2030,
                IsActive = true
            };

            var createResponse = await _client.PostAsJsonAsync("/api/Patients", request);

            Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);

            var created = await createResponse.Content.ReadFromJsonAsync<PatientDto>();
            Assert.NotNull(created);
            Assert.NotEqual(Guid.Empty, created!.Id);
            Assert.Equal(request.FirstName, created.FirstName);
            Assert.Equal(request.LastName, created.LastName);

            var getResponse = await _client.GetAsync($"/api/Patients/{created.Id}");
            Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

            var byId = await getResponse.Content.ReadFromJsonAsync<PatientDto>();
            Assert.NotNull(byId);
            Assert.Equal(created.Id, byId!.Id);
        }

        [Fact]
        public async Task Create_WithInvalidModel_ShouldReturnBadRequest()
        {
            var healthPlanId = await GetAnyHealthPlanIdAsync();

            var request = new CreatePatientRequest
            {
                FirstName = "Nome", // inválido -> deve provocar 400
                LastName = "Teste",
                DateOfBirth = new DateTime(1995, 1, 1),
                Gender = Gender.Male,
                // CPF VÁLIDO (erro vem do FirstName vazio, não do CPF)
                Cpf = "93541134780",
                Rg = "RJ000000",
                UfRg = Uf.RJ,
                Email = "teste@example.com",
                MobilePhone = "21999998888",
                LandlinePhone = null,
                HealthPlanId = healthPlanId,
                HealthPlanCardNumber = "CARD-TESTE",
                HealthPlanCardExpirationMonth = 12,
                HealthPlanCardExpirationYear = 2030,
                IsActive = true
            };

            var response = await _client.PostAsJsonAsync("/api/Patients", request);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task Delete_ShouldReturnNoContent_AndAfterThat_NotFound()
        {
            var healthPlanId = await GetAnyHealthPlanIdAsync();

            var request = new CreatePatientRequest
            {
                FirstName = "Carlos",
                LastName = "Souza",
                DateOfBirth = new DateTime(1992, 3, 10),
                Gender = Gender.Male,
                // CPF VÁLIDO
                Cpf = "52998224725",
                Rg = "RJ555555",
                UfRg = Uf.RJ,
                Email = "carlos.souza@example.com",
                MobilePhone = "21977776666",
                LandlinePhone = null,
                HealthPlanId = healthPlanId,
                HealthPlanCardNumber = "CARD-5555",
                HealthPlanCardExpirationMonth = 12,
                HealthPlanCardExpirationYear = 2030,
                IsActive = true
            };

            var createResponse = await _client.PostAsJsonAsync("/api/Patients", request);
            //createResponse.EnsureSuccessStatusCode();

            var created = await createResponse.Content.ReadFromJsonAsync<PatientDto>();
            Assert.NotNull(created);

            var deleteResponse = await _client.DeleteAsync($"/api/Patients/{created!.Id}");
            Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

            var getAfterDelete = await _client.GetAsync($"/api/Patients/{created.Id}");
            Assert.Equal(HttpStatusCode.NotFound, getAfterDelete.StatusCode);
        }
    }
}

