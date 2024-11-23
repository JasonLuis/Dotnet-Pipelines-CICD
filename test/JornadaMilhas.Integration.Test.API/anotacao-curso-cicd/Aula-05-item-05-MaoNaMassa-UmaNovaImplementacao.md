Nesta atividade prática, o seu objetivo e o da equipe de desenvolvimento do Jornada Milhas é criar uma nova funcionalidade para retornar a última oferta cadastrada na API, e aplicar todo o ciclo de CI/CD, ou seja, iniciando com os testes, implementação da funcionalidade e a publicação de mais um endpoint na API por meio da pipeline criada. Então, aceita o desafio? Lembre-se das etapas:

* Criar a função de teste para  consultar a última oferta cadastrada.
* Implementação do endpoint.

Vamos à prática então!

> Se tiver alguma dúvida, basta clicar em **Opinião do instrutor**.


Então, colocando as mãos no código nesta prática temos:

### Criação do método de teste

Na classe de teste **`OfertaViagem_GET`** vamos adicionar o método de teste:

```
[Fact]
public async Task Recuperar_Ultima_OfertaViagem_Cadastrada()
{
    //Arrange  
    var ofertaExistente = app.Context.OfertasViagem.FirstOrDefault();
    if (ofertaExistente is null)
    {
        ofertaExistente = new OfertaViagem()
        {
            Preco = 100,
            Rota = new Rota("Origem", "Destino"),
            Periodo = new Periodo(DateTime.Parse("2024-03-03"), DateTime.Parse("2024-03-06"))
        };
        app.Context.Add(ofertaExistente);
        app.Context.SaveChanges();
    }

    using var client = await app.GetClientWithAccessTokenAsync();

    //Act
    var response = await client.GetFromJsonAsync<OfertaViagem>("/ofertas-viagem/ultima-oferta");

    //Assert
    Assert.NotNull(response);
}

```

Neste momento o teste irá falhar, pois o endpoint para teste ainda não foi implementado,lembre-se estamos utilizando uma abordagem de TDD. na etapa seguinte vamos implementar a funcionalidade para o teste executar com sucesso.

### Implementação do endpoint 

Nesta etapa vamos a implementação do endpoint  **`/ofertas-viagem/ultima-oferta`** no porjeto de API da solução, sendo assim na classe **`OfertaViagemExtensions`**,implementamos:

```
 app.MapGet("/ofertas-viagem/ultima-oferta", async ([FromServices] OfertaViagemConverter converter, [FromServices] EntityDAL<OfertaViagem> entityDAL) =>
 {
     var oferta = await entityDAL.UltimoRegistroAsync();
     if (oferta is null) return Results.NotFound();
     return Results.Ok(converter.EntityToResponse(oferta));
 }).WithTags("Oferta Viagem").WithSummary("Obtem última oferta de viagem cadastrada.").WithOpenApi().RequireAuthorization();

```

Para essa implementação ser efetiva estamos utilizando a função genérica **`UltimoRegistroAsync`** presente na classe **`EntityDAL`** presente na camada de dados da solução. Abaixo a implementação deste método:

```
public async Task<T?> UltimoRegistroAsync(Expression<Func<T, bool>> condicao = null)
{
    IQueryable<T> query = context.Set<T>();
    if (condicao != null)
    {
        query = query.Where(condicao);
    }
    return await query.OrderByDescending(x => EF.Property<int>(x, "Id")).FirstOrDefaultAsync();
}

```

Uma observação importante é caso você queira executar o projeto em seu ambiente local, lembre-se de configurar a string de conexão no arquivo **`appsettings.json`** no projeto de API.

Agora, você pode executar os testes e a partir do commit iniciar sua pipeline de CI/CD para visualizar a nova demanda em seu ambiente de produção.

Você pode conferir o código completo do que foi desenvolvido nesta aula acessando [o repositório no GitHub](https://github.com/alura-cursos/JornadaMilhas-CICD/tree/aula05-video5.2-FacaComoEuFiz).