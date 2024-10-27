Neste vídeo, vamos dar continuidade à saga de escrever testes para a **consulta paginada** da API do Jornada Milhas. 

## Página não existente

Imagine o seguinte cenário: nossa API será disponibilizada para o público e, em determinado momento, alguma aplicação, seja mobile ou web, irá consumir essa API. Imagine que essa aplicação tenha uma seção, um formulário, ou até mesmo uma página, que trabalhará com **filtros** para a consulta paginada. 

Suponha que vamos informar para essa aplicação um filtro para retornar **5 páginas**. No entanto, a base de dados não nos permite entregar 5 páginas para a aplicação cliente. Nesse caso, como a aplicação irá se comportar? Isto é, como nossa API irá se comportar? 

O ideal é que isso esteja **documentado**, seja na API, seja via *Swagger*, ou de qualquer outra forma. Porém, não temos garantias de que essa documentação estará **atualizada**. 

Dito isso, vamos escrever um teste para entendermos esses comportamentos, tanto da API, quanto o que a aplicação pode esperar de resultado para uma **página inexistente**. 

### Criando o método `Recuperar_OfertasViagens_Na_Consulta_Com_Pagina_Inexistente()`

Vamos reaproveitar mais uma vez o último teste criado. Basta copiá-lo e colar logo abaixo, a partir da linha 94. Porém, o teste agora será para recuperar ofertas de viagem na consulta de uma página inexistente (método `Recuperar_OfertasViagens_Na_Consulta_Com_Pagina_Inexistente()`). 

Se deletarmos a tabela `OfertasViagem`, conforme indicado na linha 98, e cadastrarmos 80 registros, sabemos que teremos pelo menos **4 páginas**. Assim, sabemos que uma quinta página não existe. 

Dito isso, o que esperamos que seja retornado como expectativa quando executarmos o ***act***? Em vez de 5, como declarado em `Assert.Equal()`, esperamos que retorne uma lista vazia, então passamos 0.

> *`OfertaViagem_GET.cs`:*

```cs
// código omitido

[Fact]
public async Task Recuperar_OfertasViagens_Na_Consulta_Com_Pagina_Inexistente()
{
    //Arrange
    app.Context.Database.ExecuteSqlRaw("Delete from OfertasViagem");

    var ofertaDataBuilder = new OfertaViagemDataBuilder();
    var listaDeOfertas = ofertaDataBuilder.Generate(80);
    app.Context.OfertasViagem.AddRange(listaDeOfertas);
    app.Context.SaveChanges();

    using var client = await app.GetClientWithAccessTokenAsync();

    int pagina = 5;
    int tamanhoPorPagina = 25;

    //Act
    var response = await client.GetFromJsonAsync<ICollection<OfertaViagem>>($"/ofertas-viagem?pagina={pagina}&tamanhoPorPagina={tamanhoPorPagina}");

    //Assert
    Assert.True(response != null);
    Assert.Equal(0, response.Count());
}
```

Feito isso, podemos salvar as alterações e executar o teste. No **gerenciador de testes** à esquerda, vamos clicar sobre o teste `Recuperar_OfertasViagens_Na_Consulta_Com_Pagina_Inexistente` e selecionar a opção "Executar". O teste passou como esperado, então sabemos que, para uma página inexistente, o comportamento da API é retornar uma lista vazia. 

## Conclusão

Com isso, criamos mais um teste, mais um cenário importante a ser testado na nossa API quando falamos em consulta paginada. Porém, ainda existem **outros cenários**. Vamos abordar mais um na sequência!