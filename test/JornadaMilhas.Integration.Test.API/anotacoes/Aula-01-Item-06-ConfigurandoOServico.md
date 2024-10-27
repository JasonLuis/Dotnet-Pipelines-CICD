Acabamos de instalar em nosso projeto de teste a biblioteca `Microsoft.AspNetCore.Mvc.Testing`. É um nome bem extenso, então, a partir de agora, **vamos nos referir a ela apenas como `Testing`**. 

## Criando a classe `JornadaMilhasWebApplicationFactory`

Para utilizar a biblioteca, é necessário **criar uma nova classe**. Para isso, clicamos com o botão direito no projeto de `test` chamado de "JornadaMilhas.Integration.Test.API". 

No menu exibido, selecionamos "Adicionar >  classe". No campo "Nome" na parte inferior, digitamos "JornadaMilhasWebApplicationFactory". Logo após, clicamos no botão "Adicionar" no canto inferior direito.

> [JornadaMilhasWebApplicationFactory](https://github.com/bessax/JornadaMilhas-API-rec/blob/aula01-video1.3/test/JornadaMilhas.Integration.Test.API/JornadaMilhasWebApplicationFactory.cs) 

```cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JornadaMilhas.Integration.Test.API
{
    internal class JornadaMilhasWebApplicationFactory
    {

    }
}
```

Essa classe utilizará a biblioteca `Testing`, possibilitando a **criação de uma instância da nossa API em memória**. Além disso, oferecerá um maior controle sobre essa instância em execução, permitindo a realização de diversas configurações, as quais vamos iniciar agora.

Com a classe criada, precisamos começar a escrevê-la para utilizá-la em nossa biblioteca. Inserimos dois pontos após o `JornadaMilhasWebApplicationFactory` e digitamos `WebApplicationFactory`, para indicar que estamos herdando desta biblioteca. 

```cs
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JornadaMilhas.Integration.Test.API
{
    internal class JornadaMilhasWebApplicationFactory:WebApplicationFactory
    {

    }
}
```

## Configurações da classe `Program.cs`

No entanto, para utilizar a `WebApplicationFactory`, precisaremos alterar algumas **configurações do nosso servidor**, da nossa API. Na sequência, precisamos fazer uma referência ao `Program`. 

```cs
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JornadaMilhas.Integration.Test.API
{
    internal class JornadaMilhasWebApplicationFactory:WebApplicationFactory<Program>
    {

    }
}
```

Mas para que essa configuração seja efetiva, também precisamos fazer uma configuração na nossa classe `Program.cs`, que precisa ser visível agora no nosso projeto de teste para essa classe `WebApplicationFactory`.

Vamos à classe `Program.cs` da API do lado direito e adicionamos ao final da nossa classe a seguinte configuração: `public partial class Program { }`. 

> [Program.cs](https://github.com/bessax/JornadaMilhas-API-rec/blob/aula01-video1.3/src/JornadaMilhas.API/Program.cs) 

```
// código omitido

public partial class Program { }
```

Agora conseguimos ver as configurações da nossa API na nossa classe `WebApplicationFactory`.

Feita a configuração, voltamos à nossa classe `JornadaMilhasWebApplicationFactory`. Observe que "Program" está com um sublinhado na cor vermelha.

Com essa configuração feita na classe `Program.cs`, **precisamos colocar referência no nosso projeto de API**. Para isso, em "Dependências" do lado direito, clicamos com o botão direito e selecionamos "Adicionar referência de projeto". Selecionamos `JornadaMilhas.API` e clicamos em "OK".

Agora, observe que o `Program` na linha 10 está na cor verde e sem o sublinhado. 

Vamos também excluir os `using` não utilizados. Para isso, teclamos "Ctrl + ponto" e escolhemos "Remover Usos Desnecessários". Além disso, mudamos a visibilidade da nossa classe para `public`.

> JornadaMilhasWebApplicationFactory

```cs
using Microsoft.AspNetCore.Mvc.Testing;

namespace JornadaMilhas.Integration.Test.API
{
    public class JornadaMilhasWebApplicationFactory:WebApplicationFactory<Program>
    {

    }
}
```

Essa classe nos possibilita **criar uma instância da nossa API em execução na memória**, que podemos reconfigurar e usar para testes. Vamos ajustar algumas opções disponíveis na classe. Atualmente, nossa API está apontando para o *Container Docker* que hospeda nosso banco de dados SQL Server. 

**Modificaremos essa configuração**.

Vamos reconfigurar o seguinte: faremos `Override` no `ConfigureWebHost(builder)`, recebendo o `WebHostBuilder`. 

> JornadaMilhasWebApplicationFactory

```cs
using Microsoft.AspNetCore.Mvc.Testing;

namespace JornadaMilhas.Integration.Test.API
{
    public class JornadaMilhasWebApplicationFactory:WebApplicationFactory<Program>
    {
			protected override void ConfigureWebHost(IWebHostBuilder builder)
			{
				base.ConfigureWebHost(builder);
			}
    }
}
```

Faremos a configuração da nossa *string* de conexão, agora apontando para esse *Container* rodando em memória. Configuraremos utilizando o objeto `builder.ConfigureService()s`. Dentro disso, faremos a configuração.

```cs
using Microsoft.AspNetCore.Mvc.Testing;

namespace JornadaMilhas.Integration.Test.API
{
    public class JornadaMilhasWebApplicationFactory:WebApplicationFactory<Program>
    {
			protected override void ConfigureWebHost(IWebHostBuilder builder)
			{
				builder.ConfigureServices(services =>
				{
				
				});
				base.ConfigureWebHost(builder);
			}
    }
}
```

A modificação inicial que desejamos fazer diz respeito à **string de conexão**. A string atualmente configurada está direcionada ao servidor SQL Server, mas está referenciando um *Container* que observa outro *Container*. Agora, **precisamos acessar este *Container* a partir de uma instância em execução na memória**.

Então, o primeiro passo é o seguinte: `services.RemoveAll()`. Vamos eliminar a configuração que adiciona nosso DbContext.

```cs
using Microsoft.AspNetCore.Mvc.Testing;

namespace JornadaMilhas.Integration.Test.API
{
    public class JornadaMilhasWebApplicationFactory:WebApplicationFactory<Program>
    {
			protected override void ConfigureWebHost(IWebHostBuilder builder)
			{
				builder.ConfigureServices(services =>
				{
					services.RemoveAll(typeof(DbContextOptions<JornadaMilhasContext>));
					
				});
				base.ConfigureWebHost(builder);
			}
    }
}
```

Em seguida, vamos **reconfigurar** isso. Vamos **inserir a nova string de conexão com o *Container***. 

Para isso, temos um trecho de código pronto aqui para agilizar o processo. Assim, utilizamos `services.AddDbContext`, passando a referência do JornadaMilhas. No entanto, a string de conexão que utilizaremos, `Localhost.11433`, estará apontada para o Container.

```cs
using Microsoft.AspNetCore.Mvc.Testing;

namespace JornadaMilhas.Integration.Test.API
{
    public class JornadaMilhasWebApplicationFactory:WebApplicationFactory<Program>
    {
			protected override void ConfigureWebHost(IWebHostBuilder builder)
			{
				builder.ConfigureServices(services =>
				{
					  services.RemoveAll(typeof(DbContextOptions<JornadaMilhasContext>));
					  services.AddDbContext<JornadaMilhasContext>(options =>
           options
           .UseLazyLoadingProxies()
           .UseSqlServer
				 ("Server=localhost,11433;Database=JornadaMilhasV3;User Id=sa;Password=Alura#2024;Encrypt=false;TrustServerCertificate=true;MultipleActiveResultSets=true;"));
            });

				base.ConfigureWebHost(builder);
			}
    }
}
```

Estamos utilizando a nossa classe `WebApplicationFactory`, que nos permite reconfigurar essa instância da nossa API em memória. E qual a configuração que fizemos? Estamos agora apontando, utilizando uma string de conexão, que aponta para esse Container Docker em execução, o nosso SQL Server. 

A string muda um pouco porque precisamos fazer essa instância em memória, coletando um Docker. Anteriormente, tínhamos um Container Docker acessando esse outro Container Docker.

## Próximos Passos

Com essa primeira configuração, já conseguimos escrever o nosso primeiro **método de teste**, e vamos fazer isso na sequência.