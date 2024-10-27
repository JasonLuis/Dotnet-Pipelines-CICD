Neste vídeo, vamos testar mais um cenário importante na consulta paginada: quando pessoas usuárias inserem valores indevidos ao utilizar uma aplicação. 

## Página negativa

Imagine a seguinte situação: nossa API foi disponibilizada para o público e, em algum momento, alguém na aplicação quis informar um **valor negativo** na variável `pagina` para fazer uma consulta paginada.

Qual é o comportamento esperado da aplicação nesse caso? Será gerado um erro 400? Ou uma exceção? Para descobrir, poderíamos recorrer à documentação, usando *Swagger* ou qualquer outro tipo de documentação disponível para a aplicação, porém, vamos fazer um **teste** para isso.

### Criando o método `Recuperar_OfertasViagens_Na_Consulta_Com_Pagina_Com_Valor_Negativo()`

Para esse novo teste, utilizaremos o último teste criado como referência: vamos copiá-lo e colar logo abaixo, a partir da linha 118. O objetivo será passar um valor negativo para a variável `pagina`, na linha 131, então passaremos o valor -5.

Além disso, vamos dar um nome diferente ao método, que será `Recuperar_OfertasViagens_Na_Consulta_Com_Pagina_Com_Valor_Negativo()`. Neste teste, esperamos que seja retornada uma lista vazia, então vamos manter o valor 0 em `Assert.Equal()`. 

> *`OfertaViagem_GET.cs`:*

```cs
// código omitido

[Fact]
public async Task Recuperar_OfertasViagens_Na_Consulta_Com_Pagina_Com_Valor_Negativo()
{
    //Arrange
    app.Context.Database.ExecuteSqlRaw("Delete from OfertasViagem");

    var ofertaDataBuilder = new OfertaViagemDataBuilder();
    var listaDeOfertas = ofertaDataBuilder.Generate(80);
    app.Context.OfertasViagem.AddRange(listaDeOfertas);
    app.Context.SaveChanges();

    using var client = await app.GetClientWithAccessTokenAsync();

    int pagina = -5;
    int tamanhoPorPagina = 25;

    //Act
    var response = await client.GetFromJsonAsync<ICollection<OfertaViagem>>($"/ofertas-viagem?pagina={pagina}&tamanhoPorPagina={tamanhoPorPagina}");

    //Assert
    Assert.True(response != null);
    Assert.Equal(0, response.Count());

}
```

Após salvar o arquivo, vamos abrir o **gerenciador de testes** na lateral esquerda. Ao executar o teste `Recuperar_OfertasViagens_Na_Consulta_Com_Pagina_Com_Valor_Negativo`, percebemos que ele não passa, e além disso, ele retorna uma exceção. Nesse caso, precisaremos **criar mais um teste** para cobrir essa exceção que pode ser gerada. 

### Refatorando o novo método

De volta ao método `Recuperar_OfertasViagens_Na_Consulta_Com_Pagina_Com_Valor_Negativo()`, vamos remover os códigos de *act* e *assert* e trabalhar com **exceções**. Sendo assim, teremos uma seção de act mais assert. A partir da linha 135, teremos o seguinte trecho de código: 

> *`OfertaViagem_GET.cs`:*

```cs
// código omitido

//Act + Assert
await Assert.ThrowsAsync<HttpRequestException>(async () =>
{

    var response = await client.GetFromJsonAsync<ICollection<OfertaViagem>>($"/ofertas-viagem?pagina={pagina}&tamanhoPorPagina={tamanhoPorPagina}");
});
```

Primeiro, fazemos um `Assert.ThrowsAsync()`, recurso do *xUnit* para realizar testes com exceções. A exceção gerada é o `HttpRequestException`. Por se tratar de uma consulta **assíncrona**, passamos o `async` na linha 135, e depois realizamos a consulta, com o mesmo trecho de código de antes. 

Nossa expectativa é que, ao passar um valor negativo e realizar o act, seja tratada uma exceção. 

### Realizando o teste

Com as alterações salvas, vamos executar o teste novamente. Com o gerenciador de testes aberto mais uma vez, clicamos com botão direito sobre `Recuperar_OfertasViagens_Na_Consulta_Com_Pagina_Com_Valor_Negativo` e selecionamos "Executar".

Agora o teste passou conforme esperado. A partir disso, concluímos o seguinte: quando testamos um cenário onde a pessoa usuária passa um valor negativo para a variável `pagina`, nossa API retorna uma exceção, a qual estamos tratando. 

É importante **reportar esse comportamento** da API para as pessoas desenvolvedoras responsáveis pela manutenção, para que essa situação seja corrigida, trabalhada, e tratada da melhor forma. 

Após a pessoa desenvolvedora corrigir essa questão da API, o teste começará a falhar. Sendo assim, podemos dizer que esse teste é **temporário**. 

Uma vez que corrigimos a API e tratamos da maneira adequada a situação do valor negativo em `pagina`, podemos refazer o teste para a situação esperada, que seria um erro de código 400, por exemplo. 

## Conclusão

Ao longo dessa aula, exploramos mais alguns testes referentes à **consulta paginada** e utilizamos algumas estratégias para deixar os métodos de teste mais **independentes**, além de trabalhar com volumes de dados maiores de maneira mais inteligente. 

Para isso, recorremos ao acesso direto à **base de dados**, e também utilizamos a biblioteca ***Bogus*** para gerar essa massa de dados. Assim, trabalhamos cenários-chave na consulta paginada. 

Na sequência, vamos propor alguns **desafios** para você, com o objetivo de trabalhar em uma nova API tudo o que abordamos até o momento!