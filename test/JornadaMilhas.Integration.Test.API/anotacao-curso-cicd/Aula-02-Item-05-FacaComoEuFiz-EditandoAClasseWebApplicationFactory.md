Nesta aula, o trabalho concentrou-se na configuração da biblioteca TestContainer para resolver a dependência do Docker na execução dos testes para nossa pipeline no GitHub Actions. Após a instalação da biblioteca no projeto de testes de integração, precisamos refatorar a classe ** `JornadaMilhasWebApplicationFactory`** para utilização da biblioteca. Para esta prática, devemos seguir os seguintes passos:

* Criar uma instância que irá representar o container;
* Configurar a nova string de conexão apontando para a instância do container criada.

Então, vamos à prática!

> Se tiver alguma dúvida, basta clicar em **Opinião do instrutor**.


## Opinião do Instrutor

Vamos colocar a mão no código nesta prática recomendada e continuamos editando a classe `JornadaMilhasWebApplicationFactory`;

 Agora vamos cuidar do ciclo de vida da criação do container e para isso vamos usar um recurso do xUnit que é a interface `IAsyncLifetime`:

```
public class JornadaMilhasWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
```

Com esta interface precisamos  implementar os métodos **`InitializeAsync`** e  **`DisposeAsync**`:
 
```
 public async Task InitializeAsync()
 {
     await _mssqlContainer.StartAsync();
 }

 async Task IAsyncLifetime.DisposeAsync()
 {
     await _mssqlContainer.DisposeAsync();
 }
```

Neste ponto vamos mover  o código que está no construtor para o método  **`InitializeAsync`**  e remover o construtor: 

```
 public async Task InitializeAsync() {

     await _mssqlContainer.StartAsync();
     this.scope = Services.CreateScope();
     Context = scope.ServiceProvider.GetRequiredService<JornadaMilhasContext>();
 }

```

Vamos ainda ajustar a propriedade Context: `public JornadaMilhasContext Context { get; private set; }`.

Agora estamos configurando uma instância do Docker para rodar localmente e de forma mais independente. Caso ainda reste alguma dúvida, você pode conferir o código completo do que foi desenvolvido neste vídeo acessando [o repositório no GitHub](https://github.com/alura-cursos/JornadaMilhas-CICD/tree/aula02-video02.02).