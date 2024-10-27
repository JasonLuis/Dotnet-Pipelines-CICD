using JornadaMilhas.Dominio.Entidades;
using System.Net;
using System.Net.Http.Json;

namespace JornadaMilhas.Integration.Test.API;

public class Rota_POST : IClassFixture<JornadaMilhasWebApplicationFactory>
{
    private readonly JornadaMilhasWebApplicationFactory app;

    public Rota_POST(JornadaMilhasWebApplicationFactory app)
    {
        this.app = app;
    }

    [Fact]
    public async Task Cadastra_Rota_PorId()
    {
        //Arrange
        using var client = await app.GetClientWithAccessTokenAsync();


        var rotaExistente = new Rota()
        {
            Origem = "Arapiraca",
            Destino = "São Paulo"
        };

        //Act
        var response = await client.PostAsJsonAsync($"/rota-viagem", rotaExistente);

        //Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }
}
