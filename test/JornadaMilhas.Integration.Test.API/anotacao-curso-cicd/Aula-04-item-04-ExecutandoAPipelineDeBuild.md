Acabamos de definir o nosso arquivo `azure-pipelines.yml`, que será utilizado para criar a nossa pipeline na plataforma do Azure DevOps. 

Para recapitular: preparamos o ambiente utilizando o Ubuntu, definimos a branch `main` como gatilho e estabelecemos alguns passos de teste, as *tasks*. Entre elas, uma tarefa que realiza o *build* do projeto da API, uma tarefa que executa os testes de unidade e de integração, e também um passo de publicação e geração do artefato para implantarmos mais à frente no Azure. 

O próximo passo é **executar a nossa pipeline**. Para isso, precisamos acessar o portal do Azure para fazer as configurações necessárias: http://dev.azure.com. 

> Para realizar os próximos passos, você precisa criar a sua organização e a sua conta. Vamos deixar uma atividade de preparação do ambiente para ajudar nessas etapas iniciais.

## Criando um projeto no Azure DevOps

Vamos partir do formulário de **criação do projeto**, já dentro do Azure DevOps. Vamos chamá-lo de `JornadaMilhas-API`. Podemos adicionar uma descrição para o nosso projeto (opcional) e definir a sua visibilidade. Neste caso, vamos definir como privada, selecionando a opção "Private". Clicamos em "Create Project" e o projeto é criado!

## Conexão com o repositório no GitHub

A próxima etapa é uma configuração importante: precisamos conectar esse projeto com o GitHub para poder fazer o *checkout* no momento de execução da pipeline. Ou seja, precisamos configurar no Azure DevOps um serviço de conexão (*Service Connection*). 

Para isso, clicamos em "***Project Settings***" no final do menu lateral esquerdo da página do projeto, e depois em "***Service Connection***" no menu de configurações. Criamos uma *Service Connection* clicando no botão "***Create service connection***" no centro da tela.

Na janela modal de criação do serviço, procuramos pelo serviço do **GitHub**, selecionamos e clicamos em "**Next**" para ir para o formulário de criação desse serviço.

No campo "***Authentication method***", escolhemos a opção "*Grant authorization*" e no campo "***OAuth Configuration***" selecionamos "*Azurepipelines*". 

Clicamos no botão "**Authorize**" para configurar a nossa conta do GitHub, que será acessada automaticamente. 

Com isso, é criado um nome padrão para o serviço de conexão considerando o seu nome de usuário no GitHub. Podemos deixar esse nome e clicar em "**Save**".

Já criamos a conexão do projeto com o nosso repositório! Agora vamos partir para a pipeline. 

##

Vamos clicar no ícone de foguete azul (ou botão "Pipelines") no menu de serviços à esquerda da tela. Depois clicamos em "***pipelines > Create pipeline***". 

Na criação da pipeline, na aba "***Connect***", selecionamos o **GitHub** como hóspede do nosso código. 

Em seguida, na aba "***Select***", selecionamos o **repositório** do GitHub em que estamos trabalhando - como no caso do instrutor, que está usando um repositório chamado `JornadaMilhas-CICD` para este projeto. 

Na etapa de configuração (aba "***Configure***"), podemos definir um *templates* para criar o arquivo YML - para Docker, Kubernetes, etc. 

Como configuramos essa pipeline trabalhando com um repositório que já tem esse arquivo YML, isso é reconhecido diretamente. Por isso, a etapa é pulada automaticamente. 

> Vamos deixar uma atividade de preparação do ambiente sobr essa etapa em específico, porque você pode selecionar um arquivo YML já existente ou criar um arquivo do zero neste momento. 

A plataforma reconheceu a nossa pipeline no arquivo YML que já estava no repositório, `azure-pipelines.yml`, conforme podemos confirmar na aba "***Review***", de revisão. 

Agora, vamos **executar** essa pipeline clicando no botão "**Run**" no canto superior direito. 

Finalizado o processo de criação da pipeline, a página será redirecionada para a pipeline criada. Será agendado um "***Job***" nesse momento. Clicamos nele, na parte inferior da página, e aguardamos a execução da nossa pipeline.

Terminada a execução desse *Job*, o que leva cerca de 3 minutos, conseguimos **visualizar a execução de cada etapa** da pipeline e o tempo levado para cada uma, assim como no GitHub Actions. 

O *build*, por exemplo, levou 37 segundos para ser executado na máquina do instrutor. Já o teste de unidade levou 27 segundos. Foi bem rápido!

A publicação também foi feita, utilizando o **.NET Publish** da API. Também foi gerado um **artefato** que será utilizado adiante para fazermos a implantação no portal do Azure. 

## Relatório de execução dos testes

No Azure DevOps, ao final da execução dos testes, é gerado para nós um **relatório** dos testes que foram executados, que podemos acessar na linha "***Published Test Run***" ao final dos logs de execução. 

Clicando no link disponibilizado, vamos para a página desse relatório de execução. Ele exibe quantos testes foram executados, quais passaram, e um gráfico de *outcomes* dos testes. Isso é bem interessante e útil para trabalharmos em uma **apresentação**, por exemplo. 

Isso vale tanto para o teste de unidade quanto para o teste de integração - o relatório é gerado para ambos. 

## Próximos passos...

Aa primeira etapa foi concluída: *buildamos* o projeto, executamos os testes e fizemos a publicação. A próxima etapa para complementar essa pipeline é a **publicação no portal do Azure**. Faremos isso na sequência!