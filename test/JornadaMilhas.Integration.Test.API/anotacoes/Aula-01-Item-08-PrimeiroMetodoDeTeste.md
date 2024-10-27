Acabamos de configurar a API de memória utilizando a classe `WebApplicationFactory`. Assim, temos maior controle de como queremos a execução dessa nossa API para os nossos testes. Já estamos com a biblioteca de teste configurada na nossa solução também, solução de teste, programa de teste. 

**Como você costuma abordar a realização de um teste de API?** Geralmente, é necessário familiarizar-se com o *endpoint* que será testado, entender os dados de entrada esperados para esse *endpoint*, e também compreender a resposta que será obtida, ou seja, a saída, para poder realizar as verificações necessárias, validar os resultados e prosseguir com a execução da API.

Com essa classe que acabamos de criar, vamos conseguir automatizar isso aqui no Visual Studio, num projeto de teste, de maneira tal que consigamos sempre executar o nosso projeto de maneira automatizada e nossos testes com essa configuração que precisamos. 

**Vamos fazer agora o nosso primeiro método de teste.**

## Criando o método de teste

No gerenciador de soluções do lado direito, deixamos propositalmente a classe `InitTest1.cs`, não a removemos, vamos **renomeá-la**. Para renomear, clicamos com o botão direito (atalho "F2"), e vamos renomeá-la para `JornadaMilhas_AuthTest`. 

O primeiro teste que criaremos consiste em testar o *login* em nossa API. Nossa API opera com várias camadas de segurança que exigem um processo de login. Após renomear a classe e confirmar as alterações, passamos agora para a classe de teste, onde já fizemos a renomeação. Portanto, agora criaremos nosso primeiro método de teste.

> [JornadaMilhas_AuthTest](https://github.com/bessax/JornadaMilhas-API-rec/blob/aula01-video1.4/test/JornadaMilhas.Integration.Test.API/JornadaMilhas_AuthTest.cs)

```cs
namespace JornadaMilhas.Integration.Test.API
{
    public class JornadaMilhas_AuthTest
    {
        [Fact]
        public void Test1()
        {

        }
    }
}
```

A classe pública, `JornadaMilhas_AuthTest`, nosso primeiro método de teste. Vamos testar um *post*. Renomeamos esse método para `POST_Efetua_Login_Com_Sucesso()`, que será um método assíncrono, então, `async Task`. 

```cs
namespace JornadaMilhas.Integration.Test.API
{
    public class JornadaMilhas_AuthTest
    {
        [Fact]
        public async Task POST_Efetua_Login_Com_Sucesso()
        {

        }
    }
}
```

Neste contexto, revisitando essas fases, elaboramos um código de teste no qual vamos estabelecer o nosso *Arrange*, o nosso *Act* e o nosso *Assert*. Assim, estamos seguindo o padrão AAA.

No *Arrange*, onde **preparamos o ambiente para realizar a ação de teste**, necessitamos dos seguintes componentes: um cliente para se conectar à API, os valores a serem transmitidos, bem como o usuário e senha para efetuar o login na API e realizar o teste.

Para começarmos, vamos fazer o seguinte: precisamos ter acesso a essa instância rodando em memória que já está configurada do jeito que precisamos. Então, vamos criar no Arrange um `var` chamado de `app` (*application*), damos um `new`, `jornadaMilhasWebApplicationFactory`. 

```cs
namespace JornadaMilhas.Integration.Test.API
{
    public class JornadaMilhas_AuthTest
    {
        [Fact]
        public async Task POST_Efetua_Login_Com_Sucesso()
        {

        }
    }
}
```

Também será imprescindível possuirmos um cliente para estabelecer a conexão, um objeto `client`, e precisaremos igualmente de um objeto chamado `userDTO` para realizar o login na API. 

```cs
namespace JornadaMilhas.Integration.Test.API
{
    public class JornadaMilhas_AuthTest
    {
        [Fact]
        public async Task POST_Efetua_Login_Com_Sucesso()
        {
				
				//Arrange
				var app = new JornadaMilhasWebApplicationFactory();

				var user = new UserDTO { Email = "tester@email.com", Password = "Senha123@" };
				using var client = app.CreateClient();
				
				//Act
				//Assert

        }
    }
}
```

Neste cenário, definimos uma variável user do tipo `userDTO`, utilizando um valor padrão, que é o endereço de e-mail do teste "tester@email.com", juntamente com a senha.

Através do objeto `app`, que é a `jornadaMilhasWebApplicationFactory`, conseguimos recuperar um cliente utilizando esse `client`.

Para isso, criamos uma variável chamada `resultado` em *Act*. Em seguida, executamos um `await`, que é um método assíncrono. Utilizando o `client` que acabamos de definir, chamamos o método `postAsJsonAsync` no *endpoint* da rota `auth-login`, passando o usuário que acabamos de criar como parâmetro. Portanto, esta é a nossa *Action*.

```cs
namespace JornadaMilhas.Integration.Test.API
{
    public class JornadaMilhas_AuthTest
    {
        [Fact]
        public async Task POST_Efetua_Login_Com_Sucesso()
        {
				
				//Arrange
				var app = new JornadaMilhasWebApplicationFactory();

				var user = new UserDTO { Email = "tester@email.com", Password = "Senha123@" };
				using var client = app.CreateClient();
				
				//Act
				var resultado = await client.PostAsJsonAsync("/auth-login", user);
				
				//Assert

        }
    }
}
```

Em seguida, o nosso *Assert* realiza o seguinte: `Assert.Equal()` para verificar se o login foi realizado com sucesso, o que seria indicado por um *status code 200 ok*. É isso que estamos testando aqui agora. Então, no `Equals`, esperamos um *status code* "ok", que podemos encontrar na resposta através de `resultado.StatusCode`.

```cs
namespace JornadaMilhas.Integration.Test.API
{
    public class JornadaMilhas_AuthTest
    {
        [Fact]
        public async Task POST_Efetua_Login_Com_Sucesso()
        {
				
				//Arrange
				var app = new JornadaMilhasWebApplicationFactory();

				var user = new UserDTO { Email = "tester@email.com", Password = "Senha123@" };
				using var client = app.CreateClient();
				
				//Act
				var resultado = await client.PostAsJsonAsync("/auth-login", user);
				
				//Assert
				Assert.Equal(HttpStatusCode.OK,resultado.StatusCode);
        }
    }
}
```

**Temos nosso primeiro método de teste. **

Agora vamos testar. 

## Testando a aplicação

Então, vamos lá: clicamos em "Exibir" na parte superior esquerda, depois em "gerenciador de testes". Expandimos um pouco a janela do lado esquerdo para visualizar o teste. Temos o teste `jornadaMilhas.Integration.Test.API` que vamos abrir agora. Em seguida, clicamos com o botão direito em `jornadaMilhas_AuthTest` e selecionamos a opção executar (Ctrl + R).

Nosso teste passou e ficou verde, o que era nosso objetivo. Escrevemos nosso primeiro método de teste e revisamos o conceito do AAA (*Arrange, Act e Assert*), utilizando a biblioteca `mvc.testing`. Essa biblioteca nos permite criar uma instância de nossa API em memória e configurá-la conforme necessário.

Na configuração que realizamos, ajustamos a conexão com nosso banco de dados, que está em um contexto diferente rodando no Docker.

## Próximos Passos

Porém, ainda não terminamos. A seguir, vamos escrever mais métodos de teste e continuaremos nessa sequência.