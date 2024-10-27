Neste vídeo, testaremos **outros cenários da consulta paginada**. Isso é importante para aumentar a cobertura dos testes e melhorar a qualidade da aplicação.

## Dados na última página

Começaremos copiando o último teste criado, de `Recuperar_OfertasViagens_Na_Consulta_Paginada` no arquivo `OfertaViagem_GET.cs`, para usá-lo como base para testar a **consulta na última página**.

### Criando o método `Recuperar_OfertasViagens_Na_Consulta_Ultima_Pagina()`

Imagine o seguinte cenário: teremos quatro páginas (`int pagina = 4`) e, na última página, queremos 25 registros por página (`int tamanhoPorPagina = 25`). Como cadastramos 80 ofertas de viagens na base, esperamos ter 25 registros na primeira página, 25 na segunda, 25 na terceira e 5 na última. 

Como nossa expectativa é ter cinco registros na última página, vamos passar o valor 5 no lugar de `tamanhoPorPagina` no método `Assert.Equal()` da linha 88.

> *`OfertaViagem_GET.cs`*

```cs
// código omitido

[Fact]
public async Task Recuperar_OfertasViagens_Na_Consulta_Ultima_Pagina()
{
    //Arrange
    var ofertaDataBuilder = new OfertaViagemDataBuilder();
    var listaDeOfertas = ofertaDataBuilder.Generate(80);
    app.Context.OfertasViagem.AddRange(listaDeOfertas);
    app.Context.SaveChanges();

    using var client = await app.GetClientWithAccessTokenAsync();

    int pagina = 4;
    int tamanhoPorPagina = 25;

    //Act
    var response = await client.GetFromJsonAsync<ICollection<OfertaViagem>>($"/ofertas-viagem?pagina={pagina}&tamanhoPorPagina={tamanhoPorPagina}");

    //Assert
    Assert.True(response != null);
    Assert.Equal(5, response.Count());

}
```

### Realizando o teste

Após salvar o arquivo, podemos executar o teste. Com o **gerenciador de testes** aberto na lateral esquerda, vamos clicar sobre `Recuperar_OfertasViagens_Na_Consulta_Ultima_Pagina` com o botão direito e selecionar a opção "Executar". Nesse caso, o teste não passou. Vamos entender o porquê? 

A expectativa era que, na última página, fossem obtidos 5 registros, mas obtivemos 25. Sendo assim, precisamos retornar e analisar novamente o teste criado. 

Esse novo teste de consulta na última página foi executado logo após a consulta paginada, onde cadastramos 80 registros, isto é, 80 ofertas de viagens. Quando realizamos a nova consulta para a última página, **cadastramos novamente esses 80 registros**, o que bagunçou a base de dados para fazer o teste. É essencial sempre prestar atenção nisso, mas como resolveríamos essa questão? 

### Refatorando o método `Recuperar_OfertasViagens_Na_Consulta_Ultima_Pagina()`

Com acesso à base de dados, podemos usá-la para aplicar alguma estratégia. Precisamos garantir que a base de dados tenha 80 registros. Não podemos ter 1.000 registros, ou 50 registros, mas exatamente 80. 

Como temos acesso à nossa base de dados, vamos limpar essa tabela, executar o cadastro, e depois realizar a consulta paginada com os parâmetros que queremos testar. São eles: 

> * **4 páginas** (`int pagina = 4`);
> * E **25 registros** por página (`int tamanhoPorPagina = 25`);
> * Para obter **5 registros** na última página (`Assert.Equal(5, response.Count())`).

Na seção ***arrange***, antes de tudo, precisamos garantir que temos uma **base limpa**. Para isso, vamos adicionar o seguinte código na linha 74:

> *`OfertaViagem_GET.cs`*

```cs
// código omitido

//Arrange
app.Context.Database.ExecuteSqlRaw("Delete from OfertasViagem");

var ofertaDataBuilder = new OfertaViagemDataBuilder();
var listaDeOfertas = ofertaDataBuilder.Generate(80);
app.Context.OfertasViagem.AddRange(listaDeOfertas);
app.Context.SaveChanges();

using var client = await app.GetClientWithAccessTokenAsync();

int pagina = 4;
int tamanhoPorPagina = 25;

// código omitido
```

Dessa forma, limpamos a tabela após o cadastro das 80 ofertas de viagens, e com isso, teremos a garantia de que haverá 80 registros para fazer a consulta da forma que esperamos. 

Uma vez salvo o arquivo, podemos executar novamente o teste `Recuperar_OfertasViagens_Na_Consulta_Ultima_Pagina`. Agora ele deve passar conforme esperado.

## Conclusão

Conseguimos fazer o teste passar, mas ainda há um detalhe muito importante: acessamos a base de dados pois expusemos o contexto, e essa estratégia deve ser utilizada com bastante cautela. 

Nós executamos em um **ambiente controlado**, isto é, executamos o banco de dados no container *Docker* ou em um ambiente local, instalado na própria máquina para realizar os testes. 

A cautela é necessária, pois se compartilharmos essa base de teste com outras pessoas desenvolvedoras, podemos bagunçar nosso teste e o teste de outras pessoas também. 

> **Cautela** também é uma estratégia.

Na sequência, continuaremos testando outros **cenários da consulta paginada**, nos quais é importante prestarmos atenção.