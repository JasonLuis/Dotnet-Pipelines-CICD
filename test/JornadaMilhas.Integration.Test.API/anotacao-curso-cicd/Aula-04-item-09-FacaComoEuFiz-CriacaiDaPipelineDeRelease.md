Na atividade prática desta aula o objetivo é implementar na pipeline  o cenário de implantação no portal do Azure. Para isso, siga os passos:

* Definir um service connection para o portal do Azure;
* Crie uma pipeline de release.

Vamos à prática!

> Se tiver alguma dúvida, basta clicar em **Opinião do instrutor**.


Vamos iniciar acessando  as **`configurações do projeto`** no menu lateral a esquerda no ícone da engrenagem, como mostrado a seguir:

![alt text: A imagem mostra o recorte da página do Azure Devops no menu lateral a esquerda, com uma seta vermelha indicando o ícone de engrenagem para a função “Projet settings”.](http://cdn3.gnarususercontent.com.br/3661-testes-dot-net-4/imagem10.png)

Dentro de Pipeline vamos definir uma nova **`Service Connection`**, que vai me permitir criar uma conexão com o portal Azure.

Seleciona a opção **`Azure Classic`**, e clique no botão **`Next`**, será aberta uma nova aba para inserção das informações de conexão com o portal do Azure:

* Authentication Method, utilizamos na aula a opção `credentials`.
* Subscription Id, essa informação está na sua conta do portal do Azure.
* Subscription Name, o nome da subscription encontrada na sua conta do portal do Azure.
* Username, seu usuário do portal do Azure.
* Password, sua senha do portal do Azure.
* Service Connection name, o nome que você dará ao serviços de conexão.

Depois de preencher os campos clique em **`Next`**.

Agora com o serviço pronto, você deve navegar no menu lateral esquerdo e clicar na opção **Pipeline** e depois em **Release**, ao abrir a página no lado direito clicar em **create new Pipeline Release**.

A próxima etapa é escolher um template a ser utilizado, na listagem mais a direita escolha a opção **`Azure App Service deployment`** 

Na sequência clique em `1 job,1 task` para configurar a tarefa que faz o deploy no portal do Azure. Você deverá informar:

* Stage name, o nome do estágio.
* Azure Subscription, a sua subscription do Azure oriunda da service connection configurada.
* App type, o tipo de aplicação windows ou linux.
* App Service name, o nome da web app definida no portal do Azure.

Seguimos clicando em **Pipeline** onde teremos que definir o artefato, ou seja, informar que o artefato gerado na Build Pipeline é o recurso a ser implantado no Azure.

Clique em **Add an artifact**, vamos configurar as informações da origem, projeto  e a Build Pipeline.

Após a execução dessas configurações criamos nossa pipeline de release, que se encarrega de implantar nossa aplicação no portal do Azure. Continue focado nos estudos e boa prática!