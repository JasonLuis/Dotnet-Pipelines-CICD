Neste vídeo, vamos continuar escrevendo testes para o Jornada Milhas, mas daremos atenção especial à **consulta paginada**. 

> No contexto da nossa API, essa é uma consulta muito importante, pois ela não irá retornar todos os dados que temos cadastrados na base. 

## Testando a consulta paginada

Imagine que temos uma aplicação *mobile* ou web com restrições de acesso à rede tentando consumir nossa API. Por questões de performance, ter uma consulta paginada é importante. 

### Ajustando o código de `OfertaViagem_GET.cs`

Vamos começar acessando o arquivo `OfertaViagem_GET.cs` para melhorar um pouco o código. Primeiramente, adicionaremos ponto e vírgula ao final do `namespace` na linha 10, para melhorar a visualização. 

Feito isso, vamos remover também as importações desnecessárias, entre as linhas 1 e 8. Ao final, teremos o seguinte:

> *`OfertaViagem_GET.cs`:*

```cs
using JornadaMilhas.Dominio.Entidades;
using JornadaMilhas.Dominio.ValueObjects;
using System.Net.Http.Json;

namespace JornadaMilhas.Integration.Test.API;

// código omitido
```

### Criando o método `Recuperar_OfertasViagens_Na_Consulta_Paginada()`

O próximo método que vamos criar neste arquivo será um `public`, mas antes passaremos `[Fact]` na linha 45. Logo abaixo, digitamos `public async Task` seguido do nome do método `Recuperar_OfertasViagens_Na_Consulta_Paginada()`, pois queremos testar a recuperação de ofertas de viagens na consulta paginada. 

Por último, colaremos o código necessário no escopo do método. Ao final, teremos o seguinte:

> *`OfertaViagem_GET.cs`:*

```cs
// código omitido

[Fact]
public async Task Recuperar_OfertasViagens_Na_Consulta_Paginada()
{
    //Arrange
    using var client = await app.GetClientWithAccessTokenAsync();

    int pagina = 1;
    int tamanhoPorPagina = 80;

    //Act
    var response = await client.GetFromJsonAsync<ICollection<OfertaViagem>>($"/ofertas-viagem?pagina={pagina}&tamanhoPorPagina={tamanhoPorPagina}");

    //Assert
    Assert.True(response != null);
    Assert.Equal(tamanhoPorPagina, response.Count());
}
```

Com isso, já temos um método de teste criado. Inicialmente, há um ***arrange*** a partir da linha 48 onde vamos recuperar o `client`. Nele, também precisamos informar a `pagina` e o `tamanhoPorPagina`, nas linhas 51 e 52, respectivamente. Nesse caso, faremos um teste com 80 registros na primeira página. 

Na sequência, fazemos a consulta no ***act***, a partir da linha 54. Para isso, usamos `GetFromJsonAsync()`, e esperamos receber uma coleção de respostas de ofertas de viagem (`ICollection<OfertaViagem>`). Para recebê-la, precisamos passar a rota contendo a `pagina` e o `tamanhoPorPagina` via *string* (`{}`). 

No ***assert***, a partir da linha 57, vamos verificar se não foi retornada uma resposta nula (`response != null`) e também se o `tamanhoPorPagina` está de acordo com a coleção retornada. Portanto, passaremos 80 na consulta e encontraremos 80 registros na resposta também. 

### Realizando o teste

Após salvar o arquivo, vamos abrir o **gerenciador de testes** na lateral esquerda e expandir a classe `OfertaViagem_GET`. Feito isso, vamos clicar sobre `Recuperar_OfertasViagens_Na_Consulta_Paginada` com o botão direito e selecionar a opção "Executar". 

Nesse caso, o teste não passou, então vamos verificar o que aconteceu. A expectativa era de 80 registros na primeira página, mas foram retornados apenas 2 registros. 

De volta ao código de `OfertaViagem_GET.cs`, podemos fazer a seguinte análise: o teste espera que haja pelo menos 80 registros na base de dados, mas não temos certeza de que, neste momento, nessa opção de teste, temos 80 registros. Precisamos controlar melhor esse teste e torná-lo mais **independente**. 

No momento de executar esse teste e recuperar as ofertas de viagem na consulta paginada, devemos ter certeza de que temos pelo menos 80 registros cadastrados, pois nesse tipo de consulta, o acordo dos parâmetros passados irá funcionar. Porém, como podemos fazer isso? 

### Utilizando a biblioteca *Bogus*

Para isso, vamos utilizar um recurso que já conhecemos em outros cursos da formação: a biblioteca ***Bogus***. Inclusive, na atividade **Preparando o ambiente**, fazemos a configuração da instalação da biblioteca, construímos um projeto de teste, e também criamos a pasta chamada "DataBuilders". 

Antes de utilizar a biblioteca para gerar a massa de dados, vamos **cadastrar essa massa de dados no banco** para termos certeza. Dessa forma, após fazer a consulta paginada, o teste irá passar conforme esperado. Com isso, conseguiremos ter maior controle e deixaremos o teste mais independente. 

No arrange, parte do código de teste onde preparamos o cenário para executar o act, vamos adicionar algumas informações, pois como expusemos o contexto, temos acesso à base de dados. 

> *`OfertaViagem_GET.cs`:*

```cs
// código omitido

//Arrange
var ofertaDataBuilder = new OfertaViagemDataBuilder();
var listaDeOfertas = ofertaDataBuilder.Generate(80);
app.Context.OfertasViagem.AddRange(listaDeOfertas);
app.Context.SaveChanges();

using var client = await app.GetClientWithAccessTokenAsync();

int pagina = 1;
int tamanhoPorPagina = 80;

// código omitido
```

Primeiro declaramos uma variável chamada `ofertaDataBuilder`, que está na pasta "DataBuilders" do projeto de teste (`JornadaMilhas.Integration.Test.API`). Em seguida, criamos uma `listaDeOfertas` e geramos 80 ofertas utilizando a biblioteca Bogus. 

Depois, adicionamos a `listaDeOfertas` com 80 ofertas ao contexto, utilizando `app.Context.OfertasViagem.AddRamge()`, e logo na sequência salvamos com `app.Context.SaveChanges()`. 

Feito isso, criamos o `client`. Como queremos testar o `tamanhoPorPagina` de 80 registros, passamos esse valor para o `Generate()` na linha 51. Dessa forma, deixamos o teste mais isolado, pois sabemos que para executá-lo, teremos certeza de que existem 80 registros cadastrados na base. 

Agora podemos salvar o arquivo e executar o teste novamente. Com o gerenciador de testes aberto à esquerda, vamos clicar com o botão direito sobre `Recuperar_OfertasViagens_Na_Consulta_Paginada` e selecionar "Executar". Agora o teste passa como esperado. 

## Conclusão

Criamos mais um método de teste, agora para **testar a consulta paginada**, uma consulta importante para a nossa aplicação web. Porém, para ela funcionar e o teste passar com sucesso, precisamos ter certeza de que a massa de dados está cadastrada no banco. Caso contrário, não conseguiremos paginar. 

Como expusemos o nosso contexto, temos acesso à base de dados. A partir disso, alteramos essa base de dados adicionando **80 registros**. Como o teste espera 80 registros por página, criamos essa massa de dados via Bogus no método `Generate(80)`, para então fazermos a consulta paginada. 

Na sequência, continuaremos testando a **consulta paginada**, que ainda possui outros detalhes importantes a serem validados!