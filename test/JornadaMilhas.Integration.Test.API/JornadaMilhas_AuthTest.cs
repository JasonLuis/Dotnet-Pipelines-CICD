using JornadaMilhas.API.DTO.Auth;
using System.Net.Http.Json;
using System.Net;

namespace JornadaMilhas.Integration.Test.API
{
    public class JornadaMilhas_AuthTest : IClassFixture<JornadaMilhasWebApplicationFactory>
    {

        private readonly JornadaMilhasWebApplicationFactory app;

        public JornadaMilhas_AuthTest(JornadaMilhasWebApplicationFactory app)
        {
            this.app = app;
            //app.InitializeAsync().GetAwaiter().GetResult();
        }

        [Fact]
        public async Task POST_Efetua_Login_Com_Sucesso()
        {
            var user = new UserDTO { Email = "tester@email.com", Password = "Senha123@" };
            using var client = app.CreateClient();

            //Act
            var resultado = await client.PostAsJsonAsync("/auth-login", user);

            //Assert
            Assert.Equal(HttpStatusCode.OK, resultado.StatusCode);
        }

        [Fact]
        public async Task POST_Efetua_Login_Invalido()
        {
            //Arrange
            var user = new UserDTO { Email = "tester22@email.com", Password = "Senha123@22" };
            using var client = app.CreateClient();

            //Act
            var resultado = await client.PostAsJsonAsync("/auth-login", user);

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, resultado.StatusCode);
        }
    }
}