using JornadaMilhas.Dominio.Entidades;
using System.Net;
using System.Net.Http.Json;

namespace JornadaMilhas.Integration.Test.API;

public class Rota_DELETE : IClassFixture<JornadaMilhasWebApplicationFactory>
{
    private readonly JornadaMilhasWebApplicationFactory app;

    public Rota_DELETE(JornadaMilhasWebApplicationFactory app)
    {
        this.app = app;
    }

    [Fact]
    public async Task Deletar_Rota_PorId()
    {
        //Arrange
        var rotaExistente = app.Context.Rota.FirstOrDefault();
        if(rotaExistente is null)
        {
            rotaExistente = new Rota()
            {
                Origem = "Arapiraca",
                Destino = "São Paulo"
            };

            app.Context.Add(rotaExistente);
            app.Context.SaveChanges();
        }

        using var client = await app.GetClientWithAccessTokenAsync();

        //Act
        var response = await client.DeleteAsync("/rota-viagem/" + rotaExistente.Id);

        //Assert
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        
    }
}
