Acabamos de criar nossa pipeline de CI/CD no *GitHub Actions*. Agora, temos um desafio: vamos levar a construção dessa pipeline para outra plataforma, o ***Azure DevOps***. 

Por que utilizar essa outra ferramenta? Porque ela faz parte do ecossistema da Microsoft, então tem uma boa **integração** com outras ferramentas, como o *GitHub* e outros produtos da empresa. 

Para criar essa pipeline no *Azure DevOps*, também vamos trabalhar com um arquivo `.yml`. Vamos criar um novo arquivo desse tipo para criar essa pipeline na outra plataforma. 

## Criando o arquivo YML

Vamos começar criando nosso arquivo na raiz da solução. Então, clicamos com o botão direito sobre a solução "**JornadaMilhasAPI**" no gerenciador de soluções à esquerda da tela, depois selecionamos a opção "**Adicionar > Novo Item**". Vamos chamar esse arquivo de `azure-pipelines.yml`. Clicamos em "**Adicionar**" para confirmar. 

Vamos colar nesse novo arquivo o seguinte código:

> `azure-pipelines.yml`

```yml
trigger:
  branches: 
   include:
     - main
pool:
  vmImage: ubuntu-latest

steps:
- task: DotNetCoreCLI@2
  displayName: 'Build do projeto'
  inputs:
    projects: '**/src/JornadaMilhas.API/JornadaMilhas.API.csproj'
    arguments: '--configuration $(BuildConfiguration)'

- task: DotNetCoreCLI@2
  displayName: 'Testes de Unidade'
  inputs:
    command: test
    projects: '**/test/JornadaMilhas.Unit.Test/JornadaMilhas.Unit.Test.csproj'
    arguments: '--configuration $(BuildConfiguration)'

- task: DotNetCoreCLI@2
  displayName: 'Testes de Integração'
  inputs:
    command: test
    projects: '**/test/JornadaMilhas.Integration.Test.API/JornadaMilhas.Integration.Test.API.csproj'

- task: DotNetCoreCLI@2
  displayName: Publish
  inputs:
    command: publish
    publishWebProjects: True
    arguments: '--configuration $(BuildConfiguration) --output $(build.artifactstagingdirectory)'
    zipAfterPublish: True

- task: PublishBuildArtifacts@1
  displayName: 'Publish Artifact'
  inputs:
    PathtoPublish: '$(build.artifactstagingdirectory)'
  condition: succeededOrFailed()
```

## Diferenças entre a pipeline para o GitHub Actions e o Azure DevOps

Vamos fazer uma comparação com o arquivo de pipeline que criamos para o *GitHub Actions*.

O arquivo `script.yml` que utilizamos no *GitHub Actions*, apesar de ser YML também tem uma estrutura um pouco diferente do que acabamos de criar. 

Por exemplo, no `script.yml` temos inicialmente o `name` para o **nome** da pipeline, depois a propriedade `on` com **onde** e **como** ela vai rodar os testes (no caso, na branch `main`, através do `push`). 

Já no `azure-pipelines`, definimos uma `trigger` para as `branches`, incluindo a branch `main`. 

Além disso, no *Azure DevOps* definimos o **ambiente** através de `pool` e da propriedade `vmImage`. Deixamos a mesma configuração para ambas as pipelines, o `ubuntu-latest`, que é definido na propriedade `runs-on` no *GitHub Actions*. 

Em ambos os arquivos temos a parte de `steps`, as **etapas** a serem executadas na nossa pipeline. No *GitHub Actions*, definimos o nome de cada passo (`name`) e depois os **usos** (`uses`) ou **execuções** (`runs`) deles.

Já no arquivo do *Azure DevOps*, os `steps` são definidos como `tasks` (**tarefas**). Uma `task` faz o `build` do projeto, outra executa nossos testes de unidade, outra os de integração, e outra cuida da publicação desse arquivo para ser utilizado em um momento posterior, para a implantação no *Azure* também. 

Fizemos uma refatoração no projeto para deixar nosso ambiente mais **independente**! 

Estamos trabalhando com o `TesteContainer`, que vai trabalhar a questão do *Docker* para nós. Então, estamos levando essa *Build* para a outra plataforma de maneira bem simples, sem nos preocupar mais com essa questão do ambiente *Docker*, porque já fizemos essa alteração e essa configuração. 

## Commit

Vamos salvar as alterações do `azure-pipelines.yml` e commitar esse arquivo para o nosso repositório. 

Para isso, clicamos em  "**Alterações do Git**" no menu esquerdo, escrevemos a mensagem `#Update: azure-pipelines.yml`. Depois selecionamos o modo de commit como "**Confirmar tudo e enviar por push**", salvar e confirmar as alterações, e pronto!

Já definimos nosso arquivo `.yml`. O próximo passo é executar essa pipeline. Vamos fazer isso na sequência.