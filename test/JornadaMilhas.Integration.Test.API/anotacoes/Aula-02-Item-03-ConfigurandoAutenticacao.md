Acabamos de definir o nosso método de teste `Cadastra_OfertaViagem()`.

Ele não está passando porque a expectativa é um 200 OK, mas retornou um status code de não autorizado.

O que acontece? Para consumir a nossa API, estamos utilizando o `client`,  que faz o `post` na linha 30. No entanto, a nossa API tem uma camada de segurança que utiliza o **token JWT**. Portanto, precisamos configurar esse `client` com esse token.

Nas aulas anteriores, criamos um método na classe `AuthTest`. Na classe `JornadaMilhas_AuthTest`, temos um método de teste que testa o login.

No entanto, não é adequado executar primeiro aquele método de teste, para depois executar esse método que tenta cadastrar uma oferta-viagem. Isso geraria uma dependência que não é adequada para o teste.

Quando olhamos para a nossa classe `WebApplicationFactory`, temos a possibilidade de configurar a nossa API.

Se olharmos para a nossa classe `JornadaMilhasWebApplicationFactory`, essa classe permite que configuremos a nossa API rodando em memória da maneira que queremos.

Portanto, ao invés recuperar o `client` dessa maneira `using var client =  app.CreateClient();`, o ideal seria que já trouxéssemos esse `client` configurado para ser utilizado para o nosso método de teste. Deixar essa classe `OfertaViagem_POST` independente da outra classe que faz o teste de autenticação.

Vamos começar a fazer isso agora.

Na nossa classe `JornadaMilhasWebApplicationFactory`, vamos definir um outro método que configura esse `client` para nós.

Na nossa classe `WebApplicationFactory`, farei o seguinte. Logo depois de configurar o `WebHost`, vou definir um outro método.

```
public async Task<HttpClient> GetClientWithAccessTokenAsync() {
    
    }
```

Esse método vai retornar para nós um `HttpClient`. E ele vai se chamar `GetClientWithAccessTokenAsync()`.

Precisamos recuperar o `client` da nossa API. Como fazemos isso? `var client`, como quero recuperar da própria API, vou colocar `this.createClient`. Já recuperei o `client`. E agora vamos configurá-lo. O código que precisamos agora é um código que já fizemos nessa classe `AuthTest`. Que é exatamente esse daqui. No momento que criamos um objeto do tipo `UserDTO`, para ser passado ali no login:

```
        	var user = new UserDTO { Email = "tester@email.com", Password = "Senha123@" };
```

Vamos copiar essa linha que está no arquivo `JornadaMilhas_AuthTest.cs` e colaremos na classe `JornadaMilhasWebApplicationFactory`.


```
public async Task<HttpClient> GetClientWithAccessTokenAsync()
{
   var client = this.CreateClient();
   var user = new UserDTO { Email = "tester@email.com", Password = "Senha123@" };
    }
```

No entanto, estou mantendo aqui essas informações no código, mais por questões didáticas. O ideal seria trabalhar com arquivos de configurações ou com *secrets*.

Tendo o objeto `UserDTO`, vamos fazer um `PostAsJsonAsync` lá em `/auth-login`. Passando como parâmetro o usuário, o `userDTO`.

Feito isso, agora o próximo passo, vou pegar o código com sucesso, o `statusCode`. Vamos fazer uma requisição. Passando o `UserTokenDTO`. Nosso objetivo é recuperar esse objeto aqui em `Result`. E a partir daí, vamos configurar o `Client`, colocando em `Authorization`, uma nova `AuthenticationHeaderValue("Bearer", result!.Token`), e passando o `Result.token`, que vai ser recuperado dentro desse objeto `result`. E vamos retornar o `client`. Nosso código vai ficar assim:

> `JornadaMilhasWebApplicationFactory.cs`

```
public async Task<HttpClient> GetClientWithAccessTokenAsync()
    	{
        	var client = this.CreateClient();
        	var user = new UserDTO { Email = "tester@email.com", Password = "Senha123@" };
        	var resultado = await client.PostAsJsonAsync("/auth-login", user);

        	resultado.EnsureSuccessStatusCode();

        	var result = await resultado.Content.ReadFromJsonAsync<UserTokenDTO>();

        	client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result!.Token);

        	return client;
    	}
```

Agora temos um novo método que configura o `client` para nós, já com autenticação, para podermos utilizar nossos métodos de teste.

Agora sim, com esse método criado, `GetClientWithAccessTokenAsync`, vamos voltar ao nosso método `OfertaViagem_POST`. E onde está `app.CreateClient()` vamos substituir por `await app.GetClientWithAccessTokenAsync()`.

```
using var client =  await app.GetClientWithAccessTokenAsync();
```

Agora estamos recuperando o `Client`, já configurado com autenticação. Vou salvar. E agora podemos executar novamente o teste.

Clique em "Exibir > Gerenciador de testes". Clique com o botão direito em "Cadastra_OfertaViagem" e selecione a opção "Executar".

Perfeito. o teste passou como esperado.

Agora resolvemos o nosso `Cadastra_OfertaViagem`. Para isso terminamos a configuração do `client`, que é o objeto que consome a nossa API.

Já configuramos esse `client` com o token de autenticação para a nossa API. Vamos continuar testando outras funções aqui do POST, continuaremos no próximo vídeo.