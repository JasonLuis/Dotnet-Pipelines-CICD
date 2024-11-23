Nesta atividade prática, o seu objetivo é implementar um novo teste que irá falhar, pois a funcionalidade ainda não foi criada na API, ou seja, estamos no processo de aplicação da metodologia do TDD. Então você deve definir um teste para oferta de maior desconto.Então, desafio aceito? Lembre-se das etapas:

Vamos à prática!

> Se tiver alguma dúvida, basta clicar em **Opinião do instrutor**.


Para resolução desta atividade prática você deve acessar a classe `OfertaViagem_GET` e adicionar um método novo:

```
 [Fact]
 public async Task Recuperar_OfertaViagem_Com_Maior_Desconto()
 {
    //Implementação
 }
```

Na escrita do método de testes podemos usar como base os métodos já definidos nesta classe, vamos continuar utilizando a estratégia de expor a base de dados, para termos certeza que exista alguma informação na base de teste.

Seguimos com a implementação do método de testes completo:

```
 [Fact]
 public async Task Recuperar_OfertaViagem_Com_Maior_Desconto()
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
     var response = await client.GetFromJsonAsync<OfertaViagem>("/ofertas-viagem/maior-desconto");

     //Assert
     Assert.NotNull(response);
 }

```

Definimos um novo método de teste, com base no TDD, e neste momento ele deve falhar. A próxima etapa é implementar a funcionalidade na API do Jornada Milhas. Então vá ao projeto da API e acesso a classe `OfertaViagemExtensions` onde iremos adicionar o novo endpoint:

```
app.MapGet("/ofertas-viagem/maior-desconto", async ([FromServices] OfertaViagemConverter converter, [FromServices] EntityDAL<OfertaViagem> entityDAL) =>
{
    var lista = await entityDAL.Listar();
    var oferta = lista.FirstOrDefault();
    if (oferta is null) return Results.NotFound();
    return Results.Ok(converter.EntityToResponse(oferta));
}).WithTags("Oferta Viagem").WithSummary("Obtem oferta de viagem com maior desconto.").WithOpenApi().RequireAuthorization();

```

Uma observação importante é que o objetivo aqui é simular o fluxo completo, por isso a função só retorna uma consulta genérica. 

Com a implementação realizada, execute os testes, commit as alterações para inicializar a pipeline de build e release no Azure Devops que deverá implantar o novo endpoint no ambiente configurado no portal do Azure.

Você pode conferir o código completo do que foi desenvolvido nesta aula acessando [o repositório no GitHub](https://github.com/alura-cursos/JornadaMilhas-CICD/tree/aula05-video5.2).