No desenvolvimento desta aula definimos a criação de nossa pipeline no Azure DevOps, mas precisamos configurar o acesso ao serviço externo do repositório de códigos no GitHub. Sendo assim, nesta prática você é desafiado a acessar o Azure DevOps e proceder com essa configuração.

Vamos à prática então!

> Se tiver alguma dúvida, basta clicar em **Opinião do instrutor**.


Vamos iniciar acessando  as **`configurações do projeto`** no menu lateral a esquerda no ícone da engrenagem, como mostrado a seguir:

![alt text: A imagem mostra o recorte da página do Azure Devops no menu lateral a esquerda, com uma seta vermelha indicando o ícone de engrenagem para a função “Projet settings”.](http://cdn3.gnarususercontent.com.br/3661-testes-dot-net-4/imagem10.png)

Dentro de Pipeline vamos definir uma nova **`Service Connection`**, que vai me permitir criar uma conexão com o GitHub e mais a frente com o portal Azure.

Você irá clicar em `new service connection`,em seguida filtrar por github e iniciar a configuração informando: **`Grant Authorization`**; No campo **`OAuth Configuration`** selecionar **`AzurePipelines`** e clicar em **`Authorize`**.

Na sequência você irá informar um nome para sua **`service connection`**, ainda tem a opção de informar uma descrição e irá clicar em **`Save`**.

Pronto, entregamos mais uma configuração prática, continue focado e bons estudos!