Acabamos de configurar a nossa `JornadaMilhasWebApplicationFactory`! Já definimos o contêiner e estamos apontando para ele na extensão de conexão. Podemos testar, executando os testes. 

No Gerenciador de Testes, à esquerda da tela, vamos clicar com o botão direito em `JornadaMilhas.Integration.Test.API` e selecionar a opção "**Executar**" para rodar todos os testes. 

Mas o teste falhou! Expandindo os testes para entender melhor, podemos concluir que os erros ocorreram porque ele não conseguiu iniciar o `MsSqlContainer`.

Se pensarmos no ambiente local que configuramos para a execução do Docker, precisamos ter certeza de que o Docker está funcionando para a execução dos testes. 

O mesmo se aplica nesse caso: instalamos o Docker no ambiente, configuramos a extensão de conexão, mas em que momento falamos que o **contêiner foi inicializado**? 

Vamos fazer isso agora, também por meio de código. 

## Inicializando o contêiner

Vamos implementar uma interface na nossa classe `JornadaMilhasWebApplicationFactory` para gerenciar o ciclo de vida de objetos no *XUnit*. 

Então, logo depois de `<Program>`, vamos adicionar uma vírgula e o **`IAsyncLifetime`**:

> `JornadaMilhasWebApplicationFactory.cs`

```cs
public class JornadaMilhasWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
// código omitido
}
```

Essa biblioteca define que precisamos implementar a **interface**. Então, clicamos nela com o botão direito e selecionar a opção "Implementar a interface".

Nessa interface, precisamos implementar dois métodos: `Initialize()` e `Dispose()`. No `Initialize`, vamos definir o seguinte: `await` nosso contêiner (`mssqlContainer`) e `StartAsync()`. Então, o método `Initialize` deve ser assíncrono também, então o definimos como `async`. Teremos:

```cs
public async Task InitializeAsync()
{
		await _mssqlContainer.StartAsync();
}
```

No `Dispose()`, vamos inserir o mesmo código e apenas alterar `StartAsync` para `DisposeAsync`. O método também será `async`:

```cs
async Task IAsyncLifetime.DisposeAsync()
{
		await _mssqlContainer.DisposeAsync();
}
```

Pronto. Fizemos uma definição que vai iniciar o contêiner e, no final, removê-lo no `Dispose`. 

Mas ainda não acabamos! No momento que iniciarmos o contêiner, precisamos aplicar aquela abordagem de **expor o seu contexto**. 

Vamos até o nosso construtor de `JornadaMilhasWebApplicationFactory`, recortamos seu conteúdo e o apagamos.

> Trecho recortado de `JornadaMilhasWebApplicationFactory.cs`

```cs
public JornadaMilhasWebApplicationFactory()
{
		this.scope = Services.CreateScope();
		Context = scope.ServiceProvider.GetRequiredService<JornadaMilhasContext>();
}
```

Depois, no nosso `InitializeAsync`, logo depois de iniciarmos o contêiner, vamos recuperar o escopo do contêiner de gestão de dependência, colando o conteúdo que recortamos do construtor. 

```cs
public async Task InitializeAsync()
{
		await _mssqlContainer.StartAsync();
		this.scope = Services.CreateScope();
		Context = scope.ServiceProvider.GetRequiredService<JornadaMilhasContext>();

}
```

Temos um erro em `Context` porque não podemos atribuir essa propriedade, é somente leitura. Para resolver isso, temos que criar também a **definição do nosso contexto**. Nela, além de `get`, vamos adicionar um `set` - que pode ser privado, então será um `private set`. 

```cs
public JornadaMilhasContext Context { get; private set; }
```

Pronto! Resolvemos o nosso erro no `Initialize`. Salvamos o arquivo. 

Vamos executar o nosso teste novamente. No gerenciador de testes, clicamos com o botão direito no projeto de teste de integração e selecionamos a opção "**Executar**". 

Os testes passam agora, pois estão usando o contêiner através do `TestContainer`. E se repararmos, a execução de todos os testes ficou bem mais rápida!

## Recapitulando

Fizemos a configuração do `TestContainer` no nosso teste de integração. Fizemos as configurações necessárias e refatorações para iniciar o contêiner e encerrá-lo depois da execução dos testes. 

Então, o próximo passo para definir nossa pipeline de teste de integração no *GitHub Actions* é levar o código que executa o teste de integração para lá. Faremos isso na sequência.