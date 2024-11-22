Precisamos agora completar a *pipeline*. Devemos ter certeza de que a *pipeline* está rodando, executando os testes e, posteriormente, realizando a publicação.

Nosso objetivo é **publicar a API**, tornando-a disponível em um **ambiente externo** - seja um ambiente de produção ou uma *cloud*. Temos disponíveis o Azure, AWS, Google Cloud e uma série de outros.

Optaremos, neste momento do curso, por utilizar o **Azure**. Por isso, precisaremos realizar uma série de configurações que começaremos a fazer agora.

O objetivo final que queremos alcançar na nossa *pipeline* é realizar a publicação. Portanto, no final, precisamos conseguir chamar a nossa API na URL publicada. Queremos abrir o Swagger no navegador.

Vamos seguir agora alguns passos para conseguirmos fazer essa entrega no ambiente do portal do Azure.

Podemos parar de rodar a aplicação e abrir o portal do Azure no navegador.

> É importante que você confira a atividade de "Prepare o ambiente" para aprender a **criar sua conta no Azure** e poder executar os passos daqui em diante.

## Criando grupo de recursos

Na página inicial do portal do Azure, começaremos pela criação de um grupo de recursos. Para isso, vamos passar o mouse em cima de "*Resource groups*" e clicar na opção "*Create*".

> O grupo de recursos serve para organizar esses recursos que vamos criar dentro do Azure.

No formulário de detalhes do projeto, vamos definir o nome do grupo de recursos como `rg-jornadamilhas`. Dentro desse grupo, vamos definir dois recursos que precisamos colocar nesse ambiente de *cloud*: o servidor de banco de dados SQL Server e também a aplicação web, que é a nossa API.

No campo de *region*, vamos deixar a região como `(US) East US`.

Em seguida, clicaremos no botão "*Review + Create*" no canto inferior esquerdo. Depois de revisar os dados, podemos clicar no botão "*Create*" também na parte inferior da página.

Na página inicial, podemos clicar novamente em "*Resource groups*" para conferir se o recurso `rg-jornadamilhas` foi criado.

## Criando recurso de banco de dados

Voltando para a página inicial, o próximo passo é criar um novo recurso. Vamos começar pelo **banco de dados**. Após clicar em "*Create a Resource*", vamos selecionar "*Create*" na terceira opção de "SQL Database".

No formulário de detalhes do projeto, vamos selecionar o grupo de recursos onde a base de dados será criado. Para isso, no campo "*resource group*", escolhemos `rg-jornadamilhas`. 

Se não tivéssemos um grupo, também poderíamos criá-lo nesse momento, clicando em "*Create new*".

Na seção de "*Database Details*" (detalhes do banco de dados), vamos definir os detalhes do banco de dados. No campo *database name*, vamos definir o mesmo nome do banco que está configurado na nossa aplicação.

No Visual Studio, podemos abrir o arquivo `appsettings.json` para utilizar a *string* de conexão com o banco, que já possui o nome do banco. Basta copiar `JornadaMilhasV3` e colar no respectivo campo no portal do Azure.

### Definindo servidor

Outro ponto importante é a definição do servidor de banco de dados. Não temos nenhum ainda, portanto, vamos criá-lo. Basta clicar no link "*Create New*" logo abaixo do campo "*Server*".

Feito isso, somos redirecionados para outra página, omde vamos definir o nome do servidor. Nesse caso, chamaremos de `jornadamilhasserver`. Deixaremos a *location* como padrão, que é a região `(US) EAST US`.

Além disso, devemos definir também a autenticação do nosso servidor. Em *authentication method*, marcamos "*Use SQL Authentication*".

Em seguida, precisamos definir um usuário, que não pode ser denominado "admin". Por isso, no campo *server admin login*, digitamos apenas `andre`.

Também vamos copiar a senha da *string* de conexão. Voltando no `appsetting.json` no Visual Studio, copiamos a senha `Alura#2024`. Em seguida, a colamos tanto no campo "*Password*" (Senha) quanto "*Confirm Password*" (Confirmação de senha) no portal do Azure. 

Por fim, podemos clicar no botão "OK" no canto inferior esquerdo para finalizar a criação o servidor.

### Ambiente de trabalho

Somos redirecionados novamente para a página da criação do banco de dados.

O próximo passo é definir o *Workload Environment* (Ambiente de trabalho), escolhendo entre duas opções: *Development* (Desenvolvimento) ou *Production* (Produção).

Estamos utilizando uma conta gratuita que o Azure disponibiliza para a primeira criação de conta no portal, por isso, temos um número de crédito limitado. Cada um dos recursos criados no Azure será cobrado desses créditos.

Por isso, vamos escolher o ambiente de desenvolvimento que possui uma taxa menor para economizar recursos e créditos no Azure. Mais abaixo, vamos deixar marcado também "*Locally-redundant backup storage*", que é a redundância Local.

Para finalizar, vamos clicar em "*Review + Create*" e novamente em "*Create*".

## Substituindo *string* de conexão

Essa etapa de criação do banco de dados e do servidor dentro do portal do Azure pode levar um pouco de tempo. Após a implementação, podemos clicar no botão "*Go to Resource*" para conferir o recurso.

Na tela do banco de dados `JornadaMilhasV3`, podemos encontrar algumas configurações importantes também que precisamos levar para o nosso código.

Primeiro, vamos clicar em "*Connection String*" na lateral esquerda. Agora a *string* de conexão precisa apontar para o nosso banco de dados dentro do Azure. 

Na aba "ADO.NET", vamos copiar a *string* do campo "ADO.NET (SQL Authentication)" que é a forma de autenticação que definimos na criação do banco. Feito isso, vamos voltar ao `appsettings.json` no nosso projeto.

Em `ConnectionString`, vamos substituir o valor da *string* de `DefaultConnection`.

É interessante você utilizar a melhor estratégia no seu caso. Você pode criar outra variável no `ConnectionString` ou salvar uma *secret* em outro tipo de arquivo. Mas, por efeito didático, vamos apenas substituir.

Nessa *string* de conexão que copiamos, devemos substituir o trecho `{your_password}` pela senha definida. No nosso caso, é `Alura#2024`.

> `appsettings.json`:

```json
"AllowedHosts": "*",
"ConnectionString": {
	"DefaultConnection": "Server=tcp:jornadamilhasserver.database.windows.net,1433;Initial Catalog=JornadaMilhasV3;Persist Security Info=False;User ID=andre;Password=Alura#2024;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
},
```

## Configurando rede e segurança

Há outra configuração importante que precisamos fazer no portal do Azure, ainda referente ao banco de dados. Retornando a página inicial, na seção de recursos recentes, vamos clicar em *resource group* chamado `rg-jornadamilhas`.

Dentro do `rg-jornadamilhas`, encontramos o servidor e o banco. Vamos no servidor `jornadamilhaserver` e, na lateral esquerda, clicar em "*Networking*".

Em *public network access*, vamos marcar a opção *selected networks*. Feito isso, logo abaixo, vamos habilitar o IP da nossa máquina no *firewall*, caso precisemos executar o projeto localmente.

Em "*Exceptions*", vamos permitir que os serviços e recursos do Azure acessem o servidor, marcando a opção "*Allow Azure services and resources to access this server*". O ideal é que você também tenha uma estratégia para manter a segurança dentro do portal do Azure.

Feito isso, podemos clicar no botão "*Save*" na parte inferior da página.

## Próximos passos

Agora o recurso referente ao banco de dados está configurado. Precisamos agora configurar a nossa aplicação web também dentro do portal do Azure.